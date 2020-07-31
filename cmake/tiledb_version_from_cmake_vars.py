import sys
import re

cmake_filename = sys.argv[1]


def parse_define(line):
    m = re.search(r'#define\s([^\s]+)\s([^\s]+)', line)
    return m.group(1), m.group(2)


with open(cmake_filename, 'r') as cmake_file:
    defines = dict(list(map(lambda defineline: parse_define(defineline), filter(lambda line: line.startswith('#define'), cmake_file.read().split('\n')))))
    version_major = defines['TILEDB_VERSION_MAJOR']
    version_minor = defines['TILEDB_VERSION_MINOR']
    version_patch = defines['TILEDB_VERSION_PATCH']
    print(f'{version_major}.{version_minor}.{version_patch}')
