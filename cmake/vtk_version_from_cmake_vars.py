import sys
import re

cmake_filename = sys.argv[1]


def parse_var(varstr):
    m = re.search(r'(set\s*\()([^\s]+)\s*([^)]+)', varstr)
    return m.group(2), m.group(3)


with open(cmake_filename, 'r') as cmake_file:
    vars = dict(list(map(lambda varline: parse_var(varline), filter(lambda line: line.replace(' ', '').startswith('set('), cmake_file.read().split('\n')))))
    major_version = vars['VTK_MAJOR_VERSION']
    minor_version = vars['VTK_MINOR_VERSION']
    build_version = vars['VTK_BUILD_VERSION']
    print(f'{major_version}.{minor_version}.{build_version}')