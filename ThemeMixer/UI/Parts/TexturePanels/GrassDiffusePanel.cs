using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.TexturePanels
{
    public class GrassDiffusePanel : TexturePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Terrain;
            TextureID = TextureID.GrassDiffuseTexture;
            base.Awake();
        }
    }
}
