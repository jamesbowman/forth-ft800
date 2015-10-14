: helloworld
    GD.init

    begin
        $103000 GD.ClearColorRGB#
        GD.Clear
        GD.wh 2/ swap 2/ swap   \ middle of the screen
        31 GD.OPT_CENTER s" Hello world" GD.cmd_text
        GD.swap
    again
;
