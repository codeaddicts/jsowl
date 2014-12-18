using System;

namespace libjsowl
{
	public class TLogAnd : Token
	{
		public TLogAnd (int line) : base (line, "Logical AND")
		{
		}

		public override string ToString ()
		{
			return string.Format ("&&");
		}
	}
}

