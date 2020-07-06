class PackageNode:
    def __init__(self, name):
        self.name = name
        self.dependency_edges = []

    def add_dependency(self, package_node):
        self.dependency_edges.append(package_node)

    def __str__(self):
        return f'{self.name} -> [{",".join(list(map(lambda edge: edge.name, self.dependency_edges)))}]'
