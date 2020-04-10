import os
import sys


def tar_command_flag(file):
    if file.endswith('.tar.bz2'):
      return 'j'
    elif file.endswith('.tar.gz'):      
      return 'z'
    elif file.endswith('.tar.xz'):
      return 'J'

def list_files_in(file):
    cmd = f'tar -{tar_command_flag(file)}tvf {file}'
    contents = list(filter(lambda file: not file.startswith('/.'), map(lambda line: '/' + line.split(' ')[-1], os.popen(cmd).read().split('\n'))))
    return list(set(contents))

def diff_files_in(file1, file2):
    files1 = set(list_files_in(file1))
    files2 = set(list_files_in(file2))
    print(f'Number of files in {file1}: {str(len(files1))}\t{file2}: {str(len(files2))}')
    diff = list(files1 - files2)
    reverse_diff = list(files2 - files1)
    stri = ''
    for i in range(0, max(len(diff), len(reverse_diff))):
        if i < len(diff):
            stri += diff[i]
        if i < len(reverse_diff):
            if not stri.endswith('\n'):
                stri += '\t'
            stri += reverse_diff[i]
        if not stri.endswith('\n'):
            stri += '\n'
    return stri

if __name__ == '__main__':
    print(diff_files_in(sys.argv[1], sys.argv[2]))
    
      
