using System;

namespace libjsowl
{
	public class TAsMul : Token
	{
		public TAsMul (int line) : base (line, "Multiplication w/ assignment")
		{
		}

		public override string ToString ()
		{
			return string.Format ("*=");
		}
	}
}

