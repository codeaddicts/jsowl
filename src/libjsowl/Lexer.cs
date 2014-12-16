using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace libjsowl
{
	/// <summary>
	/// The jsowl tokenizer
	/// </summary>
	public class Lexer : ICompilerComponent, ILoggable, ITerminatable
	{
		// Private fields
		private SourceStream source;

		// Public properties
		public List<Token> tokens { get; private set; }

		#region ICompilerBlock implementation

		public CompilerOptions options { get; set; }

		#endregion

		#region ILoggable implementation

		public Log Logger { get; set; }
		public bool Verbose { get { return this.options.HasFlag (CompilerOptions.Verbose_Lexer); } }

		#endregion

		#region ITerminatable implementation

		public TerminationCallback terminate { get; set; }

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="libjsowl.Lexer"/> class.
		/// </summary>
		public Lexer (CompilerOptions options, Log logger, TerminationCallback terminator)
		{
			this.source = new SourceStream ();
			this.tokens = new List<Token> ();
			this.options = options;
			this.Logger = logger;
			this.terminate = terminator;
		}

		/// <summary>
		/// Tokenizes the given source code string.
		/// </summary>
		/// <param name="source">Source.</param>
		public void FeedSource (string src) {
			source.SetSource (src);
			Scan ();
		}

		/// <summary>
		/// Tokenizes the given file.
		/// </summary>
		/// <param name="path">Path.</param>
		[Obsolete ("Feeding a file to the lexer is not longer supported.", true)]
		public void FeedFile (string path) {
		}

		/// <summary>
		/// Tokenizes the source string.
		/// </summary>
		private void Scan () {
			while (source.Position < source.Length && source.Peek () != -1) {
				source.EatWhitespaces ();

				if (source.PeekChar () == '\n') {
					source.Consume ();
					tokens.Add (new TEOL (++source.Line));
					source.Column = 0;
				} else if (source.PeekChar () == '\"' || source.PeekChar () == '\'') {
					tokens.Add (ScanString ());
					LogToken ();
				} else if (char.IsLetter (source.PeekChar ()) || source.PeekChar () == '$' || source.PeekChar () == '_') {
					tokens.Add (ScanIdentifier ());
					LogToken ();
				} else if (source.PeekChar () == '/' && source.PeekChar (2) == '/') {
					tokens.Add (ScanComment ());
					LogToken ();
				} else if (source.PeekChar () == '/' && source.PeekChar (2) == '*') {
					tokens.Add (ScanMultilineComment ());
					LogToken ();
				} else if (char.IsDigit (source.PeekChar ())) {
					tokens.Add (ScanNumber ());
					LogToken ();
				} else {
					switch (source.PeekChar ()) {
					case '.':
						source.Consume ();
						tokens.Add (new TDot (source.Line));
						break;

					// Comma operator
					case ',':
						source.Consume ();
						tokens.Add (new TComma (source.Line));
						break;
					
					// Colon operator
					case ':':
						source.Consume ();
						tokens.Add (new TColon (source.Line));
						break;
					
					// End of instruction
					case ';':
						source.Consume ();
						tokens.Add (new TSemi (source.Line));
						break;
					
					// Grouping operators
					case '(':
						source.Consume ();
						tokens.Add (new TParL (source.Line));
						LogToken ();
						break;
					case ')':
						source.Consume ();
						tokens.Add (new TParR (source.Line));
						LogToken ();
						break;
					
					// Brackets
					case '{':
						source.Consume ();
						tokens.Add (new TCBrL (source.Line));
						LogToken ();
						break;
					case '}':
						source.Consume ();
						tokens.Add (new TCBrR (source.Line));
						LogToken ();
						break;
					case '[':
						source.Consume ();
						tokens.Add (new TBrL (source.Line));
						LogToken ();
						break;
					case ']':
						source.Consume ();
						tokens.Add (new TBrR (source.Line));
						LogToken ();
						break;
					
					// Assignment operator
					case '=':
						source.Consume ();

						// Equals operator
						if (source.PeekChar () == '=') {
							source.Consume ();

							// Strict equals operator
							if (source.PeekChar () == '=') {
								source.Consume ();
								tokens.Add (new TLogSeq (source.Line));
							}

							// Equals operator
							else {
								tokens.Add (new TLogEq (source.Line));
							}
						}

						// Assignment operator
						else {
							tokens.Add (new TAssign (source.Line));
						}

						LogToken ();
						break;
					
					// Addition
					case '+':
						source.Consume ();

						// Addition w/ assignment
						if (source.PeekChar () == '=') {
							source.Consume ();
							tokens.Add (new TAsAdd (source.Line));
						}

						// Increment
						else if (source.PeekChar () == '+') {
							source.Consume ();
							tokens.Add (new TArInc (source.Line));
						}

						// Addition
						else {
							tokens.Add (new TArAdd (source.Line));
						}

						LogToken ();
						break;
					
					// Subtraction
					case '-':
						source.Consume ();

						// Subtraction w/ assignment
						if (source.PeekChar () == '=') {
							source.Consume ();
							tokens.Add (new TAsSub (source.Line));
						}

						// Decrement
						else if (source.PeekChar () == '-') {
							source.Consume ();
							tokens.Add (new TArDec (source.Line));
						}

						// Subtraction
						else {
							tokens.Add (new TArSub (source.Line));
						}

						LogToken ();
						break;
					
					// Multiplication
					case '*':
						source.Consume ();

						// Multiplication w/ assignment
						if (source.PeekChar () == '=') {
							source.Consume ();
							tokens.Add (new TAsMul (source.Line));
						}

						// Multiplication
						else {
							tokens.Add (new TArMul (source.Line));
						}

						LogToken ();
						break;
					
					// Division
					case '/':
						source.Consume ();

						// Division w/ assignment
						if (source.PeekChar () == '=') {
							source.Consume ();
							tokens.Add (new TAsDiv (source.Line));
						}

						// Division
						else {
							tokens.Add (new TArDiv (source.Line));
						}

						LogToken ();
						break;
					
					// Bitwise or
					case '|':
						source.Consume ();

						// Logical or
						if (source.PeekChar () == '|') {
							source.Consume ();
							tokens.Add (new TLogOr (source.Line));
						}

						// Bitwise or w/ assignment
						else if (source.PeekChar () == '=') {
							source.Consume ();
							tokens.Add (new TAsOr (source.Line));
						}

						// Bitwise or
						else {
							tokens.Add (new TBitOr (source.Line));
						}

						LogToken ();
						break;
					
					// Bitwise and
					case '&':
						source.Consume ();

						// Logical and
						if (source.PeekChar () == '&') {
							source.Consume ();
							tokens.Add (new TLogAnd (source.Line));
						}

						// Bitwise and w/ assignment
						else if (source.PeekChar () == '=') {
							source.Consume ();
							tokens.Add (new TAsAnd (source.Line));
						}

						// Bitwise and
						else {
							tokens.Add (new TBitAnd (source.Line));
						}

						LogToken ();
						break;
					
					// Lower than
					case '<':
						source.Consume ();

						// Bitwise shift left
						if (source.PeekChar () == '<') {
							source.Consume ();

							// Bitwise shift left w/ assignment
							if (source.PeekChar () == '=') {
								source.Consume ();
								tokens.Add (new TAsShL (source.Line));
							}

							// Bitwise shift left
							else {
								tokens.Add (new TBitShL (source.Line));
							}
						}

						// Lower than or equal to
						else if (source.PeekChar () == '=') {
							source.Consume ();
							tokens.Add (new TLogLtE (source.Line));
						}

						// Lower than
						else {
							tokens.Add (new TLogLt (source.Line));
						}

						LogToken ();
						break;
					
					// Greater then
					case '>':
						source.Consume ();

						// Bitwise shift right
						if (source.PeekChar () == '>') {
							source.Consume ();

							// Bitwise shift right w/ assignment
							if (source.PeekChar () == '=') {
								source.Consume ();
								tokens.Add (new TAsShR (source.Line));
							}

							// Bitwise shift right
							else {
								tokens.Add (new TBitShR (source.Line));
							}
						}

						// Greater then or equal to
						else if (source.PeekChar () == '=') {
							source.Consume ();
							tokens.Add (new TLogGtE (source.Line));
						}

						// Greater then
						else {
							tokens.Add (new TLogGt (source.Line));
						}

						LogToken ();
						break;
					
					// Bitwise not
					case '~':
						source.Consume ();

						// Bitwise not w/ assignment
						if (source.PeekChar () == '=') {
							source.Consume ();
							tokens.Add (new TAsNot (source.Line));
						}

						// Bitwise not
						else {
							tokens.Add (new TBitNot (source.Line));
						}

						LogToken ();
						break;
					
					// Bitwise xor
					case '^':
						source.Consume ();

						// Bitwise xor w/ assignment
						if (source.PeekChar () == '=') {
							source.Consume ();
							tokens.Add (new TAsXor (source.Line));
						}

						// Bitwise xor
						else {
							tokens.Add (new TBitXor (source.Line));
						}

						LogToken ();
						break;
					
					// Logical not
					case '!':
						source.Consume ();

						// Not equal
						if (source.PeekChar () == '=') {
							source.Consume ();

							// Not strict equal
							if (source.PeekChar () == '=') {
								source.Consume ();
								tokens.Add (new TLogNSeq (source.Line));
							}

							// Not equal
							else {
								tokens.Add (new TLogNeq (source.Line));
							}
						}

						// Logical not
						else {
							tokens.Add (new TLogNot (source.Line));
						}

						LogToken ();
						break;

					// Ternary operator
					case '?':
						source.Consume ();
						tokens.Add (new TLogTernary (source.Line));
						LogToken ();
						break;
					
					// Default
					default:
						this.Error ("[Lexer] Unexpected character '{0}' at source.Line {1}:{2}. Compilation failed.\n", source.PeekChar (), source.Line, source.Column);
						terminate ("The lexical analysis failed because it hit an unimplemented character.");
						return;
					}
				}

				source.Column++;
			}
		}

		/// <summary>
		/// Tokenizes a string literal.
		/// </summary>
		/// <returns>The string.</returns>
		private TString ScanString () {
			source.Consume ();
			StringBuilder sb = new StringBuilder ();
			while (source.PeekChar () != '\"' && source.PeekChar () != '\'') {
				sb.Append (source.ReadChar ());
				source.Column++;
			}
			source.Consume ();
			return new TString (source.Line, sb.ToString ());
		}

		/// <summary>
		/// Tokenizes an identifier.
		/// </summary>
		/// <returns>The identifier.</returns>
		private TIdent ScanIdentifier () {
			StringBuilder sb = new StringBuilder ();
			sb.Append (source.ReadChar ());
			while (char.IsLetterOrDigit (source.PeekChar ()) || source.PeekChar () == '_' || source.PeekChar () == '$') {
				sb.Append (source.ReadChar ());
				source.Column++;
			}
			return new TIdent (source.Line, sb.ToString ());
		}

		/// <summary>
		/// Tokenizes a number.
		/// </summary>
		/// <returns>The number.</returns>
		private TNumber ScanNumber () {
			StringBuilder sb = new StringBuilder ();
			sb.Append (source.ReadChar ());
			source.Column++;
			while (char.IsNumber (source.PeekChar ()) || source.PeekChar () == 'E' || source.PeekChar () == 'x' || source.PeekChar () == '.') {
				sb.Append (source.ReadChar ());
				source.Column++;
			}
			return new TNumber (source.Line, sb.ToString ());
		}

		/// <summary>
		/// Tokenizes a comment.
		/// </summary>
		/// <returns>The comment.</returns>
		private TComment ScanComment () {
			StringBuilder sb = new StringBuilder ();

			source.Consume (2);
			while (source.PeekChar () != '\n') {
				sb.Append (source.ReadChar ());
			}

			return new TComment (source.Line, sb.ToString ());
		}

		/// <summary>
		/// Tokenizes a multisource.Line comment.
		/// </summary>
		/// <returns>The multisource.Line comment.</returns>
		private TMultilineComment ScanMultilineComment () {
			StringBuilder sb = new StringBuilder ();

			source.Consume (2);
			while (true) {
				if (source.PeekChar () == '*' && source.PeekChar (2) == '/') {
					break;
				}
				sb.Append (source.ReadChar ());
			}
			source.Consume (2);

			return new TMultilineComment (source.Line, sb.ToString ());
		}

		/// <summary>
		/// Logs the last token added to the token list.
		/// </summary>
		private void LogToken () {
			var tk_desc = tokens.Last ().name.PadRight (25, ' ');
			var tk_value = tokens.Last ().ToString ().Trim (' ', '\t', '\n');
			this.Log ("{0:000}:{1:000}\tToken {2}\t{3}", source.Line, source.Column, tk_desc, tk_value);
		}
	}
}

