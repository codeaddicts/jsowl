using System;
using System.Text;

namespace libjsowl
{
	public class Beautifier : ICompilerBlock
	{
		#region ICompilerBlock implementation

		public CompilerOptions options { get; set; }

		#endregion

		public Beautifier (CompilerOptions options)
		{
			this.options = options;
		}

		// int pos = 0;
		// int depth = 0;

		public string Feed (string src)
		{
			JSBeautifyOptions options = new JSBeautifyOptions ();
			options.preserve_newlines = true;
			JSBeautify beautifier = new JSBeautify (src, options);
			return beautifier.GetResult ();
			/*
			StringBuilder sb = new StringBuilder ();

			while (pos < src.Length) {
				switch (src [pos]) {
				case '{':
					depth++;
					sb.AppendFormat ("{0}\n{1}", src [pos], "".PadLeft (depth, '\t'));
					break;
				case '}':
					depth--;
					sb.AppendFormat ("\n{1}{0}\n{1}", src [pos], "".PadLeft (depth, '\t'));
					break;
				case ';':
					sb.AppendFormat ("{0}\n{1}", src [pos], "".PadLeft (depth, '\t'));
					break;
				default:
					sb.Append (src [pos]);
					break;
				}
				pos++;
			}

			pos = 0;
			src = sb.ToString ();
			sb.Clear ();
			string[] lines = src.Split (new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string line in lines) {
				bool empty = true;
				for (int i = 0; i < line.Length; i++) {
					if (!char.IsWhiteSpace (line [i])) {
						empty = false;
						break;
					}
				}

				if (!empty) {
					sb.AppendLine (line);
				}
			}

			return sb.ToString ();
			*/
		}
	}
}

