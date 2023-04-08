using ColossalFramework.Importers;
using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TM {
    public class TM2_5 {

        public void OnSettingsUI(UIHelperBase helper) {
            var panel = (helper.AddGroup("Theme Mixer 2.5") as UIHelper).self as UIPanel;
            panel.atlas = TM2Atlas.TMAtlas;
            panel.backgroundSprite = TM2Atlas.OptionPanelBG;
            //Set what you wanna size.
            panel.size = new Vector2();
            var dropDown = AddDropDown(panel, "Interface Color", new string[] { "Blue", "Green", "Yellow", "Red" }, 0, (_) => {
                if (_ == 0) {

                } else if (_ == 1) {
                    // code to set interface color to green
                } else if (_ == 2) {
                    // code to set interface color to yellow
                } else {
                    // code to set interface color to red
                }
            });
            //You have to set position, and set where you want.
            dropDown.relativePosition = new Vector2(10, 10);

            var donateButton = AddButton(panel, "Donate", () => Application.OpenURL("https://www.example.com/donate"));
            donateButton.relativePosition = new Vector2(50, 80);
            var supportButton = AddButton(panel, "Support and assistance", () => Application.OpenURL("https://www.example.com/donate"));
            supportButton.relativePosition = new Vector2(50, donateButton.relativePosition.y + donateButton.size.y + 10);
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
        public static string OptionPanelBG => nameof(OptionPanelBG);
        //Your image name, also included in you project.
        static TM2Atlas() => SpriteParams[OptionPanelBG] = new RectOffset(4, 4, 4, 4);
        //Atlas what we create.
        public static UITextureAtlas TMAtlas {
            get {
                if (tMAtlas is null) {
                    var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                    tMAtlas = UIUtils.CreateTextureAtlas(nameof(TMAtlas), $"{assemblyName}.UI.Resources.", SpriteParams);
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
                textures[i] = LoadTextureFromAssembly(path + keys[i] + "BackgroundImage.png");
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
