#!/usr/bin/env python

import os
import sys

archivefile = sys.argv[1]
print(os.popen(f'7z x -aoa -o. "{archivefile}"').read())
