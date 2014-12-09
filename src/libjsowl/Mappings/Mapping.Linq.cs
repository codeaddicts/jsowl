using System;

namespace libjsowl
{
	public static class MappingExtensions
	{
		public static bool Matches (this string str, Mapping map) {
			return map.IsMatch (str);
		}
	}
}

