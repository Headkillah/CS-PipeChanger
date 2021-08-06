using ColossalFramework;
using ColossalFramework.Globalization;
using ColossalFramework.UI;
using System;
using UnityEngine;

namespace PipeChanger.GUI
{
    public class MainPanel : UIPanel
    {
        private static readonly float _panelWidth = 370; //The width of our gui in pixel points
        private static readonly float _panelHeight = 410; // the height of our gui
        private static readonly float _titleHeight = 40;

        private UIPanel _mainPanel;
        private UILabel _lblTitle; // The title label
        private UIDragHandle _dragHandle;
        private UIButton _btnClose;
        private UILabel _infoLabel;

        // Upgrade Panel
        private UIPanel upgradePanel;

        private UIButton UpgradeButton;
        private UIButton CalcUpgradeBtn;
        private UILabel UpgradeTitleLabel;
        private UILabel PipeCalcLabel; // Used for show number of pipes after pressing calculate button
        private UILabel WaterPipeCountLabel; // Shows the number of pipes to upgrade
        private UILabel UpgradeCostLabel;
        private UILabel UpgradeResultLabel; // Shows the upgrade result
        private UILabel MoneyAmountLabel; // Shows the amount for upgrade

        // Downgrade Panel
        private UIPanel downgradePanel;

        private UIButton DowngradeButton;
        private UIButton CalcDngradeBtn;
        private UILabel DowngradeTitleLabel;
        private UILabel DGPipeCalcLabel; // Used for show number of pipes after pressing calculate button
        private UILabel DGHeatingPipeCountLabel; // Shows the number of pipes to upgrade
        private UILabel DowngradeCostLabel;
        private UILabel DowngradeResultLabel; // Shows the upgrade result
        private UILabel DGMoneyAmountLabel; // Shows the amount for upgrade

        /// <summary>
        /// Creates the Mainpanel / Close button / DragHandler
        /// </summary>
        public void Initialize()
        {
            //Main Panel
            if (_mainPanel != null) { _mainPanel.OnDestroy(); }

            isVisible = false;
            _mainPanel = AddUIComponent<UIPanel>();
            _mainPanel.name = "PipeChangerMainPanel";
            _mainPanel.backgroundSprite = "UnlockingPanel2";
            _mainPanel.color = new Color32(75, 75, 135, 255);
            width = _panelWidth; //UIComponent
            height = 380; //UIComponent
            _mainPanel.width = _panelWidth;
            _mainPanel.height = _panelHeight;
            relativePosition = new Vector3(ModSettings.instance.MenuPosX, ModSettings.instance.MenuPosY);
            _mainPanel.relativePosition = Vector3.zero;

            if (ModSettings.VerboseLogging) { Util.DebugPrint("MainPanel added"); }

            //Title Label
            _lblTitle = AddUIComponent<UILabel>();
            _lblTitle = SamsamTS.UIUtils.CreateTitleLabel(_mainPanel, "TitleLabel", "Pipe Changer", new Vector3(100, 5));

            //DragHandler
            _dragHandle = SamsamTS.UIUtils.CreateDragHandle(_mainPanel, _panelWidth, _titleHeight);

            //Close Button
            //_btnClose = _mainPanel.AddUIComponent<UIButton>();
            _btnClose = SamsamTS.UIUtils.CreateCloseButton(_mainPanel);
            _btnClose.eventClick += CloseButtonClick;

            SetUpControls();

            absolutePosition = new Vector3(ModSettings.instance.MenuPosX, ModSettings.instance.MenuPosY);
        }

