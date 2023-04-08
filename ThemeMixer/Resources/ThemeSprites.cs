using System.Collections.Generic;
using System.Text.RegularExpressions;
using ColossalFramework.UI;
using ThemeMixer.Themes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThemeMixer.Resources
{
    public class ThemeSprites
    {
        public static UITextureAtlas Atlas { get; private set; }

        private static List<string> SpriteNames { get; } = new List<string>();
        private static List<Texture2D> SpriteTextures { get; } = new List<Texture2D>();

        public const string SteamPreview = "SteamPreview";
        public const string SnapShot = "Snapshot";

        public const string GrassDiffuseTexture = "GrassDiffuseTexture";
        public const string RuinedDiffuseTexture = "RuinedDiffuseTexture";
        public const string PavementDiffuseTexture = "PavementDiffuseTexture";
        public const string GravelDiffuseTexture = "GravelDiffuseTexture";
        public const string CliffDiffuseTexture = "CliffDiffuseTexture";
        public const string OilDiffuseTexture = "OilDiffuseTexture";
        public const string OreDiffuseTexture = "OreDiffuseTexture";
        public const string SandDiffuseTexture = "SandDiffuseTexture";
        public const string CliffSandNormalTexture = "CliffSandNormalTexture";

        public const string WaterFoam = "WaterFoam";
        public const string WaterNormal = "WaterNormal";

        public const string UpwardRoadDiffuse = "UpwardRoadDiffuse";
        public const string DownwardRoadDiffuse = "DownwardRoadDiffuse";
        public const string FloorDiffuse = "FloorDiffuse";
        public const string BaseDiffuse = "BaseDiffuse";
        public const string BaseNormal = "BaseNormal";
        public const string BurntDiffuse = "BurntDiffuse";
        public const string AbandonedDiffuse = "AbandonedDiffuse";
        public const string LightColorPalette = "LightColorPalette";

        public const string MoonTexture = "MoonTexture";

        private static string[] AssetNames { get; } = {
            SteamPreview,
            SnapShot,
            GrassDiffuseTexture,
            RuinedDiffuseTexture,
            PavementDiffuseTexture,
            GravelDiffuseTexture,
            CliffDiffuseTexture,
            OilDiffuseTexture,
            OreDiffuseTexture,
            SandDiffuseTexture,
            CliffSandNormalTexture,
            WaterFoam,
            WaterNormal,
            UpwardRoadDiffuse,
            DownwardRoadDiffuse,
            FloorDiffuse,
            BaseDiffuse,
            BaseNormal,
            BurntDiffuse,
            AbandonedDiffuse,
            LightColorPalette,
            MoonTexture
        };

        public static void CreateAtlas()
        {
            SpriteNames.Clear();
            SpriteTextures.Clear();
            foreach (MapThemeMetaData meta in ThemeManager.Instance.Themes.Values)
            {
                if (meta == null) continue;
                for (var i = 0; i < AssetNames.Length; i++)
                {
                    string assetName = i < 2 ? string.Concat(meta.name, "_", AssetNames[i]) : AssetNames[i];
                    string spriteName = string.Concat(meta.assetRef.fullName, assetName);
                    spriteName = Regex.Replace(spriteName, @"(\s+|@|&|'||<|>|#|"")", ""); var tex = meta.assetRef.package.Find(assetName)?.Instantiate<Texture2D>();
                    if (tex == null)
                    {
                        Debug.Log("Failed to load texture: " + spriteName);
                        continue;
                    }
                    Texture2D spriteTex = tex.ScaledCopy(64.0f / tex.height);
                    Object.Destroy(tex);
                    spriteTex.Apply();
                    SpriteNames.Add(spriteName);
                    SpriteTextures.Add(spriteTex);
                }
            }
            Atlas = ResourceUtils.CreateAtlas("ThemesAtlas", SpriteNames.ToArray(), SpriteTextures.ToArray());
        }
    }
}
