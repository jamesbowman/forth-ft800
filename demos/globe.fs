\ http://www.jgiesen.de/elevaz/basics/
\ http://nssdc.gsfc.nasa.gov/planetary/planets/earthpage.html
\ http://www.stargazing.net/kepler/sun.html

\ include mcp7940m.fs
s" calencal.fs" included

480 constant width
240 constant height

65536 s>f fdup f*   fconstant FLT_2**32       

( calibration <-> NVSRAM                     JCB 17:55 02/15/15)

$44474653 constant SIGNATURE \ "SFGD" for SwapForth Gameduino

\ : calibrate
\     \ first word of SRAM is the signature
\     \ the rest is the 24-byte calibration block
\ 
\     $20 pad 28 mcp7940m@m
\     pad @ SIGNATURE <> if
\         SIGNATURE pad !
\         GD.calibrate
\         \ calibration -> pad
\         GD.finish GD.suspend
\         pad cell+ 24 GD.REG_TOUCH_TRANSFORM_A GD.move
\         GD.resume
\         \ pad -> NVRAM
\         pad $20 28 mcp7940m!m
\     else
\         GD.REG_TOUCH_TRANSFORM_A 24 GD.cmd_memwrite
\         pad cell+ 24 GD.supply
\     then
\ ;

create seetz 24 allot  seetz 24 erase
true seetz 4 + c!
true seetz 12 + c!
true seetz 20 + c!

JAN 1 2000 Fixed-from-Gregorian constant J2000

\ Represent JD using a 32.32 double
: mhmdy-jd  ( mm hh m d y -- jd. )
    Fixed-from-Gregorian
    J2000 - >r
    60 * + $40000000 1440 */ 2* 2*
    \ 1491308 +   \ move to half-past minute to avoid rounding
    r>
    $80000000. d-
;

: jd-mhmdy
    $80000000. d+
    >r 2 rshift 1440 $40000000 */
    60 /mod
    r> J2000 + Gregorian-from-Fixed
;

T{ 0 11 AUG 7 1997 mhmdy-jd d>f flt_2**32 f/ 100e f* f>s -> -87704 }T

\ compute the system time scaled for JD.
\ system time from us@ is total microseconds
\ JD is in days, so multiply by number of
\ seconds in a day

24 60 *         constant MINUTES-IN-DAY
24 60 * 60 *    constant SECONDS-IN-DAY

: jd@ ( -- jd. )
    us@
    $1000000
    1000000 SECONDS-IN-DAY um*
    d2/ d2/ d2/ d2/ d2/ d2/ d2/ d2/ drop
    m*/
;

\ ============================================================

variable L          \ Mean Longitude
variable g          \ Mean anomaly
variable lambda     \ ecliptic longitude
variable epsilon    \ obliquity of the ecliptic plane

: smod360
    360 mod
    dup 0< 360 and +
;

: dmod360
    smod360
;

: dscale
    10000000 m*/
;

: dadd
    flt_2**32 f* f>d
    postpone 2literal
    postpone d+
; immediate

: d360! ( d. fa -- )
    >r
    dmod360 
    d>f flt_2**32 f/ r> f!
;

: sunpos2 ( F: d -- RA declination )
\ 
\    L = 280.461 + 0.9856474 * d
\      = -583.99284 + 720  
\     (add multiples of 360 to bring in range 0 to 360)
\      = 136.00716

    2dup  9856474 dscale [ 280.461e ] dadd L d360!

\ 3. Find the Mean anomaly (g) of the Sun
\ 
\    g = 357.528 + 0.9856003 * d
\      = -506.88453 + 720
\      = 213.11547

    2dup  9856003 dscale [ 357.528e ] dadd g d360!

\ 4. Find the ecliptic longitude (lambda) of the sun
\ 
\    lambda = L + 1.915 * sin(g) + 0.020 * sin(2*g)
\           = 134.97925
\ 
\    (note that the sin(g) and sin(2*g) terms constitute an 
\     approximation to the 'equation of centre' for the orbit 
\     of the Sun)

    L f@
    g f@ degrees fsin 1.915e f* f+
    g f@ 2e f* degrees fsin 0.020e f* f+
    lambda f!

\ 
\    beta = 0 (by definition as the Sun's orbit defines the
\              ecliptic plane. This results in a simplification
\              of the formulas below)
\ 

     -0000004 dscale [  23.439e ] dadd epsilon d360!

\ 6. Find the Right Ascension (alpha) and Declination (delta) of
\    the Sun
\ 
\    Y = cos(epsilon) * sin(lambda)
\    X = cos(lambda)

    lambda f@ degrees fsin epsilon f@ degrees fcos f*
    lambda f@ degrees fcos              ( Y X )

