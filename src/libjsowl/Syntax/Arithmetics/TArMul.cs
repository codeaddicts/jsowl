using System;

namespace libjsowl
{
	public class TArMul : Token
	{
		public TArMul (int line) : base (line, "Arithmetic multiplication operator")
		{
		}

		public override string ToString ()
		{
			return string.Format ("*");
		}
	}
}

