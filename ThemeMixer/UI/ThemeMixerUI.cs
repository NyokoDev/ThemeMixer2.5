using System.Collections.Generic;
using ColossalFramework.UI;
using ThemeMixer.Serialization;
using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;
using ThemeMixer.UI.CategoryPanels;
using ThemeMixer.UI.Parts.SelectPanels;
using UnityEngine;

namespace ThemeMixer.UI
{
    public class ThemeMixerUI : PanelBase
    {
        private PanelBase _currentPanel;
        private ToolBar _toolBar;
        private UIPanel _space;

        public override void Start()
        {
            base.Start();
            Vector2 screenRes = UIView.GetAView().GetScreenResolution();
            relativePosition = SerializationService.Instance.GetToolBarPosition() ?? CalculateDefaultToolBarPosition();
            LayoutStart layoutStart = (relativePosition.x + 20.0f > screenRes.x / 2.0f) ? ((relativePosition.y + 137.5f > screenRes.y / 2.0f) ? LayoutStart.BottomRight : LayoutStart.TopRight) : (relativePosition.y + 137.5f > screenRes.y / 2.0f) ? LayoutStart.BottomLeft : LayoutStart.TopLeft;
            Setup("Theme Mixer UI", 0.0f, 275.0f, 0, true, LayoutDirection.Horizontal, layoutStart);
            autoFitChildrenHorizontally = false;
            CreateToolBar();
            EnsureToolbarOnScreen();
            RefreshZOrder();
        }

        private void CreateToolBar()
        {
            _toolBar = AddUIComponent<ToolBar>();
            _toolBar.EventButtonClicked += OnButtonClicked;
            _toolBar.EventDragEnd += OnDragEnd;
            _space = AddUIComponent<UIPanel>();
            _space.size = new Vector2(5.0f, 0.0f);
        }

        private void OnDragEnd()
        {
            Data.SetToolbarPosition(relativePosition);
        }

        public PanelBase CreatePanel(ThemeCategory category)
        {
            switch (category)
            {
                case ThemeCategory.Themes:
                    Controller.Part = ThemePart.Category;
                    return AddUIComponent<SelectThemePanel>();
                case ThemeCategory.Terrain:
                    return AddUIComponent<CategoryPanels.TerrainPanel>();
                case ThemeCategory.Water:
                    return AddUIComponent<CategoryPanels.WaterPanel>();
                case ThemeCategory.Structures:
                    return AddUIComponent<StructuresPanel>();
                case ThemeCategory.Weather:
                    return AddUIComponent<WeatherPanel>();
                case ThemeCategory.Atmosphere:
                    return AddUIComponent<AtmospherePanel>();
                case ThemeCategory.Mixes:
                    return AddUIComponent<MixesPanel>();
                case ThemeCategory.None:
                    return AddUIComponent<LutsPanel>();
                default: return null;
            }
        }

        private void OnButtonClicked(ToolbarButton button, ToolbarButton[] buttons)
        {
            UnfocusButtons(buttons);
            if (_currentPanel != null)
            {
                bool same = button.Category == _currentPanel.Category;
                Destroy(_currentPanel.gameObject);
                _currentPanel = null;
                if (same) return;
            }
            _currentPanel = CreatePanel(button.Category);
            SetButtonFocused(button);
            RefreshZOrder();
        }

        private void UnfocusButtons(IEnumerable<ToolbarButton> buttons)
        {
            foreach (ToolbarButton t in buttons)
            {
                SetButtonUnfocused(t);
            }
        }

        private static void SetButtonFocused(ToolbarButton button)
        {
            if (button == null) return;
            button.Button.normalBgSprite = button.Button.focusedBgSprite = button.Button.hoveredBgSprite = string.Concat(button.Button.normalBgSprite.Replace("Focused", ""), "Focused");
            button.Button.normalFgSprite = button.Button.focusedFgSprite = button.Button.hoveredFgSprite = string.Concat(button.Button.normalFgSprite.Replace("Focused", ""), "Focused");
        }

