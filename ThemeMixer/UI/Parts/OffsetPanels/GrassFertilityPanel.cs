using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.OffsetPanels
{
    public class GrassFertilityPanel : OffsetPanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Terrain;
            OffsetID = OffsetID.GrassFertilityColorOffset;
            base.Awake();
        }
    }
}
