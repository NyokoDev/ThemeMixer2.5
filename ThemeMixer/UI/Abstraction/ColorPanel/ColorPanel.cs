using System;
using System.Collections.Generic;
using ColossalFramework.UI;
using ThemeMixer.Locale;
using ThemeMixer.Resources;
using ThemeMixer.Serialization;
using ThemeMixer.Themes.Enums;
using ThemeMixer.TranslationFramework;
using UnityEngine;

namespace ThemeMixer.UI.Abstraction.ColorPanel
{
    public abstract class ColorPanel : PanelBase
    {
        public event EventHandler<ColorPanelVisibilityChangedEventArgs> EventVisibilityChanged;

        private UIPanel _topPanel;
        private UIButton _colorButton;
        private UILabel _colorLabel;
        private UIButton _loadButton;
        private UIButton _resetButton;

        private UIColorPicker _colorPicker;

        private PanelBase _rgbPanel;
        private PanelBase _buttonsPanel;
        private ButtonPanel _closeButton;
        private ButtonPanel _saveButton;

        private UIPanel _colorPanel;
        private UITextField _redTextField;
        private UITextField _greenTextField;
        private UITextField _blueTextField;
        private PanelBase _savedSwatchesPanel;
        private Color32 _currentColor;
        private bool _updateNeeded;
        private List<SavedSwatch> _savedSwatches;
        private const int MaxSavedSwatches = 10;

        public ColorID ColorID;
        private Color32 _defaultValue;
        private bool _visible;
        private bool _ignoreEvents;

        public override void Awake()
        {
            base.Awake();
            _ignoreEvents = true;
            Setup("Color Panel", 350.0f, 0.0f, 5, true, LayoutDirection.Vertical, LayoutStart.TopLeft, "WhiteRect");
            autoLayout = false;
            CreateUIElements();
            _defaultValue = Controller.GetCurrentColor(ColorID);
            SetupTopPanel();
            if (Mod.InGame) SetupColorPicker();
            SetupRgbPanel();
            SetupButtonsPanel();
            RefreshColors();
            OnCloseClicked();
            color = UIColorGrey;
            _ignoreEvents = false;
            autoLayout = true;
        }

