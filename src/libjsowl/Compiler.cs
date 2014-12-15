using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Security.Permissions;
using System.Text;

namespace libjsowl
{
	public class Compiler : ICompilerComponent, ILoggable
	{
		// Private fields
		private Preprocessor preprocessor;
		private Lexer lexer;
		private CodeGen generator;
		private Beautifier sexifier;
		private Thread mainThread;
		private Thread compilerThread;
		private string source;

		// Volatile private fields
		private volatile bool abort;
		private volatile bool finished;

		#region ICompilerBlock implementation

		public CompilerOptions options { get; set; }

		#endregion

		#region ILoggable implementation

		public Log Logger { get; set; }
		public bool Verbose { get { return (this.options & CompilerOptions.Verbose_Compiler) == CompilerOptions.Verbose_Compiler; } }

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="libjsowl.Compiler"/> class.
		/// </summary>
		/// <param name="options">Compiler options.</param>
		public Compiler (CompilerOptions options = default(CompilerOptions)) {
			this.abort = false;
			this.finished = false;
			this.options = options;

			// Compiler blocks
			this.Logger = new Log (this.options);
			this.preprocessor = new Preprocessor (this.options);
			this.lexer = new Lexer (this.options, this.Logger, HandleTermination);
			this.generator = new CodeGen (this.options);
			this.sexifier = new Beautifier (this.options);

			this.Log ("Flags: {0}", this.options);
		}

		/// <summary>
		/// Handles the termination of a compiler part.
		/// </summary>
		/// <param name="reason">Reason.</param>
		private void HandleTermination (string reason) {
			this.Error ("A CompilerComponent terminated.");
			this.Error ("Reason for termination: {0}", reason);
			this.abort = true;
		}

		/// <summary>
		/// Compiles the specified source file.
		/// </summary>
		/// <param name="input">Input file path.</param>
		/// <param name="output">Output file path.</param>
		public void Start (string input_file, string output_file) {
			this.compilerThread = new Thread (call => Compile (input_file, output_file));
			this.mainThread = new Thread (MainThread);
			this.mainThread.Start ();
			this.compilerThread.Start ();
			this.mainThread.Join ();
		}

		[SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
		private void MainThread () {
			while (!finished) {
				if (abort) {
					finished = true;
					compilerThread.Abort ();
				}
			}
		}

		/// <summary>
		/// Compiles the specified source file.
		/// </summary>
		/// <param name="input">Path to the source file. Both relative and absolute paths will be fine.</param>
		/// <param name="output">Path to the output file. Both relative and absolute paths will be fine.</param> 
		private void Compile (string input_file, string output_file) {
			long tpre = 0L, tlex = 0L, tgen = 0L, tsexify = 0L, ttotal = 0L;

			// Create a stopwatch to measure the time the
			// compiler takes to do specific operations.
			Stopwatch watch = new Stopwatch ();

			// Read input source from the standard input stream
			// if the CompilerOptions.ReadFromStdin is set.
			if (options.HasFlag (CompilerOptions.ReadFromStdin)) {
				StringBuilder sb = new StringBuilder ();

				while (Console.KeyAvailable) {
					sb.Append (Console.ReadKey ().KeyChar);
				}

				this.source = sb.ToString ();
			}

			// Read input source from file if the
			// CompilerOptions.ReadFromStdin is not set.
			else {
				using (var FILE = new FileStream (input_file, FileMode.Open)) {
					using (var reader = new StreamReader (FILE)) {
						this.source = reader.ReadToEnd ();
					}
				}
			}

			// Preprocessing the source
			this.Log ("Starting preprocessor.");
			watch.Start ();
			var preprocessed = this.preprocessor.Feed (this.source);
			watch.Stop ();
			tpre = watch.ElapsedMilliseconds;
			this.Log ("Preprocessing took {0}ms", tpre);

			// Lexical analysis
			this.Log ("Starting lexical analysis.");
			watch.Restart ();
			this.lexer.FeedSource (preprocessed);
			watch.Stop ();
			tlex = watch.ElapsedMilliseconds;
			this.Log ("Lexical analysis took {0}ms.", tlex);

			// Code generation
			this.Log ("Starting code generation.");
			watch.Restart ();
			var generated = this.generator.Feed (this.lexer.tokens);
			watch.Stop ();
			tgen = watch.ElapsedMilliseconds;
			this.Log ("Code generation took {0}ms.", tgen);

			// Beautification
			this.Log ("Starting beautification.");
			watch.Restart ();
			var beautified = this.sexifier.Feed (generated);
			watch.Stop ();
			tsexify = watch.ElapsedMilliseconds;
			this.Log ("Beautification took {0}ms.", tsexify);

			// Report the total time
			ttotal = tpre + tlex + tgen + tsexify;
			this.Log ("Compilation took {0}ms in total.", ttotal);

			// We need to check for a possible pending termination here,
			// because we would probably create a huge mess in the output file otherwise.
			if (this.abort)
				return;

			// Write the generated code to the standard out stream
			// if the CompilerOptions.OutputToStdout flag is set.
			if (this.options.HasFlag (CompilerOptions.OutputToStdout)) {
				// Set the Content-Type if the CompilerOptions.CGI-Flag is set
				if (this.options.HasFlag (CompilerOptions.CGI)) {
					Console.Write ("Content-Type: text/javascript\n\n");
				}
				Console.Write (beautified);
			}

			// Write the generated code to file if the
			// CompilerOptions.OutputToStdout flag is not set.
			else {
				this.Log ("Writing generated source code to file.");
				WriteOut (output_file, beautified);
				this.Log ("Done! File saved to {0}.", output_file);
			}

			// Set the finished variable to true
			this.finished = true;
		}

		/// <summary>
		/// Writes the file to disk.
		/// </summary>
		/// <param name="path">The target path. Bot relative and absolute paths will be fine.</param>
		private void WriteOut (string path, string source) {
			using (FileStream FILE = new FileStream (path, FileMode.Create)) {
				using (StreamWriter writer = new StreamWriter (FILE)) {
					writer.Write (source);
					writer.Flush ();
				}
			}
		}
	}
}