        /// <summary>
        /// SetUp all Controls (Button/Panel/Label)
        /// </summary>
        private void SetUpControls()
        {
            try
            {
                upgradePanel = _mainPanel.AddUIComponent<UIPanel>();
                upgradePanel.name = "UpgradePanel";
                upgradePanel.anchor = UIAnchorStyle.Right | UIAnchorStyle.Left | UIAnchorStyle.Top;
                upgradePanel.relativePosition = new Vector3(0, 40);
                upgradePanel.width = _mainPanel.width;
                upgradePanel.height = 200;

                UpgradeTitleLabel = SamsamTS.UIUtils.CreateLabel(upgradePanel, "UpgradeTitleLabel", "Upgrade WaterPipes", new Vector3(95, 40));

                CalcUpgradeBtn = SamsamTS.UIUtils.CreateButton(upgradePanel, "CalculateUgBtn", "Calculate Upgrade", new Vector3(10, 70));
                CalcUpgradeBtn.width = 160;
                CalcUpgradeBtn.height = 28;
                CalcUpgradeBtn.eventClick += (c, p) => Singleton<SimulationManager>.instance.AddAction(new System.Action(this.CalculateUpgrade));

                UpgradeCostLabel = SamsamTS.UIUtils.CreateLabel(upgradePanel, "UpgradeCostLabel", "Upgrade cost:", new Vector3(10, 95));
                UpgradeCostLabel.textColor = Color.white;
                UpgradeCostLabel.autoSize = false;
                UpgradeCostLabel.height = 25;
                UpgradeCostLabel.width = 130;
                UpgradeCostLabel.verticalAlignment = UIVerticalAlignment.Middle;

                MoneyAmountLabel = SamsamTS.UIUtils.CreateLabel(upgradePanel, "MoneyAmountLabel", "0.00", new Vector3(180, 97));
                MoneyAmountLabel.textColor = Color.white;
                MoneyAmountLabel.autoSize = true;
                MoneyAmountLabel.textAlignment = UIHorizontalAlignment.Right;
                MoneyAmountLabel.verticalAlignment = UIVerticalAlignment.Middle;

                PipeCalcLabel = SamsamTS.UIUtils.CreateLabel(upgradePanel, "PipeCalcLabel", "Pipe count:", new Vector3(10, 120));
                PipeCalcLabel.autoSize = false;
                PipeCalcLabel.height = 25;
                PipeCalcLabel.width = 100;
                PipeCalcLabel.verticalAlignment = UIVerticalAlignment.Middle;

                WaterPipeCountLabel = upgradePanel.AddUIComponent<UILabel>();
                WaterPipeCountLabel.cachedName = "WaterPipeCountLabel";
                WaterPipeCountLabel.relativePosition = new Vector3(120, 124);
                WaterPipeCountLabel.text = "0";
                WaterPipeCountLabel.autoSize = false;
                WaterPipeCountLabel.verticalAlignment = UIVerticalAlignment.Middle;

                UpgradeButton = SamsamTS.UIUtils.CreateButton(upgradePanel, "UpgradeBtn", "Upgrade", new Vector3(10, 150));
                UpgradeButton.tooltip = "Click to upgrade all water pipes to heating pipes.";
                UpgradeButton.textScale = 1.0f;
                UpgradeButton.width = 85;
                UpgradeButton.height = 25;
                UpgradeButton.eventClick += (c, p) => Singleton<SimulationManager>.instance.AddAction(new System.Action(this.UpgradePipes));

                UpgradeResultLabel = upgradePanel.AddUIComponent<UILabel>();
                UpgradeResultLabel.relativePosition = new Vector3(120, 150);
                UpgradeResultLabel.text = "";
                UpgradeResultLabel.autoSize = true;
                UpgradeResultLabel.verticalAlignment = UIVerticalAlignment.Middle;

                // From here to end everything for downgrading...
                downgradePanel = _mainPanel.AddUIComponent<UIPanel>();
                downgradePanel.name = "DowngradePanel";
                downgradePanel.anchor = UIAnchorStyle.Right | UIAnchorStyle.Left | UIAnchorStyle.Top;
                downgradePanel.relativePosition = new Vector3(0, 205);
                downgradePanel.width = _mainPanel.width;
                downgradePanel.height = 200;

                DowngradeTitleLabel = SamsamTS.UIUtils.CreateLabel(downgradePanel, "DowngradeTitleLabel", "Downgrade Heatpipes", new Vector3(95, 40));

                CalcDngradeBtn = SamsamTS.UIUtils.CreateButton(downgradePanel, "CalcDngradeBtn", "Calculate Downgrade", new Vector3(10, 70));
                CalcDngradeBtn.width = 190;
                CalcDngradeBtn.height = 28;
                CalcDngradeBtn.eventClick += (c, p) => Singleton<SimulationManager>.instance.AddAction(new System.Action(this.CalculateDowngrade));

                DowngradeCostLabel = SamsamTS.UIUtils.CreateLabel(downgradePanel, "DowngradeCostLabel", "Downgrade cost:", new Vector3(10, 95));
                DowngradeCostLabel.textColor = Color.white;
                DowngradeCostLabel.autoSize = false;
                DowngradeCostLabel.height = 25;
                DowngradeCostLabel.width = 140;
                DowngradeCostLabel.verticalAlignment = UIVerticalAlignment.Middle;

                DGMoneyAmountLabel = SamsamTS.UIUtils.CreateLabel(downgradePanel, "DGMoneyAmountLabel", "0.00", new Vector3(180, 100));
                DGMoneyAmountLabel.textColor = Color.white;
                DGMoneyAmountLabel.autoSize = true;
                DGMoneyAmountLabel.textAlignment = UIHorizontalAlignment.Right;
                DGMoneyAmountLabel.verticalAlignment = UIVerticalAlignment.Middle;

                DGPipeCalcLabel = SamsamTS.UIUtils.CreateLabel(downgradePanel, "DGPipeCalcLabel", "HeatPipe count:", new Vector3(10, 120));
                DGPipeCalcLabel.autoSize = false;
                DGPipeCalcLabel.height = 25;
                DGPipeCalcLabel.width = 130;
                DGPipeCalcLabel.verticalAlignment = UIVerticalAlignment.Middle;

                DGHeatingPipeCountLabel = downgradePanel.AddUIComponent<UILabel>();
                DGHeatingPipeCountLabel.cachedName = "DGHeatingPipeCountLabel";
                DGHeatingPipeCountLabel.relativePosition = new Vector3(150, 124);
                DGHeatingPipeCountLabel.text = "0";
                DGHeatingPipeCountLabel.autoSize = false;
                DGHeatingPipeCountLabel.verticalAlignment = UIVerticalAlignment.Middle;

                DowngradeButton = SamsamTS.UIUtils.CreateButton(downgradePanel, "DowngradeButton", "Downgrade", new Vector3(10, 150));
                DowngradeButton.tooltip = "Click to downgrade all heating pipes to water pipes.";
                DowngradeButton.textScale = 1.0f;
                DowngradeButton.width = 150;
                DowngradeButton.height = 25;
                DowngradeButton.eventClick += (c, p) => Singleton<SimulationManager>.instance.AddAction(new System.Action(this.DowngradePipes));

                DowngradeResultLabel = SamsamTS.UIUtils.CreateLabel(downgradePanel, "DowngradeResultLabel", "", new Vector3(170, 155));
                DowngradeResultLabel.autoSize = true;
                DowngradeResultLabel.verticalAlignment = UIVerticalAlignment.Middle;

                _infoLabel = SamsamTS.UIUtils.CreateLabel(downgradePanel, "InfoLabel", "", new Vector3(40, 170));
            }
            catch (Exception ex)
            {
                Util.LogException(ex);
            }
        }

