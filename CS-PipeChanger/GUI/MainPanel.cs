using System;
using UnityEngine;
using ColossalFramework;
using ColossalFramework.UI;
using ColossalFramework.Globalization;

namespace PipeChanger.GUI
{
    public class MainPanel : UIPanel
    {        
        private static readonly float _panelWidth = 370f; //The width of our gui in pixel points
        private static readonly float _panelHeight = 350f; // the height of our gui
        private static readonly float _titleHeight = 40f;

        private UIPanel _mainPanel;
       // private UIPanel _pnlContainer;
        private UIDragHandle _dragHandle;

        private UILabel _lblpipeCount;
        private UILabel _lblupgradeCost;
        private UILabel _lblupgradeResult;
        private UILabel _lblwaterpipeCount;
        private UILabel _lbldowngradeCost;
        private UILabel _lbldowngradeResult;
        private UILabel _lblMoneyAmount;    
        private UILabel _lblTitle;
        
        private UIButton _btnUpgrade;
        private UIButton _btnDowngrade;
        private UIButton _btnClose;

        /// <summary>
        /// Creates the Mainpanel / Close button / DragHandler
        /// </summary>
        public void SetupMainPanel()
        {
            //Main Panel
            if (_mainPanel != null) { _mainPanel.OnDestroy(); }

            _mainPanel = AddUIComponent<UIPanel>();
            _mainPanel.name = "PipeChangerMainPanel";
            _mainPanel.backgroundSprite = "UnlockingPanel2";
            _mainPanel.size = new Vector2(_panelWidth, _panelHeight);
            _mainPanel.color = new Color32(75, 75, 135, 255);
            _mainPanel.canFocus = true;
            _mainPanel.isInteractive = true;
            _mainPanel.isVisible = false;

            width = _panelWidth; //UIComponent
            height = 320; //UIComponent

            relativePosition = new Vector3(ModSettings.instance.MenuPosX, ModSettings.instance.MenuPosY);
            _mainPanel.relativePosition = Vector3.zero;

            //Title Label
            //_lblTitle = SamsamTS.UIUtils.CreateTitleLabel(_mainPanel, "TitleLabel", "Pipe Changer", new Vector3((base.width - 240f), -15f));
            _lblTitle = SamsamTS.UIUtils.CreateTitleLabel(_mainPanel, "TitleLabel", "Pipe Changer", new Vector3(100, 12));
            if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("Title Label created"); }

            //DragHandler
            SamsamTS.UIUtils.CreateDragHandle(_mainPanel, _panelWidth, _titleHeight);
            if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("DragHandler created"); }

