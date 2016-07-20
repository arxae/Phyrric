using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using RogueSharp;
using RogueSharp.MapCreation;

using Phyrric.Objects;

namespace Phyrric
{
	public class PhyrricMap : Map, IMap
	{
		int maxTerminals = 2;
		int minTerminalPlayerDistance = 10;
		int maxSigns = 5;

		public List<Entity> MapEntities { get; set; }
		public List<Entity> MapObjects { get; set; }
		public List<Rectangle> RoomCenters { get; set; }
		public List<string> Passwords { get; set; }
		public List<string> KnownPasswords { get; set; }

		public PhyrricMap() : this(1, 1) { }
		public PhyrricMap(int width, int height)
			: base(width, height)
		{
			MapEntities = new List<Entity>();
			MapObjects = new List<Entity>();
			RoomCenters = new List<Rectangle>();
			Passwords = new List<string>();
			KnownPasswords = new List<string>();
		}

		public void Update()
		{
			var playerPos = PhyrricGame.Player.Position;
			ComputeFov(playerPos.X, playerPos.Y, 5, true);

			MapEntities.ForEach(ent => ent.IsInPlayerFov = IsInFov(ent.Position.X, ent.Position.Y));
			MapObjects.ForEach(obj => obj.IsInPlayerFov = IsInFov(obj.Position.X, obj.Position.Y));
		}

		public int GetNumberOfLocks()
		{
			return MapObjects
				.Where(o => o.GetType() == typeof(Terminal)
					&& ((Terminal)o).IsLocked == true)
				.Count();
		}

		// Map population
		public void PopulateAll()
		{
			PhyrricGame.Player = new Player(PhyrricGame.CurrentMap.GetRandomSpot());

			_placeStairs();
			_populateTerminals();
			_populateSigns();
			_populateMonsters();
		}

		void _placeStairs()
		{
			var furthest = RoomCenters.Max(r => r.Center.Distance(PhyrricGame.Player.Position));
			var furthestRoom = RoomCenters.Where(r => r.Center.Distance(PhyrricGame.Player.Position) == furthest).First();

			var stairs = new Stairs(furthestRoom.Center);
			MapObjects.Add(stairs);
		}

		void _populateTerminals()
		{
			var terminals = new List<Terminal>();

			foreach (var room in RoomCenters)
			{
				if (terminals.Count >= maxTerminals) break;

				if (room.Center.Distance(PhyrricGame.Player.Position) > minTerminalPlayerDistance)
				{
					if (CellContainsObject(room.Center) == false)
					{
						var password = Util.GeneratePassword(4);
						terminals.Add(new Terminal(room.Center, password));
						Passwords.Add(password);
					}
				}
			}

			// Checks failed, just plop down a terminal down somewhere, i don't care
			if (terminals.Count == 0)
			{
				var password = Util.GeneratePassword(4);
				var pos = RoomCenters[PhyrricGame.Rng.Next(RoomCenters.Count)];
				terminals.Add(new Terminal(pos.Center, password));
				Passwords.Add(password);
			}

			MapObjects.AddRange(terminals);
		}

		void _populateSigns()
		{
			var signsToGenerate = PhyrricGame.Rng.Next(1, maxSigns);

			for (int s = 0; s < signsToGenerate; s++)
			{
				var room = RoomCenters.RandomElement();

				Point loc = new Point(
					Util.RandomChoice() ? room.Left + 1 : room.Right - 1,
					Util.RandomChoice() ? room.Top + 1 : room.Bottom - 1);

				var msg = Util.GetRandomMessage()
					.Replace("[password]", Passwords.RandomElement());

				var cstring = msg.CreateColored(Color.White, null);

				MapObjects.Add(new Sign(loc, cstring));
			}
		}

		void _populateMonsters()
		{
			int monstersToSpawn = PhyrricGame.Rng.Next(1, RoomCenters.Count);
			var monsterPositions = new List<Point>();

			for (int m = 0; m < monstersToSpawn; m++)
			{
				bool emptySpace = false;
				int attemptCounter = 0;

				do
				{
					var pos = this.GetRandomSpot();

					if (CellContainsEntity(pos) == false && CellContainsObject(pos) == false)
					{
						monsterPositions.Add(pos);
						emptySpace = true;
					}

					attemptCounter++;
				} while (emptySpace == false || attemptCounter == 100);
			}

			monsterPositions.ForEach(pos =>
			{
				var monster = new Monster(pos);
				MapEntities.Add(monster);
			});
		}

		// Utility methods
		public bool CellContainsEntity(Point pt)
		{
			return MapEntities
				.Where(e => e.Position == pt)
				.Count() > 0;
		}

		public bool CellContainsObject(Point pt)
		{
			return MapObjects
				.Where(e => e.Position == pt)
				.Count() > 0;
		}

		public bool CellContainsEntity(int x, int y)
		{
			return CellContainsEntity(new Point(x, y));
		}

		public bool CellContainsObject(int x, int y)
		{
			return CellContainsObject(new Point(x, y));
		}

		public Entity GetEntity(Point pt)
		{
			return MapEntities.Where(ent => ent.Position == pt).First();
		}

		public Entity GetEntity(int x, int y)
		{
			return GetEntity(new Point(x, y));
		}

		public Entity GetMapObject(Point pt)
		{
			return MapObjects.Where(ent => ent.Position == pt).First();
		}

		public Entity GetMapObject(int x, int y)
		{
			return GetMapObject(new Point(x, y));
		}

		/// Static methods
		public static PhyrricMap Create(IMapCreationStrategy<PhyrricMap> mapCreationStrategy)
		{
			if (mapCreationStrategy == null)
			{
				throw new ArgumentNullException("mapCreationStrategy", "Map creation strategy cannot be null");
			}

			var map = mapCreationStrategy.CreateMap();

			return map;
		}
	}
}