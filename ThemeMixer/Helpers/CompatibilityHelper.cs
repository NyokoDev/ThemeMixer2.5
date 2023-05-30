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
            ExceptionPanel panel = UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel");
            panel.SetMessage("Theme Mixer 2.5", "Compatibility issue: Theme Mixer 2 is not compatible with Theme Mixer 2.5. Please uninstall one version and restart the game.", false);

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