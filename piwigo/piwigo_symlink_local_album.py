import pathlib
import argparse
import os

IMGFORMAT = 'JPG'

if __name__ == '__main__':
    parser = argparse.ArgumentParser(description='Symlink a local album directory to the "galleries" subdirectory in a local Piwigo instance.')
    parser.add_argument('src_album', type=str, help='Location of album to symlink, relative to ALBUMS_ROOT')
    parser.add_argument('piwigo_dir', type=str, help='Location of local Piwigo instance (e.g. /srv/http/piwigo)')
    parser.add_argument('--sudo', '-su', action='store_true', help='Execute shell commands using sudo')
    parser.add_argument('--range', type=str, default=None, help='Only create symlinks for photos in numeric range')

    args = parser.parse_args()
    src_album, piwigo_dir, use_sudo = args.src_album, args.piwigo_dir, args.sudo
    minrange = int(args.range.split('-')[0]) if args.range is not None else 0
    maxrange = int(args.range.split('-')[1]) if args.range is not None else 1000000

    albums_root = os.getenv('ALBUMS_ROOT', f'{str(pathlib.Path.home())}/Pictures/Albums')

    def sh(command):
        if use_sudo:
            command = f'sudo {command}'

        os.popen(command).read()

    def symlink_img(imgfilename):
        piwigo_album_dir = f'{piwigo_dir}/galleries/{src_album}'
        if not os.path.exists(piwigo_album_dir):
            sh(f'mkdir -p {piwigo_album_dir}')

        sh(f'ln -s {albums_root}/{src_album}/{IMGFORMAT}/{imgfilename} {piwigo_album_dir}')

    def is_expected_imgformat(imgfilename):
        return imgfilename.split('.')[-1].lower() == IMGFORMAT.lower()

    def is_in_range(imgfilename):
        imgnum = imgfilename.split('.')[0].split('_')[-1]
        return minrange <= imgnum <= maxrange


    imgs = list(filter(lambda file: is_expected_imgformat(file) and is_in_range(file), os.listdir(f'{albums_root}/{src_album}/{IMGFORMAT}')))
    for img in imgs:
        symlink_img(img)
