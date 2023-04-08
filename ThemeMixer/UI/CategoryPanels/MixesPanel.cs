using ColossalFramework.UI;
using ThemeMixer.Locale;
using ThemeMixer.Resources;
using ThemeMixer.Themes;
using ThemeMixer.Themes.Enums;
using ThemeMixer.TranslationFramework;
using ThemeMixer.UI.Abstraction;
using UnityEngine;

namespace ThemeMixer.UI.CategoryPanels
{
    public class MixesPanel : PanelBase
    {
        private UIPanel _labelPanel;
        private UILabel _label;

        private PanelBase _selectMixPanel;
        private UIDropDown _selectMixDropDown;
        private CheckboxPanel _useAsDefaultCheckbox;
        private CheckboxPanel _disableCompileCheckboxPanel;
        private ButtonPanel _loadButtonPanel;
        private ButtonPanel _subscribeButtonPanel;

        private PanelBase _saveMixPanel;
        private PanelBase _textFieldPanel;
        private UITextField _saveMixTextField;
        private ButtonPanel _saveButtonPanel;

        private string _saveName;

        public override void Awake()
        {
            base.Awake();
            Category = ThemeCategory.Mixes;
            Setup("Mixes Panel", 360.0f, 0.0f, 5, true, LayoutDirection.Vertical, LayoutStart.TopLeft, "GenericPanel");
            CreateTitleLabel();
            CreatePanels();
            this.CreateSpace(0.0f, 0.1f);
            Data.EventThemeMixSaved += OnThemeMixSaved;
        }

        private void OnThemeMixSaved()
        {
            RefreshDropdown();
        }

        private void CreateTitleLabel()
        {
            _labelPanel = AddUIComponent<UIPanel>();
            _labelPanel.size = new Vector2(width, 22.0f);
            _label = _labelPanel.AddUIComponent<UILabel>();
            _label.font = UIUtils.BoldFont;
            _label.textScale = 1.0f;
            _label.textAlignment = UIHorizontalAlignment.Center;
            _label.verticalAlignment = UIVerticalAlignment.Middle;
            _label.padding = new RectOffset(0, 0, 4, 0);
            _label.text = Translation.Instance.GetTranslation(TranslationID.LABEL_MIXES);
            _label.anchor = UIAnchorStyle.CenterHorizontal | UIAnchorStyle.CenterVertical;
        }

        private void CreatePanels()
        {
            CreateSelectMixPanel();
            CreateSaveMixPanel();
        }

        private void CreateSelectMixPanel()
        {
            _selectMixPanel = AddUIComponent<PanelBase>();
            _selectMixPanel.Setup("Select Mix Panel", 350.0f, 0.0f, 5, true, LayoutDirection.Vertical, LayoutStart.TopLeft, "WhiteRect");
            _selectMixPanel.color = UIColorGrey;
            _selectMixPanel.CreateSpace(0.0f, 0.01f);
            CreateLabel(_selectMixPanel, Translation.Instance.GetTranslation(TranslationID.LABEL_SELECTMIX));
            _selectMixPanel.CreateSpace(0.0f, 0.01f);
            CreateDropDown();
            _selectMixPanel.CreateSpace(0.0f, 0.01f);
            CreateUseAsDefaultCheckbox();
            _selectMixPanel.CreateSpace(0.0f, 0.01f);
            CreateLoadButton();
            _selectMixPanel.CreateSpace(0.0f, 5.0f);
            CreateSubscribeButton();
            _selectMixPanel.CreateSpace(0.0f, 5.0f);
            RefreshDropdown();
        }

        private static void CreateLabel(UIComponent parent, string text)
        {
            var label = parent.AddUIComponent<UILabel>();
            label.autoSize = false;
            label.autoHeight = true;
            label.width = 340.0f;
            label.font = UIUtils.Font;
            label.textScale = 1.0f;
            label.textAlignment = UIHorizontalAlignment.Left;
            label.verticalAlignment = UIVerticalAlignment.Middle;
            label.padding = new RectOffset(0, 0, 4, 0);
            label.text = text;
        }

