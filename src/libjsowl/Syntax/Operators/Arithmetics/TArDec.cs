using System;

namespace libjsowl
{
	public class TArDec : Token
	{
		public TArDec (int line) : base (line, "Decrement")
		{
		}

		public override string ToString ()
		{
			return string.Format ("--");
		}
	}
}

