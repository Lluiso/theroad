using System.Collections.Generic;
using System.Linq;

namespace TeamDuaLipa
{
	public static class IEnumerableExtensions
	{
		private static System.Random _rand = new System.Random();

		public static T Random<T>(this IEnumerable<T> items)
		{
			return items.ElementAt(_rand.Next(items.Count()));
		}
	}
}