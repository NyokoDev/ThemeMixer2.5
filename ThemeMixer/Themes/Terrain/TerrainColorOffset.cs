using JetBrains.Annotations;
using ThemeMixer.Themes.Abstraction;
using UnityEngine;

namespace ThemeMixer.Themes.Terrain
{
    public class TerrainColorOffset : ThemePartBase
    {
        public OffsetName Name;

        [UsedImplicitly]
        public TerrainColorOffset() { }

        public TerrainColorOffset(OffsetName offsetName)
        {
            Name = offsetName;
        }

        public TerrainColorOffset(string themeID, OffsetName offsetName) : base(themeID)
        {
            Name = offsetName;
        }

        protected override bool SetFromTheme()
        {
            MapThemeMetaData metaData = ThemeManager.GetTheme(ThemeID);
            if (metaData == null) return false;
            switch (Name)
            {
                case OffsetName.GrassFertilityColorOffset:
                    return SetValue(metaData.grassFertilityColorOffset);
                case OffsetName.GrassFieldColorOffset:
                    return SetValue(metaData.grassFieldColorOffset);
                case OffsetName.GrassForestColorOffset:
                    return SetValue(metaData.grassForestColorOffset);
                case OffsetName.GrassPollutionColorOffset:
                    return SetValue(metaData.grassPollutionColorOffset);
                default: return false;
            }
        }

        protected override bool SetFromProperties()
        {
            TerrainProperties properties = TerrainManager.instance.m_properties;
            switch (Name)
            {
                case OffsetName.GrassFertilityColorOffset:
                    return SetValue(properties.m_grassFertilityColorOffset);
                case OffsetName.GrassFieldColorOffset:
                    return SetValue(properties.m_grassFieldColorOffset);
                case OffsetName.GrassForestColorOffset:
                    return SetValue(properties.m_grassForestColorOffset);
                case OffsetName.GrassPollutionColorOffset:
                    return SetValue(properties.m_grassPollutionColorOffset);
                default: return false;
            }
        }

        protected override void LoadValue()
        {
            TerrainProperties properties = TerrainManager.instance.m_properties;
            switch (Name)
            {
                case OffsetName.GrassFertilityColorOffset:
                    properties.m_grassFertilityColorOffset = (Vector3)(CustomValue ?? Value);
                    break;
                case OffsetName.GrassFieldColorOffset:
                    properties.m_grassFieldColorOffset = (Vector3)(CustomValue ?? Value);
                    break;
                case OffsetName.GrassForestColorOffset:
                    properties.m_grassForestColorOffset = (Vector3)(CustomValue ?? Value);
                    break;
                case OffsetName.GrassPollutionColorOffset:
                    properties.m_grassPollutionColorOffset = (Vector3)(CustomValue ?? Value);
                    break;
            }
            SetShaderVectors();
        }

        private static void SetShaderVectors()
        {
            TerrainProperties properties = TerrainManager.instance.m_properties;
            Shader.SetGlobalVector("_GrassFieldColorOffset", properties.m_grassFieldColorOffset);
            Shader.SetGlobalVector("_GrassFertilityColorOffset", properties.m_grassFertilityColorOffset);
            Shader.SetGlobalVector("_GrassForestColorOffset", properties.m_grassForestColorOffset);
            Shader.SetGlobalVector("_GrassPollutionColorOffset",
                                   new Vector4(properties.m_grassPollutionColorOffset.x,
                                   properties.m_grassPollutionColorOffset.y,
                                   properties.m_grassPollutionColorOffset.z,
                                   properties.m_cliffSandNormalTiling));
        }

        public enum OffsetName
        {
            GrassPollutionColorOffset,
            GrassFieldColorOffset,
            GrassFertilityColorOffset,
            GrassForestColorOffset,
            Count
        }
    }
}
