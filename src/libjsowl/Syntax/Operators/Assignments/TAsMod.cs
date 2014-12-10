using System;

namespace libjsowl
{
	public class TAsMod : Token
	{
		public TAsMod (int line) : base (line, "Modulus w/ assignment")
		{
		}

		public override string ToString ()
		{
			return string.Format ("%=");
		}
	}
}

