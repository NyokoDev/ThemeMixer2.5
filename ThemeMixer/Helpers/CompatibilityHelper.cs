using ThemeMixer.ModUtils;

namespace ThemeMixer.Helpers
{
    public static class CompatibilityHelper
    {
        public static readonly string[] LIGHT_COLORS_MANIPULATING_MODS = { "lightingrebalance", "daylightclassic", "softershadows" };

        public static readonly string[] SKY_MANIPULATING_MODS = { "thememixer 2.5" };

        public static readonly string[] FOG_MANIPULATING_MODS = { "fogcontroller", "fogoptions", "daylightclassic" };

        public static readonly string[] REQUIRED_MODS = { "Render It!" };

        public static bool IsAnyLightColorsManipulatingModsEnabled()
        {
            if (ModChecker.IsAnyModsEnabled(LIGHT_COLORS_MANIPULATING_MODS))
            {
                return true;
            }

            return false;
        }

        public static bool IsAnySkyManipulatingModsEnabled()
        {
            if (ModChecker.IsAnyModsEnabled(SKY_MANIPULATING_MODS))
            {
                return true;
            }

            return false;
        }

        public static bool IsAnyFogManipulatingModsEnabled()
        {
            if (ModChecker.IsAnyModsEnabled(FOG_MANIPULATING_MODS))
            {
                return true;
            }

            return false;
        }

        public static bool IsRenderItEnabled()
        {
            if (ModChecker.IsModEnabled("Render It!"))
            {
                return true;
            }

            return false;
        }
    }
}
