import os
from ilio import read, write
import argparse

CMAKE_PROJECT_NAME = 'CMAKE_PROJECT_NAME:STATIC'


def backup(filename):
    os.popen(f'cp {filename} {filename}.bak').read()


def starts_with(prefixes, string):
    for prefix in prefixes:
        if string.startswith(prefix) or string.startswith(CMAKE_PROJECT_NAME):
            return True
    return False


def not_internal(option_line):
    return ':INTERNAL' not in option_line and 'INTERNAL:' not in option_line


def options_dict(option_lines):
    return dict(map(lambda opt: [opt.split('=')[0], opt.split('=')[1]], filter(lambda line: starts_with(option_prefixes, line) and not_internal(line), option_lines)))


if __name__ == '__main__':

    parser = argparse.ArgumentParser(description='Copy CMake options between CMakeCache.txt files.')
    parser.add_argument('from_file', metavar='src', type=str, help='source CMakeCache.txt file to copy from.')
    parser.add_argument('to_file', metavar='dest', type=str, help='destination CMakeCache.txt to replace options with those from src.')
    parser.add_argument('option_prefixes', metavar='option_prefixes', type=str, help="comma-separated list of option name prefixes to copy froms src to dest.")

    args = parser.parse_args()

    from_file = args.from_file
    to_file = args.to_file
    option_prefixes = args.option_prefixes.split(',')
    print(f'from file: {from_file}')
    print(f'to file: {to_file}')
    print(f'option prefixes: {",".join(option_prefixes)}')

    backup(from_file)

    from_lines = read(from_file).split('\n')
    from_options = options_dict(from_lines)
    project_name = from_options[CMAKE_PROJECT_NAME]

    from_build_dir = from_options[f'{project_name}_BINARY_DIR:STATIC']
    from_source_dir = from_options[f'{project_name}_SOURCE_DIR:STATIC']

    print(f'from file build directory: {from_build_dir}')
    print(f'from file source directory: {from_source_dir}')

    backup(to_file)
    to_file_options = read(to_file)
    to_lines = to_file_options.split('\n')
    to_options = options_dict(to_lines)
    to_build_dir = to_options[f'{project_name}_BINARY_DIR:STATIC']
    to_source_dir = to_options[f'{project_name}_SOURCE_DIR:STATIC']
    print(f'destination file build directory: {to_build_dir}')
    print(f'destination file source directory: {to_source_dir}')

    for to_line in to_lines:
        for option in from_options.keys():
            if to_line.startswith(option):
                option_name = option.split(':')[0]
                current_value = to_line.split("=")[-1]
                replaced_value = from_options[option].replace(from_build_dir, to_build_dir).replace(from_source_dir, to_source_dir)
                if current_value != replaced_value:
                    print(f'Replacing {option_name} value of {current_value} to {replaced_value}')
                    replaced_option = f'{option}={from_options[option]}'
                    to_file_options = to_file_options.replace(to_line, replaced_option)

    write(to_file, to_file_options)
