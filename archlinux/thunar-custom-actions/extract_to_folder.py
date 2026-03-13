#!/usr/bin/env python

import os
import sys
from pathlib import Path

archivefile = sys.argv[1]
archivepath = Path(archivefile)
archivename = archivepath.stem
archivefolder = archivepath.parent
os.popen(f'mkdir -p "{archivefolder}/{archivename}"').read()
print(os.popen(f'7z x -aoa -o"{archivefolder}/{archivename}" "{archivefile}"').read())
