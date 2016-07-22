using Phyrric.Objects;

using Microsoft.Xna.Framework;
using SadConsole.Controls;

namespace Phyrric.Combat
{
	public class CombatScreen : SadConsole.Consoles.ControlsConsole
	{
		Entity A;
		Entity B;

		public CombatScreen(Entity entryA, Entity entryB)
			: base(Constants.COMBATSCREEN_WIDTH, Constants.COMBATSCREEN_HEIGHT)
		{
			A = entryA;
			B = entryB;

			// Declare buttons
			var attack_button = new Button(15, 1) { Text = "Attack", Position = new Point(1, 1) };
			var flee_button = new Button(15, 1) { Text = "Flee", Position = new Point(1, 2) };

			// Button actions
			attack_button.ButtonClicked += (s, e) => _debugResolveCombat();
			flee_button.ButtonClicked += (s, e) => PhyrricGame.GameScreen.Activate();

			// Add buttons
			Add(attack_button);
			Add(flee_button);
		}

		void _debugResolveCombat()
		{
			// resolve battle here for now
			var pw = PhyrricGame.CurrentMap.Passwords.RandomElement();
			var msg = $"You found password: {pw}";
			PhyrricGame.GameScreen.Messages.PrintGradientMessage(msg, Color.White, Color.LightBlue);

			// remove
			if (B.GetType() == typeof(Monster))
			{
				PhyrricGame.CurrentMap.MapEntities.Remove(B);
			}

			PhyrricGame.GameScreen.Activate();
		}

		public void Activate()
		{
			SadConsole.Engine.ConsoleRenderStack.Clear();
			SadConsole.Engine.ConsoleRenderStack.Add(this);
			SadConsole.Engine.ActiveConsole = this;
		}
	}
}