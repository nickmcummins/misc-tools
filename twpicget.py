import sys
import os
import subprocess

def curl(url, outputfile):
    cmd = f'curl {url} --output {outputfile}'
    print(cmd)
    subprocess.run(['curl', url, '--output', outputfile])

def sh(cmd, print_output=True):
    print(cmd)
    output = os.popen(cmd).read()
    if print_output:
        print(output)
    return output

if __name__ == '__main__':
    imageurl = sys.argv[1].replace('format=jpg', 'format=png')
    imageurl = imageurl.split('name=')[0]
    imageurl = f'{imageurl}name=large'
    print(f'image url: {imageurl}')
    imageid = imageurl.split('media/')[-1].split('?')[0]
    print(f'saving to {imageid}.png')
    curl(imageurl, f'{imageid}.png')
    sh(f'pngcompress {imageid}.png')
    
