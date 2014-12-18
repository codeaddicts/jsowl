using System;

namespace libjsowl
{
	public class TNumber : Token
	{
		private readonly string value;

		public TNumber (int line, string value) : base (line, "Number")
		{
			this.value = value;
		}

		public override string ToString ()
		{
			return value;
		}
	}
}

