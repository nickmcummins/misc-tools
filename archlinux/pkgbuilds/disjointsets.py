def make_set(x):
    return frozenset([x])


class DisjointSets:
    def __init__(self, vertices):
        self.sets = set([make_set(v) for v in vertices])
        for v in vertices:
            for u in v.edges:
                set_u = self.find_set(u)
                set_v = self.find_set(v)

                if set_u != set_v:
                    self.union(set_u, set_v)

    def find_set(self, e):
        for subset in self.sets:
            if e in subset:
                return subset

    def union(self, set_u, set_v):
        self.sets.add(frozenset.union(set_u, set_v))
        self.sets.remove(set_u)
        self.sets.remove(set_v)

    def get_sets(self):
        return self.sets