\ Smoke test for gd2.fs
\ This file defines the extra words needed by gd2.fs,
\ and compiles it.

\ These definitions for LOCALWORDS, PUBLICWORDS, DONEWORDS
\ are suitable for a Forth that has vocabularies.
\
\ For a Forth that does not have vocabularies then
\ define
\
\ : LOCALWORDS ; : PUBLICWORDS ; : DONEWORDS ;
\

: LOCALWORDS
    get-current
    get-order wordlist swap 1+ set-order definitions
;

: PUBLICWORDS
    set-current
;

: DONEWORDS
    previous
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
s" gd2.fs" included
: random 0 ;
: randrange drop 0 ;
: noop ;
s" mini-oof.fs" included

marker xxx s" demos/blobs.fs"       included  xxx
marker xxx s" demos/fizz.fs"        included  xxx
marker xxx s" demos/metaball.fs"    included  xxx
marker xxx s" demos/snow.fs"        included  xxx
marker xxx s" demos/widgets.fs"     included  xxx

words
cr .( Compilation completed)
bye
