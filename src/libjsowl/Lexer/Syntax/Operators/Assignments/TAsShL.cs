using System;

namespace libjsowl
{
	public class TAsShL : Token
	{
		public TAsShL (int line) : base (line, "Bitwise shift left w/ assignment")
		{
		}

		public override string ToString ()
		{
			return string.Format ("<<=");
		}
	}
}

