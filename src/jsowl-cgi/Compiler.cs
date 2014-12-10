using System;
using libjsowl;

namespace jsowlcgi
{
	public class Compiler
	{
		Lexer lex;
		CodeGen gen;
		Beautifier sexify;

		public Compiler () {
			lex = new Lexer ((reason) => { Console.WriteLine ("// ERROR."); });
			gen = new CodeGen (CompilerOptions.None);
			sexify = new Beautifier ();
		}

		public string Run (string content) {
			string src;
			lex.FeedSource (content);
			src = gen.Feed (lex.tokens);
			src = sexify.Feed (src);
			return src;
		}
	}
}

