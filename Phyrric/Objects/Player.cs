using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole.Input;

namespace Phyrric.Objects
{
	public class Player : Entity
	{
		public Player() : this(new Point(0)) { }
		public Player(Point startPosition)
			: base('@', new Point(0), Color.Green, Color.TransparentBlack)
		{
			Position = startPosition;
		}

		internal void ProcessKeyboard(KeyboardInfo info)
		{
			if (info.KeysPressed.Contains(AsciiKey.Get(Keys.Up))) { Move(Enums.Direction.Up); }
			if (info.KeysPressed.Contains(AsciiKey.Get(Keys.Down))) { Move(Enums.Direction.Down); }
			if (info.KeysPressed.Contains(AsciiKey.Get(Keys.Left))) { Move(Enums.Direction.Left); }
			if (info.KeysPressed.Contains(AsciiKey.Get(Keys.Right))) { Move(Enums.Direction.Right); }
		}
	}
}
