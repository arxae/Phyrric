using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using QuickGraph;
using QuickGraph.Algorithms;
using RogueSharp;
using RogueSharp.MapCreation;

using GausianRandom = RogueSharp.Random.GaussianRandom;

namespace Phyrric.MapCreation
{
	public class MinSpanningTreeCreationStrategy<T> : IMapCreationStrategy<T> where T : PhyrricMap, IMap, new()
	{
		GausianRandom rng;

		int height;
		int width;

		int maxCellNumber;
		int minCellSize;
		int maxCellSize;
		int minSurface;
		int maxSurface;

		public MinSpanningTreeCreationStrategy(int maxcellnumber = 150, int mincellsize = 3, int maxcellsize = 10,
			int minsurface = 30, int maxsurface = 40)
		{
			rng = new GausianRandom();

			height = Constants.MAPPANEL_HEIGHT;
			width = Constants.MAPPANEL_WIDTH;

			maxCellNumber = maxcellnumber;
			minCellSize = mincellsize;
			maxCellSize = maxcellsize;
			minSurface = minsurface;
			maxSurface = maxsurface;

		}

		public T CreateMap()
		{
			var map = new T();
			map.Initialize(width, height);

			var rooms = new List<Rectangle>();

			// randomly distribute points for room centers
			var cellLocations = new List<Point>();
			for (int c = 0; c < maxCellNumber; c++)
			{
				var loc = new Point(rng.Next(width), rng.Next(height));
				cellLocations.Add(loc);
			}

			// spawn a rectangle for every cell, making sure not to intersect
			// intersecting triangles will be discarded
			foreach (var loc in cellLocations)
			{
				var size = new Point(
					rng.Next(minCellSize, maxCellSize),
					rng.Next(minCellSize, maxCellSize));
				var rect = new Rectangle(loc, size);

				// reposition so the cell location points to the center instead of top left
				rect.Location = new Point(
					rect.Location.X - (rect.Width / 2),
					rect.Location.Y - (rect.Height / 2));

				bool rectIntersects = false;

				foreach (var room in rooms)
				{
					if (rect.Intersects(room))
					{
						rectIntersects = true;
						break;
					}
				}

				if (rectIntersects == false)
				{
					// Check if the rectangle is inside the map
					if (rect.Left > 0 && rect.Right < width
						&& rect.Top > 0 && rect.Bottom < height
						&& (rect.Width * rect.Height) > minSurface
						&& (rect.Width * rect.Height) < maxSurface)
					{
						rooms.Add(rect);
					}
				}
			}

			var graph = new UndirectedGraph<int, TaggedUndirectedEdge<int, int>>();

			for (int a = 0; a < rooms.Count; a++)
			{
				for (int b = 0; b < rooms.Count; b++)
				{
					if (a == b) continue;
					var edge = new TaggedUndirectedEdge<int, int>(a, b, 0);
					graph.AddVerticesAndEdge(edge);
				}
			}

			var mst = graph.MinimumSpanningTreePrim(e => e.Tag).ToList();

			mst.ForEach(e =>
			{
				Rectangle source = rooms[e.Source];
				Rectangle target = rooms[e.Target];

				Dig(map, source, target);
			});

			rooms.ForEach(r => MakeRoom(map, r));

			map.RoomCenters = rooms;

			return map;
		}

		void Dig(T map, Rectangle source, Rectangle target)
		{
			MakeHorizontalTunnel(map, source.Center.X, target.Center.X, source.Center.Y);
			MakeVerticalTunnel(map, target.Center.Y, source.Center.Y, target.Center.X);
		}

		void MakeRoom(T map, Rectangle room)
		{
			for (int x = room.Left + 1; x < room.Right; x++)
			{
				for (int y = room.Top + 1; y < room.Bottom; y++)
				{
					map.SetCellProperties(x, y, true, true);
				}
			}
		}

		void MakeHorizontalTunnel(T map, int xStart, int xEnd, int yPosition)
		{
			for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
			{
				map.SetCellProperties(x, yPosition, true, true);
			}
		}

		void MakeVerticalTunnel(T map, int yStart, int yEnd, int xPosition)
		{
			for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
			{
				map.SetCellProperties(xPosition, y, true, true);
			}
		}
	}
}
