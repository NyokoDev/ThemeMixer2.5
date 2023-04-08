using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.TexturePanels
{
    public class RuinedDiffusePanel : TexturePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Terrain;
            TextureID = TextureID.RuinedDiffuseTexture;
            base.Awake();
        }
    }
}
