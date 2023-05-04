using ColossalFramework.Importers;
using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using ThemeMixer.Locale;
using ThemeMixer.Resources;
using ThemeMixer.TranslationFramework;
using ThemeMixer.UI;
using UnityEngine;

namespace TM {
    public class TM2_5 {

        public void OnSettingsUI(UIHelperBase helper) {
            var panel = (helper.AddGroup("Theme Mixer 2.5") as UIHelper).self as UIPanel;
            panel.atlas = TM2Atlas.TMAtlas;
            panel.backgroundSprite = TM2Atlas.BCK;
            //Set what you wanna size.
            panel.size = new Vector2();
            var dropDown = AddDropDown(panel, Translation.Instance.GetTranslation(TranslationID.SETTINGS_INTERFACE_THEME_LABEL), new string[] { "Purple", "Dark Blue", "Red", "Light Blue", "Default" }, 0, (_) => {
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

            var dropDown2 = AddDropDown(panel, Translation.Instance.GetTranslation(TranslationID.SETTINGS_INTERFACE_HOTKEY_LABEL),
    new string[] { "Ctrl+Alt+Z", "Ctrl+Shift+C", "Alt+X", "Ctrl+Shift+V", "Ctrl+Alt+Shift+D" }, 0, (_) =>
    {
        // Find the UIToggle component you want to change
        UIToggle toggle = UnityEngine.Object.FindObjectOfType<UIToggle>();

        // Set the hotkey to the desired KeyCode based on the selected option from the dropdown
        if (_ == 0)
        {
            toggle._hotkey = KeyCode.LeftAlt | KeyCode.LeftControl | KeyCode.Z;
        }
        else if (_ == 1)
        {
            toggle._hotkey = KeyCode.LeftControl | KeyCode.LeftShift | KeyCode.C;
        }
        else if (_ == 2)
        {
            toggle._hotkey = KeyCode.LeftAlt | KeyCode.X;
        }
        else if (_ == 3)
        {
            toggle._hotkey = KeyCode.LeftShift | KeyCode.LeftControl | KeyCode.V;
        }
        else if (_ == 4)
        {
            toggle._hotkey = KeyCode.LeftAlt | KeyCode.LeftControl | KeyCode.LeftShift | KeyCode.D;
        }

        Debug.Log("Hotkey changed to " + toggle._hotkey);
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
                var toggle = new UIToggle();

                // Get the default position for the UIToggle from the instance
                var defaultPosition = toggle.GetDefaultPosition();
            });

            resetButton.relativePosition = new Vector2(50, resetButton.relativePosition.y + resetButton.size.y + 10);
        }


        private static UIButton AddButton(UIComponent parent, string text, Action callback) {
            var button = parent.AttachUIComponent(UITemplateManager.GetAsGameObject(@"OptionsButtonTemplate")) as UIButton;
            button.wordWrap = false;
            button.text = text;
            button.eventClicked += (s, e) => callback?.Invoke();
            return button;
        }
        private static UIDropDown AddDropDown(UIComponent parent, string text, string[] options, int defaultSelection, Action<int> callback) {
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


    internal class TM2Atlas {
        private static UITextureAtlas tMAtlas;
        public static Dictionary<string, RectOffset> SpriteParams { get; private set; } = new Dictionary<string, RectOffset>();
        public static string BCK => nameof(BCK);
        //Your image name, also included in you project.
        static TM2Atlas() => SpriteParams[BCK] = new RectOffset(4, 4, 4, 4);
        //Atlas what we create.
        public static UITextureAtlas TMAtlas {
            get {
                if (tMAtlas is null) {
                    var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                    tMAtlas = UIUtils.CreateTextureAtlas(nameof(TMAtlas), $"{assemblyName}.UI.", SpriteParams);
                    return tMAtlas;
                } else {
                    return tMAtlas;
                }
            }
        }
    }

    internal class UIUtils {
        public static UITextureAtlas CreateTextureAtlas(string atlasName, string path, Dictionary<string, RectOffset> spriteParams, int maxSpriteSize = 1024) {
            var keys = spriteParams.Keys.ToArray();
            var value = spriteParams.Values.ToArray();
            Texture2D texture2D = new Texture2D(maxSpriteSize, maxSpriteSize, TextureFormat.ARGB32, false);
            Texture2D[] textures = new Texture2D[spriteParams.Count];
            for (int i = 0; i < spriteParams.Count; i++) {
                textures[i] = LoadTextureFromAssembly(path + keys[i] + ".png");
            }
            Rect[] regions = texture2D.PackTextures(textures, 2, maxSpriteSize);
            UITextureAtlas uITextureAtlas = ScriptableObject.CreateInstance<UITextureAtlas>();
            Material material = UnityEngine.Object.Instantiate(UIView.GetAView().defaultAtlas.material);
            material.mainTexture = texture2D;
            uITextureAtlas.material = material;
            uITextureAtlas.name = atlasName;
            for (int j = 0; j < spriteParams.Count; j++) {
                UITextureAtlas.SpriteInfo item = new UITextureAtlas.SpriteInfo() {
                    name = keys[j],
                    texture = textures[j],
                    region = regions[j],
                    border = value[j]
                };
                uITextureAtlas.AddSprite(item);
            }
            return uITextureAtlas;
        }

        public static Texture2D LoadTextureFromAssembly(string fileName) {
            try {
                Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName);
                byte[] array = new byte[s.Length];
                s.Read(array, 0, array.Length);
                return new Image(array).CreateTexture();
            }
            catch (Exception e) {
                Debug.Log($"Couldn't load texture from assembly, file name:{fileName}, detial:{e.Message}");
                return null;
            }
        }

    }
}
