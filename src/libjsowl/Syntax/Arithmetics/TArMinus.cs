using System;

namespace libjsowl
{
	public class TArMinus : Token
	{
		public TArMinus (int line) : base (line, "Arithmetic subtraction operator")
		{
		}

		public override string ToString ()
		{
			return string.Format ("-");
		}
	}
}

