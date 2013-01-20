using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;
using System.IO;

namespace JSBasic.Compiler.Nodes
{
	internal class LocateStmtNode : GenericJsBasicNode
	{

		public AstNode TargetRow { get; set; }
		public AstNode TargetColumn { get; set; }


		public LocateStmtNode(AstNodeArgs args)
			: base(args)
		{
			TargetRow = args.ChildNodes[1];
			TargetColumn = args.ChildNodes[3];
		}


		public override void GenerateJavaScript(JSContext context, TextWriter textWriter)
		{
			textWriter.Write("console.setCursorPos(");
			GeneratorHelper.GenerateNode(context, textWriter, TargetRow);
			textWriter.Write(", ");
			GeneratorHelper.GenerateNode(context, textWriter, TargetColumn);
			textWriter.Write(");");
		}

	}
}
