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
		None = 0x00,

		/// <summary>
		/// Preserve newlines in the generated code.
		/// </summary>
		PreserveNewlines = 0x01,

		/// <summary>
		/// Minify the generated code.
		/// Overrides the PreserveNewlines option.
		/// </summary>
		Minify = 0x02,

		/// <summary>
		/// Verbose compiler output.
		/// </summary>
		Verbose_Compiler = 0x04,

		/// <summary>
		/// Verbose lexer output.
		/// </summary>
		Verbose_Lexer = 0x08,

		/// <summary>
		/// Verbose codegen output.
		/// </summary>
		Verbose_CodeGen = 0x16,

		/// <summary>
		/// Verbose beautifier output.
		/// </summary>
		Verbose_Beautifier = 0x32,

		/// <summary>
		/// Sets all verbose flags at once.
		/// </summary>
		Verbose_All = 0x04 | 0x08 | 0x16 | 0x32,

		/// <summary>
		/// Tells the compiler to run the main-method
		/// at the start of the script
		/// </summary>
		DefineMain = 0x64
	}
}

