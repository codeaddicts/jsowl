using System;

namespace libjsowl
{
	public class TBitNot : Token
	{
		public TBitNot (int line) : base (line, "Bitwise NOT")
		{
		}

		public override string ToString ()
		{
			return string.Format ("~");
		}
	}
}

