using System;
using System.ComponentModel;

namespace libjsowl
{
	/// <summary>
	/// Compiler options.
	/// </summary>
	[DefaultValue (None), Flags ()]
	public enum CompilerOptions {
		/// <summary>
		/// No compiler options.
		/// </summary>
		None = 0x000,

		/// <summary>
		/// Preserve newlines in the generated code.
		/// </summary>
		PreserveNewlines = 0x001,

		/// <summary>
		/// Minify the generated code.
		/// Overrides the PreserveNewlines option.
		/// </summary>
		Minify = 0x002,

		/// <summary>
		/// Verbose compiler output.
		/// </summary>
		Verbose_Compiler = 0x004,

		/// <summary>
		/// Verbose lexer output.
		/// </summary>
		Verbose_Lexer = 0x008,

		/// <summary>
		/// Verbose codegen output.
		/// </summary>
		Verbose_CodeGen = 0x016,

		/// <summary>
		/// Verbose beautifier output.
		/// </summary>
		Verbose_Beautifier = 0x032,

		/// <summary>
		/// Sets all verbose flags at once.
		/// </summary>
		Verbose_All = 0x004 | 0x008 | 0x016 | 0x032,

		/// <summary>
		/// Tells the compiler to run the main-method
		/// at the start of the script.
		/// </summary>
		DefineMain = 0x064,

		/// <summary>
		/// Writes the output source into the standard out stream.
		/// </summary>
		OutputToStdout = 0x128,

		/// <summary>
		/// Reads the input source from the standard input stream.
		/// </summary>
		ReadFromStdin = 0x256,

		/// <summary>
		/// Tells the compiler to threat this as an cgi application.
		/// </summary>
		CGI = 0x512,

		/// <summary>
		/// Optimizes the compiler for use as a cgi application.
		/// Sets the flags CGI, ReadFromStdin and OutputToStdout
		/// </summary>
		OptimizeCGI = 0x128 | 0x256 | 0x512,
	}
}

