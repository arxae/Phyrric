using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using RogueSharp;
using RogueSharp.MapCreation;

using GausianRandom = RogueSharp.Random.GaussianRandom;

namespace Phyrric.MapCreation
{
	public class TestMapCreationStrategy<T> : IMapCreationStrategy<T> where T : IMap, new()
	{
		int height;
		int width;
		GausianRandom rng;

		public TestMapCreationStrategy()
		{
			height = Constants.MAPPANEL_HEIGHT;
			width = Constants.MAPPANEL_WIDTH;
			rng = new GausianRandom();
		}

		public T CreateMap()
		{
			var rooms = new List<Rectangle>();
			var map = new T();

			map.Initialize(width, height);

			var maxRooms = 15;
			var roomMinSize = 3;
			var roomMaxSize = 15;

			for (int r = 0; r < maxRooms; r++)
			{
				int roomWidth = rng.Next(roomMinSize, roomMaxSize);
				int roomHeight = rng.Next(roomMinSize, roomMaxSize);
				int roomXPosition = rng.Next(0, width - roomWidth - 1);
				int roomYPosition = rng.Next(0, height - roomHeight - 1);

				var roomLocation = new Point(roomXPosition, roomYPosition);
				var roomSize = new Point(roomWidth, roomHeight);

				var newRoom = new Rectangle(roomLocation, roomSize);
				bool newRoomIntersects = false;

				foreach (Rectangle room in rooms)
				{
					if (newRoom.Intersects(room))
					{
						newRoomIntersects = true;
						break;
					}
				}

				if (newRoomIntersects == false)
				{
					rooms.Add(newRoom);
				}
			}

			foreach (Rectangle room in rooms)
			{
				MakeRoom(map, room);
			}

			for (int r = 0; r < rooms.Count; r++)
			{
				if (r == 0)
				{
					continue;
				}

				int previousRoomCenterX = rooms[r - 1].Center.X;
				int previousRoomCenterY = rooms[r - 1].Center.Y;
				int currentRoomCenterX = rooms[r].Center.X;
				int currentRoomCenterY = rooms[r].Center.Y;

				if (rng.Next(0, 2) == 0)
				{
					MakeHorizontalTunnel(map, previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
					MakeVerticalTunnel(map, previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
				}
				else
				{
					MakeVerticalTunnel(map, previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
					MakeHorizontalTunnel(map, previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
				}
			}

			return map;
		}

		private void MakeRoom(T map, Rectangle room)
		{
			for (int x = room.Left + 1; x < room.Right; x++)
			{
				for (int y = room.Top + 1; y < room.Bottom; y++)
				{
					map.SetCellProperties(x, y, true, true);
				}
			}
		}

		private void MakeHorizontalTunnel(T map, int xStart, int xEnd, int yPosition)
		{
			for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
			{
				map.SetCellProperties(x, yPosition, true, true);
			}
		}

		private void MakeVerticalTunnel(T map, int yStart, int yEnd, int xPosition)
		{
			for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
			{
				map.SetCellProperties(xPosition, y, true, true);
			}
		}
	}
}
