using System;

namespace libjsowl
{
	public class TComment : Token
	{
		private readonly string value;

		public TComment (int line, string value) : base (line, "Singleline comment")
		{
			this.value = value;
		}

		public override string ToString ()
		{
			return value;
		}
	}
}

