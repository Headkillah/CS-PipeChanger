using ColossalFramework;
using UnityEngine;

namespace PipeChanger
{
    public class ModSettings : Singleton<ModSettings>
    {
        private static SettingsFile _settingsFile;
        private static string FileName { get; } = "PipeChangerConfig";

        private const int DEFAULT_MENU_POS_X = 350;
        private const int DEFAULT_MENU_POS_Y = 120;

        public SavedInputKey MainKey = new SavedInputKey("PipeChangerMenu", FileName, KeyCode.D, false, false, true, true);
        public static readonly SavedBool VerboseLogging = new SavedBool(nameof(VerboseLogging), FileName, false);
        public SavedInt MenuPosX = new SavedInt("PipeChangerPanelPosX", FileName, DEFAULT_MENU_POS_X, true);
        public SavedInt MenuPosY = new SavedInt("PipeChangerPanelPosY", FileName, DEFAULT_MENU_POS_Y, true);

        static ModSettings()
        {
            TryCreateModConfig();
        }

        internal void ResetMenuPosition()
        {
            MenuPosX.value = DEFAULT_MENU_POS_X;
            MenuPosY.value = DEFAULT_MENU_POS_Y;

            if (Loader.MainUi && Loader.MainUi.MainPanel)
            {
                Loader.MainUi.MainPanel.ForceUpdateMenuPosition();
                if (ModSettings.VerboseLogging) { Util.DebugPrint("ForceUpdateMenuPosition"); }
            }
        }

        private static void TryCreateModConfig()
        {
            if (GameSettings.FindSettingsFileByName(FileName) == null)
                GameSettings.AddSettingsFile(new SettingsFile { fileName = FileName });

            _settingsFile = GameSettings.FindSettingsFileByName(FileName);
            if (ModSettings.VerboseLogging) { Util.DebugPrint("Game settings loaded"); }
        }
    }
}