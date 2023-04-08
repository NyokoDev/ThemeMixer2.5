using JetBrains.Annotations;
using ThemeMixer.Themes.Abstraction;
using UnityEngine;

namespace ThemeMixer.Themes.Terrain
{
    public class TerrainTexture : TexturePartBase
    {
        public TextureName Name;

        [UsedImplicitly]
        public TerrainTexture() { }

        public TerrainTexture(TextureName textureName)
        {
            Name = textureName;
        }

        public TerrainTexture(TextureName textureName, string themeID) : base(themeID)
        {
            Name = textureName;
        }

        public override bool Load(string themeID = null)
        {
            if (base.Load(themeID)) LoadTiling();
            return true;
        }

        protected override bool SetFromTheme()
        {
            MapThemeMetaData metaData = ThemeManager.GetTheme(ThemeID);
            if (metaData == null) return false;
            bool success;
            switch (Name)
            {
                case TextureName.GrassDiffuseTexture:
                    success = SetTexture(metaData.grassDiffuseAsset);
                    return success && SetValue(metaData.grassTiling);
                case TextureName.RuinedDiffuseTexture:
                    success = SetTexture(metaData.ruinedDiffuseAsset);
                    return success && SetValue(metaData.ruinedTiling);
                case TextureName.PavementDiffuseTexture:
                    success = SetTexture(metaData.pavementDiffuseAsset);
                    return success && SetValue(metaData.pavementTiling);
                case TextureName.GravelDiffuseTexture:
                    success = SetTexture(metaData.gravelDiffuseAsset);
                    return success && SetValue(metaData.gravelTiling);
                case TextureName.CliffDiffuseTexture:
                    success = SetTexture(metaData.cliffDiffuseAsset);
                    return success && SetValue(metaData.cliffDiffuseTiling);
                case TextureName.OilDiffuseTexture:
                    success = SetTexture(metaData.oilDiffuseAsset);
                    return success && SetValue(metaData.oilTiling);
                case TextureName.OreDiffuseTexture:
                    success = SetTexture(metaData.oreDiffuseAsset);
                    return success && SetValue(metaData.oreTiling);
                case TextureName.SandDiffuseTexture:
                    success = SetTexture(metaData.sandDiffuseAsset);
                    return success && SetValue(metaData.sandDiffuseTiling);
                case TextureName.CliffSandNormalTexture:
                    success = SetTexture(metaData.cliffSandNormalAsset);
                    return success && SetValue(metaData.cliffSandNormalTiling);
                default: return false;
            }
        }

        protected override bool SetFromProperties()
        {
            TerrainProperties properties = TerrainManager.instance.m_properties;
            bool success;
            switch (Name)
            {
                case TextureName.GrassDiffuseTexture:
                    success = SetTexture(properties.m_grassDiffuse);
                    return success && SetValue(properties.m_grassTiling);
                case TextureName.RuinedDiffuseTexture:
                    success = SetTexture(properties.m_ruinedDiffuse);
                    return success && SetValue(properties.m_ruinedTiling);
                case TextureName.PavementDiffuseTexture:
                    success = SetTexture(properties.m_pavementDiffuse);
                    return success && SetValue(properties.m_pavementTiling);
                case TextureName.GravelDiffuseTexture:
                    success = SetTexture(properties.m_gravelDiffuse);
                    return success && SetValue(properties.m_gravelTiling);
                case TextureName.CliffDiffuseTexture:
                    success = SetTexture(properties.m_cliffDiffuse);
                    return success && SetValue(properties.m_cliffTiling);
                case TextureName.OilDiffuseTexture:
                    success = SetTexture(properties.m_oilDiffuse);
                    return success && SetValue(properties.m_oilTiling);
                case TextureName.OreDiffuseTexture:
                    success = SetTexture(properties.m_oreDiffuse);
                    return success && SetValue(properties.m_oreTiling);
                case TextureName.SandDiffuseTexture:
                    success = SetTexture(properties.m_sandDiffuse);
                    return success && SetValue(properties.m_sandTiling);
                case TextureName.CliffSandNormalTexture:
                    success = SetTexture(properties.m_cliffSandNormal);
                    return success && SetValue(properties.m_cliffSandNormalTiling);
                default: return false;
            }
        }

