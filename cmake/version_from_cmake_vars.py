import sys
import re

cmake_filename = sys.argv[1]


def parse_var(varstr):
    m = re.search(r'(set\s*\()([^\s]+)\s([^)]+)', varstr)
    return m.group(2), m.group(3)


with open(cmake_filename, 'r') as cmake_file:
    setlines = list(filter(lambda line: line.startswith('set') and line.endswith(')'), cmake_file.read().split('\n')))
    vars = dict(list(map(lambda varline: parse_var(varline), setlines)))
    cmake_package = list(filter(lambda var: var.endswith('_VERSION_MAJOR'), vars.keys()))[0].split('_')[0]
    version_major = vars[f'{cmake_package}_VERSION_MAJOR']
    version_minor = vars[f'{cmake_package}_VERSION_MINOR']
    version_patch = vars[f'{cmake_package}_VERSION_PATCH']
    print(f'{version_major}.{version_minor}.{version_patch}')
