using System;

namespace libjsowl
{
	public class TAsSub : Token
	{
		public TAsSub (int line) : base (line, "Subtraction w/ assignment")
		{
		}

		public override string ToString ()
		{
			return string.Format ("-=");
		}
	}
}

