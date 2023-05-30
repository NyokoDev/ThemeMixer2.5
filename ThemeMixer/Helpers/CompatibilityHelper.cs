using System;
using ColossalFramework.UI;
using ThemeMixer.ModUtils;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;

namespace ThemeMixer.Helpers
{
    public static class CompatibilityHelper
    {
        public static bool IsAnyLightColorsManipulatingModsEnabled()
        {
            string[] modDirectories = GetModDirectories();
            foreach (string directory in modDirectories)
            {
                if (IsAnyModDllExistsInDirectory(directory, new string[] { "lightingrebalance", "daylightclassic", "softershadows" }))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsAnySkyManipulatingModsEnabled()
        {
            string[] modDirectories = GetModDirectories();
            foreach (string directory in modDirectories)
            {
                if (IsAnyModDllExistsInDirectory(directory, new string[] { "thememixer 2" }) && IsAnyModDllExistsInDirectory(directory, new string[] { "thememixer 2.5" }))
                {
                    ShowThemeMixerCompatibilityExceptionPanel();
                    return false;
                }

                if (IsAnyModDllExistsInDirectory(directory, new string[] { "thememixer 2.5" }))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsAnyFogManipulatingModsEnabled()
        {
            string[] modDirectories = GetModDirectories();
            foreach (string directory in modDirectories)
            {
                if (IsAnyModDllExistsInDirectory(directory, new string[] { "fogcontroller", "fogoptions", "daylightclassic" }))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsRenderItEnabled()
        {
            return ModChecker.IsModEnabled("Render It!");
        }

        private static string[] GetModDirectories()
        {
            string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string modsPath = localAppDataPath + @"\Colossal Order\Cities_Skylines\Addons\Mods";
            string steamModsPath = Path.Combine(GetSteamDirectory(), "steamapps");

            return new string[] { modsPath, steamModsPath };
        }

        private static bool IsAnyModDllExistsInDirectory(string directory, string[] modNames)
        {
            if (System.IO.Directory.Exists(directory))
            {
                foreach (string modName in modNames)
                {
                    string modPath = System.IO.Path.Combine(directory, modName + ".dll");
                    if (System.IO.File.Exists(modPath))
                    {
                        return true;
                    }
                }
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


        private static string GetSteamDirectory()
        {
            string operatingSystem = Environment.OSVersion.Platform.ToString().ToLower();

            switch (operatingSystem)
            {
                case "win32nt":
                case "win32s":
                case "win32windows":
                case "win64nt":
                    return GetWindowsSteamDirectory();

                case "darwin":
                    return GetMacSteamDirectory();

                case "unix":
                    return GetLinuxSteamDirectory();

                default:
                    throw new PlatformNotSupportedException("Operating system not supported.");
            }
        }

        private static string GetWindowsSteamDirectory()
        {
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            return Path.Combine(programFiles, "Steam");
        }

        private static string GetMacSteamDirectory()
        {
            return "/Applications/Steam.app/Contents/MacOS";
        }

        private static string GetLinuxSteamDirectory()
        {
            return "~/.steam/steam";
        }
    }
}