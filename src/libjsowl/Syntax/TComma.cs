using System;

namespace libjsowl
{
	public class TComma : Token
	{
		public TComma (int line) : base (line, "Comma")
		{
		}

		public override string ToString ()
		{
			return string.Format (",");
		}
	}
}

