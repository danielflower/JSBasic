using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;
using System.Globalization;

namespace JSBasic.Compiler.Nodes
{
	internal class ForStmtNode : GenericJsBasicNode, IJSBasicNode
	{

		AssignStmtNode _assignment;
		ExpressionNode _upperBound;
		int _step;

		public ForStmtNode(AstNodeArgs args)
			: base(args)
		{
			_assignment = (AssignStmtNode)args.ChildNodes[1];
			_upperBound = (ExpressionNode)args.ChildNodes[3];
			if (args.ChildNodes.Count == 6)
			{
				_step = int.Parse(((Token)args.ChildNodes[5]).Text, CultureInfo.InvariantCulture);
			}
			else
			{
				_step = 1;
			}
		}

		#region IJSBasicNode Members

		public override void GenerateJavaScript(JSContext context, System.IO.TextWriter textWriter)
		{

			context.Indentation++;
			textWriter.Write("for (var ");
			_assignment.GenerateJavaScript(context, textWriter);
			textWriter.Write(" ");

			GeneratorHelper.GenerateNode(context, textWriter, _assignment.Variable);

			if (_step > 0)
			{
				textWriter.Write("<=");
			}
			else if (_step < 0)
			{
				textWriter.Write(">=");
			}
			else
			{
				throw new BasicSyntaxErrorException("A step amount of 0 is not allowed.");
			}

			
			_upperBound.GenerateJavaScript(context, textWriter);
			textWriter.Write("; ");
			GeneratorHelper.GenerateNode(context, textWriter, _assignment.Variable);
			textWriter.Write(" += " + _step);
			
			textWriter.Write(") {");
		}

		#endregion
	}
}
