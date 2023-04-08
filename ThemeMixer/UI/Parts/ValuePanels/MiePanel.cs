using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.ValuePanels
{
    public class MiePanel : ValuePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Atmosphere;
            ValueID = ValueID.Mie;
            base.Awake();
        }
    }
}
