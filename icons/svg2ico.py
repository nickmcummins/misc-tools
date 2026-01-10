import os
from os import path
import sys
from icons.common import ICO_SIZES, RESIZE_SIZES, pngcompress, run

if __name__ == '__main__':
    svgfile = path.basename(sys.argv[1])
    filename = svgfile.replace('.svg', '')
    svgfolder = path.dirname(os.path.dirname(sys.argv[1]))
    icofile = svgfile.replace('.svg', '.ico')
    workingdir = path.dirname(__file__)

    sized_icons = set()
    for folder in os.listdir(svgfolder):
        if folder.isdigit():
            sized_icons.add(int(folder))
        else:
            print(f'{folder} is not digit')

    if not path.exists('tmp'):
        os.mkdir('tmp')

    for size in ICO_SIZES:
        sourcesize = RESIZE_SIZES[size] if size in RESIZE_SIZES else size
        sizefolder = sourcesize if sourcesize in sized_icons else 'scalable'
        run(f'inkscape -z -w {size} -h {size} -d 300 {sizefolder}/{svgfile} --export-filename tmp/{filename}_{size}.png --export-type png')
        if size > 32:
            pngcompress(f'tmp/{filename}_{size}.png')


    print(f'Writing {icofile}')
    sized_pngs = ' '.join(list(map(lambda size: f'tmp/{filename}_{size}.png', ICO_SIZES)))
    run (f'magick {sized_pngs} {icofile}')
    print(f'Wrote {icofile}.')