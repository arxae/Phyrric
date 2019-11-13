namespace Phyrric
{
	using Microsoft.Xna.Framework;
	using Console = SadConsole.Console;

	static class Program
	{
		static void Main(string[] args)
		{
			SadConsole.Game.Create(80, 25);

			SadConsole.Game.OnInitialize = Init;

			SadConsole.Game.Instance.Run();
			SadConsole.Game.Instance.Dispose();
		}

		private static void Init()
		{
			Console c = new Console(80, 25);
			c.FillWithRandomGarbage();
			c.Fill(new Rectangle(3,3,23,3), Color.Violet, Color.Black, 0, 0);
			c.Print(4, 4, "Ohai :D");

			SadConsole.Global.CurrentScreen = c;
		}
	}
}
