using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ColossalFramework.Packaging;
using ColossalFramework.PlatformServices;
using JetBrains.Annotations;
using ThemeMixer.Themes.Atmosphere;
using ThemeMixer.Themes.Structures;
using ThemeMixer.Themes.Terrain;
using ThemeMixer.Themes.Water;
using ThemeMixer.Themes.Weather;

namespace ThemeMixer.Themes
{
    public class ThemeMix : ISelectable
    {
        public string ID;
        public string Name;
        public string Lut;
        public ThemeAtmosphere Atmosphere;
        public ThemeStructures Structures;
        public ThemeTerrain Terrain;
        public ThemeWater Water;
        public ThemeWeather Weather;

        [UsedImplicitly]
        public ThemeMix()
        {
            InitializeMix();
        }

        public ThemeMix(string themeID)
        {
            InitializeMix();
            Load(themeID);
        }

        public void OnPreSerialize() { }

        public void OnPostDeserialize() { }

        public bool Load(string themeID = null)
        {
            bool success = Atmosphere.Load(themeID);
            if (!Structures.Load(themeID)) success = false;
            if (!Terrain.Load(themeID)) success = false;
            if (!Water.Load(themeID)) success = false;
            if (!Weather.Load(themeID)) success = false;
            LoadLut();
            return success;
        }

        private void LoadLut()
        {
            ThemeManager.Instance.LoadLut(Lut);
        }

        public void SetName(string name)
        {
            Name = name;
            ID = string.Concat(name.Replace(" ", ""), ID.Substring(ID.IndexOf('_')));
        }

        public bool ThemesMissing()
        {
            RefreshSubscribedThemes();

            var missing = false;

            foreach (string packageID in Atmosphere.GetPackageIDs())
            {
                if (!ThemePackageIDs.Contains(packageID)) missing = true;
            }
            foreach (string packageID in Structures.GetPackageIDs())
            {
                if (!ThemePackageIDs.Contains(packageID)) missing = true;
            }
            foreach (string packageID in Terrain.GetPackageIDs())
            {
                if (!ThemePackageIDs.Contains(packageID)) missing = true;
            }
            foreach (string packageID in Water.GetPackageIDs())
            {
                if (!ThemePackageIDs.Contains(packageID)) missing = true;
            }
            foreach (string packageID in Weather.GetPackageIDs())
            {
                if (!ThemePackageIDs.Contains(packageID)) missing = true;
            }

            return missing;
        }

        public void SubscribeMissingThemes()
        {
            RefreshSubscribedThemes();

            foreach (string packageID in Atmosphere.GetPackageIDs())
            {
                MaybeSubscribe(packageID);
            }
            foreach (string packageID in Structures.GetPackageIDs())
            {
                MaybeSubscribe(packageID);
            }
            foreach (string packageID in Terrain.GetPackageIDs())
            {
                MaybeSubscribe(packageID);
            }
            foreach (string packageID in Water.GetPackageIDs())
            {
                MaybeSubscribe(packageID);
            }
            foreach (string packageID in Weather.GetPackageIDs())
            {
                MaybeSubscribe(packageID);
            }
        }

        public bool IsSelected(string themeID)
        {
            return Atmosphere.IsSelected(themeID) &&
                   Structures.IsSelected(themeID) &&
                   Terrain.IsSelected(themeID) &&
                   Water.IsSelected(themeID) &&
                   Weather.IsSelected(themeID);
        }

        public List<string> GetUsedThemes()
        {
            ThemePackageIDs.Clear();
            ObtainUsedThemes(Atmosphere);
            ObtainUsedThemes(Structures);
            ObtainUsedThemes(Terrain);
            ObtainUsedThemes(Water);
            ObtainUsedThemes(Weather);
            return ThemePackageIDs;
        }

        private static void MaybeSubscribe(string packageID)
        {
            if (string.IsNullOrEmpty(packageID)) return;
            if (ThemePackageIDs.Contains(packageID)) return;
            if (ulong.TryParse(packageID, out ulong publishedFileID))
                PlatformService.workshop.Subscribe(new PublishedFileId(publishedFileID));
        }

        private static void RefreshSubscribedThemes()
        {
            ThemePackageIDs.Clear();
            foreach (Package.Asset mapThemeAsset in PackageManager.FilterAssets(UserAssetType.MapThemeMetaData))
            {
                if (ThemePackageIDs.Contains(mapThemeAsset.fullName)) continue;
                ThemePackageIDs.Add(mapThemeAsset.fullName);
            }
        }

        private static void ObtainUsedThemes(IPackageIDListProvider provider)
        {
            foreach (string packageID in provider.GetPackageIDs())
            {
                if (ThemePackageIDs.Contains(packageID)) continue;
                ThemePackageIDs.Add(packageID);
            }
        }

        private void InitializeMix()
        {
            ID = string.Concat(Name, "_", Guid.NewGuid().ToString("N"));
            Lut = ThemeManager.Instance.Lut;
            Terrain = new ThemeTerrain();
            Water = new ThemeWater();
            Atmosphere = new ThemeAtmosphere();
            Structures = new ThemeStructures();
            Weather = new ThemeWeather();
        }

        [XmlIgnore] private static readonly List<string> ThemePackageIDs = new List<string>();
    }
}
