using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.ValuePanels
{
    public class RainProbabilityDayPanel : ValuePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Weather;
            ValueID = ValueID.RainProbabilityDay;
            base.Awake();
        }
    }
}
