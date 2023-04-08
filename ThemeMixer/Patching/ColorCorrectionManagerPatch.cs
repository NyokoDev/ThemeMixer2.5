using HarmonyLib;
using JetBrains.Annotations;
using ThemeMixer.Themes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThemeMixer.Patching
{
    [UsedImplicitly]
    [HarmonyPatch(typeof(ColorCorrectionManager), nameof(ColorCorrectionManager.currentSelection), MethodType.Setter)]
    public static class ColorCorrectionManagerPatch
    {
        private static readonly OptionsGraphicsPanel Ogp = Object.FindObjectOfType<OptionsGraphicsPanel>();

        static ColorCorrectionManagerPatch()
        {
            if (Ogp == null)
            {
                Debug.Log("Failed to find OptionsGraphicsPanel");
            }
        }

        private static void Postfix()
        {
            ThemeManager.Instance.SetLut();
        }
    }
}
