using System;

namespace libjsowl
{
	public class TSemi : Token
	{
		public TSemi (int line) : base (line, "Semicolon")
		{
		}

		public override string ToString ()
		{
			return string.Format (";");
		}
	}
}

