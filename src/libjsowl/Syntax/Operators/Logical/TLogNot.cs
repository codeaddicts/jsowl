using System;

namespace libjsowl
{
	public class TLogNot : Token
	{
		public TLogNot (int line) : base (line)
		{
		}

		public override string ToString ()
		{
			return string.Format ("!");
		}
	}
}

