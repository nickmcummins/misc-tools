import argparse
import os
from os import path
from icons.common import run
from icons.ico_add_size import pngsize

if __name__ == '__main__':
    parser = argparse.ArgumentParser(prog='xdg_icon_resource_install_ico', description='Install a Windows icon (.ico) using xdg-icon-resource install for all embedded icons sizes..', epilog='Text at the bottom of help')
    parser.add_argument('icofile')  # positional argument
    parser.add_argument('--context', choices=['apps', 'actions', 'devices', 'emblems', 'filesystems', 'location', 'mimetypes', 'stock'], required=True)  # option that takes a value
    parser.add_argument('--sudo', action=argparse.BooleanOptionalAction)
    args = parser.parse_args()

    icofile = args.icofile
    iconname = path.basename(icofile).replace(".ico", "")

    if path.exists('tmp'):
        run('rm -rf tmp')

    os.mkdir('tmp')

    run(f'icotool -x  {icofile} -o tmp')

    pngfiles = os.listdir('tmp')

    for pngfile in pngfiles:
        prefix = 'sudo ' if args.sudo else ''
        run(f'{prefix}xdg-icon-resource install --novendor --context {args.context} --size {pngsize(pngfile)} tmp/{pngfile} "{iconname}"')