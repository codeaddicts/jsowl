using System;

namespace libjsowl
{
	public class TAsAnd : Token
	{
		public TAsAnd (int line) : base (line, "Bitwise and w/ assignment")
		{
		}

		public override string ToString ()
		{
			return string.Format ("&=");
		}
	}
}

