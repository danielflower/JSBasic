using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;
using JSBasic.Compiler.Nodes;

namespace JSBasic.Compiler
{

	/// <summary>
	/// This class defines the Grammar for the BASIC language.
	/// </summary>
	public class BasicGrammar : Grammar
	{

		public BasicGrammar()
		{

			#region Initialisation

			// BASIC is not case sensitive... 
			this.CaseSensitive = false;

			// By default, new-line characters are ignored.  Because BASIC uses
			// line breaks to delimit lines, we need to know where the line breaks
			// are.  The following line is required for this.
			this.TokenFilters.Add(new CodeOutlineFilter(false));

			// Define the Terminals
			Terminal number = new NumberLiteral("NUMBER");
			VariableIdentifierTerminal variable = new VariableIdentifierTerminal();
			Terminal stringLiteral = new StringLiteral("STRING", "\"", ScanFlags.None);
			//Important: do not add comment term to base.NonGrammarTerminals list - we do use this terminal in grammar rules
			Terminal comment = new CommentTerminal("Comment", "REM", "\n");
			
			Terminal comma = Symbol(",", "comma");

			// Make sure reserved keywords of the BASIC language aren't mistaken
			// for variables. Only the keywords ending with '$' could be mistaken
			// for variables.
			variable.AddKeywords(
				"inkey$", "left$", "right$", "mid$", "chr$", 
				"space$", "str$", "string$"
			);
			
			// Define the non-terminals
			NonTerminal PROGRAM = new NonTerminal("PROGRAM", typeof(ProgramNode));
			NonTerminal LINE = new NonTerminal("LINE", typeof(LineNode));
			NonTerminal STATEMENT_LIST = new NonTerminal("STATEMENT_LIST", typeof(StatementListNode));
			NonTerminal STATEMENT = new NonTerminal("STATEMENT", typeof(StatementNode));
			NonTerminal COMMAND = new NonTerminal("COMMAND", typeof(StatementNode)); //TODO: create command node
			NonTerminal PRINT_STMT = new NonTerminal("PRINT_STMT", typeof(PrintStmtNode));
			NonTerminal INPUT_STMT = new NonTerminal("INPUT_STMT", typeof(InputStmtNode));
			NonTerminal IF_STMT = new NonTerminal("IF_STMT", typeof(IfElseStmtNode)); //TODO: join IfStmtNode and IfElseStmtNode in one
			NonTerminal ELSE_CLAUSE_OPT = new NonTerminal("ELSE_CLAUSE_OPT", typeof(GenericJsBasicNode));
			NonTerminal EXPR = new NonTerminal("EXPRESSION", typeof(ExpressionNode));
			NonTerminal EXPR_LIST = new NonTerminal("EXPRESSION_LIST", typeof(ExprListNode));
			NonTerminal BINARY_OP = new NonTerminal("BINARY_OP", typeof(BinaryOpNode));
			NonTerminal BINARY_EXPR = new NonTerminal("BINARY_EXPR", typeof(GenericJsBasicNode)); //TODO: create Binary_expr node
			NonTerminal BRANCH_STMT = new NonTerminal("BRANCH_STMT", typeof(BranchStmtNode));
			NonTerminal ASSIGN_STMT = new NonTerminal("ASSIGN_STMT", typeof(AssignStmtNode));
			NonTerminal FOR_STMT = new NonTerminal("FOR_STMT", typeof(ForStmtNode));
			NonTerminal STEP_OPT = new NonTerminal("STEP_OPT", typeof(GenericJsBasicNode));  //TODO: create step specifier node
			NonTerminal NEXT_STMT = new NonTerminal("NEXT_STMT", typeof(NextStmtNode));
			NonTerminal LOCATE_STMT = new NonTerminal("LOCATE_STMT", typeof(LocateStmtNode));
			NonTerminal WHILE_STMT = new NonTerminal("WHILE_STMT", typeof(WhileStmtNode));
			NonTerminal WEND_STMT = new NonTerminal("WEND_STMT", typeof(WendStmtNode));
			NonTerminal SWAP_STMT = new NonTerminal("SWAP_STMT", typeof(SwapStmtNode));
			NonTerminal GLOBAL_FUNCTION_EXPR = new NonTerminal("GLOBAL_FUNCTION_EXPR", typeof(GlobalFunctionExpr));
			NonTerminal ARG_LIST = new NonTerminal("ARG_LIST", typeof(GenericJsBasicNode));
			NonTerminal FUNC_NAME = new NonTerminal("FUNC_NAME", typeof(GenericJsBasicNode));
			NonTerminal COMMENT_STMT = new NonTerminal("COMMENT_STMT", typeof(RemStmtNode));
			NonTerminal GLOBAL_VAR_EXPR = new NonTerminal("GLOBAL_VAR_EXPR", typeof(GenericJsBasicNode));

			// Set the PROGRAM to be the root node of BASIC programs.
			// A program is a bunch of lines
			this.Root = PROGRAM;

			#endregion

			#region Grammar declaration

			// A program is a collection of lines
			PROGRAM.Rule = MakePlusRule(PROGRAM, null, LINE);

			// A line can be an empty line, or it's a number followed by a statement list ended by a new-line.
			LINE.Rule = NewLine | number + NewLine | number + STATEMENT_LIST + NewLine;

			// A statement list is 1 or more statements separated by the ':' character
			STATEMENT_LIST.Rule = MakePlusRule(STATEMENT_LIST, Symbol(":"), STATEMENT);

			// A statement can be one of a number of types
			STATEMENT.Rule = EXPR | ASSIGN_STMT | PRINT_STMT | INPUT_STMT | IF_STMT | COMMENT_STMT
								| BRANCH_STMT | COMMAND | FOR_STMT | NEXT_STMT | LOCATE_STMT | SWAP_STMT
								| WHILE_STMT | WEND_STMT;
			// The different statements are defined here
			PRINT_STMT.Rule = "print" + EXPR_LIST;
			INPUT_STMT.Rule = "input" + EXPR_LIST + variable;
			IF_STMT.Rule = "if" + EXPR + "then" + STATEMENT_LIST + ELSE_CLAUSE_OPT;
			ELSE_CLAUSE_OPT.Rule = Empty | "else" + STATEMENT_LIST;
			BRANCH_STMT.Rule = "goto" + number | "gosub" + number | "return";
			ASSIGN_STMT.Rule = variable + "=" + EXPR;
			LOCATE_STMT.Rule = "locate" + EXPR + comma + EXPR;
			SWAP_STMT.Rule = "swap" + EXPR + comma + EXPR;
			COMMAND.Rule = Symbol("end") | "cls";
			COMMENT_STMT.Rule = comment;

			// An expression is a number, or a variable, a string, or the result of a binary comparison.
			EXPR.Rule = number | variable | stringLiteral | BINARY_EXPR
					  | GLOBAL_VAR_EXPR | GLOBAL_FUNCTION_EXPR | "(" + EXPR + ")";
			BINARY_EXPR.Rule = EXPR + BINARY_OP + EXPR;

			BINARY_OP.Rule = Symbol("+") | "-" | "*" | "/" | "=" | "<=" | ">=" | "<" | ">" | "<>" | "and" | "or";
			//let's do operator precedence right here
			RegisterOperators(50, "*", "/");
			RegisterOperators(40, "+", "-");
			RegisterOperators(30, "=", "<=", ">=", "<", ">", "<>");
			RegisterOperators(20, "and", "or");

			// Used by print and input to allow a bunch of expressions separated by whitespace, 
			// or be empty, for example:
			// print
			// print "Hi"
			// print "Hi " a$
			// All of these match "print" EXPR_LIST
			EXPR_LIST.Rule = MakeStarRule(EXPR_LIST, null, EXPR);

			FOR_STMT.Rule = "for" + ASSIGN_STMT + "to" + EXPR + STEP_OPT;
			STEP_OPT.Rule = Empty | "step" + number;
			NEXT_STMT.Rule = "next" + variable;
			WHILE_STMT.Rule = "while" + EXPR;
			WEND_STMT.Rule = "wend";

			//TODO: check number of arguments for particular function in node constructor
			GLOBAL_FUNCTION_EXPR.Rule = FUNC_NAME + "(" + ARG_LIST + ")";
			FUNC_NAME.Rule = Symbol("len") | "left$" | "mid$" | "right$" | "abs" | "asc" | "chr$" | "csrlin$"
						   | "cvi" | "cvs" | "cvd" | "exp" | "fix" | "log" | "pos" | "sgn" | "sin" | "cos" | "tan"
						   | "instr" | "space$" | "spc" | "sqr" | "str$" | "string$" | "val" | "cint";
			ARG_LIST.Rule = MakePlusRule(ARG_LIST, comma, EXPR);

			GLOBAL_VAR_EXPR.Rule = Symbol("rnd") | "timer" | "inkey$" | "csrlin";

			// By registering these strings as "punctuation", we exclude them from
			// appearing in as nodes in the compiled node tree.
			RegisterPunctuation("(", ")", ",");

			#endregion

		}//constructor

	}//class
}//namespace
