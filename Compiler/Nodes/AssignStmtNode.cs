using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;

namespace JSBasic.Compiler.Nodes
{
	internal class AssignStmtNode : GenericJsBasicNode, IJSBasicNode
	{

		public Token Variable { get; set; }
		public ExpressionNode Expression { get; set; }

		public AssignStmtNode(AstNodeArgs args)
			: base(args)
		{
			Variable = (Token)args.ChildNodes[0];
			Expression = (ExpressionNode)args.ChildNodes[2];
		}

		#region IJSBasicNode Members

		public override void GenerateJavaScript(JSContext context, System.IO.TextWriter textWriter)
		{
			GeneratorHelper.GenerateNodes(context, textWriter, ChildNodes);
			textWriter.Write(";");
		}

		#endregion
	}
}
