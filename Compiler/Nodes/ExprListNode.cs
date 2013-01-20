using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;

namespace JSBasic.Compiler.Nodes
{
	internal class ExprListNode : GenericJsBasicNode
	{

		public IEnumerable<ExpressionNode> Expressions
		{
			get
			{
				return ChildNodes.Cast<ExpressionNode>();
			}
		}

		public ExprListNode(AstNodeArgs args)
			: base(args)
		{
		}

		public override void GenerateJavaScript(JSContext context, System.IO.TextWriter textWriter)
		{
			bool isFirst = true;
			foreach (var expr in Expressions)
			{
				if (!isFirst)
				{
					textWriter.Write("+ ");
				}
				expr.GenerateJavaScript(context, textWriter);
				isFirst = false;
			}
		}
	
	}
}
