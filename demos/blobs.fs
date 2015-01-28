\ Conversion of the 'blobs' sample from the
\ "Gameduino 2: Tutorial, Reference and Cookbook"
\

: rr ( n0 -- n1 ) \ n1 is a random number between 0 and n0
    random um* nip
;

-16384 -16384 2constant OFFSCREEN

128 constant NBLOBS
create xys NBLOBS 2* cells allot
0 value blob_i
: xy[] 2* cells xys + ;

: blobs
    GD.init
    GD.REG_TOUCH_TRANSFORM_A 24 GD.cmd_memwrite
    442 GD.c 67773 GD.c 128238 GD.c -69606 GD.c -257 GD.c 17998943 GD.c

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
