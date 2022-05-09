import os
import sys


def run(cmd, printcmd=False):
    if printcmd:
        print(f'Running command {cmd}.')
    out = os.popen(cmd).read().strip()
    if printcmd:
        print(out)
    return out


def package_providing_file(filename):
    pacout = run(f'pacman -Qo {filename}')
    pkg = pacout.split(' is owned by ')[1]
    return pkg


if __name__ == '__main__':
    lddout = run(f'ldd {sys.argv[1]}')
    libs = list(map(lambda line: line.strip().split(' => ')[1].split(' ')[0],
                    filter(lambda line: line.find(' => ') > 0, lddout.split('\n'))))
    for lib in libs:
        print(f'{lib}\t{package_providing_file(lib)}')
