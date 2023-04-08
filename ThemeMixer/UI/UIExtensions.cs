using ColossalFramework.UI;
using ThemeMixer.UI.Abstraction;
using UnityEngine;

namespace ThemeMixer.UI
{
    public static class UIExtensions
    {
        public static UIPanel CreateSpace(this PanelBase parent, float width, float height)
        {
            var panel = parent.AddUIComponent<UIPanel>();
            panel.size = new Vector2(width, height);
            return panel;
        }

        public static void FitString(this UILabel label)
        {
            using (UIFontRenderer fontRenderer = label.ObtainRenderer())
            {
                float p2u = label.GetUIView().PixelsToUnits();
                var characterWidths = fontRenderer.GetCharacterWidths(label.text);
                float totalSize = label.padding.left + label.padding.right;
                for (var i = 0; i < characterWidths.Length; i++)
                {
                    totalSize += characterWidths[i] / p2u;
                    if (!(totalSize > label.width)) continue;
                    label.tooltip = label.text;
                    label.text = string.Concat(label.text.Substring(0, i - 1), "...");
                    break;
                }
            }
        }
    }
}
