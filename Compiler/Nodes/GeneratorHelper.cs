using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;
using System.IO;

namespace JSBasic.Compiler.Nodes
{
	internal static class GeneratorHelper
	{

		public static void GenerateNode(JSContext context, TextWriter textWriter, AstNode node)
		{
			
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}

			if (node is IJSBasicNode)
			{
				IJSBasicNode jsBasicNode = (IJSBasicNode)node;
				jsBasicNode.GenerateJavaScript(context, textWriter);
			}
			else if (node is Token)
			{
				Token t = (Token)node;
				switch (t.Terminal.Name)
				{
					case "VARIABLE":
						if (t.Text.EndsWith("$"))
						{
							textWriter.Write("s_" + t.Text.Substring(0, t.Text.Length - 1));
						}
						else
						{
							throw new BasicSyntaxErrorException("Invalid variable name: " + t.Text);
						}
						break;
					case "BOOLEAN_OP":
						{
							switch (t.Text.ToLowerInvariant())
							{
								case "and":
									textWriter.Write("&&");
									break;
								case "or":
									textWriter.Write("||");
									break;
								default:
									textWriter.Write(t.Text);
									break;
							}
						}
						break;
					default:
						textWriter.Write(ConvertToJavascript(t.Text));
						break;
				}
				textWriter.Write(" ");
			}
			else
			{
				throw new ApplicationException("Unhandled statement type: " + node.GetType().FullName);
			}
		}

		private static string ConvertToJavascript(string keyword)
		{
			switch (keyword.ToLowerInvariant())
			{
				case "end":
					return "throw \"ProgramAbortException\";";
				case "cls":
					return "console.cls();";
				case "rnd":
					return "Math.random()";
				case "timer":
					return "(new Date()).getMilliseconds()";
				case "inkey$":
					return "getInkey()";
				case "csrlin":
					return "console.cursorPosition.row";
				default:
					return keyword;
			}
		}

		public static void GenerateNodes(JSContext context, TextWriter textWriter, AstNodeList nodes)
		{
			foreach (AstNode node in nodes)
			{
				GenerateNode(context, textWriter, node);
			}
		}

	}
}
