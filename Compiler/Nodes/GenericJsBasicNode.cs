using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;

namespace JSBasic.Compiler.Nodes
{

	/// <summary>
	/// The base-class for all the AST nodes in JSBasic.  This is useful only to hold
	/// a reference to all the Children, so that the AST tree can be traversed easily
	/// using an iterator.
	/// </summary>
	internal class GenericJsBasicNode : AstNode, IJSBasicNode
	{



		public GenericJsBasicNode(AstNodeArgs args)
			: base(args)
		{
		}

		#region IJSBasicNode Members

		public virtual void GenerateJavaScript(JSContext context, System.IO.TextWriter textWriter)
		{
			GeneratorHelper.GenerateNodes(context, textWriter, ChildNodes);
		}

		#endregion

	}
}
