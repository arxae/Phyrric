using Phyrric.Data;

using Microsoft.Xna.Framework;
using SadConsole.Consoles;

namespace Phyrric.Objects
{
	public class Entity : SadConsole.Game.GameObject
	{
		public bool IsInPlayerFov { get; set; }

		// Stats (might split off later)
		public AbilityScoreSet AbilityScores { get; set; }

		public Entity(char icon, Point position) : this(icon, position, Color.White, Color.TransparentBlack) { }
		public Entity(char icon, Point position, Color fg, Color bg)
			: base(SadConsole.Engine.DefaultFont)
		{
			Animation = new AnimatedTextSurface("default", 1, 1, SadConsole.Engine.DefaultFont);

			var frame = Animation.CreateFrame();
			frame[0].GlyphIndex = icon;
			frame[0].Foreground = fg;
			frame[0].Background = bg;

			Position = position;
			AbilityScores = AbilityScoreSet.GetRandomSet();
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
				isNewPosValid = false;
				var entity = PhyrricGame.CurrentMap.GetEntity(newPos.X, newPos.Y);
				Interact(entity);
			}

			if (isNewPosValid)
			{
				Position = newPos;
			}
		}

		/// <summary>
		/// Move the player
		/// </summary>
		/// <param name="dir"></param>
		/// <returns>True if a move was attempted</returns>
		public bool Move(Enums.Direction dir)
		{
			switch (dir)
			{
				case Enums.Direction.Up: Move(new Point(0, -1)); return true;
				case Enums.Direction.Right: Move(new Point(1, 0)); return true;
				case Enums.Direction.Down: Move(new Point(0, 1)); return true;
				case Enums.Direction.Left: Move(new Point(-1, 0)); return true;
			}

			return false;
		}

		public virtual void Interact(Entity other) { }
	}
}
