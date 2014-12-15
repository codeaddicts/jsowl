using System;

namespace libjsowl
{
	public static class LogExtensions
	{
		public static void Log<TLoggable> (this TLoggable log, object obj) where TLoggable: ILoggable {
			log.Logger.WriteLine (log, obj);
		}

		public static void Log<TLoggable> (this TLoggable log, string obj, params object[] args) where TLoggable: ILoggable {
			log.Logger.WriteLine (log, obj, args);
		}

		public static void Error<TLoggable> (this TLoggable log, object obj) where TLoggable : ILoggable {
			log.Logger.WriteErrorLine (log, obj);
		}

		public static void Error<TLoggable> (this TLoggable log, string obj, params object[] args) where TLoggable: ILoggable {
			log.Logger.WriteErrorLine (log, obj, args);
		}
	}
}

