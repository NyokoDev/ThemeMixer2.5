using JetBrains.Annotations;
using ThemeMixer.Themes.Abstraction;

namespace ThemeMixer.Themes.Weather
{
    public class WeatherValue : ThemePartBase
    {
        public ValueName Name;

        [UsedImplicitly]
        public WeatherValue() { }

        public WeatherValue(ValueName valueName)
        {
            Name = valueName;
        }

        public WeatherValue(string themeID, ValueName floatName) : base(themeID)
        {
            Name = floatName;
        }

        protected override bool SetFromTheme()
        {
            MapThemeMetaData metaData = ThemeManager.GetTheme(ThemeID);
            if (metaData == null) return false;
            switch (Name)
            {
                case ValueName.MinTemperatureDay:
                    return SetValue(metaData.minTemperatureDay);
                case ValueName.MaxTemperatureDay:
                    return SetValue(metaData.maxTemperatureDay);
                case ValueName.MinTemperatureNight:
                    return SetValue(metaData.minTemperatureNight);
                case ValueName.MaxTemperatureNight:
                    return SetValue(metaData.maxTemperatureNight);
                case ValueName.MinTemperatureRain:
                    return SetValue(metaData.minTemperatureRain);
                case ValueName.MaxTemperatureRain:
                    return SetValue(metaData.maxTemperatureRain);
                case ValueName.MinTemperatureFog:
                    return SetValue(metaData.minTemperatureFog);
                case ValueName.MaxTemperatureFog:
                    return SetValue(metaData.maxTemperatureFog);
                case ValueName.RainProbabilityDay:
                    return SetValue(metaData.rainProbabilityDay);
                case ValueName.RainProbabilityNight:
                    return SetValue(metaData.rainProbabilityNight);
                case ValueName.FogProbabilityDay:
                    return SetValue(metaData.fogProbabilityDay);
                case ValueName.FogProbabilityNight:
                    return SetValue(metaData.fogProbabilityNight);
                case ValueName.NorthernLightsProbability:
                    return SetValue(metaData.northernLightsProbability);
                default: return false;
            }
        }

        protected override bool SetFromProperties()
        {
            WeatherProperties properties = WeatherManager.instance.m_properties;
            switch (Name)
            {
                case ValueName.MinTemperatureDay:
                    return SetValue(properties.m_minTemperatureDay);
                case ValueName.MaxTemperatureDay:
                    return SetValue(properties.m_maxTemperatureDay);
                case ValueName.MinTemperatureNight:
                    return SetValue(properties.m_minTemperatureNight);
                case ValueName.MaxTemperatureNight:
                    return SetValue(properties.m_maxTemperatureNight);
                case ValueName.MinTemperatureRain:
                    return SetValue(properties.m_minTemperatureRain);
                case ValueName.MaxTemperatureRain:
                    return SetValue(properties.m_maxTemperatureRain);
                case ValueName.MinTemperatureFog:
                    return SetValue(properties.m_minTemperatureFog);
                case ValueName.MaxTemperatureFog:
                    return SetValue(properties.m_maxTemperatureFog);
                case ValueName.RainProbabilityDay:
                    return SetValue(properties.m_rainProbabilityDay);
                case ValueName.RainProbabilityNight:
                    return SetValue(properties.m_rainProbabilityNight);
                case ValueName.FogProbabilityDay:
                    return SetValue(properties.m_fogProbabilityDay);
                case ValueName.FogProbabilityNight:
                    return SetValue(properties.m_fogProbabilityNight);
                case ValueName.NorthernLightsProbability:
                    return SetValue(properties.m_northernLightsProbability);
                default: return false;
            }
        }

        protected override void LoadValue()
        {
            WeatherProperties properties = WeatherManager.instance.m_properties;
            switch (Name)
            {
                case ValueName.MinTemperatureDay:
                    properties.m_minTemperatureDay = (float)(CustomValue ?? Value);
                    break;
                case ValueName.MaxTemperatureDay:
                    properties.m_maxTemperatureDay = (float)(CustomValue ?? Value);
                    break;
                case ValueName.MinTemperatureNight:
                    properties.m_minTemperatureNight = (float)(CustomValue ?? Value);
                    break;
                case ValueName.MaxTemperatureNight:
                    properties.m_maxTemperatureNight = (float)(CustomValue ?? Value);
                    break;
                case ValueName.MinTemperatureRain:
                    properties.m_minTemperatureRain = (float)(CustomValue ?? Value);
                    break;
                case ValueName.MaxTemperatureRain:
                    properties.m_maxTemperatureRain = (float)(CustomValue ?? Value);
                    break;
                case ValueName.MinTemperatureFog:
                    properties.m_minTemperatureFog = (float)(CustomValue ?? Value);
                    break;
                case ValueName.MaxTemperatureFog:
                    properties.m_maxTemperatureFog = (float)(CustomValue ?? Value);
                    break;
                case ValueName.RainProbabilityDay:
                    properties.m_rainProbabilityDay = (int)(CustomValue ?? Value);
                    break;
                case ValueName.RainProbabilityNight:
                    properties.m_rainProbabilityNight = (int)(CustomValue ?? Value);
                    break;
                case ValueName.FogProbabilityDay:
                    properties.m_fogProbabilityDay = (int)(CustomValue ?? Value);
                    break;
                case ValueName.FogProbabilityNight:
                    properties.m_fogProbabilityNight = (int)(CustomValue ?? Value);
                    break;
                case ValueName.NorthernLightsProbability:
                    properties.m_northernLightsProbability = (int)(CustomValue ?? Value);
                    break;
            }
        }

        public enum ValueName
        {
            MinTemperatureDay,
            MaxTemperatureDay,
            MinTemperatureNight,
            MaxTemperatureNight,
            MinTemperatureRain,
            MaxTemperatureRain,
            MinTemperatureFog,
            MaxTemperatureFog,

            RainProbabilityDay,
            RainProbabilityNight,
            FogProbabilityDay,
            FogProbabilityNight,
            NorthernLightsProbability,

            Count
        }
    }
}
