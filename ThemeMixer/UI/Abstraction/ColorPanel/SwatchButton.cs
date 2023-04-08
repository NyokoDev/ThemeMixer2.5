using ColossalFramework.UI;
using ThemeMixer.Resources;
using UnityEngine;

namespace ThemeMixer.UI.Abstraction.ColorPanel
{
    public class SwatchButton : UIButton
    {
        public Color Swatch;
        public delegate void SwatchClickedEventHandler(Color color, UIMouseEventParameter eventParam, UIComponent component);
        public event SwatchClickedEventHandler EventSwatchClicked;

        public void Build(Color swatchButtonColor)
        {
            size = new Vector2(19.0f, 19.0f);
            atlas = UISprites.Atlas;
            normalBgSprite = UISprites.Swatch;
            hoveredColor = new Color32((byte)Mathf.Min((swatchButtonColor.r + 32), 255), (byte)Mathf.Min((swatchButtonColor.g + 32), 255), (byte)Mathf.Min((swatchButtonColor.b + 32), 255), 255);
            pressedColor = new Color32((byte)Mathf.Min((swatchButtonColor.r + 64), 255), (byte)Mathf.Min((swatchButtonColor.g + 64), 255), (byte)Mathf.Min((swatchButtonColor.b + 64), 255), 255);
            focusedColor = swatchButtonColor;
            color = swatchButtonColor;
            Swatch = swatchButtonColor;
        }

        public override void OnDestroy()
        {
            EventSwatchClicked = null;
            eventClicked -= OnSwatchClicked;
            base.OnDestroy();
        }

        public override void Awake()
        {
            eventClicked += OnSwatchClicked;
        }

        private void OnSwatchClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            EventSwatchClicked?.Invoke(Swatch, eventParam, component);
        }
    }
}
