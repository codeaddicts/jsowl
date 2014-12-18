using System;

namespace libjsowl
{
	public class TLogSeq : Token
	{
		public TLogSeq (int line) : base (line, "Logical strictly equals")
		{
		}

		public override string ToString ()
		{
			return string.Format ("===");
		}
	}
}

