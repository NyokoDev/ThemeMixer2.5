using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction.ColorPanel;

namespace ThemeMixer.UI.Parts.ColorPanels
{
    public class LateNightZenithPanel : ColorPanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Atmosphere;
            ColorID = ColorID.LateNightZenithColor;
            base.Awake();
        }
    }
}
