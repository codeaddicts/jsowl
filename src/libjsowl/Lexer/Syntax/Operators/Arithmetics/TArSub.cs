using System;

namespace libjsowl
{
	public class TArSub : Token
	{
		public TArSub (int line) : base (line, "Subtraction")
		{
		}

		public override string ToString ()
		{
			return string.Format ("-");
		}
	}
}

