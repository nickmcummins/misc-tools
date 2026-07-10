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

static std::string join(std::vector<std::string> strings, std::string delim) {
	auto joined_view = strings | std::views::join_with(delim);
	return std::ranges::to<std::string>(joined_view);
}

static std::string windowsToWslPath(const std::string& winPath) {
	return "";
}

static void executeCommand(const std::string& command) {
	static HMODULE hLib = LoadLibrary(L"wslapi.dll");
	static auto pWslLaunch = reinterpret_cast<decltype(&WslLaunch)>(GetProcAddress(hLib, "WslLaunch"));

	std::cout << "Launching command: " << command << std::endl;

	SetEnvironmentVariable(L"DISPLAY", L"127.0.0.1:0.0");

	HANDLE hProcess;
	HANDLE stdoutHandle = GetStdHandle(STD_OUTPUT_HANDLE);
	HRESULT hr = pWslLaunch(L"archlinux", std::wstring(command.begin(), command.end()).c_str(), FALSE, GetStdHandle(STD_INPUT_HANDLE), stdoutHandle, GetStdHandle(STD_ERROR_HANDLE), &hProcess);
	if (SUCCEEDED(hr)) {
		WCHAR path[MAX_PATH * 2];
		if (GetProcessImageFileNameW(hProcess, path, _countof(path))) {
		}

		CloseHandle(hProcess);
	}
}

int main(int argc, char* argv[])
{
	std::vector<std::string> args(argv + 1, argv + argc);
	std::string command = "/bin/zsh -lc 'inkscape " + join(args, " ") + "'";
	executeCommand(command);
}