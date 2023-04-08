using JetBrains.Annotations;
using ThemeMixer.Themes.Abstraction;

namespace ThemeMixer.Themes.Atmosphere
{
    public class AtmosphereFloat : ThemePartBase
    {
        public FloatName Name;

        [UsedImplicitly]
        public AtmosphereFloat() { }

        public AtmosphereFloat(FloatName floatName)
        {
            Name = floatName;
        }

        public AtmosphereFloat(string themeID, FloatName floatName) : base(themeID)
        {
            Name = floatName;
        }

        protected override bool SetFromTheme()
        {
            MapThemeMetaData metaData = ThemeManager.GetTheme(ThemeID);
            if (metaData == null) return false;
            switch (Name)
            {
                case FloatName.Longitude:
                    return SetValue(metaData.longitude);
                case FloatName.Latitude:
                    return SetValue(metaData.latitude);
                case FloatName.SunSize:
                    return SetValue(metaData.sunSize);
                case FloatName.SunAnisotropy:
                    return SetValue(metaData.sunAnisotropy);
                case FloatName.MoonSize:
                    return SetValue(metaData.moonSize);
                case FloatName.Rayleigh:
                    return SetValue(metaData.rayleight);
                case FloatName.Mie:
                    return SetValue(metaData.mie);
                case FloatName.Exposure:
                    return SetValue(metaData.exposure);
                case FloatName.StarsIntensity:
                    return SetValue(metaData.starsIntensity);
                case FloatName.OuterSpaceIntensity:
                    return SetValue(metaData.outerSpaceIntensity);
                default: return false;
            }
        }

        protected override bool SetFromProperties()
        {
            DayNightProperties properties = DayNightProperties.instance;
            switch (Name)
            {
                case FloatName.Longitude:
                    return SetValue(properties.m_Longitude);
                case FloatName.Latitude:
                    return SetValue(properties.m_Latitude);
                case FloatName.SunSize:
                    return SetValue(properties.m_SunSize);
                case FloatName.SunAnisotropy:
                    return SetValue(properties.m_SunAnisotropyFactor);
                case FloatName.MoonSize:
                    return SetValue(properties.m_MoonSize);
                case FloatName.Rayleigh:
                    return SetValue(properties.m_RayleighScattering);
                case FloatName.Mie:
                    return SetValue(properties.m_MieScattering);
                case FloatName.Exposure:
                    return SetValue(properties.m_Exposure);
                case FloatName.StarsIntensity:
                    return SetValue(properties.m_StarsIntensity);
                case FloatName.OuterSpaceIntensity:
                    return SetValue(properties.m_OuterSpaceIntensity);
                default: return false;
            }
        }

        protected override void LoadValue()
        {
            DayNightProperties properties = DayNightProperties.instance;
            switch (Name)
            {
                case FloatName.Longitude:
                    properties.m_Longitude = (float)(CustomValue ?? Value);
                    break;
                case FloatName.Latitude:
                    properties.m_Latitude = (float)(CustomValue ?? Value);
                    break;
                case FloatName.SunSize:
                    properties.m_SunSize = (float)(CustomValue ?? Value);
                    break;
                case FloatName.SunAnisotropy:
                    properties.m_SunAnisotropyFactor = (float)(CustomValue ?? Value);
                    break;
                case FloatName.MoonSize:
                    properties.m_MoonSize = (float)(CustomValue ?? Value);
                    break;
                case FloatName.Rayleigh:
                    properties.m_RayleighScattering = (float)(CustomValue ?? Value);
                    break;
                case FloatName.Mie:
                    properties.m_MieScattering = (float)(CustomValue ?? Value);
                    break;
                case FloatName.Exposure:
                    properties.m_Exposure = (float)(CustomValue ?? Value);
                    break;
                case FloatName.StarsIntensity:
                    properties.m_StarsIntensity = (float)(CustomValue ?? Value);
                    break;
                case FloatName.OuterSpaceIntensity:
                    properties.m_OuterSpaceIntensity = (float)(CustomValue ?? Value);
                    break;
            }
        }

        public enum FloatName
        {
            Longitude,
            Latitude,

            SunSize,
            SunAnisotropy,
            MoonSize,

            Rayleigh,
            Mie,
            Exposure,
            StarsIntensity,
            OuterSpaceIntensity,

            Count
        }
    }
}
