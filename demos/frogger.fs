\
\ FROGGER - conversion from C to the FT800
\
\ This is an ANS Forth program:
\   Requiring the Core Extensions word set
\   Requiring the Exception word set
\   Requiring the Facility Extensions word set
\   Requiring the File Access word set
\

GD.init
s" demos/frogger_assets.fs" included
\ s" frogger_assets.fs" included

1 constant CONTROL_LEFT
2 constant CONTROL_RIGHT
4 constant CONTROL_UP
8 constant CONTROL_DOWN

: lookup \ a byte lookup table
    create
    does> + c@
;

\ Game variables
0 value t
0 value prevt
variable frogx          \ screen position
variable frogy          \ screen position
variable leaping        \ 0 means not leaping, 1-8 animates the leap
variable frogdir        \ while leaping, which direction is the leap?
variable frogface       \ which way is the frog facing, in furmans for CMD_ROTATE
variable dying          \ 0 means not dying, 1-64 animation counter
variable score 
variable hiscore        0 hiscore !
variable lives 
create done 5 allot

lookup homes 24 c, 72 c, 120 c, 168 c, 216 c,
variable time

: frog_start
  120 frogx !
  232 frogy !
  0 leaping !
  0 frogdir !
  0 frogface !
  0 dying !
  120 7 lshift time !
;

: level_start
    done 5 erase
;
 
: game_start
  4 lives !
  0 score !
  0 to t
;

: game_setup
  game_start
  level_start
  frog_start
;

: draw_score ( x y n -- )
    >r
    swap 8 * swap 8 *
    FONT_HANDLE 5 r>
    GD.cmd_number
;

: sprite  ( x y anim -- )
    >r
    swap 16 - 255 and swap 8 - 255 and
    over 224 > if
        r> GD.Cell
        swap 256 - 16 * swap 16 *
        GD.Vertex2f
    else
        SPRITES_HANDLE r> GD.Vertex2ii
    then
;

