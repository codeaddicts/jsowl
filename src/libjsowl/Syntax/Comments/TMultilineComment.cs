using System;

namespace libjsowl
{
	public class TMultilineComment : Token
	{
		private readonly string value;

		public TMultilineComment (int line, string value) : base (line, "Multiline comment")
		{
			this.value = value;
		}

		public override string ToString ()
		{
			return value;
		}
	}
}

