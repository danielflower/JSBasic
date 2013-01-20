using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;
using System.IO;
using JSBasic.Compiler.Nodes;
using System.Globalization;

namespace JSBasic.Compiler
{

	/// <summary>
	/// Generates JavaScript code from BASIC source code.
	/// </summary>
	public static class JavaScriptGenerator
	{

		/// <summary>
		/// Generates JavaScript based on BASIC source code, and returns a
		/// CopmileResult object containing the compiled source code if
		/// successful, or otherwise error messages.
		/// </summary>
		public static CompileResult Generate(string sourceCode)
		{

			// Create a BASIC compiler
			BasicGrammar basicGrammer = new BasicGrammar();
			LanguageCompiler compiler = new LanguageCompiler(basicGrammer);

			// Compile the source code into an Abstract Syntax Tree.
			ProgramNode rootNode;
			try
			{
				rootNode = (ProgramNode)compiler.Parse(sourceCode + Environment.NewLine);
			}
			catch (BasicSyntaxErrorException bsee)
			{
				rootNode = null;
				compiler.Context.Errors.Add(new SyntaxError(new SourceLocation(), bsee.Message, null));
			}
			if (rootNode == null || compiler.Context.Errors.Count > 0)
			{
				// Didn't compile.  Generate an error message.
				SyntaxError error = compiler.Context.Errors[0];
				string location = string.Empty;
				if (error.Location.Line > 0 && error.Location.Column > 0)
				{
					location = "Line " + (error.Location.Line + 1) + ", column " + (error.Location.Column + 1);
				}
				string message = location + ": " + error.Message + ":" + Environment.NewLine;
				message += sourceCode.Split('\n')[error.Location.Line];
				
				// Return failure.
				return new CompileResult()
				{
					IsSuccessful = false,
					ResultMessage = message
				};
			
			}

			// Set up the types of lines (e.g. whether a given line is the first,
			// last, or internal line of a function) and get the starting function
			// to call.
			string firstFunctionName = SetLineTypes(rootNode);

			// Now generate JavaScript from an abstract syntax tree.
			string code;
			StringBuilder sb = new StringBuilder();
			using (TextWriter tw = new StringWriter(sb, CultureInfo.InvariantCulture))
			{
				rootNode.GenerateJavaScript(new JSContext(), tw);
				code = sb.ToString();
			}

			// Return the JavaScript code.
			return new CompileResult()
			{
				IsSuccessful = true,
				ResultMessage = "Successfully compiled in " + compiler.CompileTime + "ms",
				JavaScript = code,
				StartFunction = firstFunctionName
			};


		}


		private static string SetLineTypes(ProgramNode rootNode)
		{

			// Get all the destination line numbers of Branch statements (goto, gosub, and return statements).
			var branchLineNumbers = from b in rootNode.DepthFirstTraversal()
						   where b is BranchStmtNode
						   select ((BranchStmtNode)b).DestinationLine;

			// Get all the Line nodes.
			var allLines = from l in rootNode.DepthFirstTraversal()
						where l is LineNode
						select (LineNode)l;

			// Figure out the branching, i.e. creating javascript 'return' statements.
			LineNode previous = null;
			foreach (var line in allLines)
			{
				if (branchLineNumbers.Contains(line.LineNumber))
				{
					line.LineTypes ^= LineTypes.InternalLine;
					line.LineTypes |= LineTypes.FunctionStart;
					if (previous != null)
					{
						previous.LineTypes ^= LineTypes.InternalLine;
						previous.LineTypes |= LineTypes.FunctionEnd;

						// add a return statement to the current node from the previous node, unless the
						// statement on the previous line was a goto.
						var branchQuery = from n in previous.DepthFirstTraversal()
													  where n is BranchStmtNode
													  select (BranchStmtNode)n;
						BranchStmtNode previousStmt = branchQuery.FirstOrDefault();

						if (previousStmt == null || previousStmt.BranchType == BranchType.Gosub)
						{
							previous.ReturnText = "return line" + line.LineNumber + ";";
						}
					}
				}
				previous = line;
			}
			if (previous != null)
			{
				previous.LineTypes ^= LineTypes.InternalLine;
				previous.LineTypes |= LineTypes.FunctionEnd;
			}

			// Get the first line, which corresponds to the first function to call.
			LineNode firstLine = allLines.First();
			firstLine.LineTypes |= LineTypes.FunctionStart;
			allLines.Last().LineTypes |= LineTypes.FunctionEnd;

			return "line" + firstLine.LineNumber;
		}

	}

	public class CompileResult
	{
		public bool IsSuccessful { get; set; }
		public string JavaScript { get; set; }
		public string StartFunction { get; set; }
		public string ResultMessage { get; set; }
	}

}
