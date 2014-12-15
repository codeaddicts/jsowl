using System;

namespace libjsowl
{
	public interface ILoggable
	{
		Log Logger { get; set; }
		bool Verbose { get; }
	}
}

