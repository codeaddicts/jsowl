using System;

namespace libjsowl
{
	public class TLogGtE : Token
	{
		public TLogGtE (int line) : base (line, "Logical greater than equals")
		{
		}

		public override string ToString ()
		{
			return string.Format (">=");
		}
	}
}

