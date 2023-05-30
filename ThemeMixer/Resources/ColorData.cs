using TM.Utils;
using UnityEngine;

namespace ThemeMixer.Resources
{
    public static class ColorData
    {
        public static readonly Color32 UIColorPurple = new Color32(87, 45, 107, 255);
        public static readonly Color32 UIColorDarkBlue = new Color32(38, 70, 83, 255);
        public static readonly Color32 UIColorRed = new Color32(200, 64, 57, 255);
        public static readonly Color32 UIColorLightBlue = new Color32(52, 152, 219, 255);
        public static Color32 UIColor = new Color32(200, 200, 200, 255);

        private const string UIColorKey = "UIColor";

        static ColorData()
        {
            // Retrieve the stored UIColor value or use the default value
            UIColor = PlayerPrefsUtility.GetColor(UIColorKey, UIColor);
        }

        public static void Save()
        {
            // Save the current UIColor value
            PlayerPrefsUtility.SetColor(UIColorKey, UIColor);
            PlayerPrefs.Save();
        }
    }
}
