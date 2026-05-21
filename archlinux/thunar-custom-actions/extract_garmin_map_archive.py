import os
import subprocess
import sys
from pathlib import Path

def run(cmd):
    print(f'Running command {cmd}.')
    result = subprocess.run(cmd, capture_output=True, shell=True)
    print(result.stdout)
    return result.stdout.decode('utf-8')


ARCHIVE_FILES_TABLE_HEADER = 'Date      Time    Attr         Size   Compressed  Name'
ARCHIVE_FILES_TABLE_FILENAME_SUBSTRING_INDEX = len('------------------- ----- ------------ ------------  ')
TABLE_END = '------------------- ----- ------------ ------------  ------------------------'

def list_archive_files(archive_file):
    lines = list(filter(lambda line: len(line) > 0, map(lambda line: line.strip(), run(f'7z l "{archive_file}"').strip().split('\n'))))
    linenum = 0
    while lines[linenum] != ARCHIVE_FILES_TABLE_HEADER:
        linenum += 1

    files = []
    linenum += 2
    while lines[linenum] != TABLE_END:
        fileinfo = lines[linenum][ARCHIVE_FILES_TABLE_FILENAME_SUBSTRING_INDEX:len(lines[linenum])]
        files.append(fileinfo)
        linenum += 1

    return files

if __name__ == '__main__':
    map_archive_file = sys.argv[1]

    output_dir = os.path.dirname(map_archive_file)
    map_suffix = Path(map_archive_file).stem.split(' ')[-1]
    map_files = list_archive_files(map_archive_file)

    run(f'7z x -aoa -o{output_dir} "{map_archive_file}"')
    for filename in map_files:
        filename = f'{output_dir}/{filename}'
        new_filename = filename.replace(' ', '-')
        ext = Path(filename).suffix
        new_filename = new_filename.replace(ext, f'-{map_suffix}{ext}')
        print(f'Renaming {filename} to {new_filename}')
        os.rename(filename, new_filename)