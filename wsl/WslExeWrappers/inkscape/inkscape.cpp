#include <iostream>
#include <Windows.h> 
#include <wslapi.h>
#include <Psapi.h>
#include <vector>
#include <string>
#include <ranges>
#include <memory>
#include <cstdio>
#include <stdexcept>
#include <stdlib.h>
#include <algorithm>
#include <filesystem>

static std::string Join(std::vector<std::string> strings, std::string delim) {
	auto joined_view = strings | std::views::join_with(delim);
	return std::ranges::to<std::string>(joined_view);
}

std::string HandleToString(HANDLE hPipeRead) {
    std::string result;

    constexpr DWORD BUFFER_SIZE = 4096;
    std::vector<char> buffer(BUFFER_SIZE);

    DWORD bytesRead = 0;
    BOOL success = FALSE;

    while (true) {
        success = ReadFile(
            hPipeRead,          // Handle to the pipe
            buffer.data(),      // Buffer to receive data
            BUFFER_SIZE,        // Number of bytes to read
            &bytesRead,         // Stores number of bytes actually read
            nullptr             // Not using overlapped I/O
        );

        if (!success || bytesRead == 0) {
			std::cerr << "Error reading from pipe: " << GetLastError() << std::endl;
            break;
        }
        result.append(buffer.data(), bytesRead);
    }

    return result;
}

std::string ExecuteAndCapture(const std::wstring& command) {
    // 1. Configure security attributes to allow handle inheritance
    SECURITY_ATTRIBUTES saAttr;
    saAttr.nLength = sizeof(SECURITY_ATTRIBUTES);
    saAttr.bInheritHandle = TRUE; // Child process must inherit this handle
    saAttr.lpSecurityDescriptor = NULL;

    // 2. Create an anonymous pipe for the child process's STDOUT
    HANDLE hChildStd_OUT_Rd = NULL;
    HANDLE hChildStd_OUT_Wr = NULL;
    if (!CreatePipe(&hChildStd_OUT_Rd, &hChildStd_OUT_Wr, &saAttr, 0)) {
        return "Error: CreatePipe failed.";
    }

    // 3. Ensure the read handle of the pipe is NOT inherited by the child
    if (!SetHandleInformation(hChildStd_OUT_Rd, HANDLE_FLAG_INHERIT, 0)) {
        CloseHandle(hChildStd_OUT_Rd);
        CloseHandle(hChildStd_OUT_Wr);
        return "Error: SetHandleInformation failed.";
    }

    // 4. Set up the structures for CreateProcess
    PROCESS_INFORMATION piProcInfo;
    STARTUPINFO siStartInfo;
    ZeroMemory(&piProcInfo, sizeof(PROCESS_INFORMATION));
    ZeroMemory(&siStartInfo, sizeof(STARTUPINFO));

    siStartInfo.cb = sizeof(STARTUPINFO);
    siStartInfo.hStdError = hChildStd_OUT_Wr;  // Redirect stderr to same pipe
    siStartInfo.hStdOutput = hChildStd_OUT_Wr; // Redirect stdout to pipe
    siStartInfo.dwFlags |= STARTF_USESTDHANDLES;

    // Duplicate command string because CreateProcessW can modify the input buffer
    std::vector<wchar_t> cmdBuffer(command.begin(), command.end());
    cmdBuffer.push_back(L'\0');

    // 5. Launch the child process
    BOOL bSuccess = CreateProcess(
        NULL,
        cmdBuffer.data(),     // Command line
        NULL,                 // Process security attributes 
        NULL,                 // Primary thread security attributes 
        TRUE,                 // INHERIT HANDLES must be TRUE
        CREATE_NO_WINDOW,     // Do not open a visible console window
        NULL,                 // Use parent's environment 
        NULL,                 // Use parent's current directory 
        &siStartInfo,         // STARTUPINFO pointer 
        &piProcInfo           // Receives PROCESS_INFORMATION 
    );

    if (!bSuccess) {
        CloseHandle(hChildStd_OUT_Rd);
        CloseHandle(hChildStd_OUT_Wr);
		auto lastError = GetLastError();
        return "Error: CreateProcess failed." + std::to_string(lastError);
    }

    // 6. CRITICAL: Close the write end on the parent side. 
    // If you don't do this, ReadFile will hang forever waiting for more data.
    CloseHandle(hChildStd_OUT_Wr);

    // 7. Read output from the child process's pipe
    DWORD dwRead;
    CHAR chBuf[4096];
    std::string output = "";

    while (ReadFile(hChildStd_OUT_Rd, chBuf, sizeof(chBuf) - 1, &dwRead, NULL) && dwRead > 0) {
        chBuf[dwRead] = '\0'; // Null-terminate chunks
        output.append(chBuf);
    }
    output.resize(output.size() - 1);
    // 8. Clean up handles
    WaitForSingleObject(piProcInfo.hProcess, INFINITE);
    CloseHandle(piProcInfo.hProcess);
    CloseHandle(piProcInfo.hThread);
    CloseHandle(hChildStd_OUT_Rd);

    return output;
}


