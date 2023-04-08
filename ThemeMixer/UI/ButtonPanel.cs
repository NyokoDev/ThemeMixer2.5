using ColossalFramework.UI;
using ThemeMixer.UI.Abstraction;
using UnityEngine;

namespace ThemeMixer.UI
{
    public class ButtonPanel : PanelBase
    {
        public delegate void ButtonClickedEventHandler();
        public event ButtonClickedEventHandler EventButtonClicked;

        private UIButton _button;
        public override void Awake()
        {
            base.Awake();
            CreateButton();
            this.CreateSpace(0.0f, 5.0f);
        }

        public override void OnDestroy()
        {
            EventButtonClicked = null;
            _button.eventClicked -= OnButtonClicked;
            base.OnDestroy();
        }

        private void CreateButton()
        {
            _button = UIUtils.CreateButton(this, new Vector2(112.5f, 30.0f));
            _button.eventClicked += OnButtonClicked;
        }

        public void SetAnchor(UIAnchorStyle anchors)
        {
            _button.anchor = anchors;
        }
        public void AlignRight()
        {
            _button.relativePosition = new Vector3(width - _button.width, 0.0f);
        }

        public void SetText(string text, string buttonTooltip = "")
        {
            _button.text = text;
            _button.tooltip = buttonTooltip;
        }

        private void OnButtonClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            EventButtonClicked?.Invoke();
        }

        internal void DisableButton()
        {
            _button.Disable();
        }
        internal void EnableButton(string text = null)
        {
            _button.Enable();
            if (text != null)
                SetText(text);
        }
    }
}
