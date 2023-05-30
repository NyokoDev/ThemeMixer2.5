using ColossalFramework.UI;
using ThemeMixer.Locale;
using ThemeMixer.Resources;
using ThemeMixer.Themes.Enums;
using ThemeMixer.TranslationFramework;
using ThemeMixer.UI.Abstraction;
using UnityEngine;

namespace ThemeMixer.UI.CategoryPanels
{
    public class LutsPanel : PanelBase
    {
        private UIPanel _labelPanel;
        private UILabel _label;

        private PanelBase _loadLutPanel;
        private UIDropDown _loadLutDropDown;

        private bool _disableEvents;

        public override void Awake()
        {
            bool isRenderItEnabled = ThemeMixer.ModUtils.ModChecker.IsModEnabled("Render It!");
            _disableEvents = isRenderItEnabled;
            base.Awake();
            Category = ThemeCategory.None;
            Setup("Luts Panel", 360.0f, 0.0f, 5, true, LayoutDirection.Vertical, LayoutStart.TopLeft, "GenericPanel");
            CreateTitleLabel();
            CreateLoadLutsPanel();
            this.CreateSpace(0.0f, 0.1f);

            if (isRenderItEnabled)
            {

                Debug.Log("Theme Mixer 2.5: Render It! mod is enabled");
            }
            else
            {
                Debug.Log("Theme Mixer 2.5: Render It! mod is not enabled");

            }
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
            _label.text = Translation.Instance.GetTranslation(TranslationID.LABEL_LUTS);
            _label.anchor = UIAnchorStyle.CenterHorizontal | UIAnchorStyle.CenterVertical;
        }

        private void CreateLoadLutsPanel()
        {
            _loadLutPanel = AddUIComponent<PanelBase>();
            _loadLutPanel.Setup("Select Mix Panel", 350.0f, 0.0f, 5, true, LayoutDirection.Vertical, LayoutStart.TopLeft, "WhiteRect");
            _loadLutPanel.color = UIColorGrey;
            _loadLutPanel.CreateSpace(0.0f, 0.01f);
            CreateLabel(_loadLutPanel, Translation.Instance.GetTranslation(TranslationID.LABEL_SELECTLUT));
            _loadLutPanel.CreateSpace(0.0f, 0.01f);
            CreateDropDown();
            _loadLutPanel.CreateSpace(0.0f, 0.01f);
            RefreshDropdown();
        }

        private void RefreshDropdown()
        {
            _disableEvents = true;
            _loadLutDropDown.localizedItems = ColorCorrectionManager.instance.items;
            _loadLutDropDown.selectedIndex = ColorCorrectionManager.instance.lastSelection;
            _disableEvents = false;
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
            var panel = _loadLutPanel.AddUIComponent<UIPanel>();
            panel.size = new Vector2(340.0f, 30.0f);
            _loadLutDropDown = panel.AddUIComponent<UIDropDown>();
            _loadLutDropDown.relativePosition = Vector3.zero;
            _loadLutDropDown.atlas = UISprites.DefaultAtlas;
            _loadLutDropDown.size = new Vector2(340f, 30f);
            _loadLutDropDown.listBackground = "StylesDropboxListbox";
            _loadLutDropDown.itemHeight = 30;
            _loadLutDropDown.itemHover = "ListItemHover";
            _loadLutDropDown.itemHighlight = "ListItemHighlight";
            _loadLutDropDown.normalBgSprite = "CMStylesDropbox";
            _loadLutDropDown.hoveredBgSprite = "CMStylesDropboxHovered";
            _loadLutDropDown.listWidth = 340;
            _loadLutDropDown.listHeight = 500;
            Vector2 screenRes = UIView.GetAView().GetScreenResolution();
            _loadLutDropDown.listPosition = _loadLutDropDown.absolutePosition.y > screenRes.y / 2.0f ? UIDropDown.PopupListPosition.Above : UIDropDown.PopupListPosition.Below;
            _loadLutDropDown.foregroundSpriteMode = UIForegroundSpriteMode.Stretch;
            _loadLutDropDown.popupColor = Color.white;
            _loadLutDropDown.popupTextColor = new Color32(170, 170, 170, 255);
            _loadLutDropDown.textScale = 0.8f;
            _loadLutDropDown.verticalAlignment = UIVerticalAlignment.Middle;
            _loadLutDropDown.horizontalAlignment = UIHorizontalAlignment.Left;
            _loadLutDropDown.textFieldPadding = new RectOffset(12, 0, 10, 0);
            _loadLutDropDown.itemPadding = new RectOffset(12, 0, 10, 0);
            _loadLutDropDown.triggerButton = _loadLutDropDown;
            _loadLutDropDown.isLocalized = true;
            _loadLutDropDown.localeID = LocaleID.BUILTIN_COLORCORRECTION;
            _loadLutDropDown.listScrollbar = Controller.Scrollbar;
            _loadLutDropDown.eventDropdownOpen += OnDropDownOpen;
            _loadLutDropDown.eventDropdownClose += OnDropDownClose;
            _loadLutDropDown.eventSelectedIndexChanged += OnSelectedIndexChanged;
        }

        private void OnSelectedIndexChanged(UIComponent component, int value)
        {
            if (_disableEvents) return;
            ColorCorrectionManager.instance.currentSelection = value;
        }

        private void OnDropDownOpen(UIDropDown dropdown, UIListBox popup, ref bool overridden)
        {
            if (_disableEvents) return;
            _loadLutDropDown.triggerButton.isInteractive = false;
        }

        private void OnDropDownClose(UIDropDown dropdown, UIListBox popup, ref bool overridden)
        {
            if (_disableEvents) return;
            _loadLutDropDown.triggerButton.isInteractive = true;
        }
    }
}
