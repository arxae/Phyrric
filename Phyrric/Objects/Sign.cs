using System;

using Microsoft.Xna.Framework;
using SadConsole;

namespace Phyrric.Objects
{
	public class Sign : Entity
	{
		ColoredString Message;

		public Sign(Point location, ColoredString text)
			: base((char)223, location, Color.White, Color.Transparent)
		{
			Message = text;
		}

		public override void Interact(Entity other)
		{
			PhyrricGame.GameScreen.Messages.PrintMessage(Message);
		}
	}
}
