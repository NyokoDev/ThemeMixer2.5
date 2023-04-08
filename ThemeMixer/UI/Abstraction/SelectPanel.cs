using System;
using System.Collections.Generic;
using ColossalFramework.Packaging;
using ColossalFramework.PlatformServices;
using ColossalFramework.UI;
using ThemeMixer.Locale;
using ThemeMixer.Themes;
using ThemeMixer.Themes.Enums;
using ThemeMixer.TranslationFramework;
using ThemeMixer.UI.FastList;
using UnityEngine;

namespace ThemeMixer.UI.Abstraction
{
    public abstract class SelectPanel : PanelBase
    {
        public event EventHandler<ThemesPanelClosingEventArgs> EventPanelClosing;
        public event EventHandler<ThemeSelectedEventArgs> EventThemeSelected;
        public ThemePart Part { get; set; } = ThemePart.None;

        private UILabel _label;
        private UIFastList _fastList;
        protected PanelBase ButtonPanel;
        private UIButton _button;
        private static readonly Dictionary<string, MapThemeMetaData> Favourites = new Dictionary<string, MapThemeMetaData>();
        private static readonly Dictionary<string, MapThemeMetaData> Blacklisted = new Dictionary<string, MapThemeMetaData>();
        private static readonly Dictionary<string, MapThemeMetaData> Normal = new Dictionary<string, MapThemeMetaData>();

        public override void Awake()
        {
            base.Awake();
            Setup("Select Theme Panel", 478.0f, 0.0f, UIUtils.DefaultSpacing, true, LayoutDirection.Vertical, LayoutStart.TopLeft, "GenericPanel");
            Part = Controller.Part;
            float panelWidth = ThemeManager.Instance.Themes.Count > 7 ? 468.0f : 456.0f;
            CreateLabel();
            CreateFastList(new Vector2(panelWidth, 720.0f), 76.0f);
            CreateButton();
            this.CreateSpace(panelWidth, 0.0f);
            SetupRowsData();
            BindEvents();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            UnbindEvents();
        }

        protected override void OnKeyDown(UIKeyEventParameter p)
        {
            if (p.keycode == KeyCode.Escape || p.keycode == KeyCode.Return)
            {
                p.Use();
                EventPanelClosing?.Invoke(this, new ThemesPanelClosingEventArgs(Category, Part));
            }
            base.OnKeyDown(p);
        }

        private void CreateButton()
        {
            ButtonPanel = AddUIComponent<PanelBase>();
            _button = UIUtils.CreateButton(ButtonPanel, new Vector2(100.0f, 30.0f), Translation.Instance.GetTranslation(TranslationID.BUTTON_OK));
            _button.eventClicked += OnOkButtonClicked;
            ButtonPanel.size = new Vector2(width - 10.0f, _button.height);
            _button.relativePosition = new Vector2(ButtonPanel.width - _button.width, 0.0f);
        }

        private void OnOkButtonClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            EventPanelClosing?.Invoke(this, new ThemesPanelClosingEventArgs(Category, Part));
        }

        private void CreateLabel()
        {
            _label = AddUIComponent<UILabel>();
            _label.text = "";
            _label.autoSize = false;
            _label.size = new Vector2(width, 32.0f);
            _label.font = UIUtils.BoldFont;
            _label.textScale = 1.0f;
            _label.textAlignment = UIHorizontalAlignment.Center;
            _label.verticalAlignment = UIVerticalAlignment.Middle;
            _label.padding = new RectOffset(0, 0, 4, 0);
            switch (Part)
            {
                case ThemePart.Category:
                    _label.text = UIUtils.GetCategoryAndPartLabel(Category, Part);
                    break;
                case ThemePart.Texture:
                    _label.text = UIUtils.GetPartAndIDLabel(Controller.TextureID);
                    break;
                case ThemePart.Color:
                    _label.text = UIUtils.GetPartAndIDLabel(Controller.ColorID);
                    break;
                case ThemePart.Offset:
                    _label.text = UIUtils.GetPartAndIDLabel(Controller.OffsetID);
                    break;
                case ThemePart.Value:
                    _label.text = UIUtils.GetPartAndIDLabel(Controller.ValueID);
                    break;
            }
        }

        protected bool IsFavourite(string itemID)
        {
            return Data.IsFavourite(itemID, Category);
        }

        protected bool IsBlacklisted(string itemID)
        {
            return Data.IsBlacklisted(itemID, Category);
        }

        private void OnThemeSelected(UIComponent component, int itemIndex)
        {
            if (_fastList.RowsData[itemIndex] is ListItem item)
            {
                EventThemeSelected?.Invoke(this, new ThemeSelectedEventArgs(item.ID, Category, Part));
            }
        }

