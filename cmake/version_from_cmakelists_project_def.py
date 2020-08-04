cmakelists = open('CMakeLists.txt', 'r')
cmakelists_lines = cmakelists.read().split('\n')
projectdef = list(filter(lambda line: line.startswith('project'), cmakelists_lines))[0].split(' ')
print(projectdef[projectdef.index('VERSION') + 1])