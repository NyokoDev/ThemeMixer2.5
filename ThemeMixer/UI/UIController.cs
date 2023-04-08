using System;
using ColossalFramework.UI;
using JetBrains.Annotations;
using ThemeMixer.Themes;
using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;
using ThemeMixer.UI.Parts.SelectPanels;
using UnityEngine;

namespace ThemeMixer.UI
{
    public class UIController : MonoBehaviour
    {
        public event EventHandler<UIDirtyEventArgs> EventUIDirty;

        private static UIController _instance;
        public static UIController Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<UIController>();
                if (_instance != null) return _instance;
                GameObject gameObject = GameObject.Find("ThemeMixer");
                if (gameObject == null) gameObject = new GameObject("ThemeMixer");
                _instance = gameObject.AddComponent<UIController>();
                DontDestroyOnLoad(_instance.gameObject);
                return _instance;
            }
        }

        public ThemePart Part { get; set; } = ThemePart.None;
        public TextureID TextureID { get; private set; }
        public ColorID ColorID { get; private set; }
        public OffsetID OffsetID { get; private set; }
        public ValueID ValueID { get; private set; }

        public UIScrollbar Scrollbar { get; private set; }

        private static bool InGame => ToolManager.instance?.m_properties != null && (ToolManager.instance.m_properties?.m_mode & ItemClass.Availability.GameAndMap) != 0;

        internal Color GetCurrentColor(ColorID colorID)
        {
            return ThemeManager.Instance.GetCurrentColor(colorID);
        }

        private UIRoot _ui;
        private UIRoot ThemeMixerUI
        {
            get
            {
                if (_ui == null) _ui = FindObjectOfType<UIRoot>();
                return _ui;
            }
            set => _ui = value;
        }
        private SelectPanel ThemeSelector { get; set; }

        private UIToggle _toggle;
        private UIToggle UIToggle
        {
            get
            {
                if (_toggle == null) _toggle = FindObjectOfType<UIToggle>();
                return _toggle;
            }
            set => _toggle = value;
        }

        public static UIController Ensure() => Instance;

        public void OnEnabled()
        {
            if (!InGame) return;
            OnLevelLoaded();
        }

        public void OnLevelLoaded()
        {
            InstantiateScrollbar();
            if (UIToggle != null)
            {
                Destroy(UIToggle.gameObject);
                UIToggle = null;
            }
            UIToggle = UIView.GetAView().AddUIComponent(typeof(UIToggle)) as UIToggle;
            if (UIToggle != null) UIToggle.EventUIToggleClicked += OnUIToggleClicked;
        }

        private void InstantiateScrollbar()
        {
            if (Scrollbar != null) return;
            Scrollbar = Instantiate(UIView.Find<UIDropDown>("ColorCorrection").listScrollbar);
            Scrollbar.transform.parent = gameObject.transform;
            Scrollbar.width = 12.0f;
            Scrollbar.trackObject.width = 12.0f;
            ((UISlicedSprite)Scrollbar.trackObject).spriteName = "LevelBarBackground";
            Scrollbar.thumbObject.width = 12.0f;
            ((UISlicedSprite)Scrollbar.thumbObject).spriteName = "LevelBarForeground";
        }

        public void OnLevelUnloaded()
        {
            DestroyUI();
        }

        public static void Release()
        {
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
                _instance = null;
            }
        }

        public bool IsSelected(string themeID, ThemeCategory category)
        {
            switch (Part)
            {
                case ThemePart.Category:
                    return ThemeManager.Instance.IsSelected(themeID, category);
                case ThemePart.Color:
                    return ThemeManager.Instance.IsSelected(themeID, ColorID);
                case ThemePart.Offset:
                    return ThemeManager.Instance.IsSelected(themeID, OffsetID);
                case ThemePart.Texture:
                    return ThemeManager.Instance.IsSelected(themeID, TextureID);
                case ThemePart.Value:
                    return ThemeManager.Instance.IsSelected(themeID, ValueID);
                default: return false;
            }
        }

        public float GetTilingValue(TextureID textureID)
        {
            return ThemeManager.Instance.GetTilingValue(textureID);
        }

        public void OnLoadFromTheme<T>(ThemeCategory category, T id)
        {
            var part = ThemePart.None;
            if (id is TextureID textureID)
            {
                part = ThemePart.Texture;
                TextureID = textureID;
            }
            else if (id is ColorID colorID)
            {
                part = ThemePart.Color;
                ColorID = colorID;
            }
            else if (id is OffsetID offsetID)
            {
                part = ThemePart.Offset;
                OffsetID = offsetID;
            }
            else if (id is ValueID valueID)
            {
                part = ThemePart.Value;
                ValueID = valueID;
            }
            else if (id is ThemeCategory)
            {
                part = ThemePart.Category;
            }
            if (part != ThemePart.None) ShowThemeSelectorPanel(category, part);
        }

        internal Vector3 GetOffsetValue(OffsetID offsetID)
        {
            return ThemeManager.Instance.GetOffsetValue(offsetID);
        }

        [UsedImplicitly]
        private void Awake()
        {
            PanelBase.EventThemeDirty += OnThemeDirty;
            ThemeManager.Instance.EventUIDirty += OnUIDirty;
        }

        private void OnUIDirty(object sender, UIDirtyEventArgs e)
        {
            EventUIDirty?.Invoke(sender, e);
        }


        [UsedImplicitly]
        private void OnDestroy()
        {
            PanelBase.EventThemeDirty -= OnThemeDirty;
            ThemeManager.Instance.EventUIDirty -= OnUIDirty;
        }

        private void DestroyUI()
        {
            if (UIToggle != null)
            {
                Destroy(UIToggle.gameObject);
                UIToggle = null;
            }

            if (ThemeMixerUI == null) return;
            Destroy(ThemeMixerUI.gameObject);
            ThemeMixerUI = null;
        }

        private void OnUIToggleClicked()
        {
            if (ThemeMixerUI != null)
            {
                Destroy(ThemeMixerUI.gameObject);
                ThemeMixerUI = null;
                return;
            }
            ThemeMixerUI = UIView.GetAView().AddUIComponent(typeof(UIRoot)) as UIRoot;
            ThemeMixerUI?.AddUIComponent<ThemeMixerUI>();
        }

        public void OnTilingChanged(TextureID textureID, float value)
        {
            ThemeManager.Instance.OnTilingChanged(textureID, value);
        }

        private void ShowThemeSelectorPanel(ThemeCategory category, ThemePart part)
        {
            Part = part;
            ThemeMixerUI.isVisible = false;
            UIToggle.isInteractive = false;
            switch (category)
            {
                case ThemeCategory.Terrain:
                    ThemeSelector = UIView.GetAView().AddUIComponent(typeof(SelectTerrainPanel)) as SelectTerrainPanel;
                    break;
                case ThemeCategory.Water:
                    ThemeSelector = UIView.GetAView().AddUIComponent(typeof(SelectWaterPanel)) as SelectWaterPanel;
                    break;
                case ThemeCategory.Structures:
                    ThemeSelector = UIView.GetAView().AddUIComponent(typeof(SelectStructuresPanel)) as SelectStructuresPanel;
                    break;
                case ThemeCategory.Atmosphere:
                    ThemeSelector = UIView.GetAView().AddUIComponent(typeof(SelectAtmospherePanel)) as SelectAtmospherePanel;
                    break;
                case ThemeCategory.Weather:
                    ThemeSelector = UIView.GetAView().AddUIComponent(typeof(SelectWeatherPanel)) as SelectWeatherPanel;
                    break;
            }
        }

        internal void SaveMix(string saveName)
        {
            ThemeManager.Instance.SaveMix(saveName);
        }

        public void OnThemeSelectorPanelClosing(object sender, ThemesPanelClosingEventArgs e)
        {
            Part = ThemePart.None;
            if (ThemeSelector != null) Destroy(ThemeSelector.gameObject);
            ThemeMixerUI.isVisible = true;
            UIToggle.isInteractive = true;
        }

        public void OnThemeSelected(object sender, ThemeSelectedEventArgs e)
        {
            switch (e.Part)
            {
                case ThemePart.Category:
                    ThemeManager.Instance.LoadCategory(e.Category, e.ThemeID);
                    break;
                case ThemePart.Texture:
                    ThemeManager.Instance.LoadTexture(TextureID, e.ThemeID);
                    break;
                case ThemePart.Color:
                    ThemeManager.Instance.LoadColor(ColorID, e.ThemeID);
                    break;
                case ThemePart.Offset:
                    ThemeManager.Instance.LoadOffset(OffsetID, e.ThemeID);
                    break;
                case ThemePart.Value:
                    ThemeManager.Instance.LoadValue(ValueID, e.ThemeID);
                    break;
            }
        }

        internal void LoadMix(ThemeMix mix)
        {
            ThemeManager.Instance.LoadMix(mix);
        }

        internal void OnOffsetChanged(OffsetID offsetID, Vector3 value)
        {
            ThemeManager.Instance.OnOffsetChanged(offsetID, value);
        }

        internal void OnValueChanged<T>(ValueID valueID, T value)
        {
            ThemeManager.Instance.OnValueChanged(valueID, value);
        }

        private void OnThemeDirty(object sender, ThemeDirtyEventArgs e)
        {
            ThemeManager.Instance.OnThemeDirty(e);
        }

        internal T GetValue<T>(ValueID valueID)
        {
            return ThemeManager.Instance.GetValue<T>(valueID);
        }

        internal void OnColorChanged(ColorID colorID, Color defaultValue)
        {
            ThemeManager.Instance.OnColorChanged(colorID, defaultValue);
        }

        internal Color GetColor(ColorID colorID, string themeID)
        {
            return ThemeManager.Instance.GetColor(colorID, themeID);
        }

        public void CloseUI()
        {
            OnUIToggleClicked();
        }
    }
}
