using System;

namespace libjsowl
{
	public class TEOF : Token
	{
		public TEOF (int line) : base (line, "End of File")
		{
		}

		public override string ToString ()
		{
			return string.Format ("[TEOF]");
		}
	}
}

