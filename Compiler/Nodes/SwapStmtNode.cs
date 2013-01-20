using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;
using System.IO;

namespace JSBasic.Compiler.Nodes
{
	internal class SwapStmtNode : GenericJsBasicNode
	{

		public AstNode Variable1 { get; set; }
		public AstNode Variable2 { get; set; }


		public SwapStmtNode(AstNodeArgs args)
			: base(args)
		{
			Variable1 = args.ChildNodes[1];
			Variable2 = args.ChildNodes[3];
		}


		public override void GenerateJavaScript(JSContext context, TextWriter textWriter)
		{
			textWriter.Write("tempVar = ");
			GeneratorHelper.GenerateNode(context, textWriter, Variable1);
			textWriter.WriteLine(";");
			textWriter.Write(context.IndentationText);
			GeneratorHelper.GenerateNode(context, textWriter, Variable1);
			textWriter.Write(" = ");
			GeneratorHelper.GenerateNode(context, textWriter, Variable2);
			textWriter.WriteLine(";");
			textWriter.Write(context.IndentationText);
			GeneratorHelper.GenerateNode(context, textWriter, Variable2);
			textWriter.WriteLine(" = tempVar;");
			textWriter.Write(context.IndentationText);
			textWriter.Write("tempVar = null;");
		}

	}
}
