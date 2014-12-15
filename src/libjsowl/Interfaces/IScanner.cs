using System;

namespace libjsowl
{
	public interface IScanner<TToken> where TToken : Token
	{
		/// <summary>
		/// Checks if this scanner matches the current character in the source stream.
		/// </summary>
		/// <returns><c>true</c>, if this scanner can scan the current character, <c>false</c> otherwise.</returns>
		bool canScan ();
		TToken Scan ();
	}
}

