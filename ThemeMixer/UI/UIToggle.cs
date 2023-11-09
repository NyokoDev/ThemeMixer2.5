using System;
using System.IO;
using AlgernonCommons.Keybinding;
using ColossalFramework.UI;
using ICities;
using ThemeMixer.Resources;
using ThemeMixer.Serialization;
using UnifiedUI.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace ThemeMixer.UI
{
    public class UIToggle : UIButton
    {

        public delegate void UIToggleClickedEventHandler();
        public event UIToggleClickedEventHandler EventUIToggleClicked;
        public KeyCode _hotkey = KeyCode.T;
        public static string referenceHotkey;


        public static bool _toggled;
        private Vector3 DeltaPos { get; set; }
        private Keybinding keybinding;


        internal static bool ensurance = true;

        public override void Start()
        {
            base.Start();
            name = "Theme Mixer Toggle";
            atlas = UISprites.Atlas;
            normalBgSprite = UISprites.UIToggleIcon;
            hoveredBgSprite = UISprites.UIToggleIconHovered;
            pressedBgSprite = UISprites.UIToggleIconPressed;
            absolutePosition = SerializationService.Instance.GetUITogglePosition() ?? GetDefaultPosition();



        }


        public Vector2 GetDefaultPosition()
        {
            UIComponent referenceComponent = GetUIView().FindUIComponent<UIComponent>("UnlockButton");
            Vector2 pos = new Vector2(referenceComponent.absolutePosition.x + 80.0f, referenceComponent.absolutePosition.y + (referenceComponent.height - height) / 2);
            return pos;
        }

        protected override void OnClick(UIMouseEventParameter p)
        {
            if (!p.buttons.IsFlagSet(UIMouseButton.Left)) return;
            _toggled = !_toggled;
            EventUIToggleClicked?.Invoke();
            normalBgSprite = _toggled ? UISprites.UIToggleIconFocused : UISprites.UIToggleIcon;
            Mod._uuiButton.IsPressed = false;
        }


        public void OnClickUUI()
        {
            _toggled = !_toggled;
            EventUIToggleClicked?.Invoke();
            normalBgSprite = _toggled ? UISprites.UIToggleIconFocused : UISprites.UIToggleIcon;
            Debug.Log("Theme Mixer 2.5: OnClick at UIToggle.cs triggered.");
        }

        protected override void OnMouseDown(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Right))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.y = m_OwnerView.fixedHeight - mousePos.y;

                DeltaPos = absolutePosition - mousePos;
                BringToFront();
            }
        }

        protected override void OnMouseMove(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Right))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.y = m_OwnerView.fixedHeight - mousePos.y;
                absolutePosition = mousePos + DeltaPos;
                SerializationService.Instance.SetUITogglePosition(new Vector2(absolutePosition.x, absolutePosition.y));
            }
        }

        protected override void OnMouseUp(UIMouseEventParameter p)
        {
            base.OnMouseUp(p);
            if (p.buttons.IsFlagSet(UIMouseButton.Right))
            {
                SerializationService.Instance.SaveData();
            }
        }

      

        public void Toggle()
        {
            _toggled = !_toggled;
            normalBgSprite = _toggled ? UISprites.UIToggleIconFocused : UISprites.UIToggleIcon;
        }
    }
}

