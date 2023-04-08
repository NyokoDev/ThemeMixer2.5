using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction.ColorPanel;

namespace ThemeMixer.UI.Parts.ColorPanels
{
    public class MoonInnerCoronaPanel : ColorPanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Atmosphere;
            ColorID = ColorID.MoonInnerCorona;
            base.Awake();
        }
    }
}
