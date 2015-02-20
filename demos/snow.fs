\ Fill the screen with random 'snow'
\
\ requires gd2.fs and:
\
\ random  ( -- u ) \ u is a random number
\
\ This is an ANS Forth program:
\   Requiring the Core Extensions word set
\   Requiring the Facility Extensions word set
\

512 512 * constant ALLRAM

: snow
    GD.init

    GD.L8 512 512 GD.BitmapLayout
    GD.NEAREST GD.REPEAT GD.REPEAT 480 272 GD.BitmapSize

    0 ALLRAM GD.cmd_memwrite
    ALLRAM 0 do
        random GD.c
    4 +loop

    begin
        random GD.BitmapTransformC
        random GD.BitmapTransformF
        GD.Clear
        GD.BITMAPS GD.Begin
        0 0 0 0 GD.Vertex2ii
        GD.RestoreContext
        240 136 31 GD.OPT_CENTER s" snow" GD.cmd_text
        GD.swap
    again
;
