# Compile everything using gforth, using standard.fs to make sure
# that only ANS standard Forth words are used.

gforth standard.fs smoketest.fs || exit

# Load everything using the interactive shell
swapforth-shell.py gd2.fs demos/*.fs
