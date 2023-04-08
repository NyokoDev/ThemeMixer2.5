using ColossalFramework.UI;
using ThemeMixer.Locale;
using ThemeMixer.Resources;
using ThemeMixer.Themes.Enums;
using ThemeMixer.TranslationFramework;
using ThemeMixer.UI.Abstraction;
using ThemeMixer.UI.Parts.ColorPanels;
using ThemeMixer.UI.Parts.TexturePanels;
using UnityEngine;

namespace ThemeMixer.UI.CategoryPanels
{
    public class WaterPanel : PanelBase
    {
        private UIPanel _labelPanel;
        private UILabel _label;
        private UIButton _loadButton;

        private WaterFoamPanel _waterFoamPanel;
        private WaterNormalPanel _waterNormalPanel;

        private PanelBase _colorsPanel;

        private WaterCleanPanel _waterCleanColorPanel;
        private WaterUnderPanel _waterUnderColorPanel;
        private WaterDirtyPanel _waterDirtyColorPanel;

        private UIPanel _space1;
        private UIPanel _space2;

        public override void Awake()
        {
            base.Awake();
            Category = ThemeCategory.Water;
            Setup("Water Panel", 360.0f, 0.0f, 5, true, LayoutDirection.Vertical, LayoutStart.TopLeft, "GenericPanel");
            CreateLabel();
            CreateTexturePanels();
            CreateColorsPanel();
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
            _label.text = Translation.Instance.GetTranslation(TranslationID.LABEL_WATER);
            _label.anchor = UIAnchorStyle.CenterHorizontal | UIAnchorStyle.CenterVertical;
            string loadTooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_LOADFROMTHEME);
            _loadButton = UIUtils.CreateButton(_label, new Vector2(22.0f, 22.0f), tooltip: loadTooltip, backgroundSprite: "ThemesIcon", atlas: UISprites.Atlas);
            _loadButton.relativePosition = new Vector2(_label.width + 5.0f, 0.0f);
            _loadButton.eventClicked += OnLoadWaterFromTheme;
        }

        private void CreateTexturePanels()
        {
            _waterNormalPanel = AddUIComponent<WaterNormalPanel>();
            _waterFoamPanel = AddUIComponent<WaterFoamPanel>();
        }

        private void CreateColorsPanel()
        {
            _colorsPanel = AddUIComponent<PanelBase>();
            _colorsPanel.Setup("Colors Panel", 360.0f, 0.0f, 0, true, LayoutDirection.Vertical);
            _waterCleanColorPanel = _colorsPanel.AddUIComponent<WaterCleanPanel>();
            _waterCleanColorPanel.EventVisibilityChanged += OnColorPanelVisibilityChanged;
            _space1 = _colorsPanel.CreateSpace(1.0f, 5.0f);
            _waterUnderColorPanel = _colorsPanel.AddUIComponent<WaterUnderPanel>();
            _waterUnderColorPanel.EventVisibilityChanged += OnColorPanelVisibilityChanged;
            _space2 = _colorsPanel.CreateSpace(1.0f, 5.0f);
            _waterDirtyColorPanel = _colorsPanel.AddUIComponent<WaterDirtyPanel>();
            _waterDirtyColorPanel.EventVisibilityChanged += OnColorPanelVisibilityChanged;
            _colorsPanel.CreateSpace(1.0f, 5.0f);
        }

        private void OnColorPanelVisibilityChanged(object sender, ColorPanelVisibilityChangedEventArgs eventArgs)
        {
            _waterFoamPanel.isVisible = !eventArgs.Visible;
            _waterNormalPanel.isVisible = !eventArgs.Visible;
            _space1.isVisible = _space2.isVisible = !eventArgs.Visible;
            _waterCleanColorPanel.isVisible = ReferenceEquals(eventArgs.Panel, _waterCleanColorPanel) || !eventArgs.Visible;
            _waterUnderColorPanel.isVisible = ReferenceEquals(eventArgs.Panel, _waterUnderColorPanel) || !eventArgs.Visible;
            _waterDirtyColorPanel.isVisible = ReferenceEquals(eventArgs.Panel, _waterDirtyColorPanel) || !eventArgs.Visible;
        }

        private void OnLoadWaterFromTheme(UIComponent component, UIMouseEventParameter eventParam)
        {
            Controller.OnLoadFromTheme(ThemeCategory.Water, ThemeCategory.Water);
        }
    }
}
