using System;
using libjsowl;

namespace jsowlcgi
{
	public class Compiler
	{
		Log logger;
		Preprocessor preprocessor;
		Lexer lex;
		CodeGen gen;
		Beautifier sexify;

		public Compiler () {
			CompilerOptions options = CompilerOptions.OptimizeCGI;

			// Compiler blocks
			this.logger = new Log (options);
			this.preprocessor = new Preprocessor (options);
			this.lex = new Lexer (options, logger, (reason) => { Console.WriteLine ("// Lexer failed to tokenize the input string."); });
			this.gen = new CodeGen (options);
			this.sexify = new Beautifier (options);
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

