using ColossalFramework.UI;
using ThemeMixer.Resources;
using ThemeMixer.Serialization;
using ThemeMixer.Themes.Enums;
using UnityEngine;

namespace ThemeMixer.UI.Abstraction.ColorPanel
{
    public class SavedSwatchPanel : PanelBase
    {
        public delegate void SwatchClickedEventHandler(Color32 color);
        public event SwatchClickedEventHandler EventSwatchClicked;
        public delegate void RemoveSwatchEventHandler(SavedSwatchPanel savedSwatchPanel);
        public event RemoveSwatchEventHandler EventRemoveSwatch;
        public delegate void SwatchRenamedEventHandler(SavedSwatch savedSwatch);
        public event SwatchRenamedEventHandler EventSwatchRenamed;
        public SavedSwatch SavedSwatch;
        private SwatchButton _swatchButton;
        private UITextField _textField;
        private UIButton _deleteButton;
        private readonly Color32 _selectedTextColor = new Color32(88, 181, 205, 255);
        public ColorID ColorID;

        public override void Update()
        {
            base.Update();
            if (_swatchButton.Swatch == Controller.GetCurrentColor(ColorID))
            {
                _textField.textColor = _textField.hasFocus ? new Color32(255, 255, 255, 255) : _selectedTextColor;
            }
            else _textField.textColor = new Color32(255, 255, 255, 255);
        }

        public override void OnDestroy()
        {
            EventSwatchClicked = null;
            base.OnDestroy();
        }

        public void Setup(SavedSwatch savedSwatch)
        {
            this.SavedSwatch = savedSwatch;
            _swatchButton = AddUIComponent<SwatchButton>();
            _swatchButton.Build(savedSwatch.Color);
            _textField = AddUIComponent<UITextField>();
            _textField.normalBgSprite = "";
            _textField.hoveredBgSprite = "ButtonSmallHovered";
            _textField.focusedBgSprite = "ButtonSmallHovered";
            _textField.size = new Vector2(290.0f, 19.0f);
            _textField.font = UIUtils.Font;
            _textField.textScale = 0.8f;
            _textField.verticalAlignment = UIVerticalAlignment.Middle;
            _textField.horizontalAlignment = UIHorizontalAlignment.Left;
            _textField.padding = new RectOffset(5, 0, 4, 4);
            _textField.builtinKeyNavigation = true;
            _textField.isInteractive = true;
            _textField.readOnly = false;
            _textField.selectionSprite = "EmptySprite";
            _textField.selectOnFocus = true;
            _textField.text = savedSwatch.Name;
            _textField.atlas = UISprites.DefaultAtlas;
            _deleteButton = AddUIComponent<UIButton>();
            _deleteButton.normalBgSprite = "";
            _deleteButton.hoveredBgSprite = "DeleteLineButtonHover";
            _deleteButton.pressedBgSprite = "DeleteLineButtonPressed";
            _deleteButton.size = new Vector2(19.0f, 19.0f);
            _deleteButton.atlas = UISprites.DefaultAtlas;
            _swatchButton.EventSwatchClicked += OnSwatchClicked;
            _textField.eventTextChanged += OnTextChanged;
            _deleteButton.eventClicked += OnDeleteClicked;
            _deleteButton.eventMouseEnter += OnMouseEnter;
            _deleteButton.eventMouseLeave += OnMouseLeave;
            eventMouseEnter += OnMouseEnter;
            eventMouseLeave += OnMouseLeave;
        }

        private void OnMouseLeave(UIComponent component, UIMouseEventParameter eventParam)
        {
            _deleteButton.normalBgSprite = "";
        }

        private void OnMouseEnter(UIComponent component, UIMouseEventParameter eventParam)
        {
            _deleteButton.normalBgSprite = "DeleteLineButton";
        }

        private void OnDeleteClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            EventRemoveSwatch?.Invoke(this);
        }

        private void OnTextChanged(UIComponent component, string value)
        {
            SavedSwatch.Name = value;
            EventSwatchRenamed?.Invoke(SavedSwatch);
        }

        private void OnSwatchClicked(Color swatchColor, UIMouseEventParameter eventParam, UIComponent component)
        {
            EventSwatchClicked?.Invoke(swatchColor);
        }

        internal void Setup(string v1, float panelWidth, float panelHeight, int v2, bool v3, LayoutDirection horizontal, LayoutStart topLeft, ColorID colorID, SavedSwatch savedSwatch)
        {
            Setup(v1, panelWidth, panelHeight, v2, v3, horizontal, topLeft);
            Setup(savedSwatch);
            ColorID = colorID;
        }
    }
}
