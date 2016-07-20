using Microsoft.Xna.Framework;
using SadConsole.Consoles;

namespace Phyrric.Panels
{
    public class StatusPanel : Console
    {

        public StatusPanel()
            : base(Constants.STATUSPANEL_WIDTH, Constants.STATUSPANEL_HEIGHT)
        {
            Position = new Point(Constants.STATUSPANEL_X, Constants.STATUSPANEL_Y);

            var border = Util.GetPanelBorder(Constants.STATUSPANEL_WIDTH, Constants.STATUSPANEL_HEIGHT);

            border.Draw(this);
        }
    }
}
