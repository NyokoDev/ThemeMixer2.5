
#if DEBUG

using UnityEngine;

namespace ThemeMixer.UI.Abstraction.ColorPanel.ColorWheel
{
    public class ColorWheel : MonoBehaviour
    {
        public float wheelRadius = 100f;
        public float saturation = 1f;
        public float brightness = 1f;
        public static bool isVisible = false;

        private Color selectedColor = Color.white;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                ToggleVisibility();
            }
        }

        void OnGUI()
        {
            if (isVisible)
            {
                Rect colorSquareRect = new Rect(10, 10, wheelRadius * 2, wheelRadius * 2);
                Rect colorWheelRect = new Rect(20, 20, wheelRadius * 2, wheelRadius * 2);

                GUI.BeginGroup(colorSquareRect);
                {
                    // Draw color square background
                    GUI.Box(new Rect(0, 0, colorSquareRect.width, colorSquareRect.height), "");

                    // Draw color wheel
                    DrawColorWheel(colorWheelRect);

                    // Get selected color from the color wheel
                    Event currentEvent = Event.current;
                    Vector2 mousePosition = new Vector2(currentEvent.mousePosition.x - colorWheelRect.x, currentEvent.mousePosition.y - colorWheelRect.y);
                    if (colorWheelRect.Contains(currentEvent.mousePosition) && currentEvent.type == EventType.MouseDown)
                    {
                        float angle = Mathf.Atan2(mousePosition.y - wheelRadius, mousePosition.x - wheelRadius) * Mathf.Rad2Deg;
                        if (angle < 0)
                            angle += 360;
                        float hue = angle / 360f;
                        selectedColor = Color.HSVToRGB(hue, saturation, brightness);
                    }
                }
                GUI.EndGroup();

                // Display selected color
                GUI.Label(new Rect(10, colorSquareRect.height + 20, 200, 20), "Selected Color:");
                GUI.color = selectedColor;
                GUI.Box(new Rect(130, colorSquareRect.height + 20, 50, 20), "");
                GUI.color = Color.white;

                // Display RGB values
                GUI.Label(new Rect(10, colorSquareRect.height + 50, 200, 20), $"R: {selectedColor.r:F2}, G: {selectedColor.g:F2}, B: {selectedColor.b:F2}");
            }
        }

        void ToggleVisibility()
        {
            isVisible = !isVisible;
        }

        void DrawColorWheel(Rect rect)
        {
            Texture2D tex = new Texture2D((int)rect.width, (int)rect.height);
            for (int y = 0; y < rect.height; y++)
            {
                for (int x = 0; x < rect.width; x++)
                {
                    float dx = x - rect.width / 2;
                    float dy = y - rect.height / 2;
                    float angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
                    if (angle < 0)
                        angle += 360;
                    float distance = Mathf.Sqrt(dx * dx + dy * dy);
                    Color color = Color.HSVToRGB(angle / 360f, 1, 1);
                    if (distance <= wheelRadius)
                        tex.SetPixel(x, y, color);
                    else
                        tex.SetPixel(x, y, Color.clear);
                }
            }
            tex.Apply();
            GUI.DrawTexture(rect, tex);
        }
    }
}
#endif