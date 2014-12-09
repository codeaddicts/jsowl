using System;

namespace libjsowl
{
	public class TCBrR : Token
	{
		public TCBrR (int line) : base (line, "Closing curly bracket")
		{
		}

		public override string ToString ()
		{
			return string.Format ("}}");
		}
	}
}

