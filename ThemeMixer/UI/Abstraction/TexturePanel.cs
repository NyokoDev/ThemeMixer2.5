using System;
using System.Globalization;
using ColossalFramework.UI;
using ThemeMixer.Locale;
using ThemeMixer.Resources;
using ThemeMixer.Themes.Enums;
using ThemeMixer.TranslationFramework;
using UnityEngine;

namespace ThemeMixer.UI.Abstraction
{
    public abstract class TexturePanel : PanelBase
    {
        public TextureID TextureID;
        private PanelBase _container;
        private UIButton _thumbBackground;
        private UIPanel _thumbMiddleGround;
        private UISprite _thumb;
        private UILabel _label;
        private UISlider _slider;
        private UITextField _textfield;

        private UIButton _loadButton;
        private UIButton _resetButton;

        private float _defaultValue;
        private bool _ignoreEvents;

        public override void Awake()
        {
            base.Awake();
            Setup("Texture Panel Container", 350.0f, 0.0f, UIUtils.DefaultSpacing, true, LayoutDirection.Vertical, LayoutStart.TopLeft, "WhiteRect");
            autoLayout = false;
            CreateUIElements();
            _defaultValue = Controller.GetTilingValue(TextureID);
            SetupThumbnail();
            SetupLabel();
            SetupButtons();
            SetupSlider();
            SetupSliderTextfield();
            autoLayout = true;
        }

