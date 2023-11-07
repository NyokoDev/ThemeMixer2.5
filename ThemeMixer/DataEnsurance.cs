using AlgernonCommons.Keybinding;
using ThemeMixer;
using System;
using System.Xml.Serialization;
using UnityEngine;
using System.IO;
using UnifiedUI.Helpers;
using AlgernonCommons.XML;
using ColossalFramework;

namespace ThemeMixer {

    [XmlRoot("ThemeMixer")]
    public class DataEnsurance : SettingsXMLBase
    {
        [XmlElement("ToggleKey")]
        public Keybinding XMLToggleKey
        {
            get => UUIKey.Keybinding;
            set => UUIKey.Keybinding = value;
        }

        /// <summary>
        /// Settings file name.
        /// </summary>
        [XmlIgnore]
        private static readonly string SettingsFileName = Path.Combine(ColossalFramework.IO.DataLocation.localApplicationData, "ThemeMixer.xml");



        [XmlIgnore]
        internal static UnsavedInputKey ToggleKey => UUIKey;
        // UUI hotkey.

        [XmlIgnore]
        private static readonly UnsavedInputKey UUIKey = new UnsavedInputKey(name: "ThemeMixer", keyCode: KeyCode.L, control: false, shift: true, alt: false);

        // <summary>
        /// Saves settings to file.
        /// </summary>
        internal static void SaveXML()
        {

            XMLFileUtils.Save<DataEnsurance>(SettingsFileName);
        }

        internal static void LoadXML()

        {

            XMLFileUtils.Load<DataEnsurance>(SettingsFileName);
        }
    }
}



