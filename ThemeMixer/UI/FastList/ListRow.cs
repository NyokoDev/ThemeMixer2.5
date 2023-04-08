using System.Text.RegularExpressions;
using ColossalFramework.UI;
using ThemeMixer.Locale;
using ThemeMixer.Resources;
using ThemeMixer.Themes.Enums;
using ThemeMixer.TranslationFramework;
using ThemeMixer.UI.Abstraction;
using UnityEngine;

namespace ThemeMixer.UI.FastList
{
    public class ListRow : PanelBase, IUIFastListRow
    {
        public delegate void FavouriteChangedEventHandler(string itemID, bool favourite);
        public event FavouriteChangedEventHandler EventFavouriteChanged;

        public delegate void BlacklistedChangedEventHandler(string itemID, bool blacklisted);
        public event BlacklistedChangedEventHandler EventBlacklistedChanged;

        public delegate void ValuesButtonClickedEventHandler(string itemID);
        public event ValuesButtonClickedEventHandler EventValuesClicked;

        private UIPanel _thumbnailPanel;
        private UISprite _thumbnailSprite;

        private PanelBase _labelsPanel;

        private UILabel _nameLabel;
        private UILabel _authorLabel;

        private UIPanel _checkboxPanel;
        private UICheckBox _favouriteCheckbox;
        private UISprite _checkedSprite;
        private UISprite _uncheckedSprite;

        private UIButton _valuesButton;
        private UILabel _valuesLabel;
        private UISprite _valuesSprite;

        private ListItem _itemData;
        private Color32 EvenColor { get; } = new Color32(67, 76, 80, 255);
        private Color32 OddColor { get; } = new Color32(57, 67, 70, 255);
        private Color32 SelectedColor { get; } = new Color32(70, 120, 130, 255);

        private bool _isRowOdd;

        public override void Awake()
        {
            base.Awake();
            Setup("List Row", 456.0f, 76.0f, UIUtils.DefaultSpacing, true, LayoutDirection.Horizontal, LayoutStart.TopLeft, "WhiteRect");
            color = _isRowOdd ? OddColor : EvenColor;
            autoLayout = false;
            CreateThumbnail();
            CreateLabels();
            CreateValuesPanel();
            CreateCheckbox();
            this.CreateSpace(0.0f, 30.0f);
            autoLayout = true;
            eventMouseEnter += OnMouseEnterEvent;
            eventMouseLeave += OnMouseLeaveEvent;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (_favouriteCheckbox != null)
                _favouriteCheckbox.eventClicked -= OnFavouriteCheckboxMouseUp;
            eventMouseEnter -= OnMouseEnterEvent;
            eventMouseLeave -= OnMouseLeaveEvent;
            EventFavouriteChanged = null;
            EventBlacklistedChanged = null;
        }

        public void Select(bool isRowOdd)
        {
            if (IsSelected()) color = SelectedColor;
        }

        public void Deselect(bool isRowOdd)
        {
            color = _isRowOdd ? OddColor : EvenColor;
        }

        public void Display(object data, bool isRowOdd)
        {
            if (!(data is ListItem item)) return;
            _itemData = item;
            _isRowOdd = isRowOdd;
            DisplayItem(isRowOdd);
        }

        private void CreateThumbnail()
        {
            _thumbnailPanel = AddUIComponent<UIPanel>();
            _thumbnailPanel.size = new Vector2(115.0f, 66.0f);
            _thumbnailPanel.atlas = UISprites.DefaultAtlas;
            _thumbnailPanel.backgroundSprite = "WhiteRect";
            _thumbnailPanel.color = Resources.ColorData.UIColor;

            _thumbnailSprite = _thumbnailPanel.AddUIComponent<UISprite>();
            _thumbnailSprite.atlas = ThemeSprites.Atlas;
            _thumbnailSprite.size = new Vector2(113.0f, 64.0f);
            _thumbnailSprite.relativePosition = new Vector2(1.0f, 1.0f);
        }

