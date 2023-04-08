using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.SelectPanels
{
    public class SelectThemePanel : SelectPanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Themes;
            base.Awake();
            ButtonPanel.isVisible = false;
        }
    }
}
