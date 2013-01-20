using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;

namespace JSBasic.Compiler.Nodes
{
	internal class ExpressionNode : GenericJsBasicNode
	{


		public ExpressionNode(AstNodeArgs args)
			: base(args)
		{
		}

	}
}
