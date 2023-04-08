using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction.ColorPanel;

namespace ThemeMixer.UI.Parts.ColorPanels
{
    public class WaterUnderPanel : ColorPanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Water;
            ColorID = ColorID.WaterUnder;
            base.Awake();
        }
    }
}
