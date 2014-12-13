using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using log = System.Console;

namespace libjsowl
{
	/// <summary>
	/// The jsowl tokenizer
	/// </summary>
	public class Lexer : ITerminatable
	{
		private int pos;
		private int line;
		private int lpos;
		private string src;
		private bool eolnix;

		public List<Token> tokens { get; private set; }
		public bool verbose { get; set; }

		#region ITerminatable implementation

		public TerminationCallback terminate { get; set; }

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="libjsowl.Lexer"/> class.
		/// </summary>
		public Lexer (TerminationCallback terminator)
		{
			pos = -1;
			line = 1;
			lpos = 0;
			src = string.Empty;
			eolnix = false;
			tokens = new List<Token> ();
			terminate = terminator;
		}

		/// <summary>
		/// Tokenizes the given source code string.
		/// </summary>
		/// <param name="source">Source.</param>
		public void FeedSource (string source) {
			src = source;
			Scan ();
		}

		/// <summary>
		/// Tokenizes the given file.
		/// </summary>
		/// <param name="path">Path.</param>
		[Obsolete ("Feeding a file to the lexer is not longer supported.", true)]
		public void FeedFile (string path) {
			var source = "";
			using (FileStream FILE = new FileStream (path, FileMode.Open)) {
				using (StreamReader reader = new StreamReader (FILE)) {
					source = reader.ReadToEnd ();
				}
			}
			FeedSource (source);
		}

		/// <summary>
		/// Normalizes the line endings of the source string.
		/// </summary>
		private void Prepare () {
			var tmp = src.Length;
			src = src.Replace ("\r\n", "\n");
			eolnix = tmp > src.Length;
		}

