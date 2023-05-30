using System;
using ColossalFramework.UI;
using ThemeMixer.ModUtils;
using UnityEngine;

namespace ThemeMixer.Helpers
{
    public static class CompatibilityHelper
    {
        public static readonly string[] LIGHT_COLORS_MANIPULATING_MODS = { "lightingrebalance", "daylightclassic", "softershadows" };

        public static readonly string[] THEME_MIXER_2_MODS = { "thememixer 2" };

        public static readonly string[] THEME_MIXER_2_5_MODS = { "thememixer 2.5" };

        public static readonly string[] FOG_MANIPULATING_MODS = { "fogcontroller", "fogoptions", "daylightclassic" };

        public static readonly string[] REQUIRED_MODS = { "Render It!" };

        public static bool IsAnyLightColorsManipulatingModsEnabled()
        {
            if (ModChecker.IsAnyModsEnabled(LIGHT_COLORS_MANIPULATING_MODS))
            {
                return true;
            }

            return false;
        }

        public static bool IsAnySkyManipulatingModsEnabled()
        {
            if (ModChecker.IsAnyModsEnabled(THEME_MIXER_2_MODS) && ModChecker.IsAnyModsEnabled(THEME_MIXER_2_5_MODS))
            {
                ShowThemeMixerCompatibilityExceptionPanel();
                return false;
            }

            if (ModChecker.IsAnyModsEnabled(THEME_MIXER_2_5_MODS))
            {
                return true;
            }

            return false;
        }

        public static bool IsAnyFogManipulatingModsEnabled()
        {
            if (ModChecker.IsAnyModsEnabled(FOG_MANIPULATING_MODS))
            {
                return true;
            }

            return false;
        }

        public static bool IsRenderItEnabled()
        {
            if (ModChecker.IsModEnabled("Render It!"))
            {
                return true;
            }

            return false;
        }

        private static void ShowThemeMixerCompatibilityExceptionPanel()
        {
            // Code to show the exception panel explaining the compatibility issue
            // You can customize this method to display the panel in your game's UI
            // Customize the appearance of the ExceptionPanel
            UIPanel exceptionPanel = UIView.library.ShowModal<UIPanel>("ExceptionPanel") as UIPanel;
            exceptionPanel.SendMessage("Theme Mixer 2 and 2.5 cannot be used together.", "Please remove either Theme Mixer 2 or 2.5 to avoid compatibility issues.");

            // Example customization:
            exceptionPanel.backgroundSprite = "GenericPanel";
            exceptionPanel.color = new Color32(0, 0, 0, 200);
            exceptionPanel.width = 500f;
            exceptionPanel.height = 250f;
            exceptionPanel.relativePosition = new Vector3(500f, 300f);
            exceptionPanel.Find<UILabel>("ExceptionTitle").textColor = Color.red;
            exceptionPanel.Find<UILabel>("ExceptionMessage").textColor = Color.white;
            exceptionPanel.Find<UILabel>("ExceptionMessage").wordWrap = true;
            exceptionPanel.AttachUIComponent(exceptionPanel.Find<UILabel>("ExceptionMessage").gameObject);


        }
    }
}
