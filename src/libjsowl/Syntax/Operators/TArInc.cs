using System;

namespace libjsowl
{
	public class TArInc : Token
	{
		public TArInc (int line) : base (line, "Increment")
		{
		}

		public override string ToString ()
		{
			return string.Format ("++");
		}
	}
}

