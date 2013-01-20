/*
A DOS-like output console for JavaScript.
http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=3290305

Example usage:
var console = new Console('myConsoleDivId', 20, 40);
console.println('A console window with 20 rows and 40 characters.');
console.setCursorPosition(6,6);
console.print('Printing in row 6, column 6');

To get user-input, include the URL of PromptWindow.htm in the constructor:
var console = new Console('myConsoleDivId', 20, 40);
var yourName = console.input('What is your name? ');
console.println('Hello, ' + yourName);

*/


function Console(elementId, rows, columns, promptWindowUrl) {
/// <summary>Creates a new Console in the HTML element with given ID, with the specified rows and columns, and optionally the URL to the PromptWindow if the input() function is used.</summary>

	// Get a reference to the HTML element which will hold the console.
	this.element = document.getElementById(elementId);
	if (!this.element) {
		alert('No element with the ID ' + elementId + ' was found.');
		return;
	}
	// remove any child nodes of the element
	while (this.element.hasChildNodes()) {
		this.element.removeChild(this.element.childNodes[0]);
	}
	// make sure it acts like a 'pre'
	this.element.style.whiteSpace = 'pre';
	
	this.rows = Math.floor(rows);
	this.columns = Math.floor(columns);
	this.cursorPosition = { row: 0, column: 0 };
	this.charGrid = new Array(this.rows);
	this.promptWindowUrl = promptWindowUrl;
	
	
	// add the TextNode objects
	for (var i = 0; i < rows; i++) {
		var textNode = document.createTextNode('');
		this.charGrid[i] = textNode;
		this.element.appendChild(textNode);
		if (i < rows - 1) {
			// add a line break between each TextNode
			this.element.appendChild(document.createElement('br'));
		}
	}
	
	// clear the console screen
	this.cls();

}

Console.prototype.cls = function() {
/// <summary>Clears all the characters from the console and sets the cursor to 0,0.</summary>
	
	// go through each row
	for (var row = 0; row < this.rows; row++) {
		
		// get the text node, make a string with 'col' spaces, and set this row as the string
		var textNode = this.charGrid[row];
		var s = '';
		for (var col = 0; col < this.columns; col++) {
			s += ' ';
		}
		textNode.data = s;
	}
	// move cursor to 0,0
	this.setCursorPos(0, 0);
};

Console.prototype.printAt = function(row, column, str, cycle) {
/// <summary>Private function - not intended to be used by outside programs. Prints a string at the given row and column, and optionally wraps the text if needed.</summary>
	if (row >= this.rows || row < 0 || column < 0 || !str) {
		// nothing to print
		return;
	}
	
	// get the text in the target row
	var oldRow = this.charGrid[row].data;
	
	// tentatively put the new text for the row in newRow. This is probably too long or too short
	var newRow = oldRow.substring(0, column) + str;
	
	if (newRow.length < this.columns) {
		// the text was too short, so get the remaining characters from the old string.
		// E.g.: oldRow = "0123456789", printing "hi" over '4' so newRow = "0123hi", then appending "6789" to get "0123hi6789"
		newRow += oldRow.substring(column + str.length);
		// move the cursor to the character after the new string, e.g. just after "hi".
		this.setCursorPos(row, column + str.length);
	} else {
		// need to wrap to the next row.
		this.setCursorPos(row + 1, 0);
		if (cycle && this.cursorPosition.row >= this.rows) {
			// moved passed the bottom of the console.  Need to delete the first line, and move each line up by one.
			for (var rowIndex = 0; rowIndex < this.rows - 1; rowIndex++) {
				this.charGrid[rowIndex].data = this.charGrid[rowIndex+1].data;
			}
			// After moving up, there needs to be a new row at the bottom. Set to empty string.
			var emptyRow = '';
			for (var col = 0; col < this.columns; col++) {
				emptyRow += ' ';
			}
			this.charGrid[this.rows-1].data = emptyRow;
			// Cycled the lines up, so the current row should cycle by one as well
			this.cursorPosition.row--;
			row--;
		}
	}
	
	// truncate the text if it is too long
	if (newRow.length > this.columns) {
		newRow = newRow.substring(0, this.columns);
	}
	// set the text to the current row.
	this.charGrid[row].data = newRow;
};

Console.prototype.print = function(str) {
/// <summary>Prints the given string at the current cursor position, wrapping text where necessary.</summary>

	// get new location of cursor after text added
	var newColumnPos = this.cursorPosition.column + str.length;
	
	if (newColumnPos > this.columns) {
		// text is too long to fit on one line.  Add as much as possible, then recursively call print with the remainder of the string
		
		var charsLeftOnCurLine = this.columns - this.cursorPosition.column;
		var s = str.substring(0, charsLeftOnCurLine);
		
		// print the first part
		this.print(s);
		
		// print rest of string
		this.print(str.substring(charsLeftOnCurLine));
		
	} else {
		// print the string at the current cursor position
		this.printAt(this.cursorPosition.row, this.cursorPosition.column, str, true);
	}

};

Console.prototype.println = function(str) {
	/// <summary>Prints the given string at the current cursor position, wrapping text where necessary, and appends a line break.</summary>
	if (!str) {
		str = '';
	}
	this.print(str);
	
	// Actually, we don't add line-breaks. We simply pad out the line with whatever was 
	// there before so that the cursor will be forced to the next line.
	var extraChars = this.charGrid[this.cursorPosition.row].data.substring(this.cursorPosition.column);
	this.print(extraChars);
};

Console.prototype.setCursorPos = function(row, column) {
	/// <summary>Sets the cursor position to the given row and column.</summary>
	this.cursorPosition.row = row;
	this.cursorPosition.column = column;
};

Console.prototype.input = function(message) {
	/// <summary>Gets textual input from the user, and prints the message and the user's input to the screen.</summary>
	if (message) {
		this.print(message);
	}
	
	var result;
	if (window.showModalDialog) {
		// IE7 blocks calls to window.prompt (insanity!), so we need to use their showModalDialog
		if (!this.promptWindowUrl) {
			alert('JS Console Error\nConsole.promptWindowUrl not set. Set this to the URL of PromptWindow.htm\nPrompts disabled in Internet Explorer.');
			return '';
		}
		result = window.showModalDialog(this.promptWindowUrl, message, "dialogWidth:300px;dialogHeight:200px");
	} else {
		result = prompt(message);
	}
	
	if (result) {
	this.println(result);
		return result;
	} else {
		return '';
	}
};



