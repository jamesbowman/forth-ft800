# forth-ft800

This is a Forth driver for the
[FTDI FT800](http://www.ftdichip.com/Products/ICs/FT800.html) GPU, as
used in many devices, including the
[Gameduino 2](http://gameduino.com).

It assumes a 32-bit ANS Forth plus a handful of other words for interfacing.

`gd2.fs` contains the bindings themselves.
There are notes at the beginning of the file on the required support words.
`smoketest.fs` includes some reference implementations of the support words.

There is also a simple compile test.
You can run it like this:

    gforth standard.fs smoketest.fs
