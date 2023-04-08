using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.TexturePanels
{
    public class UpwardRoadDiffusePanel : TexturePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Structures;
            TextureID = TextureID.UpwardRoadDiffuse;
            base.Awake();
        }
    }
}
