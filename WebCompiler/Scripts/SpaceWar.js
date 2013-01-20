
function runSpaceWar() {
      run('console', 22, 40, line10);
}

function line10() {
	console.cls(); 
	console.setCursorPos(5 , 8 );
	console.println(" S P A C E   W A R" );
	console.setCursorPos(8 , 2 );
	console.println("Keys: 'A' to go up" );
	console.println(getSpaces((8 ))+ "'Z' to go down" );
	console.println(getSpaces((8 ))+ "<space> to shoot" );
	console.println();
	console.println();
	console.println(getSpaces((8 ))+ "First to 10 points wins" );
	console.println();
	console.println();
	console.println(getSpaces((8 ))+ "Press <space> to start" );
	return line90;
}
function line90() {
	if (getInkey() !=" " ) {
		return line90;
	}

	// Constants
	s_minY = 3 ;
	s_maxY = 17 ;
	// Game initialisation
	console.cls(); 
	s_player1y = 5 ;
	s_player1x = 5 ;
	s_missile1y = 0 ;
	s_missile1x = 0 ;
	s_player2y = 15 ;
	s_player2x = 30 ;
	s_missile2y = 0 ;
	s_missile2x = 0 ;
	s_player1Score = 0 ;
	s_player2Score = 0 ;
	s_playerToExplode = 0 ;
	line400();
	return line300;
}
function line300() {
	// Main game loop
	if (s_player1Score ==10 &&s_playerToExplode ==0 ) {
		return line900;
	}

	if (s_player2Score ==10 &&s_playerToExplode ==0 ) {
		return line900;
	}

	if (s_playerToExplode !=0 ) {
		return line1000;
	}

	console.setCursorPos(s_player1y , s_player1x );
	console.println(">=-" );
	console.setCursorPos(s_player2y , s_player2x );
	console.println("-=<" );
	if (s_missile1x !=0 ) {
		line750();
	}

	if (s_missile2x !=0 ) {
		line850();
	}

	s_k = getInkey() ;
	if (s_k =="a" &&s_player1y >s_minY ) {
		line610();
		s_player1y = s_player1y - 1 ;
	}

	if (s_k =="z" &&s_player1y <s_maxY ) {
		line610();
		s_player1y = s_player1y + 1 ;
	}

	if (s_k ==" " ) {
		line700();
	}

	if (s_k =="q" ) {
		throw "ProgramAbortException"; 
	}

	// Control the computer player
	line500();
	return line300;
	return line400;
}
function line400() {
	// Update scores
	console.setCursorPos(1 , 2 );
	console.println("Player 1: " + s_player1Score );
	console.setCursorPos(1 , 24 );
	console.println("Player 2: " + s_player2Score );
	return;
	return line500;
}
function line500() {
	// Artificial intelligence
	s_r = Math.floor((Math.random() * 50 ));
	if (s_r ==1 &&s_player2y >s_minY ) {
		line650();
		s_player2y = s_player2y - 1 ;
	}

	if (s_r ==2 &&s_player2y <s_maxY ) {
		line650();
		s_player2y = s_player2y + 1 ;
	}

	if (s_r ==3 ||s_player1y ==s_player2y ) {
		line800();
	}

	return;
	// Sub-routines
	return line610;
}
function line610() {
	// Clear player one's ship
	console.setCursorPos(s_player1y , s_player1x );
	console.println("   " );
	return;
	return line650;
}
function line650() {
	// Clear player two's ship
	console.setCursorPos(s_player2y , s_player2x );
	console.println("   " );
	return;
	return line700;
}
function line700() {
	// initialise player one's missile
	if (s_missile1y !=0 ) {
		return;
	}

	s_missile1y = s_player1y ;
	s_missile1x = s_player1x + 3 ;
	return;
	return line750;
}
function line750() {
	// Process player one's missile
	console.setCursorPos(s_missile1y , s_missile1x );
	console.println(" " );
	if (s_missile1x ==s_player2x + 3 ) {
		s_missile1y = 0 ;
		s_missile1x = 0 ;
		return;
	}

	s_missile1x = s_missile1x + 1 ;
	console.setCursorPos(s_missile1y , s_missile1x );
	console.println("." );
	if (s_missile1y ==s_player2y &&s_missile1x ==s_player2x ) {
		s_missile1y = 0 ;
		s_missile1x = 0 ;
		s_player1Score = s_player1Score + 1 ;
		s_playerToExplode = 2 ;
		s_i = 1 ;
	}

	return;
	return line800;
}
function line800() {
	// initialise player two's missile  
	if (s_missile2y !=0 ) {
		return;
	}

	s_missile2y = s_player2y ;
	s_missile2x = s_player2x - 1 ;
	return;
	return line850;
}
function line850() {
	// Process player two's missile
	console.setCursorPos(s_missile2y , s_missile2x );
	console.println(" " );
	if (s_missile2x ==s_player1x - 1 ) {
		s_missile2y = 0 ;
		s_missile2x = 0 ;
		return;
	}

	s_missile2x = s_missile2x - 1 ;
	console.setCursorPos(s_missile2y , s_missile2x );
	console.println("." );
	if (s_missile2y ==s_player1y &&s_missile2x >=s_player1x &&s_missile2x <s_player1x + 3 ) {
		s_missile2y = 0 ;
		s_missile2x = 0 ;
		s_player2Score = s_player2Score + 1 ;
		s_playerToExplode = 1 ;
		s_i = 1 ;
	}

	return;
	return line900;
}
function line900() {
	// Print that a player was won
	if (s_player1Score ==10 ) {
		s_message = "PLAYER ONE WINS!!!!" ;
	} else {
		s_message = "PLAYER TWO WINS!!!!" ;
	}

	console.setCursorPos(7 , 10 );
	console.println(s_message );
	console.setCursorPos(9 , 9 );
	console.println("Press 'C' to continue" );
	return line950;
}
function line950() {
	if (getInkey() =="c" ) {
		return line10;
	} else {
		return line950;
	}

	return line1000;
}
function line1000() {
	// Explode a ship. Assumes playerToExplode$ has been set to 1 or 2
	if (s_playerToExplode ==1 ) {
		s_x = s_player1x + 1 ;
		s_y = s_player1y ;
	} else {
		s_x = s_player2x + 1 ;
		s_y = s_player2y ;
	}

	if (s_i ==4 ) {
		return line1030;
	}

	console.setCursorPos(s_y - s_i , s_x );
	console.println("*" );
	console.setCursorPos(s_y + s_i , s_x );
	console.println("*" );
	console.setCursorPos(s_y - s_i , s_x - s_i );
	console.println("." );
	console.setCursorPos(s_y - s_i , s_x + s_i );
	console.println("." );
	console.setCursorPos(s_y + s_i , s_x + s_i );
	console.println("." );
	console.setCursorPos(s_y + s_i , s_x - s_i );
	console.println("." );
	return line1030;
}
function line1030() {
	// Tight loop execution to slow down the animation speed only
	for (var s_j = 1 ; s_j <=1000 ; s_j  += 1) {
		console.setCursorPos(s_y , s_x - s_i );
		console.println(generateString((s_i * 2 + 1 ),("*" )));
		
	}
	s_i = s_i + 1 ;
	if (s_i <5 ) {
		return line300;
	}

	s_playerToExplode = 0 ;
	console.cls(); 
	line400();
	s_player1y = s_minY + 1 ;
	s_player2y = s_maxY - 1 ;
	return line300;
}