        private void CreateLabels()
        {
            _labelsPanel = AddUIComponent<PanelBase>();
            _labelsPanel.Setup("Labels Panel", 255.0f, 64.0f, 0, true, LayoutDirection.Vertical);
            _labelsPanel.autoFitChildrenHorizontally = true;

            _nameLabel = _labelsPanel.AddUIComponent<UILabel>();
            _nameLabel.autoSize = false;
            _nameLabel.size = new Vector2(width, 33.0f);
            _nameLabel.padding = new RectOffset(5, 0, 8, 0);
            _nameLabel.textScale = 1.0f;
            _nameLabel.font = UIUtils.BoldFont;

            _authorLabel = _labelsPanel.AddUIComponent<UILabel>();
            _authorLabel.autoSize = false;
            _authorLabel.size = new Vector2(width, 33.0f);
            _authorLabel.padding = new RectOffset(5, 0, 2, 0);
            _authorLabel.textScale = 0.8f;
            _authorLabel.font = UIUtils.BoldFont;
        }

        private void CreateValuesPanel()
        {
            _valuesButton = AddUIComponent<UIButton>();
            _valuesButton.size = new Vector2(66.0f, 66.0f);
            _valuesButton.eventClicked += OnValuesButtonClicked;

            _valuesLabel = _valuesButton.AddUIComponent<UILabel>();
            _valuesLabel.text = " ";
            _valuesLabel.autoSize = false;
            _valuesLabel.size = _valuesButton.size;
            _valuesLabel.relativePosition = new Vector2(0.0f, 0.0f);
            _valuesLabel.textAlignment = UIHorizontalAlignment.Center;
            _valuesLabel.verticalAlignment = UIVerticalAlignment.Middle;

            _valuesSprite = _valuesButton.AddUIComponent<UISprite>();
            _valuesSprite.atlas = ThemeSprites.Atlas;
            _valuesSprite.size = new Vector2(64.0f, 64.0f);
            _valuesSprite.relativePosition = new Vector2(1.0f, 1.0f);

            _valuesButton.isVisible = false;
        }

        private void OnValuesButtonClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            EventValuesClicked?.Invoke(_itemData.ID);
        }

        private void CreateCheckbox()
        {
            _checkboxPanel = AddUIComponent<UIPanel>();
            _checkboxPanel.size = new Vector2(66.0f, 66.0f);

            _favouriteCheckbox = _checkboxPanel.AddUIComponent<UICheckBox>();
            _favouriteCheckbox.size = new Vector2(22f, 22f);
            _favouriteCheckbox.relativePosition = new Vector3(22.0f, 22.0f);

            _uncheckedSprite = _favouriteCheckbox.AddUIComponent<UISprite>();
            _uncheckedSprite.atlas = UISprites.Atlas;
            _uncheckedSprite.spriteName = UISprites.StarOutline;
            _uncheckedSprite.size = _favouriteCheckbox.size;
            _uncheckedSprite.relativePosition = Vector3.zero;

            _checkedSprite = _uncheckedSprite.AddUIComponent<UISprite>();
            _checkedSprite.atlas = UISprites.Atlas;
            _checkedSprite.spriteName = UISprites.Star;
            _checkedSprite.size = _favouriteCheckbox.size;
            _checkedSprite.relativePosition = Vector2.zero;

            _favouriteCheckbox.checkedBoxObject = _checkedSprite;
            _favouriteCheckbox.eventMouseUp += OnFavouriteCheckboxMouseUp;
            _favouriteCheckbox.isChecked = false;
        }

        private void OnFavouriteCheckboxMouseUp(UIComponent component, UIMouseEventParameter eventParam)
        {
            switch (eventParam.buttons)
            {
                case UIMouseButton.Right:
                    {
                        bool blackListed = !_itemData.IsBlacklisted;
                        _itemData.IsBlacklisted = blackListed;
                        if (blackListed)
                        {
                            _favouriteCheckbox.isChecked = true;
                            _checkedSprite.spriteName = UISprites.Blacklisted;
                            _uncheckedSprite.spriteName = "";
                            if (_itemData.IsFavourite)
                            {
                                _itemData.IsFavourite = false;
                                EventFavouriteChanged?.Invoke(_itemData.ID, false);
                            }
                        }
                        else
                        {
                            if (!_itemData.IsFavourite)
                            {
                                _favouriteCheckbox.isChecked = false;
                            }
                            _uncheckedSprite.spriteName = UISprites.StarOutline;
                        }
                        EventBlacklistedChanged?.Invoke(_itemData.ID, blackListed);
                        break;
                    }
                case UIMouseButton.Left:
                    {
                        bool favourite = !_itemData.IsFavourite;
                        _itemData.IsFavourite = favourite;
                        if (favourite)
                        {
                            _favouriteCheckbox.isChecked = true;
                            _checkedSprite.spriteName = UISprites.Star;
                            _uncheckedSprite.spriteName = UISprites.StarOutline;
                            if (_itemData.IsBlacklisted)
                            {
                                _itemData.IsBlacklisted = false;
                                EventBlacklistedChanged?.Invoke(_itemData.ID, false);
                            }
                        }
                        else
                        {
                            if (!_itemData.IsBlacklisted)
                            {
                                _favouriteCheckbox.isChecked = false;
                            }
                        }
                        EventFavouriteChanged?.Invoke(_itemData.ID, favourite);
                        break;
                    }
            }

            UpdateCheckboxTooltip();
        }