        protected override void OnPositionChanged()
        {
            base.OnPositionChanged();

            bool posChanged = ModSettings.instance.MenuPosX != (int)absolutePosition.x
                              || ModSettings.instance.MenuPosY != (int)absolutePosition.y;
            if (posChanged)
            {
                ModSettings.instance.MenuPosX.value = (int)absolutePosition.x;
                ModSettings.instance.MenuPosY.value = (int)absolutePosition.y;
                //    Debug.Log("Xpos: " + (int)absolutePosition.x + " Ypos: " + (int)absolutePosition.y);
            }
        }

        internal void ForceUpdateMenuPosition()
        {
            absolutePosition = new Vector3(ModSettings.instance.MenuPosX, ModSettings.instance.MenuPosY);
            // if (ModSettings.VerboseLogging) { Util.DebugPrint("Position reseted. absolutePosition: " + absolutePosition); }
        }

        // Reset the panels
        public void OnVisibilityChanged(UIComponent component, bool value)
        {
            base.isVisible = value;
            if (base.isVisible)
            {
                Debug.Log("OnVisibilityChanged called");
                MoneyAmountLabel.text = "0.00";
                MoneyAmountLabel.textColor = Color.white;
                WaterPipeCountLabel.text = "0";
                UpgradeResultLabel.text = "";
                DGMoneyAmountLabel.text = "0.00";
                DGMoneyAmountLabel.textColor = Color.white;
                DGHeatingPipeCountLabel.text = "0";
                DowngradeResultLabel.text = "";
            }
        }

