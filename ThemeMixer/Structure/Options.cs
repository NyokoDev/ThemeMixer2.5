using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using AlgernonCommons.UI;
using ColossalFramework.UI;
using UnityEngine;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using AlgernonCommons.Keybinding;
using ThemeMixer.TranslationFramework;
using ThemeMixer.Locale;
using ThemeMixer;

namespace ThemeMixer.Structure
{
    /// <summary>
    /// The mod's settings options panel.
    /// </summary>
    public sealed class OptionsPanel : OptionsPanelBase
    {
        // Layout constants.
        private const float Margin = 5f;
        private const float LeftMargin = 24f;
        private const float GroupMargin = 40f;
        private const float LabelWidth = 40f;
        private const float TabHeight = 20f;

        /// <summary>
        /// Performs on-demand panel setup.
        /// </summary>
        protected override void Setup()
        {
            autoLayout = false;
            float currentY = Margin;
            m_BackgroundSprite = "UnlockingPanel";

            UISprite image2Sprite = this.AddUIComponent<UISprite>();

            image2Sprite.height = 1000f;
            image2Sprite.relativePosition = new Vector3(0f, -50f);
            image2Sprite.width = 1000f;
            image2Sprite.atlas = UITextures.LoadSingleSpriteAtlas("..\\Resources\\bck");
            image2Sprite.spriteName = "normal";
            image2Sprite.zOrder = 1;



            UILabels.AddLabel(this, 0f, 0f, Translation.Instance.GetTranslation(TranslationID.LABEL2), -1f, 1.0f, UIHorizontalAlignment.Left);
            currentY += 35f;

            // Hotkey control.
            OptionsKeymapping uuiKeymapping = OptionsKeymapping.AddKeymapping(this, LeftMargin, currentY, Translation.Instance.GetTranslation(TranslationID.HOTKEY), DataEnsurance.ToggleKey.Keybinding);
            currentY += 35f;


            UIButton SaveButton = UIButtons.AddEvenSmallerButton(this, LeftMargin, currentY, Translation.Instance.GetTranslation(TranslationID.SAVEBUTTON_CLICK));
            currentY += 35f;
            SaveButton.eventClicked += (component, eventParam) => OnSaveButtonClicked(component, eventParam);


            void OnSaveButtonClicked(UIComponent component, UIMouseEventParameter eventParam)
            {

                DataEnsurance.SaveXML();

                DataEnsurance.LoadXML();
            }

            UIButton supportbutton = UIButtons.AddSmallerButton(this, LeftMargin, currentY, "Support");
            currentY += 35f;
            supportbutton.eventClicked += (sender, args) =>
            {
                Process.Start("https://discord.gg/gdhyhfcj7A");
            };

            UILabel version = UILabels.AddLabel(this, LabelWidth, currentY, Assembly.GetExecutingAssembly().GetName().Version.ToString(), textScale: 0.7f, alignment: UIHorizontalAlignment.Center);



        }
    }
}
