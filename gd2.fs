\ GD library: FT800 Forth interface
\
\ Assumes a 32-bit ANS Forth plus the following:
\
\ LOCALWORDS PUBLICWORDS DONEWORDS
\                       words in LOCAL section are only
\                       visible between PUBLICWORDS and
\                       DONEWORDS.
\                       If your Forth lacks vocabularies,
\                       ignore them.
\
\ gd2-spi-init  ( -- )  initialize SPI and GD select signal
\ gd2-sel               assert the GD2 SPI select signal
\ gd2-unsel             deassert the GD2 SPI select signal
\ spi>      ( x -- )    send a byte to SPI
\ >spi      ( -- x )    receive a byte from SPI
\ uw@       ( a -- x )  unsigned 16-bit fetch
\ w@        ( a -- n )  signed 16-bit fetch
\
\
\ This is an ANS Forth program:
\   Requiring the Core Extensions word set
\   Requiring the Facility Extensions word set
\
\  Environmental dependency on 32 bit arithmetic.

\ Board configuration
\ -------------------
\
\ Depending on how the FT800 is hooked up, there may be
\ some custom initialization required. This initialization
\ includes:
\   * crystal/no-crystal operation
\   * screen rotation
\   * RGB pin swizzle
\   * LCD custom resolution/timing selection
\
\ These options are selected by a custom initialization
\ word, which is called by GD.init
\
\ This word calls GD.crystal or GD.nocrystal, then
\ writes any registers needed for by the configuration,
\ finally it should set up the LCD panel, using one of
\ the predefined panel setup words.
\
\ The default word is for Gameduino 2, and looks like:
\
\ : gameduino2
\     GD.nocrystal
\     3 GD.REG_SWIZZLE GD.c!
\     1 GD.REG_ROTATE  GD.c!
\     GD.480x272
\ ;
\
\ For other FT800/801 boards (e.g. FTDI's modules) then the hardware
\ defaults are fine, so a simpler word can be used:
\
\ : ftdi-eval
\     GD.crystal
\     GD.480x272
\ ;
\
\ To set the custom word, give its xt to GD.setcustom *before*
\ calling GD.init
\
\ ' ftdi-eval GD.setcustom
\

variable model      \ set to 0 for FT800, 4 for FT81x

\ FT800 is an SPI peripheral controlled by reads and writes
\ into its internal 24-bit address space.

\ Hardware registers
\ The main difference between the FT800 and FT810 architecture
\ is the register addresses.  Word gdconst hides this.

: gdconst ( ft810-addr ft800-addr -- )
    create  , ,
    does>   model @ + @
;

hex

302008 102408 gdconst GD.REG_CLOCK
302100 1024ec gdconst GD.REG_CMD_DL
3020f8 1024e4 gdconst GD.REG_CMD_READ
3020fc 1024e8 gdconst GD.REG_CMD_WRITE
302020 10241c gdconst GD.REG_CPURESET
302068 102464 gdconst GD.REG_CSPREAD
302060 10245c gdconst GD.REG_DITHER
302054 102450 gdconst GD.REG_DLSWAP
302004 102404 gdconst GD.REG_FRAMES
30200c 10240c gdconst GD.REG_FREQUENCY
302094 102490 gdconst GD.REG_GPIO
302090 10248c gdconst GD.REG_GPIO_DIR
30202c 102428 gdconst GD.REG_HCYCLE
302030 10242c gdconst GD.REG_HOFFSET
302034 102430 gdconst GD.REG_HSIZE
302038 102434 gdconst GD.REG_HSYNC0
30203c 102438 gdconst GD.REG_HSYNC1
302000 102400 gdconst GD.REG_ID
3020ac 10249c gdconst GD.REG_INT_EN
3020a8 102498 gdconst GD.REG_INT_FLAGS
3020b0 1024a0 gdconst GD.REG_INT_MASK
3020d8 1024c8 gdconst GD.REG_MACRO_0
3020dc 1024cc gdconst GD.REG_MACRO_1
30205c 102458 gdconst GD.REG_OUTBITS
302070 10246c gdconst GD.REG_PCLK
30206c 102468 gdconst GD.REG_PCLK_POL
30208c 102488 gdconst GD.REG_PLAY
3020c4 1024b4 gdconst GD.REG_PLAYBACK_FORMAT
3020c0 1024b0 gdconst GD.REG_PLAYBACK_FREQ
3020b8 1024a8 gdconst GD.REG_PLAYBACK_LENGTH
3020c8 1024b8 gdconst GD.REG_PLAYBACK_LOOP
3020cc 1024bc gdconst GD.REG_PLAYBACK_PLAY
3020bc 1024ac gdconst GD.REG_PLAYBACK_READPTR
3020b4 1024a4 gdconst GD.REG_PLAYBACK_START
3020d4 1024c4 gdconst GD.REG_PWM_DUTY
3020d0 1024c0 gdconst GD.REG_PWM_HZ
302058 102454 gdconst GD.REG_ROTATE
302088 102484 gdconst GD.REG_SOUND
302064 102460 gdconst GD.REG_SWIZZLE
30207c 102478 gdconst GD.REG_TAG
302074 102470 gdconst GD.REG_TAG_X
302078 102474 gdconst GD.REG_TAG_Y
302108 1024f4 gdconst GD.REG_TOUCH_ADC_MODE
30210c 1024f8 gdconst GD.REG_TOUCH_CHARGE
30218c 102574 gdconst GD.REG_TOUCH_DIRECT_XY
302190 102578 gdconst GD.REG_TOUCH_DIRECT_Z1Z2
302104 1024f0 gdconst GD.REG_TOUCH_MODE
302114 102500 gdconst GD.REG_TOUCH_OVERSAMPLE
30211c 102508 gdconst GD.REG_TOUCH_RAW_XY
302120 10250c gdconst GD.REG_TOUCH_RZ
302118 102504 gdconst GD.REG_TOUCH_RZTHRESH
302124 102510 gdconst GD.REG_TOUCH_SCREEN_XY
302110 1024fc gdconst GD.REG_TOUCH_SETTLE
30212c 102518 gdconst GD.REG_TOUCH_TAG
302128 102514 gdconst GD.REG_TOUCH_TAG_XY
302150 10251c gdconst GD.REG_TOUCH_TRANSFORM_A
309000 109000 gdconst GD.REG_TRACKER
302180 10256c gdconst GD.REG_TRIM
302040 10243c gdconst GD.REG_VCYCLE
302044 102440 gdconst GD.REG_VOFFSET
302080 10247c gdconst GD.REG_VOL_PB
302084 102480 gdconst GD.REG_VOL_SOUND
302048 102444 gdconst GD.REG_VSIZE
30204c 102448 gdconst GD.REG_VSYNC0
302050 10244c gdconst GD.REG_VSYNC1

308000 108000 gdconst GD.RAM_CMD
300000 100000 gdconst GD.RAM_DL

102000 constant GD.RAM_PAL              \ Only applies to FT800

\ 00000001 constant GD.CTOUCH_MODE_COMPATIBILITY
\ 00000000 constant GD.CTOUCH_MODE_EXTENDED

001024f4 constant GD.REG_CTOUCH_EXTENDED
00102510 constant GD.REG_CTOUCH_TOUCH0_XY
00102508 constant GD.REG_CTOUCH_TOUCH1_XY
00102574 constant GD.REG_CTOUCH_TOUCH2_XY
00102578 constant GD.REG_CTOUCH_TOUCH3_XY
00102538 constant GD.REG_CTOUCH_TOUCH4_X
0010250c constant GD.REG_CTOUCH_TOUCH4_Y

\ Graphics definitions

00000001 constant GD.BITMAPS
00000002 constant GD.POINTS
00000003 constant GD.LINES
00000004 constant GD.LINE_STRIP
00000005 constant GD.EDGE_STRIP_R
00000006 constant GD.EDGE_STRIP_L
00000007 constant GD.EDGE_STRIP_A
00000008 constant GD.EDGE_STRIP_B
00000009 constant GD.RECTS

00000000 constant GD.NEVER
00000001 constant GD.LESS
00000002 constant GD.LEQUAL
00000003 constant GD.GREATER
00000004 constant GD.GEQUAL
00000005 constant GD.EQUAL
00000006 constant GD.NOTEQUAL
00000007 constant GD.ALWAYS

00000000 constant GD.NEAREST
00000001 constant GD.BILINEAR

00000000 constant GD.BORDER
00000001 constant GD.REPEAT

00000000 constant GD.ARGB1555
00000001 constant GD.L1
00000002 constant GD.L4
00000003 constant GD.L8
00000004 constant GD.RGB332
00000005 constant GD.ARGB2
00000006 constant GD.ARGB4
00000007 constant GD.RGB565
00000008 constant GD.PALETTED
00000009 constant GD.TEXT8X8
0000000a constant GD.TEXTVGA
0000000b constant GD.BARGRAPH

00000002 constant GD.SRC_ALPHA
00000003 constant GD.DST_ALPHA
00000004 constant GD.ONE_MINUS_SRC_ALPHA
00000005 constant GD.ONE_MINUS_DST_ALPHA

00000000 constant GD.ADC_SINGLE_ENDED
00000001 constant GD.ADC_DIFFERENTIAL

\ 00000000 constant GD.DLSWAP_DONE
\ 00000001 constant GD.DLSWAP_LINE
\ 00000002 constant GD.DLSWAP_FRAME

\ 00000020 constant GD.INT_CMDEMPTY
\ 00000040 constant GD.INT_CMDFLAG
\ 00000080 constant GD.INT_CONVCOMPLETE
\ 00000010 constant GD.INT_PLAYBACK
\ 00000008 constant GD.INT_SOUND
\ 00000001 constant GD.INT_SWAP
\ 00000004 constant GD.INT_TAG
\ 00000002 constant GD.INT_TOUCH

00000001 constant GD.KEEP
00000002 constant GD.REPLACE
00000003 constant GD.INCR
00000004 constant GD.DECR
00000005 constant GD.INVERT

\ System register values
00000000 constant GD.LINEAR_SAMPLES
00000001 constant GD.ULAW_SAMPLES
00000002 constant GD.ADPCM_SAMPLES

\ Options for commands
0600 constant GD.OPT_CENTER
0200 constant GD.OPT_CENTERX
0400 constant GD.OPT_CENTERY
0100 constant GD.OPT_FLAT
0001 constant GD.OPT_MONO
1000 constant GD.OPT_NOBACK
0002 constant GD.OPT_NODL
c000 constant GD.OPT_NOHANDS
4000 constant GD.OPT_NOHM
4000 constant GD.OPT_NOPOINTER
8000 constant GD.OPT_NOSECS
2000 constant GD.OPT_NOTICKS
0800 constant GD.OPT_RIGHTX
0100 constant GD.OPT_SIGNED

\ 'instrument' argument to GD.play

00 constant GD.SILENCE
01 constant GD.SQUAREWAVE
02 constant GD.SINEWAVE
03 constant GD.SAWTOOTH
04 constant GD.TRIANGLE
05 constant GD.BEEPING
06 constant GD.ALARM
07 constant GD.WARBLE
08 constant GD.CAROUSEL
40 constant GD.HARP
41 constant GD.XYLOPHONE
42 constant GD.TUBA
43 constant GD.GLOCKENSPIEL
44 constant GD.ORGAN
45 constant GD.TRUMPET
46 constant GD.PIANO
47 constant GD.CHIMES
48 constant GD.MUSICBOX
49 constant GD.BELL
50 constant GD.CLICK
51 constant GD.SWITCH
52 constant GD.COWBELL
53 constant GD.NOTCH
54 constant GD.HIHAT
55 constant GD.KICKDRUM
56 constant GD.POP
57 constant GD.CLACK
58 constant GD.CHACK
60 constant GD.MUTE
61 constant GD.UNMUTE
: GD.PIPS 0f + ;

000ffffc constant GD.ROM_FONTROOT

00000000 constant GD.TOUCHMODE_OFF
00000001 constant GD.TOUCHMODE_ONESHOT
00000002 constant GD.TOUCHMODE_FRAME
00000003 constant GD.TOUCHMODE_CONTINUOUS

decimal

\ #######   INTERFACE   #######################################

LOCALWORDS

: GD.a  ( a -- ) \ start SPI read transaction, 24-bit address a
    gd2-sel
    dup 16 rshift >spi
    dup 8 rshift >spi
    dup >spi
    >spi      \ dummy
;

: GD.wa  ( a -- ) \ SPI write transaction, 24-bit address a
    gd2-sel
    dup 16 rshift 128 or >spi
    dup 8 rshift >spi
    >spi
;

\ Low-level FT800 memory access words
: GD.c@ ( addr -- x )
    GD.a
    spi>
    gd2-unsel
;

: GD.@ ( addr -- x )
    GD.a
    spi>
    spi> 8 lshift or
    spi> 16 lshift or
    spi> 24 lshift or
    gd2-unsel
;

: GD.move   ( caddr u a -- ) \ copy u bytes to caddr from GD memory a
    GD.a
    over + swap do
        spi> i c!
    loop
    gd2-unsel
;

: GD.c! ( addr -- x )
    GD.wa
    >spi
    gd2-unsel
;

: GD.! ( addr -- x )
    GD.wa
    4 0 do
        dup >spi
        8 rshift
    loop
    drop
    gd2-unsel
;

: hostcmd ( u -- )
    gd2-sel
        >spi
    00 >spi
    00 >spi
    gd2-unsel
    60 ms
;

: measureF ( -- u ) \ measure FT800's actual clock frequency
    1 ms
    GD.REG_CLOCK GD.@
    10 ms
    GD.REG_CLOCK GD.@
    swap - 100 *
;

47040000 constant LOW_FREQ_BOUND

: tune ( -- ) \ adjust the clock trim to get close to 48 MHz
    0 \ keep last-measured frequency on the stack
    32 0 do
        i GD.REG_TRIM GD.c!
        drop measureF dup LOW_FREQ_BOUND > if
            leave
        then
    loop
    GD.REG_FREQUENCY GD.!
;


variable wp             \ write pointer 0-4095
variable room           \ how much space is in the command FIFO
create inputs 18 allot  \ sampled touch inputs

: mod4K
    4095 and
;

: getspace  ( -- u )    \ u is the space in the command FIFO
    4092
    wp @ GD.REG_CMD_READ GD.@
    dup 3 and 0<> 257 and throw
    -
    mod4K -
;

: gostream
    GD.RAM_CMD wp @ mod4K +
    GD.wa
;

: unstream
    gd2-unsel
    wp @ GD.REG_CMD_WRITE GD.!
;

: >gd       ( x -- )    \ write x to the command stream
    room  @
    begin
        dup 0=
    while
        drop
        unstream
        getspace
        gostream
    repeat
    4 - room  !
    dup             >spi
    dup 8 rshift    >spi
    dup 16 rshift   >spi
    24 rshift       >spi
    4 wp +!
;

: stream
    getspace room  !
    gostream
;

\ Serialization helper words, mostly named
\ after the type that they serialize:
\ h     16-bit
\ i     32-bit

: cmd   ( x -- )
    -256 or >gd
;

: hh    ( h0 h1 -- )
    16 lshift swap $ffff and or >gd
;

: hhhh ( h0 h1 h2 h3 -- )
    2swap hh hh
;

: hhhhhh ( h0 h1 h2 h3 h4 h5 -- )
    2>r hhhh
    2r> hh
;

: hhhhhhhh ( h0 h1 h2 h3 h4 h5 h6 h7 -- )
    2>r 2>r hhhh
    2r> 2r> hhhh
;

: ii   ( x0 x1 -- )
    swap >gd >gd
;

: iii   ( x0 x1 x2 -- )
    >r ii r> >gd
;

\ s>gd appends a string to the command buffer, appends a
\ zero byte, and pads to the next 32-bit boundary.  It
\ iterates through the string, ORing the characters into the
\ 32-bit accumulator on top-of-stack. Every fourth character,
\ it sends the word to the hardware, and clears the
\ accumulator. After the loop, it flushes the partially-filled
\ accumulator to the hardware.

: s>gd   ( caddr u -- ) \ send string to the command buffer
     0 swap 0           ( caddr 0 u 0 )
     ?do
                        ( caddr u32 )
        over i + c@     ( caddr u32 byte )
        i 3 and dup >r
        3 lshift lshift or
        r> 3 = if
            >gd
            0
        then
    loop
    >gd                 ( caddr )
    drop
;

variable custom

PUBLICWORDS

\ #######   DRAWING COMMANDS   ################################

: GD.VERTEX2F
    1 30 lshift
\ y
    swap 32767 and
    or
\ x
    swap 32767 and
    15 lshift
    or
    >gd
;
: GD.VERTEX2II
    2 30 lshift
\ cell
    swap 127 and
    or
\ handle
    swap 31 and
    7 lshift
    or
\ y
    swap 511 and
    12 lshift
    or
\ x
    swap 511 and
    21 lshift
    or
    >gd
;
: GD.BITMAPSOURCE
    1 24 lshift
\ addr
    swap 1048575 and
    or
    >gd
;
: GD.CLEARCOLORRGB
    2 24 lshift
\ blue
    swap 255 and
    or
\ green
    swap 255 and
    8 lshift
    or
\ red
    swap 255 and
    16 lshift
    or
    >gd
;
: GD.TAG
    3 24 lshift
\ s
    swap 255 and
    or
    >gd
;
: GD.COLORRGB
    4 24 lshift
\ blue
    swap 255 and
    or
\ green
    swap 255 and
    8 lshift
    or
\ red
    swap 255 and
    16 lshift
    or
    >gd
;
: GD.BITMAPHANDLE
    5 24 lshift
\ handle
    swap 31 and
    or
    >gd
;
: GD.CELL
    6 24 lshift
\ cell
    swap 127 and
    or
    >gd
;
: GD.BITMAPLAYOUT
    7 24 lshift
\ height
    swap 511 and
    or
\ linestride
    swap 1023 and
    9 lshift
    or
\ format
    swap 31 and
    19 lshift
    or
    >gd
;
: GD.BITMAPSIZE
    8 24 lshift
\ height
    swap 511 and
    or
\ width
    swap 511 and
    9 lshift
    or
\ wrapy
    swap 1 and
    18 lshift
    or
\ wrapx
    swap 1 and
    19 lshift
    or
\ filter
    swap 1 and
    20 lshift
    or
    >gd
;
: GD.ALPHAFUNC
    9 24 lshift
\ ref
    swap 255 and
    or
\ func
    swap 7 and
    8 lshift
    or
    >gd
;
: GD.STENCILFUNC
    10 24 lshift
\ mask
    swap 255 and
    or
\ ref
    swap 255 and
    8 lshift
    or
\ func
    swap 7 and
    16 lshift
    or
    >gd
;
: GD.BLENDFUNC
    11 24 lshift
\ dst
    swap 7 and
    or
\ src
    swap 7 and
    3 lshift
    or
    >gd
;
: GD.STENCILOP
    12 24 lshift
\ spass
    swap 7 and
    or
\ sfail
    swap 7 and
    3 lshift
    or
    >gd
;
: GD.POINTSIZE
    13 24 lshift
\ size
    swap 8191 and
    or
    >gd
;
: GD.LINEWIDTH
    14 24 lshift
\ width
    swap 4095 and
    or
    >gd
;
: GD.CLEARCOLORA
    15 24 lshift
\ alpha
    swap 255 and
    or
    >gd
;
: GD.COLORA
    16 24 lshift
\ alpha
    swap 255 and
    or
    >gd
;
: GD.CLEARSTENCIL
    17 24 lshift
\ s
    swap 255 and
    or
    >gd
;
: GD.CLEARTAG
    18 24 lshift
\ s
    swap 255 and
    or
    >gd
;
: GD.STENCILMASK
    19 24 lshift
\ mask
    swap 255 and
    or
    >gd
;
: GD.TAGMASK
    20 24 lshift
\ mask
    swap 1 and
    or
    >gd
;
: GD.BITMAPTRANSFORMA
    21 24 lshift
\ a
    swap 131071 and
    or
    >gd
;
: GD.BITMAPTRANSFORMB
    22 24 lshift
\ b
    swap 131071 and
    or
    >gd
;
: GD.BITMAPTRANSFORMC
    23 24 lshift
\ c
    swap 16777215 and
    or
    >gd
;
: GD.BITMAPTRANSFORMD
    24 24 lshift
\ d
    swap 131071 and
    or
    >gd
;
: GD.BITMAPTRANSFORME
    25 24 lshift
\ e
    swap 131071 and
    or
    >gd
;
: GD.BITMAPTRANSFORMF
    26 24 lshift
\ f
    swap 16777215 and
    or
    >gd
;
: GD.SCISSORXY
    27 24 lshift
\ y
    swap 511 and
    or
\ x
    swap 511 and
    9 lshift
    or
    >gd
;
: GD.SCISSORSIZE
    28 24 lshift
\ height
    swap 1023 and
    or
\ width
    swap 1023 and
    10 lshift
    or
    >gd
;
: GD.CALL
    29 24 lshift
\ dest
    swap 65535 and
    or
    >gd
;
: GD.JUMP
    30 24 lshift
\ dest
    swap 65535 and
    or
    >gd
;
: GD.BEGIN
    31 24 lshift
\ prim
    swap 15 and
    or
    >gd
;
: GD.COLORMASK
    32 24 lshift
\ a
    swap 1 and
    or
\ b
    swap 1 and
    1 lshift
    or
\ g
    swap 1 and
    2 lshift
    or
\ r
    swap 1 and
    3 lshift
    or
    >gd
;
: GD.CLEARtsc
    38 24 lshift
\ t
    swap 1 and
    or
\ s
    swap 1 and
    1 lshift
    or
\ c
    swap 1 and
    2 lshift
    or
    >gd
;
: GD.CLEAR
    38 24 lshift
    7 or
    >gd
;
: GD.END
    33 24 lshift
    >gd
;
: GD.SAVECONTEXT
    34 24 lshift
    >gd
;
: GD.RESTORECONTEXT
    35 24 lshift
    >gd
;
: GD.RETURN
    36 24 lshift
    >gd
;
: GD.MACRO
    37 24 lshift
\ m
    swap 1 and
    or
    >gd
;
: GD.DISPLAY
    0
    >gd
;

: GD.COLORRGB#  ( x -- )
    16777215 and
    4 24 lshift
    or
    >gd
;

: GD.CLEARCOLORRGB#  ( x -- )
    16777215 and
    2 24 lshift
    or
    >gd
;

\ #######   COPROCESSOR COMMANDS   ############################
\
\ These are higher-level FT800 commands, for drawing widgets etc.

hex

: GD.cmd_append  ( ptr num -- )
    01e cmd
    ii
;

: GD.cmd_bgcolor  ( c -- )
    009 cmd
    >gd
;

: GD.cmd_button  ( x y w h font options s -- )
    00d cmd
    2>r hhhhhh
    2r> s>gd
;

: GD.cmd_calibrate  ( -- )
    015 cmd
    0 >gd
;

: GD.cmd_clock  ( x y r options h m s ms -- )
    014 cmd
    hhhhhhhh
;

: GD.cmd_coldstart  ( -- )
    032 cmd
;

: GD.cmd_dial  ( x y r options val -- )
    02d cmd
    >r hhhh r> >gd
;

: GD.cmd_dlstart
    000 cmd
;

: GD.cmd_fgcolor  ( c -- )
    00a cmd
    >gd
;

: GD.cmd_gauge  ( x y r options major minor val range -- )
    013 cmd
    hhhhhhhh
;

: GD.cmd_getmatrix  ( -- )
    033 cmd
;

\ : GD.cmd_getprops  ( ptr w h -- )
\     025 cmd
\ ;

\ : GD.cmd_getptr  ( -- )
\     023 cmd
\ ;

: GD.cmd_gradcolor  ( c -- )
    034 cmd
    >gd
;

: GD.cmd_gradient  ( x0 y0 rgb0 x1 y1 rgb1 -- )
    00b cmd
    >r 2>r
    >r
    hh
    r> >gd
    2r> hh
    r> >gd
;

: GD.cmd_inflate  ( ptr -- )
    022 cmd
    >gd
;

: GD.cmd_interrupt  ( ms -- )
    002 cmd
    >gd
;

: GD.cmd_keys  ( x y w h font options s -- )
    00e cmd
    2>r
    hhhhhh
    2r> s>gd
;

: GD.cmd_loadidentity  ( -- )
    026 cmd
;

: GD.cmd_loadimage  ( ptr options -- )
    024 cmd
    ii
;

: GD.cmd_memcpy  ( dest src num -- )
    01d cmd
    iii
;

: GD.cmd_memset  ( ptr value num -- )
    01b cmd
    iii
;

: GD.cmd_memzero  ( ptr num -- )
    01c cmd
    ii
;

: GD.cmd_memwrite  ( ptr num -- )
    01a cmd
    ii
;

: GD.cmd_regwrite  ( ptr val -- )
    01a cmd
    swap >gd
    4 >gd
    >gd
;

: GD.cmd_number  ( x y font options n -- )
    02e cmd
    >r hhhh
    r> >gd
;

: GD.cmd_progress  ( x y w h options val range -- )
    00f cmd
    0 hhhhhhhh
;

\ : GD.cmd_regread  ( ptr -- )
\     $19 cmd
\ ;

: GD.cmd_rotate  ( a -- )
    029 cmd
    >gd
;

: GD.cmd_scale  ( sx sy -- )
    028 cmd
    ii
;

: GD.cmd_screensaver  ( -- )
    02f cmd
;

: GD.cmd_scrollbar  ( x y w h options val size range -- )
    011 cmd
    hhhhhhhh
;

: GD.cmd_setfont  ( font ptr -- )
    02b cmd
    ii
;

: GD.cmd_setmatrix  ( -- )
    02a cmd
;

: GD.cmd_sketch  ( x y w h ptr format -- )
    030 cmd
    2>r
    hhhh
    2r> ii
;

: GD.cmd_slider  ( x y w h options val range -- )
    010 cmd
    0 hhhhhhhh
;

: GD.cmd_snapshot  ( ptr -- )
    01f cmd
    >gd
;

: GD.cmd_spinner  ( x y style scale -- )
    016 cmd
    hhhh
;

: GD.cmd_stop  ( -- )
    017 cmd
;

: GD.cmd_text  ( x y font options s -- )
    00c cmd
    2>r hhhh
    2r>
    s>gd
;

: GD.cmd_toggle  ( x y w font options state s -- )
    012 cmd
    2>r
    hhhhhh
    2r> s>gd
;

: GD.cmd_track  ( x y w h tag -- )
    02c cmd
    0 hhhhhh
;

: GD.cmd_translate  ( tx ty -- )
    027 cmd
    ii
;

decimal

\ #######   TOP-LEVEL COMMANDS   ##############################

: GD.flush  \ Sends all pending commands to the FT800
    unstream
    stream
;

: GD.finish  \ Wait for all pending commands to complete
    unstream
    begin
        getspace 4092 =
    until
    stream
;

: GD.calibrate  \ run the FT800's interactive calibration procedure
    GD.Clear
    unstream
    GD.REG_HSIZE GD.@ 2/
    GD.REG_VSIZE GD.@ 3 10 */
    30
    stream
    GD.OPT_CENTERX s" please tap on the dot" GD.cmd_text
    GD.cmd_calibrate
    GD.finish
    GD.cmd_dlstart
;

: GD.swap  \ swaps the working and displayed images
    GD.display
    01 cmd             \ cmd_swap
    GD.flush
    GD.cmd_dlstart
;

: GD.getinputs  \ collects touch input
    GD.finish
    unstream
    inputs      4  GD.REG_TRACKER GD.move
    inputs 4 +  13 GD.REG_TOUCH_RZ GD.move
    inputs 17 + 1  GD.REG_TAG GD.move
    stream
;

\ accessors for the values collected by GD.getinputs
: GD.inputs.track_tag   inputs 0 + uw@ ;
: GD.inputs.track_val   inputs 2 + uw@ ;
: GD.inputs.rz          inputs 4 + uw@ ;
: GD.inputs.y           inputs 8 + w@ ;
: GD.inputs.x           inputs 10 + w@ ;
: GD.inputs.tag_y       inputs 12 + w@ ;
: GD.inputs.tag_x       inputs 14 + w@ ;
: GD.inputs.tag         inputs 16 + c@ ;
: GD.inputs.ptag        inputs 17 + c@ ;

hex
: GD.init  \ initialize the device
    gd2-spi-init
    gd2-unsel

    000 hostcmd         \ ACTIVE

    custom @ execute
;

: common-init
    \ cr
    \ gd2-sel
    \     10 spix hex2.
    \     24 spix hex2.
    \     00 spix hex2.
    \     0 spix hex2.
    \     0 spix hex2.
    \     0 spix hex2.
    \ gd2-unsel

    0c0001 GD.c@ 4 rshift cells model !

    080 GD.REG_GPIO_DIR     GD.c!
    080 GD.REG_GPIO         GD.c!

    0 wp !
    stream

    GD.cmd_dlstart
    GD.Clear

    GD.swap
;

: GD.nocrystal ( -- ) \ initialize the FT800 for no-crystal
    062 hostcmd     \ CLK48M used for no-crystal parts like Gameduino2
    068 hostcmd     \ CORERST
    tune
    common-init
;

: GD.crystal ( -- ) \ initialize the FT800 for external crystal
    044 hostcmd     \ CLKEXT, use external crystal
    068 hostcmd     \ CORERST
    common-init
;

: GD.setcustom ( xt -- ) \ Set the custom initialization word
    custom !
;

decimal

: GD.c  ( x -- ) \ send x to the command buffer
    >gd
;

: GD.s  ( caddr u -- ) \ send buffer to the command buffer
    s>gd
;

: GD.suspend  \ suspend the current SPI stream transaction
    unstream
;

: GD.resume  \ resume the SPI stream transaction
    stream
;

: GD.supply ( caddr u -- )    \ write blk to the command stream
    aligned
    begin
        dup
    while
        room @
        over min
        ?dup
        if
            >r
            r@ negate room +!
            over r@ blk>spi
            r@ wp +!
            r> /string
        else
            unstream
            stream
        then
    repeat
    2drop
;

: GD.play  ( instrument note -- ) \ play a sound
    unstream
    8 lshift or
    GD.REG_SOUND GD.!
    1 GD.REG_PLAY GD.!
    stream
;

: GD.wh ( -- w h ) \ size of the current screen
    unstream
    GD.REG_HSIZE GD.@ GD.REG_VSIZE GD.@
    GD.REG_ROTATE GD.@ 2 and if     \ deal with landscape/portrait
        swap
    then
    stream
;

\ FT800 memory access
: GD.c!     unstream GD.c! stream ;
: GD.c@     unstream GD.c@ stream ;
: GD.@      unstream GD.@ stream ;
: GD.!      unstream GD.! stream ;
: GD.move   unstream GD.move stream ;

DONEWORDS

: GD.480x272
    1 GD.REG_PCLK_POL       GD.c!
    5 GD.REG_PCLK           GD.c!
;

: gameduino2
    GD.nocrystal
    3 GD.REG_SWIZZLE GD.c!
    1 GD.REG_ROTATE  GD.c!
    GD.480x272
;

: ftdi-eval
    GD.crystal
    GD.480x272
;

' gameduino2 GD.setcustom
