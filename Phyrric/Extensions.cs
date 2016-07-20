using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Phyrric
{
	public static class Extensions
	{
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (action == null) throw new ArgumentNullException("action");

			foreach (var item in source)
			{
				action(item);
			}
		}

		public static Point GetRandomSpot(this PhyrricMap map, bool validOnly = true, int maxTries = 0)
		{
			if (maxTries == 0)
			{
				maxTries = map.Width * map.Height;
			}

			var triedPositions = new List<Point>(maxTries);

			for (int i = 0; i < maxTries; i++)
			{
				var pos = new Point(
					PhyrricGame.Rng.Next(map.Width),
					PhyrricGame.Rng.Next(map.Height));

				if (Settings.Debug)
				{
					if (triedPositions.Contains(pos)) Console.WriteLine($"Duplicate attept: {pos}");
				}

				var cell = map.GetCell(pos.X, pos.Y);

				// Cell has to be walkable and not contain an entity alread
				if (cell.IsWalkable
					&& map.MapEntities.Where(ent => ent.Position == pos).Count() == 0)
				{
					return pos;
				}
			}

			throw new Exception(string.Format("No valid map spot found after {0} tries.", maxTries.ToString()));
		}

		public static int Distance(this Point source, Point target)
		{
			int x = source.X - target.X;
			int y = source.Y - target.Y;

			return (int)Math.Sqrt((x * x) + (y * y));
		}

		public static T RandomElement<T>(this List<T> lst)
		{
			return lst[PhyrricGame.Rng.Next(lst.Count)];
		}

		public static List<T> RandomElements<T>(this List<T> lst, int count)
		{
			return lst
				.OrderBy(e => PhyrricGame.Rng.Next(int.MaxValue))
				.Take(count)
				.ToList();
		}
	}
}
