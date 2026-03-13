import os

import desktop_entry_lib
import sys

WSL_EXE = 'C:\Windows\System32\wsl.exe'

def get_wsl_distribution_name():
    with open('/etc/wsl-distribution.conf', 'r') as wsl_distribution_conf_file:
        for line in wsl_distribution_conf_file.read().splitlines():
            if line.startswith('defaultName = '):
                return line.split('=')[1].strip()
        return None

linux_distribution_name = get_wsl_distribution_name()

def wsl_exe_run_args(linux_cmd):
    linux_cmd = linux_cmd.replace('"', '')
    return f'-d {linux_distribution_name} /bin/zsh -lc \'nohup {linux_cmd} >/dev/null 2>&1 & sleep 1\''

def wsl_windows_default_lnk_folder():
    return f'/mnt/c/Users/nickm/AppData/Roaming/Microsoft/Windows/Start Menu/Programs/{linux_distribution_name}'


desktop_entry_file = sys.argv[1]
windows_lnk_file = os.path.basename(desktop_entry_file.replace('.desktop', '.lnk'))
windows_link_outdir = sys.argv[2] if len(sys.argv) > 2 else os.path.dirname(desktop_entry_file)
print(windows_link_outdir)

entry = desktop_entry_lib.DesktopEntry.from_file(desktop_entry_file)

print("Name: " + entry.Name.default_text)
print("Comment: " + entry.Comment.default_text)
print("Type: " + entry.Type)
print("Exec: " + entry.Exec)
print("Icon: " + entry.Icon)

working_dir_arg = f'-w {entry.Path}' if entry.Path is not None else f"\\\\wsl.localhost\\{linux_distribution_name}\\home"
print(f'mslink -l "{WSL_EXE}" -n "{entry.Comment}" -w "{working_dir_arg}" -o "{wsl_windows_default_lnk_folder()}/{windows_lnk_file}" -a "{wsl_exe_run_args(entry.Exec)}"')