		/// <summary>
		/// Tokenizes the source string.
		/// </summary>
		private void Scan () {
			while (pos < src.Length && Peek () != -1) {
				EatWhitespaces ();

				if (PeekChar () == '\n') {
					Consume ();
					tokens.Add (new TEOL (++line));
					lpos = 0;
				} else if (PeekChar () == '\"' || PeekChar () == '\'') {
					tokens.Add (ScanString ());
					LogToken ();
				} else if (char.IsLetter (PeekChar ()) || PeekChar () == '$' || PeekChar () == '_') {
					tokens.Add (ScanIdentifier ());
					LogToken ();
				} else if (PeekChar () == '/' && PeekChar (2) == '/') {
					tokens.Add (ScanComment ());
					LogToken ();
				} else if (PeekChar () == '/' && PeekChar (2) == '*') {
					tokens.Add (ScanMultilineComment ());
					LogToken ();
				} else if (char.IsDigit (PeekChar ())) {
					tokens.Add (ScanNumber ());
					LogToken ();
				} else {
					switch (PeekChar ()) {
					case '.':
						Consume ();
						tokens.Add (new TDot (line));
						break;

					// Comma operator
					case ',':
						Consume ();
						tokens.Add (new TComma (line));
						break;
					
					// Colon operator
					case ':':
						Consume ();
						tokens.Add (new TColon (line));
						break;
					
					// End of instruction
					case ';':
						Consume ();
						tokens.Add (new TSemi (line));
						break;
					
					// Grouping operators
					case '(':
						Consume ();
						tokens.Add (new TParL (line));
						LogToken ();
						break;
					case ')':
						Consume ();
						tokens.Add (new TParR (line));
						LogToken ();
						break;
					
					// Brackets
					case '{':
						Consume ();
						tokens.Add (new TCBrL (line));
						LogToken ();
						break;
					case '}':
						Consume ();
						tokens.Add (new TCBrR (line));
						LogToken ();
						break;
					case '[':
						Consume ();
						tokens.Add (new TBrL (line));
						LogToken ();
						break;
					case ']':
						Consume ();
						tokens.Add (new TBrR (line));
						LogToken ();
						break;
					
					// Assignment operator
					case '=':
						Consume ();

						// Equals operator
						if (PeekChar () == '=') {
							Consume ();

							// Strict equals operator
							if (PeekChar () == '=') {
								Consume ();
								tokens.Add (new TLogSeq (line));
							}

							// Equals operator
							else {
								tokens.Add (new TLogEq (line));
							}
						}

						// Assignment operator
						else {
							tokens.Add (new TAssign (line));
						}

						LogToken ();
						break;
					
					// Addition
					case '+':
						Consume ();

						// Addition w/ assignment
						if (PeekChar () == '=') {
							Consume ();
							tokens.Add (new TAsAdd (line));
						}

						// Increment
						else if (PeekChar () == '+') {
							Consume ();
							tokens.Add (new TArInc (line));
						}

						// Addition
						else {
							tokens.Add (new TArAdd (line));
						}

						LogToken ();
						break;
					
					// Subtraction
					case '-':
						Consume ();

						// Subtraction w/ assignment
						if (PeekChar () == '=') {
							Consume ();
							tokens.Add (new TAsSub (line));
						}

						// Decrement
						else if (PeekChar () == '-') {
							Consume ();
							tokens.Add (new TArDec (line));
						}

						// Subtraction
						else {
							tokens.Add (new TArSub (line));
						}

						LogToken ();
						break;
					
					// Multiplication
					case '*':
						Consume ();

						// Multiplication w/ assignment
						if (PeekChar () == '=') {
							Consume ();
							tokens.Add (new TAsMul (line));
						}

						// Multiplication
						else {
							tokens.Add (new TArMul (line));
						}

						LogToken ();
						break;
					
					// Division
					case '/':
						Consume ();

						// Division w/ assignment
						if (PeekChar () == '=') {
							Consume ();
							tokens.Add (new TAsDiv (line));
						}

						// Division
						else {
							tokens.Add (new TArDiv (line));
						}

						LogToken ();
						break;
					
					// Bitwise or
					case '|':
						Consume ();

						// Logical or
						if (PeekChar () == '|') {
							Consume ();
							tokens.Add (new TLogOr (line));
						}

						// Bitwise or w/ assignment
						else if (PeekChar () == '=') {
							Consume ();
							tokens.Add (new TAsOr (line));
						}

						// Bitwise or
						else {
							tokens.Add (new TBitOr (line));
						}

						LogToken ();
						break;
					
					// Bitwise and
					case '&':
						Consume ();

						// Logical and
						if (PeekChar () == '&') {
							Consume ();
							tokens.Add (new TLogAnd (line));
						}

						// Bitwise and w/ assignment
						else if (PeekChar () == '=') {
							Consume ();
							tokens.Add (new TAsAnd (line));
						}

						// Bitwise and
						else {
							tokens.Add (new TBitAnd (line));
						}

						LogToken ();
						break;
					
					// Lower than
					case '<':
						Consume ();

						// Bitwise shift left
						if (PeekChar () == '<') {
							Consume ();

							// Bitwise shift left w/ assignment
							if (PeekChar () == '=') {
								Consume ();
								tokens.Add (new TAsShL (line));
							}

							// Bitwise shift left
							else {
								tokens.Add (new TBitShL (line));
							}
						}

						// Lower than or equal to
						else if (PeekChar () == '=') {
							Consume ();
							tokens.Add (new TLogLtE (line));
						}

						// Lower than
						else {
							tokens.Add (new TLogLt (line));
						}

						LogToken ();
						break;
					
					// Greater then
					case '>':
						Consume ();

						// Bitwise shift right
						if (PeekChar () == '>') {
							Consume ();

							// Bitwise shift right w/ assignment
							if (PeekChar () == '=') {
								Consume ();
								tokens.Add (new TAsShR (line));
							}

							// Bitwise shift right
							else {
								tokens.Add (new TBitShR (line));
							}
						}

						// Greater then or equal to
						else if (PeekChar () == '=') {
							Consume ();
							tokens.Add (new TLogGtE (line));
						}

						// Greater then
						else {
							tokens.Add (new TLogGt (line));
						}

						LogToken ();
						break;
					
					// Bitwise not
					case '~':
						Consume ();

						// Bitwise not w/ assignment
						if (PeekChar () == '=') {
							Consume ();
							tokens.Add (new TAsNot (line));
						}

						// Bitwise not
						else {
							tokens.Add (new TBitNot (line));
						}

						LogToken ();
						break;
					
					// Bitwise xor
					case '^':
						Consume ();

						// Bitwise xor w/ assignment
						if (PeekChar () == '=') {
							Consume ();
							tokens.Add (new TAsXor (line));
						}

						// Bitwise xor
						else {
							tokens.Add (new TBitXor (line));
						}

						LogToken ();
						break;
					
					// Logical not
					case '!':
						Consume ();

						// Not equal
						if (PeekChar () == '=') {
							Consume ();

							// Not strict equal
							if (PeekChar () == '=') {
								Consume ();
								tokens.Add (new TLogNSeq (line));
							}

							// Not equal
							else {
								tokens.Add (new TLogNeq (line));
							}
						}

						// Logical not
						else {
							tokens.Add (new TLogNot (line));
						}

						LogToken ();
						break;
					
					// Default
					default:
						log.Error.Write ("[Lexer] Unexpected character '{0}' at line {1}:{2}. Compilation failed.\n", PeekChar (), line, lpos);
						terminate ("The lexical analysis failed because it hit an unimplemented character.");
						return;
					}
				}

				lpos++;
			}
		}

