using System;

namespace libjsowl
{
	public class TAsShR : Token
	{
		public TAsShR (int line) : base (line, "Bitwise shift right w/ assignment")
		{
		}

		public override string ToString ()
		{
			return string.Format (">>=");
		}
	}
}

