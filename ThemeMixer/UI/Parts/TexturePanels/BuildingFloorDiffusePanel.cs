using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.TexturePanels
{
    public class BuildingFloorDiffusePanel : TexturePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Structures;
            TextureID = TextureID.BuildingFloorDiffuse;
            base.Awake();
        }
    }
}
