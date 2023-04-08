using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.TexturePanels
{
    public class BuildingBaseDiffusePanel : TexturePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Structures;
            TextureID = TextureID.BuildingBaseDiffuse;
            base.Awake();
        }
    }
}
