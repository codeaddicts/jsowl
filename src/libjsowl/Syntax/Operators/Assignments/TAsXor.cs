using System;

namespace libjsowl
{
	public class TAsXor : Token
	{
		public TAsXor (int line) : base (line, "Bitwise xor w/ assigment")
		{
		}

		public override string ToString ()
		{
			return string.Format ("^=");
		}
	}
}

