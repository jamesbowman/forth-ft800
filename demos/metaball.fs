\ Converted from FTDI's 'metaball' sample
\
\ requires gd2.fs and:
\
\ mini-oof.fs       tiny object-oriented library
\
\ randrange  ( u0 -- u1 ) \ u1 is a random number less than u0
\
\
\ This is an ANS Forth program:
\   Requiring the Core Extensions word set
\   Requiring the Facility Extensions word set
\   Requiring the String word set
\

51                      constant w
31                      constant h
80                      constant NBLOBS

0 value centerx
0 value centery
0 value scale

w h *       constant wh

object class
    1 cells var x
    1 cells var y
    1 cells var dx
    1 cells var dy
    method kick         \ randomize velocity
    method born         \ randomize position, velocity
    method animate      \ compute new position
    method draw         \ draw self
    method brightness   \ return brightness from center point
end-class blob

: rr randrange ;

: rvel  ( -- n ) \ n is a random velocity
    512 randrange 256 -
;

:noname
    >r
    rvel r@ dx !
    rvel r> dy !
; blob defines kick

:noname
    >r
    centerx 2* rr r@ x !
    centery 2* rr r@ y !
    r> kick
; blob defines born

:noname
    dup x @
    swap y @
    GD.Vertex2f
; blob defines draw

: attract ( c pos -- v )
    < if
        -6
    else
        6
    then
;

:noname
    >r
    centerx r@ x @ attract r@ dx +!
    centery r@ y @ attract r@ dy +!

    r@ dx @ r@ x +!
    r@ dy @ r> y +!
; blob defines animate

\ array of blob addresses
create bb NBLOBS cells allot
: b[] ( u -- a ) cells bb + @ ;

\ reciprocal table
w dup * h dup * + 1+ constant recipsz
create recip recipsz allot

:noname ( i j blob -- u )
    >r
    8 lshift r@ y @ - dup * swap
    8 lshift r> x @ - dup *
    + 19 rshift
    recipsz 1- min
    recip + c@
; blob defines brightness


: new ( class -- o )  align here over @ allot tuck ! ;

: metaball
    GD.init
    GD.REG_HSIZE GD.@ 2/ 16 * to centerx
    GD.REG_VSIZE GD.@ 2/ 16 * to centery

    GD.REG_HSIZE GD.@ 6 800 */ to scale

    \ Build the reciprocal table
    200 recip c!
    recipsz 1 do
        4800 i 4 * / 200 min
        recip i + c!
    loop

cr ." HERE "

    \ Create blobs at random locations
    NBLOBS 0 do
        blob new
        i cells bb + !
    loop
    NBLOBS 0 do
        i b[] born
    loop

    \ Background bitmap
    GD.L8 w h GD.BitmapLayout
    GD.BILINEAR GD.BORDER GD.BORDER 0 0 GD.BitmapSize

    begin
        GD.SaveContext

        \ Draw the background
        16 GD.BitmapTransformA
        16 GD.BitmapTransformE
        GD.BITMAPS GD.Begin
        GD.SRC_ALPHA 0 GD.BlendFunc
        255 0 0 GD.ColorRGB
        0 0 GD.Vertex2f
        GD.SRC_ALPHA 1 GD.BlendFunc
        255 255 0 GD.ColorRGB
        0 0 GD.Vertex2f

        \ Draw the black blobs on top
        GD.RestoreContext
        0 GD.ColorRGB#
        GD.POINTS GD.Begin
        NBLOBS 3 do
            i scale * GD.PointSize
            i b[] draw
        loop

        \ Move all blobs
        NBLOBS 0 do
            i b[] animate
        loop

        \ Randomize one blob's velocity
        10 randrange 0= if
            NBLOBS randrange b[] kick
        then

        \ Build up a new w*h background image at pad
        pad
        h 0 do
            w 0 do
                i j 0 b[] brightness
                i j 1 b[] brightness +
                i j 2 b[] brightness +
                255 min
                over c! 1+
            loop
        loop
        drop

        \ Transfer it to graphics memory
        0 wh GD.cmd_memwrite
        pad wh GD.supply
        \ pad wh bounds do
        \     i @ GD.c
        \ 4 +loop
        GD.swap
    again
;
