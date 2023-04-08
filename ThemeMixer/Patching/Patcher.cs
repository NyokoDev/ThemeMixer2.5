using System.Reflection;
using HarmonyLib;
using static ThemeMixer.Mod;

namespace ThemeMixer.Patching
{
    public static class Patcher
    {
        private const string HarmonyId = "com.nyoko.thememixer2.5";

        private static bool patched = false;

        private static UltimateEyeCandyPatch UltimateEyeCandyPatch { get; set; }

        public static void PatchAll()
        {
            if (patched) return;

            UnityEngine.Debug.Log("ThemeMixer 2.5: Patching...");

            patched = true;

            Harmony harmony = new Harmony(HarmonyId);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            if (IsModEnabled(672248733UL, "UltimateEyeCandy"))
            {
                UltimateEyeCandyPatch = new UltimateEyeCandyPatch();
                UltimateEyeCandyPatch.Patch(harmony);
            }
        }

        public static void UnpatchAll()
        {
            if (!patched) return;

            Harmony harmony = new Harmony(HarmonyId);
            harmony.UnpatchAll(HarmonyId);

            patched = false;

            UnityEngine.Debug.Log("ThemeMixer 2.5: Reverted...");
        }
    }
}