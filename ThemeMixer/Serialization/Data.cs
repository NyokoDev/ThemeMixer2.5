using System.Collections.Generic;
using System.Xml.Serialization;
using ThemeMixer.Themes;
using ThemeMixer.Themes.Enums;
using UnityEngine;

namespace ThemeMixer.Serialization
{
    [XmlRoot("ThemeMixerSettings.xml")]
    public class Data
    {
        public string DefaultMix { get; set; }
        public ThemeMix LocalMix { get; set; }
        public bool DisableCompile { get; set; }
        public Vector2? ToolbarPosition { get; set; }
        public Vector2? UITogglePosition { get; set; }
        public List<string>[] Favourites { get; set; } = new List<string>[(int)ThemeCategory.Count];
        public List<string>[] Blacklisted { get; set; } = new List<string>[(int)ThemeCategory.Count];
        public List<SavedSwatch>[] SavedSwatches { get; set; } = new List<SavedSwatch>[(int)ColorID.Count];

        public Data()
        {
            for (var i = 0; i < (int)ThemeCategory.Count; i++)
            {
                Favourites[i] = new List<string>();
                Blacklisted[i] = new List<string>();
            }
            for (var i = 0; i < (int)ColorID.Count; i++)
            {
                SavedSwatches[i] = new List<SavedSwatch>();
            }
        }

        public void OnPreSerialize()
        {
        }

        public void OnPostDeserialize()
        {
        }
    }
}
