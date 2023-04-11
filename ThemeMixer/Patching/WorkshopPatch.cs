using System.IO;
using ColossalFramework.PlatformServices;
using HarmonyLib;

namespace ThemeMixer.Patching
{
    [HarmonyPatch(typeof(Workshop), "UpdateItem", typeof(PublishedFileId), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string[]))]
    public static class WorkshopUpdateItemPatch
    {
        private static void Prefix(string contentPath, ref string[] tags)
        {
            if (File.Exists(Path.Combine(contentPath, "ThemeMix.xml")))
            {
                tags = new[] { SteamHelper.kSteamTagMapTheme, "Theme Mix" };
            }
        }
    }
}
