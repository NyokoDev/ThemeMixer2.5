using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.TexturePanels
{
    public class WaterNormalPanel : TexturePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Water;
            TextureID = TextureID.WaterNormal;
            base.Awake();
        }
    }
}
