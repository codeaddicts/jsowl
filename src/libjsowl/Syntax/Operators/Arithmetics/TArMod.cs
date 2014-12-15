using System;

namespace libjsowl
{
	public class TArMod : Token
	{
		public TArMod (int line) : base (line, "Modulus")
		{
		}

		public override string ToString ()
		{
			return string.Format ("%");
		}
	}
}

