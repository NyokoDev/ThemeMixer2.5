using ColossalFramework.UI;
using UnityEngine;

namespace ThemeMixer.UI
{
    public class UIRoot : UIPanel
    {
        public override void Start()
        {
            base.Start();
            relativePosition = Vector3.zero;
            size = new Vector2(1.0f, 1.0f);
        }
    }
}
