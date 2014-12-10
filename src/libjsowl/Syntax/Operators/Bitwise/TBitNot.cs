using System;

namespace libjsowl
{
	public class TBitNot : Token
	{
		public TBitNot (int line) : base (line, "Bitwise not")
		{
		}

		public override string ToString ()
		{
			return string.Format ("~");
		}
	}
}

