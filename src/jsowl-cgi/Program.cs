using System;
using System.IO;
using System.Text;

namespace jsowlcgi
{
	class MainClass
	{
		[STAThread]
		public static void Main (string[] args)
		{
			Console.Write ("Content-Type: text/javascript\n\n");
			string path = Environment.GetEnvironmentVariable ("PATH_TRANSLATED");
			if (File.Exists (path)) {
				string input = string.Empty;
				using (FileStream FILE = new FileStream (path, FileMode.Open)) {
					using (StreamReader reader = new StreamReader (FILE)) {
						input = reader.ReadToEnd ();
					}
				}
				Compiler compiler = new Compiler ();
				string src = compiler.Run (input);
				Console.WriteLine (src);
			}
		}
	}
}
