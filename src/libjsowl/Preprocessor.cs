using System;
using System.IO;
using System.Text;
using System.Linq;
using log = System.Console;

namespace libjsowl
{
	public class Preprocessor : ICompilerBlock
	{
		#region ICompilerBlock implementation

		public CompilerOptions options { get; set; }

		#endregion

		public Preprocessor (CompilerOptions options)
		{
			this.options = options;
		}

		/// <summary>
		/// Preprocesses the specified source string.
		/// </summary>
		/// <param name="src">Source.</param>
		public string Feed (string src) {
			StringBuilder sb = new StringBuilder ();

			// Normalize line endings
			src = Normalize (src);

			// Strip the shebang
			src = StripShebang (src);

			// Split the string into lines
			string[] lines = src.Split (new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

			// Iterate through every single line
			for (int i = 0; i < lines.Length; i++) {

				// Check if the current line starts with a hash
				if (lines [i].TrimStart ().StartsWith ("#")) {

					// Get the preprocessor directive and its arguments
					string[] directive = lines [i].Trim ().Remove (0, 1).Split (' ');

					// Switch the directive identifier
					switch (directive [0]) {

					// #include
					case "include":
						if (directive.Length == 1) {
							log.Write ("[Preprocessor] Include directive is empty.\n");
							continue;
						}
						log.Write ("[Preprocessor] Including '{0}'\n", directive [1]);
						if (File.Exists (directive [1])) {
							using (FileStream FILE = new FileStream (directive [1], FileMode.Open)) {
								using (StreamReader reader = new StreamReader (FILE)) {
									sb.AppendLine (reader.ReadToEnd ());
								}
							}
						} else {
							log.Write ("[Preprocessor] Failed to include '{0}'\n", directive [1]);
							continue;
						}
						break;

						// #plug
						// This is likely to be removed in the future.
					case "plug":
						log.Write ("[Preprocessor] The plug directive is not yet implemented.\n");
						break;

					}
				} else {
					sb.AppendLine (lines [i]);
				}
			}

			return sb.ToString ();
		}

		/// <summary>
		/// Normalizes the line endings
		/// </summary>
		/// <param name="src">Source.</param>
		public string Normalize (string src) {
			return src.Replace ("\r\n", "\n");
		}

		/// <summary>
		/// Strips the shebang.
		/// </summary>
		/// <returns>The shebang.</returns>
		/// <param name="src">Source.</param>
		public string StripShebang (string src) {
			StringBuilder sb = new StringBuilder ();
			string[] parts = src.Split ('\n');
			int i = 0;
			if (parts.Length > 0 && parts [0].StartsWith ("#!")) {
				i++;
			}
			for (; i < parts.Length; i++) {
				sb.Append (parts [i]);
			}
			return sb.ToString ();
		}
	}
}

