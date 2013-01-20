using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;

namespace JSBasic.Compiler.Nodes
{
	internal class InputStmtNode : GenericJsBasicNode, IJSBasicNode
	{

		private ExprListNode _toPrint;
		private Token _variable;

		public InputStmtNode(AstNodeArgs args)
			: base(args)
		{
			_toPrint = (ExprListNode)args.ChildNodes[1];
			_variable = (Token)args.ChildNodes[2];
		}

		#region IJSBasicNode Members

		public override void GenerateJavaScript(JSContext context, System.IO.TextWriter textWriter)
		{

			GeneratorHelper.GenerateNode(context, textWriter, _variable);
			textWriter.Write("= console.input(");

			if (_toPrint != null)
			{
				_toPrint.GenerateJavaScript(context, textWriter);
			}
			
			textWriter.Write(");");
		}

		#endregion
	}
}