: r1  ( x y -- x' y ) \ move 16 pixels right, a sprite's width
    swap 16 + swap
;

: turtleanim  ( -- u ) \ the current turtle animation frame
    t 5 rshift 3 mod 50 +
;

: turtle3  ( x y -- ) \ draw three turtles
    turtleanim >r
    2dup r@ sprite
    r1 2dup r@ sprite
    r1 r> sprite
;

: turtle2  ( x y -- ) \ draw two turtles
    turtleanim >r
    2dup r@ sprite
    r1 r> sprite
;

: log ( length x y -- )
    2dup 86 sprite r1
    rot 0 do
        2dup 87 sprite r1
    loop
    88 sprite
;

: riverat  ( y tt -- )
    swap case
        120 of negate endof
        104 of endof
        88  of 5 4 */ endof
        72  of negate 2/ endof
        56  of 2/ endof
    endcase
;

: squarewave ( note amp -- )  \ continuous MIDI note, amp 0-255
    GD.REG_VOL_SOUND GD.c!
    8 lshift 1 or GD.REG_SOUND GD.!
    1 GD.REG_PLAY GD.c!
;

: sound
    dying @ if
        84 dying @ 2/ -
        100
    else
        leaping @ if
            leaping @ 2 and 0<> 12 and 60 +
            100
        else
            0 0
        then
    then
    squarewave
;

: silence
    0 GD.REG_VOL_SOUND GD.c!
;

: rotate_around ( x y a -- ) \ rotate sprite a degrees around (x,y)
    14 lshift >r
    swap 65536 * swap 65536 *
    GD.cmd_loadidentity
    2dup GD.cmd_translate
    r> GD.cmd_rotate
    swap negate swap negate GD.cmd_translate
    GD.cmd_setmatrix
;

: GD.touching
    GD.inputs.x -32768 <>
;

: letter  ( x spr -- x' ) \ one letter of "F R O G G E R"
    >r
    dup 50 SPRITES_HANDLE r> GD.Vertex2ii
    24 +
;

: game_over
    silence
    60 0 do
        GD.Clear
        \ Draw "F R O G G E R" using the sprites 90-94
        GD.BITMAPS GD.Begin
        160
        90 letter   \ F
        91 letter   \ R
        92 letter   \ O
        93 letter   \ G
        93 letter   \ G
        94 letter   \ E
        91 letter   \ R
        drop
        240 136 FONT_HANDLE GD.OPT_CENTER s" GAME OVER" GD.cmd_text
        i 59 = if
            240 200 FONT_HANDLE GD.OPT_CENTER s" PRESS TO PLAY" GD.cmd_text
        then

        GD.swap
    loop
    begin GD.getinputs GD.touching until
    begin GD.getinputs GD.touching 0= until
;

: padx ( i - x )
    3 - 48 * 480 +
;

: pady ( i -- y )
    3 - 48 * 272 +
;

: pads
    CONTROL_RIGHT GD.Tag
    2 padx 1 pady ARROW_HANDLE 0 GD.Vertex2ii

    24 24 3 rotate_around
    CONTROL_UP GD.Tag
    1 padx 0 pady ARROW_HANDLE 0 GD.Vertex2ii

    24 24 2 rotate_around
    CONTROL_LEFT GD.Tag
    0 padx 1 pady ARROW_HANDLE 0 GD.Vertex2ii

    24 24 1 rotate_around
    CONTROL_DOWN GD.Tag
    1 padx 2 pady ARROW_HANDLE 0 GD.Vertex2ii
;

lookup frog_anim
    2 c, 1 c, 0 c, 0 c, 2 c,
lookup die_anim
    31 c, 32 c, 33 c, 30 c,

: drawfrog \ draw the frog himself, or his death animation
    frogx @ frogy @
    dying @ 0= if
        leaping @ 2/ frog_anim
    else
        dying @ 16 / die_anim
    then
    sprite
;

: touching
    GD.inputs.ptag 2 =
;

: die
    1 dying !
;

: gameloop
    GD.getinputs
    GD.Clear
    1 GD.Tag
    SPRITES_HANDLE GD.BitmapHandle
    GD.SaveContext
    224 256 GD.ScissorSize
    GD.BITMAPS GD.Begin
    0 0 BACKGROUND_HANDLE 0 GD.Vertex2ii

    frogx @ 8 - GD.REG_TAG_X GD.!
    frogy @     GD.REG_TAG_Y GD.!

    2 GD.Tag
    GD.GREATER 0 GD.AlphaFunc  \ on road, don't tag transparent pixels

    \ Completed homes
    5 0 do
        done i + c@ if
            i homes 40 63 sprite
        then
    loop

    \ Yellow cars
    t negate        216 3 sprite
    t negate 128 +  216 3 sprite

    \ Dozers
    t               200 4 sprite
    t 50 +          200 4 sprite
    t 150 +         200 4 sprite

    \ Purple cars
    t negate        184 7 sprite
    t negate 75 +   184 7 sprite
    t negate 150 +  184 7 sprite

    \ Green and white racecars
    t 2*            168 8 sprite

    \ Trucks
    t negate 2/
    dup             152 5 sprite
    dup 16 +        152 6 sprite
    dup 100 +       152 5 sprite
        116 +       152 6 sprite

    GD.ALWAYS 0 GD.AlphaFunc   \ on river, tag transparent pixels

    \ Turtles
    256 0 do
        120 t riverat i + 120 turtle3
    64 +loop

    \ Short logs
    240 0 do
        1 104 t riverat i + 104 log
    80 +loop

    \ Long logs
    256 0 do
        5 88 t riverat i + 88 log
    128 +loop

    \ Turtles again, but slower
    250 0 do
        72 t riverat i + 72 turtle2
    50 +loop

    \ Top logs
    210 0 do
        2 56 t riverat i + 56 log
    70 +loop

    0 GD.TagMask
    frogface @ 8 8 rotate_around
    drawfrog

    t to prevt
    t 1+ to t

    time @ 1- 0 max dup time !  \ lose time, die if zero
    0= if
        die
    then

    \ Draw 'time remaining' by clearing a black rectangle
    GD.SaveContext
    72 248 GD.ScissorXY
    120 time @ 7 rshift - 8 GD.ScissorSize
    GD.Clear
    GD.RestoreContext

    1 GD.TagMask

    GD.RestoreContext
    GD.SaveContext
    GD.BITMAPS GD.Begin
  
    pads

    GD.RestoreContext

    255 85 0 GD.ColorRGB
    3 1 score @ draw_score
    11 1 hiscore @ draw_score

    255 255 255 GD.ColorRGB
    lives @ 0 ?do
        i 8 * 240 LIFE_HANDLE 0 GD.Vertex2ii
    loop
    depth throw

    GD.swap

    GD.touching GD.inputs.tag and >r
    r@ 0<>
    dying @ 0= and
    leaping @ 0= and if
        r@ frogdir !
        1 leaping !
        10 score +!
    else
        leaping @ 0<> if
            leaping @ 9 < if
                frogdir @ case
                CONTROL_LEFT    of 3 -2 frogx endof
                CONTROL_RIGHT   of 1  2 frogx endof
                CONTROL_UP      of 0 -2 frogy endof
                CONTROL_DOWN    of 2  2 frogy endof
                endcase
                +! frogface !
                1 leaping +!
            else
                0 leaping !
            then
        then
    then
    r> drop

    dying @ if
        1 dying +!
        dying @ 64 = if
            -1 lives +!
            lives @ 0= time @ 0= or if
                game_over
                game_start
                level_start
            then
            frog_start
        then
    else
        frogx @ 8 224 within 0= if
            die
        else
            \ GD.inputs.ptag cr .
            frogy @ 128 > if \ road section
                touching if
                    die
                then
            else
                frogy @ 40 > if \ river section
                    leaping @ 0= if
                        touching if
                            \ move frog according to lane speed
                            frogy @ t riverat
                            frogy @ prevt riverat
                            - frogx +!
                        else
                            die
                        then
                    then
                else \ riverbank
                    false
                    5 0 do
                        done i + c@ 0=
                        i homes frogx @ - abs 4 < and if
                            1 done i + c!
                            10 score +!
                            1+
                        then
                    loop
                    if
                        true 5 0 do
                            done i + c@ and
                        loop
                        if
                            level_start
                        else
                            frog_start
                        then
                    else
                        die
                    then
                then
            then
        then
    then

    sound
    score @ hiscore @ max hiscore !
;

: frogger
    GD.init
    GD.calibrate

    game_setup
    begin
        gameloop
    again
;
