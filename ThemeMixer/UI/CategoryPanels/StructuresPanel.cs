using ColossalFramework.UI;
using ThemeMixer.Locale;
using ThemeMixer.Resources;
using ThemeMixer.Themes.Enums;
using ThemeMixer.TranslationFramework;
using ThemeMixer.UI.Abstraction;
using ThemeMixer.UI.Parts.TexturePanels;
using UnityEngine;

namespace ThemeMixer.UI.CategoryPanels
{
    public class StructuresPanel : PanelBase
    {
        private UIPanel _labelPanel;
        private UILabel _label;
        private UIButton _loadButton;

        private PanelBase _container;
        private PanelBase _panelLeft;
        private PanelBase _panelRight;

        public override void Awake()
        {
            base.Awake();
            Category = ThemeCategory.Structures;
            Setup("Terrain Panel", 715.0f, 0.0f, 0, true, LayoutDirection.Vertical, LayoutStart.TopLeft, "GenericPanel");
            this.CreateSpace(1.0f, 5.0f);
            CreateLabel();
            CreateContainers();
            CreateLeftSideTexturePanels();
            CreateRightSideTexturePanels();
            this.CreateSpace(1.0f, 5.0f);
        }

        private void CreateLabel()
        {
            _labelPanel = AddUIComponent<UIPanel>();
            _labelPanel.size = new Vector2(width, 22.0f);
            _label = _labelPanel.AddUIComponent<UILabel>();
            _label.font = UIUtils.BoldFont;
            _label.textScale = 1.0f;
            _label.textAlignment = UIHorizontalAlignment.Center;
            _label.verticalAlignment = UIVerticalAlignment.Middle;
            _label.padding = new RectOffset(0, 0, 4, 0);
            _label.text = Translation.Instance.GetTranslation(TranslationID.LABEL_STRUCTURES);
            _label.anchor = UIAnchorStyle.CenterHorizontal | UIAnchorStyle.CenterVertical;
            string loadTooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_LOADFROMTHEME);
            _loadButton = UIUtils.CreateButton(_label, new Vector2(22.0f, 22.0f), tooltip: loadTooltip, backgroundSprite: "ThemesIcon", atlas: UISprites.Atlas);
            _loadButton.relativePosition = new Vector2(_label.width + 5.0f, 0.0f);
            _loadButton.eventClicked += OnLoadTerrainFromTheme;
        }

        private void OnLoadTerrainFromTheme(UIComponent component, UIMouseEventParameter eventParam)
        {
            Controller.OnLoadFromTheme(ThemeCategory.Structures, ThemeCategory.Structures);
        }

        private void CreateContainers()
        {
            _container = AddUIComponent<PanelBase>();
            _container.Setup("Structures Container", 0.0f, 460.0f, 5, true);
            _container.autoFitChildrenVertically = true;
            _panelLeft = _container.AddUIComponent<PanelBase>();
            _panelLeft.Setup("Structures Panel Left", 350.0f, 0.0f, 0, true, LayoutDirection.Vertical);
            _panelRight = _container.AddUIComponent<PanelBase>();
            _panelRight.Setup("Structures Panel Right", 350.0f, 0.0f, 0, true, LayoutDirection.Vertical);
        }

        private void CreateLeftSideTexturePanels()
        {
            _panelLeft.AddUIComponent<UpwardRoadDiffusePanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);
            _panelLeft.AddUIComponent<DownwardRoadDiffusePanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);
            _panelLeft.AddUIComponent<BuildingBaseDiffusePanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);
            _panelLeft.AddUIComponent<BuildingBaseNormalPanel>();
        }

        private void CreateRightSideTexturePanels()
        {
            _panelRight.AddUIComponent<BuildingFloorDiffusePanel>();
            _panelRight.CreateSpace(1.0f, 5.0f);
            _panelRight.AddUIComponent<BuildingBurntDiffusePanel>();
            _panelRight.CreateSpace(1.0f, 5.0f);
            _panelRight.AddUIComponent<BuildingAbandonedDiffusePanel>();
            _panelRight.CreateSpace(1.0f, 5.0f);
            _panelRight.AddUIComponent<LightColorPalettePanel>();
        }
    }
}