        protected override void LoadValue()
        {
            TerrainProperties properties = TerrainManager.instance.m_properties;
            Texture2D oldTexture = null;
            switch (Name)
            {
                case TextureName.GrassDiffuseTexture:
                    oldTexture = properties.m_grassDiffuse;
                    properties.m_grassDiffuse = Texture;
                    Shader.SetGlobalTexture("_TerrainGrassDiffuse", properties.m_grassDiffuse);
                    break;
                case TextureName.RuinedDiffuseTexture:
                    oldTexture = properties.m_ruinedDiffuse;
                    properties.m_ruinedDiffuse = Texture;
                    Shader.SetGlobalTexture("_TerrainRuinedDiffuse", properties.m_ruinedDiffuse);
                    break;
                case TextureName.PavementDiffuseTexture:
                    oldTexture = properties.m_pavementDiffuse;
                    properties.m_pavementDiffuse = Texture;
                    Shader.SetGlobalTexture("_TerrainPavementDiffuse", properties.m_pavementDiffuse);
                    break;
                case TextureName.GravelDiffuseTexture:
                    oldTexture = properties.m_gravelDiffuse;
                    properties.m_gravelDiffuse = Texture;
                    Shader.SetGlobalTexture("_TerrainGravelDiffuse", properties.m_gravelDiffuse);
                    break;
                case TextureName.CliffDiffuseTexture:
                    oldTexture = properties.m_cliffDiffuse;
                    properties.m_cliffDiffuse = Texture;
                    Shader.SetGlobalTexture("_TerrainCliffDiffuse", properties.m_cliffDiffuse);
                    break;
                case TextureName.OreDiffuseTexture:
                    oldTexture = properties.m_oreDiffuse;
                    properties.m_oreDiffuse = Texture;
                    Shader.SetGlobalTexture("_TerrainOreDiffuse", properties.m_oreDiffuse);
                    break;
                case TextureName.OilDiffuseTexture:
                    oldTexture = properties.m_oilDiffuse;
                    properties.m_oilDiffuse = Texture;
                    Shader.SetGlobalTexture("_TerrainOilDiffuse", properties.m_oilDiffuse);
                    break;
                case TextureName.SandDiffuseTexture:
                    oldTexture = properties.m_sandDiffuse;
                    properties.m_sandDiffuse = Texture;
                    Shader.SetGlobalTexture("_TerrainSandDiffuse", properties.m_sandDiffuse);
                    break;
                case TextureName.CliffSandNormalTexture:
                    oldTexture = properties.m_cliffSandNormal;
                    properties.m_cliffSandNormal = Texture;
                    Shader.SetGlobalTexture("_TerrainCliffSandNormal", properties.m_cliffSandNormal);
                    break;
            }
            if (oldTexture != null && !ReferenceEquals(oldTexture, Texture)) Object.Destroy(oldTexture);
            ThemeManager.MaybeUpdateThemeDecal(this);
        }

        public void LoadTiling()
        {
            TerrainProperties properties = TerrainManager.instance.m_properties;
            switch (Name)
            {
                case TextureName.GrassDiffuseTexture:
                    properties.m_grassTiling = (float)(CustomValue ?? Value);
                    break;
                case TextureName.RuinedDiffuseTexture:
                    properties.m_ruinedTiling = (float)(CustomValue ?? Value);
                    break;
                case TextureName.PavementDiffuseTexture:
                    properties.m_pavementTiling = (float)(CustomValue ?? Value);
                    break;
                case TextureName.GravelDiffuseTexture:
                    properties.m_gravelTiling = (float)(CustomValue ?? Value);
                    break;
                case TextureName.CliffDiffuseTexture:
                    properties.m_cliffTiling = (float)(CustomValue ?? Value);
                    break;
                case TextureName.OilDiffuseTexture:
                    properties.m_oilTiling = (float)(CustomValue ?? Value);
                    break;
                case TextureName.OreDiffuseTexture:
                    properties.m_oreTiling = (float)(CustomValue ?? Value);
                    break;
                case TextureName.SandDiffuseTexture:
                    properties.m_sandTiling = (float)(CustomValue ?? Value);
                    break;
                case TextureName.CliffSandNormalTexture:
                    properties.m_cliffSandNormalTiling = (float)(CustomValue ?? Value);
                    break;
            }

            Shader.SetGlobalVector("_GrassPollutionColorOffset", // Needed here to set CliffSandNormalTiling
                                   new Vector4(properties.m_grassPollutionColorOffset.x,
                                   properties.m_grassPollutionColorOffset.y,
                                   properties.m_grassPollutionColorOffset.z,
                                   properties.m_cliffSandNormalTiling));

            Shader.SetGlobalVector("_TerrainTextureTiling1",
                                   new Vector4(properties.m_pavementTiling,
                                   properties.m_ruinedTiling,
                                   properties.m_sandTiling,
                                   properties.m_cliffTiling));

            Shader.SetGlobalVector("_TerrainTextureTiling2",
                                   new Vector4(properties.m_grassTiling,
                                   properties.m_gravelTiling,
                                   properties.m_oreTiling,
                                   properties.m_oilTiling));
        }

        public enum TextureName
        {
            GrassDiffuseTexture,
            RuinedDiffuseTexture,
            PavementDiffuseTexture,
            GravelDiffuseTexture,
            CliffDiffuseTexture,
            OilDiffuseTexture,
            OreDiffuseTexture,
            SandDiffuseTexture,
            CliffSandNormalTexture,
            Count
        }
    }
}
