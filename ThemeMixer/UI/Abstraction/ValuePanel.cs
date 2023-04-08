using System;
using ColossalFramework.UI;
using ThemeMixer.Locale;
using ThemeMixer.Resources;
using ThemeMixer.Themes.Enums;
using ThemeMixer.TranslationFramework;
using UnityEngine;

namespace ThemeMixer.UI.Abstraction
{
    public abstract class ValuePanel : PanelBase
    {
        public ValueID ValueID;
        private PanelBase _containerTitle;
        private PanelBase _containerSlider;
        private UILabel _labelTitle;
        private UISlider _slider;
        private UITextField _textfield;
        private UIButton _resetButton;
        private float _defaultValue;
        private bool _ignoreEvents;

        public override void Awake()
        {
            base.Awake();
            _ignoreEvents = true;
            Setup("Value Panel", 350.0f, 0.0f, 5, true, LayoutDirection.Vertical, LayoutStart.TopLeft, "WhiteRect");
            autoLayout = false;
            CreateUIElements();
            CacheDefaultValue();
            SetupLabels();
            SetupButtons();
            SetupSlider();
            SetupTextfield();
            autoLayout = true;
            _ignoreEvents = false;
        }

        private void CreateUIElements()
        {
            _containerTitle = AddUIComponent<PanelBase>();
            _containerTitle.size = new Vector2(340.0f, 22.0f);
            _containerTitle.padding = new RectOffset(5, 0, 5, 0);
            _containerSlider = AddUIComponent<PanelBase>();
            _containerSlider.size = new Vector2(340.0f, 22.0f);
            _containerSlider.padding = new RectOffset(5, 0, 5, 0);

            _labelTitle = _containerTitle.AddUIComponent<UILabel>();
            string resetTooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_RESET);
            _resetButton = UIUtils.CreateButton(_containerTitle, new Vector2(22.0f, 22.0f), tooltip: resetTooltip, backgroundSprite: "", foregroundSprite: "UndoIcon", atlas: UISprites.Atlas);

            _slider = UIUtils.CreateSlider(_containerSlider, 240.0f, 0.0f, 1.0f, 1.0f);
            _textfield = _containerSlider.AddUIComponent<UITextField>();

            this.CreateSpace(0.0f, 0.01f);

            color = UIColorGrey;
        }

