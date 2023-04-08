using JetBrains.Annotations;
using ThemeMixer.Themes.Abstraction;
using UnityEngine;

namespace ThemeMixer.Themes.Atmosphere
{
    public class AtmosphereColor : ThemePartBase
    {
        public ColorName Name;

        [UsedImplicitly]
        public AtmosphereColor() { }

        public AtmosphereColor(ColorName colorName)
        {
            Name = colorName;
        }

        public AtmosphereColor(string themeID, ColorName floatName) : base(themeID)
        {
            Name = floatName;
        }

        protected override bool SetFromTheme()
        {
            MapThemeMetaData metaData = ThemeManager.GetTheme(ThemeID);
            if (metaData == null) return false;
            switch (Name)
            {
                case ColorName.MoonInnerCorona:
                    return SetValue(metaData.moonInnerCorona);
                case ColorName.MoonOuterCorona:
                    return SetValue(metaData.moonOuterCorona);
                case ColorName.SkyTint:
                    return SetValue(metaData.skyTint);
                case ColorName.NightHorizonColor:
                    return SetValue(metaData.nightHorizonColor);
                case ColorName.EarlyNightZenithColor:
                    return SetValue(metaData.earlyNightZenithColor);
                case ColorName.LateNightZenithColor:
                    return SetValue(metaData.lateNightZenithColor);
                default: return false;
            }
        }

        protected override bool SetFromProperties()
        {
            DayNightProperties properties = DayNightProperties.instance;
            switch (Name)
            {
                case ColorName.MoonInnerCorona:
                    return SetValue(properties.m_MoonInnerCorona);
                case ColorName.MoonOuterCorona:
                    return SetValue(properties.m_MoonOuterCorona);
                case ColorName.SkyTint:
                    return SetValue(properties.m_SkyTint);
                case ColorName.NightHorizonColor:
                    return SetValue(properties.nightHorizonColor);
                case ColorName.EarlyNightZenithColor:
                    return SetValue(properties.m_NightZenithColor.colorKeys[1].color);
                case ColorName.LateNightZenithColor:
                    return SetValue(properties.m_NightZenithColor.colorKeys[0].color);
                default: return false;
            }
        }

        protected override void LoadValue()
        {
            DayNightProperties properties = DayNightProperties.instance;
            switch (Name)
            {
                case ColorName.MoonInnerCorona:
                    properties.m_MoonInnerCorona = (Color)(CustomValue ?? Value);
                    break;
                case ColorName.MoonOuterCorona:
                    properties.m_MoonOuterCorona = (Color)(CustomValue ?? Value);
                    break;
                case ColorName.SkyTint:
                    properties.m_SkyTint = (Color)(CustomValue ?? Value);
                    break;
                case ColorName.NightHorizonColor:
                    properties.m_NightHorizonColor = (Color)(CustomValue ?? Value);
                    break;
                case ColorName.EarlyNightZenithColor:
                    {
                        GradientColorKey[] c = properties.m_NightZenithColor.colorKeys;
                        GradientAlphaKey[] a = properties.m_NightZenithColor.alphaKeys;
                        c[0].color = c[3].color = properties.m_NightZenithColor.colorKeys[0].color;
                        c[1].color = c[2].color = (Color)(CustomValue ?? Value);
                        properties.m_NightZenithColor.SetKeys(c, a);
                    }
                    break;
                case ColorName.LateNightZenithColor:
                    {
                        GradientColorKey[] c = properties.m_NightZenithColor.colorKeys;
                        GradientAlphaKey[] a = properties.m_NightZenithColor.alphaKeys;
                        c[0].color = c[3].color = (Color)(CustomValue ?? Value);
                        c[1].color = c[2].color = properties.m_NightZenithColor.colorKeys[1].color;
                        properties.m_NightZenithColor.SetKeys(c, a);
                    }
                    break;
            }
        }

        public enum ColorName
        {
            MoonInnerCorona,
            MoonOuterCorona,

            SkyTint,
            NightHorizonColor,
            EarlyNightZenithColor,
            LateNightZenithColor,

            Count
        }
    }
}