        private void CloseButtonClick(UIComponent component, UIMouseEventParameter eventparam)
        {
            eventparam.Use();
            Hide();
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
                if (ModSettings.VerboseLogging) { Util.DebugPrint("CalculateUpgrade"); }

                int totalCost = 0;
                float factor = 0.01f;

                ToolBase.ToolErrors none = ToolBase.ToolErrors.None;
                int num2 = UpgradeWaterToHeat(true, out totalCost, out none);
                MoneyAmountLabel.text = (totalCost * factor).ToString(ColossalFramework.Globalization.Locale.Get("MONEY_FORMAT"), LocaleManager.cultureInfo);

                if (Singleton<EconomyManager>.instance.PeekResource(EconomyManager.Resource.Construction, totalCost) != totalCost)
                {
                    MoneyAmountLabel.textColor = Color.red;
                }
                else
                {
                    MoneyAmountLabel.textColor = Color.green;
                }
                WaterPipeCountLabel.text = num2.ToString();
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
                if (ModSettings.VerboseLogging) { Util.DebugPrint("CalculateDowngrade() called"); }

                int totalCost = 0;
                float factor = 0.01f;

                ToolBase.ToolErrors none = ToolBase.ToolErrors.None;
                int num2 = DowngradeHeatToWater(true, out totalCost, out none);
                DGMoneyAmountLabel.text = (totalCost * factor).ToString(ColossalFramework.Globalization.Locale.Get("MONEY_FORMAT"), LocaleManager.cultureInfo);

                if (Singleton<EconomyManager>.instance.PeekResource(EconomyManager.Resource.Construction, totalCost) != totalCost)
                {
                    this.DGMoneyAmountLabel.textColor = Color.red;
                }
                else
                {
                    this.DGMoneyAmountLabel.textColor = Color.green;
                }
                DGHeatingPipeCountLabel.text = num2.ToString();
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
                if (ModSettings.VerboseLogging) { Util.DebugPrint("UpgradePipes"); }

                int totalCost = 0;
                ToolBase.ToolErrors none = ToolBase.ToolErrors.None;
                int num2 = this.UpgradeWaterToHeat(false, out totalCost, out none);
                string str = "";
                if (none != ToolBase.ToolErrors.None)
                {
                    str = " Not enough money.";
                    this.UpgradeResultLabel.textColor = Color.red;
                }
                else
                {
                    this.UpgradeResultLabel.textColor = Color.green;
                }

                object[] objArray1 = new object[] { "Upgraded ", num2, " pipes.", str };
                UpgradeResultLabel.text = string.Concat(objArray1);

                if (ModSettings.VerboseLogging)
                {
                    Util.DebugPrint(PipeChanger.MODNAME + " " + PipeChanger.MODVERSION + " " + objArray1.ToString());
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
                if (ModSettings.VerboseLogging) { Util.DebugPrint("DowngradePipes"); }

                int totalCost = 0;
                ToolBase.ToolErrors none = ToolBase.ToolErrors.None;
                int num2 = DowngradeHeatToWater(false, out totalCost, out none);
                string str = "";
                if (none != ToolBase.ToolErrors.None)
                {
                    str = " Not enough money.";
                    this.DowngradeResultLabel.textColor = Color.red;
                }
                else
                {
                    this.DowngradeResultLabel.textColor = Color.green;
                }
                object[] objArray1 = new object[] { "Downgraded ", num2, " pipes.", str };
                this.DowngradeResultLabel.text = string.Concat(objArray1);

                if (ModSettings.VerboseLogging)
                {
                    Util.DebugPrint(PipeChanger.MODNAME + " " + PipeChanger.MODVERSION + " " + objArray1.ToString());
                }
            }
            catch (Exception ex)
            {
                Util.LogException(ex);
            }
        }

        private int UpgradeWaterToHeat(bool test, out int totalCost, out ToolBase.ToolErrors errors)
        {
            if (ModSettings.VerboseLogging) { Util.DebugPrint("UpgradeWaterToHeat"); }

            int num = 0;
            totalCost = 0;
            errors = ToolBase.ToolErrors.None;
            NetInfo info = PrefabCollection<NetInfo>.FindLoaded("Heating Pipe");
            if (info == null)
            {
                if (ModSettings.VerboseLogging) { Util.DebugPrint("Couldn't find Heating Pipe, aborting."); }

                // _infoLabel.textColor = Color.red;
                // _infoLabel.text = "Couldn't find any Heating Pipe, aborting.";
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
            if (ModSettings.VerboseLogging) { Util.DebugPrint("DowngradeHeatToWater"); }

            int num = 0;
            totalCost = 0;
            errors = ToolBase.ToolErrors.None;
            NetInfo info = PrefabCollection<NetInfo>.FindLoaded("Water Pipe");

            if (info == null)
            {
                if (ModSettings.VerboseLogging) { Util.DebugPrint("Couldn't find any Water Pipe, aborting."); }

                //_infoLabel.textColor = Color.red;
                //_infoLabel.text = "Couldn't find any Water Pipe, aborting.";
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