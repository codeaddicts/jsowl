using System;

namespace libjsowl
{
	public abstract class Token
	{
		public readonly int line;
		public readonly string name;

		public Token (int line)
		{
			this.line = line;
			this.name = string.Empty;
		}

		public Token (int line, string name) : this (line) {
			this.name = name;
		}
	}
}