        private void CreateDropDown()
        {
            var panel = _selectMixPanel.AddUIComponent<UIPanel>();
            panel.size = new Vector2(340.0f, 30.0f);
            _selectMixDropDown = panel.AddUIComponent<UIDropDown>();
            _selectMixDropDown.relativePosition = Vector3.zero;
            _selectMixDropDown.atlas = UISprites.DefaultAtlas;
            _selectMixDropDown.size = new Vector2(340f, 30f);
            _selectMixDropDown.listBackground = "StylesDropboxListbox";
            _selectMixDropDown.itemHeight = 30;
            _selectMixDropDown.itemHover = "ListItemHover";
            _selectMixDropDown.itemHighlight = "ListItemHighlight";
            _selectMixDropDown.normalBgSprite = "CMStylesDropbox";
            _selectMixDropDown.hoveredBgSprite = "CMStylesDropboxHovered";
            _selectMixDropDown.listWidth = 340;
            _selectMixDropDown.listHeight = 500;
            _selectMixDropDown.listPosition = UIDropDown.PopupListPosition.Below;
            _selectMixDropDown.foregroundSpriteMode = UIForegroundSpriteMode.Stretch;
            _selectMixDropDown.popupColor = Color.white;
            _selectMixDropDown.popupTextColor = new Color32(170, 170, 170, 255);
            _selectMixDropDown.textScale = 0.8f;
            _selectMixDropDown.verticalAlignment = UIVerticalAlignment.Middle;
            _selectMixDropDown.horizontalAlignment = UIHorizontalAlignment.Left;
            _selectMixDropDown.textFieldPadding = new RectOffset(12, 0, 10, 0);
            _selectMixDropDown.itemPadding = new RectOffset(12, 0, 10, 0);
            _selectMixDropDown.triggerButton = _selectMixDropDown;
            _selectMixDropDown.eventDropdownOpen += OnDropDownOpen;
            _selectMixDropDown.eventDropdownClose += OnDropDownClose;
            _selectMixDropDown.eventSelectedIndexChanged += OnSelectedIndexChanged;
        }

        private void OnSelectedIndexChanged(UIComponent component, int value)
        {
            ThemeMix mix = Data.GetMixByIndex(_selectMixDropDown.selectedIndex);
            if (mix == null) return;
            _subscribeButtonPanel.isVisible = mix.ThemesMissing();
        }

        private void OnDropDownOpen(UIDropDown dropdown, UIListBox popup, ref bool overridden)
        {
            _selectMixDropDown.triggerButton.isInteractive = false;
        }

        private void OnDropDownClose(UIDropDown dropdown, UIListBox popup, ref bool overridden)
        {
            _selectMixDropDown.triggerButton.isInteractive = true;
        }

        private void RefreshDropdown()
        {
            _selectMixDropDown.items = Data.MixNames;
            if (_selectMixDropDown.items.Length > 0)
                _selectMixDropDown.selectedIndex = 0;
        }

        private void CreateUseAsDefaultCheckbox()
        {
            _useAsDefaultCheckbox = _selectMixPanel.AddUIComponent<CheckboxPanel>();
            var state = false;
            if (_selectMixDropDown.items.Length > 0 &&
                _selectMixDropDown.selectedIndex >= 0 &&
                _selectMixDropDown.selectedIndex < _selectMixDropDown.items.Length)
            {
                state = Data.IsDefaultMix(_selectMixDropDown.items[_selectMixDropDown.selectedIndex]);
            }
            string label = Translation.Instance.GetTranslation(TranslationID.LABEL_USEASDEFAULT);
            string checkboxTooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_USEASDEFAULT);
            _useAsDefaultCheckbox.Initialize(state, label, checkboxTooltip);
            _useAsDefaultCheckbox.MakeSmallVersion();
            _useAsDefaultCheckbox.EventCheckboxStateChanged += OnUseAsDefaultCheckChanged;
        }

        private void OnUseAsDefaultCheckChanged(UIComponent component, bool value)
        {
            if (_selectMixDropDown.items.Length == 0 ||
                _selectMixDropDown.selectedIndex < 0 ||
                _selectMixDropDown.selectedIndex >= _selectMixDropDown.items.Length) return;
            if (value) Data.SetDefaultMix(_selectMixDropDown.items[_selectMixDropDown.selectedIndex]);
            if (!value && Data.IsDefaultMix(_selectMixDropDown.items[_selectMixDropDown.selectedIndex]))
                Data.SetDefaultMix(string.Empty);
        }

