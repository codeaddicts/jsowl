using System;

namespace libjsowl
{
	public class TBrL : Token
	{
		public TBrL (int line) : base (line, "Opening bracket")
		{
		}

		public override string ToString ()
		{
			return string.Format ("[");
		}
	}
}

