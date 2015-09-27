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

16 50 * constant pointsize
: fizz
    GD.init
    GD.REG_HSIZE GD.@ 16 * GD.REG_VSIZE GD.@ 16 *   ( width height )
    begin
        GD.Clear
        GD.POINTS GD.Begin
        200 0 do
            pointsize rr GD.PointSize
            random GD.ColorRGB#
            random GD.ColorA
            over rr over rr GD.Vertex2f
        loop
        GD.swap
    again
;
