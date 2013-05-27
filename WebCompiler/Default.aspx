<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" ValidateRequest="false"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JSBasic</title>
	<script src="Scripts/Console.js" type="text/javascript"></script>
	<script src="Scripts/JSBasic.js" type="text/javascript"></script>
	
	<style type="text/css">
	
	body 
	{
		margin:0px; padding: 10px; background-color: White;
		font-family: Arial;
	}
	
	h1 
	{
		font-size: 17pt;
	}
	
	h2 
	{
		font-size: 15pt;
	}
	
	#console 
	{
		margin: 10px;
	}
	
	.TextArea 
	{
		width: 90%;
		height: 120px;
	}
	
	.dos
	{
		font-family: Courier New, Courier;
		background-color: Black;
		color: White;
		display:block;
		float:left;
		padding: 2px;
	}
	.commodore64
	{
		font-family: Courier;
		background-color: #4242E7;
		color: #A5A5FF;
		display:block;
		float:left;
		font-weight: bold;
		text-transform:uppercase;
		font-size:17px;
		border: solid 20px #A5A5FF;
		padding: 2px;
	}
	</style>
</head>
<body>
<form runat="server" enableviewstate="false">


	<h1>JSBasic Demo</h1>
	
	<p>
		This page allows you to enter your own BASIC program and have it converted
		to JavaScript.  You can then see your program run.
	</p>
	<p>
		For an example game written in JSBasic, see <a href="SpaceWar.htm">Space War</a>.
		To learn more about JSBasic, see the 
		<a href="http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=3290305">article at CodeProject</a>.
	</p>
	
	<h2>Step one: Enter a Basic program</h2>
	
	<p>
		Please enter a Basic program below.  For a list of commands, please refer
		to the <a href="http://www.antonis.de/qbebooks/gwbasman/">GWBASIC User's Manual</a>.
	</p>
	
	<div>
		<asp:TextBox ID="basicSourceTB" runat="server" CssClass="TextArea" 
			TabIndex="1" TextMode="MultiLine">10 print "Hello, world!"
20 goto 10</asp:TextBox>
	</div>
	
	<h2>Step two: convert to JavaScript</h2>
	
	<div>
		<asp:Button ID="convertButton" runat="server" Text="Convert BASIC to JavaScript" TabIndex="1" CssClass="Button" />
	</div>
	
	<div>
		<asp:TextBox ID="javascriptSourceTB" runat="server" CssClass="TextArea"
			TabIndex="1" TextMode="MultiLine" onkeydown="return true;" onkeypress="return true;" onkeyup="return true;" />
	</div>

	<a name="stepThree"></a>
	<h2>Step three: run your script</h2>

	<div>
		<asp:Button ID="runButton" runat="server" Text="Run Program" TabIndex="1" CssClass="Button" />		
	</div>

	<div id="console" class="commodore64"></div>
	
	<div style="clear:both;">
	<strong>Console style: </strong>
		<input type="radio" id="c64RB" checked="checked" 
		onclick="document.getElementById('console').className='commodore64';" 
		name="styleButtons" />
		<label for="c64RB">Commodore 64</label>
		<input type="radio" id="dosRB" 
		onclick="document.getElementById('console').className='dos';" 
		name="styleButtons" />
		<label for="dosRB">DOS/GWBasic</label>
	</div>

	<script type="text/javascript">
		
		function init() {
			// revive the keypress events for the input controls
			var inputs = document.getElementsByTagName('textarea');
			for (var i = 0; i < inputs.length; i++) {
				var input = inputs[i];
				input.onfocus = function () { cancelKeyEffects = false; };
				input.onblur = function () { cancelKeyEffects = true; };
			}
		}
		
		init();
		
	</script>
	
</form>
</body>
</html>
