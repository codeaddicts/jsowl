using System;

namespace libjsowl
{
	public class TIdent : Token
	{
		private readonly string value;

		public TIdent (int line, string value) : base (line, "Identifier")
		{
			this.value = value;
		}

		public override string ToString ()
		{
			return value;
		}
	}
}

