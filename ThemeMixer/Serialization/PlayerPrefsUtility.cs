using UnityEngine;

namespace TM.Utils
{
    public static class PlayerPrefsUtility
    {
        public static void SetColor(string key, Color32 value)
        {
            PlayerPrefs.SetString(key, ColorUtility.ToHtmlStringRGBA(value));
        }

        public static Color32 GetColor(string key, Color32 defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                string colorString = PlayerPrefs.GetString(key);
                Color color;
                if (ColorUtility.TryParseHtmlString(colorString, out color))
                {
                    return color;
                }
            }
            return defaultValue;
        }
    }
}
