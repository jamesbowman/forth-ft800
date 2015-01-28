\ Conversion of the 'fizz' sample from the
\ "Gameduino 2: Tutorial, Reference and Cookbook"
\

: rr ( n0 -- n1 ) \ n1 is a random number between 0 and n1
    random um* nip
;

: fizz
    GD.init
    begin
        GD.Clear
        GD.POINTS GD.Begin
        100 0 do
            16 50 * rr GD.PointSize
            256 rr 256 rr 256 rr GD.ColorRGB
            256 rr GD.ColorA
            480 rr 272 rr 0 0 GD.Vertex2ii
        loop
        GD.swap
    again
;
