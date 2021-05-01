import os
import sys

icontheme = sys.argv[1]
filelist = os.popen(f'ls -a -R {icontheme}/*').read()
files = filelist.split('\n')
current_dir = None
files_to_remove = []
for file in files:
    if file.endswith(':'):
        current_dir = file.replace(':', '')
        print(f'Entering directory {current_dir}.')
    if file.find(' ') > 0:
        files_to_remove.append(f'{current_dir}/{file}')
        print(f'Found file {current_dir}/{file} containing spaces.')


print('\n'.join(map(lambda filename: 'sudo rm "' + filename + '"', files_to_remove)))
