using System;

namespace libjsowl
{
	public class TLogOr : Token
	{
		public TLogOr (int line) : base (line)
		{
		}

		public override string ToString ()
		{
			return string.Format ("||");
		}
	}
}

