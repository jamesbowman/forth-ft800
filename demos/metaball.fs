\ Converted from FTDI's 'metaball' sample
\
\ requires gd2.fs and:
\
\ mini-oof.fs       tiny object-oriented library
\
\ randrange  ( u0 -- u1 ) \ u1 is a random number less than u0
\

31                      constant w
18                      constant h
80                      constant NBLOBS

240 16 *    constant centerx
136 16 *    constant centery

w h *       constant wh

object class
    1 cells var x
    1 cells var y
    1 cells var dx
    1 cells var dy
    method placement    \ random placement
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
    480 16 * rr r@ x !
    272 16 * rr r@ y !
    rvel r@ dx !
    rvel r> dy !
; blob defines placement

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
    r@ y @ 256 / - dup * swap
    r> x @ 256 / - dup *
    + 3 rshift
    recipsz 1- min
    recip + c@
; blob defines brightness

: metaball
    GD.init

    \ Build the reciprocal table
    200 recip c!
    recipsz 1 do
        4800 i 4 * / 200 min
        recip i + c!
    loop

    \ Create blobs at random locations
    NBLOBS 0 do
        blob new
        i cells bb + !
    loop
    NBLOBS 0 do
        i b[] placement
    loop

    \ Background bitmap
    GD.L8 w h GD.BitmapLayout
    GD.BILINEAR GD.BORDER GD.BORDER 480 272 GD.BitmapSize

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
            i 3 * GD.PointSize
            i b[] draw
        loop

        \ Move all blobs
        NBLOBS 0 do
            i b[] animate
        loop

        \ Build up a new w*h background image at pad
        h 0 do
            w 0 do
                i j 0 b[] brightness
                i j 1 b[] brightness +
                i j 2 b[] brightness +
                255 min
                pad j w * + i + c!
            loop
        loop

        \ Transfer it to graphics memory
        0 wh GD.cmd_memwrite
        pad wh GD.s

        GD.swap
    again
;
