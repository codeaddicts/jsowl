using System;
using System.Linq;
using System.Collections.Generic;

namespace libjsowl
{
	public class CodeGen : ICompilerComponent
	{
		// Private fields
		private string output = string.Empty;
		private int line = 0;

		// Private properties
		private bool define_main { get { return this.options.HasFlag (CompilerOptions.DefineMain); } }

		#region ICompilerBlock implementation

		public CompilerOptions options { get; set; }

		#endregion

		public CodeGen (CompilerOptions options)
		{
			this.options = options;
		}

		public string Feed (List<Token> tokens) {
			for (int i = 0; i < tokens.Count; i++) {

				// Newline
				if (tokens [i] is TEOL) {
					if ((options & CompilerOptions.PreserveNewlines) == CompilerOptions.PreserveNewlines)
						CreateNewline ();

					line++;
				} else if (tokens [i] is TIdent) {
					string ident = tokens [i].ToString ();

					// Class
					if (ident == "class") {
						if (tokens [i + 1] is TIdent) {
							string classname = tokens [++i].ToString ();

							// Create Class
							CreateClass (classname);
						}
					}

					// Public class function
					else if (ident == "public") {
						while (tokens [i + 1] is TEOL) {
							i++;
						}
						if (tokens [i + 1] is TIdent) {
							if (tokens [i + 1].ToString () == "def") {
								++i;
								if (tokens [i + 1] is TIdent) {
									string funcname = tokens [++i].ToString ();
									while (tokens [i + 1] is TEOL) {
										i++;
									}
									if (tokens [i + 1] is TCBrL) {
										// Create public function
										CreatePublicFunction (funcname);
									} else if (tokens [i + 1] is TParL) {
										// Create public parameterized function
										CreatePublicParameterizedFunction (funcname);
									}
								}
							}
						}
					}

					// Private class function
					// Not needed currently. We are handling
					// private functions using the def keyword.
					/*
					else if (ident == "private") {
						while (tokens [i + 1] is TEOL) {
							i++;
						}
						if (tokens [i + 1] is TIdent) {
							if (tokens [i + 1].ToString () == "def") {
								++i;
								if (tokens [i + 1] is TIdent) {
									string funcname = tokens [++i].ToString ();

									// Create private function
									CreatePrivateFunction (funcname);
								}
							}
						}
					}
					*/

					// Def
					else if (ident == "def") {
						while (tokens [i + 1] is TEOL) {
							i++;
						}
						if (tokens [i + 1] is TIdent) {
							string funcname = tokens [++i].ToString ();
							while (tokens [i + 1] is TEOL) {
								i++;
							}
							if (tokens [i + 1] is TCBrL) {
								// Create function
								CreateFunction (funcname);
							} else if (tokens [i + 1] is TParL) {
								// Create parameterized function
								CreateParameterizedFunction (funcname);
							}
						}
					}

					// Lambda
					else if (ident == "lambda") {
						while (tokens [i + 1] is TEOL) {
							i++;
						}
						if (tokens [i + 1] is TCBrL) {
							// Create lambda function
							CreateLambda ();
						} else if (tokens [i + 1] is TParL) {
							// Create parameterized lambda function
							CreateParameterizedLamba ();
						}
					}

					// New
					else if (ident == "new") {
						output += string.Format ("{0} ", ident);
					}

					// Let
					else if (ident == "let") {
						if (tokens [i + 1] is TIdent) {
							string varname = tokens [++i].ToString ();

							// Create Variable
							CreateVariable (varname);
						}
					}

					// Return
					else if (ident == "return") {
						output += string.Format ("{0} ", tokens [i].ToString ());
					}

					// Anything else
					else {
						output += tokens [i].ToString ();
					}
				}

				// Comments
				else if (tokens [i] is TComment) {
					CreateComment (tokens [i].ToString ());
				}

				// Multiline comments
				else if (tokens [i] is TMultilineComment) {
					CreateMultilineComment (tokens [i].ToString ());
				}

				// Numbers
				else if (tokens [i] is TNumber) {
					CreateNumber (tokens [i].ToString ());
				}

				// String literals
				else if (tokens [i] is TString) {
					string str = tokens [i].ToString ();

					// Create string literal
					CreateString (str);
				}

				// Dot
				else if (tokens [i] is TDot) {
					output += tokens [i].ToString ();
				}

				// Colon, Semicolon, Comma
				else if (
					tokens [i] is TSemi ||
					tokens [i] is TColon ||
					tokens [i] is TComma
				) {
					output += tokens [i].ToString ();
				}

				// Arithmetics
				else if (
					tokens [i] is TArAdd ||
					tokens [i] is TArSub ||
					tokens [i] is TArMul ||
					tokens [i] is TArDiv ||
					tokens [i] is TArMod
				) {
					output += string.Format (" {0} ", tokens [i].ToString ());
				}

				// Increment, decrement
				else if (
					tokens [i] is TArInc ||
					tokens [i] is TArDec
				) {
					output += string.Format ("{0}", tokens [i].ToString ());
				}

				// Bitwise arithmetics
				else if (
					tokens [i] is TBitAnd ||
					tokens [i] is TBitOr ||
					tokens [i] is TBitShL ||
					tokens [i] is TBitShR ||
					tokens [i] is TBitXor ||
					tokens [i] is TBitNot
				) {
					output += string.Format (" {0} ", tokens [i].ToString ());
				}

				// Logical stuff
				else if (
					tokens [i] is TLogAnd ||
					tokens [i] is TLogOr ||
					tokens [i] is TLogGt ||
					tokens [i] is TLogGtE ||
					tokens [i] is TLogLt ||
					tokens [i] is TLogLtE ||
					tokens [i] is TLogNot ||
					tokens [i] is TLogEq ||
					tokens [i] is TLogNeq ||
					tokens [i] is TLogSeq ||
					tokens [i] is TLogNSeq ||
					tokens [i] is TLogTernary
				) {
					output += string.Format (" {0} ", tokens [i].ToString ());
				}

				// Assignments
				else if (
					tokens [i] is TAssign ||
					tokens [i] is TAsAdd ||
					tokens [i] is TAsSub ||
					tokens [i] is TAsMul ||
					tokens [i] is TAsDiv ||
					tokens [i] is TAsAnd ||
					tokens [i] is TAsOr ||
					tokens [i] is TAsShL ||
					tokens [i] is TAsShR ||
					tokens [i] is TAsNot ||
					tokens [i] is TAsXor ||
					tokens [i] is TAsMod
				) {
					output += string.Format (" {0} ", tokens [i].ToString ());
				}

				// Brackets, Parenthesis, Braces
				else if (
					tokens [i] is TBrL ||
					tokens [i] is TBrR ||
					tokens [i] is TCBrL ||
					tokens [i] is TCBrR ||
					tokens [i] is TParL ||
					tokens [i] is TParR
				) {
					output += tokens [i].ToString ();
				}

			}

			Workify ();
			return output;
		}

