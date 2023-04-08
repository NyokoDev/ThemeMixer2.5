using System.Xml.Serialization;
using ColossalFramework.Packaging;
using JetBrains.Annotations;
using UnityEngine;

namespace ThemeMixer.Themes.Abstraction
{
    public abstract class TexturePartBase : ThemePartBase
    {
        [XmlIgnore]
        public Texture2D Texture { get; set; }

        [UsedImplicitly]
        protected TexturePartBase() { }

        protected TexturePartBase(string themeID) : base(themeID) { }

        public bool SetTexture(Package.Asset asset)
        {
            if (asset == null) return false;
            Texture2D oldTexture = Texture;
            Texture = asset.Instantiate<Texture2D>();
            if (Texture == null)
            {
                Texture = oldTexture;
                return false;
            }
            Texture.anisoLevel = 8;
            Texture.filterMode = FilterMode.Trilinear;
            Texture.Apply();
            if (oldTexture != null) Object.Destroy(oldTexture);
            return true;
        }

        public bool SetTexture(Texture2D texture)
        {
            Texture = texture;
            return Texture != null;
        }

        public bool SetTexture(Texture texture)
        {
            Texture = (Texture2D)texture;
            return Texture != null;
        }
    }
}
