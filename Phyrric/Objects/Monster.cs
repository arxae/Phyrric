using Microsoft.Xna.Framework;

namespace Phyrric.Objects
{
	public class Monster : Entity
	{
		public Monster(Point position)
			: base('x', position, Color.Crimson, Color.Transparent)
		{

		}

		public override void Update()
		{
			_wander();
		}

		void _wander()
		{
			var dir = new Point(
				Util.RandomChoice() ? 1 : -1,
				Util.RandomChoice() ? 1 : -1);

			Move(dir);
		}
	}
}
