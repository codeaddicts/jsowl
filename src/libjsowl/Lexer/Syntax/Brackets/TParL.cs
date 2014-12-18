using System;

namespace libjsowl
{
	public class TParL : Token
	{
		public TParL (int line) : base (line, "Opening parenthesis")
		{
		}

		public override string ToString ()
		{
			return string.Format ("(");
		}
	}
}

