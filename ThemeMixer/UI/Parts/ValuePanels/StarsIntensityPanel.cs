using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.ValuePanels
{
    public class StarsIntensityPanel : ValuePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Atmosphere;
            ValueID = ValueID.StarsIntensity;
            base.Awake();
        }
    }
}
