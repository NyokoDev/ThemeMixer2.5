using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.OffsetPanels
{
    public class GrassPollutionPanel : OffsetPanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Terrain;
            OffsetID = OffsetID.GrassPollutionColorOffset;
            base.Awake();
        }
    }
}
