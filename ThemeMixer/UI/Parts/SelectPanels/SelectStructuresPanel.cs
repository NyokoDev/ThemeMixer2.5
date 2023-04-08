using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.SelectPanels
{
    public class SelectStructuresPanel : SelectPanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Structures;
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
