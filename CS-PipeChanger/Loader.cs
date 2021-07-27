using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using PipeChanger.GUI;
using System;
using UnityEngine;

namespace PipeChanger
{
    public class Loader : LoadingExtensionBase
    {
        #region Fields
                
        public static MainUI MainUi { get; private set; }

        #endregion Fields

        #region Methods
        
        /// <summary>
        /// Optional
        /// This core function will get called just after the Level - aka your map has been fully loaded.
        /// That means the game data has all been read from your file and the simulation is ready to go.
        /// </summary>
        /// <param name="mode">a LoadMode enum (ie newgame,newmap,loadgame,loadmap,newasset,loadassett)</param>
        public override void OnLevelLoaded(LoadMode mode)
        {
           try
            {
                base.OnLevelLoaded(mode);

                    // only setup gui when in a real game, not in the asset editor
                    if (mode == LoadMode.NewGame ||
                        mode == LoadMode.LoadGame ||
                        mode == LoadMode.LoadMap ||
                        mode == LoadMode.NewMap ||
                        mode == LoadMode.NewScenarioFromGame)
                    {
                        if (!MainUi)
                        {
                        if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("OnLevelLoaded: Creating Pipe Changer MainUI."); }
                        MainUi = (MainUI) UIView.GetAView().AddUIComponent(typeof(MainUI));
                            if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("OnLevelLoaded: MainUI component created"); }
                        }
                }             
            }

            catch (Exception ex)
            { Util.LogException(ex); }
        }

        /// <summary>
        /// Optional
        /// This function gets called by the game when you've asked to unload a map. Either because you are going to the main menu
        /// or because you are in a map, but have asked to load another map.
        /// </summary>
        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();  //see prior comments same concept.

            try
            {
                if (MainUi)
                {
                    UnityEngine.Object.Destroy(MainUi);
                    MainUi = null;
                    if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("Pipe Changer MainUI destroyed."); }
                }
            }
            catch (Exception ex)
            {
                Util.LogException(ex);
            }          
        } 

        #endregion Methods
    }
}
