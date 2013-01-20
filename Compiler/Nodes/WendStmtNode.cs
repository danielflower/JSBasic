using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;

namespace JSBasic.Compiler.Nodes
{
	internal class WendStmtNode : GenericJsBasicNode
	{

		public WendStmtNode(AstNodeArgs args)
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