        private void CreateUIElements()
        {
            _topPanel = AddUIComponent<UIPanel>();
            string colorButtonTooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_OPENCOLORPICKER);
            _colorButton = UIUtils.CreateButton(_topPanel, new Vector2(22.0f, 22.0f), tooltip: colorButtonTooltip, backgroundSprite: "WhiteRect", atlas: UISprites.DefaultAtlas);
            _colorLabel = _topPanel.AddUIComponent<UILabel>();
            string loadTooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_LOADFROMTHEME);
            _loadButton = UIUtils.CreateButton(_topPanel, new Vector2(22.0f, 22.0f), tooltip: loadTooltip, backgroundSprite: "ThemesIcon", atlas: UISprites.Atlas);
            string resetTooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_RESET);
            _resetButton = UIUtils.CreateButton(_topPanel, new Vector2(22.0f, 22.0f), tooltip: resetTooltip, backgroundSprite: "", foregroundSprite: "UndoIcon", atlas: UISprites.Atlas);
            _savedSwatches = Data.GetSavedSwatches(ColorID);
            if (Mod.InGame)
            {
                var field = UITemplateManager.Get<UIPanel>("LineTemplate").Find<UIColorField>("LineColor");
                field = Instantiate(field);
                _colorPicker = Instantiate(field.colorPicker);
                AttachUIComponent(_colorPicker.gameObject);
                var hsb = _colorPicker.component.Find<UITextureSprite>("HSBField");
                var hue = _colorPicker.component.Find<UISlider>("HueSlider");
                hsb.relativePosition = new Vector3(55.0f, 7.0f);
                hue.relativePosition = new Vector3(267.0f, 7.0f);
            }
            _rgbPanel = AddUIComponent<PanelBase>();
            _buttonsPanel = AddUIComponent<PanelBase>();
            _closeButton = _buttonsPanel.AddUIComponent<ButtonPanel>();
            _saveButton = _buttonsPanel.AddUIComponent<ButtonPanel>();

            RefreshSavedSwatchesPanel();
            this.CreateSpace(1.0f, 0.1f);

            color = DataEnsurance.UIColor;
            autoFitChildrenHorizontally = true;
        }

        private void RefreshSavedSwatchesPanel()
        {
            if (_savedSwatchesPanel != null) Destroy(_savedSwatchesPanel.gameObject);
            _savedSwatchesPanel = AddUIComponent<PanelBase>();
            _savedSwatchesPanel.Setup("Saved Swatches Panel", 256.0f, 0.0f, 0, true, LayoutDirection.Vertical);
            _savedSwatchesPanel.padding = new RectOffset(0, 0, 0, 5);
            foreach (var savedSwatch in _savedSwatches) AddSavedSwatch(savedSwatch);
            _savedSwatchesPanel.isVisible = _savedSwatches.Count != 0;
        }

        private void SetupTopPanel()
        {
            _topPanel.size = new Vector2(345, 22.0f);
            _colorButton.relativePosition = new Vector3(0.0f, 0.0f);
            _colorButton.eventClicked += OnColorButtonClicked;
            _colorLabel.height = 22.0f;
            _colorLabel.font = UIUtils.Font;
            _colorLabel.textScale = 1.0f;
            _colorLabel.padding = new RectOffset(4, 0, 4, 0);
            _colorLabel.text = UIUtils.GetColorName(ColorID);
            _colorLabel.relativePosition = new Vector3(32.0f, 0.0f);
            _loadButton.relativePosition = new Vector3(291.0f, 0.0f);
            _loadButton.eventClicked += OnLoadClicked;
            _resetButton.relativePosition = new Vector3(318.0f, 0.0f);
            _resetButton.eventClicked += OnResetClicked;
        }

        private void OnLoadClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (_ignoreEvents) return;
            Controller.OnLoadFromTheme(Category, ColorID);
        }

        private void OnResetClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (_ignoreEvents) return;
            Controller.OnColorChanged(ColorID, _defaultValue);
            RefreshColors();
        }

        private void OnColorButtonClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (_ignoreEvents) return;
            component.isInteractive = false;
            _ignoreEvents = true;
            _visible = !_visible;
            if (_colorPicker != null) _colorPicker.component.isVisible = _visible;
            _rgbPanel.isVisible = _visible;
            _buttonsPanel.isVisible = _visible;
            _savedSwatchesPanel.isVisible = _visible;
            EventVisibilityChanged?.Invoke(this, new ColorPanelVisibilityChangedEventArgs(this, _visible));
            _ignoreEvents = false;
            component.isInteractive = true;
        }

        public override void Update()
        {
            base.Update();
            if (_updateNeeded && Input.GetMouseButtonUp(0))
            {
                ColorChanged();
                _updateNeeded = false;
            }
            if (_savedSwatches.Count == MaxSavedSwatches || _savedSwatches.Find(s => s.Color == Controller.GetCurrentColor(ColorID)) != null)
            {
                if (_saveButton.isEnabled) _saveButton.Disable();
                if (_savedSwatches.Count == MaxSavedSwatches)
                {
                    _saveButton.tooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_BUTTON_SAVE_MAXREACHED);
                }
                else if (_savedSwatches.Find(s => s.Color == Controller.GetCurrentColor(ColorID)) != null)
                {
                    _saveButton.tooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_BUTTON_SAVE_COLOREXISTS);
                }
            }
            else if (_savedSwatches.Count < MaxSavedSwatches)
            {
                if (!_saveButton.isEnabled) _saveButton.Enable();
                _saveButton.tooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_BUTTON_SAVE);
            }
        }

        private void SetupColorPicker()
        {
            _colorPicker.eventColorUpdated += OnColorUpdated;
            _colorPicker.color = Controller.GetCurrentColor(ColorID);
            _colorPicker.component.color = DataEnsurance.UIColor;
            var pickerPanel = _colorPicker.component as UIPanel;
            if (pickerPanel == null) return;
            pickerPanel.backgroundSprite = "";
            pickerPanel.size = new Vector2(340f, 212f);
        }

        private void SetupRgbPanel()
        {
            _rgbPanel.Setup("RGB Panel", 0.0f, 25.0f, 5, true);
            _rgbPanel.padding = new RectOffset(55, 0, 0, 5);
            _colorPanel = _rgbPanel.AddUIComponent<UIPanel>();
            _colorPanel.backgroundSprite = "WhiteRect";
            _colorPanel.size = new Vector2(28.0f, 25.0f);
            _colorPanel.color = Controller.GetCurrentColor(ColorID);
            _colorPanel.atlas = UISprites.DefaultAtlas;

            Color32 color32 = _colorPanel.color;
            CreateLabel(Translation.Instance.GetTranslation(TranslationID.LABEL_RED));
            _redTextField = CreateTextfield(color32.r.ToString());
            CreateLabel(Translation.Instance.GetTranslation(TranslationID.LABEL_GREEN));
            _greenTextField = CreateTextfield(color32.g.ToString());
            CreateLabel(Translation.Instance.GetTranslation(TranslationID.LABEL_BLUE));
            _blueTextField = CreateTextfield(color32.b.ToString());
        }

        private void CreateLabel(string text)
        {
            var label = _rgbPanel.AddUIComponent<UILabel>();
            label.font = UIUtils.Font;
            label.text = text;
            label.autoSize = false;
            label.autoHeight = false;
            label.size = new Vector2(15.0f, 25.0f);
            label.verticalAlignment = UIVerticalAlignment.Middle;
            label.textAlignment = UIHorizontalAlignment.Right;
            label.padding = new RectOffset(0, 0, 5, 0);
            label.atlas = UISprites.DefaultAtlas;
        }

        private UITextField CreateTextfield(string text)
        {
            var textField = _rgbPanel.AddUIComponent<UITextField>();
            textField.size = new Vector2(44.0f, 25.0f);
            textField.padding = new RectOffset(6, 6, 6, 6);
            textField.builtinKeyNavigation = true;
            textField.isInteractive = true;
            textField.readOnly = false;
            textField.horizontalAlignment = UIHorizontalAlignment.Center;
            textField.selectionSprite = "EmptySprite";
            textField.selectionBackgroundColor = new Color32(0, 172, 234, 255);
            textField.normalBgSprite = "TextFieldPanelHovered";
            textField.disabledBgSprite = "TextFieldPanelHovered";
            textField.textColor = new Color32(0, 0, 0, 255);
            textField.disabledTextColor = new Color32(80, 80, 80, 128);
            textField.color = new Color32(255, 255, 255, 255);
            textField.eventGotFocus += OnGotFocus;
            textField.eventKeyPress += OnKeyPress;
            textField.eventTextChanged += OnTextChanged;
            textField.eventTextSubmitted += OnTextSubmitted;
            textField.text = text;
            textField.atlas = UISprites.DefaultAtlas;
            return textField;
        }

        private void SetupButtonsPanel()
        {
            _buttonsPanel.Setup("Buttons Panel", 0.0f, 30.0f, 10, true);
            _buttonsPanel.padding = new RectOffset(55, 0, 0, 0);
            SetupCloseButton();
            SetupSaveButton();
        }

        private void SetupCloseButton()
        {
            _closeButton.Setup("Close Button", 0.0f, 30.0f, 0, true);
            _closeButton.SetAnchor(UIAnchorStyle.Left | UIAnchorStyle.CenterVertical);
            _closeButton.SetText(Translation.Instance.GetTranslation(TranslationID.BUTTON_CLOSE));
            _closeButton.EventButtonClicked += OnCloseClicked;
        }

        private void SetupSaveButton()
        {
            _saveButton.Setup("Save Button", 0.0f, 30.0f, 0, true);
            _saveButton.SetAnchor(UIAnchorStyle.Left | UIAnchorStyle.CenterVertical);
            _saveButton.SetText(Translation.Instance.GetTranslation(TranslationID.BUTTON_SAVE));
            _saveButton.EventButtonClicked += OnSaveClicked;
            if (_savedSwatches.Count == MaxSavedSwatches) _saveButton.Disable();
        }

        private void AddSavedSwatch(SavedSwatch savedSwatch)
        {
            var savedSwatchPanel = _savedSwatchesPanel.AddUIComponent<SavedSwatchPanel>();
            savedSwatchPanel.Setup("Saved Swatch", 256.0f, 24.0f, 0, true, LayoutDirection.Horizontal, LayoutStart.TopLeft, ColorID, savedSwatch);
            savedSwatchPanel.SavedSwatch = savedSwatch;
            savedSwatchPanel.autoLayoutPadding = new RectOffset(0, 5, 5, 5);
            savedSwatchPanel.EventSwatchClicked += OnSavedSwatchClicked;
            savedSwatchPanel.EventRemoveSwatch += OnSavedSwatchRemoved;
            savedSwatchPanel.EventSwatchRenamed += OnSavedSwatchRenamed;
        }

        private void OnSavedSwatchRenamed(SavedSwatch savedSwatch)
        {
            if (_ignoreEvents) return;
            //
        }

        private void OnSavedSwatchRemoved(SavedSwatchPanel savedSwatchPanel)
        {
            if (_ignoreEvents) return;
            if (savedSwatchPanel == null) return;
            _savedSwatches.Remove(savedSwatchPanel.SavedSwatch);
            Data.UpdateSavedSwatches(_savedSwatches, ColorID);
            Destroy(savedSwatchPanel.gameObject);
        }

        private void OnSavedSwatchClicked(Color32 swatchColor)
        {
            if (_ignoreEvents) return;
            UpdateColor(swatchColor);
        }

        private void OnSaveClicked()
        {
            if (_ignoreEvents) return;
            if (_savedSwatches.Find(s =>
                    Math.Abs(s.Color.r - _currentColor.r) < float.Epsilon && Math.Abs(s.Color.g - _currentColor.g) < float.Epsilon && Math.Abs(s.Color.b - _currentColor.b) < float.Epsilon) !=
                null) return;
            var newSwatch = new SavedSwatch() { Name = Translation.Instance.GetTranslation(TranslationID.LABEL_NEW_SWATCH), Color = _currentColor };
            AddSavedSwatch(newSwatch);
            _savedSwatches.Add(newSwatch);
            Data.UpdateSavedSwatches(_savedSwatches, ColorID);
        }

        private void OnCloseClicked()
        {
            if (_ignoreEvents) return;
            _visible = false;
            if (_colorPicker != null) _colorPicker.component.isVisible = _visible;
            _rgbPanel.isVisible = _visible;
            _buttonsPanel.isVisible = _visible;
            _savedSwatchesPanel.isVisible = _visible;
            EventVisibilityChanged?.Invoke(this, new ColorPanelVisibilityChangedEventArgs(this, _visible));
   
        }

        private void OnGotFocus(UIComponent component, UIFocusEventParameter eventParam)
        {
            if (_ignoreEvents) return;
            var textField = component as UITextField;
            textField?.SelectAll();
        }

        private void OnTextChanged(UIComponent component, string value)
        {
            if (_ignoreEvents) return;
            var textField = component as UITextField;
            if (textField == null) return;
            textField.eventTextChanged -= OnTextChanged;
            textField.text = GetClampedString(value);
            textField.eventTextChanged += OnTextChanged;
        }

        private static string GetClampedString(string value)
        {
            return value == "" ? value : GetClampedFloat(value).ToString("F0");
        }

        private static float GetClampedFloat(string value)
        {
            return !float.TryParse(value, out float number) ? 0.0f : Mathf.Clamp(number, 0, 255);
        }

        private void OnKeyPress(UIComponent component, UIKeyEventParameter parameter)
        {
            if (_ignoreEvents) return;
            char ch = parameter.character;
            if (!char.IsControl(ch) && !char.IsDigit(ch))
            {
                parameter.Use();
            }
        }

        private void OnTextSubmitted(UIComponent component, string value)
        {
            if (_ignoreEvents) return;
            var textField = component as UITextField;
            Color32 currentColor = _currentColor;
            if (textField == _redTextField)
            {
                currentColor = new Color32((byte)GetClampedFloat(value), currentColor.g, currentColor.b, 255);
            }
            else if (textField == _greenTextField)
            {
                currentColor = new Color32(currentColor.r, (byte)GetClampedFloat(value), currentColor.b, 255);
            }
            else if (textField == _blueTextField)
            {
                currentColor = new Color32(currentColor.r, currentColor.g, (byte)GetClampedFloat(value), 255);
            }

            UpdateColor(currentColor);
        }

        private void ColorChanged()
        {
            Controller.OnColorChanged(ColorID, _currentColor);
        }

        private void UpdateColor(Color value)
        {
            if (_colorPicker != null)
            {
                _colorPicker.eventColorUpdated -= OnColorUpdated;
                _colorPicker.color = value;
                _colorPicker.eventColorUpdated += OnColorUpdated;
            }
            OnColorUpdated(value);
        }

        private void OnColorUpdated(Color value)
        {
            if (_ignoreEvents) return;
            _currentColor = value;
            if (_colorPanel != null) _colorPanel.color = value;
            _colorButton.color = _colorButton.hoveredColor = _colorButton.pressedColor = _colorButton.focusedColor = value;
            UpdateTextfields();
            _updateNeeded = true;
        }

        private void UpdateTextfields()
        {
            if (_redTextField != null)
            {
                _redTextField.text = _currentColor.r.ToString();
            }
            if (_greenTextField != null)
            {
                _greenTextField.text = _currentColor.g.ToString();
            }
            if (_blueTextField != null)
            {
                _blueTextField.text = _currentColor.b.ToString();
            }
        }

        protected override void OnRefreshUI(object sender, UIDirtyEventArgs eventArgs)
        {
            base.OnRefreshUI(sender, eventArgs);
            RefreshColors();
        }

        private void RefreshColors()
        {
            _ignoreEvents = true;
            _currentColor = _colorPanel.color =
                _colorButton.color =
                _colorButton.hoveredColor =
                _colorButton.pressedColor =
                _colorButton.focusedColor =
                Controller.GetCurrentColor(ColorID);
            if (_colorPicker != null)
            {
                _colorPicker.color = Controller.GetCurrentColor(ColorID);
            }
            UpdateTextfields();
            _ignoreEvents = false;
        }
    }
}
