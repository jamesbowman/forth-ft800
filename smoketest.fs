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

s" minimal.fs" included

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

s" gd2.fs" included

: noop ;
s" mini-oof.fs" included

marker xxx s" demos/blobs.fs"       included  xxx
marker xxx s" demos/fizz.fs"        included  xxx
marker xxx s" demos/metaball.fs"    included  xxx
marker xxx s" demos/snow.fs"        included  xxx
: s\" postpone s" ; immediate
marker xxx s" demos/widgets.fs"     included  xxx

cr .( Compilation completed)
bye
