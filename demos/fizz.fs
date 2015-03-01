\ Conversion of the 'fizz' sample from the
\ "Gameduino 2: Tutorial, Reference and Cookbook"
\
\
\ This is an ANS Forth program:
\   Requiring the Core Extensions word set
\   Requiring the Facility Extensions word set
\
\
\
\ Requires gd2.fs and:
\
\ randrange  ( u0 -- u1 ) \ u1 is a random number less than u0
\

: rr randrange ;

: fizz
    GD.init
    GD.REG_HSIZE GD.@ 16 * GD.REG_VSIZE GD.@ 16 *   ( width height )
    begin
        GD.Clear
        GD.POINTS GD.Begin
        200 0 do
            16 50 * rr GD.PointSize
            256 rr 256 rr 256 rr GD.ColorRGB
            256 rr GD.ColorA
            over rr over rr GD.Vertex2f
        loop
        GD.swap
    again
;
