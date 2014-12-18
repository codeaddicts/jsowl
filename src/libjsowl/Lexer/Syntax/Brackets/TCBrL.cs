using System;

namespace libjsowl
{
	public class TCBrL : Token
	{
		public TCBrL (int line) : base (line, "Opening curly bracket")
		{
		}

		public override string ToString ()
		{
			return string.Format ("{{");
		}
	}
}