        private void DisplayItem(bool isRowOdd)
        {
            color = IsSelected() ? SelectedColor : isRowOdd ? OddColor : EvenColor;
            string spriteName = string.Concat(_itemData.ID, _itemData.DisplayName, "_", "Snapshot");
            spriteName = Regex.Replace(spriteName, @"(\s+|@|&|'|\(|\)|<|>|#|"")", "");
            _thumbnailSprite.spriteName = spriteName;
            _nameLabel.text = _itemData.DisplayName;
            _authorLabel.text = string.Concat(Translation.Instance.GetTranslation(TranslationID.LABEL_BY), " ", _itemData.Author);
            _favouriteCheckbox.isChecked = _itemData.IsFavourite || _itemData.IsBlacklisted;
            _checkedSprite.spriteName = _itemData.IsBlacklisted ? UISprites.Blacklisted : UISprites.Star;
            _uncheckedSprite.spriteName = _itemData.IsBlacklisted ? "" : UISprites.StarOutline;
            float labelsPanelWidth = _itemData.Category == ThemeCategory.Themes || _itemData.Category == ThemeCategory.None ? 255.0f : 189.0f;
            _authorLabel.width = _nameLabel.width = _labelsPanel.width = labelsPanelWidth;
            _nameLabel.tooltip = string.Empty;
            _nameLabel.FitString();
            _authorLabel.tooltip = string.Empty;
            _authorLabel.FitString();
            _valuesButton.isVisible = _itemData.Category != ThemeCategory.Themes && _itemData.Category != ThemeCategory.None;
            switch (Controller.Part)
            {
                case ThemePart.Texture:
                    _valuesButton.normalBgSprite = "WhiteRect";
                    _valuesButton.color = Color.white;
                    _valuesSprite.spriteName = UIUtils.GetTextureSpriteName(Controller.TextureID, _itemData.ID);
                    break;
                case ThemePart.Color:
                    _valuesButton.normalBgSprite = "WhiteRect";
                    _valuesButton.hoveredBgSprite = "WhiteRect";
                    _valuesButton.pressedBgSprite = "WhiteRect";
                    _valuesButton.focusedBgSprite = "WhiteRect";
                    Color32 buttonColor = Controller.GetColor(Controller.ColorID, _itemData.ID);
                    _valuesButton.color = _valuesButton.hoveredColor = _valuesButton.pressedColor = _valuesButton.focusedColor = buttonColor;
                    break;
                case ThemePart.Offset:
                    break;
                case ThemePart.Value:
                    break;
            }

            UpdateCheckboxTooltip();
        }

        private bool IsSelected()
        {
            return Controller.IsSelected(_itemData.ID, _itemData.Category);
        }

        private void UpdateCheckboxTooltip()
        {
            _favouriteCheckbox.tooltip = _itemData.IsFavourite
                            ? Translation.Instance.GetTranslation(TranslationID.TOOLTIP_REMOVEFAVOURITE)
                            : _itemData.IsBlacklisted
                            ? Translation.Instance.GetTranslation(TranslationID.TOOLTIP_REMOVEBLACKLIST)
                            : Translation.Instance.GetTranslation(TranslationID.TOOLTIP_ADDFAVOURITE_ADDBLACKLIST);
        }

        private void OnMouseLeaveEvent(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (_itemData != null)
            {
                color = IsSelected() ? SelectedColor : _isRowOdd ? OddColor : EvenColor;
            }
        }

        private void OnMouseEnterEvent(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (_itemData != null && !IsSelected())
            {
                color = new Color32((byte)(OddColor.r + 25), (byte)(OddColor.g + 25), (byte)(OddColor.b + 25), 255);
            }
        }
    }
}
