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
	public class DelaunayMapCreationStrategy<T> : IMapCreationStrategy<T> where T : PhyrricMap, IMap, new()
	{
		int width;
		int height;
		GausianRandom rng;

		public DelaunayMapCreationStrategy()
		{
			width = Constants.MAPPANEL_WIDTH;
			height = Constants.MAPPANEL_HEIGHT;
			rng = new GausianRandom();
		}

		public T CreateMap()
		{
			int maxCellNumber = 150;
			int minCellSize = 3;
			int maxCellSize = 10;
			int minSurface = 30;
			int maxSurface = 40;

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

			if (rooms.Count == 0)
			{
				throw new Exception("No rooms created");
			}

			//Meshing stuff
			var points = new List<Triangulator.Geometry.Point>();

			rooms.ForEach(room =>
			{
				points.Add(new Triangulator.Geometry.Point(room.Center.X, room.Center.Y));
			});

			var tris = Triangulator.Delauney.Triangulate(points);

			var graph = new UndirectedGraph<int, TaggedUndirectedEdge<int, int>>();

			foreach (var tri in tris)
			{
				var edge = new TaggedUndirectedEdge<int, int>(tri.p1, tri.p2, 0);
				graph.AddVerticesAndEdge(edge);
			}

			var mst = graph.MinimumSpanningTreePrim(e => e.Tag).ToList();

			mst.ForEach(e =>
			{
				Rectangle room1 = rooms[e.Source];
				Rectangle room2 = rooms[e.Target];

				MakeHorizontalTunnel(map, room1.Center.X, room2.Center.X, room1.Center.Y);
				MakeVerticalTunnel(map, room2.Center.Y, room1.Center.Y, room2.Center.X);
			});

			rooms.ForEach(room => MakeRoom(map, room));

			return map;
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
