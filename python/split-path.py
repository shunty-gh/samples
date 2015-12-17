#-------------------------------------------------------------------------------
# Name:        module1
# Purpose:
#
# Author:      sph
#
# Created:     03/09/2012
# Copyright:   (c) sph 2012
# Licence:     <your licence>
#-------------------------------------------------------------------------------

import os

def main():
    envpath = os.environ["PATH"]
    paths = envpath.split(";")
    srch = os.sep + "windows"
    for path in paths:
        if srch in path.lower():
            print(path)


if __name__ == '__main__':
    main()
