using System;
using System.Diagnostics;
using System.IO;
using AlgernonCommons.Translation;
using ColossalFramework.UI;
using ICities;
using ThemeMixer;
using ThemeMixer.Locale;
using ThemeMixer.Resources;
using ThemeMixer.TranslationFramework;
using ThemeMixer.UI;
using ThemeMixer.UI.Abstraction;
using UnifiedUI;
using UnifiedUI.Helpers;
using UnityEngine;
using ThemeMixer;
using UnityEngine.UI;
using static RenderManager;
using ColossalFramework;
using System.Runtime.InteropServices;
using AlgernonCommons.Keybinding;
using AlgernonCommons.UI;
using Epic.OnlineServices;
using System.Runtime.Remoting.Messaging;

namespace TM
{
    public class SettingsUI : OptionsPanelBase
    {
        
        private const float Margin = 5f;
        private const float LeftMargin = 24f;
        private const float GroupMargin = 40f;


        public void OnSettingsUI(UIHelperBase helper)
        {
            var panel = (helper.AddGroup("Theme Mixer 2.5") as UIHelper).self as UIPanel;
            autoLayout = false;
            float currentY = Margin;

            //Set what you wanna size.
            UIDropDown languageDropDown = UIDropDowns.AddPlainDropDown(panel, LeftMargin, currentY, Translations.Translate("LANGUAGE_CHOICE"), Translations.LanguageList, Translations.Index);
            languageDropDown.eventSelectedIndexChanged += (c, index) =>
            {
                Translations.Index = index;
                OptionsPanelManager<SettingsUI>.LocaleChanged();
            };
            currentY += languageDropDown.parent.height + GroupMargin;

            // Hotkey control.
            OptionsKeymapping uuiKeymapping = OptionsKeymapping.AddKeymapping(panel, LeftMargin, currentY, Translation.Instance.GetTranslation(TranslationID.LABEL_RED), DataEnsurance.ToggleKey.Keybinding);
            currentY += uuiKeymapping.Panel.height + GroupMargin;

        }

        protected override void Setup()
        {
            
        }
    }
}






