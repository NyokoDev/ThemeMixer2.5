using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.SelectPanels
{
    public class SelectAtmospherePanel : SelectPanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Atmosphere;
            base.Awake();
            ButtonPanel.isVisible = true;
        }

        public override void Start()
        {
            base.Start();
            CenterToParent();
        }
    }
}
