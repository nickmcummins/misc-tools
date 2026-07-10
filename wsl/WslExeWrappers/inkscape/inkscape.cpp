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
            break;
        }
        result.append(buffer.data(), bytesRead);
    }

    return result;
}

static std::string ExecuteCommand(const std::string& command, bool redirectStdOut = true) {
	static HMODULE hLib = LoadLibrary(L"wslapi.dll");
	static auto pWslLaunch = reinterpret_cast<decltype(&WslLaunch)>(GetProcAddress(hLib, "WslLaunch"));

	std::cout << "Launching command: " << command << std::endl;

	SetEnvironmentVariable(L"DISPLAY", L"127.0.0.1:0.0");

	HANDLE hProcess;
	HANDLE outputHandle;
	HANDLE readOutputHandle = nullptr;

	if (redirectStdOut) {
		outputHandle = GetStdHandle(STD_OUTPUT_HANDLE);
	}
	else {
		SECURITY_ATTRIBUTES saAttr;
		saAttr.nLength = sizeof(SECURITY_ATTRIBUTES);
		saAttr.bInheritHandle = TRUE; // Child process must inherit this handle
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
	HRESULT hr = pWslLaunch(L"archlinux", std::wstring(command.begin(), command.end()).c_str(), FALSE, GetStdHandle(STD_INPUT_HANDLE), outputHandle, outputHandle, &hProcess);
	if (SUCCEEDED(hr)) {
		CloseHandle(hProcess);
		if (redirectStdOut) {
			return "";
		}
		else {
			CloseHandle(outputHandle);

			std::string result = HandleToString(readOutputHandle);
			CloseHandle(readOutputHandle);
		}
	}

	return "";
}

static std::string WindowsToWslPath(const std::string& winPath) {
	return ExecuteCommand("wslpath -a \"" + winPath + "\"", false);
}

int main(int argc, char* argv[])
{
	std::vector<std::string> args(argv + 1, argv + argc);
	std::string command = "/bin/zsh -lc 'inkscape " + Join(args, " ") + "'";

	std::string translatedPaths = WindowsToWslPath(args[0]);
	std::cout << "Translated path: " << translatedPaths << std::endl;
	ExecuteCommand(command);
}