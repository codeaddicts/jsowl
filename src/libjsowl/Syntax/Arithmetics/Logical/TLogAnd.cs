using System;

namespace libjsowl
{
	public class TLogAnd : Token
	{
		public TLogAnd (int line) : base (line)
		{
		}

		public override string ToString ()
		{
			return string.Format ("&&");
		}
	}
}

