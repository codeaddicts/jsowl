using System;
using System.IO;
using System.Text;
using System.Linq;
using log = System.Console;

namespace libjsowl
{
	public class Preprocessor
	{
		public Preprocessor (CompilerOptions options)
		{
		}

		private void Process (string src) {
			StringBuilder sb = new StringBuilder ();

			string[] lines = src.Split (new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < lines.Length; i++) {
				if (lines [i].TrimStart ().StartsWith ("#")) {
					string[] directive = lines [i].Trim ().Remove (0, 1).Split (' ');

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
					case "plug":
						log.Write ("[Preprocessor] The plug directive is not yet implemented.\n");
						break;

					}
				} else {
					sb.AppendLine (lines [i]);
				}
			}

			src = sb.ToString ();
		}
	}
}