\ 
\    a = arctan(Y/X)

    fover fover f/ fatan radians

\    If X < 0 then alpha = a + 180
\    If Y < 0 and X > 0 then alpha = a + 360
\    else alpha = a

    fswap f0< if
        180e f+ fswap fdrop
    else
        fswap f0< if
            360e f+
        then
    then

\ 
\    Y =  0.6489924
\    X = -0.7068507
\ 
\    a = -42.556485
\    alpha = -42.556485 + 180 = 137.44352 (degrees)

\    delta = arcsin(sin(epsilon)*sin(lambda))
\          = 16.342193 degrees

    epsilon f@ degrees fsin
    lambda f@ degrees fsin f*
    fasin radians
;

variable gst

: jdwhere2
    2dup $80000000. d+ drop  \ keep the day part
    >r

    2dup sunpos2        ( F: RA dec )
    \ [char] | emit .f [char] | emit
    fswap               ( F: dec RA )

    -9856474 dscale [   -100.74145133333332e ] dadd

                        ( F: dec RA )
    \ gst d360!  gst f@ f+
    flt_2**32 f* f>d d+
    r> 360 um* d-
    dmod360
    d>f flt_2**32 f/

    fswap
;

\ ============================================================


: dms.
    \ [char] ( emit fdup f. [char] ) emit space
    fdup f>s 3 .r ." °"
    fabs
    frac 3600e f* f>s
    60 /mod 2 u.r ." '"
    2 u.r space
;

: ew.
    fdup 180.0e f< if
        dms. [char] E emit
    else
        360.0e fswap f- dms. [char] W emit
    then
    space
;

0 365               2constant YEAR
0 1                 2constant DAY
DAY 1 24 m*/        2constant HOUR
DAY 1 24 60 * m*/   2constant MINUTE
DAY 1 24 60 * 60 * m*/   2constant SECOND


: onmap ( F: ra dec -- ) ( -- x y )
    fswap
    [ width s>f 360e f/ ] fliteral f* width 2/ s>f f+ width s>f fmod
    16.e f* f>s
    [ height 2/ negate s>f 90e f/ ] fliteral f* height 2/ s>f f+
    16.e f* f>s
;

T{ -180e 90e onmap -> 0 0 }T
\ T{ 0e 0e onmap -> 400 16 * 240 16 * }T

256 constant YLINE

: monthname ( x - caddr u )
    dup 1 13 within not throw
    1- 3 *
    S" JANFEBMARAPRMAYJUNJULAUGSEPOCTNOVDEC" DROP + 3
;

: statusline ( x -- x y font opt )
    width 800 */
    YLINE 30 GD.OPT_CENTER
;

: drawnow ( JD. -- )
    500 GD.LineWidth

    jd-mhmdy
    >r              160 statusline r> GD.cmd_number
    >r              400 statusline 2 or r> GD.cmd_number
    monthname 2>r   300 statusline 2r> GD.cmd_text
    >r              540 statusline 2 or r> GD.cmd_number
    >r              620 statusline 2 or r> GD.cmd_number
;

( Cities                                     JCB 11:56 02/15/15)

\ 40.70  -74.00 "New York"

: latlong
    fswap onmap
;

variable citylist 0 citylist !

: city ( F: lat long -- ) ( name tz -- )
    align here >r
    citylist @ ,
    fswap f> , f> ,
    ,
    dup c,
    bounds do
        i c@ c,
    loop
    r> citylist !
;

 0200 constant Africa/Cairo
 0000 constant Africa/Dakar
 0300 constant Africa/Dar_es_Salaam
 0200 constant Africa/Gaborone
 0200 constant Africa/Harare
 0200 constant Africa/Johannesburg
 0300 constant Africa/Khartoum
 0100 constant Africa/Lagos
 0300 constant Africa/Mogadishu
