using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;

namespace JSBasic.Compiler.Nodes
{
	internal class WhileStmtNode : GenericJsBasicNode
	{

		ExpressionNode _condition;

		public WhileStmtNode(AstNodeArgs args)
			: base(args)
		{
			_condition = (ExpressionNode)args.ChildNodes[1];
		}

		#region IJSBasicNode Members

		public override void GenerateJavaScript(JSContext context, System.IO.TextWriter textWriter)
		{

			context.Indentation++;
			textWriter.Write("while (");
			_condition.GenerateJavaScript(context, textWriter);
			textWriter.Write(") {");
		}

		#endregion

	}
}
