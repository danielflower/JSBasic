using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;

namespace JSBasic.Compiler.Nodes
{

	internal class BinaryOpNode : GenericJsBasicNode, IJSBasicNode
	{

		private Token _operator;

		public BinaryOpNode(AstNodeArgs args)
			: base(args)
		{
			_operator = (Token)args.ChildNodes[0];
		}


		#region IJSBasicNode Members

		public override void GenerateJavaScript(JSContext context, System.IO.TextWriter textWriter)
		{
			string op;
			switch (_operator.Text)
			{
				case "=":
					op = "==";
					break;
				case "<>":
					op = "!=";
					break;
				case "and":
					op = "&&";
					break;
				case "or":
					op = "||";
					break;
				default:
					op = _operator.Text;
					break;
			}
			textWriter.Write(op);
		}

		#endregion
	}
}
