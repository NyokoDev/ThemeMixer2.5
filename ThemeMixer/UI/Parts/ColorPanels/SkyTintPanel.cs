using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction.ColorPanel;

namespace ThemeMixer.UI.Parts.ColorPanels
{
    public class SkyTintPanel : ColorPanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Atmosphere;
            ColorID = ColorID.SkyTint;
            base.Awake();
        }
    }
}
