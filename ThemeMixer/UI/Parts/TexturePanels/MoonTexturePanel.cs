using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.TexturePanels
{
    public class MoonTexturePanel : TexturePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Atmosphere;
            TextureID = TextureID.MoonTexture;
            base.Awake();
        }
    }
}
