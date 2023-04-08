using ColossalFramework.UI;
using ThemeMixer.Locale;
using ThemeMixer.Resources;
using ThemeMixer.Themes.Enums;
using ThemeMixer.TranslationFramework;
using ThemeMixer.UI.Abstraction;
using ThemeMixer.UI.Parts.ColorPanels;
using ThemeMixer.UI.Parts.TexturePanels;
using ThemeMixer.UI.Parts.ValuePanels;
using UnityEngine;

namespace ThemeMixer.UI.CategoryPanels
{
    public class AtmospherePanel : PanelBase
    {
        private UIPanel _labelPanel;
        private UILabel _label;
        private UIButton _loadButton;
        private PanelBase _container;
        private PanelBase _panelLeft;
        private PanelBase _panelCenter;
        private PanelBase _panelRight;

        private SkyTintPanel _skyTint;
        private MoonTexturePanel _moonTexture;
        private MoonInnerCoronaPanel _moonInnerCorona;
        private MoonOuterCoronaPanel _moonOuterCorona;
        private NightHorizonPanel _nightHorizonColor;
        private EarlyNightZenithPanel _earlyNightZenithColor;
        private LateNightZenithPanel _lateNightZenithColor;

        public override void Awake()
        {
            base.Awake();
            Category = ThemeCategory.Atmosphere;
            Setup("Atmosphere Panel", 1070.0f, 0.0f, 0, true, LayoutDirection.Vertical, LayoutStart.TopLeft, "GenericPanel");
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
            _label.text = Translation.Instance.GetTranslation(TranslationID.LABEL_ATMOSPHERE);
            _label.anchor = UIAnchorStyle.CenterHorizontal | UIAnchorStyle.CenterVertical;
            string loadTooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_LOADFROMTHEME);
            _loadButton = UIUtils.CreateButton(_label, new Vector2(22.0f, 22.0f), tooltip: loadTooltip, backgroundSprite: "ThemesIcon", atlas: UISprites.Atlas);
            _loadButton.relativePosition = new Vector2(_label.width + 5.0f, 0.0f);
            _loadButton.eventClicked += OnLoadWeatherFromTheme;
        }

        private void OnLoadWeatherFromTheme(UIComponent component, UIMouseEventParameter eventParam)
        {
            Controller.OnLoadFromTheme(ThemeCategory.Atmosphere, ThemeCategory.Atmosphere);
        }

        private void CreateContainers()
        {
            _container = AddUIComponent<PanelBase>();
            _container.Setup("Atmosphere Container", 0.0f, 460.0f, 5, true);
            _container.autoFitChildrenVertically = true;
            _panelLeft = _container.AddUIComponent<PanelBase>();
            _panelLeft.Setup("Atmosphere Panel Left", 350.0f, 0.0f, 0, true, LayoutDirection.Vertical);
            _panelCenter = _container.AddUIComponent<PanelBase>();
            _panelCenter.Setup("Atmosphere Panel Center", 350.0f, 0.0f, 0, true, LayoutDirection.Vertical);
            _panelRight = _container.AddUIComponent<PanelBase>();
            _panelRight.Setup("Atmosphere Panel Right", 350.0f, 0.0f, 0, true, LayoutDirection.Vertical);
            _panelRight.autoLayoutPadding = new RectOffset(0, 0, 0, 8);
        }

        private void CreatePanels()
        {
            _panelLeft.AddUIComponent<LongitudePanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);
            _panelLeft.AddUIComponent<LatitudePanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);
            _panelLeft.AddUIComponent<SunSizePanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);
            _panelLeft.AddUIComponent<SunAnisotropyPanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);
            _panelLeft.AddUIComponent<ExposurePanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);

            _panelCenter.AddUIComponent<RayleighPanel>();
            _panelCenter.CreateSpace(1.0f, 5.0f);
            _panelCenter.AddUIComponent<MiePanel>();
            _panelCenter.CreateSpace(1.0f, 5.0f);
            _panelCenter.AddUIComponent<StarsIntensityPanel>();
            _panelCenter.CreateSpace(1.0f, 5.0f);
            _panelCenter.AddUIComponent<OuterSpaceIntensityPanel>();
            _panelCenter.CreateSpace(1.0f, 5.0f);
            _panelCenter.AddUIComponent<MoonSizePanel>();
            _panelCenter.CreateSpace(1.0f, 5.0f);


            _skyTint = _panelRight.AddUIComponent<SkyTintPanel>();
            _skyTint.EventVisibilityChanged += OnColorPanelVisibilityChanged;
            _earlyNightZenithColor = _panelRight.AddUIComponent<EarlyNightZenithPanel>();
            _earlyNightZenithColor.EventVisibilityChanged += OnColorPanelVisibilityChanged;
            _lateNightZenithColor = _panelRight.AddUIComponent<LateNightZenithPanel>();
            _lateNightZenithColor.EventVisibilityChanged += OnColorPanelVisibilityChanged;
            _nightHorizonColor = _panelRight.AddUIComponent<NightHorizonPanel>();
            _nightHorizonColor.EventVisibilityChanged += OnColorPanelVisibilityChanged;
            _moonInnerCorona = _panelRight.AddUIComponent<MoonInnerCoronaPanel>();
            _moonInnerCorona.EventVisibilityChanged += OnColorPanelVisibilityChanged;
            _moonOuterCorona = _panelRight.AddUIComponent<MoonOuterCoronaPanel>();
            _moonOuterCorona.EventVisibilityChanged += OnColorPanelVisibilityChanged;
            _moonTexture = _panelRight.AddUIComponent<MoonTexturePanel>();
        }

        private void OnColorPanelVisibilityChanged(object sender, ColorPanelVisibilityChangedEventArgs eventArgs)
        {
            bool isSkyTint = ReferenceEquals(eventArgs.Panel, _skyTint);
            bool isInnerCorona = ReferenceEquals(eventArgs.Panel, _moonInnerCorona);
            bool isOuterCorona = ReferenceEquals(eventArgs.Panel, _moonOuterCorona);
            bool isNightHorizon = ReferenceEquals(eventArgs.Panel, _nightHorizonColor);
            bool isEarlyNightZenith = ReferenceEquals(eventArgs.Panel, _earlyNightZenithColor);
            bool isLateNightZenith = ReferenceEquals(eventArgs.Panel, _lateNightZenithColor);

            _skyTint.isVisible = isSkyTint || !eventArgs.Visible;
            _moonInnerCorona.isVisible = isInnerCorona || !eventArgs.Visible;
            _moonOuterCorona.isVisible = isOuterCorona || !eventArgs.Visible;
            _nightHorizonColor.isVisible = isNightHorizon || !eventArgs.Visible;
            _earlyNightZenithColor.isVisible = isEarlyNightZenith || !eventArgs.Visible;
            _lateNightZenithColor.isVisible = isLateNightZenith || !eventArgs.Visible;
            _moonTexture.isVisible = !eventArgs.Visible;

            eventArgs.Panel.backgroundSprite = eventArgs.Visible ? "" : "WhiteRect";
        }
    }
}
