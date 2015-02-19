\ Handheld: swapForth-specific handheld console driver.
\ This module replaces the regular 'emit' and 'key' hooks
\ that drive the FT800 as a portrait-orientation tty
\ emulator.
\ The soft-keyboard implementation uses the touchscreen.
\

: xyii ( x y i i -- )
    2swap
    479 swap - swap
    2swap
    GD.vertex2ii
;

: xy ( x y -- )
    0 0 xyii
;

create widths 128 allot
: width  ( c -- w )
    widths + c@
;

: loadfont ( u -- )
    16 - 148 *
    GD.ROM_FONTROOT GD.@ + >r

    \ GD.L8 480 272 GD.BitmapLayout
    \ GD.NEAREST GD.BORDER GD.BORDER 0 0 GD.BitmapSize

    r@ 144 + GD.@   GD.BitmapSource

    r@ 128 + GD.@
    r@ 132 + GD.@
    r@ 140 + GD.@   GD.BitmapLayout

    GD.BILINEAR GD.BORDER GD.BORDER
    r@ 140 + GD.@
    r@ 136 + GD.@   GD.BitmapSize

    r>
    128 0 do
        dup i + GD.c@
        widths i + c!
    loop
    drop
;

: button ( x y ch -- )
    \ GD.SRC_ALPHA GD.ONE_MINUS_SRC_ALPHA GD.BlendFunc
    >r
    r@ GD.Tag
    $202040 GD.ColorRGB#
    GD.RECTS GD.Begin
    2dup xy
    2dup 14 dup d+ xy
    \ GD.SRC_ALPHA 0 GD.BlendFunc
    $ffffff GD.ColorRGB#
    GD.BITMAPS GD.Begin
    14 +
    swap 16 r@ widths + c@ - 2/ + swap
    0 r> xyii
;

variable shift 0 shift !

: 1ch  ( u k. k. -- c ) \ from two shifted row strings, pick char u
    shift @ if
        2swap
    then
    2drop
    drop + c@
;

\ Alternative to s" for strings that contain a "
: s| [char] | parse postpone sliteral ; immediate

: row0 s" 1234567890"   s" !@#$%^&*()"   1ch ;
: row1 s" QWERTYUIOP"   s" `~[]\{}|  "   1ch ;
: row2 s" ASDFGHJKL"    s| _+-= :;"'|    1ch ;
: row3 s" ZXCVBNM"      s"  <>,./?"      1ch ;

: label
    bounds do
        2dup 0 i c@ xyii
        i c@ width 1+ 0 d+
    loop
    2drop
;

: keyboard
    GD.cmd_loadidentity
    $80000 $10000 GD.cmd_translate
    $4000 GD.cmd_rotate
    $-10000 $-80000 GD.cmd_translate
    GD.cmd_setmatrix

    64 GD.LineWidth
    10 0 do
        i 27 * 6 +
        330
        i row0 
        button
    loop
    10 0 do
        i 27 * 6 +
        360
        i row1 
        button
    loop
    9 0 do
        i 27 * 19 +
        390
        i row2 
        button
    loop
    7 0 do
        i 27 * 46 +
        420
        i row3 
        button
    loop

    $202040 GD.ColorRGB#

    GD.RECTS GD.Begin

    \ backspace
    8 GD.Tag
    236 420 xy
    266 434 xy

    \ numberwang
    128 GD.Tag
    6 450 xy
    45 464 xy

    \ space bar
    32 GD.Tag
    60 450 xy
    210 464 xy

    \ enter
    13 GD.Tag
    225 450 xy
    266 464 xy

    0 GD.TagMask
    $c0c060 GD.ColorRGB#
    GD.BITMAPS GD.Begin
    shift @ if
       13 464 s" abc"
    else
       11 464 s" sym"
    then label
    243 434 s" <<" label
    228 464 s" enter" label
;

: redraw
    GD.Clear

    GD.cmd_loadidentity
    330 $10000 * $-08000 GD.cmd_translate
    $4000 GD.cmd_rotate
    GD.cmd_setmatrix
    \ 1 0 GD.BlendFunc
    GD.BITMAPS GD.Begin

    0 GD.Tag
    0 330 1 0 xyii
    GD.RestoreContext

    $ff8080 GD.ColorRGB#
    0 GD.Macro

    keyboard

    GD.swap
;

variable cursor

2 34 * 20 * constant SZ
68 constant 2W              \ Width doubled, line stride

: scroll
    SZ 1- cursor @ < if
        2W negate cursor +!
        0 2W SZ 2W - GD.cmd_memcpy
        SZ 2W - 2W GD.cmd_memzero
    then
;

action-of emit constant oldemit

: tty-emit  ( u -- )
    \ dup [ oldemit compile, ]
    case
        $08     of -2 cursor +! endof
        $0a     of 2W cursor +! endof
        $0d     of cursor @ 2W / 2W * cursor ! endof

                dup 31 > if
                    dup
                    cursor @ 2 GD.cmd_memwrite
                    $ff00 or GD.c
                    2 cursor +!
                then
    endcase
    scroll
    GD.REG_MACRO_0 4 GD.cmd_memwrite
    cursor @ 2W mod 2/ 8 *
    cursor @ 2W / 16 * 15 +
    17 221 xyii
    GD.flush
;

: at-xy
    60 * + 2*
    cursor !
;

: page
    0 cursor !
    0 SZ GD.cmd_memzero
;

: tty-init
    page

    1 GD.BitmapHandle
    0 GD.BitmapSource
    GD.TEXTVGA 2W 20 GD.BitmapLayout
    GD.NEAREST GD.BORDER GD.BORDER 330 272 GD.BitmapSize
    0 GD.BitmapHandle
;

: sense
    GD.getinputs
    GD.inputs.tag
;

: sound  ( u -- )
    GD.REG_SOUND 4 GD.cmd_memwrite GD.c
    GD.REG_PLAY  4 GD.cmd_memwrite 1 GD.c
    GD.flush
;

variable dirty dirty off

: tty-key
    dirty @ if
        tty-init 26 loadfont
        redraw
        0 tty-emit  \ update cursor pos
        dirty off
    then
    begin
        sense 0=
    until
    begin
        sense
        ?dup
    until
    dup 128 = if
        drop
        shift dup @ if
            off GD.NOTCH
        else
            on  GD.SWITCH
        then
        sound
        redraw
        recurse exit
    then
    dup bl = over 13 = or if
        shift off redraw
    then
    GD.CHACK sound
;

: calibrate
    0 if
        GD.calibrate
        GD.REG_TOUCH_TRANSFORM_A 24 bounds do
            i GD.@ . ." GD.c "
        4 +loop
    else
        GD.REG_TOUCH_TRANSFORM_A 24 GD.cmd_memwrite
        \ 41078 GD.c -378 GD.c -392457 GD.c 327 GD.c -39228 GD.c 18215789 GD.c
        442 GD.c 67773 GD.c 128238 GD.c -69606 GD.c -257 GD.c 17998943 GD.c
    then
;

: handheld
    GD.init
    calibrate

    tty-init
    26 loadfont

    redraw

    ['] tty-key is key
    ['] tty-emit is emit
    page
    .version cr
    cr
;

\ To make the system PERMANENTLY use the 
: cold
    cold
    handheld
;
