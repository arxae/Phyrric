using System;

using Microsoft.Xna.Framework;

namespace Phyrric.Objects
{
	public class Stairs : Entity
	{
		public Stairs(Point position)
			: base((char)16, position, Color.Black, Color.Transparent)
		{

		}

		public override void Interact(Entity other)
		{
			if (PhyrricGame.CurrentMap.GetNumberOfLocks() == 0)
			{
				PhyrricGame.NextMap();
			}
			else
			{
				PhyrricGame.GameScreen.Messages.PrintMessage(
					"Not all the terminals are unlocked!".CreateColored(Color.Crimson, Color.Black));
			}
		}
	}
}
