using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;

namespace JSBasic.Compiler.Nodes
{
	internal class StatementListNode : GenericJsBasicNode, IJSBasicNode
	{

		public IEnumerable<StatementNode> Statements
		{
			get { return ChildNodes.Cast<StatementNode>(); }
		}

		public StatementListNode(AstNodeArgs args)
			: base(args)
		{
		}

		public override void GenerateJavaScript(JSContext context, System.IO.TextWriter textWriter)
		{
			bool isFirst = true;
			foreach (var stmt in Statements)
			{
				if (!isFirst)
				{
					textWriter.WriteLine();
					textWriter.Write(context.IndentationText);
				}
				stmt.GenerateJavaScript(context, textWriter);
				isFirst = false;
			}

		}

	}
}
