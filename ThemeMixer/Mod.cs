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
using AlgernonCommons.Patching;
using ThemeMixer.Structure;
using static UnityEngine.UI.Toggle;
using static ThemeMixer.Structure.ToggleHandler;

namespace ThemeMixer
{

    using AlgernonCommons.Patching;
    using AlgernonCommons.Translation;
    using AlgernonCommons;
    using ICities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ThemeMixer.Structure;
    using System.Reflection;

#if DEBUG
    using ThemeMixer.UI.Abstraction.ColorPanel.ColorWheel;
#endif
    public class Mod : PatcherMod<OptionsPanel, PatcherBase>, IUserMod
    {


        public override string Name => "Theme Mixer 2.5";
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


        internal UUICustomButton UUIButton => _uuiButton;
        internal static UUICustomButton _uuiButton;
        private static UltimateEyeCandyPatch UltimateEyeCandyPatch { get; set; }

        public object gameObject { get; private set; }

        public override string HarmonyID => "com.nyoko.thememixer2.5";

        public override string BaseName => "Theme Mixer 2.5";

        private UILabel catalogVersionLabel;
        private KeyCode _hotkey;
        private static UIToggle UnifiedUICall;
        public static bool UUIToggled = false; // UUI Call bool
        public static bool UUIExecuted = false;// UUI Call bool
        UIToggleClickedEventHandler EventHandler;







        [UsedImplicitly]
        public override void OnEnabled()
        {
            base.OnEnabled();



            EventHandler += HandleUIToggleClickedEvent;
            EnsureManagers();
            ManagersOnEnabled();
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
            UnityEngine.Debug.Log("Theme Mixer 2.5 has been initialized.");
            ToggleInstance();


            // Load XML data 
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
                        active = !active;
                        HandleUIToggleClickedEvent();
                        Debug.Log("Theme Mixer 2.5: UUI button clicked.");
                    }
                    else
                    {

                        active = !active;
                        HandleUIToggleClickedEvent();
                        Debug.Log("Theme Mixer 2.5: UUI button clicked.");
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



        }






        /// <summary>
        /// Resets the UUI button to the non-pressed state.
        /// </summary>
        internal void ResetButton() => _uuiButton.IsPressed = false;

        [UsedImplicitly]
        public override void OnDisabled()
        {
            base.OnDisabled();

            ReleaseManagers();
            if (HarmonyHelper.IsHarmonyInstalled)
            {
                Patcher.UnpatchAll();
            }
        }

        public void OnCreated(ILoading loading) { }

        public void OnReleased() { }

        public void Initializer() {

            Ensurance();
            UUI();
            ThemeSprites.CreateAtlas();
            ManagersOnLevelLoaded();
            DataEnsurance.LoadXML();
            InitializeCW();

        }

        private void InitializeCW()
        {
            GameObject newGameObject = new GameObject("ColorWheelObjects");
            newGameObject.transform.position = new Vector3(0f, 0f, 0f);
        }

#if DEBUG
            ColorWheel colorWheel = newGameObject.AddComponent<ColorWheel>();
#endif



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

        public static void ManagersOnLevelLoaded()
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

        public override void LoadSettings()
    {
            DataEnsurance.LoadXML();
            // Enable detailed logging.
            Logging.DetailLogging = true;
        }

    public override void SaveSettings()
    {
            DataEnsurance.SaveXML();
    }
}
}



