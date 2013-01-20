using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;
using System.IO;

namespace JSBasic.Compiler.Nodes
{
	internal class GlobalFunctionExpr : ExpressionNode
	{

		

		public Token FunctionName { get; set; }
		public ExpressionNode InputParameter1 { get; set; }
		public ExpressionNode InputParameter2 { get; set; }
		public ExpressionNode InputParameter3 { get; set; }

		public GlobalFunctionExpr(AstNodeArgs args)
			: base(args)
		{
			this.FunctionName = (Token)args.ChildNodes[0].DepthFirstTraversal().OfType<Token>().Single();
			AstNodeList funcArgs = args.ChildNodes[2].ChildNodes;
			this.InputParameter1 = (ExpressionNode)funcArgs[0];
			if (funcArgs.Count > 1)
				this.InputParameter2 = (ExpressionNode)funcArgs[1];
			if (funcArgs.Count > 2)
				this.InputParameter3 = (ExpressionNode)funcArgs[2];
		}

		public override void GenerateJavaScript(JSContext context, System.IO.TextWriter textWriter)
		{

			switch (FunctionName.Text.ToLowerInvariant()) {
				case "len":
					Write(context, textWriter, InputParameter1, ".length");
					break;
				case "left$":
					Write(context, textWriter, InputParameter1, ".substring(0, ", InputParameter2, ")");
					break;
				case "right$":
					Write(context, textWriter, InputParameter1, ".substring(", InputParameter1, ".length - ", InputParameter2, ")");
					break;
				case "mid$":
					if (InputParameter3 == null)
					{
						Write(context, textWriter, InputParameter1, ".substring(", InputParameter2, "-1)");
					}
					else
					{
						Write(context, textWriter, InputParameter1, ".substring(", InputParameter2, "-1,", InputParameter2, "-1+",InputParameter3, ")");
					}
					break;
				case "abs":
					Write(context, textWriter, "Math.abs(", InputParameter1, ")");
					break;
				case "asc":
					Write(context, textWriter, InputParameter1, ".charCodeAt(0)");
					break;
				case "chr$":
					Write(context, textWriter, "String.fromCharCode(", InputParameter1, ")");
					break;
				case "cint":
					Write(context, textWriter, "Math.round(", InputParameter1, ")");
					break;
				case "cvi":
					Write(context, textWriter, "parseInt(", InputParameter1, ")");
					break;
				case "cvs":
				case "cvd":
					Write(context, textWriter, "parseFloat(", InputParameter1, ")");
					break;
				case "exp":
					Write(context, textWriter, "Math.exp(", InputParameter1, ")");
					break;
				case "fix":
					Write(context, textWriter, "Math.floor(", InputParameter1, ")");
					break;
				case "pos":
					Write(context, textWriter, "console.cursorPosition.column");
					break;
				case "sgn":
					Write(context, textWriter, "(", InputParameter1 + " == 0 ? 0 : (", InputParameter1 , " > 0 ? 1 : -1))");
					break;
				case "sin":
					Write(context, textWriter, "Math.sin(", InputParameter1, ")");
					break;
				case "cos":
					Write(context, textWriter, "Math.cos(", InputParameter1, ")");
					break;
				case "tan":
					Write(context, textWriter, "Math.tan(", InputParameter1, ")");
					break;
				case "spc":
				case "space$":
					Write(context, textWriter, "getSpaces(", InputParameter1, ")");
					break;
				case "instr":
					if (InputParameter3 == null)
					{
						Write(context, textWriter, "(" + InputParameter1, ".indexOf(", InputParameter2, ")+1)");
					}
					else
					{
						Write(context, textWriter, "(" + InputParameter2, ".indexOf(", InputParameter3, "," + InputParameter1 + ")+1)");
					}
					break;
				case "log":
					Write(context, textWriter, "Math.log(", InputParameter1, ")");
					break;
				case "sqr":
					Write(context, textWriter, "Math.sqrt(", InputParameter1, ")");
					break;
				case "str$":
					Write(context, textWriter, "('' + ", InputParameter1, ")");
					break;
				case "string$":
					Write(context, textWriter, "generateString(", InputParameter1, ",", InputParameter2, ")");
					break;
				case "val":
					Write(context, textWriter, "parseInt(", InputParameter1, ")");
					break;
				default:
					throw new BasicSyntaxErrorException("Unknown global function " + FunctionName.Text);
			}
		}

		private static void Write(JSContext context, TextWriter textWriter, params object[] values)
		{
			foreach (object val in values)
			{
				if (val is AstNode)
				{
					textWriter.Write("(");
					GeneratorHelper.GenerateNode(context, textWriter, (AstNode)val);
					textWriter.Write(")");
				}
				else
				{
					textWriter.Write(val);
				}
			}
		}

	}
}
