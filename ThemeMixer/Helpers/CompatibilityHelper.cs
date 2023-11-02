using ColossalFramework.Plugins;
using System.Reflection;

namespace ThemeMixer.Helpers
{
    public static class CompatibilityHelper
    {
        public static readonly string[] LIGHT_COLORS_MANIPULATING_MODS = { "lightingrebalance", "daylightclassic", "softershadows", "renderit" };

        public static readonly string[] SKY_MANIPULATING_MODS = { "thememixer" };

        public static readonly string[] FOG_MANIPULATING_MODS = { "fogcontroller", "fogoptions", "daylightclassic" };

        public static bool IsAnyLightColorsManipulatingModsEnabled()
        {
            if (ModUtils.IsAnyModsEnabled(LIGHT_COLORS_MANIPULATING_MODS))
            {
                return true;
            }

            return false;
        }

        public static bool IsAnySkyManipulatingModsEnabled()
        {
            if (ModUtils.IsAnyModsEnabled(SKY_MANIPULATING_MODS))
            {
                return true;
            }

            return false;
        }

        public static bool IsAnyFogManipulatingModsEnabled()
        {
            if (ModUtils.IsAnyModsEnabled(FOG_MANIPULATING_MODS))
            {
                return true;
            }

            return false;
        }


        public static class ModUtils
        {
            public static bool IsAnyModsEnabled(string[] names)
            {
                foreach (string name in names)
                {
                    if (IsModEnabled(name))
                    {
                        return true;
                    }
                }

                return false;
            }

            public static bool IsModEnabled(string name)
            {
                foreach (PluginManager.PluginInfo plugin in PluginManager.instance.GetPluginsInfo())
                {
                    foreach (Assembly assembly in plugin.GetAssemblies())
                    {
                        if (assembly.GetName().Name.ToLower() == name.ToLower())
                        {
                            return plugin.isEnabled;
                        }
                    }
                }

                return false;
            }
        }
    }
}
