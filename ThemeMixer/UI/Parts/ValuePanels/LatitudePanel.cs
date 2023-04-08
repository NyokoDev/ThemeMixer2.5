using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.ValuePanels
{
    public class LatitudePanel : ValuePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Atmosphere;
            ValueID = ValueID.Latitude;
            base.Awake();
        }
    }
}
