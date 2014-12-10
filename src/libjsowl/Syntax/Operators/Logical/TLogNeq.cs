using System;

namespace libjsowl
{
	public class TLogNeq : Token
	{
		public TLogNeq (int line) : base (line)
		{
		}

		public override string ToString ()
		{
			return string.Format ("!=");
		}
	}
}