-0900 constant America/Anchorage
-0500 constant America/Bogota
-0300 constant America/Buenos_Aires
-0430 constant America/Caracas
-0600 constant America/Chicago
-0700 constant America/Denver
-0500 constant America/Havana
-0400 constant America/La_Paz
-0500 constant America/Lima
-0800 constant America/Los_Angeles
-0600 constant America/Mexico_City
-0500 constant America/Montreal
-0500 constant America/New_York
-0300 constant America/Recife
-0200 constant America/Sao_Paulo
-0800 constant America/Vancouver
 0700 constant Asia/Bangkok
 0530 constant Asia/Calcutta
 0530 constant Asia/Colombo
 0600 constant Asia/Dhaka
 0200 constant Asia/Istanbul
 0700 constant Asia/Jakarta
 0430 constant Asia/Kabul
 0800 constant Asia/Manila
 0300 constant Asia/Riyadh
 0900 constant Asia/Seoul
 0800 constant Asia/Shanghai
 0800 constant Asia/Singapore
 0330 constant Asia/Tehran
 0900 constant Asia/Tokyo
 0000 constant Atlantic/Reykjavik
 0930 constant Australia/Darwin
 0800 constant Australia/Perth
 1100 constant Australia/Sydney
 0100 constant Europe/Berlin
 0000 constant Europe/London
 0100 constant Europe/Madrid
 0400 constant Europe/Moscow
 1300 constant Pacific/Auckland
-0500 constant Pacific/Easter
-1000 constant Pacific/Honolulu

 61.22e -149.90e s" Anchorage"     America/Anchorage    city
-36.92e  174.78e s" Auckland"      Pacific/Auckland     city
 13.73e  100.50e s" Bangkok"       Asia/Bangkok         city
 52.53e   13.42e s" Berlin"        Europe/Berlin        city
  4.63e  -74.08e s" Bogota"        America/Bogota       city
-34.67e  -58.50e s" Buenos Aires"  America/Buenos_Aires city
 30.05e   31.25e s" Cairo"         Africa/Cairo         city
-33.93e   18.47e s" Cape Town"     Africa/Johannesburg  city
 10.50e  -66.92e s" Caracas"       America/Caracas      city
  6.92e   79.87e s" Colombo"       Asia/Colombo         city
 14.63e  -17.45e s" Dakar"         Africa/Dakar         city
 -6.85e   39.30e s" Dar es Salaam" Africa/Dar_es_Salaam city
-12.38e  130.73e s" Darwin"        Australia/Darwin     city
 28.67e   77.22e s" Delhi"         Asia/Calcutta        city
 39.73e -104.98e s" Denver"        America/Denver       city
 23.72e   90.37e s" Dhaka"         Asia/Dhaka           city
-27.12e -109.37e s" Easter Island" Pacific/Easter       city
-24.75e   25.92e s" Gaborone"      Africa/Gaborone      city
-17.83e   31.05e s" Harare"        Africa/Harare        city
 23.17e  -82.35e s" Havana"        America/Havana       city
 21.30e -157.85e s" Honolulu"      Pacific/Honolulu     city
 29.75e  -95.35e s" Houston"       America/Chicago      city
 41.03e   28.95e s" Istanbul"      Asia/Istanbul        city
 -6.13e  106.75e s" Jakarta"       Asia/Jakarta         city
 34.52e   69.18e s" Kabul"         Asia/Kabul           city
 15.55e   32.53e s" Khartoum"      Africa/Khartoum      city
-16.50e  -68.17e s" La Paz"        America/La_Paz       city
  6.45e    3.47e s" Lagos"         Africa/Lagos         city
-12.05e  -77.05e s" Lima"          America/Lima         city
 51.50e   -0.17e s" London"        Europe/London        city
 34.05e -118.23e s" Los Angeles"   America/Los_Angeles  city
 40.42e   -3.72e s" Madrid"        Europe/Madrid        city
 14.62e  120.97e s" Manila"        Asia/Manila          city
 19.40e  -99.15e s" Mexico City"   America/Mexico_City  city
 45.50e  -73.60e s" Montreal"      America/Montreal     city
 55.75e   37.70e s" Moscow"        Europe/Moscow        city
  2.03e   45.35e s" Muqdisho"      Africa/Mogadishu     city
 40.70e  -74.00e s" New York"	   America/New_York     city
-31.97e  115.82e s" Perth"         Australia/Perth      city
 -8.10e  -34.88e s" Recife"        America/Recife       city
 64.15e  -21.97e s" Reykjavik"     Atlantic/Reykjavik   city
 24.65e   46.77e s" Riyadh"        Asia/Riyadh          city
 37.53e  127.00e s" Seoul"         Asia/Seoul           city
 31.10e  121.37e s" Shanghai"      Asia/Shanghai        city
  1.28e  103.85e s" Singapore"     Asia/Singapore       city
-33.92e  151.17e s" Sydney"        Australia/Sydney     city
-23.55e  -46.65e s" Sao Paulo"     America/Sao_Paulo    city
 35.67e   51.43e s" Tehran"        Asia/Tehran          city
 35.75e  139.50e s" Tokyo"         Asia/Tokyo           city
 49.22e -123.10e s" Vancouver"     America/Vancouver    city

