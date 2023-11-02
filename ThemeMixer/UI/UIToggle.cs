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
       

            // Read the hotkey from the TM2.5_key_config.txt file
            string hotkeyFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "TM2.5_key_config.txt"
            );

            if (!File.Exists(hotkeyFilePath))
            {
                // Create the file with a default hotkey if it doesn't exist
                KeyCode defaultHotkey = KeyCode.None; // Replace with your desired default hotkey
                UIToggle.referenceHotkey = defaultHotkey.ToString();
                File.WriteAllText(hotkeyFilePath, defaultHotkey.ToString());
            }

            string hotkeyString = File.ReadAllText(hotkeyFilePath).Trim();
            KeyCode hotkey;
            if (Enum.IsDefined(typeof(KeyCode), hotkeyString))
            {
                hotkey = (KeyCode)Enum.Parse(typeof(KeyCode), hotkeyString);

                // Check if the hotkey has changed
                if (hotkey != _hotkey)
                {
                    // Save the new hotkey
                    _hotkey = hotkey;

                    // Perform any necessary actions with the new hotkey
                    // ...

                    // Show a message indicating that the hotkey has been updated
                    Debug.Log("New hotkey saved: " + _hotkey);
                    

                }
            }
            else
            {
                Debug.LogError("Invalid hotkey: " + hotkeyString);
                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage("Hotkey Save Error", "Failed to save the hotkey settings.", true);
            }
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
        }

        public override void Update()
        {
            base.Update();

            // Handle the key press event using the current hotkey value
            if (Input.GetKeyDown(_hotkey))
            {
                Debug.Log("Hotkey pressed: " + _hotkey);
                EventUIToggleClicked?.Invoke();
                _toggled = !_toggled;
                normalBgSprite = _toggled ? UISprites.UIToggleIconFocused : UISprites.UIToggleIcon;
            }
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

