using ColossalFramework.UI;
using UnityEngine;

namespace ThemeMixer.Resources
{
    public static class ResourceUtils
    {
        public static Texture2D MakeReadable(this Texture texture)
        {
            RenderTexture temporary = RenderTexture.GetTemporary(texture.width, texture.height, 0);
            Graphics.Blit(texture, temporary);
            Texture2D result = temporary.ToTexture2D();
            RenderTexture.ReleaseTemporary(temporary);
            return result;
        }

        public static bool IsTransparent(this Texture2D texture)
        {
            if (texture == null) return true;
            try
            {
                return ArePixelsTransparent(texture.GetPixels());
            }
            catch (UnityException)
            {
                Texture2D readableTexture = texture.MakeReadable();
                bool isTransparent = ArePixelsTransparent(readableTexture.GetPixels());
                Object.Destroy(readableTexture);
                return isTransparent;
            }

        }

        private static bool ArePixelsTransparent(Color[] pixels)
        {
            for (int i = 0; i < pixels.Length; i++)
                if (pixels[i].a != 0.0f)
                    return false;
            return true;
        }

        public static Texture2D ToTexture2D(this RenderTexture rt)
        {
            RenderTexture active = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D texture2D = new Texture2D(rt.width, rt.height);
            texture2D.ReadPixels(new Rect(0f, 0f, rt.width, rt.height), 0, 0);
            texture2D.Apply();
            RenderTexture.active = active;
            return texture2D;
        }

        public static Texture2D GetSpriteTexture(this UITextureAtlas atlas, string spriteName)
        {
            return atlas?.sprites?.Find(sprite => sprite?.name == spriteName)?.texture;
        }

        public static UITextureAtlas CreateAtlas(string atlasName, string[] spriteNames, Texture2D[] sprites)
        {
            UITextureAtlas textureAtlas = ScriptableObject.CreateInstance<UITextureAtlas>();

            Texture2D texture2D = new Texture2D(0, 0, TextureFormat.ARGB32, false);

            int maxSize = 4096;
            Rect[] regions = texture2D.PackTextures(sprites, 4, maxSize);
            Shader shader = Shader.Find("UI/Default UI Shader");
            Material material = new Material(shader);
            material.mainTexture = texture2D;
            textureAtlas.material = material;
            textureAtlas.name = atlasName;

            for (int i = 0; i < spriteNames.Length; i++)
            {
                UITextureAtlas.SpriteInfo item = new UITextureAtlas.SpriteInfo
                {
                    name = spriteNames[i],
                    texture = sprites[i],
                    region = regions[i],
                };

                textureAtlas.AddSprite(item);
            }

            return textureAtlas;
        }

        public static Texture2D ScaledCopy(this Texture2D src, float scale, FilterMode mode = FilterMode.Trilinear)
        {
            int width = Mathf.RoundToInt((float)src.width * scale);
            int height = Mathf.RoundToInt((float)src.height * scale);
            Rect texR = new Rect(0, 0, width, height);
            Gpu_scale(src, width, height, mode);
            Texture2D result = new Texture2D(width, height, TextureFormat.ARGB32, true);
            result.Resize(width, height);
            result.ReadPixels(texR, 0, 0, true);
            return result;
        }

        private static void Gpu_scale(Texture2D src, int width, int height, FilterMode fmode)
        {
            src.filterMode = fmode;
            src.Apply(true);
            RenderTexture rtt = new RenderTexture(width, height, 32);
            Graphics.SetRenderTarget(rtt);
            GL.LoadPixelMatrix(0, 1, 1, 0);
            GL.Clear(true, true, new Color(0, 0, 0, 0));
            Graphics.DrawTexture(new Rect(0, 0, 1, 1), src);
        }
    }
}
