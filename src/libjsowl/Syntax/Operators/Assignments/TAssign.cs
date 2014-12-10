using System;

namespace libjsowl
{
	public class TAssign : Token
	{
		public TAssign (int line) : base (line, "Assignment operator")
		{
		}

		public override string ToString ()
		{
			return string.Format ("=");
		}
	}
}

