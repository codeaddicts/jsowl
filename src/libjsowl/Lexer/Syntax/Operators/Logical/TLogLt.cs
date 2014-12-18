using System;

namespace libjsowl
{
	public class TLogLt : Token
	{
		public TLogLt (int line) : base (line, "Logical lower than")
		{
		}

		public override string ToString ()
		{
			return string.Format ("<");
		}
	}
}

