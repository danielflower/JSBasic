using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace JSBasic.Compiler.Nodes
{

	/// <summary>
	/// A AST-node which can generate JavaScript code.
	/// </summary>
	public interface IJSBasicNode
	{
		void GenerateJavaScript(JSContext context, TextWriter textWriter);
	}

	/// <summary>
	/// A class to hold the current indentation information when
	/// compiling JavaScript (e.g. code in while {} statements
	/// is indented).
	/// </summary>
	public class JSContext
	{
		public int Indentation { get; set; }
		public string IndentationText
		{
			get { return new String('\t', Indentation); }
		}
	}

}
