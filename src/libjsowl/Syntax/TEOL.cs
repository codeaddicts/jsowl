using System;

namespace libjsowl
{
	public class TEOL : Token
	{
		public TEOL (int line) : base (line, "End Of Line")
		{
		}

		public override string ToString ()
		{
			return string.Format ("[TEOL]");
		}
	}
}