        private void OnFavouriteChanged(string itemID, bool favourite)
        {
            if (favourite)
            {
                Data.AddToFavourites(itemID, Category);
            }
            else Data.RemoveFromFavourites(itemID, Category);
        }

        private void OnBlacklistedChanged(string itemID, bool blacklisted)
        {
            if (blacklisted)
            {
                Data.AddToBlacklist(itemID, Category);
            }
            else Data.RemoveFromBlacklist(itemID, Category);
        }

        private void CreateFastList(Vector2 listSize, float rowHeight)
        {
            _fastList = UIFastList.Create<ListRow>(this);
            _fastList.BackgroundSprite = "UnlockingPanel";
            _fastList.size = listSize;
            _fastList.RowHeight = rowHeight;
            _fastList.CanSelect = true;
            _fastList.AutoHideScrollbar = true;
        }

        private void BindEvents()
        {
            _fastList.EventItemClick += OnThemeSelected;
            for (var rowIndex = 0; rowIndex < _fastList.Rows.m_size; rowIndex++)
            {
                if (!(_fastList.Rows[rowIndex] is ListRow row)) continue;
                row.EventFavouriteChanged += OnFavouriteChanged;
                row.EventBlacklistedChanged += OnBlacklistedChanged;
            }

            EventPanelClosing += Controller.OnThemeSelectorPanelClosing;
            EventThemeSelected += Controller.OnThemeSelected;
        }

        private void UnbindEvents()
        {
            _fastList.EventItemClick -= OnThemeSelected;
            for (var rowIndex = 0; rowIndex < _fastList.Rows.m_size; rowIndex++)
            {
                if (!(_fastList.Rows[rowIndex] is ListRow row)) continue;
                row.EventFavouriteChanged -= OnFavouriteChanged;
                row.EventBlacklistedChanged -= OnBlacklistedChanged;
            }

            EventPanelClosing -= Controller.OnThemeSelectorPanelClosing;
            EventThemeSelected -= Controller.OnThemeSelected;
        }
        protected void SetupRowsData()
        {
            if (_fastList.RowsData == null)
            {
                _fastList.RowsData = new FastList<object>();
            }
            _fastList.RowsData.Clear();
            Favourites.Clear();
            Blacklisted.Clear();
            Normal.Clear();
            var index = 0;
            var count = 0;
            var selectedIndex = 0;
            var favList = Data.GetFavourites(Category);
            var blacklist = Data.GetBlacklisted(Category);
            foreach (var kvp in ThemeManager.Instance.Themes)
            {
                if (favList.Contains(kvp.Key))
                {
                    Favourites[kvp.Key] = kvp.Value;
                }
                else if (blacklist.Contains(kvp.Key))
                {
                    Blacklisted[kvp.Key] = kvp.Value;
                }
                else Normal[kvp.Key] = kvp.Value;
            }
            foreach (var fav in Favourites)
            {
                CreateAndAddItemToFastList(fav.Value, ref count, ref index, ref selectedIndex);
            }
            foreach (var norm in Normal)
            {
                CreateAndAddItemToFastList(norm.Value, ref count, ref index, ref selectedIndex);
            }
            foreach (var black in Blacklisted)
            {
                CreateAndAddItemToFastList(black.Value, ref count, ref index, ref selectedIndex);
            }
            _fastList.RowsData.SetCapacity(count);
            count = Mathf.Clamp(count, 0, 7);
            _fastList.height = count * 76.0f;
            _fastList.DisplayAt(selectedIndex);
            _fastList.SelectedIndex = selectedIndex;
        }

        private void CreateAndAddItemToFastList(MapThemeMetaData metaData, ref int count, ref int index, ref int selectedIndex)
        {
            ListItem listItem = CreateListItem(metaData);
            _fastList.RowsData.Add(listItem);
            if (Controller.IsSelected(listItem.ID, Category)) selectedIndex = index;
            count++;
            index++;
        }

        protected ListItem CreateListItem(MapThemeMetaData metaData)
        {
            string id = metaData.assetRef.fullName;
            string displayName = metaData.name;
            string author = GetAuthorName(metaData.assetRef);
            bool isFavourite = IsFavourite(id);
            bool isBlacklisted = IsBlacklisted(id);
            return new ListItem(id, displayName, author, isFavourite, isBlacklisted, Category);
        }

        private static string GetAuthorName(Package.Asset asset)
        {
            if (!ulong.TryParse(asset.package.packageAuthor.Substring("steamid:".Length), out ulong authorID))
                return "N/A";
            string author = new Friend(new UserID(authorID)).personaName;
            return author;
        }
    }
}
