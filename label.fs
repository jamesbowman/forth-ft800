\ label
\
\ Create a new wordlist that only contains the ANS words that the
\ host Forth supports.
\
\ This becomes the Forth wordlist, so the system then behaves
\ just like a pure-ANS Forth.
\
\ BYE prints the ANS program label, with program requirements.
\

: wrap \ if the following word exists in the host Forth, define it
       \ otherwise, ignore the rest of the line.
    >IN @
    BL WORD FIND NIP
    IF
        >IN !
        :
    ELSE
        cr SOURCE type
        DROP
        POSTPONE \
    THEN
;

: (exec)  ( xt -- ) \ compile xt if compiling, otherwise execute it
    STATE @ IF
        COMPILE,
    ELSE
        EXECUTE
    THEN
;

: exec
    ' POSTPONE LITERAL POSTPONE (EXEC)
; immediate

: conly \ a compile-only word
    STATE @ 0= IF
        abort" Interpretation semantics for this word are undefined"
    THEN
;

: flag
    CREATE FALSE ,
    DOES>  TRUE SWAP !
;

flag BLOCK
flag BLOCK-EXT
flag CORE
flag CORE-EXT
flag DOUBLE
flag DOUBLE-EXT
flag EXCEPTION
flag FACILITY
flag FACILITY-EXT
flag FILE
flag FILE-EXT
flag FLOATING
flag FLOATING-EXT
flag LOCAL
flag LOCAL-EXT
flag MEMORY
flag SEARCH
flag SEARCH-EXT
flag STRING
flag TOOLS
flag TOOLS-EXT

: requiring
    CR
    ." \   Requiring the "
    TYPE
    ."  word set"
;

