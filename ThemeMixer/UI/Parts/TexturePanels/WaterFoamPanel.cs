using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;

namespace ThemeMixer.UI.Parts.TexturePanels
{
    public class WaterFoamPanel : TexturePanel
    {
        public override void Awake()
        {
            Category = ThemeCategory.Water;
            TextureID = TextureID.WaterFoam;
            base.Awake();
        }
    }
}