        private void OnResetClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            _ignoreEvents = true;
            _slider.value = _defaultValue;
            _textfield.text = _defaultValue.ToString("0.####");
            _ignoreEvents = false;
            SetValue(_defaultValue);
        }

        private void SetupLabels()
        {
            string title = Translation.Instance.GetTranslation(TranslationID.ValueToTranslationID(ValueID));
            SetupLabel(_labelTitle, title, new Vector2(0.0f, 0.0f), new Vector2(340.0f, 22.0f));
        }

        private static void SetupLabel(UILabel label, string text, Vector2 labelPosition, Vector2 labelSize)
        {
            label.autoSize = false;
            label.autoHeight = true;
            label.size = labelSize;
            label.font = UIUtils.Font;
            label.textScale = 1.0f;
            label.padding = new RectOffset(4, 0, 4, 0);
            label.text = text;
            label.relativePosition = labelPosition;
        }

        private void SetupButtons()
        {
            _resetButton.eventClicked += OnResetClicked;
            _resetButton.relativePosition = new Vector2(308.0f, 0.0f);
        }

        private void SetupSlider()
        {
            _slider.eventValueChanged += OnSliderValueChanged;
            _slider.relativePosition = new Vector2(5.0f, 6.0f);
            _slider.tooltip = Translation.Instance.GetTranslation(TranslationID.GetValueTooltipID(ValueID));
            GetSliderMinMaxAndStep(out float min, out float max, out float step);
            _slider.minValue = min;
            _slider.maxValue = max;
            _slider.stepSize = step;
            _slider.scrollWheelAmount = step;
            _slider.value = _defaultValue;
        }

        private void SetupTextfield()
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
            _textfield.text = _defaultValue.ToString("0.####");
            _textfield.tooltip = Translation.Instance.GetTranslation(TranslationID.GetValueTooltipID(ValueID));
            _textfield.eventTextSubmitted += OnTextfieldTextSubmitted;
            _textfield.eventKeyPress += OnTextfieldKeyPress;
            _textfield.eventLostFocus += OnTextfieldLostFocus;
            _textfield.relativePosition = new Vector2(261.0f, 0.0f);
        }

        private void OnTextfieldTextSubmitted(UIComponent component, string value)
        {
            if (_ignoreEvents) return;
            var textfield = component as UITextField;
            if (!float.TryParse(textfield?.text.Replace(',', '.'), out float f)) return;
            if (textfield != null) textfield.text = f.ToString("0.####");
            _slider.value = f;
        }

        private void OnTextfieldLostFocus(UIComponent component, UIFocusEventParameter eventParam)
        {
            if (_ignoreEvents) return;
            var textfield = component as UITextField;
            OnTextfieldTextSubmitted(component, textfield?.text);
        }

        private void OnTextfieldKeyPress(UIComponent component, UIKeyEventParameter eventParam)
        {
            if (_ignoreEvents) return;
            var textfield = component as UITextField;
            char ch = eventParam.character;
            if (textfield != null && (!char.IsControl(ch) && !char.IsDigit(ch) &&
                  (ch != '.' || (ch == '.' && textfield.text.Contains(".") || !CanHaveDecimals())) &&
                  (ch != ',' || (ch == ',' && textfield.text.Contains(",") || !CanHaveDecimals())) &&
                  (ch != '-' || (ch == '-' && textfield.text.Contains("-") || !CanBeNegative()))))
            {
                eventParam.Use();
            }

            if (eventParam.keycode != KeyCode.Escape) return;
            textfield?.Unfocus();
            eventParam.Use();
        }

        private void OnSliderValueChanged(UIComponent component, float value)
        {
            if (_ignoreEvents) return;
            string valueString = value.ToString("0.####");
            _textfield.text = valueString;
            SetValue(value);
        }

        protected override void OnRefreshUI(object sender, UIDirtyEventArgs eventArgs)
        {
            base.OnRefreshUI(sender, eventArgs);
            try
            {
                _labelTitle.text = Translation.Instance.GetTranslation(TranslationID.ValueToTranslationID(ValueID));
                CacheDefaultValue();
                _ignoreEvents = true;
                _slider.value = _defaultValue;
                _textfield.text = _defaultValue.ToString("0.####");
                _ignoreEvents = false;
            }
            catch (Exception)
            {
                Debug.Log("Exception caught in TexturePanel.OnRefreshUI");
            }
        }
        private void CacheDefaultValue()
        {
            switch (ValueID)
            {
                case ValueID.Longitude:
                case ValueID.Latitude:
                case ValueID.SunSize:
                case ValueID.SunAnisotropy:
                case ValueID.MoonSize:
                case ValueID.Rayleigh:
                case ValueID.Mie:
                case ValueID.Exposure:
                case ValueID.StarsIntensity:
                case ValueID.OuterSpaceIntensity:
                case ValueID.MinTemperatureDay:
                case ValueID.MaxTemperatureDay:
                case ValueID.MinTemperatureNight:
                case ValueID.MaxTemperatureNight:
                case ValueID.MinTemperatureRain:
                case ValueID.MaxTemperatureRain:
                case ValueID.MinTemperatureFog:
                case ValueID.MaxTemperatureFog:
                    _defaultValue = Controller.GetValue<float>(ValueID);
                    break;
                case ValueID.RainProbabilityDay:
                case ValueID.RainProbabilityNight:
                case ValueID.FogProbabilityDay:
                case ValueID.FogProbabilityNight:
                case ValueID.NorthernLightsProbability:
                    _defaultValue = Controller.GetValue<int>(ValueID);
                    break;
            }
        }

        private void SetValue(float value)
        {
            switch (ValueID)
            {
                case ValueID.Longitude:
                case ValueID.Latitude:
                case ValueID.SunSize:
                case ValueID.SunAnisotropy:
                case ValueID.MoonSize:
                case ValueID.Rayleigh:
                case ValueID.Mie:
                case ValueID.Exposure:
                case ValueID.StarsIntensity:
                case ValueID.OuterSpaceIntensity:
                case ValueID.MinTemperatureDay:
                case ValueID.MaxTemperatureDay:
                case ValueID.MinTemperatureNight:
                case ValueID.MaxTemperatureNight:
                case ValueID.MinTemperatureRain:
                case ValueID.MaxTemperatureRain:
                case ValueID.MinTemperatureFog:
                case ValueID.MaxTemperatureFog:
                    Controller.OnValueChanged(ValueID, value);
                    break;
                case ValueID.RainProbabilityDay:
                case ValueID.RainProbabilityNight:
                case ValueID.FogProbabilityDay:
                case ValueID.FogProbabilityNight:
                case ValueID.NorthernLightsProbability:
                    Controller.OnValueChanged(ValueID, (int)value);
                    break;
            }
        }

        private bool CanBeNegative()
        {
            switch (ValueID)
            {
                case ValueID.Longitude:
                case ValueID.Latitude:
                case ValueID.MinTemperatureDay:
                case ValueID.MaxTemperatureDay:
                case ValueID.MinTemperatureNight:
                case ValueID.MaxTemperatureNight:
                case ValueID.MinTemperatureRain:
                case ValueID.MaxTemperatureRain:
                case ValueID.MinTemperatureFog:
                case ValueID.MaxTemperatureFog:
                    return true;
                default: return false;
            }
        }

        private bool CanHaveDecimals()
        {
            switch (ValueID)
            {
                case ValueID.Longitude:
                case ValueID.Latitude:
                case ValueID.SunSize:
                case ValueID.SunAnisotropy:
                case ValueID.MoonSize:
                case ValueID.Rayleigh:
                case ValueID.Mie:
                case ValueID.Exposure:
                case ValueID.StarsIntensity:
                case ValueID.OuterSpaceIntensity:
                case ValueID.MinTemperatureDay:
                case ValueID.MaxTemperatureDay:
                case ValueID.MinTemperatureNight:
                case ValueID.MaxTemperatureNight:
                case ValueID.MinTemperatureRain:
                case ValueID.MaxTemperatureRain:
                case ValueID.MinTemperatureFog:
                case ValueID.MaxTemperatureFog:
                    return true;
                case ValueID.RainProbabilityDay:
                case ValueID.RainProbabilityNight:
                case ValueID.FogProbabilityDay:
                case ValueID.FogProbabilityNight:
                case ValueID.NorthernLightsProbability:
                    return false;
            }
            return false;
        }

        private void GetSliderMinMaxAndStep(out float minValue, out float maxValue, out float step)
        {
            minValue = 0.0f;
            maxValue = 1.0f;
            step = 1.0f;
            switch (ValueID)
            {
                case ValueID.Longitude:
                    minValue = -180.0f;
                    maxValue = 180.0f;
                    step = 1.0f;
                    break;
                case ValueID.Latitude:
                    minValue = -80.0f;
                    maxValue = 80.0f;
                    step = 1.0f;
                    break;
                case ValueID.SunSize:
                    minValue = 0.001f;
                    maxValue = 10.0f;
                    step = 0.001f;
                    break;
                case ValueID.SunAnisotropy:
                case ValueID.MoonSize:
                    minValue = 0.0f;
                    maxValue = 1.0f;
                    step = 0.001f;
                    break;
                case ValueID.Rayleigh:
                case ValueID.Mie:
                case ValueID.StarsIntensity:
                case ValueID.Exposure:
                case ValueID.OuterSpaceIntensity:
                    minValue = 0.0f;
                    maxValue = 5.0f;
                    step = 0.001f;
                    break;
                case ValueID.MinTemperatureDay:
                case ValueID.MaxTemperatureDay:
                case ValueID.MinTemperatureNight:
                case ValueID.MaxTemperatureNight:
                case ValueID.MinTemperatureRain:
                case ValueID.MaxTemperatureRain:
                case ValueID.MinTemperatureFog:
                case ValueID.MaxTemperatureFog:
                    minValue = -50.0f;
                    maxValue = 50.0f;
                    step = 0.1f;
                    break;
                case ValueID.RainProbabilityDay:
                case ValueID.RainProbabilityNight:
                case ValueID.FogProbabilityDay:
                case ValueID.FogProbabilityNight:
                case ValueID.NorthernLightsProbability:
                    minValue = 0.0f;
                    maxValue = 100.0f;
                    step = 1.0f;
                    break;
            }
        }
    }
}
