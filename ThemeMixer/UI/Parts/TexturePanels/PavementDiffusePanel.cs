using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.TexturePanels
{
    public class PavementDiffusePanel : TexturePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Terrain;
            TextureID = TextureID.PavementDiffuseTexture;
            base.Awake();
        }
    }
}
