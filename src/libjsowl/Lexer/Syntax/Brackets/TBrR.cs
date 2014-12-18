using System;

namespace libjsowl
{
	public class TBrR : Token
	{
		public TBrR (int line) : base (line, "Closing bracket")
		{
		}

		public override string ToString ()
		{
			return string.Format ("]");
		}
	}
}

