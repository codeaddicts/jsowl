using System;

namespace libjsowl
{
	public class TArDiv : Token
	{
		public TArDiv (int line) : base (line, "Arithmetic division operator")
		{
		}

		public override string ToString ()
		{
			return string.Format ("/");
		}
	}
}