        private void SetButtonUnfocused(ToolbarButton button)
        {
            if (button == null) return;
            button.Button.normalBgSprite = button.Button.focusedBgSprite = button.Button.normalBgSprite.Replace("Focused", "");
            button.Button.hoveredBgSprite = button.Button.hoveredBgSprite.Replace("Focused", "Hovered");
            button.Button.normalFgSprite = button.Button.focusedFgSprite = button.Button.normalFgSprite.Replace("Focused", "");
            button.Button.hoveredFgSprite = button.Button.hoveredFgSprite.Replace("Focused", "Hovered");
        }

        #region Position
        private static Vector2 CalculateDefaultToolBarPosition()
        {
            Vector2 screenRes = UIView.GetAView().GetScreenResolution();
            return new Vector2(10.0f, screenRes.y - 403.0f);
        }

        private void EnsureToolbarOnScreen()
        {
            Vector2 screenRes = UIView.GetAView().GetScreenResolution();
            if (relativePosition.x < 0f || relativePosition.x > screenRes.x || relativePosition.y < 0f || relativePosition.y > screenRes.y)
            {
                relativePosition = CalculateDefaultToolBarPosition();
            }
        }

        public override void Update()
        {
            base.Update();
            Vector2 screenRes = UIView.GetAView().GetScreenResolution();

            if ((autoLayoutStart == LayoutStart.TopLeft || autoLayoutStart == LayoutStart.BottomLeft) && relativePosition.x > screenRes.x / 2.0f)
            {
                autoLayoutStart = autoLayoutStart == LayoutStart.TopLeft ? LayoutStart.TopRight : LayoutStart.BottomRight;
                RefreshZOrder();

            }
            else if ((autoLayoutStart == LayoutStart.TopRight || autoLayoutStart == LayoutStart.BottomRight) && relativePosition.x + width < screenRes.x / 2.0f)
            {
                autoLayoutStart = autoLayoutStart == LayoutStart.TopRight ? LayoutStart.TopLeft : LayoutStart.BottomLeft;
                RefreshZOrder();
            }
            if (_currentPanel == null) return;
            if ((autoLayoutStart == LayoutStart.TopLeft || autoLayoutStart == LayoutStart.TopRight) && relativePosition.y > screenRes.y / 2.0f || _currentPanel.absolutePosition.y + _currentPanel.height > screenRes.y)
            {
                autoLayoutStart = autoLayoutStart == LayoutStart.TopLeft ? LayoutStart.BottomLeft : LayoutStart.BottomRight;
                RefreshZOrder();
            }
            else if ((autoLayoutStart == LayoutStart.BottomLeft || autoLayoutStart == LayoutStart.BottomRight) && relativePosition.y + height < screenRes.y / 2.0f || _currentPanel.absolutePosition.y < 0.0f)
            {
                autoLayoutStart = autoLayoutStart == LayoutStart.BottomLeft ? LayoutStart.TopLeft : LayoutStart.TopRight;
                RefreshZOrder();
            }
        }

        private void RefreshZOrder()
        {
            switch (autoLayoutStart)
            {
                case LayoutStart.TopLeft:
                case LayoutStart.BottomLeft:
                    {
                        _toolBar.zOrder = 0;
                        _space.zOrder = 1;
                        if (_currentPanel != null) _currentPanel.zOrder = 2;
                        break;
                    }
                case LayoutStart.TopRight:
                case LayoutStart.BottomRight:
                    {
                        if (_currentPanel != null)
                        {
                            _currentPanel.zOrder = 0;
                            _space.zOrder = 1;
                            _toolBar.zOrder = 2;
                        }
                        else
                        {
                            _space.zOrder = 0;
                            _toolBar.zOrder = 1;
                        }

                        break;
                    }
            }
        }
        #endregion
    }
}
