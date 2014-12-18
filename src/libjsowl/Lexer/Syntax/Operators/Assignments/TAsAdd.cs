using System;

namespace libjsowl
{
	public class TAsAdd : Token
	{
		public TAsAdd (int line) : base (line, "Addition w/ assignment")
		{
		}

		public override string ToString ()
		{
			return string.Format ("+=");
		}
	}
}

