using System;

namespace libjsowl
{
	public interface ITerminatable
	{
		TerminationCallback terminate { get; set; }
	}
}

