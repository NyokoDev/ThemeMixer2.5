using UnityEngine;
using ThemeMixer;
using TM;
using System;
using System.Collections.Generic;

namespace ThemeMixer.ColorArray
{
    public class ColorArray
    {
        string[] colorNames = new string[]
{
    "Red", "Green", "Blue", "Yellow", "Purple", "Orange", "Pink", "Cyan", "Magenta", "Lime", "Brown", "Black", "White", "Gray", "Navy", "Teal", "Silver", "Maroon", "Olive", "Lavender", "Aqua", "Lime Green", "Turquoise", "Goldenrod", "Sky Blue", "Indigo", "Violet", "Dark Red", "Dark Green", "Dark Blue", "Dark Yellow", "Dark Purple", "Dark Orange", "Dark Pink", "Dark Cyan", "Dark Magenta", "Dark Lime", "Dark Brown", "Dark Gray", "Dark Navy", "Dark Teal", "Dark Silver", "Dark Maroon", "Dark Olive", "Dark Lavender", "Dark Aqua", "Dark Lime Green", "Dark Turquoise", "Dark Goldenrod", "Dark Sky Blue", "Dark Indigo", "Dark Violet"
};
        public static ColorInfo instance;
        public int GetColorIndex(int index)
        {
            for (int i = 0; i < instance.ColorCodes.Length; i++)
            {
                if (instance.ColorCodes[i].Equals(index))
                {
                    return i;
                }
            }

            return -1; // Return -1 if color is not found
        }

        public class ColorInfo
        {
            public string[] ColorNames { get; } = new string[]
            {
        "Red", "Green", "Blue", "Yellow", "Purple", "Orange", "Pink", "Cyan", "Magenta", "Lime", "Brown", "Black", "White", "Gray", "Navy", "Teal", "Silver", "Maroon", "Olive", "Lavender", "Aqua", "Lime Green", "Turquoise", "Goldenrod", "Sky Blue", "Indigo", "Violet", "Dark Red", "Dark Green", "Dark Blue", "Dark Yellow", "Dark Purple", "Dark Orange", "Dark Pink", "Dark Cyan", "Dark Magenta", "Dark Lime", "Dark Brown", "Dark Gray", "Dark Navy", "Dark Teal", "Dark Silver", "Dark Maroon", "Dark Olive", "Dark Lavender", "Dark Aqua", "Dark Lime Green", "Dark Turquoise", "Dark Goldenrod", "Dark Sky Blue", "Dark Indigo", "Dark Violet"
            };

            public Color32[] ColorCodes { get; } = new Color32[]
            {
        new Color32(255, 0, 0, 255), new Color32(0, 255, 0, 255), new Color32(0, 0, 255, 255), new Color32(255, 255, 0, 255), new Color32(128, 0, 128, 255),
        new Color32(255, 165, 0, 255), new Color32(255, 192, 203, 255), new Color32(0, 255, 255, 255), new Color32(255, 0, 255, 255), new Color32(0, 255, 0, 255),
        new Color32(165, 42, 42, 255), new Color32(0, 0, 0, 255), new Color32(255, 255, 255, 255), new Color32(128, 128, 128, 255), new Color32(0, 0, 128, 255),
        new Color32(0, 128, 128, 255), new Color32(192, 192, 192, 255), new Color32(128, 0, 0, 255), new Color32(128, 128, 0, 255), new Color32(128, 128, 0, 255),
        new Color32(230, 230, 250, 255), new Color32(0, 255, 255, 255), new Color32(50, 205, 50, 255), new Color32(64, 224, 208, 255), new Color32(218, 165, 32, 255),
        new Color32(135, 206, 250, 255), new Color32(75, 0, 130, 255), new Color32(238, 130, 238, 255), new Color32(148, 0, 211, 255), new Color32(139, 0, 139, 255),
        new Color32(255, 0, 0, 255), new Color32(0, 255, 0, 255), new Color32(0, 0, 255, 255), new Color32(255, 255, 0, 255), new Color32(128, 0, 128, 255),
        new Color32(255, 165, 0, 255), new Color32(255, 192, 203, 255), new Color32(0, 255, 255, 255), new Color32(255, 0, 255, 255), new Color32(0, 255, 0, 255),
        new Color32(165, 42, 42, 255), new Color32(0, 0, 0, 255), new Color32(255, 255, 255, 255), new Color32(128, 128, 128, 255), new Color32(0, 0, 128, 255),
        new Color32(0, 128, 128, 255), new Color32(192, 192, 192, 255), new Color32(128, 0, 0, 255), new Color32(128, 128, 0, 255), new Color32(128, 128, 0, 255),
        new Color32(230, 230, 250, 255), new Color32(0, 255, 255, 255), new Color32(50, 205, 50, 255), new Color32(64, 224, 208, 255), new Color32(218, 165, 32, 255),
        new Color32(135, 206, 250, 255), new Color32(75, 0, 130, 255), new Color32(238, 130, 238, 255), new Color32(148, 0, 211, 255), new Color32(139, 0, 139, 255)
            };

            public Dictionary<string, Color32> ColorDictionary { get; }

            public ColorInfo()
            {
                ColorDictionary = new Dictionary<string, Color32>();

                for (int i = 0; i < ColorNames.Length; i++)
                {
                    ColorDictionary[ColorNames[i]] = ColorCodes[i];
                }
            }
        }

    }
}


    


    