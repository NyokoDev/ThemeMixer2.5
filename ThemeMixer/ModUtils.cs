using System.Reflection;
using ColossalFramework.Plugins;

namespace ThemeMixer.ModUtils
{
    public static class ModChecker
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
