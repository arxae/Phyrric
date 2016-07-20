using System;

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
			ColoredString cstring = "Welcome to Phyrric".CreateGradient(Color.Orange, Color.OrangeRed, null);
			Messages.PrintMessage(cstring);
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
				if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.F1)))
				{
					PhyrricGame.CurrentMap.GetAllCells().ForEach(cell =>
					{
						PhyrricGame.CurrentMap.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
					});
				}

				if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.F2)))
				{
					Settings.Debug_DisplayObjectsOutOfFOV = !Settings.Debug_DisplayObjectsOutOfFOV;
				}
			}

			if (info.KeysReleased.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Space)))
			{
				var player = PhyrricGame.Player;

				if (PhyrricGame.CurrentMap.CellContainsObject(player.Position))
				{
					var ent = PhyrricGame.CurrentMap.GetMapObject(player.Position);
					ent.Interact(player);
				}
			}

			return true;
		}
	}
}