static std::string ExecuteCommand(const std::wstring& command, bool redirectStdOut = true) {
    static HMODULE hLib = LoadLibrary(L"wslapi.dll");
	static auto pWslLaunch = reinterpret_cast<decltype(&WslLaunch)>(GetProcAddress(hLib, "WslLaunch"));
    

	SetEnvironmentVariable(L"DISPLAY", L"127.0.0.1:0.0");

	HANDLE hProcess;
	HANDLE outputHandle = NULL;
	HANDLE readOutputHandle = NULL;

	if (redirectStdOut) {
		outputHandle = GetStdHandle(STD_OUTPUT_HANDLE);
	}
	else {
		SECURITY_ATTRIBUTES saAttr;
		saAttr.nLength = sizeof(SECURITY_ATTRIBUTES);
		saAttr.bInheritHandle = TRUE;
		saAttr.lpSecurityDescriptor = NULL;

		if (!CreatePipe(&readOutputHandle, &outputHandle, &saAttr, 0)) {
			return "Error: CreatePipe failed.";
		}

		if (!SetHandleInformation(readOutputHandle, HANDLE_FLAG_INHERIT, 0)) {
			CloseHandle(readOutputHandle);
			CloseHandle(outputHandle);
			return "Error: SetHandleInformation failed.";
		}
	}
	HRESULT hr = pWslLaunch(L"archlinux", command.data(), FALSE, GetStdHandle(STD_INPUT_HANDLE), outputHandle, outputHandle, &hProcess);
	if (SUCCEEDED(hr)) {
		if (redirectStdOut) {
			return "";
		}
		else {
			CloseHandle(outputHandle);

			std::string result = HandleToString(readOutputHandle);
			WaitForSingleObject(hProcess, INFINITE);
			CloseHandle(hProcess);
			CloseHandle(readOutputHandle);
		}
	}

	return "";
}

static std::string WindowsToWslPath(const std::string& winPath) {
	const std::wstring command = L"wsl wslpath -a \"" + std::wstring(winPath.begin(), winPath.end()) + L"\"";
	std::cout << "Executing command: " << std::string(command.begin(), command.end()) << std::endl;
	return ExecuteAndCapture(command);
}

int main(int argc, char* argv[])
{
	std::vector<std::string> args(argv + 1, argv + argc);
	std::ranges::for_each(args, [](std::string& strArg) {
        if (strArg[0] != '-' && strArg[0] != '/' && strArg.length() > 1 && ((strArg[0] == 'C' && strArg[1] == ':') || strArg[strArg.size() - 4] == '.' || strArg.contains('\\'))) {
			strArg = WindowsToWslPath(strArg);
		}
	});

    const std::string argsStr = Join(args, " ");
    const std::wstring command = L"/bin/zsh -lc 'inkscape " + std::wstring(argsStr.begin(), argsStr.end()) + L"'";
    std::cout << "Executing command: " << std::string(command.begin(), command.end()) << std::endl;
    ExecuteCommand(command);
}