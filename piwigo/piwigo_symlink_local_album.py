import pathlib
import argparse
import os


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description='Symlink a local album directory to the "galleries" subdirectory in a local Piwigo instance.')
    parser.add_argument('src_album', type=str, help='Location of album to symlink, relative to ALBUMS_ROOT')
    parser.add_argument('piwigo_dir', type=str, help='Location of local Piwigo instance (e.g. /srv/http/piwigo)')

    args = parser.parse_args()
    src_album = args.src_album
    piwigo_dir = args.piwigo_dir
    albums_root = os.getenv('ALBUMS_ROOT', f'{str(pathlib.Path.home())}/Pictures/Albums')

    def symlink_img(img):
        piwigo_album_dir = f'{piwigo_dir}/gallery/{src_album}'
        if not os.path.exists(piwigo_album_dir):
            os.popen(f'mkdir -p {piwigo_album_dir}').read()

        os.popen(f'ln -s {albums_root}/{src_album}/{img} {piwigo_album_dir}').read()

    imgs = os.listdir(f'{albums_root}/{src_album}/JPG')
    for img in imgs:
        symlink_img(img)
