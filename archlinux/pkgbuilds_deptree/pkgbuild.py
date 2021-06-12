import subprocess

PKGBUILD_VARS = ['pkgname', 'provides', 'depends', 'optdepends']


def _remove_versioned_and_multipackages(pkgname_withver):
    pkgname_withver = pkgname_withver.split(' ')[0]
    if pkgname_withver.find('>=') > 0:
        return pkgname_withver.split('>=')[0]
    else:
        return pkgname_withver.split('=')[0]


def _handle_deps_with_descriptions(deps):
    words_ending_with_colon = list(filter(lambda word: word.endswith(':'), deps))
    if len(words_ending_with_colon) > 0:
        return list(map(lambda word: word.replace(':', ''), words_ending_with_colon))
    else:
        return deps


def _parse_deps(depstr):
    if len(depstr) > 0:
        return list(map(lambda dep: _remove_versioned_and_multipackages(dep), _handle_deps_with_descriptions(depstr.split(' '))))
    else:
        return []


class PkgBuild:
    def __init__(self, pkgbuildfile):
        self.pkgbuildfile = pkgbuildfile
        vars_dict = self._get_vars(PKGBUILD_VARS)
        self.pkgname = vars_dict['pkgname']
        self.provides = vars_dict['provides'] if 'provides' in vars_dict.keys() and len(
            vars_dict['provides']) > 0 else None
        self.depends = _parse_deps(vars_dict['depends'])
        self.optdepends = _parse_deps(vars_dict['optdepends'])

    def _get_vars(self, varnames):
        print_vars = '; '.join(map(lambda varname: f'print ${varname}', varnames))
        p = subprocess.Popen(f'source {self.pkgbuildfile}; {print_vars}', stdout=subprocess.PIPE, shell=True, executable='/bin/zsh')
        varlines = p.stdout.readlines()
        vars_printed = list(map(lambda line: line.decode('utf-8').strip(), varlines))
        return {varname: vars_printed[varnames.index(varname)] for varname in varnames}

    def package_name(self):
        pkgname = self.provides if self.provides is not None else self.pkgname
        pkgname = _remove_versioned_and_multipackages(pkgname)
        return pkgname

    def __repr__(self):
        providesstr = f'provides={self.provides}' if self.provides is not None else ''
        return f'{self.pkgname}    {providesstr},depends=({self.depends}),optdepends=({self.optdepends})'