: city-tz ( a -- tz )   \ return tz -12..12 for a city
    3 cells + @
    100 /
;

: city-pos ( a -- lat lon )
    cell+ dup f@
    cell+ f@
;

: cities
    $c0b0a0 GD.ColorRGB#
    GD.SRC_ALPHA 1 GD.BlendFunc

    GD.POINTS GD.Begin
    citylist @
    begin
        dup
    while
        dup city-tz 12 + seetz + c@ 13 and 9 +
        GD.PointSize

        dup city-pos latlong
        GD.Vertex2f
        @
    repeat
    drop

    $a09080 GD.ColorRGB#
    citylist @
    begin
        dup
    while
        dup city-tz 12 + seetz + c@
        if
            >r
            r@ city-pos latlong
            swap 16 / swap 16 / 3 +
            26 GD.OPT_CENTERX 
            r@ 4 cells + count
            GD.cmd_text
            \ 2drop
            r>
        then
        @
    repeat
    drop
;

( Clocks                                     JCB 14:22 02/15/15)

: drawclocks ( JD -- )
    \ Track the localtime as minutes-in-week,
    \ considering each hour to be 100 minutes.
    \ So Sunday 11:30am is  1130
    \    Monday 11:30am is  2530 etc.

    1 - 7 mod 2400 * swap
    MINUTES-IN-DAY um* nip
    60 /mod 100 * +
    +

    25 0 do
        >r
        i 24 mod
        dup 100 + GD.Tag
        seetz + c@ if
            i 800 24 */ dup     ( x x )
            47 26 GD.OPT_CENTER
            r@ 2400 mod s>d <# # # [char] : hold # # #>
            GD.cmd_text
            63 26 GD.OPT_CENTER
            r@ 2400 / 7 mod case
            0 of s" Sun" endof
            1 of s" Mon" endof
            2 of s" Tue" endof
            3 of s" Wed" endof
            4 of s" Thu" endof
            5 of s" Fri" endof
            6 of s" Sat" endof
            endcase
            GD.cmd_text
            $303060
        else
            $000000
        then
        GD.cmd_bgcolor

        i 800 24 */
        17 16 GD.OPT_FLAT GD.OPT_NOSECS or GD.OPT_NOTICKS or
        r@ 100 /mod swap 0 0 GD.cmd_clock

        r> 100 +    \ advance 1 hour to next zone
    loop
    drop
;

( Illumination bitmap                        JCB 19:06 02/14/15)

480 240 * 2* constant ILL-BASE

: ill-setup
    1 GD.BitmapHandle
    ILL-BASE GD.BitmapSource
    GD.BILINEAR GD.BORDER GD.BORDER 480 240 GD.BitmapSize
    GD.L8 200 120 GD.BitmapLayout
    200 120 * 0 do
        i 200 mod 0=
        i 200 < or
        i 200 mod 100 = i 200 / 1 and 0= and or
        i ILL-BASE + GD.c!
    loop
;

\ ============================================================
: array
    create cells allot
    does>  swap cells +
;

\ 'g' is 0.15 fixed-point math format
\ all numbers are -1 to +1.

: f>g
    32767e f* f>s
;

: g*
    *
    15 0 do 2/ loop
;

: g>s
    7 0 do 2/ loop
;

120 array latsin
120 array latcos
200 array loncos

marker scratchinit
:noname
    \ lat
    120 0 do
        i 60 - s>f 120e f/ pi f*
        fdup fsin f>g i latsin !
        fcos f>g i latcos !
    loop
    \ long
    200 0 do
        i 100 - s>f 100e f/ pi f*
        fcos f>g i loncos !
    loop
; execute
scratchinit

: asvector ( lon lat -- x z )
    fdup fsin frot frot
    fcos fswap fcos f*
    fswap
;

variable sx
variable sz

create linebuf 200 allot

\ lights8 is a lighting table, indexed -128 to 127

here 256 allot 128 + constant lights8

marker tmp
    :noname
        128 -128 do
            i
            5 *
            0 max 255 min
            invert
            lights8 i + c!
        loop
    ; execute
tmp

code renderline  ( multerm addterm -- )
    r0 r3 move,             \ r3: addterm
    ' drop call,
    0 loncos r1 ldk,        \ r1: loncos pointer
    200 loncos r2 ldk,      \ r2: loncos limit
    linebuf r4 ldk,         \ r4: linebuf pointer
    lights8 r5 ldk,         \ r5: lights8 pointer
    begin
        0 r1 cc ldi,        \ fetch from loncos
        r0 cc cc mul,       \ * multerm
        15 # cc cc ashr,    \
        r3 cc cc add,       \ + addterm
        8 # cc cc ashr,     \ g>s 2/

        r5 cc cc add,
        0 cc cc ldi.b,
        r4 0 cc sti.b,      \ store

        1 # r4 r4 add,      \ bump
        4 # r1 r1 add,
        r1 r2 cmp,
    z until

    ' drop jmp,
