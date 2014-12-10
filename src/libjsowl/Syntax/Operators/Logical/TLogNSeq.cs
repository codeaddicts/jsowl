using System;

namespace libjsowl
{
	public class TLogNSeq : Token
	{
		public TLogNSeq (int line) : base (line)
		{
		}

		public override string ToString ()
		{
			return string.Format ("!==");
		}
	}
}

