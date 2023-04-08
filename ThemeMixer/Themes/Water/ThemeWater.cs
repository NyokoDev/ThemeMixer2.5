using System;

namespace ThemeMixer.Themes.Water
{
    [Serializable]
    public class ThemeWater : ISelectable, IPackageIDListProvider
    {
        public WaterTexture WaterFoam;
        public WaterTexture WaterNormal;

        public WaterColor WaterClean;
        public WaterColor WaterDirty;
        public WaterColor WaterUnder;

        public ThemeWater()
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
                WaterFoam.ThemeID,
                WaterNormal.ThemeID,
                WaterClean.ThemeID,
                WaterDirty.ThemeID,
                WaterUnder.ThemeID
            };
        }

        public bool IsSelected(string themeID)
        {
            return WaterFoam.IsSelected(themeID) &&
                   WaterNormal.IsSelected(themeID) &&
                   WaterClean.IsSelected(themeID) &&
                   WaterDirty.IsSelected(themeID) &&
                   WaterUnder.IsSelected(themeID);
        }

        private void Initialize()
        {
            WaterFoam = new WaterTexture(WaterTexture.TextureName.WaterFoam);
            WaterNormal = new WaterTexture(WaterTexture.TextureName.WaterNormal);

            WaterClean = new WaterColor(WaterColor.ColorName.WaterClean);
            WaterDirty = new WaterColor(WaterColor.ColorName.WaterDirty);
            WaterUnder = new WaterColor(WaterColor.ColorName.WaterUnder);
        }

        private void SetAll(string themeID)
        {
            for (int i = 0; i < (int)WaterTexture.TextureName.Count; i++)
            {
                SetTexture(themeID, (WaterTexture.TextureName)i);
            }
            for (int j = 0; j < (int)WaterColor.ColorName.Count; j++)
            {
                SetColor(themeID, (WaterColor.ColorName)j);
            }
        }

        private bool LoadAll()
        {
            bool success = true;
            for (int i = 0; i < (int)WaterTexture.TextureName.Count; i++)
            {
                if (!LoadTexture((WaterTexture.TextureName)i)) success = false;
            }
            for (int j = 0; j < (int)WaterColor.ColorName.Count; j++)
            {
                if (!LoadColor((WaterColor.ColorName)j)) success = false;
            }
            return success;
        }

        private void SetTexture(string themeID, WaterTexture.TextureName textureName)
        {
            switch (textureName)
            {
                case WaterTexture.TextureName.WaterFoam:
                    WaterFoam = new WaterTexture(textureName, themeID);
                    break;
                case WaterTexture.TextureName.WaterNormal:
                    WaterNormal = new WaterTexture(textureName, themeID);
                    break;
            }
        }

        private bool LoadTexture(WaterTexture.TextureName textureName)
        {
            switch (textureName)
            {
                case WaterTexture.TextureName.WaterFoam:
                    return WaterFoam.Load();
                case WaterTexture.TextureName.WaterNormal:
                    return WaterNormal.Load();
                default: return false;
            }
        }

        private void SetColor(string themeID, WaterColor.ColorName name)
        {
            switch (name)
            {
                case WaterColor.ColorName.WaterClean:
                    WaterClean = new WaterColor(themeID, name);
                    break;
                case WaterColor.ColorName.WaterDirty:
                    WaterDirty = new WaterColor(themeID, name);
                    break;
                case WaterColor.ColorName.WaterUnder:
                    WaterUnder = new WaterColor(themeID, name);
                    break;
            }
        }

        private bool LoadColor(WaterColor.ColorName name)
        {
            switch (name)
            {
                case WaterColor.ColorName.WaterClean:
                    return WaterClean.Load();
                case WaterColor.ColorName.WaterDirty:
                    return WaterDirty.Load();
                case WaterColor.ColorName.WaterUnder:
                    return WaterUnder.Load();
                default: return false;
            }
        }
    }
}
