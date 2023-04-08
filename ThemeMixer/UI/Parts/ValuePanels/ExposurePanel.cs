using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.ValuePanels
{
    public class ExposurePanel : ValuePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Atmosphere;
            ValueID = ValueID.Exposure;
            base.Awake();
        }
    }
}
