using System;

namespace libjsowl
{
	public class TAsOr : Token
	{
		public TAsOr (int line) : base (line, "Bitwise or w/ assignment")
		{
		}

		public override string ToString ()
		{
			return string.Format ("|=");
		}
	}
}

