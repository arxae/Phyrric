using Microsoft.Xna.Framework;
using SadConsole.Shapes;

namespace Phyrric
{
	public static class Util
	{
		public static Box GetPanelBorder(int w, int h)
		{
			return new Box(179, 45, 42, 42, 42, 42, w, h);
		}

		public static Line GetLine(Point start, Point end)
		{
			var line = new Line
			{
				StartingLocation = start,
				EndingLocation = end,
				CellAppearance = { GlyphIndex = 45 },
				UseStartingCell = false,
				UseEndingCell = false
			};

			return line;
		}

		public static string GeneratePassword(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";

			var stringChars = new char[length];

			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = chars[PhyrricGame.Rng.Next(chars.Length)];
			}

			return new string(stringChars);
		}
	}
}
