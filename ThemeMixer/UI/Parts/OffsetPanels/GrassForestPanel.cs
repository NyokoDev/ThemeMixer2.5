using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.OffsetPanels
{
    public class GrassForestPanel : OffsetPanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Terrain;
            OffsetID = OffsetID.GrassForestColorOffset;
            base.Awake();
        }
    }
}
