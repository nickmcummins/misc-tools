class Node:
    def __init__(self, name):
        self.name = name
        self.edges = []

    def add_dependency(self, package_node):
        self.edges.append(package_node)

    def __repr__(self):
        return f'{self.name} -> [{",".join(list(map(lambda edge: edge.name, self.edges)))}]'
