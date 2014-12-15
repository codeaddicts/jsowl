using System;

namespace libjsowl
{
	public class TArMul : Token
	{
		public TArMul (int line) : base (line, "Multiplication")
		{
		}

		public override string ToString ()
		{
			return string.Format ("*");
		}
	}
}

