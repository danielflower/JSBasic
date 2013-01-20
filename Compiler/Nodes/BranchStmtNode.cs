using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;
using System.Globalization;

namespace JSBasic.Compiler.Nodes
{
	internal class BranchStmtNode : GenericJsBasicNode, IJSBasicNode
	{

		public BranchType BranchType { get; set; }
		public int DestinationLine { get; set; }

		public BranchStmtNode(AstNodeArgs args)
			: base(args)
		{
			Token command = (Token)args.ChildNodes[0];

			if (args.ChildNodes.Count == 1)
			{
				BranchType = BranchType.Return;
			}
			else
			{
				Token line = (Token)args.ChildNodes[1];

				BranchType = (command.Text.ToLowerInvariant().Equals("goto") ? BranchType.Goto : BranchType.Gosub);
				DestinationLine = int.Parse(line.Text, CultureInfo.InvariantCulture);
			}

		}

		#region IJSBasicNode Members

		public override void GenerateJavaScript(JSContext context, System.IO.TextWriter textWriter)
		{
			if (BranchType == BranchType.Gosub)
			{
				textWriter.Write("line" + DestinationLine + "();");
			}
			else if (BranchType == BranchType.Return)
			{
				textWriter.Write("return;");
			}
			else
			{
				textWriter.Write("return line" + DestinationLine + ";");
			}
		}

		#endregion
	}


	public enum BranchType
	{
		Goto, Gosub, Return
	}

}
