using System;

namespace libjsowl
{
	public class TAsNot : Token
	{
		public TAsNot (int line) : base (line, "Bitwise not w/ assignment")
		{
		}

		public override string ToString ()
		{
			return string.Format ("~=");
		}
	}
}

