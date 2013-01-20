using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;
using System.Text.RegularExpressions;

namespace JSBasic.Compiler
{

	/// <summary>
	/// Matches tokens such as "a$", i.e. BASIC variables.
	/// </summary>
	internal class VariableIdentifierTerminal : IdentifierTerminal
	{

		static readonly Regex IdentifierPattern = new Regex("^[A-Za-z][A-Za-z0-9]*[$%!]$");

		public VariableIdentifierTerminal()
			: base("VARIABLE", "$%!", null)
		{
		}

		public override Token TryMatch(CompilerContext context, ISourceStream source)
		{
			Token token = base.TryMatch(context, source);
			if (token != null && (IdentifierPattern.IsMatch(token.Text) && !token.IsKeyword))
			{
				return token;
			}
			else
			{
				return null;
			}
			

		}

	}
}
