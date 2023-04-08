using System.IO;
using System.Reflection;
using ColossalFramework.UI;
using UnityEngine;

namespace ThemeMixer.Resources
{
    public class UISprites
    {
        private static UITextureAtlas _atlas;
        public static UITextureAtlas Atlas
        {
            get
            {
                if (_atlas == null) _atlas = CreateAtlas();
                return _atlas;
            }
        }
        public static UITextureAtlas DefaultAtlas => UIView.library?.Get<OptionsMainPanel>("OptionsPanel")?.GetComponent<UIPanel>()?.atlas;

        public const string DragHandle = "DragHandle";
        public const string Blacklisted = "Blacklisted";
        public const string Star = "Star";
        public const string StarOutline = "StarOutline";

        public const string IconBorder = "IconBorder";
        public const string IconBorderHovered = "IconBorderHovered";
        public const string IconBorderPressed = "IconBorderPressed";
        public const string IconBorderFocused = "IconBorderFocused";

        public const string ThemesIcon = "ThemesIcon";
        public const string ThemesIconHovered = "ThemesIconHovered";
        public const string ThemesIconPressed = "ThemesIconPressed";
        public const string ThemesIconFocused = "ThemesIconFocused";

        public const string TerrainIcon = "TerrainIcon";
        public const string TerrainIconHovered = "TerrainIconHovered";
        public const string TerrainIconPressed = "TerrainIconPressed";
        public const string TerrainIconFocused = "TerrainIconFocused";

        public const string WaterIcon = "WaterIcon";
        public const string WaterIconHovered = "WaterIconHovered";
        public const string WaterIconPressed = "WaterIconPressed";
        public const string WaterIconFocused = "WaterIconFocused";

        public const string AtmosphereIcon = "AtmosphereIcon";
        public const string AtmosphereIconHovered = "AtmosphereIconHovered";
        public const string AtmosphereIconPressed = "AtmosphereIconPressed";
        public const string AtmosphereIconFocused = "AtmosphereIconFocused";

        public const string LutIcon = "LutIcon";
        public const string LutIconHovered = "LutIconHovered";
        public const string LutIconPressed = "LutIconPressed";
        public const string LutIconFocused = "LutIconFocused";

        public const string StructuresIcon = "StructuresIcon";
        public const string StructuresIconHovered = "StructuresIconHovered";
        public const string StructuresIconPressed = "StructuresIconPressed";
        public const string StructuresIconFocused = "StructuresIconFocused";

        public const string WeatherIcon = "WeatherIcon";
        public const string WeatherIconHovered = "WeatherIconHovered";
        public const string WeatherIconPressed = "WeatherIconPressed";
        public const string WeatherIconFocused = "WeatherIconFocused";

        public const string SettingsIcon = "SettingsIcon";
        public const string SettingsIconHovered = "SettingsIconHovered";
        public const string SettingsIconPressed = "SettingsIconPressed";
        public const string SettingsIconFocused = "SettingsIconFocused";

        public const string UIToggleIcon = "UIToggleIcon";
        public const string UIToggleIconHovered = "UIToggleIconHovered";
        public const string UIToggleIconPressed = "UIToggleIconPressed";
        public const string UIToggleIconFocused = "UIToggleIconFocused";

        public const string UndoIcon = "UndoIcon";
        public const string UndoIconHovered = "UndoIconHovered";
        public const string UndoIconPressed = "UndoIconPressed";

        public const string Swatch = "Swatch";


        private static readonly string[] _spriteNames = new string[] {
            DragHandle,
            Blacklisted,
            Star,
            StarOutline,

            IconBorder,
            IconBorderHovered,
            IconBorderPressed,
            IconBorderFocused,

            ThemesIcon,
            ThemesIconHovered,
            ThemesIconPressed,
            ThemesIconFocused,

            TerrainIcon,
            TerrainIconHovered,
            TerrainIconPressed,
            TerrainIconFocused,

            WaterIcon,
            WaterIconHovered,
            WaterIconPressed,
            WaterIconFocused,

            AtmosphereIcon,
            AtmosphereIconHovered,
            AtmosphereIconPressed,
            AtmosphereIconFocused,

            LutIcon,
            LutIconHovered,
            LutIconPressed,
            LutIconFocused,

            StructuresIcon,
            StructuresIconHovered,
            StructuresIconPressed,
            StructuresIconFocused,

            WeatherIcon,
            WeatherIconHovered,
            WeatherIconPressed,
            WeatherIconFocused,

            SettingsIcon,
            SettingsIconHovered,
            SettingsIconPressed,
            SettingsIconFocused,

            UIToggleIcon,
            UIToggleIconHovered,
            UIToggleIconPressed,
            UIToggleIconFocused,

            UndoIcon,
            UndoIconHovered,
            UndoIconPressed,

            Swatch
        };

        private static UITextureAtlas CreateAtlas()
        {
            Texture2D[] textures = new Texture2D[_spriteNames.Length];
            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = GetTextureFromAssemblyManifest(string.Concat(_spriteNames[i], ".png"));
            }
            return ResourceUtils.CreateAtlas("ThemeMixerAtlas", _spriteNames, textures);
        }

        private static Texture2D GetTextureFromAssemblyManifest(string file)
        {
            string path = string.Concat(Assembly.GetExecutingAssembly().GetName().Name, ".Resources.Files.", file);
            Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
            {
                byte[] array = new byte[manifestResourceStream.Length];
                manifestResourceStream.Read(array, 0, array.Length);
                texture2D.LoadImage(array);
            }
            return texture2D;
        }
    }
}
