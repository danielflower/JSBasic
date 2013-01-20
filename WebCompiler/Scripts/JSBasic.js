/// <reference path="Console.js" />

var console;
var terminateProgram = false;
var curLine = null;
var inkey = '';
var cancelKeyEffects = true; // set to false to allow propagation of key events to the browser

function getInkey() {
	var c = this.inkey;
	inkey = '';
	return c;
}

function setInkey(e) {
	e = (e ? e : window.event);
	var code = (e.keyCode ? e.keyCode : e.charCode);
	if (code) {
		var c = String.fromCharCode(code);
		inkey = c;
		if (cancelKeyEffects) {
			if (e.cancelable) {
				e.preventDefault();
				e.stopPropagation();
			} else {
				e.returnValue = false;
				e.cancelBubble = true;
			}
		}
	}
	
}

function generateString(numToGet, character) {
	var s = '';
	var c = character.toString().charAt(0);
	for (var i = 0; i < numToGet; i++) {
		s += c;
	}
	return s;
}

function getSpaces(numToGet) {
	return generateString(numToGet, ' ');
}

function run(consoleContainerId, consoleRows, consoleColumns, firstLine) {

	console = new Console(consoleContainerId, consoleRows, consoleColumns, "PromptWindow.htm");
	
	
	if (document.addEventListener){
		document.addEventListener('keypress', setInkey, false); 
	} else if (document.attachEvent){
		document.attachEvent('onkeypress', setInkey);
	} else {
		document.onkeydown = setInkey;
	}
	
	curLine = firstLine;
	mainLoop();

}

function mainLoop() {
	if (terminateProgram || curLine == null) {
		return;
	}
	try {
		curLine = curLine();
	} catch (e) {
		if (e == 'ProgramAbortException') {
			// do nothing, just exit
			console.println();
			console.println("Program execution stopped.");
			return;
		} else {
			throw e;
		}
	}
	setTimeout('mainLoop()', 10);
}


function terminate() {
	terminateProgram = true;
	
}


