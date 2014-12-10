using System;

namespace libjsowl
{
	public class TBitShL : Token
	{
		public TBitShL (int line) : base (line, "Bitwise shift left")
		{
		}

		public override string ToString ()
		{
			return string.Format ("<<");
		}
	}
}

