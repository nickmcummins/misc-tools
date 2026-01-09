import argparse
import os
from os import popen, path


def run(cmd):
    print(f'Running {cmd} ...')
    output = popen(cmd).read()
    return output

def pngsize(pngfile):
    return pngfile.split('_')[-1].split('x')[0]

class IcoSize:
    def __str__(self):
        return super().__str__()

    def __init__(self, info):
        self.index = info['index']
        self.size = info['width']
        self.bit_depth = info['bit-depth']
        self.palette_size = info['palette-size']

    @staticmethod
    def parse_info_line(infoline):
        info = {}
        info = dict(map(lambda p: [p[0], p[1]], infoline))
        print(len(info))
        #print(','.join(info.keys()))
        print(str(info))
        return info


if __name__ == '__main__':
    parser = argparse.ArgumentParser(prog='ico_add_size', description='Adds missing size(s) to an .ico file')
    parser.add_argument('icofile')  # positional argument
    parser.add_argument('--sizes', default='16,20,24,32,40,48,64,128,256')
    args = parser.parse_args()

    icofile = args.icofile

    if path.exists('tmp'):
        run('rm -rf tmp')

    os.mkdir('tmp')

   # run(f'icotool -x  {icofile} -o tmp')

    #sizes = list(map(lambda pngfile: pngsize(pngfile), os.listdir('tmp')))
    print(str(list(map(lambda line: list(map(lambda linep: linep.split('='), line.split(' '))), run(f'icotool -l {icofile}').split('\n')))))
    sizes = list(map(lambda sizeline: IcoSize(IcoSize.parse_info_line(sizeline)), list(map(lambda line: list(map(lambda linep: linep.split('='), filter(lambda p: '=' in p, line.split(' ')))), run(f'icotool -l {icofile}').replace('--', '').strip().split('\n')))))
    print(str(sizes))
    missing_sizes = set(args.sizes.split(',')).difference(set(map(lambda icosize: icosize.size, sizes)))
    for size in sizes:
        print(f'Size {size}')
    for size in missing_sizes:
        print(f'Missing size: {size}')