		public void CreateClass (string classname) {
			output += string.Format ("function {0} () ", classname);
		}

		public void CreateVariable (string varname) {
			output += string.Format ("var {0}", varname);
		}

		public void CreateFunction (string funcname) {
			output += string.Format ("function {0} () ", funcname);
		}

		public void CreateParameterizedFunction (string funcname) {
			output += string.Format ("function {0} ", funcname);
		}

		public void CreatePrivateFunction (string funcname) {
			output += string.Format ("{0} = function ", funcname); 
		}

		public void CreatePublicFunction (string funcname) {
			output += string.Format ("this.{0} = function () ", funcname);
		}

		public void CreatePublicParameterizedFunction (string funcname) {
			output += string.Format ("this.{0} = function ", funcname);
		}

		public void CreateLambda () {
			output += string.Format ("function () ");
		}

		public void CreateParameterizedLamba () {
			output += string.Format ("function ");
		}

		public void CreateString (string str) {
			output += string.Format ("{1}{0}{1}", str, str.Contains ("\"") ? "\'" : str.Contains ("\'") ? "\"" : "'");
		}

		public void CreateComment (string comment) {
			output += string.Format ("\n//{0}\n", comment);
		}

		public void CreateMultilineComment (string comment) {
			output += string.Format ("\n/*{0}*/", comment);
		}

		public void CreateNumber (string number) {
			output += string.Format ("{0}", number);
		}

		public void CreateNewline () {
			output += '\n';
		}

		public void Workify () {
			string tmp = output;
			output = string.Empty;
			output = string.Format ("document.addEventListener(\"DOMContentLoaded\", function(_event_) {{{0}{1}}});", tmp, define_main ? "main ();" : ""); 
			//output = string.Format ("(function () {{{0}main ();}}) ();", tmp);
		}
	}
}

