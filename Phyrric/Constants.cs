namespace Phyrric
{
	public class Constants
	{
		// Console sizes
		public const int CONSOLE_HEIGHT = 50;
		public const int CONSOLE_WIDTH = 60;

		public const int MAPPANEL_HEIGHT = CONSOLE_HEIGHT - 10;
		public const int MAPPANEL_WIDTH = CONSOLE_WIDTH;
		public const int MAPPANEL_X = 0;
		public const int MAPPANEL_Y = 0;

		public const int MESSAGEPANEL_HEIGHT = CONSOLE_HEIGHT - MAPPANEL_HEIGHT;
		public const int MESSAGEPANEL_WIDTH = CONSOLE_WIDTH - 15;
		public const int MESSAGEPANEL_X = 0;
		public const int MESSAGEPANEL_Y = CONSOLE_HEIGHT - MESSAGEPANEL_HEIGHT;

		public const int STATUSPANEL_HEIGHT = 10;
		public const int STATUSPANEL_WIDTH = CONSOLE_WIDTH - MESSAGEPANEL_WIDTH;
		public const int STATUSPANEL_X = CONSOLE_WIDTH - STATUSPANEL_WIDTH;
		public const int STATUSPANEL_Y = CONSOLE_HEIGHT - STATUSPANEL_HEIGHT;

		public const int COMBATSCREEN_HEIGHT = CONSOLE_HEIGHT;
		public const int COMBATSCREEN_WIDTH = CONSOLE_WIDTH;
	}
}