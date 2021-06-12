import os
import sys
import networkx as nx
import matplotlib.pyplot as plt
from networkx.drawing.nx_pydot import graphviz_layout
from pkgbuild import PkgBuild

EXCLUDE_PKGDIR_PREFIXES = ['cartotype-maps']


def starts_with_prefixes(filename, prefixes):
    return len(list(filter(lambda prefix: filename.startswith(prefix), prefixes))) > 0


def get_pkgbuilds(pkgbuildsdir):
    dirfiles = list(filter(lambda dirfile: not starts_with_prefixes(dirfile, EXCLUDE_PKGDIR_PREFIXES), os.listdir(pkgbuildsdir)))
    dirfilepaths = list(filter(lambda dirfile: os.path.isdir(dirfile) and 'PKGBUILD' in os.listdir(dirfile), map(lambda dirfile: f'{pkgbuildsdir}/{dirfile}', dirfiles)))
    return list(map(lambda pkgbuilddir: PkgBuild(pkgbuilddir), map(lambda pkgdir: f'{pkgdir}/PKGBUILD', dirfilepaths)))


class PkgbuildDependencyTree:
    def __init__(self, pkgbuildsdir):
        self.pkgbuilds = get_pkgbuilds(pkgbuildsdir)
        self.known_packages = list(map(lambda p: p.package_name(), self.pkgbuilds))
        self.graph = nx.DiGraph()
        self.graph.add_nodes_from(self.known_packages)
        for pkgbuild in self.pkgbuilds:
            for depend in pkgbuild.depends:
                if depend in self.known_packages:
                    self.graph.add_edge(pkgbuild.package_name(), depend)
            for optdepend in pkgbuild.optdepends:
                if optdepend in self.known_packages:
                    self.graph.add_edge(pkgbuild.package_name(), optdepend)
        self.graph.remove_nodes_from(list(nx.isolates(self.graph)))

    def show_graph(self):
        pos = graphviz_layout(self.graph, prog='dot')
        nx.draw(self.graph, pos=pos, with_labels=True, node_size=2500, font_size=8, font_family='Liberation Sans')
        plt.show()


if __name__ == '__main__':
    pkgbuild_dependency_tree = PkgbuildDependencyTree(sys.argv[1])
    pkgbuild_dependency_tree.show_graph()
