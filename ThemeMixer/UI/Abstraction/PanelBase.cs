using System;
using ColossalFramework.UI;
using ThemeMixer.Resources;
using ThemeMixer.Serialization;
using ThemeMixer.Themes.Enums;
using UnityEngine;

namespace ThemeMixer.UI.Abstraction
{
    public class PanelBase : UIPanel
    {
        public static event EventHandler<ThemeDirtyEventArgs> EventThemeDirty;
        protected UIController Controller => UIController.Instance;
        protected SerializationService Data => SerializationService.Instance;
        
        protected static Color32 UIColorDark { get; set; } = new Color32(191, 191, 191, 255);
        protected static Color32 UIColorLight { get; set; } = new Color32(128, 128, 128, 255);
        protected static Color32 UIColorGrey { get; set; } = new Color32(55, 58, 60, 255);
        public ThemeCategory Category { get; set; } = ThemeCategory.None;

        public override void Awake()
        {
            base.Awake();
            Controller.EventUIDirty += OnRefreshUI;
        }

        public override void OnDestroy()
        {
            Controller.EventUIDirty -= OnRefreshUI;
            base.OnDestroy();
        }

        public virtual void Setup(string panelName, float panelWidth, float panelHeight, int spacing = UIUtils.DefaultSpacing, bool panelAutoLayout = false, LayoutDirection layoutDirection = LayoutDirection.Horizontal, LayoutStart layoutStart = LayoutStart.TopLeft, string bgSprite = "")
        {
            name = panelName;
            width = panelWidth;
            height = panelHeight;
            autoLayout = panelAutoLayout;
            autoLayoutDirection = layoutDirection;
            switch (layoutDirection)
            {
                case LayoutDirection.Horizontal: autoFitChildrenHorizontally = true; break;
                case LayoutDirection.Vertical: autoFitChildrenVertically = true; break;
            }
            autoLayoutStart = layoutStart;
            atlas = UISprites.DefaultAtlas;
            backgroundSprite = bgSprite;
            switch (layoutStart)
            {
                case LayoutStart.TopLeft:
                    padding = new RectOffset(spacing, 0, spacing, 0);
                    autoLayoutPadding = new RectOffset(0, spacing, 0, spacing);
                    break;

                case LayoutStart.BottomLeft:
                    padding = new RectOffset(spacing, 0, 0, spacing);
                    autoLayoutPadding = new RectOffset(0, spacing, spacing, 0);
                    break;

                case LayoutStart.TopRight:
                    padding = new RectOffset(0, spacing, 0, spacing);
                    autoLayoutPadding = new RectOffset(spacing, 0, spacing, 0);
                    break;

                case LayoutStart.BottomRight:
                    padding = new RectOffset(spacing, 0, 0, spacing);
                    autoLayoutPadding = new RectOffset(0, spacing, spacing, 0);
                    break;
            }
            builtinKeyNavigation = true;
            color = Resources.ColorData.UIColor;
        }

        protected virtual void OnRefreshUI(object sender, UIDirtyEventArgs eventArgs) { }

        protected virtual void OnRefreshTheme(object sender, ThemeDirtyEventArgs eventArgs)
        {
            EventThemeDirty?.Invoke(sender, eventArgs);
        }
    }
}
