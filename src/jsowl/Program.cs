using System;
using System.IO;
using libjsowl;

namespace jsowl
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string input = "";
			string output = "";

			CompilerOptions options = new CompilerOptions ();

			for (int i = 0; i < args.Length; i++) {
				switch (args [i]) {
				case "-i":
				case "--input":
					if (args.Length > ++i) {
						input = args [i];
					}
					break;
				case "-o":
				case "--output":
					if (args.Length > ++i) {
						output = args [i];
					}
					break;
				case "-v":
				case "-vcompiler":
					if ((options & CompilerOptions.Verbose_Compiler) != CompilerOptions.Verbose_Compiler)
						options |= CompilerOptions.Verbose_Compiler;
					break;
				case "-vlex":
				case "-vlexer":
					if ((options & CompilerOptions.Verbose_Lexer) != CompilerOptions.Verbose_Lexer)
						options |= CompilerOptions.Verbose_Lexer;
					break;
				case "-vgen":
				case "-vcodegen":
					if ((options & CompilerOptions.Verbose_CodeGen) != CompilerOptions.Verbose_CodeGen)
						options |= CompilerOptions.Verbose_CodeGen;
					break;
				case "-vbeautifier":
					if ((options & CompilerOptions.Verbose_Beautifier) != CompilerOptions.Verbose_Beautifier)
						options |= CompilerOptions.Verbose_Beautifier;
					break;
				case "-vall":
					// Unset all previous set verbose flags
					if ((options & CompilerOptions.Verbose_Compiler) == CompilerOptions.Verbose_Compiler)
						options &= ~CompilerOptions.Verbose_Compiler;
					if ((options & CompilerOptions.Verbose_Lexer) == CompilerOptions.Verbose_Lexer)
						options &= ~CompilerOptions.Verbose_Lexer;
					if ((options & CompilerOptions.Verbose_CodeGen) == CompilerOptions.Verbose_CodeGen)
						options &= ~CompilerOptions.Verbose_CodeGen;
					if ((options & CompilerOptions.Verbose_Beautifier) == CompilerOptions.Verbose_Beautifier)
						options &= ~CompilerOptions.Verbose_Beautifier;

					// Set all verbose flags at once
					options |= CompilerOptions.Verbose_All;
					break;
				case "--minify":
					if ((options & CompilerOptions.Minify) != CompilerOptions.Minify)
						options |= CompilerOptions.Minify;
					break;
				case "--keep-newlines":
				case "--preserve-newlines":
					if ((options & CompilerOptions.PreserveNewlines) != CompilerOptions.PreserveNewlines)
						options |= CompilerOptions.PreserveNewlines;
					break;
				}
			}

			// Remove CompilerOptions.PreserveNewlines if CompilerOptions.Minify is set
			if ((options & CompilerOptions.PreserveNewlines) == CompilerOptions.PreserveNewlines &&
				(options & CompilerOptions.Minify) == CompilerOptions.Minify) {
				options &= ~CompilerOptions.PreserveNewlines;
			}

			// Make sure that the user specified an input file
			if (input == "") {
				Console.WriteLine ("Usage:\t\tjsowl -i <INPUT> [-o <OUTPUT>] [OPTIONS]");
				Console.WriteLine ("Options:\n\t\t--verbose\n\t\t--minify\n\t\t--keep-newlines");
				return;
			}

			// Check if the user specified an output file.
			// If not, append ".output.js" to the input file name
			if (output == "") {
				output = string.Format ("{0}.output.js", Path.GetFileNameWithoutExtension (input));
			}

			// Create the compiler
			Compiler compiler = new Compiler (options);

			// Invoke the compiler with the specified input and output paths
			compiler.Start (input, output);
		}
	}
}
