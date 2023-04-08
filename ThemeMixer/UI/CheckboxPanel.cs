using ColossalFramework.UI;
using ThemeMixer.Resources;
using ThemeMixer.Themes.Enums;
using ThemeMixer.UI.Abstraction;
using UnityEngine;

namespace ThemeMixer.UI
{
    public class CheckboxPanel : PanelBase
    {
        public event PropertyChangedEventHandler<bool> EventCheckboxStateChanged;

        private UIPanel _checkboxPanel;
        private UICheckBox _checkbox;
        private UILabel _label;

        public override void OnDestroy()
        {
            _checkbox.eventCheckChanged -= OnCheckboxStateChanged;
            base.OnDestroy();
        }

        public override void Awake()
        {
            base.Awake();
            Category = ThemeCategory.Terrain;
            Setup("Checkbox Panel", 0.0f, 22.0f, 0, true);
            CreateCheckbox();
            CreateLabel();
        }

        private void CreateCheckbox()
        {
            _checkboxPanel = AddUIComponent<UIPanel>();
            _checkboxPanel.size = new Vector2(25.0f, 22.0f);
            _checkbox = _checkboxPanel.AddUIComponent<UICheckBox>();
            _checkbox.size = new Vector2(15.0f, 15.0f);
            _checkbox.relativePosition = new Vector2(5.0f, 3.5f);
            var sprite = _checkbox.AddUIComponent<UISprite>();
            sprite.spriteName = "check-unchecked";
            sprite.atlas = UISprites.DefaultAtlas;
            sprite.size = _checkbox.size;
            sprite.transform.parent = _checkbox.transform;
            sprite.transform.localPosition = Vector3.zero;
            var checkedBoxObj = sprite.AddUIComponent<UISprite>();
            checkedBoxObj.spriteName = "check-checked";
            checkedBoxObj.atlas = UISprites.DefaultAtlas;
            checkedBoxObj.size = _checkbox.size;
            checkedBoxObj.relativePosition = Vector3.zero;
            _checkbox.checkedBoxObject = checkedBoxObj;
            _checkbox.eventCheckChanged += OnCheckboxStateChanged;
            _checkbox.anchor = UIAnchorStyle.CenterVertical | UIAnchorStyle.Left;
        }

        private void CreateLabel()
        {
            _label = AddUIComponent<UILabel>();
            _label.textAlignment = UIHorizontalAlignment.Left;
            _label.verticalAlignment = UIVerticalAlignment.Middle;
            _label.font = UIUtils.Font;
            _label.padding = new RectOffset(4, 0, 4, 0);
            _label.anchor = UIAnchorStyle.CenterVertical | UIAnchorStyle.Left;
            _label.relativePosition = new Vector2(25.0f, 0.0f);
        }

        public void Initialize(bool state, string text, string checkboxTooltip)
        {
            SetState(state);
            _label.text = text;
            _label.tooltip = _checkbox.tooltip = checkboxTooltip;
            autoFitChildrenHorizontally = true;
        }

        public void SetState(bool state)
        {
            _checkbox.isChecked = state;
        }

        public void MakeSmallVersion()
        {
            _label.textScale = 0.8f;
            _checkboxPanel.height = 18.0f;
            height = 18.0f;

        }

        private void OnCheckboxStateChanged(UIComponent component, bool value)
        {
            EventCheckboxStateChanged?.Invoke(this, value);
        }
    }
}
