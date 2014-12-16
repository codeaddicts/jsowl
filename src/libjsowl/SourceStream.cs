using System;
using System.IO;

namespace libjsowl
{
	/// <summary>
	/// Source stream.
	/// This class is not derived from System.IO.Stream because
	/// the Stream members don't really fit its needs
	/// </summary>
	public class SourceStream
	{
		private string src;
		private int pos;
		private int lpos;
		private int line;

		/// <summary>
		/// Initializes a new instance of the <see cref="libjsowl.SourceStream"/> class.
		/// </summary>
		public SourceStream ()
		{
			src = string.Empty;
			pos = -1;
			lpos = 0;
			line = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="libjsowl.SourceStream"/> class.
		/// </summary>
		/// <param name="source">Source code.</param>
		public SourceStream (string source) {
			src = source;
			pos = -1;
			lpos = 0;
			line = 0;
		}

		/// <summary>
		/// Sets the source.
		/// </summary>
		/// <param name="source">Source.</param>
		public void SetSource (string source) {
			this.src = source;
		}

		/// <summary>
		/// Peeks at the next character in the source stream
		/// without incrementing the position.
		/// </summary>
		/// <returns>The next character in the source stream.</returns>
		/// <param name="lookahead">Lookahead.</param>
		public int Peek (int lookahead = 1) {
			return pos < src.Length - lookahead ? (int)src [pos + lookahead] : -1;
		}

		/// <summary>
		/// Peeks at the next character in the source stream
		/// without incrementing the position.
		/// </summary>
		/// <returns>The next character in the source stream.</returns>
		/// <param name="lookahead">Lookahead.</param>
		public char PeekChar (int lookahead = 1) {
			return (char)Peek (lookahead);
		}

		/// <summary>
		/// Reads a character from the source stream
		/// and increments the position.
		/// </summary>
		/// <returns>The next character in the source stream.</returns>
		public int Read () {
			lpos++;
			return pos < src.Length - 1 ? (int)src [++pos] : -1;
		}


		/// <summary>
		/// Reads a character from the source stream
		/// and increments the position
		/// </summary>
		/// <returns>The next character in the source stream.</returns>
		public char ReadChar () {
			return (char)Read ();
		}

		/// <summary>
		/// Increments the position by <paramref name="count"/>
		/// without returning anything. 
		/// </summary>
		/// <param name="count">Count.</param>
		public void Consume (int count = 1) {
			pos += count;
			lpos += count;
		}

		/// <summary>
		/// Increments the position until it hits a non-whitespace character.
		/// </summary>
		public void EatWhitespaces () {
			while (PeekChar () != '\n' && char.IsWhiteSpace (PeekChar ())) {
				Consume ();
			}
		}

		/// <summary>
		/// Gets the position.
		/// </summary>
		/// <value>The position.</value>
		public int Position { get { return this.pos; } }

		/// <summary>
		/// Gets the line.
		/// </summary>
		/// <value>The line.</value>
		public int Line { get { return this.line; } set { this.line = value; } }

		/// <summary>
		/// Gets the row.
		/// </summary>
		/// <value>The row.</value>
		public int Row { get { return Line; } }

		/// <summary>
		/// Gets the column.
		/// </summary>
		/// <value>The column.</value>
		public int Column { get { return this.lpos; } set { this.lpos = value; } }

		/// <summary>
		/// Gets the length.
		/// </summary>
		/// <value>The length.</value>
		public int Length { get { return this.src.Length; } }
	}
}

