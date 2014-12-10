using System;

namespace libjsowl
{
	public class TAsDiv : Token
	{
		public TAsDiv (int line) : base (line, "Division w/ assignment")
		{
		}

		public override string ToString ()
		{
			return string.Format ("/=");
		}
	}
}

