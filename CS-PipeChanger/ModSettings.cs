using ColossalFramework;
using UnityEngine;

namespace PipeChanger {
    public class ModSettings: Singleton<ModSettings> {
        private const int DEFAULT_MENU_POS_X = 250;
        private const int DEFAULT_MENU_POS_Y = 20;
        private static string FILE_NAME { get; } = "PipeChangerConfig";                
        public SavedInputKey MainKey = new SavedInputKey("PipeChangerMenu", FILE_NAME, KeyCode.Keypad5, true, false, false, true);

        // public SavedBool DEBUG_LOG_ON = new SavedBool("DebugLogging", FILE_NAME, false);
        public bool DEBUG_LOG_ON = true;

        public SavedInt MenuPosX = new SavedInt("PipeChangerPanelPosX", FILE_NAME, DEFAULT_MENU_POS_X, true);
        public SavedInt MenuPosY = new SavedInt("PipeChangerPanelPosY", FILE_NAME, DEFAULT_MENU_POS_Y, true);

        private static SettingsFile _settingsFile;

        static ModSettings() {
            TryCreateModConfig();
        }

        internal void ResetMenuPosition()
        {
            MenuPosX.value = DEFAULT_MENU_POS_X;
            MenuPosY.value = DEFAULT_MENU_POS_Y;

            if (Loader.MainUi && Loader.MainUi.MainPanel)
            {                
                Loader.MainUi.MainPanel.ForceUpdateMenuPosition();
                if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("ForceUpdateMenuPosition"); }
            }
        }

        private static void TryCreateModConfig() {
            if (GameSettings.FindSettingsFileByName(FILE_NAME) == null)
                GameSettings.AddSettingsFile(new SettingsFile {fileName = FILE_NAME});

            _settingsFile = GameSettings.FindSettingsFileByName(FILE_NAME);
            if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("Game settings loaded"); }
        }
    }
}