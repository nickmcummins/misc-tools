from os import popen

ICO_SIZES = [16, 20, 24, 32, 40, 48, 64, 128, 256]
ICO_SIZES.reverse()
RESIZE_SIZES = {
    32: 24,
    20: 24
}

def run(cmd):
    print(f'Running {cmd} ...')
    output = popen(cmd).read()
    return output


def pngcompress(pngfile):
    run(f'pngout {pngfile}')
    run(f'optipng -silent -o7 {pngfile}')
    run(f'advpng -z4 {pngfile}')

