using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.OffsetPanels
{
    public class GrassFieldPanel : OffsetPanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Terrain;
            OffsetID = OffsetID.GrassFieldColorOffset;
            base.Awake();
        }
    }
}
