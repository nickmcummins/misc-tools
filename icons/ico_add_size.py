import argparse
import os
from os import popen, path


def run(cmd):
    print(f'Running {cmd} ...')
    output = popen(cmd).read()
    return output

def pngsize(pngfile):
    return pngfile.split('_')[-1].split('x')[0]

def nextsize(size, available_sizes):
    bigger_sizes = list(filter(lambda available_size: available_size > size, available_sizes))
    bigger_sizes.sort()
    return bigger_sizes[0]

class IcoSize:
    def __init__(self, name, info):
        self.name = name
        self.index = info['index']
        self.size = int(info['width'])
        self.bit_depth = info['bit-depth']
        self.palette_size = info['palette-size']
        self.pngfilename = f'{self.name}_{self.index}_{self.size}x{self.size}x{self.bit_depth}.png'

    @staticmethod
    def parse_info_line(infoline):
        info = dict(map(lambda p: [p[0], p[1]], infoline))
        return info

    def __str__(self):
        return f'IcoSize - {self.name} [index={self.index}, size={str(self.size)}, bit_depth={self.bit_depth}, palette-size={self.palette_size}]'


if __name__ == '__main__':
    parser = argparse.ArgumentParser(prog='ico_add_size', description='Adds missing size(s) to an .ico file')
    parser.add_argument('icofile')  # positional argument
    parser.add_argument('--sizes', default='16,20,24,32,40,48,64,128,256')
    args = parser.parse_args()

    icofile = args.icofile
    icon_name = path.splitext(path.basename(icofile))[0]

    if path.exists('tmp'):
        run('rm -rf tmp')

    os.mkdir('tmp')

    run(f'icotool -x  {icofile} -o tmp')

    ico_sizes = dict(map(lambda icosize: (icosize.size, icosize), map(lambda sizeline: IcoSize(icon_name, IcoSize.parse_info_line(sizeline)), list(map(lambda line: list(map(lambda linep: linep.split('='), filter(lambda p: '=' in p, line.split(' ')))), run(f'icotool -l {icofile}').replace('--', '').strip().split('\n'))))))

    wanted_sizes = list(map(lambda wanted_size: int(wanted_size), args.sizes.split(',')))
    missing_sizes = set(wanted_sizes).difference(set(map(lambda icosize: icosize, ico_sizes.keys())))
    for size in ico_sizes:
        print(f'Size {size}')
    for missing_size in missing_sizes:
        print(f'Missing size: {size}')
        next_size = nextsize(missing_size, ico_sizes.keys())
        next_size_ico = ico_sizes[next_size]
        print(f'Next size: {next_size_ico}')

        resized_pngfilename = next_size_ico.pngfilename.replace(str(next_size), str(missing_size))
        run(f'convert tmp/{next_size_ico.pngfilename} -resize {missing_size}x{missing_size} {resized_pngfilename}')