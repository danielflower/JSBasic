using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;

namespace JSBasic.Compiler.Nodes
{
	internal class PrintStmtNode : GenericJsBasicNode, IJSBasicNode
	{

		private ExprListNode _exprList;

		public PrintStmtNode(AstNodeArgs args)
			: base(args)
		{
			_exprList = (ExprListNode)args.ChildNodes[1];
		}

		#region IJSBasicNode Members

		public override void GenerateJavaScript(JSContext context, System.IO.TextWriter textWriter)
		{
			textWriter.Write("console.println(");
			_exprList.GenerateJavaScript(context, textWriter);
			textWriter.Write(");");
		}

		#endregion
	}
}
