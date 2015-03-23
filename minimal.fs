\ This file defines the minimal extra words needed by gd2.fs.

: LOCALWORDS
;

: PUBLICWORDS
;

: DONEWORDS
;

\ These are the 16-bit access words

: uw@ ( a -- u )  \ unsigned 16-bit fetch
    dup c@ swap 1+ c@ 8 lshift +
;

: w@ ( a -- n )  \ signed 16-bit fetch
    uw@
    dup 32768 and if
        65536 -
    then
;


\ These SPI IO words are only stubs

: gd2-spi-init
;

: gd2-sel
;

: gd2-unsel
;

: >spi
    drop
;

: spi>
    0
;

: random 0 ;
: randrange ;
