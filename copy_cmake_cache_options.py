import sys
import os
from ilio import read, write


def backup(filename):
    os.popen(f'cp {filename} {filename}.bak').read()


def starts_with(prefixes, string):
    for prefix in prefixes:
        if string.startswith(prefix):
            return True
    return False


if __name__ == '__main__':
    from_file = sys.argv[1]
    to_file = sys.argv[2]
    option_prefixes = sys.argv[3].split(',')
    print(f'from file: {from_file}')
    print(f'to file: {to_file}')
    print(f'option prefixes: {",".join(option_prefixes)}')

    backup(from_file)

    from_lines = read(from_file).split('\n')
    from_options = dict(map(lambda option: [option.split('=')[0], option.split('=')[1]], filter(lambda line: starts_with(option_prefixes, line) and not ':INTERNAL' in line and not 'INTERNAL:' in line, from_lines)))

    backup(to_file)
    to_file_options = read(to_file)
    to_lines = to_file_options.split('\n')
    for to_line in to_lines:
        for option in from_options.keys():
            if to_line.startswith(option):
                replaced_option = f'{option}={from_options[option]}'
                to_file_options = to_file_options.replace(to_line, replaced_option)

    write(to_file, to_file_options)
