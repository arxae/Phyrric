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
		public List<Entity> MapEntities { get; set; }
		public List<Entity> MapObjects { get; set; }
		public List<Rectangle> RoomCenters { get; set; }

		public PhyrricMap() : this(1, 1) { }
		public PhyrricMap(int width, int height)
			: base(width, height)
		{
			MapEntities = new List<Entity>();
			MapObjects = new List<Entity>();
			RoomCenters = new List<Rectangle>();
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
			int maxTerminals = 2;
			int minTerminalPlayerDistance = 10;

			var terminals = new List<Terminal>();
			foreach (var room in RoomCenters)
			{
				if (terminals.Count >= maxTerminals) break;

				if (room.Center.Distance(PhyrricGame.Player.Position) > minTerminalPlayerDistance)
				{
					if (CellContainsObject(room.Center) == false)
					{
						terminals.Add(new Terminal(room.Center, Util.GeneratePassword(4)));
					}
				}
			}

			// Checks failed, just plop down a terminal down somewhere, i don't care
			if (terminals.Count == 0)
			{
				var pos = RoomCenters[PhyrricGame.Rng.Next(RoomCenters.Count)];
				terminals.Add(new Terminal(pos.Center, Util.GeneratePassword(4)));
			}

			MapObjects.AddRange(terminals);
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