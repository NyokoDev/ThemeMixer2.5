using ColossalFramework.UI;
using ThemeMixer.Locale;
using ThemeMixer.Resources;
using ThemeMixer.Themes.Enums;
using ThemeMixer.TranslationFramework;
using ThemeMixer.UI.Abstraction;
using UnityEngine;

namespace ThemeMixer.UI
{
    public class ButtonBar : PanelBase
    {
        public delegate void ButtonClickedEventHandler(ToolbarButton button, ToolbarButton[] buttons);
        public event ButtonClickedEventHandler EventButtonClicked;

        private ToolbarButton _themesButton;
        private ToolbarButton _terrainButton;
        private ToolbarButton _waterButton;
        private ToolbarButton _structuresButton;
        private ToolbarButton _atmosphereButton;
        private ToolbarButton _weatherButton;
        private ToolbarButton _lutButton;

        private ToolbarButton _mixesButton;

        private ToolbarButton[] _buttons;

        public override void Awake()
        {
            base.Awake();
            Setup("Button Bar", 40.0f, 0.0f, UIUtils.DefaultSpacing, true, LayoutDirection.Vertical);
            CreateButtons();
            var space = AddUIComponent<UIPanel>();
            space.size = new Vector2(width, 0.1f);
        }

        public override void OnDestroy()
        {
            _themesButton.EventClicked -= OnButtonClicked;
            _terrainButton.EventClicked -= OnButtonClicked;
            _waterButton.EventClicked -= OnButtonClicked;
            _atmosphereButton.EventClicked -= OnButtonClicked;
            _structuresButton.EventClicked -= OnButtonClicked;
            _weatherButton.EventClicked -= OnButtonClicked;
            _lutButton.EventClicked -= OnButtonClicked;
            _mixesButton.EventClicked -= OnButtonClicked;
            base.OnDestroy();
        }

        private void CreateButtons()
        {

            _themesButton = new ToolbarButton(ThemeCategory.Themes, this);
            _themesButton.EventClicked += OnButtonClicked;

            _terrainButton = new ToolbarButton(ThemeCategory.Terrain, this);
            _terrainButton.EventClicked += OnButtonClicked;

            _waterButton = new ToolbarButton(ThemeCategory.Water, this);
            _waterButton.EventClicked += OnButtonClicked;

            _structuresButton = new ToolbarButton(ThemeCategory.Structures, this);
            _structuresButton.EventClicked += OnButtonClicked;

            _atmosphereButton = new ToolbarButton(ThemeCategory.Atmosphere, this);
            _atmosphereButton.EventClicked += OnButtonClicked;

            _weatherButton = new ToolbarButton(ThemeCategory.Weather, this);
            _weatherButton.EventClicked += OnButtonClicked;

            _lutButton = new ToolbarButton(ThemeCategory.None, this);
            _lutButton.EventClicked += OnButtonClicked;

            _mixesButton = new ToolbarButton(ThemeCategory.Mixes, this);
            _mixesButton.EventClicked += OnButtonClicked;

            CreateButtonArray();
        }

        private void OnButtonClicked(ToolbarButton button)
        {
            EventButtonClicked?.Invoke(button, _buttons);
        }

        private void CreateButtonArray()
        {
            _buttons = new[] {
                _themesButton,
                _terrainButton,
                _waterButton,
                _atmosphereButton,
                _structuresButton,
                _weatherButton,
                _lutButton,
                _mixesButton
            };
        }
    }

    public class ToolbarButton
    {
        public delegate void ButtonClickedEventHandler(ToolbarButton button);
        public event ButtonClickedEventHandler EventClicked;

        public ThemeCategory Category;
        public UIButton Button;

        private static readonly Vector2 ButtonSize = new Vector2(40.0f, 40.0f);

        public ToolbarButton(ThemeCategory part, UIComponent parent)
        {
            Category = part;
            string icon;
            string locale;
            switch (part)
            {
                case ThemeCategory.Themes:
                    icon = UISprites.ThemesIcon;
                    locale = TranslationID.TOOLTIP_THEMES;
                    break;
                case ThemeCategory.Terrain:
                    icon = UISprites.TerrainIcon;
                    locale = TranslationID.TOOLTIP_TERRAIN;
                    break;
                case ThemeCategory.Water:
                    icon = UISprites.WaterIcon;
                    locale = TranslationID.TOOLTIP_WATER;
                    break;
                case ThemeCategory.Structures:
                    icon = UISprites.StructuresIcon;
                    locale = TranslationID.TOOLTIP_STRUCTURES;
                    break;
                case ThemeCategory.Atmosphere:
                    icon = UISprites.AtmosphereIcon;
                    locale = TranslationID.TOOLTIP_ATMOSPHERE;
                    break;
                case ThemeCategory.Weather:
                    icon = UISprites.WeatherIcon;
                    locale = TranslationID.TOOLTIP_WEATHER;
                    break;
                case ThemeCategory.Mixes:
                    icon = UISprites.SettingsIcon;
                    locale = TranslationID.TOOLTIP_MIXES;
                    break;
                default:
                    icon = UISprites.LutIcon;
                    locale = TranslationID.TOOLTIP_LUTS;
                    break;
            }
            Button = UIUtils.CreateButton(parent, ButtonSize, foregroundSprite: UISprites.IconBorder, backgroundSprite: icon, atlas: UISprites.Atlas, isFocusable: true, tooltip: Translation.Instance.GetTranslation(locale));
            Button.eventClicked += OnButtonClicked;
        }

        private void OnButtonClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            EventClicked?.Invoke(this);
        }
    }
}