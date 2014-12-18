using System;

namespace libjsowl
{
	public class TString : Token
	{
		private readonly string value;

		public TString (int line, string value) : base (line, "String literal")
		{
			this.value = value;
		}

		public override string ToString ()
		{
			return value;
		}
	}
}

