#-------------------------------------------------------------------------------
# Name:        module1
# Purpose:
#  Sample written in response to a question on StackOverflow
#  http://stackoverflow.com/questions/12380163/dcpcrypt-encryption-decrypt-in-pycrypto
#
#  The cipher text was created in a Delphi program using DCPCrypt
#
# See also: http://www.codekoala.com/blog/2009/aes-encryption-python-using-pycrypto/
#
# Author:      sph
#
# Created:     12/09/2012
# Copyright:   (c) sph 2012
# Licence:     <your licence>
#-------------------------------------------------------------------------------

import Crypto
import base64
import binascii
from Crypto.Hash import SHA
from Crypto.Cipher import AES


def main():
    pwd = "password"
    ct = "k8b+uce5Fkp7Hbk/CaGYcuEWTfxlI05as88lJL0mHmJxLsKWqki2YwiFPU9Rx8qiUC2cvWZrQIOnkw=="
    ctbytes = base64.b64decode(ct.encode('utf-8'))
    key = pwd.encode('utf-8')
    s = SHA.new()
    s.update(key)
    key = s.digest()
    key = key + bytes([0,0,0,0])

    print("Hashed key")
    print(binascii.hexlify(key))

    # Set up the IV, note that in ECB the third parameter to the AES.new function is ignored since ECB doesn't use an IV
    # DCPCrypt uses FF bytes in v1 compatability mode or 00 bytes in normal mode
    # This is set by a compiler directive
    ivbytes = bytes([255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255])
    #ivbytes = bytes([0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0])
    #ecb = AES.new(key, AES.MODE_ECB, ivbytes)
    ecb = AES.new(key)
    iv = ecb.encrypt(ivbytes)

    print("IV")
    print(binascii.hexlify(iv))

    cipher = AES.new(key, AES.MODE_CBC, iv)
    msg = cipher.decrypt(ctbytes[:48])

    print("Message")
    print(msg)

if __name__ == '__main__':
    main()
