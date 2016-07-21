using System;
using System.Linq;

using SadConsole;
using SadConsole.Consoles;
using Microsoft.Xna.Framework;

using Phyrric.Panels;
using SadConsole.Input;

namespace Phyrric
{
	public class GameScreen : ConsoleList
	{
		TextSurfaceRenderer _messagePanelBorder;
		TextSurface _messagePanelBorderSurface;

		public MapPanel Map;
		public MessagePanel Messages;
		public StatusPanel Status;

		public GameScreen()
		{
			// Create consoles
			Map = new MapPanel();
			Messages = new MessagePanel();
			Status = new StatusPanel();

			// Set parents
			Map.Parent = this;
			Messages.Parent = this;
			Status.Parent = this;

			// Add to list
			Add(Map);
			Add(Messages);
			Add(Status);

			// Border around the message panel
			// Done here so the messagepanel can scroll
			_messagePanelBorder = new TextSurfaceRenderer();
			_messagePanelBorderSurface = new TextSurface(Constants.MESSAGEPANEL_WIDTH,
				Constants.MESSAGEPANEL_HEIGHT, SadConsole.Engine.DefaultFont);

			var _surfaceEd = new SurfaceEditor(_messagePanelBorderSurface);

			var border = Util.GetPanelBorder(Constants.MESSAGEPANEL_WIDTH, Constants.MESSAGEPANEL_HEIGHT);
			border.BottomRightCharacter = 45;
			border.RightSideCharacter = 0;
			border.TopRightCharacter = 45;

			border.Draw(_surfaceEd);

			_surfaceEd.Print(2, 0, "Messages");

			// Say Hi!
			Messages.PrintGradientMessage("Welcome to Phyrric", Color.Orange, Color.OrangeRed);
		}

		public void Activate()
		{
			Engine.ConsoleRenderStack.Clear();
			Engine.ConsoleRenderStack.Add(this);
			Engine.ActiveConsole = this;
		}

		public override void Render()
		{
			base.Render();

			_messagePanelBorder.Render(_messagePanelBorderSurface, new Point(
				Constants.MESSAGEPANEL_X, Constants.MESSAGEPANEL_Y));

			Map.Render();
			Messages.Render();
			Status.Render();
		}

		public override bool ProcessKeyboard(KeyboardInfo info)
		{
			PhyrricGame.Player.ProcessKeyboard(info);

			// Debugkeys
			if (Settings.Debug)
			{
				// Explore all
				if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.F1)))
				{
					PhyrricGame.CurrentMap.GetAllCells().ForEach(cell =>
					{
						PhyrricGame.CurrentMap.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
					});
				}

				// Show objects out of fov
				if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.F2)))
				{
					Settings.Debug_DisplayObjectsOutOfFOV = !Settings.Debug_DisplayObjectsOutOfFOV;
				}

				// Unlock everything
				if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.F3)))
				{
					var terminals = PhyrricGame.CurrentMap.MapObjects
						.Where(obj => obj.GetType() == typeof(Objects.Terminal));
					terminals.ForEach(term => ((Objects.Terminal)term).Unlock());
				}

				// Move player to exit
				if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.F4)))
				{
					var stairs = PhyrricGame.CurrentMap.MapObjects
						.First(o => o.GetType() == typeof(Objects.Stairs))
						.Position;

					PhyrricGame.Player.Position = stairs;
				}
			}

			return true;
		}
	}
}
