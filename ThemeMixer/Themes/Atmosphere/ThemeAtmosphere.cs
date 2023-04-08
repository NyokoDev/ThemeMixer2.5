using System;
namespace ThemeMixer.Themes.Atmosphere
{
    [Serializable]
    public class ThemeAtmosphere : ISelectable, IPackageIDListProvider
    {
        public AtmosphereFloat Longitude;
        public AtmosphereFloat Latitude;
        public AtmosphereFloat SunSize;
        public AtmosphereFloat SunAnisotropy;
        public AtmosphereFloat Rayleigh;
        public AtmosphereFloat Mie;
        public AtmosphereFloat Exposure;
        public AtmosphereColor SkyTint;

        public MoonTexture MoonTexture;
        public AtmosphereFloat MoonSize;
        public AtmosphereColor MoonInnerCorona;
        public AtmosphereColor MoonOuterCorona;
        public AtmosphereColor NightHorizonColor;
        public AtmosphereColor EarlyNightZenithColor;
        public AtmosphereColor LateNightZenithColor;
        public AtmosphereFloat StarsIntensity;
        public AtmosphereFloat OuterSpaceIntensity;

        public ThemeAtmosphere()
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
                Longitude.ThemeID,
                Latitude.ThemeID,
                SunSize.ThemeID,
                SunAnisotropy.ThemeID,
                Rayleigh.ThemeID,
                Mie.ThemeID,
                Exposure.ThemeID,
                SkyTint.ThemeID,
                MoonTexture.ThemeID,
                MoonSize.ThemeID,
                MoonInnerCorona.ThemeID,
                MoonOuterCorona.ThemeID,
                NightHorizonColor.ThemeID,
                EarlyNightZenithColor.ThemeID,
                LateNightZenithColor.ThemeID,
                StarsIntensity.ThemeID,
                OuterSpaceIntensity.ThemeID
            };
        }
        public bool IsSelected(string themeID)
        {
            return Longitude.IsSelected(themeID) &&
                   Latitude.IsSelected(themeID) &&
                   SunSize.IsSelected(themeID) &&
                   SunAnisotropy.IsSelected(themeID) &&
                   Rayleigh.IsSelected(themeID) &&
                   Mie.IsSelected(themeID) &&
                   Exposure.IsSelected(themeID) &&
                   SkyTint.IsSelected(themeID) &&
                   MoonTexture.IsSelected(themeID) &&
                   MoonSize.IsSelected(themeID) &&
                   MoonInnerCorona.IsSelected(themeID) &&
                   MoonOuterCorona.IsSelected(themeID) &&
                   NightHorizonColor.IsSelected(themeID) &&
                   EarlyNightZenithColor.IsSelected(themeID) &&
                   LateNightZenithColor.IsSelected(themeID) &&
                   StarsIntensity.IsSelected(themeID) &&
                   OuterSpaceIntensity.IsSelected(themeID);
        }

        private void Initialize()
        {
            Longitude = new AtmosphereFloat(AtmosphereFloat.FloatName.Longitude);
            Latitude = new AtmosphereFloat(AtmosphereFloat.FloatName.Latitude);
            SunSize = new AtmosphereFloat(AtmosphereFloat.FloatName.SunSize);
            SunAnisotropy = new AtmosphereFloat(AtmosphereFloat.FloatName.SunAnisotropy);
            Rayleigh = new AtmosphereFloat(AtmosphereFloat.FloatName.Rayleigh);
            Mie = new AtmosphereFloat(AtmosphereFloat.FloatName.Mie);
            Exposure = new AtmosphereFloat(AtmosphereFloat.FloatName.Exposure);
            StarsIntensity = new AtmosphereFloat(AtmosphereFloat.FloatName.StarsIntensity);
            OuterSpaceIntensity = new AtmosphereFloat(AtmosphereFloat.FloatName.OuterSpaceIntensity);
            MoonSize = new AtmosphereFloat(AtmosphereFloat.FloatName.MoonSize);
            MoonTexture = new MoonTexture();
            MoonInnerCorona = new AtmosphereColor(AtmosphereColor.ColorName.MoonInnerCorona);
            MoonOuterCorona = new AtmosphereColor(AtmosphereColor.ColorName.MoonOuterCorona);
            SkyTint = new AtmosphereColor(AtmosphereColor.ColorName.SkyTint);
            NightHorizonColor = new AtmosphereColor(AtmosphereColor.ColorName.NightHorizonColor);
            EarlyNightZenithColor = new AtmosphereColor(AtmosphereColor.ColorName.EarlyNightZenithColor);
            LateNightZenithColor = new AtmosphereColor(AtmosphereColor.ColorName.LateNightZenithColor);
        }

        private void SetAll(string themeID)
        {
            for (int i = 0; i < (int)AtmosphereFloat.FloatName.Count; i++)
            {
                SetFloat(themeID, (AtmosphereFloat.FloatName)i);
            }
            for (int j = 0; j < (int)AtmosphereColor.ColorName.Count; j++)
            {
                SetColor(themeID, (AtmosphereColor.ColorName)j);
            }
            SetMoon(themeID);
        }

        private bool LoadAll()
        {
            bool success = true;
            for (int i = 0; i < (int)AtmosphereFloat.FloatName.Count; i++)
            {
                if (!LoadFloat((AtmosphereFloat.FloatName)i)) success = false;
            }
            for (int j = 0; j < (int)AtmosphereColor.ColorName.Count; j++)
            {
                if (!LoadColor((AtmosphereColor.ColorName)j)) success = false;
            }
            if (!LoadMoon()) success = false;
            return success;
        }

        private void SetFloat(string themeID, AtmosphereFloat.FloatName name)
        {
            switch (name)
            {
                case AtmosphereFloat.FloatName.Longitude:
                    Longitude = new AtmosphereFloat(themeID, name);
                    break;
                case AtmosphereFloat.FloatName.Latitude:
                    Latitude = new AtmosphereFloat(themeID, name);
                    break;
                case AtmosphereFloat.FloatName.SunSize:
                    SunSize = new AtmosphereFloat(themeID, name);
                    break;
                case AtmosphereFloat.FloatName.SunAnisotropy:
                    SunAnisotropy = new AtmosphereFloat(themeID, name);
                    break;
                case AtmosphereFloat.FloatName.MoonSize:
                    MoonSize = new AtmosphereFloat(themeID, name);
                    break;
                case AtmosphereFloat.FloatName.Rayleigh:
                    Rayleigh = new AtmosphereFloat(themeID, name);
                    break;
                case AtmosphereFloat.FloatName.Mie:
                    Mie = new AtmosphereFloat(themeID, name);
                    break;
                case AtmosphereFloat.FloatName.Exposure:
                    Exposure = new AtmosphereFloat(themeID, name);
                    break;
                case AtmosphereFloat.FloatName.StarsIntensity:
                    StarsIntensity = new AtmosphereFloat(themeID, name);
                    break;
                case AtmosphereFloat.FloatName.OuterSpaceIntensity:
                    OuterSpaceIntensity = new AtmosphereFloat(themeID, name);
                    break;
            }
        }

        private bool LoadFloat(AtmosphereFloat.FloatName name)
        {
            switch (name)
            {
                case AtmosphereFloat.FloatName.Longitude:
                    return Longitude.Load();
                case AtmosphereFloat.FloatName.Latitude:
                    return Latitude.Load();
                case AtmosphereFloat.FloatName.SunSize:
                    return SunSize.Load();
                case AtmosphereFloat.FloatName.SunAnisotropy:
                    return SunAnisotropy.Load();
                case AtmosphereFloat.FloatName.MoonSize:
                    return MoonSize.Load();
                case AtmosphereFloat.FloatName.Rayleigh:
                    return Rayleigh.Load();
                case AtmosphereFloat.FloatName.Mie:
                    return Mie.Load();
                case AtmosphereFloat.FloatName.Exposure:
                    return Exposure.Load();
                case AtmosphereFloat.FloatName.StarsIntensity:
                    return StarsIntensity.Load();
                case AtmosphereFloat.FloatName.OuterSpaceIntensity:
                    return OuterSpaceIntensity.Load();
                default: return false;
            }
        }

        private void SetColor(string themeID, AtmosphereColor.ColorName name)
        {
            switch (name)
            {
                case AtmosphereColor.ColorName.MoonInnerCorona:
                    MoonInnerCorona = new AtmosphereColor(themeID, name);
                    break;
                case AtmosphereColor.ColorName.MoonOuterCorona:
                    MoonOuterCorona = new AtmosphereColor(themeID, name);
                    break;
                case AtmosphereColor.ColorName.SkyTint:
                    SkyTint = new AtmosphereColor(themeID, name);
                    break;
                case AtmosphereColor.ColorName.NightHorizonColor:
                    NightHorizonColor = new AtmosphereColor(themeID, name);
                    break;
                case AtmosphereColor.ColorName.EarlyNightZenithColor:
                    EarlyNightZenithColor = new AtmosphereColor(themeID, name);
                    break;
                case AtmosphereColor.ColorName.LateNightZenithColor:
                    LateNightZenithColor = new AtmosphereColor(themeID, name);
                    break;
            }
        }

        private bool LoadColor(AtmosphereColor.ColorName name)
        {
            switch (name)
            {
                case AtmosphereColor.ColorName.MoonInnerCorona:
                    return MoonInnerCorona.Load();
                case AtmosphereColor.ColorName.MoonOuterCorona:
                    return MoonOuterCorona.Load();
                case AtmosphereColor.ColorName.SkyTint:
                    return SkyTint.Load();
                case AtmosphereColor.ColorName.NightHorizonColor:
                    return NightHorizonColor.Load();
                case AtmosphereColor.ColorName.EarlyNightZenithColor:
                    return EarlyNightZenithColor.Load();
                case AtmosphereColor.ColorName.LateNightZenithColor:
                    return LateNightZenithColor.Load();
                default: return false;
            }
        }

        private void SetMoon(string themeID)
        {
            MoonTexture = new MoonTexture(themeID);
        }

        private bool LoadMoon()
        {
            return MoonTexture.Load();
        }
    }
}
