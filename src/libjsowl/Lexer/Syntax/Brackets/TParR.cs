using System;

namespace libjsowl
{
	public class TParR : Token
	{
		public TParR (int line) : base (line, "Closing parenthesis")
		{
		}

		public override string ToString ()
		{
			return string.Format (")");
		}
	}
}

