using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.ValuePanels
{
    public class MinTemperatureFogPanel : ValuePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Weather;
            ValueID = ValueID.MinTemperatureFog;
            base.Awake();
        }
    }
}
