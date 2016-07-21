using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Consoles;

namespace Phyrric.Panels
{
	public class MessagePanel : SadConsole.Consoles.Console
	{

		public MessagePanel()
			: base(Constants.MESSAGEPANEL_WIDTH - 2, Constants.MESSAGEPANEL_HEIGHT - 2)
		{
			Position = new Point(Constants.MESSAGEPANEL_X + 1, Constants.MESSAGEPANEL_Y + 1);

			ShiftRight(1);
		}

		public void PrintMessage(ColoredString msg)
		{
			var lines = msg.String.SplitByLength(Constants.MESSAGEPANEL_WIDTH - 4);
			lines = lines.Reverse();

			foreach (var line in lines)
			{
				ShiftDown(1);
				var cline = line.CreateColored(msg.Foreground, msg.Background);
				VirtualCursor.Print(cline).CarriageReturn();
			}
		}

		public void PrintGradientMessage(string msg, Color startColor, Color endColor)
		{
			var lines = msg.SplitByLength(Constants.MESSAGEPANEL_WIDTH - 4);
			lines = lines.Reverse();

			foreach (var line in lines)
			{
				ShiftDown(1);
				var gline = msg.CreateGradient(startColor, endColor, null);
				VirtualCursor.Print(gline).CarriageReturn();
			}
		}
	}
}
