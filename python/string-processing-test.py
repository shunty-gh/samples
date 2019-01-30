import re
import string
import random

# Intall pre-requisites (as well as Python 3):
# $> pip3 install pytest pytest-benchmark
# then:
# $> pytest sring-processing-test.py
# Taken from https://developers.redhat.com/blog/2017/11/16/speed-python-using-rust/?utm_medium=Email&utm_campaign=editorspicks&sc_cid=701f2000000RZg1AAG
# part of a demo on using Rust extensions to speed things up

def count_doubles(val):
    total = 0
    for c1, c2 in zip(val, val[1:]):
        if c1 == c2:
            total += 1
    return total

def count_doubles_2(val):
    total = 0
    chars = iter(val)
    c1 = next(chars)
    for c2 in chars:
        if c1 == c2:
            total += 1
        c1 = c2
    return total

double_re = re.compile(r'(?=(.)\1)')

def count_doubles_regex(val):
    return len(double_re.findall(val))

val = ''.join(random.choice(string.ascii_letters) for i in range(1000000))

def test_pure_python(benchmark):
    benchmark(count_doubles, val)

def test_pure_python2(benchmark):
    benchmark(count_doubles_2, val)

def test_regex(benchmark):
    benchmark(count_doubles_regex, val)
