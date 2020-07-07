import os
import sys

from disjointsets import DisjointSets
from node import Node
from pkgbuild_parser import parse_file

packages_by_name = {}


def lookup_or_create_package_node(pkgname):
    if pkgname in packages_by_name.keys():
        package = packages_by_name[pkgname]
    else:
        package = Node(pkgname)
        packages_by_name[pkgname] = package
    return package


if __name__ == '__main__':
    parentdir = sys.argv[1]
    pkgdirs = os.listdir(parentdir)
    provides = list(map(lambda pkgname: pkgname.replace('-git', '').replace('-hg', '').replace('-svn', ''), pkgdirs))
    pkgbuild_pkgdirs = list(filter(lambda path: os.path.isdir(path) and 'PKGBUILD' in os.listdir(path), map(lambda filepath: f'{parentdir}/{filepath}', pkgdirs)))
    for pkgdir in pkgbuild_pkgdirs:
        try:
            pkgbuild = dict(parse_file(open(f'{pkgdir}/PKGBUILD')))
            pkg = lookup_or_create_package_node(pkgbuild['pkgname'])
            if 'depends' in pkgbuild.keys() and len(pkgbuild['depends']) > 0:
                for dependency in filter(lambda depend: depend in provides, pkgbuild['depends']):
                    pkg.add_dependency(lookup_or_create_package_node(dependency))
            else:
                print('[]')

        except Exception as ae:
            print(ae)

    print('\n'.join(map(lambda pkg: str(pkg), packages_by_name.values())))
    ds = DisjointSets(packages_by_name.values())
    disjointed_sets = ds.get_sets()
    print(disjointed_sets)