using System;

namespace libjsowl
{
	public class TLogGtE : Token
	{
		public TLogGtE (int line) : base (line)
		{
		}

		public override string ToString ()
		{
			return string.Format (">=");
		}
	}
}

