using System;

namespace libjsowl
{
	public class TBitOr : Token
	{
		public TBitOr (int line) : base (line, "Bitwise OR")
		{
		}

		public override string ToString ()
		{
			return string.Format ("|");
		}
	}
}

