using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction.ColorPanel;

namespace ThemeMixer.UI.Parts.ColorPanels
{
    public class WaterCleanPanel : ColorPanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Water;
            ColorID = ColorID.WaterClean;
            base.Awake();
        }
    }
}
