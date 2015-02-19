: ram>GD ( caddr u -- ) \ copy RAM to the FT800 command buffer
    bounds ?do
        i @ GD.c
    4 +loop
;

: file>GD ( caddr u -- ) \ feed a file to the FT800 command buffer
    r/o open-file throw >r
    begin
        pad 512 r@ read-file throw
        ?dup
    while
        pad swap ram>GD
    repeat
    r> close-file throw
;

: (GD.dump)  ( caddr -- caddr' ) \ dump one line of FT800 RAM
    cr dup dup 
    0 <# # # # # # # #> type 
    space space
    16 0 do
        dup GD.c@ 0 <# # # #> 
        type space char+
    loop
    space swap
    16 0 do
        dup GD.c@ 127 and dup 0 bl within
        over 127 = or
        if drop [char] . then
        emit char+
    loop
    drop
;

: GD.dump  ( a u -- ) \ dump FT800 memory, useful for debugging
    ?dup if
        base @ >r hex
        1- 16 / 1+
        0 do
          (GD.dump)
        loop
        r> base !
    then
    drop
;