		/// <summary>
		/// Tokenizes a string literal.
		/// </summary>
		/// <returns>The string.</returns>
		private TString ScanString () {
			Consume ();
			StringBuilder sb = new StringBuilder ();
			while (PeekChar () != '\"' && PeekChar () != '\'') {
				sb.Append (ReadChar ());
				lpos++;
			}
			Consume ();
			return new TString (line, sb.ToString ());
		}

		/// <summary>
		/// Tokenizes an identifier.
		/// </summary>
		/// <returns>The identifier.</returns>
		private TIdent ScanIdentifier () {
			StringBuilder sb = new StringBuilder ();
			sb.Append (ReadChar ());
			while (char.IsLetterOrDigit (PeekChar ()) || PeekChar () == '_' || PeekChar () == '$') {
				sb.Append (ReadChar ());
				lpos++;
			}
			return new TIdent (line, sb.ToString ());
		}

		/// <summary>
		/// Tokenizes a number.
		/// </summary>
		/// <returns>The number.</returns>
		private TNumber ScanNumber () {
			StringBuilder sb = new StringBuilder ();
			sb.Append (ReadChar ());
			lpos++;
			while (char.IsNumber (PeekChar ()) || PeekChar () == 'E' || PeekChar () == 'x' || PeekChar () == '.') {
				sb.Append (ReadChar ());
				lpos++;
			}
			return new TNumber (line, sb.ToString ());
		}

		/// <summary>
		/// Tokenizes a comment.
		/// </summary>
		/// <returns>The comment.</returns>
		private TComment ScanComment () {
			StringBuilder sb = new StringBuilder ();

			Consume (2);
			while (PeekChar () != '\n') {
				sb.Append (ReadChar ());
			}

			return new TComment (line, sb.ToString ());
		}

		/// <summary>
		/// Tokenizes a multiline comment.
		/// </summary>
		/// <returns>The multiline comment.</returns>
		private TMultilineComment ScanMultilineComment () {
			StringBuilder sb = new StringBuilder ();

			Consume (2);
			while (true) {
				if (PeekChar () == '*' && PeekChar (2) == '/') {
					break;
				}
				sb.Append (ReadChar ());
			}
			Consume (2);

			return new TMultilineComment (line, sb.ToString ());
		}

		/// <summary>
		/// Logs the last token added to the token list.
		/// </summary>
		private void LogToken () {
			if (verbose)
				Console.Out.WriteLine ("[Lexer] {2:000}:{3:000}\tToken {0}\t{1}",
					tokens.Last ().name.PadRight (25, ' '), tokens.Last ().ToString ().Trim (' ', '\n', '\t'), line, lpos);
		}

		/// <summary>
		/// Peeks at the next character in the source stream
		/// without incrementing the position.
		/// </summary>
		/// <returns>The next character in the source stream.</returns>
		/// <param name="lookahead">Lookahead.</param>
		private int Peek (int lookahead = 1) {
			return pos < src.Length - lookahead ? (int)src [pos + lookahead] : -1;
		}

		/// <summary>
		/// Peeks at the next character in the source stream
		/// without incrementing the position.
		/// </summary>
		/// <returns>The next character in the source stream.</returns>
		/// <param name="lookahead">Lookahead.</param>
		private char PeekChar (int lookahead = 1) {
			return (char)Peek (lookahead);
		}

		/// <summary>
		/// Reads a character from the source stream
		/// and increments the position.
		/// </summary>
		/// <returns>The next character in the source stream.</returns>
		private int Read () {
			lpos++;
			return pos < src.Length - 1 ? (int)src [++pos] : -1;
		}


		/// <summary>
		/// Reads a character from the source stream
		/// and increments the position
		/// </summary>
		/// <returns>The next character in the source stream.</returns>
		private char ReadChar () {
			return (char)Read ();
		}

		/// <summary>
		/// Increments the position by <paramref name="count"/>
		/// without returning anything. 
		/// </summary>
		/// <param name="count">Count.</param>
		private void Consume (int count = 1) {
			pos += count;
			lpos += count;
		}

		/// <summary>
		/// Increments the position until it hits a non-whitespace character.
		/// </summary>
		private void EatWhitespaces () {
			while (PeekChar () != '\n' && char.IsWhiteSpace (PeekChar ())) {
				Consume ();
			}
		}
	}
}

