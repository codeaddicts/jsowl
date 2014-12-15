using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace libjsowl
{
	public class Log : ICompilerComponent
	{
		#region ICompilerBlock implementation

		public CompilerOptions options { get; set; }

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="libjsowl.Log"/> class.
		/// </summary>
		/// <param name="options">Options.</param>
		public Log (CompilerOptions options)
		{
			this.options = options;
		}

		public void Write (ILoggable log, object obj) {
			if (!log.Verbose)
				return;

			var classname = GetCallingClassName ();

			if (!options.HasFlag (CompilerOptions.CGI)) {
				Console.Write ("[{0}] {1}", classname, obj);
			}
		}

		public void Write (ILoggable log, string obj, params object[] args) {
			if (!log.Verbose)
				return;

			var classname = GetCallingClassName ();

			if (!options.HasFlag (CompilerOptions.CGI)) {
				Console.Write (string.Format ("[{0}] {1}", classname, obj), args);
			}
		}

		public void WriteLine (ILoggable log, object obj) {
			if (!log.Verbose)
				return;

			var classname = GetCallingClassName ();

			if (!options.HasFlag (CompilerOptions.CGI)) {
				Console.WriteLine ("[{0}] {1}", classname, obj);
			}
		}

		public void WriteLine (ILoggable log, string obj, params object[] args) {
			if (!log.Verbose)
				return;

			var classname = GetCallingClassName ();

			if (!options.HasFlag (CompilerOptions.CGI)) {
				Console.WriteLine (string.Format ("[{0}] {1}", classname, obj), args);
			}
		}

		public void WriteError (ILoggable log, object obj) {
			var classname = GetCallingClassName ();

			if (options.HasFlag (CompilerOptions.CGI)) {
				Console.Error.Write ("// Error in {0}: {1}", obj);
			} else {
				Console.Error.Write ("[{0}->Error] {1}", classname, obj);
			}
		}

		public void WriteError (ILoggable log, string obj, params object[] args) {
			var classname = GetCallingClassName ();

			if (options.HasFlag (CompilerOptions.CGI)) {
				Console.Error.Write (string.Format ("// Error in {0}: {1}", classname, obj), args);
			} else {
				Console.Error.Write (string.Format ("[{0}->Error] {1}", classname, obj), args);
			}
		}

		public void WriteErrorLine (ILoggable log, object obj) {
			var classname = GetCallingClassName ();

			if (options.HasFlag (CompilerOptions.CGI)) {
				Console.Error.WriteLine ("// Error in {0}: {1}", classname, obj);
			} else {
				Console.Error.WriteLine ("[{0}->Error] {1}", classname, obj);
			}
		}

		public void WriteErrorLine (ILoggable log, string obj, params object[] args) {
			var classname = GetCallingClassName ();

			if (options.HasFlag (CompilerOptions.CGI)) {
				Console.Error.WriteLine (string.Format ("// Error in {0}: {1}", classname, obj), args);
			} else {
				Console.Error.WriteLine (string.Format ("[{0}->Error] {1}", classname, obj), args);
			}
		}

		/// <summary>
		/// Gets the calling class.
		/// </summary>
		/// <returns>The calling class type.</returns>
		[MethodImpl (MethodImplOptions.NoInlining)]
		private Type GetCallingClass () {
			var stackTrace = new StackTrace();
			var stackFrame = stackTrace.GetFrames ();
			var frame = stackFrame [4];
			var method = frame.GetMethod ();
			var type = method.DeclaringType;
			return type;
		}

		/// <summary>
		/// Gets the name of the calling class.
		/// </summary>
		/// <returns>The calling class name.</returns>
		private string GetCallingClassName () {
			return GetCallingClass ().Name;
		}
	}
}

