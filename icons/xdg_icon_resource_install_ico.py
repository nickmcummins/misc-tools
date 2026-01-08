import os
import sys
from os import popen, path


def run(cmd):
    print(f'Running {cmd} ...')
    output = popen(cmd).read()
    return output

def pngsize(pngfile):
    return pngfile.split('_')[-1].split('x')[0]


if __name__ == '__main__':
    icofile = sys.argv[1]
    iconname = path.basename(icofile).replace(".ico", "").replace("-", " ")

    if path.exists('tmp'):
        run('rm -rf tmp')

    os.mkdir('tmp')

    run(f'icotool -x  {icofile} -o tmp')

    pngfiles = os.listdir('tmp')

    for pngfile in pngfiles:
        #print(pngfile)
        run(f'xdg-icon-resource install --novendor --size {pngsize(pngfile)} tmp/{pngfile} "{iconname}"')