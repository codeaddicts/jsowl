using System;

namespace libjsowl
{
	public class TLogTernary : Token
	{
		public TLogTernary (int line) : base (line, "Ternary operator")
		{
		}

		public override string ToString ()
		{
			return string.Format ("?");
		}
	}
}

