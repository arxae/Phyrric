using Microsoft.Xna.Framework;

using Phyrric.Objects;

namespace Phyrric
{
	public class PhyrricGame : Game
	{
		GraphicsDeviceManager _graphics;

		public static Player Player { get; set; }
		public static GameScreen GameScreen { get; set; }
		public static PhyrricMap CurrentMap { get; set; }
		public static RogueSharp.Random.GaussianRandom Rng { get; set; }

		public PhyrricGame()
			: base()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Data";

			Rng = new RogueSharp.Random.GaussianRandom();

			var engineComponent = new SadConsole.EngineGameComponent(this, _graphics, "Data/Fonts/C64.font",
				Constants.CONSOLE_WIDTH, Constants.CONSOLE_HEIGHT, () =>
				 {
					 Window.Title = "Phyrric";
					 IsMouseVisible = true;
					 IsFixedTimeStep = true;

					 SadConsole.Engine.UseMouse = true;
					 SadConsole.Engine.UseKeyboard = true;
					 SadConsole.Engine.ProcessMouseWhenOffScreen = false;

					 SadConsole.Engine.Keyboard.RepeatDelay = 0.07f;
					 SadConsole.Engine.Keyboard.InitialRepeatDelay = 0.1f;

					 GameScreen = new GameScreen();
					 GameScreen.Activate();

					 NextMap();
				 });

			Components.Add(engineComponent);
		}

		public static void ProcessTurn()
		{
			CurrentMap.MapEntities.ForEach(ent => ent.Update());
			CurrentMap.MapObjects.ForEach(obj => obj.Update());
		}

		public static void NextMap()
		{
			CurrentMap = PhyrricMap.Create(
						 new MapCreation.MinSpanningTreeCreationStrategy<PhyrricMap>());
			CurrentMap.PopulateAll();
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			CurrentMap.Update();
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			base.Draw(gameTime);
		}
	}
}
