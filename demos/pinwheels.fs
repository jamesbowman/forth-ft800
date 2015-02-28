\ Draws animated rotating pinwheels
\
\ Uses floating-point to compute coordinates
\
\ This is an ANS Forth program:
\   Requiring the Core Extensions word set
\   Requiring the Double-Number word set
\   Requiring the Facility Extensions word set
\   Requiring the Floating-Point word set
\   Requiring the Floating-Point Extensions word set
\

3.1415926e fconstant PI
PI 2.0e f* fconstant 2PI

: v ( F: x y -- ) \ draw a vertex
    fswap 16.0e f* f>d d>s 16.0e f* f>d d>s
    GD.Vertex2f
;

variable radius
variable time

: wheel ( npoints -- )
    GD.LINE_STRIP GD.Begin
    2PI 17e f* dup s>d d>f f/       ( step )
    time @ s>d d>f radius f@ f/     ( step theta )
    1+ 0 do
        fdup  fcos radius f@ f* 240e f+
        fover fsin radius f@ f* 136e f+
        v fover f+
    loop
    fdrop fdrop
;

: pinwheel
    GD.init
    0 time !
    begin
        GD.Clear
        $c0ffc0 GD.ColorRGB# 30.0e radius f! 30  wheel 
        $ffffc0 GD.ColorRGB# 48.0e radius f! 20  wheel 
        $ffc0c0 GD.ColorRGB# 110.0e radius f! 13 wheel 
        $c0c0ff GD.ColorRGB# 230.0e radius f! 52 wheel 
        GD.swap 1 time +!
    again
;
