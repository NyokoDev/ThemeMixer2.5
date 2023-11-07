using CitiesHarmony.API;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ColossalFramework.IO;
using ICities;
using JetBrains.Annotations;
using ThemeMixer.Locale;
using ThemeMixer.Patching;
using UnifiedUI.Helpers;
using ThemeMixer.Resources;
using ThemeMixer.Serialization;
using ThemeMixer.Themes;
using ThemeMixer.TranslationFramework;
using ThemeMixer.UI;
using UnityEngine;
using static ThemeMixer.UI.UIToggle;
using AlgernonCommons.UI;
using AlgernonCommons.Keybinding;
using ColossalFramework.Threading;
using AlgernonCommons;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ThemeMixer
{



    public class Mod : IUserMod, ILoadingExtension
    {
        private const string HarmonyID = "com.nyoko.thememixer2.5";

        public string Name => "Theme Mixer 2.5";

        public string Description => Translation.Instance.GetTranslation(TranslationID.MOD_DESCRIPTION);

        public static bool InGame => (ToolManager.instance.m_properties.m_mode == ItemClass.Availability.Game);

        public static bool ThemeDecalsEnabled => IsModEnabled(895061550UL, "Theme Decals");

        private const float Margin = 5f;
        private const float LeftMargin = 24f;
        private const float GroupMargin = 40f;

        public void Ensurance()
        {
            UIToggle toggle = UnityEngine.Object.FindObjectOfType<UIToggle>();
            UIToggle s_instance = null;

            if (toggle != null)
            {
                s_instance = toggle;
                var _hotkey = s_instance._hotkey;

            }

        }



        public void OnSettingsUI(UIHelperBase helper)
        {
            var panel = (helper.AddGroup("Theme Mixer 2.5") as UIHelper).self as UIPanel;
            float currentY = Margin;

            UILabels.AddLabel(panel, 0f, 0f, Translation.Instance.GetTranslation(TranslationID.LABEL2), -1f, 1.0f, UIHorizontalAlignment.Left);

            // Hotkey control.
            OptionsKeymapping uuiKeymapping = OptionsKeymapping.AddKeymapping(panel, LeftMargin, currentY, Translation.Instance.GetTranslation(TranslationID.HOTKEY), DataEnsurance.ToggleKey.Keybinding);

        
    
}
    







        internal UUICustomButton UUIButton => _uuiButton;
        internal static UUICustomButton _uuiButton;
        private static UltimateEyeCandyPatch UltimateEyeCandyPatch { get; set; }
        public static object Instance { get; private set; }
        public object gameObject { get; private set; }
        private UILabel catalogVersionLabel;
        private KeyCode _hotkey;
        private static UIToggle UnifiedUICall;
        public static bool UUIToggled = false; // UUI Call bool
        public static bool UUIExecuted = false;// UUI Call bool
        UIToggleClickedEventHandler EventHandler;







        [UsedImplicitly]
        public void OnEnabled()

        {

            EventHandler += HandleUIToggleClickedEvent;
            EnsureManagers();
            ManagersOnEnabled();
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
            UnityEngine.Debug.Log("Theme Mixer 2.5 has been initialized.");
            ToggleInstance();
            DataEnsurance.SaveXML();

            // Load XML data after SaveXML completes
            DataEnsurance.LoadXML();
            // Enable detailed logging.
            Logging.DetailLogging = true;

        }


        // UUI Button Start with null check
        [UsedImplicitly]
        public void UUI()
        {
            if (UIToggle.ensurance == true)
            {
                OnLoad();


            }
            else
            {
                UnityEngine.Debug.Log("[ThemeMixer2.5] Ensurance is false.");
            }
        }


        UIToggleClickedEventHandler eventHandler;



        static UIToggle ToggleInstance()
        {
            // Code to get or create an instance of UIToggle and return it
            // For example:
            return new UIToggle();
        }

        private bool _toggled = UIToggle._toggled;

        public void OnLoad()
        {
            UIToggle toggle = UnityEngine.Object.FindObjectOfType<UIToggle>();

            // Add UUI button.
            _uuiButton = UUIHelpers.RegisterCustomButton(
                name: "Theme Mixer 2.5",
                groupName: null, // default group
                tooltip: "Theme Mixer 2.5",
                icon: UUIHelpers.LoadTexture(UUIHelpers.GetFullPath<Mod>("Resources", "UUI.png")),
                onToolChanged: OnToolChanged,
                onToggle: (value) =>
                {
                    if (value)
                    {
                        EventHandler?.Invoke();
                    }
                    else
                    {
                        EventHandler?.Invoke();
                    }
                },
                hotkeys: new UUIHotKeys { ActivationKey = DataEnsurance.ToggleKey }

            );
            Debug.Log("Theme Mixer 2.5: UUI Button created.");
        }






        public void HandleUIToggleClickedEvent()
        {
            UIToggle toggle = UnityEngine.Object.FindObjectOfType<UIToggle>();
            toggle.OnClickUUI();

        }



        public void OnToolChanged(ToolBase newTool)
        {

            Debug.Log("Theme Mixer 2.5 UUI Button clicked");

        }






        /// <summary>
        /// Resets the UUI button to the non-pressed state.
        /// </summary>
        internal void ResetButton() => _uuiButton.IsPressed = false;

        [UsedImplicitly]
        public void OnDisabled()
        {
            ReleaseManagers();
            if (HarmonyHelper.IsHarmonyInstalled)
            {
                Patcher.UnpatchAll();
            }
        }

        public void OnCreated(ILoading loading) { }

        public void OnReleased() { }

        public void OnLevelLoaded(LoadMode mode)
        {
            Ensurance();
            UUI();
            ThemeSprites.CreateAtlas();
            ManagersOnLevelLoaded();


        }

        public void OnLevelUnloading()
        {
            ManagersOnLevelUnloaded();

        }

        internal static bool IsModEnabled(ulong publishedFileID, string modName)
        {
            foreach (var plugin in PluginManager.instance.GetPluginsInfo())
            {
                if (plugin.publishedFileID.AsUInt64 == publishedFileID
                    || plugin.name == modName)
                {
                    return plugin.isEnabled;
                }
            }
            return false;
        }



        private static void EnsureManagers()
        {
            SerializationService.Ensure();
            ThemeManager.Ensure();
            UIController.Ensure();
        }

        private static void ManagersOnEnabled()
        {
            SerializationService.Instance.OnEnabled();
            ThemeManager.Instance.OnEnabled();
            UIController.Instance.OnEnabled();

        }

        private static void ReleaseManagers()
        {
            UIController.Release();
            ThemeManager.Release();
            SerializationService.Release();
        }

        private static void ManagersOnLevelLoaded()
        {
            SerializationService.Instance.OnLevelLoaded();

            ThemeManager.Instance.OnLevelLoaded();
            UIController.Instance.OnLevelLoaded();
        }



        private static void ManagersOnLevelUnloaded()
        {
            ThemeManager.Instance.OnLevelUnloaded();
            UIController.Instance.OnLevelUnloaded();
        }

    }
}



