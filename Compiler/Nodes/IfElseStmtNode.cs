using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;

namespace JSBasic.Compiler.Nodes
{
	internal class IfElseStmtNode : GenericJsBasicNode, IJSBasicNode
	{

		private IJSBasicNode _condition;
		private IJSBasicNode _thenExpression;
		private IJSBasicNode _elseExpression;

		public IfElseStmtNode(AstNodeArgs args)
			: base(args)
		{

			_condition = (IJSBasicNode)args.ChildNodes[1];
			_thenExpression = (IJSBasicNode)args.ChildNodes[3];
			//Child #4 is ELSE_CLAUSE
			AstNode elseClause = args.ChildNodes[4];
			if (elseClause.ChildNodes.Count > 0)
			{
				_elseExpression = (IJSBasicNode)elseClause.ChildNodes[1];
			}

		}

		public override void GenerateJavaScript(JSContext context, System.IO.TextWriter textWriter)
		{
			textWriter.Write("if (");
			_condition.GenerateJavaScript(context, textWriter);
			textWriter.WriteLine(") {");
			context.Indentation++;
			textWriter.Write(context.IndentationText);
			_thenExpression.GenerateJavaScript(context, textWriter);
			textWriter.WriteLine();

			context.Indentation--;
			textWriter.Write(context.IndentationText);

			if (_elseExpression != null)
			{

				textWriter.WriteLine("} else {");
				context.Indentation++;
				textWriter.Write(context.IndentationText);

				_elseExpression.GenerateJavaScript(context, textWriter);
				context.Indentation--;

				textWriter.WriteLine();
				textWriter.Write(context.IndentationText);
			}

			textWriter.WriteLine("}");

		}

	}
}
