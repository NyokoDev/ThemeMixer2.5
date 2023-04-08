using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.ValuePanels
{
    public class FogProbabilityNightPanel : ValuePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Weather;
            ValueID = ValueID.FogProbabilityNight;
            base.Awake();
        }
    }
}
