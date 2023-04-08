using System;
using ColossalFramework.UI;
using ThemeMixer.Locale;
using ThemeMixer.Resources;
using ThemeMixer.Themes.Enums;
using ThemeMixer.TranslationFramework;
using UnityEngine;

namespace ThemeMixer.UI.Abstraction
{
    public abstract class OffsetPanel : PanelBase
    {
        public OffsetID OffsetID;
        private PanelBase _containerTitle;
        private PanelBase _containerR;
        private PanelBase _containerG;
        private PanelBase _containerB;
        private UILabel _labelTitle;
        private UILabel _labelR;
        private UILabel _labelG;
        private UILabel _labelB;
        private UISlider _sliderR;
        private UISlider _sliderG;
        private UISlider _sliderB;
        private UITextField _textfieldR;
        private UITextField _textfieldG;
        private UITextField _textfieldB;
        private UIButton _loadButton;
        private UIButton _resetButton;
        private Vector3 _defaultValue;
        private bool _ignoreEvents;

        public override void Awake()
        {
            base.Awake();
            Setup("Offset Panel", 350.0f, 0.0f, 5, true, LayoutDirection.Vertical, LayoutStart.TopLeft, "WhiteRect");
            CreateUIElements();
            _defaultValue = Controller.GetOffsetValue(OffsetID);
            SetupLabels();
            SetupButtons();
            SetupSliders();
            SetupTextfields();
        }

        private void CreateUIElements()
        {
            _containerTitle = AddUIComponent<PanelBase>();
            _containerTitle.size = new Vector2(340.0f, 22.0f);
            _containerTitle.padding = new RectOffset(5, 0, 5, 0);
            _labelTitle = _containerTitle.AddUIComponent<UILabel>();
            string loadTooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_LOADFROMTHEME);
            _loadButton = UIUtils.CreateButton(_containerTitle, new Vector2(22.0f, 22.0f), tooltip: loadTooltip, backgroundSprite: "ThemesIcon", atlas: UISprites.Atlas);
            string resetTooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_RESET);
            _resetButton = UIUtils.CreateButton(_containerTitle, new Vector2(22.0f, 22.0f), tooltip: resetTooltip, backgroundSprite: "", foregroundSprite: "UndoIcon", atlas: UISprites.Atlas);

            _containerR = AddUIComponent<PanelBase>();
            _containerR.size = new Vector2(340.0f, 22.0f);
            _containerR.padding = new RectOffset(5, 0, 5, 0);
            _labelR = _containerR.AddUIComponent<UILabel>();
            _sliderR = UIUtils.CreateSlider(_containerR, 218.0f, -0.1f, 0.1f, 0.0001f);
            _textfieldR = _containerR.AddUIComponent<UITextField>();

            _containerG = AddUIComponent<PanelBase>();
            _containerG.size = new Vector2(340.0f, 22.0f);
            _containerG.padding = new RectOffset(5, 0, 5, 0);
            _labelG = _containerG.AddUIComponent<UILabel>();
            _sliderG = UIUtils.CreateSlider(_containerG, 218.0f, -0.1f, 0.1f, 0.0001f);
            _textfieldG = _containerG.AddUIComponent<UITextField>();

            _containerB = AddUIComponent<PanelBase>();
            _containerB.size = new Vector2(340.0f, 22.0f);
            _containerB.padding = new RectOffset(5, 0, 5, 0);
            _labelB = _containerB.AddUIComponent<UILabel>();
            _sliderB = UIUtils.CreateSlider(_containerB, 218.0f, -0.1f, 0.1f, 0.0001f);
            _textfieldB = _containerB.AddUIComponent<UITextField>();

            this.CreateSpace(0.0f, 0.01f);

