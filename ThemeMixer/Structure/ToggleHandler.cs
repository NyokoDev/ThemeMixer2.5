using UnityEngine;
using ThemeMixer.UI;

namespace ThemeMixer.Structure
{
   
    internal class ToggleHandler
    {
        public static bool active;
        private bool hasRun;

        UIToggle toggle = new UIToggle();

        public void RunWhenActiveIsTrue()
        {
            if (active)
            {
                if (!hasRun)
                {
                    toggle.OnClickUUI();
                    Debug.Log("Theme Mixer 2.5: UnifiedUI MethodToRunWhenToggled ran.");
                    hasRun = true;
                }
            }
            else
            {
                hasRun = false; // Reset hasRun when active becomes false
            }
        }
    }
}
