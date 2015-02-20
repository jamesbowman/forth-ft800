\ Conversion of the 'blobs' sample from the
\ "Gameduino 2: Tutorial, Reference and Cookbook"
\
\ This is an ANS Forth program:
\   Requiring the Core Extensions word set
\   Requiring the Double-Number word set
\   Requiring the Facility Extensions word set
\

-16384 -16384 2constant OFFSCREEN

128 constant NBLOBS
create xys NBLOBS 2* cells allot
0 value blob_i
: xy[] 2* cells xys + ;

: blobs
    GD.init
    GD.calibrate

    NBLOBS 0 do
        OFFSCREEN i xy[] 2!
    loop

    begin
        GD.getinputs
        GD.inputs.x -32768 <> if
            GD.inputs.x 16 * GD.inputs.y 16 *
        else
            OFFSCREEN
        then
        blob_i xy[] 2!
        blob_i 1+ NBLOBS mod to blob_i

        255 255 255 GD.ClearColorRGB
        GD.Clear
        GD.POINTS GD.Begin
        NBLOBS 0 do
            i 2* GD.ColorA
            1040 i 8 * - GD.PointSize

            blob_i i + NBLOBS mod >r

            \ Random color for each blob, keyed from (blob_i + i)
            r@ 17 * r@ 23 * r@ 147 * GD.ColorRGB

            r> xy[] 2@ GD.Vertex2f
        loop
        GD.swap
    again
;
