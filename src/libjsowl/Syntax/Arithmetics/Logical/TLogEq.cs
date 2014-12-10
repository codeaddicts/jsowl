using System;

namespace libjsowl
{
	public class TLogEq : Token
	{
		public TLogEq (int line) : base (line)
		{
		}

		public override string ToString ()
		{
			return string.Format ("==");
		}
	}
}

