using Phyrric.Objects;

using SadConsole.Input;
using Microsoft.Xna.Framework.Input;

namespace Phyrric.Combat
{
	public class CombatScreen : SadConsole.Consoles.Console
	{
		Entity A;
		Entity B;

		public CombatScreen(Entity entryA, Entity entryB)
			: base(Constants.COMBATSCREEN_WIDTH, Constants.COMBATSCREEN_HEIGHT)
		{
			A = entryA;
			B = entryB;

			Print(1, 1, $"A: {A.GetType()}");
			Print(1, 2, $"B: {B.GetType()}");
		}

		public void Activate()
		{
			SadConsole.Engine.ConsoleRenderStack.Clear();
			SadConsole.Engine.ConsoleRenderStack.Add(this);
			SadConsole.Engine.ActiveConsole = this;
		}

		public override bool ProcessKeyboard(KeyboardInfo info)
		{
			var ctrl = info.KeysDown.Contains(AsciiKey.Get(Keys.LeftControl))
				|| info.KeysDown.Contains(AsciiKey.Get(Keys.RightControl));

			if (ctrl && info.KeysPressed.Contains(AsciiKey.Get(Keys.X)))
			{
				PhyrricGame.GameScreen.Activate();
			}

			return true;
		}
	}
}
