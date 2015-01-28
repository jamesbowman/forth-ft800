: snow
    GD.init

    GD.L8 480 272 GD.BitmapLayout
    GD.NEAREST GD.BORDER GD.BORDER 0 0 GD.BitmapSize

    0 $40000 GD.cmd_memwrite
    $10000 0 do
        random GD.c
    loop

    begin
        $40000 480 272 * - rr GD.BitmapSource
        GD.Clear
        GD.BITMAPS GD.Begin
        0 0 0 0 GD.Vertex2ii
        240 136 31 GD.OPT_CENTER s" snow" GD.cmd_text
        GD.swap
    again
;
