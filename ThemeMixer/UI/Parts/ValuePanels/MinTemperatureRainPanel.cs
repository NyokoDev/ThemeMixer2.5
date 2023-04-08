using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.ValuePanels
{
    public class MinTemperatureRainPanel : ValuePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Weather;
            ValueID = ValueID.MinTemperatureRain;
            base.Awake();
        }
    }
}
