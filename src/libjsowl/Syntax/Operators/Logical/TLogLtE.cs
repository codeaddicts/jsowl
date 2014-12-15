using System;

namespace libjsowl
{
	public class TLogLtE : Token
	{
		public TLogLtE (int line) : base (line, "Logical lower than equals")
		{
		}

		public override string ToString ()
		{
			return string.Format ("<=");
		}
	}
}

