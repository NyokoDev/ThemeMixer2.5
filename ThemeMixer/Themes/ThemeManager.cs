using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using ColossalFramework;
using ColossalFramework.Packaging;
using ICities;
using ThemeMixer.Serialization;
using ThemeMixer.Themes.Enums;
using ThemeMixer.Themes.Terrain;
using ThemeMixer.UI;
using UnityEngine;

namespace ThemeMixer.Themes
{
    public class ThemeManager : MonoBehaviour
    {
        public event EventHandler<UIDirtyEventArgs> EventUIDirty;

        private static ThemeManager _instance;

        public static ThemeManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<ThemeManager>();
                if (_instance != null) return _instance;
                GameObject gameObject = GameObject.Find("ThemeMixer");
                if (gameObject == null) gameObject = new GameObject("ThemeMixer");
                _instance = gameObject.AddComponent<ThemeManager>();
                DontDestroyOnLoad(_instance.gameObject);
                return _instance;
            }
        }

        internal MapThemeMetaData GetTheme(string themeID)
        {
            if (themeID == null) return null;
            return Themes.TryGetValue(themeID, out MapThemeMetaData theme) ? theme : null;
        }

        public static ThemeManager Ensure() => Instance;

        private static bool InGame => ToolManager.instance?.m_properties != null &&
                                      (ToolManager.instance.m_properties?.m_mode & ItemClass.Availability.GameAndMap) != 0;

        private Dictionary<string, MapThemeMetaData> _themes;
        public Dictionary<string, MapThemeMetaData> Themes => _themes ?? CacheThemes();

        private const string DataID = "THEME-MIXER-2.5";
        public ThemeMix CurrentMix { get; set; }

        private string MixID { get; set; }

        private FieldInfo _lutField;
        private FieldInfo LutField => _lutField ?? (_lutField = typeof(ColorCorrectionManager).GetField("m_SavedAsset", BindingFlags.NonPublic | BindingFlags.Instance));

        public string Lut
        {
            get
            {
                string userLut = ((SavedString)LutField.GetValue(ColorCorrectionManager.instance))?.value;
                return !string.IsNullOrEmpty(userLut) ? userLut : BuiltInLut == 0 ? null : BuiltInLuts[BuiltInLut - 1].name;
            }
        }

        private FieldInfo _builtInLutField;
        private FieldInfo BuiltInLutField => _builtInLutField ?? (_builtInLutField = typeof(ColorCorrectionManager).GetField("m_SavedBuiltin", BindingFlags.NonPublic | BindingFlags.Instance));

        public int BuiltInLut => ((SavedInt)BuiltInLutField.GetValue(ColorCorrectionManager.instance)).value;

        private FieldInfo _userLutsField;
        private FieldInfo UserLutsField => _userLutsField ?? (_userLutsField = typeof(ColorCorrectionManager).GetField("m_UserAssets", BindingFlags.NonPublic | BindingFlags.Instance));

        public List<Package.Asset> UserLuts => (List<Package.Asset>)UserLutsField.GetValue(ColorCorrectionManager.instance);

        public Texture3DWrapper[] BuiltInLuts => ColorCorrectionManager.instance.m_BuiltinLUTs;

        internal void OnSaveData(ISerializableData serializableDataManager)
        {
            if (MixID == null) return;
            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    var binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(memoryStream, MixID);
                    serializableDataManager.SaveData(DataID, memoryStream.ToArray());
                }
                catch (Exception exception)
                {
                    Debug.Log(exception);
                }
            }
        }

        internal void OnLoadData(ISerializableData serializableDataManager)
        {
            var data = serializableDataManager.LoadData(DataID);
            if (data == null || data.Length == 0) return;

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(data))
            {
                try
                {
                    MixID = binaryFormatter.Deserialize(memoryStream) as string;
                }
                catch (Exception exception)
                {
                    Debug.Log(exception);
                }
            }

            if (MixID == null) return;
            ThemeMix mix = SerializationService.Instance.GetMix(MixID);
            if (mix != null && !mix.ThemesMissing() && mix.Load()) CurrentMix = mix;
        }

        internal Color GetCurrentColor(ColorID colorID)
        {
            switch (colorID)
            {
                case ColorID.MoonInnerCorona: return (Color)(CurrentMix.Atmosphere.MoonInnerCorona.CustomValue ?? CurrentMix.Atmosphere.MoonInnerCorona.Value);
                case ColorID.MoonOuterCorona: return (Color)(CurrentMix.Atmosphere.MoonOuterCorona.CustomValue ?? CurrentMix.Atmosphere.MoonOuterCorona.Value);
                case ColorID.SkyTint: return (Color)(CurrentMix.Atmosphere.SkyTint.CustomValue ?? CurrentMix.Atmosphere.SkyTint.Value);
                case ColorID.NightHorizonColor: return (Color)(CurrentMix.Atmosphere.NightHorizonColor.CustomValue ?? CurrentMix.Atmosphere.NightHorizonColor.Value);
                case ColorID.EarlyNightZenithColor: return (Color)(CurrentMix.Atmosphere.EarlyNightZenithColor.CustomValue ?? CurrentMix.Atmosphere.EarlyNightZenithColor.Value);
                case ColorID.LateNightZenithColor: return (Color)(CurrentMix.Atmosphere.LateNightZenithColor.CustomValue ?? CurrentMix.Atmosphere.LateNightZenithColor.Value);
                case ColorID.WaterClean: return (Color)(CurrentMix.Water.WaterClean.CustomValue ?? CurrentMix.Water.WaterClean.Value);
                case ColorID.WaterDirty: return (Color)(CurrentMix.Water.WaterDirty.CustomValue ?? CurrentMix.Water.WaterDirty.Value);
                case ColorID.WaterUnder: return (Color)(CurrentMix.Water.WaterUnder.CustomValue ?? CurrentMix.Water.WaterUnder.Value);
                default: return default;
            }
        }

        private Dictionary<string, MapThemeMetaData> CacheThemes()
        {
            if (_themes == null) _themes = new Dictionary<string, MapThemeMetaData>();
            _themes.Clear();
            foreach (Package.Asset asset in PackageManager.FilterAssets(UserAssetType.MapThemeMetaData))
            {
                if (asset == null || asset.package == null) continue;
                if (asset.fullName.Contains("CO-Winter-Theme") && !SteamHelper.IsDLCOwned(SteamHelper.DLC.SnowFallDLC)) continue;
                _themes[asset.fullName] = asset.Instantiate<MapThemeMetaData>();
                _themes[asset.fullName].assetRef = asset;

            }
            return _themes;
        }

        private void SaveLocalMix()
        {
            CurrentMix.Lut = Lut;
            SerializationService.Instance.SaveLocalMix(CurrentMix);
        }

        public void OnEnabled()
        {
            if (!InGame) return;
            OnLevelLoaded();
        }

        public void OnLevelLoaded()
        {
            CacheThemes();
            if (CurrentMix != null) return;
            CurrentMix = SerializationService.Instance.GetDefaultMix() ?? SerializationService.Instance.GetSavedLocalMix();
            if (CurrentMix == null)
            {
                if (SimulationManager.instance.m_metaData.m_MapThemeMetaData != null)
                    CurrentMix = new ThemeMix(SimulationManager.instance.m_metaData.m_MapThemeMetaData.mapThemeRef);
                else
                {
                    switch (SimulationManager.instance.m_metaData.m_environment)
                    {
                        case "Sunny":
                            CurrentMix = new ThemeMix("1899640536.CO-Temperate-Theme");
                            break;
                        case "Europe":
                            CurrentMix = new ThemeMix("1899640536.CO-European-Theme");
                            break;
                        case "Winter":
                            CurrentMix = new ThemeMix("1899640536.CO-Winter-Theme");
                            break;
                        case "North":
                            CurrentMix = new ThemeMix("1899640536.CO-Boreal-Theme");
                            break;
                        case "Tropical":
                            CurrentMix = new ThemeMix("1899640536.CO-Tropical-Theme");
                            break;
                    }
                }
                SaveLocalMix();
            }
            else CurrentMix.Load();
        }

        internal void SaveMix(string saveName)
        {
            CurrentMix.SetName(saveName);
            MixID = CurrentMix.ID;
            SaveLocalMix();
            SerializationService.Instance.SaveMix(CurrentMix);
        }

        internal void OnLevelUnloaded()
        {
        }

        public static void Release()
        {
            if (_instance == null) return;
            Destroy(_instance.gameObject);
            _instance = null;
        }

        public void LoadCategory(ThemeCategory category, string themeID)
        {
            switch (category)
            {
                case ThemeCategory.Themes: CurrentMix = new ThemeMix(themeID); break;
                case ThemeCategory.Terrain: CurrentMix.Terrain.Load(themeID); break;
                case ThemeCategory.Water: CurrentMix.Water.Load(themeID); break;
                case ThemeCategory.Structures: CurrentMix.Structures.Load(themeID); break;
                case ThemeCategory.Atmosphere: CurrentMix.Atmosphere.Load(themeID); break;
                case ThemeCategory.Weather: CurrentMix.Weather.Load(themeID); break;
            }
            SaveLocalMix();
            EventUIDirty?.Invoke(this, new UIDirtyEventArgs(CurrentMix));
        }

        internal void LoadMix(ThemeMix mix)
        {
            CurrentMix = mix;
            MixID = mix.ID;
            CurrentMix.Load();
        }

        public void LoadTexture(TextureID textureID, string themeID)
        {
            switch (textureID)
            {
                case TextureID.GrassDiffuseTexture: CurrentMix.Terrain.GrassDiffuseTexture.Load(themeID); break;
                case TextureID.RuinedDiffuseTexture: CurrentMix.Terrain.RuinedDiffuseTexture.Load(themeID); break;
                case TextureID.PavementDiffuseTexture: CurrentMix.Terrain.PavementDiffuseTexture.Load(themeID); break;
                case TextureID.GravelDiffuseTexture: CurrentMix.Terrain.GravelDiffuseTexture.Load(themeID); break;
                case TextureID.CliffDiffuseTexture: CurrentMix.Terrain.CliffDiffuseTexture.Load(themeID); break;
                case TextureID.SandDiffuseTexture: CurrentMix.Terrain.SandDiffuseTexture.Load(themeID); break;
                case TextureID.OilDiffuseTexture: CurrentMix.Terrain.OilDiffuseTexture.Load(themeID); break;
                case TextureID.OreDiffuseTexture: CurrentMix.Terrain.OreDiffuseTexture.Load(themeID); break;
                case TextureID.CliffSandNormalTexture: CurrentMix.Terrain.CliffSandNormalTexture.Load(themeID); break;
                case TextureID.UpwardRoadDiffuse: CurrentMix.Structures.UpwardRoadDiffuse.Load(themeID); break;
                case TextureID.DownwardRoadDiffuse: CurrentMix.Structures.DownwardRoadDiffuse.Load(themeID); break;
                case TextureID.BuildingFloorDiffuse: CurrentMix.Structures.BuildingFloorDiffuse.Load(themeID); break;
                case TextureID.BuildingBaseDiffuse: CurrentMix.Structures.BuildingBaseDiffuse.Load(themeID); break;
                case TextureID.BuildingBaseNormal: CurrentMix.Structures.BuildingBaseNormal.Load(themeID); break;
                case TextureID.BuildingBurntDiffuse: CurrentMix.Structures.BuildingBurntDiffuse.Load(themeID); break;
                case TextureID.BuildingAbandonedDiffuse: CurrentMix.Structures.BuildingAbandonedDiffuse.Load(themeID); break;
                case TextureID.LightColorPalette: CurrentMix.Structures.LightColorPalette.Load(themeID); break;
                case TextureID.MoonTexture: CurrentMix.Atmosphere.MoonTexture.Load(themeID); break;
                case TextureID.WaterFoam: CurrentMix.Water.WaterFoam.Load(themeID); break;
                case TextureID.WaterNormal: CurrentMix.Water.WaterNormal.Load(themeID); break;
            }
            SaveLocalMix();
            EventUIDirty?.Invoke(this, new UIDirtyEventArgs(CurrentMix));
        }

        internal Color GetColor(ColorID colorID, string themeID)
        {
            switch (colorID)
            {
                case ColorID.MoonInnerCorona: return Themes[themeID].moonInnerCorona;
                case ColorID.MoonOuterCorona: return Themes[themeID].moonOuterCorona;
                case ColorID.SkyTint: return Themes[themeID].skyTint;
                case ColorID.NightHorizonColor: return Themes[themeID].nightHorizonColor;
                case ColorID.EarlyNightZenithColor: return Themes[themeID].earlyNightZenithColor;
                case ColorID.LateNightZenithColor: return Themes[themeID].lateNightZenithColor;
                case ColorID.WaterClean: return Themes[themeID].waterClean;
                case ColorID.WaterDirty: return Themes[themeID].waterDirty;
                case ColorID.WaterUnder: return Themes[themeID].waterUnder;
                default: return default;
            }
        }

        internal void OnColorChanged(ColorID colorID, Color value)
        {
            switch (colorID)
            {
                case ColorID.MoonInnerCorona: CurrentMix.Atmosphere.MoonInnerCorona.SetCustomValue(value); break;
                case ColorID.MoonOuterCorona: CurrentMix.Atmosphere.MoonOuterCorona.SetCustomValue(value); break;
                case ColorID.SkyTint: CurrentMix.Atmosphere.SkyTint.SetCustomValue(value); break;
                case ColorID.NightHorizonColor: CurrentMix.Atmosphere.NightHorizonColor.SetCustomValue(value); break;
                case ColorID.EarlyNightZenithColor: CurrentMix.Atmosphere.EarlyNightZenithColor.SetCustomValue(value); break;
                case ColorID.LateNightZenithColor: CurrentMix.Atmosphere.LateNightZenithColor.SetCustomValue(value); break;
                case ColorID.WaterClean: CurrentMix.Water.WaterClean.SetCustomValue(value); break;
                case ColorID.WaterDirty: CurrentMix.Water.WaterDirty.SetCustomValue(value); break;
                case ColorID.WaterUnder: CurrentMix.Water.WaterUnder.SetCustomValue(value); break;
            }
            SaveLocalMix();
        }

        internal void OnTilingChanged(TextureID textureID, float value)
        {
            switch (textureID)
            {
                case TextureID.GrassDiffuseTexture: CurrentMix.Terrain.GrassDiffuseTexture.SetCustomValue(value); break;
                case TextureID.RuinedDiffuseTexture: CurrentMix.Terrain.RuinedDiffuseTexture.SetCustomValue(value); break;
                case TextureID.PavementDiffuseTexture: CurrentMix.Terrain.PavementDiffuseTexture.SetCustomValue(value); break;
                case TextureID.GravelDiffuseTexture: CurrentMix.Terrain.GravelDiffuseTexture.SetCustomValue(value); break;
                case TextureID.CliffDiffuseTexture: CurrentMix.Terrain.CliffDiffuseTexture.SetCustomValue(value); break;
                case TextureID.SandDiffuseTexture: CurrentMix.Terrain.SandDiffuseTexture.SetCustomValue(value); break;
                case TextureID.OilDiffuseTexture: CurrentMix.Terrain.OilDiffuseTexture.SetCustomValue(value); break;
                case TextureID.OreDiffuseTexture: CurrentMix.Terrain.OreDiffuseTexture.SetCustomValue(value); break;
                case TextureID.CliffSandNormalTexture: CurrentMix.Terrain.CliffSandNormalTexture.SetCustomValue(value); break;
            }
            SaveLocalMix();
        }

        internal void OnValueChanged<T>(ValueID valueID, T value)
        {
            switch (valueID)
            {
                case ValueID.Longitude: CurrentMix.Atmosphere.Longitude.SetCustomValue(value); break;
                case ValueID.Latitude: CurrentMix.Atmosphere.Latitude.SetCustomValue(value); break;
                case ValueID.SunSize: CurrentMix.Atmosphere.SunSize.SetCustomValue(value); break;
                case ValueID.SunAnisotropy: CurrentMix.Atmosphere.SunAnisotropy.SetCustomValue(value); break;
                case ValueID.MoonSize: CurrentMix.Atmosphere.MoonSize.SetCustomValue(value); break;
                case ValueID.Rayleigh: CurrentMix.Atmosphere.Rayleigh.SetCustomValue(value); break;
                case ValueID.Mie: CurrentMix.Atmosphere.Mie.SetCustomValue(value); break;
                case ValueID.Exposure: CurrentMix.Atmosphere.Exposure.SetCustomValue(value); break;
                case ValueID.StarsIntensity: CurrentMix.Atmosphere.StarsIntensity.SetCustomValue(value); break;
                case ValueID.OuterSpaceIntensity: CurrentMix.Atmosphere.OuterSpaceIntensity.SetCustomValue(value); break;
                case ValueID.GrassDetailEnabled: CurrentMix.Terrain.GrassDetailEnabled.SetCustomValue(value); break;
                case ValueID.FertileDetailEnabled: CurrentMix.Terrain.FertileDetailEnabled.SetCustomValue(value); break;
                case ValueID.RocksDetailEnabled: CurrentMix.Terrain.RocksDetailEnabled.SetCustomValue(value); break;
                case ValueID.MinTemperatureDay: CurrentMix.Weather.MinTemperatureDay.SetCustomValue(value); break;
                case ValueID.MaxTemperatureDay: CurrentMix.Weather.MaxTemperatureDay.SetCustomValue(value); break;
                case ValueID.MinTemperatureNight: CurrentMix.Weather.MinTemperatureNight.SetCustomValue(value); break;
                case ValueID.MaxTemperatureNight: CurrentMix.Weather.MaxTemperatureNight.SetCustomValue(value); break;
                case ValueID.MinTemperatureRain: CurrentMix.Weather.MinTemperatureRain.SetCustomValue(value); break;
                case ValueID.MaxTemperatureRain: CurrentMix.Weather.MaxTemperatureRain.SetCustomValue(value); break;
                case ValueID.MinTemperatureFog: CurrentMix.Weather.MinTemperatureFog.SetCustomValue(value); break;
                case ValueID.MaxTemperatureFog: CurrentMix.Weather.MaxTemperatureFog.SetCustomValue(value); break;
                case ValueID.RainProbabilityDay: CurrentMix.Weather.RainProbabilityDay.SetCustomValue(value); break;
                case ValueID.RainProbabilityNight: CurrentMix.Weather.RainProbabilityNight.SetCustomValue(value); break;
                case ValueID.FogProbabilityDay: CurrentMix.Weather.FogProbabilityDay.SetCustomValue(value); break;
                case ValueID.FogProbabilityNight: CurrentMix.Weather.FogProbabilityNight.SetCustomValue(value); break;
                case ValueID.NorthernLightsProbability: CurrentMix.Weather.NorthernLightsProbability.SetCustomValue(value); break;
            }
            SaveLocalMix();
        }

        internal void OnOffsetChanged(OffsetID offsetID, Vector3 value)
        {
            switch (offsetID)
            {
                case OffsetID.GrassPollutionColorOffset: CurrentMix.Terrain.GrassPollutionColorOffset.SetCustomValue(value); break;
                case OffsetID.GrassFieldColorOffset: CurrentMix.Terrain.GrassFieldColorOffset.SetCustomValue(value); break;
                case OffsetID.GrassFertilityColorOffset: CurrentMix.Terrain.GrassFertilityColorOffset.SetCustomValue(value); break;
                case OffsetID.GrassForestColorOffset: CurrentMix.Terrain.GrassForestColorOffset.SetCustomValue(value); break;
            }
            SaveLocalMix();
        }

        public void LoadColor(ColorID colorID, string themeID)
        {
            switch (colorID)
            {
                case ColorID.MoonInnerCorona: CurrentMix.Atmosphere.MoonInnerCorona.Load(themeID); break;
                case ColorID.MoonOuterCorona: CurrentMix.Atmosphere.MoonOuterCorona.Load(themeID); break;
                case ColorID.SkyTint: CurrentMix.Atmosphere.SkyTint.Load(themeID); break;
                case ColorID.NightHorizonColor: CurrentMix.Atmosphere.NightHorizonColor.Load(themeID); break;
                case ColorID.EarlyNightZenithColor: CurrentMix.Atmosphere.EarlyNightZenithColor.Load(themeID); break;
                case ColorID.LateNightZenithColor: CurrentMix.Atmosphere.LateNightZenithColor.Load(themeID); break;
                case ColorID.WaterClean: CurrentMix.Water.WaterClean.Load(themeID); break;
                case ColorID.WaterDirty: CurrentMix.Water.WaterDirty.Load(themeID); break;
                case ColorID.WaterUnder: CurrentMix.Water.WaterUnder.Load(themeID); break;
            }
            SaveLocalMix();
            EventUIDirty?.Invoke(this, new UIDirtyEventArgs(CurrentMix));
        }

        public void OnThemeDirty(ThemeDirtyEventArgs e)
        {
            Debug.Log("Theme Mixer 2.5: ThemeDirtyEventArgs: This message is harmless.");
        }

        public void LoadOffset(OffsetID offsetID, string themeID)
        {
            switch (offsetID)
            {
                case OffsetID.GrassPollutionColorOffset: CurrentMix.Terrain.GrassPollutionColorOffset.Load(themeID); break;
                case OffsetID.GrassFieldColorOffset: CurrentMix.Terrain.GrassFieldColorOffset.Load(themeID); break;
                case OffsetID.GrassFertilityColorOffset: CurrentMix.Terrain.GrassFertilityColorOffset.Load(themeID); break;
                case OffsetID.GrassForestColorOffset: CurrentMix.Terrain.GrassForestColorOffset.Load(themeID); break;
            }
            SaveLocalMix();
            EventUIDirty?.Invoke(this, new UIDirtyEventArgs(CurrentMix));
        }

        public void LoadValue(ValueID valueID, string themeID)
        {
            switch (valueID)
            {
                case ValueID.Longitude: CurrentMix.Atmosphere.Longitude.Load(themeID); break;
                case ValueID.Latitude: CurrentMix.Atmosphere.Latitude.Load(themeID); break;
                case ValueID.SunSize: CurrentMix.Atmosphere.SunSize.Load(themeID); break;
                case ValueID.SunAnisotropy: CurrentMix.Atmosphere.SunAnisotropy.Load(themeID); break;
                case ValueID.MoonSize: CurrentMix.Atmosphere.MoonSize.Load(themeID); break;
                case ValueID.Rayleigh: CurrentMix.Atmosphere.Rayleigh.Load(themeID); break;
                case ValueID.Mie: CurrentMix.Atmosphere.Mie.Load(themeID); break;
                case ValueID.Exposure: CurrentMix.Atmosphere.Exposure.Load(themeID); break;
                case ValueID.StarsIntensity: CurrentMix.Atmosphere.StarsIntensity.Load(themeID); break;
                case ValueID.OuterSpaceIntensity: CurrentMix.Atmosphere.OuterSpaceIntensity.Load(themeID); break;
                case ValueID.GrassDetailEnabled: CurrentMix.Terrain.GrassDetailEnabled.Load(themeID); break;
                case ValueID.FertileDetailEnabled: CurrentMix.Terrain.FertileDetailEnabled.Load(themeID); break;
                case ValueID.RocksDetailEnabled: CurrentMix.Terrain.RocksDetailEnabled.Load(themeID); break;
                case ValueID.MinTemperatureDay: CurrentMix.Weather.MinTemperatureDay.Load(themeID); break;
                case ValueID.MaxTemperatureDay: CurrentMix.Weather.MaxTemperatureDay.Load(themeID); break;
                case ValueID.MinTemperatureNight: CurrentMix.Weather.MinTemperatureNight.Load(themeID); break;
                case ValueID.MaxTemperatureNight: CurrentMix.Weather.MaxTemperatureNight.Load(themeID); break;
                case ValueID.MinTemperatureRain: CurrentMix.Weather.MinTemperatureRain.Load(themeID); break;
                case ValueID.MaxTemperatureRain: CurrentMix.Weather.MaxTemperatureRain.Load(themeID); break;
                case ValueID.MinTemperatureFog: CurrentMix.Weather.MinTemperatureFog.Load(themeID); break;
                case ValueID.MaxTemperatureFog: CurrentMix.Weather.MaxTemperatureFog.Load(themeID); break;
                case ValueID.RainProbabilityDay: CurrentMix.Weather.RainProbabilityDay.Load(themeID); break;
                case ValueID.RainProbabilityNight: CurrentMix.Weather.RainProbabilityNight.Load(themeID); break;
                case ValueID.FogProbabilityDay: CurrentMix.Weather.FogProbabilityDay.Load(themeID); break;
                case ValueID.FogProbabilityNight: CurrentMix.Weather.FogProbabilityNight.Load(themeID); break;
                case ValueID.NorthernLightsProbability: CurrentMix.Weather.NorthernLightsProbability.Load(themeID); break;
            }
            SaveLocalMix();
            EventUIDirty?.Invoke(this, new UIDirtyEventArgs(CurrentMix));
        }

        public float GetTilingValue(TextureID textureID)
        {
            switch (textureID)
            {
                case TextureID.GrassDiffuseTexture: return (float)(CurrentMix.Terrain.GrassDiffuseTexture.CustomValue ?? CurrentMix.Terrain.GrassDiffuseTexture.Value);
                case TextureID.RuinedDiffuseTexture: return (float)(CurrentMix.Terrain.RuinedDiffuseTexture.CustomValue ?? CurrentMix.Terrain.RuinedDiffuseTexture.Value);
                case TextureID.PavementDiffuseTexture: return (float)(CurrentMix.Terrain.PavementDiffuseTexture.CustomValue ?? CurrentMix.Terrain.PavementDiffuseTexture.Value);
                case TextureID.GravelDiffuseTexture: return (float)(CurrentMix.Terrain.GravelDiffuseTexture.CustomValue ?? CurrentMix.Terrain.GravelDiffuseTexture.Value);
                case TextureID.CliffDiffuseTexture: return (float)(CurrentMix.Terrain.CliffDiffuseTexture.CustomValue ?? CurrentMix.Terrain.CliffDiffuseTexture.Value);
                case TextureID.SandDiffuseTexture: return (float)(CurrentMix.Terrain.SandDiffuseTexture.CustomValue ?? CurrentMix.Terrain.SandDiffuseTexture.Value);
                case TextureID.OilDiffuseTexture: return (float)(CurrentMix.Terrain.OilDiffuseTexture.CustomValue ?? CurrentMix.Terrain.OilDiffuseTexture.Value);
                case TextureID.OreDiffuseTexture: return (float)(CurrentMix.Terrain.OreDiffuseTexture.CustomValue ?? CurrentMix.Terrain.OreDiffuseTexture.Value);
                case TextureID.CliffSandNormalTexture: return (float)(CurrentMix.Terrain.CliffSandNormalTexture.CustomValue ?? CurrentMix.Terrain.CliffSandNormalTexture.Value);
                default: return 0.5f;
            }
        }

        public Vector3 GetOffsetValue(OffsetID offsetID)
        {
            switch (offsetID)
            {
                case OffsetID.GrassPollutionColorOffset: return (Vector3)(CurrentMix.Terrain.GrassPollutionColorOffset.CustomValue ?? CurrentMix.Terrain.GrassPollutionColorOffset.Value);
                case OffsetID.GrassFieldColorOffset: return (Vector3)(CurrentMix.Terrain.GrassFieldColorOffset.CustomValue ?? CurrentMix.Terrain.GrassFieldColorOffset.Value);
                case OffsetID.GrassFertilityColorOffset: return (Vector3)(CurrentMix.Terrain.GrassFertilityColorOffset.CustomValue ?? CurrentMix.Terrain.GrassFertilityColorOffset.Value);
                case OffsetID.GrassForestColorOffset: return (Vector3)(CurrentMix.Terrain.GrassForestColorOffset.CustomValue ?? CurrentMix.Terrain.GrassForestColorOffset.Value);
                default: return Vector3.zero;
            }
        }

        public T GetValue<T>(ValueID valueID)
        {
            switch (valueID)
            {
                case ValueID.Longitude: return (T)(CurrentMix.Atmosphere.Longitude.CustomValue ?? CurrentMix.Atmosphere.Longitude.Value);
                case ValueID.Latitude: return (T)(CurrentMix.Atmosphere.Latitude.CustomValue ?? CurrentMix.Atmosphere.Latitude.Value);
                case ValueID.SunSize: return (T)(CurrentMix.Atmosphere.SunSize.CustomValue ?? CurrentMix.Atmosphere.SunSize.Value);
                case ValueID.SunAnisotropy: return (T)(CurrentMix.Atmosphere.SunAnisotropy.CustomValue ?? CurrentMix.Atmosphere.SunAnisotropy.Value);
                case ValueID.MoonSize: return (T)(CurrentMix.Atmosphere.MoonSize.CustomValue ?? CurrentMix.Atmosphere.MoonSize.Value);
                case ValueID.Rayleigh: return (T)(CurrentMix.Atmosphere.Rayleigh.CustomValue ?? CurrentMix.Atmosphere.Rayleigh.Value);
                case ValueID.Mie: return (T)(CurrentMix.Atmosphere.Mie.CustomValue ?? CurrentMix.Atmosphere.Mie.Value);
                case ValueID.Exposure: return (T)(CurrentMix.Atmosphere.Exposure.CustomValue ?? CurrentMix.Atmosphere.Exposure.Value);
                case ValueID.StarsIntensity: return (T)(CurrentMix.Atmosphere.StarsIntensity.CustomValue ?? CurrentMix.Atmosphere.StarsIntensity.Value);
                case ValueID.OuterSpaceIntensity: return (T)(CurrentMix.Atmosphere.OuterSpaceIntensity.CustomValue ?? CurrentMix.Atmosphere.OuterSpaceIntensity.Value);
                case ValueID.GrassDetailEnabled: return (T)(CurrentMix.Terrain.GrassDetailEnabled.CustomValue ?? CurrentMix.Terrain.GrassDetailEnabled.Value);
                case ValueID.FertileDetailEnabled: return (T)(CurrentMix.Terrain.FertileDetailEnabled.CustomValue ?? CurrentMix.Terrain.FertileDetailEnabled.Value);
                case ValueID.RocksDetailEnabled: return (T)(CurrentMix.Terrain.RocksDetailEnabled.CustomValue ?? CurrentMix.Terrain.RocksDetailEnabled.Value);
                case ValueID.MinTemperatureDay: return (T)(CurrentMix.Weather.MinTemperatureDay.CustomValue ?? CurrentMix.Weather.MinTemperatureDay.Value);
                case ValueID.MaxTemperatureDay: return (T)(CurrentMix.Weather.MaxTemperatureDay.CustomValue ?? CurrentMix.Weather.MaxTemperatureDay.Value);
                case ValueID.MinTemperatureNight: return (T)(CurrentMix.Weather.MinTemperatureNight.CustomValue ?? CurrentMix.Weather.MinTemperatureNight.Value);
                case ValueID.MaxTemperatureNight: return (T)(CurrentMix.Weather.MaxTemperatureNight.CustomValue ?? CurrentMix.Weather.MaxTemperatureNight.Value);
                case ValueID.MinTemperatureRain: return (T)(CurrentMix.Weather.MinTemperatureRain.CustomValue ?? CurrentMix.Weather.MinTemperatureRain.Value);
                case ValueID.MaxTemperatureRain: return (T)(CurrentMix.Weather.MaxTemperatureRain.CustomValue ?? CurrentMix.Weather.MaxTemperatureRain.Value);
                case ValueID.MinTemperatureFog: return (T)(CurrentMix.Weather.MinTemperatureFog.CustomValue ?? CurrentMix.Weather.MinTemperatureFog.Value);
                case ValueID.MaxTemperatureFog: return (T)(CurrentMix.Weather.MaxTemperatureFog.CustomValue ?? CurrentMix.Weather.MaxTemperatureFog.Value);
                case ValueID.RainProbabilityDay: return (T)(CurrentMix.Weather.RainProbabilityDay.CustomValue ?? CurrentMix.Weather.RainProbabilityDay.Value);
                case ValueID.RainProbabilityNight: return (T)(CurrentMix.Weather.RainProbabilityNight.CustomValue ?? CurrentMix.Weather.RainProbabilityNight.Value);
                case ValueID.FogProbabilityDay: return (T)(CurrentMix.Weather.FogProbabilityDay.CustomValue ?? CurrentMix.Weather.FogProbabilityDay.Value);
                case ValueID.FogProbabilityNight: return (T)(CurrentMix.Weather.FogProbabilityNight.CustomValue ?? CurrentMix.Weather.FogProbabilityNight.Value);
                case ValueID.NorthernLightsProbability: return (T)(CurrentMix.Weather.NorthernLightsProbability.CustomValue ?? CurrentMix.Weather.NorthernLightsProbability.Value);
                default: return default;
            }
        }

        public bool IsSelected(string themeID, ThemeCategory category)
        {
            switch (category)
            {
                case ThemeCategory.Themes: return CurrentMix.IsSelected(themeID);
                case ThemeCategory.Atmosphere: return CurrentMix.Atmosphere.IsSelected(themeID);
                case ThemeCategory.Structures: return CurrentMix.Structures.IsSelected(themeID);
                case ThemeCategory.Terrain: return CurrentMix.Terrain.IsSelected(themeID);
                case ThemeCategory.Water: return CurrentMix.Water.IsSelected(themeID);
                case ThemeCategory.Weather: return CurrentMix.Weather.IsSelected(themeID);
                default: return false;
            }
        }

        public bool IsSelected(string themeID, ColorID colorID)
        {
            switch (colorID)
            {
                case ColorID.EarlyNightZenithColor: return CurrentMix.Atmosphere.EarlyNightZenithColor.IsSelected(themeID);
                case ColorID.LateNightZenithColor: return CurrentMix.Atmosphere.LateNightZenithColor.IsSelected(themeID);
                case ColorID.MoonInnerCorona: return CurrentMix.Atmosphere.MoonInnerCorona.IsSelected(themeID);
                case ColorID.MoonOuterCorona: return CurrentMix.Atmosphere.MoonOuterCorona.IsSelected(themeID);
                case ColorID.NightHorizonColor: return CurrentMix.Atmosphere.NightHorizonColor.IsSelected(themeID);
                case ColorID.SkyTint: return CurrentMix.Atmosphere.SkyTint.IsSelected(themeID);
                case ColorID.WaterClean: return CurrentMix.Water.WaterClean.IsSelected(themeID);
                case ColorID.WaterDirty: return CurrentMix.Water.WaterDirty.IsSelected(themeID);
                case ColorID.WaterUnder: return CurrentMix.Water.WaterDirty.IsSelected(themeID);
                default: return false;
            }
        }

        public bool IsSelected(string themeID, OffsetID offsetID)
        {
            switch (offsetID)
            {
                case OffsetID.GrassFertilityColorOffset: return CurrentMix.Terrain.GrassFertilityColorOffset.IsSelected(themeID);
                case OffsetID.GrassFieldColorOffset: return CurrentMix.Terrain.GrassFieldColorOffset.IsSelected(themeID);
                case OffsetID.GrassForestColorOffset: return CurrentMix.Terrain.GrassForestColorOffset.IsSelected(themeID);
                case OffsetID.GrassPollutionColorOffset: return CurrentMix.Terrain.GrassPollutionColorOffset.IsSelected(themeID);
                default: return false;
            }
        }

        public bool IsSelected(string themeID, TextureID textureID)
        {
            switch (textureID)
            {
                case TextureID.GrassDiffuseTexture: return CurrentMix.Terrain.GrassDiffuseTexture.IsSelected(themeID);
                case TextureID.RuinedDiffuseTexture: return CurrentMix.Terrain.RuinedDiffuseTexture.IsSelected(themeID);
                case TextureID.PavementDiffuseTexture: return CurrentMix.Terrain.PavementDiffuseTexture.IsSelected(themeID);
                case TextureID.GravelDiffuseTexture: return CurrentMix.Terrain.GravelDiffuseTexture.IsSelected(themeID);
                case TextureID.CliffDiffuseTexture: return CurrentMix.Terrain.CliffDiffuseTexture.IsSelected(themeID);
                case TextureID.SandDiffuseTexture: return CurrentMix.Terrain.SandDiffuseTexture.IsSelected(themeID);
                case TextureID.OilDiffuseTexture: return CurrentMix.Terrain.OilDiffuseTexture.IsSelected(themeID);
                case TextureID.OreDiffuseTexture: return CurrentMix.Terrain.OreDiffuseTexture.IsSelected(themeID);
                case TextureID.CliffSandNormalTexture: return CurrentMix.Terrain.CliffSandNormalTexture.IsSelected(themeID);
                case TextureID.UpwardRoadDiffuse: return CurrentMix.Structures.UpwardRoadDiffuse.IsSelected(themeID);
                case TextureID.DownwardRoadDiffuse: return CurrentMix.Structures.DownwardRoadDiffuse.IsSelected(themeID);
                case TextureID.BuildingFloorDiffuse: return CurrentMix.Structures.BuildingFloorDiffuse.IsSelected(themeID);
                case TextureID.BuildingBaseDiffuse: return CurrentMix.Structures.BuildingBaseDiffuse.IsSelected(themeID);
                case TextureID.BuildingBaseNormal: return CurrentMix.Structures.BuildingBaseNormal.IsSelected(themeID);
                case TextureID.BuildingBurntDiffuse: return CurrentMix.Structures.BuildingBurntDiffuse.IsSelected(themeID);
                case TextureID.BuildingAbandonedDiffuse: return CurrentMix.Structures.BuildingAbandonedDiffuse.IsSelected(themeID);
                case TextureID.LightColorPalette: return CurrentMix.Structures.LightColorPalette.IsSelected(themeID);
                case TextureID.MoonTexture: return CurrentMix.Atmosphere.MoonTexture.IsSelected(themeID);
                case TextureID.WaterFoam: return CurrentMix.Water.WaterFoam.IsSelected(themeID);
                case TextureID.WaterNormal: return CurrentMix.Water.WaterNormal.IsSelected(themeID);
                default: return false;
            }
        }

        public bool IsSelected(string themeID, ValueID valueID)
        {
            switch (valueID)
            {
                case ValueID.Longitude: return CurrentMix.Atmosphere.Longitude.IsSelected(themeID);
                case ValueID.Latitude: return CurrentMix.Atmosphere.Latitude.IsSelected(themeID);
                case ValueID.SunSize: return CurrentMix.Atmosphere.SunSize.IsSelected(themeID);
                case ValueID.SunAnisotropy: return CurrentMix.Atmosphere.SunAnisotropy.IsSelected(themeID);
                case ValueID.MoonSize: return CurrentMix.Atmosphere.MoonSize.IsSelected(themeID);
                case ValueID.Rayleigh: return CurrentMix.Atmosphere.Rayleigh.IsSelected(themeID);
                case ValueID.Mie: return CurrentMix.Atmosphere.Mie.IsSelected(themeID);
                case ValueID.Exposure: return CurrentMix.Atmosphere.Exposure.IsSelected(themeID);
                case ValueID.StarsIntensity: return CurrentMix.Atmosphere.StarsIntensity.IsSelected(themeID);
                case ValueID.OuterSpaceIntensity: return CurrentMix.Atmosphere.OuterSpaceIntensity.IsSelected(themeID);
                case ValueID.GrassDetailEnabled: return CurrentMix.Terrain.GrassDetailEnabled.IsSelected(themeID);
                case ValueID.FertileDetailEnabled: return CurrentMix.Terrain.FertileDetailEnabled.IsSelected(themeID);
                case ValueID.RocksDetailEnabled: return CurrentMix.Terrain.RocksDetailEnabled.IsSelected(themeID);
                case ValueID.MinTemperatureDay: return CurrentMix.Weather.MinTemperatureDay.IsSelected(themeID);
                case ValueID.MaxTemperatureDay: return CurrentMix.Weather.MaxTemperatureDay.IsSelected(themeID);
                case ValueID.MinTemperatureNight: return CurrentMix.Weather.MinTemperatureNight.IsSelected(themeID);
                case ValueID.MaxTemperatureNight: return CurrentMix.Weather.MaxTemperatureNight.IsSelected(themeID);
                case ValueID.MinTemperatureRain: return CurrentMix.Weather.MinTemperatureRain.IsSelected(themeID);
                case ValueID.MaxTemperatureRain: return CurrentMix.Weather.MaxTemperatureRain.IsSelected(themeID);
                case ValueID.MinTemperatureFog: return CurrentMix.Weather.MinTemperatureFog.IsSelected(themeID);
                case ValueID.MaxTemperatureFog: return CurrentMix.Weather.MaxTemperatureFog.IsSelected(themeID);
                case ValueID.RainProbabilityDay: return CurrentMix.Weather.RainProbabilityDay.IsSelected(themeID);
                case ValueID.RainProbabilityNight: return CurrentMix.Weather.RainProbabilityNight.IsSelected(themeID);
                case ValueID.FogProbabilityDay: return CurrentMix.Weather.FogProbabilityDay.IsSelected(themeID);
                case ValueID.FogProbabilityNight: return CurrentMix.Weather.FogProbabilityNight.IsSelected(themeID);
                case ValueID.NorthernLightsProbability: return CurrentMix.Weather.NorthernLightsProbability.IsSelected(themeID);
                default: return false;
            }
        }

        public void MaybeUpdateThemeDecal(TerrainTexture terrainTexture)
        {
            if (!Mod.ThemeDecalsEnabled) return;
            if (terrainTexture == null || terrainTexture.Texture == null) return;
            string meshName = string.Empty;
            switch (terrainTexture.Name)
            {
                case TerrainTexture.TextureName.CliffDiffuseTexture:
                    meshName = "themedecal-cliff";
                    break;
                case TerrainTexture.TextureName.GrassDiffuseTexture:
                    meshName = "themedecal-grass";
                    break;
                case TerrainTexture.TextureName.GravelDiffuseTexture:
                    meshName = "themedecal-gravel";
                    break;
                case TerrainTexture.TextureName.OreDiffuseTexture:
                    meshName = "themedecal-ore";
                    break;
                case TerrainTexture.TextureName.OilDiffuseTexture:
                    meshName = "themedecal-oil";
                    break;
                case TerrainTexture.TextureName.PavementDiffuseTexture:
                    meshName = "themedecal-pavement";
                    break;
                case TerrainTexture.TextureName.RuinedDiffuseTexture:
                    meshName = "themedecal-ruined";
                    break;
                case TerrainTexture.TextureName.SandDiffuseTexture:
                    meshName = "themedecal-sand";
                    break;
            }
            for (uint i = 0; i < PrefabCollection<PropInfo>.LoadedCount(); i++)
            {
                PropInfo prefab = PrefabCollection<PropInfo>.GetLoaded(i);
                if (prefab == null) continue;
                if (prefab.m_mesh == null) continue;
                if (prefab.m_mesh.name != meshName) continue;
                prefab.m_material.SetTexture("_MainTex", terrainTexture.Texture);
                prefab.m_lodMaterialCombined.SetTexture("_MainTex", terrainTexture.Texture);
                prefab.m_lodMaterial = prefab.m_material;
                prefab.m_lodRenderDistance = 18000;
            }
        }

        public void LoadLut(string lut)
        {
            if (string.IsNullOrEmpty(lut)) return;
            Texture3DWrapper wrapper = BuiltInLuts.FirstOrDefault(t3d => t3d.name == lut);
            int index = wrapper != null ? Array.IndexOf(BuiltInLuts, wrapper) : -1;
            if (index != -1)
            {
                ColorCorrectionManager.instance.currentSelection = index + 1;
                return;
            }
            var userLuts = UserLuts;
            Package.Asset asset = userLuts.Find(l => l.name == lut);
            index = asset != null ? UserLuts.IndexOf(asset) : -1;
            if (index != -1) ColorCorrectionManager.instance.currentSelection = index + BuiltInLuts.Length + 1;
            OptionsGraphicsPanel ogp = FindObjectOfType<OptionsGraphicsPanel>();
            if (ogp == null) return;
            ogp.SendMessage("RefreshColorCorrectionLUTs");
        }

        public void SetLut()
        {
            if (CurrentMix == null) return;
            CurrentMix.Lut = Lut;
            SaveLocalMix();
        }
    }
}
