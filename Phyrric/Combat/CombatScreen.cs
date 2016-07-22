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
			var attack_button = new Button(15, 1) { Text = "Attack", Position = new Point(1, Height - 2) };
			var flee_button = new Button(15, 1) { Text = "Flee", Position = new Point(17, Height - 2) };

			// Button actions
			attack_button.ButtonClicked += (s, e) => _debugResolveCombat();
			flee_button.ButtonClicked += (s, e) => PhyrricGame.GameScreen.Activate();

			// Add buttons
			Add(attack_button);
			Add(flee_button);

			_debugPrintStats();
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

		void _debugPrintStats()
		{
			int a_column = 1;
			int b_column = 25;

			_printStat(A, a_column, "Entity A");
			_printStat(B, b_column, "Entity B");
		}

		void _printStat(Entity ent, int startingColumn, string name)
		{
			int x = startingColumn;

			Print(x, 1, $"{name}: {ent.GetType().Name}");
			x++;
			Print(x, 2, $"Average score: {ent.AbilityScores.AverageScore}");
			Print(x, 3, $"STR: {ent.AbilityScores.Strength.Value}");
			Print(x, 4, $"DEX: {ent.AbilityScores.Dexterity.Value}");
			Print(x, 5, $"CON: {ent.AbilityScores.Constitution.Value}");
			Print(x, 6, $"INT: {ent.AbilityScores.Intelligence.Value}");
		}

		public void Activate()
		{
			SadConsole.Engine.ConsoleRenderStack.Clear();
			SadConsole.Engine.ConsoleRenderStack.Add(this);
			SadConsole.Engine.ActiveConsole = this;
		}
	}
}
