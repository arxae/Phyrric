using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole.Input;

namespace Phyrric.Objects
{
	public class Player : Entity
	{
		public Player(Point startPosition)
			: base('@', new Point(0), Color.Green, Color.TransparentBlack)
		{
			Position = startPosition;
		}

		internal void ProcessKeyboard(KeyboardInfo info)
		{
			bool hasMoved = false;

			if (info.KeysPressed.Contains(AsciiKey.Get(Keys.Up))) { hasMoved = Move(Enums.Direction.Up); }
			if (info.KeysPressed.Contains(AsciiKey.Get(Keys.Down))) { hasMoved = Move(Enums.Direction.Down); }
			if (info.KeysPressed.Contains(AsciiKey.Get(Keys.Left))) { hasMoved = Move(Enums.Direction.Left); }
			if (info.KeysPressed.Contains(AsciiKey.Get(Keys.Right))) { hasMoved = Move(Enums.Direction.Right); }

			if (hasMoved)
			{
				PhyrricGame.ProcessTurn();
			}
		}

		public override void Interact(Entity other)
		{

		}
	}
}
