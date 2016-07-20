using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Consoles;

namespace Phyrric.Panels
{
    public class MessagePanel : Console
    {
        SurfaceEditor _editor;

        public MessagePanel()
            : base(Constants.MESSAGEPANEL_WIDTH - 2, Constants.MESSAGEPANEL_HEIGHT - 2)
        {
            Position = new Point(Constants.MESSAGEPANEL_X + 1, Constants.MESSAGEPANEL_Y + 1);

            _editor = new SurfaceEditor(TextSurface);
            _editor.ShiftRight(1);
        }

        public void PrintMessage(ColoredString msg)
        {
            _editor.ShiftDown(1);
            VirtualCursor.Print(msg).CarriageReturn();
        }
    }
}