        private void CreateLoadButton()
        {
            _loadButtonPanel = _selectMixPanel.AddUIComponent<ButtonPanel>();
            _loadButtonPanel.Setup("Load Button", 340.0f, 30.0f);
            _loadButtonPanel.SetAnchor(UIAnchorStyle.Left | UIAnchorStyle.CenterVertical);
            _loadButtonPanel.SetText(Translation.Instance.GetTranslation(TranslationID.BUTTON_LOAD));
            _loadButtonPanel.AlignRight();
            _loadButtonPanel.EventButtonClicked += OnLoadClicked;
        }
        private void CreateSubscribeButton()
        {
            _subscribeButtonPanel = _selectMixPanel.AddUIComponent<ButtonPanel>();
            _subscribeButtonPanel.Setup("Download Button", 340.0f, 30.0f);
            _subscribeButtonPanel.SetAnchor(UIAnchorStyle.Left | UIAnchorStyle.CenterVertical);
            _subscribeButtonPanel.SetText(Translation.Instance.GetTranslation(TranslationID.BUTTON_SUBSCRIBE), Translation.Instance.GetTranslation(TranslationID.TOOLTIP_BUTTON_SUBSCRIBE));
            _subscribeButtonPanel.AlignRight();
            _subscribeButtonPanel.EventButtonClicked += OnSubscribeClicked;
            _subscribeButtonPanel.isVisible = false;
        }

        private void OnSubscribeClicked()
        {
            ThemeMix mix = Data.GetMixByIndex(_selectMixDropDown.selectedIndex);
            if (mix == null) return;
            mix.SubscribeMissingThemes();
            Controller.CloseUI();
            UIView.Find("DefaultTooltip")?.Hide();
            UIUtils.ShowExceptionPanel(TranslationID.LABEL_SUBSCRIBE_WARNING_TITLE, TranslationID.LABEL_SUBSCRIBE_WARNING_MESSAGE, false);
        }

        private void OnLoadClicked()
        {
            ThemeMix mix = Data.GetMixByIndex(_selectMixDropDown.selectedIndex);
            if (mix == null) return;
            Controller.LoadMix(mix);
        }

