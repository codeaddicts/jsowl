using System;

namespace libjsowl
{
	public class TBitShR : Token
	{
		public TBitShR (int line) : base (line, "Bitwise shift right")
		{
		}

		public override string ToString ()
		{
			return string.Format (">>");
		}
	}
}

