using ColossalFramework.UI;
using ThemeMixer.Resources;
using ThemeMixer.UI.Abstraction;
using UnityEngine;

namespace ThemeMixer.UI
{
    public class DragHandle : PanelBase
    {
        private UIDragHandle _dragHandle;
        private UIPanel _panel;
        public delegate void DragEndEventHandler();
        public event DragEndEventHandler EventDragEnd;

        public override void Awake()
        {
            base.Awake();
            Setup("Drag Handle", 50.0f, 18.0f, 0);
            _dragHandle = AddUIComponent<UIDragHandle>();
            _dragHandle.isInteractive = true;
            _dragHandle.size = new Vector2(width, height);
            _dragHandle.relativePosition = new Vector2(0.0f, 0.0f);
            _dragHandle.eventMouseUp += OnDragEnd;

            _panel = AddUIComponent<UIPanel>();
            _panel.size = new Vector2(width, height);
            _panel.relativePosition = new Vector2(0.0f, 0.0f);
            _panel.atlas = UISprites.Atlas;
            _panel.backgroundSprite = UISprites.DragHandle;
            _panel.isInteractive = false;
            _panel.color = UIColorDark;
        }

        public override void Start()
        {
            base.Start();
            _dragHandle.target = parent.parent;
        }

        public override void OnDestroy()
        {
            _dragHandle.eventMouseUp -= OnDragEnd;

            base.OnDestroy();
        }

        private void OnDragEnd(UIComponent component, UIMouseEventParameter eventParam)
        {
            EventDragEnd?.Invoke();
        }
    }
}