            //Close Button           
            _btnClose = SamsamTS.UIUtils.CreateCloseButton(_mainPanel);
            _btnClose.eventClick += CloseButtonClick;
            if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("CloseButton created"); }

            SetUpControls();
        }

        /// <summary>
        /// SetUp all Controls (Button/Panel/Label)
        /// </summary>
        private void SetUpControls()
        {
            try
            {
                //UIPanel pnlContainer = base.AddUIComponent<UIPanel>();
                //pnlContainer.name = "ContainerPanel";
                //pnlContainer.anchor = UIAnchorStyle.Right | UIAnchorStyle.Left | UIAnchorStyle.Top;
                //pnlContainer.transform.localPosition = Vector3.zero;
                //pnlContainer.width = base.width;
                //pnlContainer.height = base.height;
                //pnlContainer.autoLayout = false;
                // pnlContainer.autoLayoutDirection = LayoutDirection.Vertical;
                // pnlContainer.autoLayoutPadding = new RectOffset(0, 0, 0, 1);
                // pnlContainer.autoLayoutStart = LayoutStart.TopLeft;
                // pnlContainer.relativePosition = new Vector3(0f, 50f);

                _lblupgradeCost = SamsamTS.UIUtils.CreateLabel(_mainPanel, "UpgradeCostLabel", "Upgrade cost:", new Vector2(0, 30));
                _lblupgradeCost.textColor = Color.white;
                _lblupgradeCost.autoSize = false;
                _lblupgradeCost.height = 30;
                _lblupgradeCost.width = 130;
                _lblupgradeCost.verticalAlignment = UIVerticalAlignment.Middle;

                _lblMoneyAmount = SamsamTS.UIUtils.CreateLabel(_mainPanel, "MoneyFormatLabel", "0.00", new Vector2(0, 180));
                _lblMoneyAmount.textColor = Color.white;
                _lblMoneyAmount.autoSize = false;
                _lblMoneyAmount.height = 30;
                _lblMoneyAmount.width = 140;
                _lblMoneyAmount.textAlignment = UIHorizontalAlignment.Right;
                _lblMoneyAmount.verticalAlignment = UIVerticalAlignment.Middle;

                _lblpipeCount = SamsamTS.UIUtils.CreateLabel(_mainPanel, "PipeCountLabel", "Pipe count:", new Vector2(0, 110f));
                _lblpipeCount.autoSize = false;
                _lblpipeCount.height = 30f;
                _lblpipeCount.width = 100f;
                _lblpipeCount.verticalAlignment = UIVerticalAlignment.Middle;

                _btnUpgrade = SamsamTS.UIUtils.CreateButton(_mainPanel, "CalculateButton", "Calculate", new Vector2(60, 110));
                _btnUpgrade.width = 75f;
                _btnUpgrade.height = 22f;
                _btnUpgrade.eventClick += (c, p) => Singleton<SimulationManager>.instance.AddAction(new System.Action(this.CalculateUpgrade));





                // From here to end everythin for downgrading...



                if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("[CreatePanel]: WPU panel created."); }
            }
            catch (Exception ex)
            {
                Util.LogException(ex);
            }
        }

        protected override void OnPositionChanged()
        {
            base.OnPositionChanged();
           if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("OnPositionChanged called"); }

            bool posChanged = ModSettings.instance.MenuPosX != (int)absolutePosition.x
                              || ModSettings.instance.MenuPosY != (int)absolutePosition.y;
            if (posChanged)
            {
                Vector2 resolution = GetUIView().GetScreenResolution();

                absolutePosition = new Vector2(
                    Mathf.Clamp(absolutePosition.x, 0, resolution.x - width),
                    Mathf.Clamp(absolutePosition.y, 0, resolution.y - height));

                if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("absolutePosition: " + absolutePosition); }

                ModSettings.instance.MenuPosX.value = (int)absolutePosition.x;
                ModSettings.instance.MenuPosY.value = (int)absolutePosition.y;
            }
        }

        private void CloseButtonClick(UIComponent component, UIMouseEventParameter eventparam)
        {
            eventparam.Use();
            isVisible = false;
        }

        internal void ForceUpdateMenuPosition()
        {
            absolutePosition = new Vector3(ModSettings.instance.MenuPosX, ModSettings.instance.MenuPosY);
            if (ModSettings.instance.DEBUG_LOG_ON) { Util.DebugPrint("Position reseted. absolutePosition: " + absolutePosition); }
        }

        private void GetSegmentControlPoints(int segmentIndex, out NetTool.ControlPoint startPoint, out NetTool.ControlPoint middlePoint, out NetTool.ControlPoint endPoint)
        {
            //Logger.DebugPrint("GetSegmentControlPoints");

            NetManager instance = Singleton<NetManager>.instance;
            NetInfo info = instance.m_segments.m_buffer[segmentIndex].Info;
            startPoint.m_node = instance.m_segments.m_buffer[segmentIndex].m_startNode;
            startPoint.m_segment = 0;
            startPoint.m_position = instance.m_nodes.m_buffer[startPoint.m_node].m_position;
            startPoint.m_direction = instance.m_segments.m_buffer[segmentIndex].m_startDirection;
            startPoint.m_elevation = instance.m_nodes.m_buffer[startPoint.m_node].m_elevation;
            startPoint.m_outside = (instance.m_nodes.m_buffer[startPoint.m_node].m_flags & NetNode.Flags.Outside) > NetNode.Flags.None;
            endPoint.m_node = instance.m_segments.m_buffer[segmentIndex].m_endNode;
            endPoint.m_segment = 0;
            endPoint.m_position = instance.m_nodes.m_buffer[endPoint.m_node].m_position;
            endPoint.m_direction = -instance.m_segments.m_buffer[segmentIndex].m_endDirection;
            endPoint.m_elevation = instance.m_nodes.m_buffer[endPoint.m_node].m_elevation;
            endPoint.m_outside = (instance.m_nodes.m_buffer[endPoint.m_node].m_flags & NetNode.Flags.Outside) > NetNode.Flags.None;
            middlePoint.m_node = 0;
            middlePoint.m_segment = (ushort)segmentIndex;
            middlePoint.m_position = startPoint.m_position + ((Vector3)(startPoint.m_direction * (info.GetMinNodeDistance() + 1f)));
            middlePoint.m_direction = startPoint.m_direction;
            middlePoint.m_elevation = Mathf.Lerp(startPoint.m_elevation, endPoint.m_elevation, 0.5f);
            middlePoint.m_outside = false;
        }

        private void CalculateUpgrade()
        {
            try
            {
                if (ModSettings.instance.DEBUG_LOG_ON)
                {
                    Util.DebugPrint("CalculateUpgrade");
                }

                int totalCost = 0;
                float factor = 0.01f;

                ToolBase.ToolErrors none = ToolBase.ToolErrors.None;
                int num2 = this.UpgradeWaterToHeat(true, out totalCost, out none);
                this._lblMoneyAmount.text = (totalCost * factor).ToString(ColossalFramework.Globalization.Locale.Get("MONEY_FORMAT"), LocaleManager.cultureInfo);


                if (Singleton<EconomyManager>.instance.PeekResource(EconomyManager.Resource.Construction, totalCost) != totalCost)
                {
                    this._lblMoneyAmount.textColor = Color.red;
                }
                else
                {
                    this._lblMoneyAmount.textColor = Color.green;
                }
                this._lblpipeCount.text += num2.ToString();

            } 

            catch (Exception ex)
            { 
              Util.LogException(ex);
            } 
        }

        private void CalculateDowngrade()
        {
            try
            {
                if (ModSettings.instance.DEBUG_LOG_ON)
                {
                    Util.DebugPrint("CalculateDowngrade");
                }

                int totalCost = 0;
                float factor = 0.01f;

                ToolBase.ToolErrors none = ToolBase.ToolErrors.None;
                int num2 = this.DowngradeHeatToWater(true, out totalCost, out none);
                this._lbldowngradeCost.text = (totalCost * factor).ToString(ColossalFramework.Globalization.Locale.Get("MONEY_FORMAT"), LocaleManager.cultureInfo);

                if (Singleton<EconomyManager>.instance.PeekResource(EconomyManager.Resource.Construction, totalCost) != totalCost)
                {
                    this._lbldowngradeCost.textColor = Color.red;
                }
                else
                {
                    this._lbldowngradeCost.textColor = Color.green;
                }
                this._lblwaterpipeCount.text = num2.ToString();

            }

            catch (Exception ex)
            {
                Util.LogException(ex);
            } 
        }

        private void UpgradePipes()
        {
            try
            {
                if (ModSettings.instance.DEBUG_LOG_ON)
                {
                    Util.DebugPrint("UpgradePipes");
                }

                int totalCost = 0;
                ToolBase.ToolErrors none = ToolBase.ToolErrors.None;
                int num2 = this.UpgradeWaterToHeat(false, out totalCost, out none);
                string str = "";
                if (none != ToolBase.ToolErrors.None)
                {
                    str = " Not enough money.";
                    this._lblupgradeResult.textColor = Color.red;
                }
                else
                {
                    this._lblupgradeResult.textColor = Color.green;
                }

                object[] objArray1 = new object[] { "Upgraded ", num2, " pipes.", str };
                this._lblupgradeResult.text = string.Concat(objArray1);

                if (ModSettings.instance.DEBUG_LOG_ON)
                {
                    Util.DebugPrint(PipeChanger.MODNAME + " " + PipeChanger.MODVERSION + " All pipes upgraded");
                }
                }

            catch (Exception ex)
            {
                Util.LogException(ex);
            }
        }

        private void DowngradePipes()
        {
            try
            {
                if (ModSettings.instance.DEBUG_LOG_ON)
                {
                    Util.DebugPrint("DowngradePipes");
                }

                int totalCost = 0;
                ToolBase.ToolErrors none = ToolBase.ToolErrors.None;
                int num2 = this.DowngradeHeatToWater(false, out totalCost, out none);
                string str = "";
                if (none != ToolBase.ToolErrors.None)
                {
                    str = " Not enough money.";
                    this._lbldowngradeResult.textColor = Color.red;
                }
                else
                {
                    this._lbldowngradeResult.textColor = Color.green;
                }
                object[] objArray1 = new object[] { "Downgraded ", num2, " pipes.", str };
                this._lbldowngradeResult.text = string.Concat(objArray1);

                if (ModSettings.instance.DEBUG_LOG_ON)
                {
                    Util.DebugPrint(PipeChanger.MODNAME + " " + PipeChanger.MODVERSION + " All pipes downgraded to water pipes");
                }
            }

            catch (Exception ex)
            {
                Util.LogException(ex);
            }
        }

        private int UpgradeWaterToHeat(bool test, out int totalCost, out ToolBase.ToolErrors errors)
        {
            if (ModSettings.instance.DEBUG_LOG_ON)
            {
                Util.DebugPrint("UpgradeWaterToHeat");
            }

            int num = 0;
            totalCost = 0;
            errors = ToolBase.ToolErrors.None;
            NetInfo info = PrefabCollection<NetInfo>.FindLoaded("Heating Pipe");
            if (info == null)
            {
                if (ModSettings.instance.DEBUG_LOG_ON)
                {
                    Util.DebugPrint("Couldn't find Heating Pipe, aborting.");
                }
                            
                return num;
            }
            NetSegment[] buffer = Singleton<NetManager>.instance.m_segments.m_buffer;
            for (int i = 0; i < buffer.Length; i++)
            {
                NetSegment segment = buffer[i];
                if (((segment.Info != null) && (segment.Info.category == "WaterServices")) && (segment.m_flags != NetSegment.Flags.None))
                {
                    NetTool.ControlPoint point;
                    NetTool.ControlPoint point2;
                    NetTool.ControlPoint point3;
                    this.GetSegmentControlPoints(i, out point, out point2, out point3);
                    bool visualize = false;
                    bool autoFix = true;
                    bool needMoney = true;
                    bool invert = false;
                    ushort node = 0;
                    ushort num4 = 0;
                    int cost = 0;
                    int productionRate = 0;
                    errors = NetTool.CreateNode(info, point, point2, point3, NetTool.m_nodePositionsSimulation, 0x3e8, test, visualize, autoFix, needMoney, invert, false, 0, out node, out num4, out cost, out productionRate);
                    if ((errors == ToolBase.ToolErrors.None) | test)
                    {
                        num++;
                        totalCost += cost;
                    }
                    else if ((errors & ToolBase.ToolErrors.NotEnoughMoney) != ToolBase.ToolErrors.None)
                    {
                        return num;
                    }
                }
            }
            return num;
        }

        private int DowngradeHeatToWater(bool test, out int totalCost, out ToolBase.ToolErrors errors)
        {
            if (ModSettings.instance.DEBUG_LOG_ON)
            {
                Util.DebugPrint("DowngradeHeatToWater");
            }

            int num = 0;
            totalCost = 0;
            errors = ToolBase.ToolErrors.None;
            NetInfo info = PrefabCollection<NetInfo>.FindLoaded("Water Pipe");

            if (info == null)
            {
                if (ModSettings.instance.DEBUG_LOG_ON)
                {
                    Util.DebugPrint("Couldn't find Water Pipe, aborting.");
                }
             
                return num;
            }
            NetSegment[] buffer = Singleton<NetManager>.instance.m_segments.m_buffer;
            for (int i = 0; i < buffer.Length; i++)
            {
                NetSegment segment = buffer[i];
                if (((segment.Info != null) && (segment.Info.category == "WaterHeating")) && (segment.m_flags != NetSegment.Flags.None))
                {
                    NetTool.ControlPoint point;
                    NetTool.ControlPoint point2;
                    NetTool.ControlPoint point3;
                    this.GetSegmentControlPoints(i, out point, out point2, out point3);
                    bool visualize = false;
                    bool autoFix = true;
                    bool needMoney = true;
                    bool invert = false;
                    ushort node = 0;
                    ushort num4 = 0;
                    int cost = 0;
                    int productionRate = 0;
                    errors = NetTool.CreateNode(info, point, point2, point3, NetTool.m_nodePositionsSimulation, 0x3e8, test, visualize, autoFix, needMoney, invert, false, 0, out node, out num4, out cost, out productionRate);
                    if ((errors == ToolBase.ToolErrors.None) | test)
                    {
                        num++;
                        totalCost += cost;
                    }
                    else if ((errors & ToolBase.ToolErrors.NotEnoughMoney) != ToolBase.ToolErrors.None)
                    {
                        return num;
                    }
                }
            }

            return num;
        }
          
    }
}
   