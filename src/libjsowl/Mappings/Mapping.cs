using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace libjsowl
{
	public abstract class Mapping
	{
		public readonly Regex pattern;
		public readonly string replacement;

		public Mapping (string _pattern, string _replacement) {
			this.pattern = new Regex (_pattern, RegexOptions.CultureInvariant);
			this.replacement = _replacement;
		}

		public virtual bool IsMatch (string str) {
			return pattern.IsMatch (str);
		}

		public virtual string Replace (string str) {
			return pattern.Replace (str, replacement);
		}

		public virtual string Replace (Token tk) {
			return "";
		}
	}
}

