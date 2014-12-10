using System;

namespace libjsowl
{
	public class TBitXor : Token
	{
		public TBitXor (int line) : base (line, "Bitwise xor")
		{
		}

		public override string ToString ()
		{
			return string.Format ("^");
		}
	}
}

