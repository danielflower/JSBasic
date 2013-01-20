using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;

namespace JSBasic.Compiler.Nodes
{
	internal class RemStmtNode : GenericJsBasicNode, IJSBasicNode
	{
		private string _comment;

		public RemStmtNode(AstNodeArgs args)
			: base(args)
		{
			Token token = (Token)args.ChildNodes[0];
			string text = token.Text;
			if (text.Length < 5)
			{
				_comment = "no comment";
			}
			else
			{
				_comment = text.Substring(4);
			}
		}

		public override void GenerateJavaScript(JSContext context, System.IO.TextWriter textWriter)
		{
			textWriter.Write("// " + _comment);
		}

	}
}
