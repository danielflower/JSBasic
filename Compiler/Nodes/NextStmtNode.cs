using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;

namespace JSBasic.Compiler.Nodes
{
	internal class NextStmtNode : GenericJsBasicNode, IJSBasicNode
	{

		public NextStmtNode(AstNodeArgs args)
			: base(args)
		{
		}

		#region IJSBasicNode Members

		public override void GenerateJavaScript(JSContext context, System.IO.TextWriter textWriter)
		{
			textWriter.WriteLine();
			context.Indentation--;
			textWriter.Write(context.IndentationText);
			textWriter.Write("}");
		}

		#endregion

	}
}
