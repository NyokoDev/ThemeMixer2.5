using ColossalFramework.UI;
using ThemeMixer.Locale;
using ThemeMixer.Resources;
using ThemeMixer.Themes.Enums;
using ThemeMixer.TranslationFramework;
using ThemeMixer.UI.Abstraction;
using ThemeMixer.UI.Parts.ValuePanels;
using UnityEngine;

namespace ThemeMixer.UI.CategoryPanels
{
    public class WeatherPanel : PanelBase
    {
        private UIPanel _labelPanel;
        private UILabel _label;
        private UIButton _loadButton;

        private PanelBase _container;
        private PanelBase _panelLeft;
        private PanelBase _panelCenter;
        private PanelBase _panelRight;

        public override void Awake()
        {
            base.Awake();
            Category = ThemeCategory.Weather;
            Setup("Weather Panel", 1070.0f, 0.0f, 0, true, LayoutDirection.Vertical, LayoutStart.TopLeft, "GenericPanel");
            this.CreateSpace(1.0f, 5.0f);
            CreateLabel();
            CreateContainers();
            CreatePanels();
            this.CreateSpace(0.0f, 0.1f);
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
            _label.text = Translation.Instance.GetTranslation(TranslationID.LABEL_WEATHER);
            _label.anchor = UIAnchorStyle.CenterHorizontal | UIAnchorStyle.CenterVertical;
            string loadTooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_LOADFROMTHEME);
            _loadButton = UIUtils.CreateButton(_label, new Vector2(22.0f, 22.0f), tooltip: loadTooltip, backgroundSprite: "ThemesIcon", atlas: UISprites.Atlas);
            _loadButton.relativePosition = new Vector2(_label.width + 5.0f, 0.0f);
            _loadButton.eventClicked += OnLoadWeatherFromTheme;
        }

        private void OnLoadWeatherFromTheme(UIComponent component, UIMouseEventParameter eventParam)
        {
            Controller.OnLoadFromTheme(ThemeCategory.Weather, ThemeCategory.Weather);
        }

        private void CreateContainers()
        {
            _container = AddUIComponent<PanelBase>();
            _container.Setup("Weather Container", 0.0f, 460.0f, 5, true);
            _container.autoFitChildrenVertically = true;
            _panelLeft = _container.AddUIComponent<PanelBase>();
            _panelLeft.Setup("Weather Panel Left", 350.0f, 0.0f, 0, true, LayoutDirection.Vertical);
            _panelCenter = _container.AddUIComponent<PanelBase>();
            _panelCenter.Setup("Weather Panel Center", 350.0f, 0.0f, 0, true, LayoutDirection.Vertical);
            _panelRight = _container.AddUIComponent<PanelBase>();
            _panelRight.Setup("Weather Panel Right", 350.0f, 0.0f, 0, true, LayoutDirection.Vertical);
        }

        private void CreatePanels()
        {
            _panelLeft.AddUIComponent<RainProbabilityDayPanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);
            _panelLeft.AddUIComponent<RainProbabilityNightPanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);
            _panelLeft.AddUIComponent<FogProbabilityDayPanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);
            _panelLeft.AddUIComponent<FogProbabilityNightPanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);
            _panelLeft.AddUIComponent<NorthernLightsProbabilityPanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);

            _panelCenter.AddUIComponent<MinTemperatureDayPanel>();
            _panelCenter.CreateSpace(1.0f, 5.0f);
            _panelCenter.AddUIComponent<MaxTemperatureDayPanel>();
            _panelCenter.CreateSpace(1.0f, 5.0f);
            _panelCenter.AddUIComponent<MinTemperatureNightPanel>();
            _panelCenter.CreateSpace(1.0f, 5.0f);
            _panelCenter.AddUIComponent<MaxTemperatureNightPanel>();
            _panelCenter.CreateSpace(1.0f, 5.0f);

            _panelRight.AddUIComponent<MinTemperatureRainPanel>();
            _panelRight.CreateSpace(1.0f, 5.0f);
            _panelRight.AddUIComponent<MaxTemperatureRainPanel>();
            _panelRight.CreateSpace(1.0f, 5.0f);
            _panelRight.AddUIComponent<MinTemperatureFogPanel>();
            _panelRight.CreateSpace(1.0f, 5.0f);
            _panelRight.AddUIComponent<MaxTemperatureFogPanel>();
            _panelRight.CreateSpace(1.0f, 5.0f);
        }
    }
}
