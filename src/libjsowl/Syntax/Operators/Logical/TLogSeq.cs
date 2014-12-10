using System;

namespace libjsowl
{
	public class TLogSeq : Token
	{
		public TLogSeq (int line) : base (line, "Strict equals")
		{
		}

		public override string ToString ()
		{
			return string.Format ("===");
		}
	}
}