        private void CreateUIElements()
        {
            _container = AddUIComponent<PanelBase>();
            _container.size = new Vector2(340.0f, 66.0f);
            this.CreateSpace(340.0f, 0.1f);
            _thumbBackground = _container.AddUIComponent<UIButton>();
            _thumbMiddleGround = _container.AddUIComponent<UIPanel>();
            _thumb = _container.AddUIComponent<UISprite>();
            _label = _container.AddUIComponent<UILabel>();
            _slider = UIUtils.CreateSlider(_container, 165.0f, 1.0f, 2000.0f, 1.0f);
            _textfield = _container.AddUIComponent<UITextField>();
            string loadTooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_LOADFROMTHEME);
            _loadButton = UIUtils.CreateButton(_container, new Vector2(22.0f, 22.0f), tooltip: loadTooltip, backgroundSprite: "ThemesIcon", atlas: UISprites.Atlas);
            string resetTooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_RESET);
            _resetButton = UIUtils.CreateButton(_container, new Vector2(22.0f, 22.0f), tooltip: resetTooltip, backgroundSprite: "", foregroundSprite: "UndoIcon", atlas: UISprites.Atlas);
            color = UIColorGrey;
        }

        private void SetupButtons()
        {
            _loadButton.eventClicked += OnLoadClicked;
            _loadButton.relativePosition = new Vector2(281.0f, 5.0f);
            _resetButton.eventClicked += OnResetClicked;
            _resetButton.relativePosition = new Vector2(308.0f, 5.0f);
            if (Category == ThemeCategory.Terrain) return;
            _resetButton.Hide();
            _loadButton.relativePosition = _resetButton.relativePosition;
            _loadButton.anchor |= UIAnchorStyle.CenterVertical;
        }
        private void OnResetClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            _ignoreEvents = true;
            _slider.value = _defaultValue * 10000.0f;
            _textfield.text = string.Concat(Math.Round(_defaultValue, 4, MidpointRounding.AwayFromZero));
            _ignoreEvents = false;
            Controller.OnTilingChanged(TextureID, _defaultValue);
        }

        private void OnLoadClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            Controller.OnLoadFromTheme(Category, TextureID);
        }

        private void SetupSliderTextfield()
        {
            _textfield.atlas = UISprites.DefaultAtlas;
            _textfield.size = new Vector2(70.0f, 21.0f);
            _textfield.padding = new RectOffset(2, 2, 4, 4);
            _textfield.builtinKeyNavigation = true;
            _textfield.isInteractive = true;
            _textfield.readOnly = false;
            _textfield.selectOnFocus = true;
            _textfield.horizontalAlignment = UIHorizontalAlignment.Center;
            _textfield.selectionSprite = "EmptySprite";
            _textfield.selectionBackgroundColor = new Color32(0, 172, 234, 255);
            _textfield.normalBgSprite = "TextFieldPanelHovered";
            _textfield.textColor = new Color32(0, 0, 0, 255);
            _textfield.textScale = 0.85f;
            _textfield.color = new Color32(255, 255, 255, 255);
            _textfield.relativePosition = new Vector2(261.0f, 35.0f);
            _textfield.text = string.Concat(Math.Round(Controller.GetTilingValue(TextureID), 4, MidpointRounding.AwayFromZero));
            _textfield.eventTextSubmitted += OnTextfieldTextSubmitted;
            _textfield.eventKeyPress += OnTextfieldKeyPress;
            _textfield.eventLostFocus += OnTextfieldLostFocus;
            _textfield.tooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_TILING);
            if (Category != ThemeCategory.Terrain) _textfield.isVisible = false;
        }

        private void OnTextfieldTextSubmitted(UIComponent component, string value)
        {
            if (_ignoreEvents) return;
            if (float.TryParse(_textfield.text.Replace(',', '.'), out float f))
            {
                float finalValue = Mathf.Clamp(f, 0.0001f, 0.2f);
                _textfield.text = finalValue.ToString(CultureInfo.InvariantCulture);
                _slider.value = finalValue * 10000.0f;
            }
        }

        private void OnTextfieldLostFocus(UIComponent component, UIFocusEventParameter eventParam)
        {
            if (_ignoreEvents) return;
            OnTextfieldTextSubmitted(_textfield, _textfield.text);
        }

        private void OnTextfieldKeyPress(UIComponent component, UIKeyEventParameter eventParam)
        {
            if (_ignoreEvents) return;
            char ch = eventParam.character;
            if (!char.IsControl(ch) && !char.IsDigit(ch) && (ch != '.' || (ch == '.' && _textfield.text.Contains("."))) && (ch != ',' || (ch == ',' && _textfield.text.Contains(","))))
            {
                eventParam.Use();
            }
            if (eventParam.keycode == KeyCode.Escape)
            {
                _textfield.Unfocus();
                eventParam.Use();
            }
        }

        private void SetupSlider()
        {
            _slider.relativePosition = new Vector2(80.0f, 40.0f);
            _slider.value = Controller.GetTilingValue(TextureID) * 10000.0f;
            _slider.eventValueChanged += OnSliderValueChanged;
            _slider.tooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_TILING);
            if (Category != ThemeCategory.Terrain) _slider.isVisible = false;
        }

        private void SetupLabel()
        {
            _label.relativePosition = new Vector2(71.0f, 5.0f);
            _label.autoSize = false;
            _label.autoHeight = true;
            _label.size = new Vector2(219.0f, 22.0f);
            _label.font = UIUtils.Font;
            _label.textScale = 1.0f;
            _label.padding = new RectOffset(4, 0, 4, 0);
            _label.text = Translation.Instance.GetTranslation(TranslationID.TextureToTranslationID(TextureID));
            if (Category != ThemeCategory.Terrain) _label.anchor |= UIAnchorStyle.CenterVertical;
        }

        private void SetupThumbnail()
        {
            _thumbBackground.normalBgSprite = "WhiteRect";
            _thumbBackground.relativePosition = new Vector2(0.0f, 0.0f);
            _thumbBackground.size = new Vector2(66.0f, 66.0f);
            _thumbBackground.color = UIColorLight;
            _thumbBackground.focusedColor = UIColorLight;
            _thumbBackground.hoveredColor = new Color32(20, 155, 215, 255);
            _thumbBackground.pressedColor = new Color32(20, 155, 215, 255);
            _thumbBackground.eventClicked += OnTextureClicked;
            string select = Translation.Instance.GetTranslation(TranslationID.LABEL_SELECT);
            string texture = Translation.Instance.GetTranslation(TranslationID.LABEL_TEXTURE);
            _thumbBackground.tooltip = string.Concat(select, " ", texture);

            _thumbMiddleGround.backgroundSprite = "WhiteRect";
            _thumbMiddleGround.relativePosition = new Vector2(2.0f, 2.0f);
            _thumbMiddleGround.size = new Vector2(62.0f, 62.0f);
            _thumbMiddleGround.isInteractive = false;

            _thumb.size = new Vector2(62.0f, 62.0f);
            _thumb.atlas = ThemeSprites.Atlas;
            _thumb.spriteName = UIUtils.GetTextureSpriteName(TextureID);
            _thumb.relativePosition = new Vector2(2.0f, 2.0f);
            _thumb.isInteractive = false;
        }

        private void OnTextureClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (_ignoreEvents) return;
            Controller.OnLoadFromTheme(Category, TextureID);
        }

        private void OnSliderValueChanged(UIComponent component, float value)
        {
            if (_ignoreEvents) return;
            float finalValue = value / 10000.0f;
            Controller.OnTilingChanged(TextureID, finalValue);
            _textfield.text = string.Concat(Math.Round(finalValue, 4, MidpointRounding.AwayFromZero));
        }

        protected override void OnRefreshUI(object sender, UIDirtyEventArgs eventArgs)
        {
            base.OnRefreshUI(sender, eventArgs);
            try
            {
                float tiling = Controller.GetTilingValue(TextureID);
                _defaultValue = tiling;
                _thumb.spriteName = UIUtils.GetTextureSpriteName(TextureID);
                _label.text = Translation.Instance.GetTranslation(TranslationID.TextureToTranslationID(TextureID));
                _ignoreEvents = true;
                _slider.value = tiling * 10000.0f;
                _textfield.text = string.Concat(Math.Round(tiling, 4, MidpointRounding.AwayFromZero));
                _ignoreEvents = false;
            }
            catch (Exception)
            {
                Debug.Log("Exception caught in TexturePanel.OnRefreshUI");
            }
        }
    }
}
