\    --STANDARD--                             \  Wil Baden  2003-02-22

\  *******************************************************************
\  *                                                                 *
\  *                    ONLY STANDARD DEFINITIONS                    *
\  *                                                                 *
\  *******************************************************************

WORDLIST CONSTANT STANDARD
STANDARD SET-CURRENT

\ Standard-Clone

: ! ! ; 
: # # ; 
: #> #> ; 
: #S #S ; 
: ' ' ; 
: ( POSTPONE ( ; IMMEDIATE 
: (LOCAL) POSTPONE (LOCAL) ; IMMEDIATE
: * * ; 
: */ */ ; 
: */MOD */MOD ; 
: + + ; 
: +! +! ; 
: +LOOP POSTPONE +LOOP ; IMMEDIATE 
: , , ; 
: - - ; 
: -TRAILING -TRAILING ; 
: . . ; 
: ." POSTPONE ." ; IMMEDIATE 
: .( POSTPONE .( ; IMMEDIATE 
: .R .R ; 
: .S .S ; 
: / / ; 
: /MOD /MOD ; 
: /STRING /STRING ; 
: 0< 0< ; 
: 0<> 0<> ; 
: 0= 0= ; 
: 0> 0> ; 
: 1+ 1+ ; 
: 1- 1- ; 
: 2! 2! ; 
: 2* 2* ; 
: 2/ 2/ ; 
: 2>R POSTPONE 2>R ; IMMEDIATE
: 2@ 2@ ;
: 2CONSTANT 2CONSTANT ; 
: 2DROP 2DROP ; 
: 2DUP 2DUP ; 
: 2LITERAL POSTPONE 2LITERAL ; IMMEDIATE 
: 2OVER 2OVER ; 
: 2R> POSTPONE 2R> ; IMMEDIATE
: 2R@ POSTPONE 2R@ ; IMMEDIATE
: 2ROT 2ROT ; 
: 2SWAP 2SWAP ; 
: 2VARIABLE 2VARIABLE ; 
: : : ; 
: :NONAME :NONAME ; 
: ; POSTPONE ; ; IMMEDIATE 
: ;CODE POSTPONE ;CODE ; IMMEDIATE 
: < < ; 
: <# <# ; 
: <> <> ; 
: = = ; 
: > > ; 
: >BODY >BODY ; 
: >FLOAT >FLOAT ; 
: >IN >IN ; 
: >NUMBER >NUMBER ; 
: >R POSTPONE >R ; IMMEDIATE
: ? POSTPONE ? ; IMMEDIATE 
: ?DO POSTPONE ?DO ; IMMEDIATE 
: ?DUP ?DUP ; 
: @ @ ; 
: ABORT ABORT ; 
: ABORT" POSTPONE ABORT" ; IMMEDIATE 
: ABS ABS ; 
: ACCEPT ACCEPT ; 
: AGAIN POSTPONE AGAIN ; IMMEDIATE 
: AHEAD POSTPONE AHEAD ; IMMEDIATE
: ALIGN ALIGN ; 
: ALIGNED ALIGNED ; 
: ALLOCATE ALLOCATE ; 
: ALLOT ALLOT ; 
: ALSO ALSO ; 
: AND AND ; 
: ASSEMBLER ASSEMBLER ; 
: AT-XY AT-XY ; 
: BASE BASE ; 
: BEGIN POSTPONE BEGIN ; IMMEDIATE 
: BIN BIN ; 
: BL BL ; 
: BLANK BLANK ; 
: BLK BLK ; 
: BLOCK BLOCK ; 
\ BUFFER
: BYE BYE ; 
: C! C! ; 
: C" POSTPONE C" ; IMMEDIATE 
: C, C, ; 
: C@ C@ ; 
: CASE POSTPONE CASE ; IMMEDIATE 
: CATCH CATCH ; 
: CELL+ CELL+ ; 
: CELLS CELLS ;
: CHAR CHAR ; 
: CHAR+ CHAR+ ; 
: CHARS CHARS ; 
: CLOSE-FILE CLOSE-FILE ; 
: CMOVE CMOVE ; 
: CMOVE> CMOVE> ; 
: CODE CODE ; 
: COMPARE COMPARE ; 
: COMPILE, COMPILE, ; 
: CONSTANT CONSTANT ; 
: COUNT COUNT ; 
: CR CR ; 
: CREATE CREATE ; 
: CREATE-FILE CREATE-FILE ; 
: CS-PICK CS-PICK ; 
: CS-ROLL CS-ROLL ; 
: D+ D+ ; 
: D- D- ; 
: D. D. ; 
: D.R D.R ; 
: D0< D0< ; 
: D0= D0= ; 
: D2* D2* ; 
: D2/ D2/ ; 
: D< D< ; 
: D= D= ; 
: D>F D>F ; 
: D>S D>S ; 
: DABS DABS ; 
: DECIMAL DECIMAL ; 
: DEFINITIONS DEFINITIONS ; 
: DELETE-FILE DELETE-FILE ; 
: DEPTH DEPTH ; 
: DF! DF! ; 
: DF@ DF@ ; 
: DFALIGN DFALIGN ; 
: DFALIGNED DFALIGNED ; 
: DFLOAT+ DFLOAT+ ; 
: DFLOATS DFLOATS ; 
: DMAX DMAX ; 
: DMIN DMIN ; 
: DNEGATE DNEGATE ; 
: DO POSTPONE DO ; IMMEDIATE 
: DOES> POSTPONE DOES> ; IMMEDIATE 
: DROP DROP ; 
: DU< DU< ; 
: DUMP DUMP ; 
: DUP DUP ; 
\ : EDITOR EDITOR ; 
: EKEY EKEY ; 
: EKEY>CHAR EKEY>CHAR ; 
: EKEY? EKEY? ; 
: ELSE POSTPONE ELSE ; IMMEDIATE 
: EMIT EMIT ; 
\ : EMIT? EMIT? ; 
\ EMPTY-BUFFERS
: ENDCASE POSTPONE ENDCASE ; IMMEDIATE 
: ENDOF POSTPONE ENDOF ; IMMEDIATE 
: ENVIRONMENT? ENVIRONMENT? ; 
: ERASE ERASE ; 
: EVALUATE EVALUATE ; 
: EXECUTE EXECUTE ; 
: EXIT POSTPONE EXIT ; IMMEDIATE 
: F! F! ; 
: F* F* ; 
: F** F** ; 
: F+ F+ ; 
: F- F- ; 
: F. F. ; 
: F/ F/ ; 
: F0< F0< ; 
: F0= F0= ; 
: F< F< ; 
: F>D F>D ; 
: F@ F@ ; 
: FABS FABS ; 
: FACOS FACOS ; 
: FACOSH FACOSH ; 
: FALIGN FALIGN ; 
: FALIGNED FALIGNED ; 
: FALOG FALOG ; 
: FALSE FALSE ; 
: FASIN FASIN ; 
: FASINH FASINH ; 
: FATAN FATAN ; 
: FATAN2 FATAN2 ; 
: FATANH FATANH ; 
: FCONSTANT FCONSTANT ; 
: FCOS FCOS ; 
: FCOSH FCOSH ; 
: FDEPTH FDEPTH ; 
: FDROP FDROP ; 
: FDUP FDUP ; 
: FE. FE. ; 
: FEXP FEXP ; 
: FEXPM1 FEXPM1 ; 
: FILE-POSITION FILE-POSITION ; 
: FILE-SIZE FILE-SIZE ; 
: FILE-STATUS FILE-STATUS ; 
: FILL FILL ; 
: FIND FIND ; 
: FLITERAL POSTPONE FLITERAL ; IMMEDIATE 
: FLN FLN ; 
: FLNP1 FLNP1 ; 
: FLOAT+ FLOAT+ ; 
: FLOATS FLOATS ; 
: FLOG FLOG ; 
: FLOOR FLOOR ; 
\ FLUSH
: FLUSH-FILE FLUSH-FILE ; 
: FM/MOD FM/MOD ; 
: FMAX FMAX ; 
: FMIN FMIN ; 
: FNEGATE FNEGATE ; 
: FORTH GET-ORDER NIP STANDARD SWAP SET-ORDER ;
: FORTH-WORDLIST STANDARD ; 
: FOVER FOVER ; 
: FREE FREE ; 
: FROT FROT ; 
: FROUND FROUND ; 
: FS. FS. ; 
: FSIN FSIN ; 
: FSINCOS FSINCOS ; 
: FSINH FSINH ; 
: FSQRT FSQRT ; 
: FSWAP FSWAP ; 
: FTAN FTAN ; 
: FTANH FTANH ; 
: FVARIABLE FVARIABLE ; 
: F~ F~ ; 
: GET-CURRENT GET-CURRENT ; 
: GET-ORDER GET-ORDER ; 
: HERE HERE ; 
: HEX HEX ; 
: HOLD HOLD ; 
: I POSTPONE I ;  IMMEDIATE
: IF POSTPONE IF ; IMMEDIATE 
: IMMEDIATE IMMEDIATE ; 
: INCLUDE-FILE INCLUDE-FILE ; 
: INCLUDED INCLUDED ; 
: INVERT INVERT ; 
: J POSTPONE J ; IMMEDIATE
: KEY KEY ; 
: KEY? KEY? ; 
: LEAVE POSTPONE LEAVE ; IMMEDIATE
\ LIST
: LITERAL POSTPONE LITERAL ; IMMEDIATE 
\ LOAD
: LOCALS| POSTPONE LOCALS| ; IMMEDIATE 
: LOOP POSTPONE LOOP ; IMMEDIATE 
: LSHIFT LSHIFT ; 
: M* M* ; 
: M*/ M*/ ; 
: M+ M+ ; 
: MARKER MARKER ; 
: MAX MAX ; 
: MIN MIN ; 
: MOD MOD ; 
: MOVE MOVE ; 
: MS MS ; 
: NEGATE NEGATE ; 
: NIP NIP ; 
: OF POSTPONE OF ; IMMEDIATE 
: ONLY STANDARD 1 SET-ORDER ; 
: OPEN-FILE OPEN-FILE ; 
: OR OR ; 
: ORDER ORDER ; 
: OVER OVER ; 
: PAD PAD ; 
: PAGE PAGE ; 
: PARSE PARSE ; 
: PICK PICK ; 
: POSTPONE POSTPONE POSTPONE ; IMMEDIATE 
: PRECISION PRECISION ; 
: PREVIOUS PREVIOUS ; 
: QUIT QUIT ; 
: R/O R/O ; 
: R/W R/W ; 
: R> POSTPONE R> ; IMMEDIATE
: R@ POSTPONE R@ ; IMMEDIATE
: READ-FILE READ-FILE ; 
: READ-LINE READ-LINE ; 
: RECURSE POSTPONE RECURSE ; IMMEDIATE 
: REFILL REFILL ; 
: RENAME-FILE RENAME-FILE ; 
: REPEAT POSTPONE REPEAT ; IMMEDIATE 
: REPOSITION-FILE REPOSITION-FILE ; 
: REPRESENT REPRESENT ; 
: RESIZE RESIZE ; 
: RESIZE-FILE RESIZE-FILE ; 
: RESTORE-INPUT RESTORE-INPUT ; 
: ROLL ROLL ; 
: ROT ROT ; 
: RSHIFT RSHIFT ; 
: S" STATE @ IF POSTPONE S" ELSE ['] S" EXECUTE THEN ; IMMEDIATE 
: S>D S>D ; 
\ SAVE-BUFFERS
: SAVE-INPUT SAVE-INPUT ; 
\ SCR
: SEARCH SEARCH ; 
: SEARCH-WORDLIST SEARCH-WORDLIST ; 
: SEE SEE ; 
: SET-CURRENT SET-CURRENT ; 
: SET-ORDER SET-ORDER ; 
: SET-PRECISION SET-PRECISION ; 
: SF! SF! ; 
: SF@ SF@ ; 
: SFALIGN SFALIGN ; 
: SFALIGNED SFALIGNED ; 
: SFLOAT+ SFLOAT+ ; 
: SFLOATS SFLOATS ; 
: SIGN SIGN ; 
: SLITERAL POSTPONE SLITERAL ; IMMEDIATE 
: SM/REM SM/REM ; 
: SOURCE SOURCE ; 
: SOURCE-ID SOURCE-ID ; 
: SPACE SPACE ; 
: SPACES SPACES ; 
: STATE STATE ; 
: SWAP SWAP ; 
: THEN POSTPONE THEN ; IMMEDIATE 
: THROW THROW ; 
\ THRU
: TIME&DATE TIME&DATE ; 
: TO STATE @ IF POSTPONE TO ELSE ['] TO EXECUTE THEN ; IMMEDIATE 
: TRUE TRUE ; 
: TUCK TUCK ; 
: TYPE TYPE ; 
: U. U. ; 
: U.R U.R ; 
: U< U< ; 
: U> U> ; 
: UM* UM* ; 
: UM/MOD UM/MOD ; 
: UNLOOP POSTPONE UNLOOP ; IMMEDIATE
: UNTIL POSTPONE UNTIL ; IMMEDIATE 
: UNUSED UNUSED ; 
\ UPDATE
: VALUE VALUE ; 
: VARIABLE VARIABLE ; 
: W/O W/O ; 
: WHILE POSTPONE WHILE ; IMMEDIATE 
: WITHIN WITHIN ; 
: WORD WORD ; 
: WORDLIST WORDLIST ; 
: WORDS WORDS ; 
: WRITE-FILE WRITE-FILE ; 
: WRITE-LINE WRITE-LINE ; 
: XOR XOR ; 
: [ POSTPONE [ ; IMMEDIATE 
: ['] POSTPONE ['] ; IMMEDIATE 
: [CHAR] POSTPONE [CHAR] ; IMMEDIATE 
: [COMPILE] POSTPONE [COMPILE] ; IMMEDIATE 
: [ELSE] POSTPONE [ELSE] ; IMMEDIATE 
: [IF] POSTPONE [IF] ; IMMEDIATE 
: [THEN] POSTPONE [THEN] ; IMMEDIATE 
: \ POSTPONE \ ; IMMEDIATE 
: ] ] ; 

STANDARD 1 SET-ORDER