        private void CreateSaveMixPanel()
        {
            _saveMixPanel = AddUIComponent<PanelBase>();
            _saveMixPanel.Setup("Save Mix Panel", 350.0f, 0.0f, 5, true, LayoutDirection.Vertical, LayoutStart.TopLeft, "WhiteRect");
            _saveMixPanel.color = UIColorGrey;

            _saveMixPanel.CreateSpace(0.0f, 0.01f);
            CreateLabel(_saveMixPanel, Translation.Instance.GetTranslation(TranslationID.LABEL_SAVEMIX));
            _saveMixPanel.CreateSpace(0.0f, 0.01f);
            CreateTextField();
            _saveMixPanel.CreateSpace(0.0f, 0.01f);
            if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                CreateDisableCompileCheckbox();
                _saveMixPanel.CreateSpace(0.0f, 0.01f);
            }
            CreateSaveButton();
            _saveMixPanel.CreateSpace(0.0f, 5.0f);
        }

        private void CreateTextField()
        {
            _textFieldPanel = _saveMixPanel.AddUIComponent<PanelBase>();
            _textFieldPanel.Setup("Name Text Field", 340.0f, 30.0f);
            var nameLabel = _textFieldPanel.AddUIComponent<UILabel>();
            nameLabel.pivot = UIPivotPoint.MiddleRight;
            nameLabel.font = UIUtils.Font;
            nameLabel.padding = new RectOffset(0, 0, 8, 4);
            nameLabel.text = Translation.Instance.GetTranslation(TranslationID.LABEL_NAME);
            nameLabel.relativePosition = new Vector3(110.0f - nameLabel.width, 0.0f);
            _saveMixTextField = _textFieldPanel.AddUIComponent<UITextField>();
            _saveMixTextField.atlas = UISprites.DefaultAtlas;
            _saveMixTextField.size = new Vector2(220.0f, 30.0f);
            _saveMixTextField.padding = new RectOffset(4, 4, 6, 6);
            _saveMixTextField.builtinKeyNavigation = true;
            _saveMixTextField.isInteractive = true;
            _saveMixTextField.readOnly = false;
            _saveMixTextField.selectOnFocus = true;
            _saveMixTextField.horizontalAlignment = UIHorizontalAlignment.Center;
            _saveMixTextField.selectionSprite = "EmptySprite";
            _saveMixTextField.selectionBackgroundColor = new Color32(0, 172, 234, 255);
            _saveMixTextField.normalBgSprite = "TextFieldPanelHovered";
            _saveMixTextField.textColor = Color.black;
            _saveMixTextField.textScale = 1.0f;
            _saveMixTextField.color = new Color32(255, 255, 255, 255);
            _saveMixTextField.relativePosition = new Vector3(120.0f, 0.0f);
            _saveMixTextField.eventTextSubmitted += OnTextfieldTextSubmitted;
            _saveMixTextField.eventKeyPress += OnTextfieldKeyPress;
            _saveMixTextField.eventLostFocus += OnTextfieldLostFocus;
            _saveMixTextField.eventTextChanged += OnTextFieldTextChanged;
        }

        private void OnTextFieldTextChanged(UIComponent component, string value)
        {
            var textfield = component as UITextField;
            if (textfield == null) return;
            if (textfield.text.Length > 0 && !int.TryParse(textfield.text[0].ToString(), out int _))
            {
                _saveButtonPanel.EnableButton(Translation.Instance.GetTranslation(TranslationID.BUTTON_SAVE));
                textfield.textColor = Color.black;
            }
            else
            {
                textfield.textColor = Color.red;
                _saveButtonPanel.DisableButton();
            }
        }

        private void OnTextfieldTextSubmitted(UIComponent component, string value)
        {
            _saveName = value;
        }

        private void OnTextfieldLostFocus(UIComponent component, UIFocusEventParameter eventParam)
        {
            var textfield = component as UITextField;
            if (textfield != null) OnTextfieldTextSubmitted(component, textfield.text);
        }

        private static void OnTextfieldKeyPress(UIComponent component, UIKeyEventParameter eventParam)
        {
            var textfield = component as UITextField;
            char ch = eventParam.character;
            if (!char.IsControl(ch) && !char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch))
            {
                eventParam.Use();
            }
            if (eventParam.keycode != KeyCode.Escape) return;
            if (textfield != null) textfield.Unfocus();
            eventParam.Use();
        }

        private void CreateDisableCompileCheckbox()
        {
            _disableCompileCheckboxPanel = _saveMixPanel.AddUIComponent<CheckboxPanel>();
            string label = Translation.Instance.GetTranslation(TranslationID.LABEL_DISABLECOMPILE);
            string checkboxTooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_DISABLECOMPILE);
            _disableCompileCheckboxPanel.Initialize(Data.DisableCompile, label, checkboxTooltip);
            _disableCompileCheckboxPanel.MakeSmallVersion();
            _disableCompileCheckboxPanel.EventCheckboxStateChanged += OnDisableCompileCheckChanged;
        }

        private void OnDisableCompileCheckChanged(UIComponent component, bool value)
        {
            Data.DisableCompile = value;
        }

        private void CreateSaveButton()
        {
            _saveButtonPanel = _saveMixPanel.AddUIComponent<ButtonPanel>();
            _saveButtonPanel.Setup("Save Button", 340.0f, 30.0f);
            _saveButtonPanel.SetAnchor(UIAnchorStyle.Left | UIAnchorStyle.CenterVertical);
            _saveButtonPanel.SetText(Translation.Instance.GetTranslation(TranslationID.BUTTON_SAVE));
            _saveButtonPanel.AlignRight();
            _saveButtonPanel.DisableButton();
            _saveButtonPanel.EventButtonClicked += OnSaveClicked;
        }

        private void OnSaveClicked()
        {
            Controller.SaveMix(_saveName);
            _saveButtonPanel.SetText(Translation.Instance.GetTranslation(TranslationID.BUTTON_SAVED));
            _saveButtonPanel.DisableButton();
        }
    }
}
