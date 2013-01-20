using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;
using JSBasic.Compiler.Nodes;
using System.IO;

namespace JSBasic.Compiler
{
	internal static class Utilities
	{

		/// <summary>
		/// An iterator to to a depth-first traversal of an Abstract-Syntax Tree.
		/// Very useful when combined with LINQ-to-Objects.
		/// </summary>
		public static IEnumerable<AstNode> DepthFirstTraversal(this AstNode startNode)
		{
			yield return startNode;
			GenericJsBasicNode childBearer = startNode as GenericJsBasicNode;
			if (childBearer != null)
			{
				foreach (AstNode child in childBearer.ChildNodes)
				{
					if (child is GenericJsBasicNode)
					{
						foreach (var item in DepthFirstTraversal(child))
						{
							yield return item;
						}
					}
					else
					{
						yield return child;
					}
				}
			}
		}

	}

	/// <summary>
	/// An error in some BASIC source code.
	/// </summary>
	public class BasicSyntaxErrorException : Exception
	{

		public BasicSyntaxErrorException() { }
		public BasicSyntaxErrorException(string message) : base(message) { }
		public BasicSyntaxErrorException(string message, Exception inner) : base(message, inner) { }
		protected BasicSyntaxErrorException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

}
