using System;

namespace libjsowl
{
	public class TArPlus : Token
	{
		public TArPlus (int line) : base (line, "Arithmetic addition operator")
		{
		}

		public override string ToString ()
		{
			return string.Format ("+");
		}
	}
}

