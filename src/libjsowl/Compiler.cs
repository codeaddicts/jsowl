using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Security.Permissions;

namespace libjsowl
{
	public class Compiler
	{
		// Private fields
		private CompilerOptions options;
		private Lexer lexer;
		private CodeGen generator;
		private Beautifier sexifier;
		private Thread mainThread;
		private Thread compilerThread;

		// Volatile private fields
		private volatile bool abort;
		private volatile bool finished;

		// Public fields
		public string value { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="libjsowl.Compiler"/> class.
		/// </summary>
		/// <param name="options">Compiler options.</param>
		public Compiler (CompilerOptions options = default(CompilerOptions)) {
			this.abort = false;
			this.finished = false;
			this.options = options;
			this.lexer = new Lexer (HandleTermination);
			this.generator = new CodeGen (options);
			this.sexifier = new Beautifier ();

			if ((options & CompilerOptions.Verbose_Lexer) == CompilerOptions.Verbose_Lexer)
				this.lexer.verbose = true;

			LogOptions ();
		}

		/// <summary>
		/// Handles the termination of a compiler part.
		/// </summary>
		/// <param name="reason">Reason.</param>
		private void HandleTermination (string reason) {
			this.abort = true;
		}

		/// <summary>
		/// Compiles the specified source file.
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="output">Output.</param>
		public void Start (string input, string output) {
			compilerThread = new Thread (call => Compile (input, output));
			mainThread = new Thread (MainThread);
			mainThread.Start ();
			compilerThread.Start ();
			mainThread.Join ();
		}

		[SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
		private void MainThread () {
			while (!finished) {
				if (abort) {
					finished = true;
					compilerThread.Abort ();
					Log ("One of the compiler components terminated unexpectedly.");
				}
			}
		}

		/// <summary>
		/// Compiles the specified source file.
		/// </summary>
		/// <param name="input">Path to the source file. Both relative and absolute paths will be fine.</param>
		/// <param name="output">Path to the output file. Both relative and absolute paths will be fine.</param> 
		private void Compile (string input, string output) {
			long tlex = 0, tgen = 0, tsexify = 0, ttotal = 0;

			// Create a stopwatch to measure the time the
			// compiler takes to do specific operations.
			Stopwatch watch = new Stopwatch ();

			// Lexical analysis
			Log ("Starting lexical analysis.");
			watch.Start ();
			lexer.FeedFile (input);
			watch.Stop ();
			tlex = watch.ElapsedMilliseconds;
			LogTime ("Lexical analysis took {0}ms.", tlex);

			// Code generation
			Log ("Starting code generation.");
			watch.Restart ();
			value = generator.Feed (lexer.tokens);
			watch.Stop ();
			tgen = watch.ElapsedMilliseconds;
			LogTime ("Code generation took {0}ms.", tgen);

			// Beautification
			Log ("Starting beautification.");
			watch.Restart ();
			value = sexifier.Feed (value);
			watch.Stop ();
			tsexify = watch.ElapsedMilliseconds;
			LogTime ("Beautification took {0}ms.", tsexify);

			// Report the total time
			ttotal = tlex + tgen + tsexify;
			LogTime ("Compilation took {0}ms in total.", ttotal);

			// We need to check for a possible pending termination here,
			// because we would probably create a huge mess in the output file otherwise.
			if (abort)
				return;

			// Write the generated source code to file
			Log ("Writing generated source code to file.");
			WriteOut (output);
			LogFile ("Done! File saved to {0}.", output);

			// Set the finished variable to true
			finished = true;
		}

		/// <summary>
		/// Writes the file to disk.
		/// </summary>
		/// <param name="path">The target path. Bot relative and absolute paths will be fine.</param>
		private void WriteOut (string path) {
			using (FileStream FILE = new FileStream (path, FileMode.Create)) {
				using (StreamWriter writer = new StreamWriter (FILE)) {
					writer.Write (value);
					writer.Flush ();
				}
			}
		}

		/// <summary>
		/// Log the specified message.
		/// </summary>
		/// <param name="msg">Message.</param>
		private void Log (string msg) {
			if ((options & CompilerOptions.Verbose_Compiler) == CompilerOptions.Verbose_Compiler)
				Console.Out.WriteLine ("[Compiler] {0}", msg);
		}

		/// <summary>
		/// Logs the specified time.
		/// </summary>
		/// <param name="msg">Message.</param>
		/// <param name="tms">Time in milliseconds.</param>
		private void LogTime (string msg, long tms) {
			if ((options & CompilerOptions.Verbose_Compiler) == CompilerOptions.Verbose_Compiler)
				Console.Out.WriteLine ("[Compiler] {0}", string.Format (msg, tms));
		}

		/// <summary>
		/// Logs the specified file path.
		/// </summary>
		/// <param name="msg">Message.</param>
		/// <param name="file">Path.</param>
		private void LogFile (string msg, string file) {
			if ((options & CompilerOptions.Verbose_Compiler) == CompilerOptions.Verbose_Compiler)
				Console.Out.WriteLine ("[Compiler] {0}", string.Format (msg, file));
		}

		/// <summary>
		/// Logs the compiler options.
		/// </summary>
		private void LogOptions () {
			if ((options & CompilerOptions.Verbose_Compiler) == CompilerOptions.Verbose_Compiler)
				Console.Out.WriteLine ("[Compiler] Flags: {0}", this.options);
		}
	}
}