            color = UIColorGrey;
        }

        private void SetupButtons()
        {
            _loadButton.eventClicked += OnLoadOffsetClicked;
            _loadButton.relativePosition = new Vector2(281.0f, 0.0f);
            _resetButton.eventClicked += OnResetClicked;
            _resetButton.relativePosition = new Vector2(308.0f, 0.0f);
        }

        private void OnResetClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            _ignoreEvents = true;
            _sliderR.value = _defaultValue.x;
            _sliderG.value = _defaultValue.y;
            _sliderB.value = _defaultValue.z;
            _textfieldR.text = _defaultValue.x.ToString("0.####");
            _textfieldG.text = _defaultValue.y.ToString("0.####");
            _textfieldB.text = _defaultValue.z.ToString("0.####");
            _ignoreEvents = false;
            Controller.OnOffsetChanged(OffsetID, _defaultValue);
        }

        private void OnLoadOffsetClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            Controller.OnLoadFromTheme(Category, OffsetID);
        }

        private void SetupLabels()
        {
            string title = Translation.Instance.GetTranslation(TranslationID.OffsetToTranslationID(OffsetID));
            string red = Translation.Instance.GetTranslation(TranslationID.LABEL_RED);
            string green = Translation.Instance.GetTranslation(TranslationID.LABEL_GREEN);
            string blue = Translation.Instance.GetTranslation(TranslationID.LABEL_BLUE);
            SetupLabel(_labelTitle, title, new Vector2(0.0f, 0.0f), new Vector2(340.0f, 22.0f));
            SetupLabel(_labelR, red, new Vector2(0.0f, 0.0f), new Vector2(22.0f, 22.0f));
            SetupLabel(_labelG, green, new Vector2(0.0f, 0.0f), new Vector2(22.0f, 22.0f));
            SetupLabel(_labelB, blue, new Vector2(0.0f, 0.0f), new Vector2(22.0f, 22.0f));

        }

        private static void SetupLabel(UILabel label, string text, Vector2 position, Vector2 size)
        {
            label.autoSize = false;
            label.autoHeight = true;
            label.size = size;
            label.font = UIUtils.Font;
            label.textScale = 1.0f;
            label.padding = new RectOffset(4, 0, 4, 0);
            label.text = text;
            label.relativePosition = position;
        }

        private void SetupSliders()
        {
            SetupSlider(_sliderR, new Vector2(27.0f, 6.0f));
            SetupSlider(_sliderG, new Vector2(27.0f, 6.0f));
            SetupSlider(_sliderB, new Vector2(27.0f, 6.0f));
        }

        private void SetupSlider(UISlider slider, Vector2 sliderPosition)
        {
            float value = ReferenceEquals(slider, _sliderR) ? _defaultValue.x : ReferenceEquals(slider, _sliderG) ? _defaultValue.y : _defaultValue.z;
            slider.value = value;
            slider.eventValueChanged += OnSliderValueChanged;
            slider.tooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_OFFSET);
            slider.relativePosition = sliderPosition;
        }

        private void SetupTextfields()
        {
            SetupTextfield(_textfieldR, new Vector2(261.0f, 0.0f));
            SetupTextfield(_textfieldG, new Vector2(261.0f, 0.0f));
            SetupTextfield(_textfieldB, new Vector2(261.0f, 0.0f));
        }

        private void SetupTextfield(UITextField textfield, Vector2 textfieldPosition)
        {
            textfield.atlas = UISprites.DefaultAtlas;
            textfield.size = new Vector2(70.0f, 21.0f);
            textfield.padding = new RectOffset(2, 2, 4, 4);
            textfield.builtinKeyNavigation = true;
            textfield.isInteractive = true;
            textfield.readOnly = false;
            textfield.selectOnFocus = true;
            textfield.horizontalAlignment = UIHorizontalAlignment.Center;
            textfield.selectionSprite = "EmptySprite";
            textfield.selectionBackgroundColor = new Color32(0, 172, 234, 255);
            textfield.normalBgSprite = "TextFieldPanelHovered";
            textfield.textColor = new Color32(0, 0, 0, 255);
            textfield.textScale = 0.85f;
            textfield.color = new Color32(255, 255, 255, 255);
            float value = ReferenceEquals(textfield, _textfieldR) ? _defaultValue.x : ReferenceEquals(textfield, _textfieldG) ? _defaultValue.y : _defaultValue.z;
            textfield.text = value.ToString("0.####");
            textfield.tooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_OFFSET);
            textfield.eventTextSubmitted += OnTextfieldTextSubmitted;
            textfield.eventKeyPress += OnTextfieldKeyPress;
            textfield.eventLostFocus += OnTextfieldLostFocus;
            textfield.relativePosition = textfieldPosition;
        }

        private void OnTextfieldTextSubmitted(UIComponent component, string value)
        {
            if (_ignoreEvents) return;
            var textfield = component as UITextField;
            if (!float.TryParse(textfield?.text.Replace(',', '.'), out float f)) return;
            float finalValue = Mathf.Clamp(f, -0.1f, 0.1f);
            if (textfield == null) return;
            textfield.text = finalValue.ToString("0.####");
            UISlider slider = ReferenceEquals(textfield, _textfieldR) ? _sliderR :
                ReferenceEquals(textfield, _textfieldG) ? _sliderG : _sliderB;
            slider.value = finalValue;
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
              (ch != '.' || (ch == '.' && textfield.text.Contains("."))) &&
              (ch != ',' || (ch == ',' && textfield.text.Contains(","))) &&
              (ch != '-' || (ch == '-' && textfield.text.Contains("-")))))
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
            float rValue = _sliderR.value;
            float gValue = _sliderG.value;
            float bValue = _sliderB.value;

            var slider = component as UISlider;
            float finalValue = value;
            string valueString = finalValue.ToString("0.####");

            if (ReferenceEquals(slider, _sliderR))
            {
                _textfieldR.text = valueString;
                Controller.OnOffsetChanged(OffsetID, new Vector3(finalValue, gValue, bValue));
            }
            if (ReferenceEquals(slider, _sliderG))
            {
                _textfieldG.text = valueString;
                Controller.OnOffsetChanged(OffsetID, new Vector3(rValue, finalValue, bValue));
            }
            if (ReferenceEquals(slider, _sliderB))
            {
                _textfieldB.text = valueString;
                Controller.OnOffsetChanged(OffsetID, new Vector3(rValue, gValue, finalValue));
            }
        }

        protected override void OnRefreshUI(object sender, UIDirtyEventArgs eventArgs)
        {
            base.OnRefreshUI(sender, eventArgs);
            try
            {
                _labelTitle.text = Translation.Instance.GetTranslation(TranslationID.OffsetToTranslationID(OffsetID));
                _defaultValue = Controller.GetOffsetValue(OffsetID);
                _ignoreEvents = true;
                _sliderR.value = _defaultValue.x;
                _sliderG.value = _defaultValue.y;
                _sliderB.value = _defaultValue.z;
                _textfieldR.text = _defaultValue.x.ToString("0.####");
                _textfieldG.text = _defaultValue.y.ToString("0.####");
                _textfieldB.text = _defaultValue.z.ToString("0.####");
                _ignoreEvents = false;
            }
            catch (Exception)
            {
                Debug.Log("Exception caught in TexturePanel.OnRefreshUI");
            }
        }
    }
}
