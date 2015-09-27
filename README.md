# forth-ft800

[![Build Status](https://travis-ci.org/jamesbowman/swapforth.svg?branch=master)](https://travis-ci.org/jamesbowman/forth-ft800)

This is a Forth driver for the
[FTDI FT800](http://www.ftdichip.com/Products/ICs/FT800.html) GPU, as
used in many devices, including the
[Gameduino 2](http://gameduino.com).

It assumes a 32-bit ANS Forth plus a handful of other words for interfacing.
The API is identical to the one documented in the
[Gameduino 2 book](http://excamera.com/files/gd2book_v0.pdf).
There are also a handful of sample applications.

`gd2.fs` contains the bindings themselves.
There are notes at the beginning of the file on the required support words.
`smoketest.fs` includes some reference implementations of the support words.

There is also a simple compile test.
You can run it like this:

    gforth standard.fs smoketest.fs

There are a handful of demos, all of which require only a 32-bit ANS Forth and the driver.

### helloworld

[Single line of text](demos/helloworld.fs)

![fizz](https://github.com/jamesbowman/gd2-book/blob/master/assets/helloworld.png)

### fizz

[High-speed circle drawing](demos/fizz.fs)

![fizz](https://github.com/jamesbowman/gd2-book/blob/master/assets/fizz-6.png)

### widgets

[Full widget set demo](demos/widgets.fs)

![widgets](https://github.com/jamesbowman/gd2-book/blob/master/assets/widgets3d.png)

### blobs

[Interactive drawing using the touch-screen](demos/blobs.fs)

![blobs](https://github.com/jamesbowman/gd2-book/blob/master/assets/blobs.png)

### frogger

[Simple animated game, a direct conversion of the Arduino version](demos/frogger.fs)

![frogger](https://github.com/jamesbowman/gd2-book/blob/master/assets/frogger.png)
