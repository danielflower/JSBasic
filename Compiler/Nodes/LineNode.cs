using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;
using System.Globalization;

namespace JSBasic.Compiler.Nodes
{
	internal class LineNode : GenericJsBasicNode, IJSBasicNode
	{


		public int LineNumber { get; private set; }
		public GenericJsBasicNode StatementList { get; private set; }

		public string ReturnText { get; set; }
		public LineTypes LineTypes { get; set; }

		public LineNode(AstNodeArgs args)
			: base(args)
		{
			LineTypes = LineTypes.InternalLine; // overwritten later by JavaScriptGenerator
			LineNumber = (int)((Token)args.ChildNodes[0]).Value;
			if (args.ChildNodes.Count > 2)
			{
				StatementList = (GenericJsBasicNode)args.ChildNodes[1];
			}
			else
			{
				StatementList = new GenericJsBasicNode(args); //empty node
			}
		}

		#region IJSBasicNode Members

		public override void GenerateJavaScript(JSContext context, System.IO.TextWriter textWriter)
		{
			if (LineTypes == LineTypes.None)
			{
				throw new ApplicationException("Line type not set for line " + LineNumber);
			}

			if ((LineTypes & LineTypes.FunctionStart) > 0)
			{
				textWriter.Write(context.IndentationText);
				textWriter.WriteLine("function line" + LineNumber + "() {");
				context.Indentation++;
			}


			textWriter.Write(context.IndentationText);
			StatementList.GenerateJavaScript(context, textWriter);
			textWriter.WriteLine();

			if ((LineTypes & LineTypes.FunctionEnd) > 0)
			{
				if (!string.IsNullOrEmpty(ReturnText))
				{
					textWriter.Write(context.IndentationText);
					textWriter.WriteLine(ReturnText);
				}
				context.Indentation--;
				textWriter.Write(context.IndentationText);
				textWriter.WriteLine("}");
			}

		}

		#endregion

		public override string ToString()
		{
			return base.ToString() + " (" + LineNumber + ")";
		}

	}

	[Flags]
	public enum LineTypes
	{
		None = 0,
		FunctionStart = 1,
		InternalLine = 2,
		FunctionEnd = 4
	}

}
