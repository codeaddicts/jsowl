using System;

namespace libjsowl
{
	public class TColon : Token
	{
		public TColon (int line) : base (line, "Colon")
		{
		}

		public override string ToString ()
		{
			return string.Format (":");
		}
	}
}

