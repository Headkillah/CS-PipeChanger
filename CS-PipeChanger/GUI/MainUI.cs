using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace PipeChanger.GUI
{
    public class MainUI : UIComponent
    {
        public MainPanel MainPanel { get; private set; }
        private SavedInputKey MainKey { get; set; }
        private float _hitInterval = 0.25f;
        private float _lastKeybindHitTime = 0;

        public override void Awake()
        {
            base.Awake();
            if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("base Awake"); }
            
            var uiView = UIView.GetAView();
            if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("UIView.GetAView"); }
            
            MainPanel = (MainPanel)uiView.AddUIComponent(typeof(MainPanel));
            if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("MainPanel added to uiView"); }

            MainPanel.SetupMainPanel();
            if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("SetupMainPanel called"); }

            MainKey = ModSettings.instance.MainKey;
            if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("MainKey: " + MainKey); }
        }

        public override void Update()
        {
            if (!MainKey.IsPressed() ||  Time.time - _lastKeybindHitTime < _hitInterval) return;
            if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("MainKey pressed"); }

            if (!MainPanel.isVisible)
                MainPanel.Show();
            
            if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("MainPanel SHOW"); }

            else
               // MainPanel.Hide();
            
            if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("MainPanel HIDE"); }
            _lastKeybindHitTime = Time.time;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (MainPanel != null)
            {
                Destroy(MainPanel);
                MainPanel = null;
                MainKey = null;
            }
        }
    }
}
