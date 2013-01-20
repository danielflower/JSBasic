using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;

namespace JSBasic.Compiler.Nodes
{
	internal class StatementNode : GenericJsBasicNode, IJSBasicNode
	{

		public StatementNode(AstNodeArgs args)
			: base(args)
		{
		}
		
		public override void GenerateJavaScript(JSContext context, System.IO.TextWriter textWriter)
		{
			base.GenerateJavaScript(context, textWriter);
			
			// put semi-colons on the end of expression-statements.
			if (ChildNodes.Count == 1)
			{
				ExpressionNode expr = ChildNodes[0] as ExpressionNode;
				if (expr != null)
				{
					textWriter.Write(";");
				}
			}
		}

	}
}
