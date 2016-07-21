using System.Collections.Generic;

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
				stringChars[i] = chars[PhyrricGame.Rng.Next(chars.Length - 1)];
			}

			return new string(stringChars);
		}

		/// <summary>
		/// Returns yes or no chance% of the time
		/// </summary>
		/// <param name="chance"></param>
		/// <returns></returns>
		public static bool RandomChoice(int chance = 50)
		{
			return PhyrricGame.Rng.Next(100) > chance;
		}

		static List<string> _messages;
		public static string GetRandomMessage()
		{
			if (_messages == null)
			{
				_messages = new List<string>()
				{
					"What does [password] mean?",
					"Well hello there",
					"Look behind you!"
				};
			}

			return _messages.RandomElement();
		}
	}
}
