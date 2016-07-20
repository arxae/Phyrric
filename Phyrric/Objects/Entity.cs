using Microsoft.Xna.Framework;
using SadConsole.Consoles;

namespace Phyrric.Objects
{
	public class Entity : SadConsole.Game.GameObject
	{
		public bool IsInPlayerFov { get; set; }

		public Entity(char icon, Point position) : this(icon, position, Color.White, Color.TransparentBlack) { }
		public Entity(char icon, Point position, Color fg, Color bg)
		{
			Animation = new AnimatedTextSurface("default", 1, 1, SadConsole.Engine.DefaultFont);

			var frame = Animation.CreateFrame();
			frame[0].GlyphIndex = icon;
			frame[0].Foreground = fg;
			frame[0].Background = bg;

			Position = position;
		}

		public void SetForegroundColor(Color col)
		{
			Animation.CurrentFrame[0].Foreground = col;
		}

		public void SetBackgroundColor(Color col)
		{
			Animation.CurrentFrame[0].Background = col;
		}

		public void Move(Point direction)
		{
			var newPos = Position + direction;

			bool isNewPosValid = false;

			if (PhyrricGame.CurrentMap.IsWalkable(newPos.X, newPos.Y))
			{
				isNewPosValid = true;
			}

			if (PhyrricGame.CurrentMap.CellContainsEntity(newPos.X, newPos.Y))
			{
				// TODO: Interaction and such, 
				// allow move through entity for now
				//isNewPosValid = false;
				//var entity = PhyrricGame.CurrentMap.GetEntity(newPos.X, newPos.Y);
				//entity.Interact(Entity);
			}

			if (isNewPosValid)
			{
				Position = newPos;
			}
		}

		public void Move(int x, int y)
		{
			Move(new Point(x, y));
		}

		public void Move(Enums.Direction dir)
		{
			switch (dir)
			{
				case Enums.Direction.Up: Move(0, -1); break;
				case Enums.Direction.Right: Move(1, 0); break;
				case Enums.Direction.Down: Move(0, 1); break;
				case Enums.Direction.Left: Move(-1, 0); break;
			}
		}

		public virtual void Interact(Entity other)
		{
			System.Console.WriteLine($"Ohai {other.GetType()}");
		}
	}
}
