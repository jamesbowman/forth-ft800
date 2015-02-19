: vx ( F: x y -- )
    fswap 16.0e f* f>s 16.0e f* f>s
    GD.Vertex2f
;

PI 2.0e f* fconstant 2PI
variable radius
variable time

: wheel ( points -- )
    0 swap
    GD.LINE_STRIP GD.Begin
    2PI dup s>f f/
    1+ 0 do
        dup s>f fover f*
        time @ s>f radius f@ f/ f+
        fdup fcos radius f@ f* 240e f+
        fswap fsin radius f@ f* 136e f+
        vx
        17 +
    loop
    fdrop
    drop
;

: x
    GD.init

    begin
        1 time +!
        GD.Clear
        $c0ffc0 GD.ColorRGB# 30.0e radius f! 30  wheel 
        $ffffc0 GD.ColorRGB# 48.0e radius f! 20  wheel 
        $ffc0c0 GD.ColorRGB# 110.0e radius f! 13 wheel 
        $c0c0ff GD.ColorRGB# 230.0e radius f! 52 wheel 
        GD.swap
    again
;
