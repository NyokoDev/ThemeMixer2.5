using System;
using System.IO;
using ColossalFramework.UI;
using ICities;
using ThemeMixer.Locale;
using ThemeMixer.Resources;
using ThemeMixer.TranslationFramework;
using ThemeMixer.UI;
using ThemeMixer.UI.Abstraction;
using UnityEngine;

namespace TM
{
    public class TM2_5
    {

        public void OnSettingsUI(UIHelperBase helper)
        {
            var panel = (helper.AddGroup("Theme Mixer 2.5") as UIHelper).self as UIPanel;
            panel.backgroundSprite = null; 
            //Set what you wanna size.
            var dropDown = AddDropDown(panel, Translation.Instance.GetTranslation(TranslationID.SETTINGS_INTERFACE_THEME_LABEL), new string[] { "Purple", "Dark Blue", "Red", "Light Blue", "Default" }, 0, (_) =>
            {
                if (_ == 0)
                {
                    ColorData.UIColor = ColorData.UIColorPurple;
                }
                else if (_ == 1)
                {
                    ColorData.UIColor = ColorData.UIColorDarkBlue;
                }
                else if (_ == 2)
                {
                    ColorData.UIColor = ColorData.UIColorRed;
                }
                else if (_ == 3)
                {
                    ColorData.UIColor = ColorData.UIColorLightBlue;
                }
                else
                {
                    ColorData.UIColor = new Color32(200, 200, 200, 255);
                }
                ColorData.Save(); // Save the selected color to storage
            });

            UIToggle toggle = UnityEngine.Object.FindObjectOfType<UIToggle>();



            // Add the dropdown to the panel
            var dropDown2 = AddDropDown(panel, Translation.Instance.GetTranslation(TranslationID.SETTINGS_INTERFACE_HOTKEY_LABEL),
   new string[] { UIToggle.referenceHotkey, "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "Insert", "Delete", "Home", "Page Up", "Page Down", "End", "Left Shift", "Right Shift" }, 0, (_) =>
   {
       // Find the UIToggle component you want to change
       UIToggle toggle3 = UnityEngine.Object.FindObjectOfType<UIToggle>();

       // Set the hotkey to the desired KeyCode based on the selected option from the dropdown
       if (_ == 0)
       {
           toggle3._hotkey = toggle3._hotkey;
       }
       else if (_ == 1)
       {
           toggle3._hotkey = KeyCode.F1;
       }
       else if (_ == 2)
       {
           toggle3._hotkey = KeyCode.F2;
       }
       else if (_ == 3)
       {
           toggle3._hotkey = KeyCode.F3;
       }
       else if (_ == 4)
       {
           toggle3._hotkey = KeyCode.F4;
       }
       else if (_ == 5)
       {
           toggle3._hotkey = KeyCode.F5;
       }
       else if (_ == 6)
       {
           toggle3._hotkey = KeyCode.F6;
       }
       else if (_ == 7)
       {
           toggle3._hotkey = KeyCode.F7;
       }
       else if (_ == 8)
       {
           toggle3._hotkey = KeyCode.F8;
       }
       else if (_ == 9)
       {
           toggle3._hotkey = KeyCode.F9;
       }
       else if (_ == 10)
       {
           toggle3._hotkey = KeyCode.F10;
       }
       else if (_ == 11)
       {
           toggle3._hotkey = KeyCode.F11;
       }
       else if (_ == 12)
       {
           toggle3._hotkey = KeyCode.F12;
       }
       else if (_ == 13)
       {
           toggle3._hotkey = KeyCode.Insert;
       }
       else if (_ == 14)
       {
           toggle3._hotkey = KeyCode.Delete;
       }
       else if (_ == 15)
       {
           toggle3._hotkey = KeyCode.Home;
       }
       else if (_ == 16)
       {
           toggle3._hotkey = KeyCode.PageUp;
       }
       else if (_ == 17)
       {
           toggle3._hotkey = KeyCode.PageDown;
       }
       else if (_ == 18)
       {
           toggle3._hotkey = KeyCode.End;
       }
       else if (_ == 19)
       {
           toggle3._hotkey = KeyCode.LeftShift;
       }
       else if (_ == 20)
       {
           toggle3._hotkey = KeyCode.RightShift;
       }

       Debug.Log("Hotkey changed to " + toggle3._hotkey);
   });





            dropDown.relativePosition = new Vector2(10, 10);

            var donateButton = AddButton(panel, Translation.Instance.GetTranslation(TranslationID.SETTINGS_DONATE_THEME_LABEL), () => Application.OpenURL("https://www.paypal.com/donate/?hosted_button_id=DZYTC3AEG85V8"));
            donateButton.relativePosition = new Vector2(50, 80);
            var supportButton = AddButton(panel, Translation.Instance.GetTranslation(TranslationID.SETTINGS_SUPPORT_THEME_LABEL), () => Application.OpenURL("https://steamcommunity.com/workshop/filedetails/discussion/2954236385/3819655917505218354/"));
            supportButton.relativePosition = new Vector2(50, donateButton.relativePosition.y + donateButton.size.y + 10);
            // Create a reset button that sets the UIToggle position to the default position

            var resetButton = AddButton(panel, Translation.Instance.GetTranslation(TranslationID.RESET_UI_TOGGLE_LABEL), () =>
            {
                // Create an instance of the UIToggle class


                // Get the default position for the UIToggle from the instance
                var defaultPosition = toggle.GetDefaultPosition();
            });

            resetButton.relativePosition = new Vector2(50, resetButton.relativePosition.y + resetButton.size.y + 10);
        }

        private void SaveHotkeyToFile(string hotkey)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string directoryPath = appDataPath + "\\Colossal Order\\Cities_Skylines";
            Directory.CreateDirectory(directoryPath);

            string filePath = directoryPath + "\\TM2.5_key_config.txt";

            // Check if the file already exists
            if (!File.Exists(filePath))
            {
                // Create the file if it doesn't exist
                File.Create(filePath).Close();
            }

            // Write the hotkey to the file
            File.WriteAllText(filePath, hotkey);

            // Show the exception panel to inform the user about the hotkey change
            

            Debug.Log("Hotkey changed to " + hotkey);
        }

        private static UIButton AddButton(UIComponent parent, string text, Action callback)
        {
            var button = parent.AttachUIComponent(UITemplateManager.GetAsGameObject(@"OptionsButtonTemplate")) as UIButton;
            button.wordWrap = false;
            button.text = text;
            button.eventClicked += (s, e) => callback?.Invoke();
            return button;
        }
        private static UIDropDown AddDropDown(UIComponent parent, string text, string[] options, int defaultSelection, Action<int> callback)
        {
            UIPanel uiPanel = parent.AttachUIComponent(UITemplateManager.GetAsGameObject(@"OptionsDropdownTemplate")) as UIPanel;
            UILabel label = uiPanel.Find<UILabel>(@"Label");
            label.autoSize = true;
            label.textScale = 0.95f;
            label.text = text;
            UIDropDown dropDown = uiPanel.Find<UIDropDown>(@"Dropdown");
            dropDown.width = 380;
            dropDown.items = options;
            dropDown.selectedIndex = defaultSelection;
            dropDown.eventSelectedIndexChanged += (c, v) => callback?.Invoke(v);
            return dropDown;
        }

        internal void OnSettingsUI()
        {
            throw new NotImplementedException();
        }
    }
}
