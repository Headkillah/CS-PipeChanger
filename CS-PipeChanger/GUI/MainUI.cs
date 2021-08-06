using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace PipeChanger.GUI
{
    public class MainUI : UIComponent
    {
        public MainPanel MainPanel { get; private set; }
        private SavedInputKey MainKey { get; set; }

        private float _lastKeybindHitTime;
        private readonly float _hitInterval = 0.25f;

        public override void Awake()
        {
            base.Awake();
            var uiView = UIView.GetAView();
            MainPanel = (MainPanel)uiView.AddUIComponent(typeof(MainPanel));

            // Add a handler, so if the MainPanel is visible, some labels will be resetted
            MainPanel.eventVisibilityChanged += new PropertyChangedEventHandler<bool>(MainPanel.OnVisibilityChanged);
            MainPanel.Initialize();
            MainKey = ModSettings.instance.MainKey;
            if (ModSettings.VerboseLogging) Util.DebugPrint("MainKey: " + MainKey);
        }

        public override void Update()
        {
            if (!MainKey.IsPressed() || Time.time - _lastKeybindHitTime < _hitInterval) return;

            if (!MainPanel.isVisible)
                MainPanel.Show();
            else
                MainPanel.Hide();

            _lastKeybindHitTime = Time.time;
        }

        public override void OnDestroy()
        {
            if (MainPanel != null)
            {
                // Remove the Visibility Handler, that was initialized in the MainUI
                MainPanel.eventVisibilityChanged -= new PropertyChangedEventHandler<bool>(MainPanel.OnVisibilityChanged);
                Destroy(MainPanel);
                MainPanel = null;
                MainKey = null;
            }
        }
    }
}