using System;

namespace libjsowl
{
	public class TLogEq : Token
	{
		public TLogEq (int line) : base (line, "Logical equals")
		{
		}

		public override string ToString ()
		{
			return string.Format ("==");
		}
	}
}

