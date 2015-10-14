# python hue.py ; exit
# Compile everything using gforth, using standard.fs to make sure
# that only ANS standard Forth words are used.

gforth standard.fs smoketest.fs
exit

# Load everything using the interactive shell
# swapforth-shell.py gd2.fs demos/chess.fs
# gforth standard.fs demos/pentom.fs 
swapforth-shell.py -h /dev/ttyUSB0 gd2.fs ../swapForth-private/src/detector.fs demos/tools.fs fakecal.fs \
demos/helloworld.fs \
demos/counting.fs \
demos/fizz.fs \
demos/widgets.fs \
demos/blobs.fs \
demos/pinwheels.fs \
demos/metaball.fs \
demos/globe.fs \
demos/menu.fs \
