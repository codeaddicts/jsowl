using System;

namespace libjsowl
{
	public class TLogGt : Token
	{
		public TLogGt (int line) : base (line, "Logical greater than")
		{
		}

		public override string ToString ()
		{
			return string.Format (">");
		}
	}
}

