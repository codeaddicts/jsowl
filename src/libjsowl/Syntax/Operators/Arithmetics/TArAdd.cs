using System;

namespace libjsowl
{
	public class TArAdd : Token
	{
		public TArAdd (int line) : base (line, "Addition")
		{
		}

		public override string ToString ()
		{
			return string.Format ("+");
		}
	}
}

