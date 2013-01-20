10 cls
20 locate 5, 8
30 print " S P A C E   W A R"
40 locate 8, 2
50 print "Keys: 'A' to go up"
70 print space$(8) "'Z' to go down" : print space$(8) "<space> to shoot"
75 print : print : print space$(8) "First to 10 points wins"
80 print : print : print space$(8) "Press <space> to start"
90 if inkey$ <> " " then goto 90

100 rem Constants
110 minY$ = 3 : maxY$ = 17

200 rem Game initialisation
205 cls
210 player1y$ =  5 : player1x$ =  5 : missile1y$ = 0 : missile1x$ = 0
220 player2y$ = 15 : player2x$ = 30 : missile2y$ = 0 : missile2x$ = 0
230 player1Score$ = 0 : player2Score$ = 0 : playerToExplode$ = 0
240 gosub 400


300 rem Main game loop
301 if player1Score$ = 10 and playerToExplode$ = 0 then goto 900
302 if player2Score$ = 10 and playerToExplode$ = 0 then goto 900
305 if playerToExplode$ <> 0 then goto 1000
310 locate player1y$, player1x$
311 print ">=-"
315 locate player2y$, player2x$
316 print "-=<"
325 if missile1x$ <> 0 then gosub 750
326 if missile2x$ <> 0 then gosub 850
330 k$ = inkey$
340 if k$ = "a" and player1y$ > minY$  then gosub 610 : player1y$ = player1y$ - 1
350 if k$ = "z" and player1y$ < maxY$ then gosub 610 : player1y$ = player1y$ + 1
355 if k$ = " " then gosub 700
360 if k$ = "q" then end
369 rem Control the computer player
370 gosub 500
380 goto 300

400 rem Update scores
410 locate 1, 2
420 print "Player 1: " player1Score$
430 locate 1, 24
440 print "Player 2: " player2Score$
450 return

500 rem Artificial intelligence
510 r$ = fix(rnd * 50)
520 if r$ = 1 and player2y$ > minY$ then gosub 650 : player2y$ = player2y$ - 1
530 if r$ = 2 and player2y$ < maxY$ then gosub 650 : player2y$ = player2y$ + 1
540 if r$ = 3 or player1y$ = player2y$ then gosub 800
590 return

600 rem Sub-routines
610 rem Clear player one's ship
620 locate player1y$, player1x$
630 print "   "
640 return

650 rem Clear player two's ship
660 locate player2y$, player2x$
670 print "   "
680 return


700 rem initialise player one's missile
710 if missile1y$ <> 0 then return
720 missile1y$ = player1y$
730 missile1x$ = player1x$ + 3
740 return


750 rem Process player one's missile
752 locate missile1y$, missile1x$
753 print " "
755 if missile1x$ = player2x$ + 3 then missile1y$ = 0 : missile1x$ = 0 : return
757 missile1x$ = missile1x$ + 1
760 locate missile1y$, missile1x$
770 print "."
780 if missile1y$ = player2y$ and missile1x$ = player2x$ then missile1y$ = 0 : missile1x$ = 0 : player1Score$ = player1Score$ + 1 : playerToExplode$ = 2 : i$ = 1
790 return

800 rem initialise player two's missile  
810 if missile2y$ <> 0 then return
820 missile2y$ = player2y$
830 missile2x$ = player2x$ - 1
840 return

850 rem Process player two's missile
852 locate missile2y$, missile2x$
853 print " "
855 if missile2x$ = player1x$ - 1 then missile2y$ = 0 : missile2x$ = 0 : return
857 missile2x$ = missile2x$ - 1
860 locate missile2y$, missile2x$
870 print "."
880 if missile2y$ = player1y$ and missile2x$ >= player1x$ and missile2x$ < player1x$ + 3 then missile2y$ = 0 : missile2x$ = 0 : player2Score$ = player2Score$ + 1 : playerToExplode$ = 1 : i$ = 1
890 return

900 rem Print that a player was won
910 if player1Score$ = 10 then message$ = "PLAYER ONE WINS!!!!" else message$ = "PLAYER TWO WINS!!!!"
930 locate 7, 10 : print message$
940 locate 9, 9 : print "Press 'C' to continue"
950 if inkey$ = "c" then goto 10 else goto 950

1000 rem Explode a ship. Assumes playerToExplode$ has been set to 1 or 2
1005 if playerToExplode$ = 1 then x$ = player1x$ + 1 : y$ = player1y$ else  x$ = player2x$ + 1 : y$ = player2y$
1006 if i$ = 4 then goto 1030
1007 locate y$ - i$, x$
1008 print "*"
1009 locate y$ + i$, x$
1013 print "*"
1015 locate y$ - i$, x$ - i$ : print "."
1016 locate y$ - i$, x$ + i$ : print "."
1017 locate y$ + i$, x$ + i$ : print "."
1018 locate y$ + i$, x$ - i$ : print "."
1030 rem Tight loop execution to slow down the animation speed only
1040 for j$ = 1 to 1000
1050   locate y$, x$ - i$
1060   print string$(i$ * 2 + 1, "*")
1070 next j$
1080 i$ = i$ + 1
1090 if i$ < 5 then goto 300
1100 playerToExplode$ = 0
1110 cls
1120 gosub 400
1130 player1y$ = minY$ + 1 : player2y$ = maxY$ - 1
1140 goto 300


