using CitiesHarmony.API;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using JetBrains.Annotations;
using ThemeMixer.Locale;
using ThemeMixer.Patching;
using ThemeMixer.Resources;
using ThemeMixer.Serialization;
using ThemeMixer.Themes;
using ThemeMixer.TranslationFramework;
using ThemeMixer.UI;


namespace ThemeMixer
{
    public class Mod : IUserMod, ILoadingExtension
    {
        private const string HarmonyID = "com.nyoko.thememixer2.5";

        public string Name => "Theme Mixer 2.5";

        public string Description => Translation.Instance.GetTranslation(TranslationID.MOD_DESCRIPTION);

        public static bool InGame => (ToolManager.instance.m_properties.m_mode == ItemClass.Availability.Game);

        public static bool ThemeDecalsEnabled => IsModEnabled(895061550UL, "Theme Decals");


        private static UltimateEyeCandyPatch UltimateEyeCandyPatch { get; set; }
        public static object Instance { get; private set; }
        public object gameObject { get; private set; }
        private UILabel catalogVersionLabel;

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



