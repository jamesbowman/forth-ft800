# forth-ft800
Forth bindings for the FTDI FT800/Gameduino2

This is a Forth driver for the
[FTDI FT800](http://www.ftdichip.com/Products/ICs/FT800.html), as
used in many devices, including the
[Gameduino 2](http://gameduino.com).

It assumes a 32-bit ANS Forth plus a handful of other words for interfacing.

`gd2.fs` - the bindings

`standard.fs` and `smoketest.fs` are a simple compile test.
You can run it like this:

    gforth standard.fs smoketest.fs
