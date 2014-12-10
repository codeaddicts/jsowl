using System;

namespace libjsowl
{
	public class TLogGt : Token
	{
		public TLogGt (int line) : base (line)
		{
		}

		public override string ToString ()
		{
			return string.Format (">");
		}
	}
}

