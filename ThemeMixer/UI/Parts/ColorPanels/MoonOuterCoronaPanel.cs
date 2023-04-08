using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction.ColorPanel;

namespace ThemeMixer.UI.Parts.ColorPanels
{
    public class MoonOuterCoronaPanel : ColorPanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Atmosphere;
            ColorID = ColorID.MoonOuterCorona;
            base.Awake();
        }
    }
}
