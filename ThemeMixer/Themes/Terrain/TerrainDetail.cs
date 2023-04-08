using JetBrains.Annotations;
using ThemeMixer.Themes.Abstraction;

namespace ThemeMixer.Themes.Terrain
{
    public class TerrainDetail : ThemePartBase
    {
        public Name DetailName;

        [UsedImplicitly]
        public TerrainDetail() { }

        public TerrainDetail(Name detailName)
        {
            DetailName = detailName;
        }

        public TerrainDetail(string themeID, Name name) : base(themeID)
        {
            DetailName = name;
        }

        protected override bool SetFromTheme()
        {
            MapThemeMetaData metaData = ThemeManager.GetTheme(ThemeID);
            if (metaData == null) return false;
            switch (DetailName)
            {
                case Name.GrassDetailEnabled:
                    return SetValue(metaData.grassDetailEnabled);
                case Name.FertileDetailEnabled:
                    return SetValue(metaData.fertileDetailEnabled);
                case Name.RocksDetailEnabled:
                    return SetValue(metaData.rocksDetailEnabled);
                default: return false;
            }
        }

        protected override bool SetFromProperties()
        {
            TerrainProperties properties = TerrainManager.instance.m_properties;
            switch (DetailName)
            {
                case Name.GrassDetailEnabled:
                    return SetValue(properties.m_useGrassDecorations);
                case Name.FertileDetailEnabled:
                    return SetValue(properties.m_useFertileDecorations);
                case Name.RocksDetailEnabled:
                    return SetValue(properties.m_useCliffDecorations);
                default: return false;
            }
        }

        protected override void LoadValue()
        {
            TerrainProperties properties = TerrainManager.instance.m_properties;
            switch (DetailName)
            {
                case Name.GrassDetailEnabled:
                    properties.m_useGrassDecorations = (bool)(CustomValue ?? Value);
                    break;
                case Name.FertileDetailEnabled:
                    properties.m_useFertileDecorations = (bool)(CustomValue ?? Value);
                    break;
                case Name.RocksDetailEnabled:
                    properties.m_useCliffDecorations = (bool)(CustomValue ?? Value);
                    break;
            }
        }

        public enum Name
        {
            GrassDetailEnabled,
            FertileDetailEnabled,
            RocksDetailEnabled,
            Count
        }
    }
}
