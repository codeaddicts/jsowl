using System;

namespace libjsowl
{
	public class TBitAnd : Token
	{
		public TBitAnd (int line) : base (line, "Bitwise and")
		{
		}

		public override string ToString ()
		{
			return string.Format ("&");
		}
	}
}

