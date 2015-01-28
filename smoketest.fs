\ Smoke test for gd2.fs
\ This file defines the extra words needed by gd2.fs,
\ and compiles it.

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

: uw@ ( a -- x )
    dup c@ swap 1+ c@ 8 lshift +
;

: w@    uw@ ;

s" gd2.fs" included
: random 0 ;
: noop ;
s" mini-oof.fs" included
s" demos/metaball.fs" included
s" demos/snow.fs" included
cr .( Compilation completed)
bye
