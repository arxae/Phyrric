using Microsoft.Xna.Framework;
using SadConsole.Consoles;

using Phyrric.Objects;

namespace Phyrric.Panels
{
	public class MapPanel : Console
	{

		public MapPanel()
			: base(Constants.MAPPANEL_WIDTH, Constants.MAPPANEL_HEIGHT)
		{
			Position = new Point(Constants.MAPPANEL_X, Constants.MAPPANEL_Y);
		}

		public override void Render()
		{
			base.Render();
			Clear();

			_renderMap();
		}

		void _renderMap()
		{
			var map = PhyrricGame.CurrentMap;

			// Render map cells
			map.GetAllCells().ForEach(cell =>
			{
				if (cell.IsInFov)
				{
					map.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);

					if (cell.IsWalkable)
					{
						SetGlyph(cell.X, cell.Y, 219);
						SetForeground(cell.X, cell.Y, Colors.Tile_InFov_Floor);
					}
					else
					{
						SetGlyph(cell.X, cell.Y, 219);
						SetForeground(cell.X, cell.Y, Colors.Tile_InFov_Wall);
					}
				}
				else if (cell.IsExplored)
				{
					if (cell.IsWalkable)
					{
						SetGlyph(cell.X, cell.Y, 219);
						SetForeground(cell.X, cell.Y, Colors.Tile_Floor);
					}
					else
					{
						SetGlyph(cell.X, cell.Y, 219);
						SetForeground(cell.X, cell.Y, Colors.Tile_Wall);
					}
				}
			});

			// Render map entities
			if (Settings.Debug_DisplayObjectsOutOfFOV)
			{
				map.MapEntities.ForEach(ent => ent.Render());
				map.MapObjects.ForEach(obj => obj.Render());
			}
			else
			{
				map.MapEntities?.ForEach(ent => { if (ent.IsInPlayerFov) { ent.Render(); } });
				map.MapObjects?.ForEach(obj => { if (obj.IsInPlayerFov) { obj.Render(); } });
			}

			// Render player
			PhyrricGame.Player.Render();
		}
	}
}