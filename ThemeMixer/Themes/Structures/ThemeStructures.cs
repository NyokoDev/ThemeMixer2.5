using System;
namespace ThemeMixer.Themes.Structures
{
    [Serializable]
    public class ThemeStructures : ISelectable, IPackageIDListProvider
    {
        public StructureTexture UpwardRoadDiffuse;
        public StructureTexture DownwardRoadDiffuse;
        public StructureTexture BuildingFloorDiffuse;
        public StructureTexture BuildingBaseDiffuse;
        public StructureTexture BuildingBaseNormal;
        public StructureTexture BuildingBurntDiffuse;
        public StructureTexture BuildingAbandonedDiffuse;
        public StructureTexture LightColorPalette;

        public ThemeStructures()
        {
            Initialize();
        }

        public void Set(string themeID)
        {
            SetAll(themeID);
        }

        public bool Load(string themeID = null)
        {
            if (themeID != null)
            {
                Set(themeID);
            }
            return LoadAll();
        }

        public string[] GetPackageIDs()
        {
            return new[]
            {
                UpwardRoadDiffuse.ThemeID,
                DownwardRoadDiffuse.ThemeID,
                BuildingFloorDiffuse.ThemeID,
                BuildingBaseDiffuse.ThemeID,
                BuildingBaseNormal.ThemeID,
                BuildingBurntDiffuse.ThemeID,
                BuildingAbandonedDiffuse.ThemeID,
                LightColorPalette.ThemeID
            };
        }

        public bool IsSelected(string themeID)
        {
            return UpwardRoadDiffuse.IsSelected(themeID) &&
                DownwardRoadDiffuse.IsSelected(themeID) &&
                BuildingFloorDiffuse.IsSelected(themeID) &&
                BuildingBaseDiffuse.IsSelected(themeID) &&
                BuildingBaseNormal.IsSelected(themeID) &&
                BuildingBurntDiffuse.IsSelected(themeID) &&
                BuildingAbandonedDiffuse.IsSelected(themeID) &&
                LightColorPalette.IsSelected(themeID);
        }

        private void Initialize()
        {
            UpwardRoadDiffuse = new StructureTexture(StructureTexture.TextureName.UpwardRoadDiffuse);
            DownwardRoadDiffuse = new StructureTexture(StructureTexture.TextureName.DownwardRoadDiffuse);
            BuildingFloorDiffuse = new StructureTexture(StructureTexture.TextureName.BuildingFloorDiffuse);
            BuildingBaseDiffuse = new StructureTexture(StructureTexture.TextureName.BuildingBaseDiffuse);
            BuildingBaseNormal = new StructureTexture(StructureTexture.TextureName.BuildingBaseNormal);
            BuildingBurntDiffuse = new StructureTexture(StructureTexture.TextureName.BuildingBurntDiffuse);
            BuildingAbandonedDiffuse = new StructureTexture(StructureTexture.TextureName.BuildingAbandonedDiffuse);
            LightColorPalette = new StructureTexture(StructureTexture.TextureName.LightColorPalette);
        }

        private void SetAll(string themeID)
        {
            for (int i = 0; i < (int)StructureTexture.TextureName.Count; i++)
            {
                SetTexture(themeID, (StructureTexture.TextureName)i);
            }
        }

        private bool LoadAll()
        {
            bool success = true;
            for (int i = 0; i < (int)StructureTexture.TextureName.Count; i++)
            {
                if (!LoadTexture((StructureTexture.TextureName)i)) success = false;
            }
            return success;
        }

        private void SetTexture(string themeID, StructureTexture.TextureName textureName)
        {
            switch (textureName)
            {
                case StructureTexture.TextureName.UpwardRoadDiffuse:
                    UpwardRoadDiffuse = new StructureTexture(textureName, themeID);
                    break;
                case StructureTexture.TextureName.DownwardRoadDiffuse:
                    DownwardRoadDiffuse = new StructureTexture(textureName, themeID);
                    break;
                case StructureTexture.TextureName.BuildingFloorDiffuse:
                    BuildingFloorDiffuse = new StructureTexture(textureName, themeID);
                    break;
                case StructureTexture.TextureName.BuildingBaseDiffuse:
                    BuildingBaseDiffuse = new StructureTexture(textureName, themeID);
                    break;
                case StructureTexture.TextureName.BuildingBaseNormal:
                    BuildingBaseNormal = new StructureTexture(textureName, themeID);
                    break;
                case StructureTexture.TextureName.BuildingBurntDiffuse:
                    BuildingBurntDiffuse = new StructureTexture(textureName, themeID);
                    break;
                case StructureTexture.TextureName.BuildingAbandonedDiffuse:
                    BuildingAbandonedDiffuse = new StructureTexture(textureName, themeID);
                    break;
                case StructureTexture.TextureName.LightColorPalette:
                    LightColorPalette = new StructureTexture(textureName, themeID);
                    break;
            }
        }

        private bool LoadTexture(StructureTexture.TextureName textureName)
        {
            switch (textureName)
            {
                case StructureTexture.TextureName.UpwardRoadDiffuse:
                    return UpwardRoadDiffuse.Load();
                case StructureTexture.TextureName.DownwardRoadDiffuse:
                    return DownwardRoadDiffuse.Load();
                case StructureTexture.TextureName.BuildingFloorDiffuse:
                    return BuildingFloorDiffuse.Load();
                case StructureTexture.TextureName.BuildingBaseDiffuse:
                    return BuildingBaseDiffuse.Load();
                case StructureTexture.TextureName.BuildingBaseNormal:
                    return BuildingBaseNormal.Load();
                case StructureTexture.TextureName.BuildingBurntDiffuse:
                    return BuildingBurntDiffuse.Load();
                case StructureTexture.TextureName.BuildingAbandonedDiffuse:
                    return BuildingAbandonedDiffuse.Load();
                case StructureTexture.TextureName.LightColorPalette:
                    return LightColorPalette.Load();
                default: return false;
            }
        }
    }
}