: report
    CR ." \"
    CR ." \ This is an ANS Forth program:"
    ['] BLOCK        >BODY @ IF S" Block" requiring THEN
    ['] BLOCK-EXT    >BODY @ IF S" Block Extensions" requiring THEN
    ['] CORE-EXT     >BODY @ IF S" Core Extensions" requiring THEN
    ['] DOUBLE       >BODY @ IF S" Double-Number" requiring THEN
    ['] DOUBLE-EXT   >BODY @ IF S" Double-Number Extensions" requiring THEN
    ['] EXCEPTION    >BODY @ IF S" Exception" requiring THEN
    ['] FACILITY     >BODY @ IF S" Facility" requiring THEN
    ['] FACILITY-EXT >BODY @ IF S" Facility Extensions" requiring THEN
    ['] FILE         >BODY @ IF S" File Access" requiring THEN
    ['] FILE-EXT     >BODY @ IF S" File Access Extensions" requiring THEN
    ['] FLOATING     >BODY @ IF S" Floating-Point" requiring THEN
    ['] FLOATING-EXT >BODY @ IF S" Floating-Point Extensions" requiring THEN
    ['] LOCAL        >BODY @ IF S" Locals" requiring THEN
    ['] LOCAL-EXT    >BODY @ IF S" Locals Extensions" requiring THEN
    ['] MEMORY       >BODY @ IF S" Memory-Allocation Extensions" requiring THEN
    ['] SEARCH       >BODY @ IF S" Search-Order" requiring THEN
    ['] SEARCH-EXT   >BODY @ IF S" Search-Order Extensions" requiring THEN
    ['] STRING       >BODY @ IF S" String" requiring THEN
    ['] TOOLS        >BODY @ IF S" Programming-Tools" requiring THEN
    ['] TOOLS-EXT    >BODY @ IF S" Programming-Tools Extensions" requiring THEN
    CR ." \"
;

WORDLIST CONSTANT STANDARD
STANDARD SET-CURRENT

wrap !               CORE         exec ! ; IMMEDIATE
wrap #               CORE         exec # ; IMMEDIATE
wrap #>              CORE         exec #> ; IMMEDIATE
wrap #S              CORE         exec #S ; IMMEDIATE
wrap #TIB            CORE-EXT     exec #TIB ; IMMEDIATE
wrap '               CORE         exec ' ; IMMEDIATE
wrap (               CORE         conly POSTPONE ( ; IMMEDIATE
wrap (LOCAL)         LOCAL        conly POSTPONE (LOCAL) ; IMMEDIATE
wrap *               CORE         exec * ; IMMEDIATE
wrap */              CORE         exec */ ; IMMEDIATE
wrap */MOD           CORE         exec */MOD ; IMMEDIATE
wrap +               CORE         exec + ; IMMEDIATE
wrap +!              CORE         exec +! ; IMMEDIATE
wrap +LOOP           CORE         conly POSTPONE +LOOP ; IMMEDIATE
wrap ,               CORE         exec , ; IMMEDIATE
wrap -               CORE         exec - ; IMMEDIATE
wrap -TRAILING       STRING       -TRAILING ;
wrap .               CORE         exec . ; IMMEDIATE
wrap ."              CORE         conly POSTPONE ." ; IMMEDIATE
wrap .(              CORE-EXT     POSTPONE .( ; IMMEDIATE
wrap .R              CORE-EXT     exec .R ; IMMEDIATE
wrap .S              TOOLS        exec .S ; IMMEDIATE
wrap /               CORE         exec / ; IMMEDIATE
wrap /MOD            CORE         exec /MOD ; IMMEDIATE
wrap /STRING         STRING       exec /STRING ; IMMEDIATE
wrap 0<              CORE         exec 0< ; IMMEDIATE
wrap 0<>             CORE-EXT     exec 0<> ; IMMEDIATE
wrap 0=              CORE         exec 0= ; IMMEDIATE
wrap 0>              CORE-EXT     exec 0> ; IMMEDIATE
wrap 1+              CORE         exec 1+ ; IMMEDIATE
wrap 1-              CORE         exec 1- ; IMMEDIATE
wrap 2!              CORE         exec 2! ; IMMEDIATE
wrap 2*              CORE         exec 2* ; IMMEDIATE
wrap 2/              CORE         exec 2/ ; IMMEDIATE
wrap 2>R             CORE-EXT     conly POSTPONE 2>R ; IMMEDIATE
wrap 2@              CORE         exec 2@ ; IMMEDIATE
wrap 2CONSTANT       DOUBLE       exec 2CONSTANT ; IMMEDIATE
wrap 2DROP           CORE         exec 2DROP ; IMMEDIATE
wrap 2DUP            CORE         exec 2DUP ; IMMEDIATE
wrap 2LITERAL        DOUBLE       conly POSTPONE 2LITERAL ; IMMEDIATE
wrap 2OVER           CORE         exec 2OVER ; IMMEDIATE
wrap 2R>             CORE-EXT     conly POSTPONE 2R> ; IMMEDIATE
wrap 2R@             CORE-EXT     conly POSTPONE 2R@ ; IMMEDIATE
wrap 2ROT            DOUBLE-EXT   exec 2ROT ; IMMEDIATE
wrap 2SWAP           CORE         exec 2SWAP ; IMMEDIATE
wrap 2VARIABLE       DOUBLE       2VARIABLE ;
wrap :               CORE         exec : ; IMMEDIATE
wrap :NONAME         CORE-EXT     exec :NONAME ; IMMEDIATE
wrap ;               CORE         conly POSTPONE ; ; IMMEDIATE
wrap ;CODE           TOOLS-EXT    conly POSTPONE ;CODE ; IMMEDIATE
wrap <               CORE         exec < ; IMMEDIATE
wrap <#              CORE         exec <# ; IMMEDIATE
wrap <>              CORE-EXT     exec <> ; IMMEDIATE
wrap =               CORE         exec = ; IMMEDIATE
wrap >               CORE         exec > ; IMMEDIATE
wrap >BODY           CORE         exec >BODY ; IMMEDIATE
wrap >FLOAT          FLOATING     exec >FLOAT ; IMMEDIATE
wrap >IN             CORE         exec >IN ; IMMEDIATE
wrap >NUMBER         CORE         exec >NUMBER ; IMMEDIATE
wrap >R              CORE         conly POSTPONE >R ; IMMEDIATE
wrap ?               TOOLS        conly POSTPONE ? ; IMMEDIATE
wrap ?DO             CORE-EXT     conly POSTPONE ?DO ; IMMEDIATE
wrap ?DUP            CORE         exec ?DUP ; IMMEDIATE
wrap @               CORE         exec @ ; IMMEDIATE
wrap ABORT           CORE         exec ABORT ; IMMEDIATE
wrap ABORT"          CORE         conly POSTPONE ABORT" ; IMMEDIATE
wrap ABS             CORE         exec ABS ; IMMEDIATE
wrap ACCEPT          CORE         exec ACCEPT ; IMMEDIATE
wrap AGAIN           CORE-EXT     conly POSTPONE AGAIN ; IMMEDIATE
wrap AHEAD           TOOLS-EXT    conly POSTPONE AHEAD ; IMMEDIATE
wrap ALIGN           CORE         exec ALIGN ; IMMEDIATE
wrap ALIGNED         CORE         exec ALIGNED ; IMMEDIATE
wrap ALLOCATE        MEMORY       exec ALLOCATE ; IMMEDIATE
wrap ALLOT           CORE         exec ALLOT ; IMMEDIATE
wrap ALSO            SEARCH-EXT   exec ALSO ; IMMEDIATE
wrap AND             CORE         exec AND ; IMMEDIATE
wrap ASSEMBLER       TOOLS-EXT    exec ASSEMBLER ; IMMEDIATE
wrap AT-XY           FACILITY     exec AT-XY ; IMMEDIATE
wrap BASE            CORE         exec BASE ; IMMEDIATE
wrap BEGIN           CORE         conly POSTPONE BEGIN ; IMMEDIATE
wrap BIN             FILE         exec BIN ; IMMEDIATE
wrap BL              CORE         exec BL ; IMMEDIATE
wrap BLANK           STRING       exec BLANK ; IMMEDIATE
wrap BLK             BLOCK        exec BLK ; IMMEDIATE
wrap BLOCK           BLOCK        exec BLOCK ; IMMEDIATE
wrap BUFFER          BLOCK        exec BUFFER ; IMMEDIATE
wrap BYE           ( TOOLS-EXT )  report BYE ;
wrap C!              CORE         exec C! ; IMMEDIATE
wrap C"              CORE-EXT     conly POSTPONE C" ; IMMEDIATE
wrap C,              CORE         exec C, ; IMMEDIATE
wrap C@              CORE         exec C@ ; IMMEDIATE
wrap CASE            CORE-EXT     conly POSTPONE CASE ; IMMEDIATE
wrap CATCH           EXCEPTION    exec CATCH ; IMMEDIATE
wrap CELL+           CORE         exec CELL+ ; IMMEDIATE
wrap CELLS           CORE         exec CELLS ; IMMEDIATE
wrap CHAR            CORE         exec CHAR ; IMMEDIATE
wrap CHAR+           CORE         exec CHAR+ ; IMMEDIATE
wrap CHARS           CORE         exec CHARS ; IMMEDIATE
wrap CLOSE-FILE      FILE         exec CLOSE-FILE ; IMMEDIATE
wrap CMOVE           STRING       exec CMOVE ; IMMEDIATE
wrap CMOVE>          STRING       exec CMOVE> ; IMMEDIATE
wrap CODE            TOOLS-EXT    exec CODE ; IMMEDIATE
wrap COMPARE         STRING       exec COMPARE ; IMMEDIATE
wrap COMPILE,        CORE-EXT     exec COMPILE, ; IMMEDIATE
wrap CONSTANT        CORE         exec CONSTANT ; IMMEDIATE
wrap CONVERT         CORE-EXT     exec CONVERT ; IMMEDIATE
wrap COUNT           CORE         exec COUNT ; IMMEDIATE
wrap CR              CORE         exec CR ; IMMEDIATE
wrap CREATE          CORE         exec CREATE ; IMMEDIATE
wrap CREATE-FILE     FILE         exec CREATE-FILE ; IMMEDIATE
wrap CS-PICK         TOOLS-EXT    exec CS-PICK ; IMMEDIATE
wrap CS-ROLL         TOOLS-EXT    exec CS-ROLL ; IMMEDIATE
wrap D+              DOUBLE       exec D+ ; IMMEDIATE
wrap D-              DOUBLE       exec D- ; IMMEDIATE
wrap D.              DOUBLE       exec D. ; IMMEDIATE
wrap D.R             DOUBLE       exec D.R ; IMMEDIATE
wrap D0<             DOUBLE       exec D0< ; IMMEDIATE
wrap D0=             DOUBLE       exec D0= ; IMMEDIATE
wrap D2*             DOUBLE       exec D2* ; IMMEDIATE
wrap D2/             DOUBLE       exec D2/ ; IMMEDIATE
wrap D<              DOUBLE       exec D< ; IMMEDIATE
wrap D=              DOUBLE       exec D= ; IMMEDIATE
wrap D>F             FLOATING     exec D>F ; IMMEDIATE
wrap D>S             DOUBLE       exec D>S ; IMMEDIATE
wrap DABS            DOUBLE       exec DABS ; IMMEDIATE
wrap DECIMAL         CORE         exec DECIMAL ; IMMEDIATE
wrap DEFINITIONS     SEARCH       exec DEFINITIONS ; IMMEDIATE
wrap DELETE-FILE     FILE         exec DELETE-FILE ; IMMEDIATE
wrap DEPTH           CORE         exec DEPTH ; IMMEDIATE
wrap DF!             FLOATING-EXT exec DF! ; IMMEDIATE
wrap DF@             FLOATING-EXT exec DF@ ; IMMEDIATE
wrap DFALIGN         FLOATING-EXT exec DFALIGN ; IMMEDIATE
wrap DFALIGNED       FLOATING-EXT exec DFALIGNED ; IMMEDIATE
wrap DFLOAT+         FLOATING-EXT exec DFLOAT+ ; IMMEDIATE
wrap DFLOATS         FLOATING-EXT exec DFLOATS ; IMMEDIATE
wrap DMAX            DOUBLE       exec DMAX ; IMMEDIATE
wrap DMIN            DOUBLE       exec DMIN ; IMMEDIATE
wrap DNEGATE         DOUBLE       exec DNEGATE ; IMMEDIATE
wrap DO              CORE         conly POSTPONE DO ; IMMEDIATE
wrap DOES>           CORE         conly POSTPONE DOES> ; IMMEDIATE
wrap DROP            CORE         exec DROP ; IMMEDIATE
wrap DU<             DOUBLE-EXT   exec DU< ; IMMEDIATE
wrap DUMP            TOOLS        exec DUMP ; IMMEDIATE
wrap DUP             CORE         exec DUP ; IMMEDIATE
wrap EDITOR          TOOLS-EXT    exec EDITOR ; IMMEDIATE
wrap EKEY            FACILITY-EXT exec EKEY ; IMMEDIATE
wrap EKEY>CHAR       FACILITY-EXT exec EKEY>CHAR ; IMMEDIATE
wrap EKEY?           FACILITY-EXT exec EKEY? ; IMMEDIATE
wrap ELSE            CORE         conly POSTPONE ELSE ; IMMEDIATE
wrap EMIT            CORE         exec EMIT ; IMMEDIATE
wrap EMIT?           FACILITY-EXT exec EMIT? ; IMMEDIATE
wrap EMPTY-BUFFERS   BLOCK-EXT    exec EMPTY-BUFFERS ; IMMEDIATE
wrap ENDCASE         CORE-EXT     conly POSTPONE ENDCASE ; IMMEDIATE
wrap ENDOF           CORE-EXT     conly POSTPONE ENDOF ; IMMEDIATE
wrap ENVIRONMENT?    CORE         exec ENVIRONMENT? ; IMMEDIATE
wrap ERASE           CORE-EXT     exec ERASE ; IMMEDIATE
wrap EVALUATE        CORE         exec EVALUATE ; IMMEDIATE
wrap EXECUTE         CORE         exec EXECUTE ; IMMEDIATE
wrap EXIT            CORE         conly POSTPONE EXIT ; IMMEDIATE
wrap EXPECT          CORE-EXT     exec EXPECT ; IMMEDIATE
wrap F!              FLOATING     exec F! ; IMMEDIATE
wrap F*              FLOATING     exec F* ; IMMEDIATE
wrap F**             FLOATING-EXT exec F** ; IMMEDIATE
wrap F+              FLOATING     exec F+ ; IMMEDIATE
wrap F-              FLOATING     exec F- ; IMMEDIATE
wrap F.              FLOATING-EXT exec F. ; IMMEDIATE
wrap F/              FLOATING     exec F/ ; IMMEDIATE
wrap F0<             FLOATING     exec F0< ; IMMEDIATE
wrap F0=             FLOATING     exec F0= ; IMMEDIATE
wrap F<              FLOATING     exec F< ; IMMEDIATE
wrap F>D             FLOATING     exec F>D ; IMMEDIATE
wrap F@              FLOATING     exec F@ ; IMMEDIATE
wrap FABS            FLOATING-EXT exec FABS ; IMMEDIATE
wrap FACOS           FLOATING-EXT exec FACOS ; IMMEDIATE
wrap FACOSH          FLOATING-EXT exec FACOSH ; IMMEDIATE
wrap FALIGN          FLOATING     exec FALIGN ; IMMEDIATE
wrap FALIGNED        FLOATING     exec FALIGNED ; IMMEDIATE
wrap FALOG           FLOATING-EXT exec FALOG ; IMMEDIATE
wrap FALSE           CORE-EXT     exec FALSE ; IMMEDIATE
wrap FASIN           FLOATING-EXT exec FASIN ; IMMEDIATE
wrap FASINH          FLOATING-EXT exec FASINH ; IMMEDIATE
wrap FATAN           FLOATING-EXT exec FATAN ; IMMEDIATE
wrap FATAN2          FLOATING-EXT exec FATAN2 ; IMMEDIATE
wrap FATANH          FLOATING-EXT exec FATANH ; IMMEDIATE
wrap FCONSTANT       FLOATING     exec FCONSTANT ; IMMEDIATE
wrap FCOS            FLOATING-EXT exec FCOS ; IMMEDIATE
wrap FCOSH           FLOATING-EXT exec FCOSH ; IMMEDIATE
wrap FDEPTH          FLOATING     exec FDEPTH ; IMMEDIATE
wrap FDROP           FLOATING     exec FDROP ; IMMEDIATE
wrap FDUP            FLOATING     exec FDUP ; IMMEDIATE
wrap FE.             FLOATING-EXT exec FE. ; IMMEDIATE
wrap FEXP            FLOATING-EXT exec FEXP ; IMMEDIATE
wrap FEXPM1          FLOATING-EXT exec FEXPM1 ; IMMEDIATE
wrap FILE-POSITION   FILE         exec FILE-POSITION ; IMMEDIATE
wrap FILE-SIZE       FILE         exec FILE-SIZE ; IMMEDIATE
wrap FILE-STATUS     FILE-EXT     exec FILE-STATUS ; IMMEDIATE
wrap FILL            CORE         exec FILL ; IMMEDIATE
wrap FIND            CORE         exec FIND ; IMMEDIATE
wrap FLITERAL        FLOATING     conly POSTPONE FLITERAL ; IMMEDIATE
wrap FLN             FLOATING-EXT exec FLN ; IMMEDIATE
wrap FLNP1           FLOATING-EXT exec FLNP1 ; IMMEDIATE
wrap FLOAT+          FLOATING     exec FLOAT+ ; IMMEDIATE
wrap FLOATS          FLOATING     exec FLOATS ; IMMEDIATE
wrap FLOG            FLOATING-EXT exec FLOG ; IMMEDIATE
wrap FLOOR           FLOATING     exec FLOOR ; IMMEDIATE
wrap FLUSH           BLOCK        exec FLUSH ; IMMEDIATE
wrap FLUSH-FILE      FILE-EXT     exec FLUSH-FILE ; IMMEDIATE
wrap FM/MOD          CORE         exec FM/MOD ; IMMEDIATE
wrap FMAX            FLOATING     exec FMAX ; IMMEDIATE
wrap FMIN            FLOATING     exec FMIN ; IMMEDIATE
wrap FNEGATE         FLOATING     exec FNEGATE ; IMMEDIATE
wrap FORGET          TOOLS-EXT    exec FORGET ; IMMEDIATE
wrap FORTH           SEARCH-EXT   GET-ORDER NIP STANDARD SWAP SET-ORDER ;
wrap FORTH-WORDLIST  SEARCH       exec STANDARD ; IMMEDIATE
wrap FOVER           FLOATING     exec FOVER ; IMMEDIATE
wrap FREE            MEMORY       exec FREE ; IMMEDIATE
wrap FROT            FLOATING     exec FROT ; IMMEDIATE
wrap FROUND          FLOATING     exec FROUND ; IMMEDIATE
wrap FS.             FLOATING-EXT exec FS. ; IMMEDIATE
wrap FSIN            FLOATING-EXT exec FSIN ; IMMEDIATE
wrap FSINCOS         FLOATING-EXT exec FSINCOS ; IMMEDIATE
wrap FSINH           FLOATING-EXT exec FSINH ; IMMEDIATE
wrap FSQRT           FLOATING-EXT exec FSQRT ; IMMEDIATE
wrap FSWAP           FLOATING     exec FSWAP ; IMMEDIATE
wrap FTAN            FLOATING-EXT exec FTAN ; IMMEDIATE
wrap FTANH           FLOATING-EXT exec FTANH ; IMMEDIATE
wrap FVARIABLE       FLOATING     exec FVARIABLE ; IMMEDIATE
wrap F~              FLOATING-EXT exec F~ ; IMMEDIATE
wrap GET-CURRENT     SEARCH       exec GET-CURRENT ; IMMEDIATE
wrap GET-ORDER       SEARCH       exec GET-ORDER ; IMMEDIATE
wrap HERE            CORE         exec HERE ; IMMEDIATE
wrap HEX             CORE-EXT     exec HEX ; IMMEDIATE
wrap HOLD            CORE         exec HOLD ; IMMEDIATE
wrap I               CORE         conly POSTPONE I ;  IMMEDIATE
wrap IF              CORE         conly POSTPONE IF ; IMMEDIATE
wrap IMMEDIATE       CORE         exec IMMEDIATE ; IMMEDIATE
wrap INCLUDE-FILE    FILE         exec INCLUDE-FILE ; IMMEDIATE
wrap INCLUDED        FILE         exec INCLUDED ; IMMEDIATE
wrap INVERT          CORE         exec INVERT ; IMMEDIATE
wrap J               CORE         conly POSTPONE J ; IMMEDIATE
wrap KEY             CORE         exec KEY ; IMMEDIATE
wrap KEY?            FACILITY     exec KEY? ; IMMEDIATE
wrap LEAVE           CORE         conly POSTPONE LEAVE ; IMMEDIATE
wrap LIST            BLOCK-EXT    exec LIST ; IMMEDIATE
wrap LITERAL         CORE         conly POSTPONE LITERAL ; IMMEDIATE
wrap LOAD            BLOCK        exec LOAD ; IMMEDIATE
wrap LOCALS|         LOCAL-EXT    conly POSTPONE LOCALS| ; IMMEDIATE
wrap LOOP            CORE         conly POSTPONE LOOP ; IMMEDIATE
wrap LSHIFT          CORE         exec LSHIFT ; IMMEDIATE
wrap M*              CORE         exec M* ; IMMEDIATE
wrap M*/             DOUBLE       exec M*/ ; IMMEDIATE
wrap M+              DOUBLE       exec M+ ; IMMEDIATE
wrap MARKER          CORE-EXT     exec MARKER ; IMMEDIATE
wrap MAX             CORE         exec MAX ; IMMEDIATE
wrap MIN             CORE         exec MIN ; IMMEDIATE
wrap MOD             CORE         exec MOD ; IMMEDIATE
wrap MOVE            CORE         exec MOVE ; IMMEDIATE
wrap MS              FACILITY-EXT exec MS ; IMMEDIATE
wrap NEGATE          CORE         exec NEGATE ; IMMEDIATE
wrap NIP             CORE-EXT     exec NIP ; IMMEDIATE
wrap OF              CORE-EXT     conly POSTPONE OF ; IMMEDIATE
wrap ONLY            SEARCH-EXT   STANDARD 1 SET-ORDER ;
wrap OPEN-FILE       FILE         exec OPEN-FILE ; IMMEDIATE
wrap OR              CORE         exec OR ; IMMEDIATE
wrap ORDER           SEARCH-EXT   exec ORDER ; IMMEDIATE
wrap OVER            CORE         exec OVER ; IMMEDIATE
wrap PAD             CORE-EXT     exec PAD ; IMMEDIATE
wrap PAGE            FACILITY     exec PAGE ; IMMEDIATE
wrap PARSE           CORE-EXT     exec PARSE ; IMMEDIATE
wrap PICK            CORE-EXT     exec PICK ; IMMEDIATE
wrap POSTPONE        CORE         conly POSTPONE POSTPONE ; IMMEDIATE
wrap PRECISION       FLOATING-EXT exec PRECISION ; IMMEDIATE
wrap PREVIOUS        SEARCH-EXT   exec PREVIOUS ; IMMEDIATE
wrap QUERY           CORE-EXT     exec QUERY ; IMMEDIATE
wrap QUIT            CORE         exec QUIT ; IMMEDIATE
wrap R/O             FILE         exec R/O ; IMMEDIATE
wrap R/W             FILE         exec R/W ; IMMEDIATE
wrap R>              CORE         conly POSTPONE R> ; IMMEDIATE
wrap R@              CORE         conly POSTPONE R@ ; IMMEDIATE
wrap READ-FILE       FILE         exec READ-FILE ; IMMEDIATE
wrap READ-LINE       FILE         exec READ-LINE ; IMMEDIATE
wrap RECURSE         CORE         conly POSTPONE RECURSE ; IMMEDIATE
wrap REFILL          CORE-EXT     exec REFILL ; IMMEDIATE
wrap RENAME-FILE     FILE-EXT     exec RENAME-FILE ; IMMEDIATE
wrap REPEAT          CORE         conly POSTPONE REPEAT ; IMMEDIATE
wrap REPOSITION-FILE FILE         exec REPOSITION-FILE ; IMMEDIATE
wrap REPRESENT       FLOATING     exec REPRESENT ; IMMEDIATE
wrap RESIZE          MEMORY       exec RESIZE ; IMMEDIATE
wrap RESIZE-FILE     FILE         exec RESIZE-FILE ; IMMEDIATE
wrap RESTORE-INPUT   CORE-EXT     exec RESTORE-INPUT ; IMMEDIATE
wrap ROLL            CORE-EXT     exec ROLL ; IMMEDIATE
wrap ROT             CORE         exec ROT ; IMMEDIATE
wrap RSHIFT          CORE         exec RSHIFT ; IMMEDIATE
wrap S"              CORE         STATE @ IF POSTPONE S" ELSE FILE ['] S" EXECUTE THEN ; IMMEDIATE
wrap S>D             CORE         exec S>D ; IMMEDIATE
wrap SAVE-BUFFERS    BLOCK        exec SAVE-BUFFERS ; IMMEDIATE
wrap SAVE-INPUT      CORE-EXT     exec SAVE-INPUT ; IMMEDIATE
wrap SCR             BLOCK-EXT    exec SCR ; IMMEDIATE
wrap SEARCH          STRING       exec SEARCH ; IMMEDIATE
wrap SEARCH-WORDLIST SEARCH       exec SEARCH-WORDLIST ; IMMEDIATE
wrap SEE             TOOLS        exec SEE ; IMMEDIATE
wrap SET-CURRENT     SEARCH       exec SET-CURRENT ; IMMEDIATE
wrap SET-ORDER       SEARCH       exec SET-ORDER ; IMMEDIATE
wrap SET-PRECISION   FLOATING-EXT exec SET-PRECISION ; IMMEDIATE
wrap SF!             FLOATING-EXT exec SF! ; IMMEDIATE
wrap SF@             FLOATING-EXT exec SF@ ; IMMEDIATE
wrap SFALIGN         FLOATING-EXT exec SFALIGN ; IMMEDIATE
wrap SFALIGNED       FLOATING-EXT exec SFALIGNED ; IMMEDIATE
wrap SFLOAT+         FLOATING-EXT exec SFLOAT+ ; IMMEDIATE
wrap SFLOATS         FLOATING-EXT exec SFLOATS ; IMMEDIATE
wrap SIGN            CORE         exec SIGN ; IMMEDIATE
wrap SLITERAL        STRING       conly POSTPONE SLITERAL ; IMMEDIATE
wrap SM/REM          CORE         exec SM/REM ; IMMEDIATE
wrap SOURCE          CORE         exec SOURCE ; IMMEDIATE
wrap SOURCE-ID       CORE-EXT     exec SOURCE-ID ; IMMEDIATE
wrap SPACE           CORE         exec SPACE ; IMMEDIATE
wrap SPACES          CORE         exec SPACES ; IMMEDIATE
wrap SPAN            CORE-EXT     exec SPAN ; IMMEDIATE
wrap STATE           CORE         exec STATE ; IMMEDIATE
wrap SWAP            CORE         exec SWAP ; IMMEDIATE
wrap THEN            CORE         conly POSTPONE THEN ; IMMEDIATE
wrap THROW           EXCEPTION    exec THROW ; IMMEDIATE
wrap THRU            BLOCK-EXT    exec THRU ; IMMEDIATE
wrap TIB             CORE-EXT     exec TIB ; IMMEDIATE
wrap TIME&DATE       FACILITY-EXT exec TIME&DATE ; IMMEDIATE
wrap TO              CORE-EXT     STATE @ IF POSTPONE TO ELSE ['] TO EXECUTE THEN ; IMMEDIATE
wrap TRUE            CORE-EXT     exec TRUE ; IMMEDIATE
wrap TUCK            CORE-EXT     exec TUCK ; IMMEDIATE
wrap TYPE            CORE         exec TYPE ; IMMEDIATE
wrap U.              CORE         exec U. ; IMMEDIATE
wrap U.R             CORE-EXT     exec U.R ; IMMEDIATE
wrap U<              CORE         exec U< ; IMMEDIATE
wrap U>              CORE-EXT     exec U> ; IMMEDIATE
wrap UM*             CORE         exec UM* ; IMMEDIATE
wrap UM/MOD          CORE         exec UM/MOD ; IMMEDIATE
wrap UNLOOP          CORE         conly POSTPONE UNLOOP ; IMMEDIATE
wrap UNTIL           CORE         conly POSTPONE UNTIL ; IMMEDIATE
wrap UNUSED          CORE-EXT     exec UNUSED ; IMMEDIATE
wrap UPDATE          BLOCK        exec UPDATE ; IMMEDIATE
wrap VALUE           CORE-EXT     exec VALUE ; IMMEDIATE
wrap VARIABLE        CORE         exec VARIABLE ; IMMEDIATE
wrap W/O             FILE         exec W/O ; IMMEDIATE
wrap WHILE           CORE         conly POSTPONE WHILE ; IMMEDIATE
wrap WITHIN          CORE-EXT     exec WITHIN ; IMMEDIATE
wrap WORD            CORE         exec WORD ; IMMEDIATE
wrap WORDLIST        SEARCH       exec WORDLIST ; IMMEDIATE
wrap WORDS           TOOLS        exec WORDS ; IMMEDIATE
wrap WRITE-FILE      FILE         exec WRITE-FILE ; IMMEDIATE
wrap WRITE-LINE      FILE         exec WRITE-LINE ; IMMEDIATE
wrap XOR             CORE         exec XOR ; IMMEDIATE
wrap [               CORE         conly POSTPONE [ ; IMMEDIATE
wrap [']             CORE         conly POSTPONE ['] ; IMMEDIATE
wrap [CHAR]          CORE         conly POSTPONE [CHAR] ; IMMEDIATE
wrap [COMPILE]       CORE-EXT     conly POSTPONE [COMPILE] ; IMMEDIATE
wrap [ELSE]          TOOLS-EXT    POSTPONE [ELSE] ; IMMEDIATE
wrap [IF]            TOOLS-EXT    POSTPONE [IF] ; IMMEDIATE
wrap [THEN]          TOOLS-EXT    POSTPONE [THEN] ; IMMEDIATE
wrap \               CORE-EXT     POSTPONE \ ; IMMEDIATE
wrap ]               CORE         exec ] ; IMMEDIATE

STANDARD 1 SET-ORDER
