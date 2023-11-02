using CitiesHarmony.API;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using JetBrains.Annotations;
using ThemeMixer.Locale;
using ThemeMixer.Patching;
using UnifiedUI;
using UnifiedUI.Helpers;
using ThemeMixer.Resources;
using ThemeMixer.Serialization;
using ThemeMixer.Themes;
using ThemeMixer.TranslationFramework;
using ThemeMixer.UI;
using TM;
using AlgernonCommons.Translation;
using static RenderManager;
using UnityEngine.UI;
using UnityEngine;
using static ThemeMixer.UI.UIToggle;

namespace ThemeMixer
{
    public class Mod : IUserMod, ILoadingExtension
    {
        private const string HarmonyID = "com.nyoko.thememixer2.5";

        public string Name => "Theme Mixer 2.5";

        public string Description => Translation.Instance.GetTranslation(TranslationID.MOD_DESCRIPTION);

        public static bool InGame => (ToolManager.instance.m_properties.m_mode == ItemClass.Availability.Game);

        public static bool ThemeDecalsEnabled => IsModEnabled(895061550UL, "Theme Decals");


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



        public ColossalFramework.SavedInputKey ToggleKey
        {
            get
            {
                return new ColossalFramework.SavedInputKey(Settings.gameSettingsFile, Settings.gameSettingsFile + "_options", _hotkey, false, false, false, false);
            }
        }

        internal UUICustomButton UUIButton => _uuiButton;
        internal UUICustomButton _uuiButton;
        private static UltimateEyeCandyPatch UltimateEyeCandyPatch { get; set; }
        public static object Instance { get; private set; }
        public object gameObject { get; private set; }
        private UILabel catalogVersionLabel;
        private KeyCode _hotkey;

        public void OnSettingsUI(UIHelperBase helper)
        {
            var tm2 = new TM.TM2_5();
            tm2.OnSettingsUI(helper);
        }
        [UsedImplicitly]
        public void OnEnabled()
        {
            EnsureManagers();
            ManagersOnEnabled();
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
            UnityEngine.Debug.Log("Theme Mixer 2.5 has been initialized.");
            ColorData.Load();
            ToggleInstance();
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
        static UIToggleClickedEventHandler EventUIToggleClicked;

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
                tooltip: Translations.Translate("MOD_NAME"),
                icon: UUIHelpers.LoadTexture(UUIHelpers.GetFullPath<Mod>("Resources", "UUI.png")),
                onToggle: (value) =>
                {
                    if (value)
                    {
                        EventUIToggleClicked?.Invoke();
                        _toggled = !_toggled;

                    }
                    else
                    {
                        EventUIToggleClicked?.Invoke();
                        _toggled = !_toggled;
                    }
                },
                hotkeys: new UUIHotKeys { ActivationKey = DataEnsurance.ToggleKey }
            );
        }



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