end-code

\ variable addterm
\ 
\ : renderline ( multerm addterm -- )
\     addterm !
\     200 0 do
\         dup i loncos @ g*
\         addterm @ +
\         g>s 2/
\         lights8 + c@
\         linebuf i + c!
\     loop
\     drop
\ ;

: lightmap  ( F: dec -- ) \ compute lightmap, load to bitmap
    0.0e fswap degrees fnegate
    asvector f>g sz ! f>g sx !

    120 0 do
        i latcos @ sx @ g*
        i latsin @ sz @ g*
        renderline
        linebuf 200 GD.supply
    loop
;

: across ( x y dx -- x' y ) \ move dx pixels across
    16 *
    rot + $fffffff0 and
    swap
;

: draw-wrap  ( x y xt -- )
    >r
    2dup  r@ execute
    2dup  width negate across r@ execute
          width across r> execute
;

: light ( F: RA dec -- )
    GD.BITMAPS GD.Begin
    \ 63 GD.BitmapTransformA
    \ 64 GD.BitmapTransformE
    200 255 width */ GD.BitmapTransformA
    120 255 height */ GD.BitmapTransformE
    \ 0 0 1 0 GD.Vertex2ii

    ILL-BASE 200 120 * GD.cmd_memwrite
    lightmap

    1 GD.BitmapHandle
    0 GD.Cell
    GD.BITMAPS GD.Begin
    90e onmap

    $000000 GD.ColorRGB#
    220 GD.ColorA
    width 2/ negate across
    ['] GD.Vertex2f draw-wrap
;
\ ============================================================


: asun ( x y -- ) \ draw a sun
    8 0 do
        500 i 50 * -
        width 800 */
        GD.PointSize
        2dup GD.Vertex2f
    loop
    2drop
;

variable prevtag

: announce ( message -- )
    2>r
    GD.Clear
    width 2/ height 2/ 31 GD.OPT_CENTER 2r> GD.cmd_text
    GD.swap
;

2variable time0

: globe
    GD.init

    s" initializing RTC..." announce

    \ mcp7940m-init

    s" loading from microSD" announce

    0 0 GD.cmd_loadimage
    s" EARTH480.JPG" file>gd

    ill-setup

    \ 30 07 FEB 15 2015
    \ time&date
    00 01 06 01 01 2015

    >r swap r>
    Fixed-from-Gregorian
    \ 60 + 183 +
    J2000 - 0 swap
    $80000000. d- 2>r   \ save the day number as a JD

                        ( ss mm hh )
    60 * + 60 * +       \ seconds since midnight
    0 swap SECONDS-IN-DAY um/mod nip
    0                   \ time of day as a JD
    2r> d+              \ merge them: now as a JD

    jd@ d- time0 2!

    begin
        jd@ time0 2@ d+
        time0 2@
        MINUTE d+ MINUTE d+
        MINUTE d+ MINUTE d+
        DAY d+
        time0 2!

        \ cr .s 2dup .x .x space
        \ 2dup jd-mhmdy . . . . .

        GD.getinputs
        GD.inputs.tag 100 125 within 
        prevtag @ 0= and if
            GD.inputs.tag 100 - seetz +
            dup c@ invert swap c!
        then
        GD.inputs.tag prevtag !

        GD.Clear
        GD.BITMAPS GD.Begin
        0 0 0 0 GD.Vertex2ii
        GD.RestoreContext

        2dup jdwhere2
        \ ms@
        fover fover light
        \ ms@ swap - cr .ms

        \ Draw the Sun itself as 8 layered circles
        onmap
        GD.SRC_ALPHA 1 GD.BlendFunc
        GD.POINTS GD.Begin
        $c08040 GD.ColorRGB#
        80 GD.ColorA
        ['] asun draw-wrap

        GD.RestoreContext
        \ cities

        GD.RestoreContext
        2dup drawnow

        GD.RestoreContext
        \ 2dup drawclocks

        \ 0 240 31 0 GD.inputs.tag GD.cmd_number

        GD.swap

        \ $100000000. d+
        \ DAY d+
        \ MINUTE d+
        \ MINUTE d+
        \ SECOND 60 1 m*/ d+
        \ DAY d+
        \ GD.REG_FRAMES GD.@ .

        2drop
    again
;
