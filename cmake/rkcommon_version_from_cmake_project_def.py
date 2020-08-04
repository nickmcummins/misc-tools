cmakelists = open('CMakeLists.txt', 'r')
cmakelists_lines = cmakelists.read().split('\n')
projectdef = list(filter(lambda line: line.startswith('project'), cmakelists_lines))[0].split(' ')

for i in range(0, len(projectdef)):
    if projectdef[i] == 'VERSION':
        projectversion = projectdef[i + 1]
print(projectversion)