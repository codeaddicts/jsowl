using System;

namespace libjsowl
{
	public class TDot : Token
	{
		public TDot (int line) : base (line, "Chaining operator")
		{
		}

		public override string ToString ()
		{
			return string.Format (".");
		}
	}
}

