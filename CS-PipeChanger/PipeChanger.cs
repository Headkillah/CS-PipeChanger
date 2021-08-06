using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using PipeChanger.GUI;
using System;

namespace PipeChanger
{
    public class PipeChanger : IUserMod
    {
        #region Fields

        internal const string MODNAME = "Pipe Changer";  //the name of your mod, make it constant in case you use it more then once.
        internal const string MODDESC = "Change Water to Heat Pipes and vice versa";  //keep it short.
        public const string MODVERSION = "1.0"; // Variable zum speichern der Version des Mods
        public static string LogFile = "PipeChanger_Log.txt";

        /// <summary>
        /// Implements the REQUIRED Name property - The game will request this of your mod.
        /// </summary>
        public string Name => MODNAME;

        /// <summary>
        /// Implements the REQUIRED Description property - The game will request this of your mod.
        /// </summary>
        public string Description => MODDESC;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Optional
        /// This will fire when your mod is Enabled upon the game starting if your mod is already marked enabled by the user.
        /// or it will fire later when the user 'enables' it.
        /// </summary>
        public void OnEnabled()
        {
            if (!SteamHelper.IsDLCOwned(SteamHelper.DLC.SnowFallDLC)) //ist das benötigte DLC vorhanden?
            {
                // Falls NEIN eine Meldung im Debugfenster / Debuglog ausgeben und...
                Util.DebugPrint("Snowfall is not installed, aborting.");
                Util.WriteLog("Snowfall is not installed, aborting.");

                //... ein PopUp Fenster (Modal) über die Loadingmanager IntroLoaded Klasse aufrufen
                Singleton<LoadingManager>.instance.m_introLoaded += DisplayError;
            }
            else
            {
                Util.DebugPrint("ModSettings.Ensure");
                ModSettings.Ensure();
            }
        }

        /// <summary>
        /// Optional
        /// This will fire when either the game shuts down and your dll is unloaded.
        /// or upon your mod being disabled and your mod dll unloaded.
        ///
        /// </summary>
        public void OnDisabled()
        {
        }

        /// <summary>
        /// Display an ErrorPanel, if the needed DLC isn´t present
        /// </summary>
        private void DisplayError()
        {
            try
            {
                // Show warning, if the required Snowfall DLC isn´t enabled
                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                    "Missing dependency",
                    Name + " requires the Snowfall DLC. Please enable / buy this DLC if you wan´t to use the features of the mod!",
                    false);
                return;
            }
            catch (Exception ex)
            {
                Util.LogException(ex);
            }
        }

        public void OnSettingsUI(UIHelper helper)
        {
            new SettingsUI().BuildUI(helper);
            if (ModSettings.VerboseLogging) { Util.DebugPrint("OnSettingsUI"); }
        }

        #endregion Methods
    }
}