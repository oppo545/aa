using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

public class Cls_Evt : Cls_RealInformation
{
    [Description("钥匙档位")]
    /// <summary>
    /// 钥匙档位
    /// </summary>
    public int? VCU_Keyposition { get; set; }
    [Description("扩展档位")]
    /// <summary>
    /// 扩展档位
    /// </summary>
    public int? TCU_ExtensionGearPosition { get; set; }
    [Description("ECO模式")]
    /// <summary>
    /// ECO模式
    /// </summary>
    public int? VCU_ECOMode { get; set; }
    [Description("正极绝缘电阻")]
    /// <summary>
    /// 正极绝缘电阻
    /// </summary>
    public int? PositiveInsulationResistance { get; set; }
    [Description("负极绝缘电阻")]
    /// <summary>
    /// 负极绝缘电阻
    /// </summary>
    public int? NegativeInsulationResistance { get; set; }
    [Description("油门深度(分辨率1%)")]
    /// <summary>
    /// 油门深度(分辨率1%)
    /// </summary>
    public int? Motor_ThrottleDepth { get; set; }
    [Description("制动深度(分辨率1%)")]
    /// <summary>
    /// 制动深度(分辨率1%)
    /// </summary>
    public int? BrakeDepth { get; set; }
    [Description(" 充、放电系统工作状态")]
    /// <summary>
    /// 充、放电系统工作状态
    /// </summary>
    public int? SystemWorkState_Charge_Discharge { get; set; }
    [Description("母线电压")]
    /// <summary>
    /// 母线电压
    /// </summary>
    public double BusVoltage { get; set; }
    [Description("IPM散热器温度（分辨率1℃)")]
    /// <summary>
    /// IPM散热器温度（分辨率1℃)
    /// </summary>
    public int? IPMRadiatorTemperature { get; set; }
    [Description(" IGBT温度（分辨率1℃）")]
    /// <summary>
    /// IGBT温度（分辨率1℃）
    /// </summary>
    public int? IGBTTemperature { get; set; }
    [Description("动力电机母线电压")]
    /// <summary>
    /// 动力电机母线电压
    /// </summary>
    public double Motor_PowerBusVoltage { get; set; }
    [Description("冷却液温度")]
    /// <summary>
    /// 冷却液温度
    /// </summary>
    public int? CoolantTemperature { get; set; }
    [Description("目标压力")]
    /// <summary>
    /// 目标压力
    /// </summary>
    public int? TargetPressure { get; set; }
    [Description("后轿左气室压力")]
    /// <summary>
    /// 后轿左气室压力
    /// </summary>
    public int? BackLeftAirChamberPressure { get; set; }
    [Description("真空泵状态")]
    /// <summary>
    /// 真空泵状态
    /// </summary>
    public int? VacuumPumpState { get; set; }
    [Description("助力转向油泵状态")]
    /// <summary>
    /// 助力转向油泵状态
    /// </summary>
    public int? PowerSteeringOilPumpState { get; set; }

    [Description("小计里程")]
    public double IC_Odmeter { get; set; }

    [Description("漏电检测")]
    public double BMS_CreepageMonitor { get; set; }
    [Description("SOC经过计算")]
    public double BMS_SOCCalculate { get; set; }
    [Description("外部充电信号")]
    public int? BMS_OutsideChargeSignal { get; set; }
    [Description("非车载充电连接指示信号")]
    public int? BMS_OFCConnectSignal { get; set; }
    [Description("制动开关状")]
    public int? VCU_BrakePedalSt { get; set; }
    [Description("制动踏板电压信号")]
    public double BrakeVoltageSignal { get; set; }
    [Description("制动信号")]
    public int? ABS_BrakeSignal { get; set; }
    [Description("制动开关有效信号")]
    public int? VCU_BrakePedalSwitchValid { get; set; }
    [Description("整车工作模式")]
    public int? VehicleState { get; set; }
    [Description("电机控制模式请求")]
    public int? mode_Motor_req { get; set; }
    [Description("回馈制动请求")]
    public int? req_Regen { get; set; }
    [Description("高压上电请求")]
    public int? req_BattPackHV { get; set; }
    [Description("BMS休眠允许标志")]
    public int? flg_BMSSleepAllow { get; set; }
    [Description("充电口连接状态")]
    public int? ChargePortConnect_VCU { get; set; }
    [Description("CP电压值")]
    public double CP_Voltage { get; set; }
    [Description("供电设备最大供电电流")]
    public double ACCharger_Max_Cur { get; set; }
    [Description("充电电缆容量")]

    public double charge_cable_capacity { get; set; }
    [Description("动力电池亏电")]
    public string PowerBattLV { get; set; }
    [Description("动力电池故障")]
    public string batt_sys_flt { get; set; }
    [Description("电机或控制器过热")]
    public string MCUorMotor_ot_flt { get; set; }
    [Description("EHPS故障")]
    public string EPS_flt { get; set; }
    [Description("Ready指示灯")]
    public string state_PowerReady { get; set; }
    [Description("EPB状态指示灯")]
    public string state_EPB_enable { get; set; }
    [Description("主告警灯")]
    public string state_mainErrLight { get; set; }
    [Description("DC系统告警灯")]
    public string state_DCSysErrLight { get; set; }
    [Description("定速巡航控制指示灯")]
    public string state_CruiseLight { get; set; }
    [Description("定速巡航控制指示灯")]
    public string state_PowerSysErrLight { get; set; }
    [Description("超速提醒")]
    public string state_OverSpeedAlarm { get; set; }
    [Description("PTC互锁信号")]
    public string state_PTCContactorInterlock { get; set; }
    [Description("空调压缩机互锁信号")]
    public string state_ACContactorInterlock { get; set; }

    [Description("ABS系统故障")]
    public string ABS_SysFault { get; set; }
    [Description("油门踏板故障")]
    public string AcceleratorFault { get; set; }
    [Description("车辆关机异常")]
    public string VehErrShutDown { get; set; }
    [Description("三相互锁信号")]
    public string state_ThreehaseInterlock { get; set; }
    [Description("转向互锁信号")]
    public string state_EHPSInterlock { get; set; }
    [Description("交流互锁信号")]
    public string state_ACPowerInterlock { get; set; }
    [Description("直流互锁信号")]
    public string state_DCPowerInterlock { get; set; }
    [Description("动力电池互锁信号")]
    public string state_PowerBatteryInterlock { get; set; }
    [Description("仪表提示内容1")]
    public string state_Information1 { get; set; }
    [Description("仪表提示内容2")]
    public string state_Information2 { get; set; }
    [Description("仪表提示内容3")]
    public string state_Information3 { get; set; }
    [Description("仪表提示内容4")]
    public string state_Information4 { get; set; }
    [Description("仪表提示内容5")]
    public string state_Information5 { get; set; }
    [Description("仪表提示内容6")]
    public string state_Information6 { get; set; }
    [Description("仪表提示内容7")]
    public string state_Information7 { get; set; }
    [Description("仪表提示内容8")]
    public string state_Information8 { get; set; }
    [Description("百公里电耗")]
    public double PowerCons { get; set; }
    [Description("整车功率")]
    public double VehPowerPercent { get; set; }
    [Description("电耗计算重置")]
    public int? req_PowerCostReset { get; set; }
    [Description("状态指示灯")]
    public int? flg_EPB_State { get; set; }

    [Description("左前轮速")]
    public double LeftFrontWheelSpeed { get; set; }
    [Description("右前轮速")]
    public double RightFrontWheelSpeed { get; set; }
    [Description("左后轮速")]
    public double LeftRearWheelSpeed { get; set; }
    [Description("右后轮速")]
    public double RightRearWheelSpeed { get; set; }
    [Description("电机扭矩")]
    /// <summary>
    /// 电机扭矩
    /// </summary>
    public double Motor_OutputTorque { get; set; }
    [Description("电机目标扭矩")]
    /// <summary>
    /// 电机目标扭矩
    /// </summary>
    public double VCU_Motor_Target_tq { get; set; }
    [Description("电机控制器请求")]
    /// <summary>
    /// 电机控制器请求
    /// </summary>
    public int? Motor_ControllerRequest { get; set; }
    [Description("电机工作模式反馈")]
    /// <summary>
    /// 电机工作模式反馈
    /// </summary>
    public int? Motor_WorkModeFeedback { get; set; }
    [Description("电机控制器工作状态")]
    public int? MCU_ElecPowerTrainMngtState { get; set; }
    [Description("电机转子温度")]
    public int? MCU_InternalMachineTemp { get; set; }
    [Description("电机最大电动扭矩")]
    public int? MCU_MaxMotorTorque { get; set; }
    [Description("电机最大发电扭矩")]
    public int? Motor_MaxGenTorque { get; set; }
    [Description("电机控制器母线电容放电状")]
    public int? MCU_ActiveDischarge { get; set; }
    [Description("电机状态及信号强度")]
    public int? VCU_BrakeEnergy { get; set; }
    [Description("电机状态")]
    public int? Motor_State { get; set; }
    [Description("电机反馈扭矩")]
    public double Motor_TorqueFeedback { get; set; }
    [Description("电机最大允许扭矩")]
    public double Motor_AllowMaxTorque { get; set; }
    [Description("电机功率")]
    public double Motor_OutputPower { get; set; }
    [Description("转向电机转速请求")]
    public int? EpsMotSpeed { get; set; }
    [Description("电机U相电流")]
    public double U_MotCurrent { get; set; }
    [Description("电机V相电流")]
    public double V_MotCurrent { get; set; }
    [Description("电机W相电流")]
    public double W_MotCurrent { get; set; }
    [Description("电机当前工作模式")]
    public int? MCU_MotRunMode { get; set; }
    [Description("电机当前回馈模式")]
    public int? MCU_MotFeedBackMode { get; set; }
    [Description("电机预充电完成状态")]
    public int? MCU_PreComplete { get; set; }
    [Description("MCU初始化完成状态")]
    public int? MCU_InitializeCompl { get; set; }
    [Description("电机过温故障")]
    public int? MCU_MotOTFault { get; set; }
    [Description("电机过流故障")]
    public int? MCU_MotOverCurrent { get; set; }
    [Description("IGBT温度保护")]
    public int? MCU_IGBTTempProtect { get; set; }
    [Description("控制器过压故障")]
    public int? MCU_OverVoltage { get; set; }
    [Description("控制器欠压故障")]
    public int? MCU_UnderVoltage { get; set; }
    [Description("控制器欠压故障")]
    public int? MCU_OTFault { get; set; }
    [Description("电机超速故障")]
    public int? MCU_MotOverSpeed { get; set; }
    [Description("电机控制器过流")]
    public int? MCU_OverCurrent { get; set; }
    [Description("CAN故障")]
    public string MCU_CANFault { get; set; }
    [Description("控制器温度传感器故障")]
    public string MCU_TSensorFault { get; set; }
    [Description("电机温度传感器故障")]
    public string MCU_MotTSensorFault { get; set; }
    [Description("控制器电流传感器故障")]
    public string MCU_CSensorFault { get; set; }
    [Description("电机/控制器自检故障")]
    public string MCU_MotMCUCheckSelfFault { get; set; }
    [Description("IGBT故障")]
    public string MCU_IGBTFault { get; set; }
    [Description("电机编码器故障")]
    public string MCU_MotEncoderFault { get; set; }
    [Description("预充电故障")]
    public string MCU_PreChargeFault { get; set; }
    [Description("电机对地短路")]
    public string GroundShortCircuit { get; set; }
    [Description("电机相间短路")]
    public string PhaseShortCircuit { get; set; }
    [Description("电机缺相故障")]
    public string MotLackPhase { get; set; }
    [Description("电机过载")]
    public string MotOverLoad { get; set; }
    [Description("电机失速")]   
    public string MotLoseSpeed { get; set; }
    [Description("电机最大输出转矩")]
    public double MotMaxTorque { get; set; }
    [Description("充电机输出的充电电压")]
    public double ONC_OutputVoltage { get; set; }
    [Description("充电机输出的充电电流")]
    public double ONC_OutputCurrent { get; set; }
    [Description("充电机输入电压状态")]
    public int? ONC_InputVoltageSt { get; set; }
    [Description("充电机通信状态")]
    public int? CCS_ChargerCommunication { get; set; }
    [Description("充电机温度")]
    public int? ONC_ONCTemp { get; set; }
    [Description("交流输入状态")]
    public int? BMS_ChargerACInput { get; set; }

    [Description("充电机当前输入电压")]
    public double InChargingVolt { get; set; }
    [Description("充电机当前输入电流")]
    public double InChargingCurr { get; set; }
    [Description("电池放电电流限值")]
    public int? HVDischargeLimit { get; set; }
    [Description("电池充电电流限值")]
    public int? HVChargeLimit { get; set; }
    [Description("充电接触器状态")]
    public int? state_ChargeContactor { get; set; }
    [Description("预充电状态")]
    public int? HVPreChrgRdy { get; set; }
    [Description("充电接触器接通请求")]
    public int? req_ChargeContactorEn { get; set; }
    [Description("充电机使能请求")]
    public int? req_Charger_Enable { get; set; }
    [Description("充电电压请求")]
    public int? req_ChargeVoltage { get; set; }
    [Description("充电电流请求")]
    public int? req_ChargeCurrent { get; set; }
    [Description("输入电流功率因数")]
    public double InputPowerFactor { get; set; }
    [Description("充电机效率")]
    public double ChargerEfficiency { get; set; }
    [Description("充电机温度")]
    public int? ChargerTemp { get; set; }
    [Description("当前充电状态")]
    public int? state_Charging { get; set; }
    [Description("高精度充电机输出的充电电流")]
    public double ONC_HighPreOutputCurrent { get; set; }

    [Description("最高允许充电端电流")]
    public double MaxAllowChargingCurrent { get; set; }
    [Description("最高允许充电端电压")]
    public double MaxAllowChargingVoltage { get; set; }
    [Description("充电控制指令")]
    public int? ChargeControlCommand { get; set; }
    [Description("慢充充电连接状态")]
    public int? SlowChargingStatus { get; set; }
    [Description("快充充电连接状态")]
    public int? FastChargingStatus { get; set; }
    [Description("硬件故障")]
    public int? HardwareFailure { get; set; }
    [Description("启动状态")]
    public int? StartState { get; set; }
    [Description("充电机最大充电电流")]
    public double ONC_Charge_Maxallow_A { get; set; }
    [Description("高精度充电电流")]
    public double HighPreChargingCurrent { get; set; }

    [Description("DCDC状态")]
    /// <summary>
    /// DCDC状态
    /// </summary>
    public int? DCDC_Work_State { get; set; }

    [Description("直流母线电压")]
    public double Motor_DCVolt { get; set; }

    [Description("直流母线电流")]
    public int? Motor_DCCurrent { get; set; }
    [Description("DCDC温度")]
    public int? DCDC_Temperature { get; set; }
    [Description("DC-DC输出电压")]
    public double DCDC_OutputVoltage { get; set; }
    [Description("DC-DC输出电流")]
    public double DCDC_OutputCurrent { get; set; }
    [Description("DC-DC使能应答")]
    public int? DCDC_EnableResponse { get; set; }
    [Description("DC-DC输入电压")]
    public double DCDC_InputVoltage { get; set; }
    [Description("DC-DC输入电流")]
    public double DCDC_InputCurrent { get; set; }

    [Description("DCDC效率")]
    public double DCDC_Efficiency { get; set; }
    [Description("DCDC温度")]
    public double DCDC_Temp { get; set; }

    [Description("ABS故障")]
    /// <summary>
    /// ABS故障
    /// </summary>
    public int? ABS_Fault { get; set; }
    [Description("空压机故障")]
    /// <summary>
    /// 空压机故障
    /// </summary>
    public int? AirCompressor_Fault { get; set; }
    [Description("DCDC故障")]
    /// <summary>
    /// DCDC故障
    /// </summary>
    public int? DCDC_Fault { get; set; }
    [Description("DCDC输出短路故障")]
    public string DCDC_OutputShortcircuit { get; set; }
    [Description("DCDC过热故障")]
    public string DCDC_OverheatFault { get; set; }

    [Description("助力转向故障")]
    /// <summary>
    /// 助力转向故障
    /// </summary>
    public int? PowerSteering_Fault { get; set; }
    [Description("冷却系统故障")]
    /// <summary>
    /// 冷却系统故障
    /// </summary>
    public int? CoolingSystem_Fault { get; set; }
    [Description("VCU故障")]
    public int? VCU_Fault { get; set; }

    [Description("电池故障")]
    public int? BMS_FaultDislpay { get; set; }

    [Description("充电机故障码")]
    public int? ONC_Fault { get; set; }

    [Description("电机故障类型")]
    public int? MCU_ElecMachineFault { get; set; }



    [Description("CAN网络故障")]
    public int? CANNetworkFailure { get; set; }

    [Description("电机控制器电压故障")]
    public int? Motor_ControllersVoltageFault { get; set; }

    [Description("电机故障")]
    public int? MCU_FaultDInternalInverterMotor { get; set; }

    [Description("DC/DC内部故障")]
    public int? DCDC_InternalFault { get; set; }

    [Description("DC/DC输出短路故障")]
    public int? DCDC_OutputCircuitFault { get; set; }

    [Description("DCDC故障类型")]
    public int? DCDC_ElecMachineFault { get; set; }
    [Description("DC/DC输出过压故障")]
    public int? DCDC_OutputOverVoltageFault { get; set; }


    [Description("TCU故障码")]
    public int? TCU_Fault { get; set; }
    [Description("EPS故障报警")]
    public int? EPS_FaultWarning { get; set; }

    #region E0报警
    [Description("电机控制器预充故障")]
    public int? MotorControllerFault_Precharge { get; set; }
    [Description("电机控制器主接故障")]
    public int? MotorControllerFault_MainContactor { get; set; }
    [Description("电机控制器IGBT故障")]
    public int? MotorControllerFault_IGBTDamage { get; set; }
    [Description("电机控制器过流故障")]
    public int? MotorControllerFault_OverCurrent { get; set; }
    [Description("电机控制器过温故障")]
    public int? MotorControllerFault_OverTemperature { get; set; }

    [Description("电机控制器过压故障")]
    public int? MotorControllerFault_OverVoltage { get; set; }

    [Description("电机控制器欠压故障")]
    public int? MotorControllerFault_UnderVoltage { get; set; }

    [Description("电机控制器堵转故障")]
    public int? MotorControllerFault_Blocking
    { get; set; }

    [Description("电机控制器超速故障")]
    public int? MotorControllerFault_OverSpeed { get; set; }
    [Description("电机控制器电压上升故障")]
    public int? MotorControllerFault_VoltageRise
    { get; set; }
    [Description("电机控制器电流上升故障")]
    public int? MotorControllerFault_CurrentRise { get; set; }
    [Description("电机控制器总线故障")]
    public int? MotorControllerFault_Bus { get; set; }

    [Description("自检异常")]
    public int? SelfCheckAbnormal { get; set; }
    [Description("短路/绝缘检测（高压）")]
    public int? MotorInsulationTest { get; set; }
    [Description("断路/开路（高压）")]
    public int? MotorBreakerCircuit { get; set; }
    [Description("驱动电机过载")]
    public int? Motor_VoltageOver { get; set; }
    [Description("驱动电机控制器温度过高")]
    public int? Motor_ControllerTemperatureOver { get; set; }
    [Description("驱动电机控制器24V欠压")]
    public int? Motor_Controller24VTemperatureLow { get; set; }
    [Description("旋变故障")]
    public int? Motor_ResolverFault { get; set; }
    [Description("驱动电机输出缺相")]
    public int? Motor_DriveOutputPhase { get; set; }

    [Description("电机故障码")]
    public int? Motor_Fault { get; set; }


    [Description("过温输出功率减半")]
    public string ONC_OverTempOutputPowerReduceHalf { get; set; }
    [Description("过温输出关闭")]
    public string ONC_OverTempOutputClose
    { get; set; }
    [Description("输入欠压")]
    public string ONC_InputUndervoltage { get; set; }
    [Description("输入欠压")]
    public string ONC_InputOvervoltage
    { get; set; }
    [Description("内部PFC过压")]
    public string ONC_InnerPFCOvervoltage { get; set; }
    [Description("内部PFC欠压")]
    public string ONC_InnerPFCUndervoltage { get; set; }
    [Description("输出过流")]
    public string ONC_OutputOvercurrent
    { get; set; }
    [Description("输出短路/欠压")]
    public string ONC_OutputShortcircuitOrUndervoltage { get; set; }
    [Description("内部中间电压欠压")]
    public string ONC_InnerMiddleUndervoltage { get; set; }
    [Description("输出欠流")]
    public string ONC_InputUndercurrent
    { get; set; }
    [Description("输出过压")]
    public string ONC_OutputOvervoltage
    { get; set; }
    [Description("通讯超时")]
    public string ONC_CommunicationTimeout
    { get; set; }
    [Description("输入过压")]
    public string DCDC_InputOvervoltage { get; set; }
    [Description("输入欠压")]
    public string DCDC_InputUndervoltage
    { get; set; }
    [Description("输出过压")]
    public string DCDC_OutputOvervoltage
    { get; set; }
    [Description("输出欠压")]
    public string DCDC_OutputUndervoltage
    { get; set; }
    [Description("输出过流")]
    public string DCDC_OutputOvercurrent
    { get; set; }
    [Description("低温报警")]
    public string DCDC_LowTempAlarm { get; set; }
    [Description("高温报警")]
    public string DCDC_HighTempAlarm { get; set; }
     [Description("DCDC使能请求")]
    public int? req_DCDC_Enable { get; set; }
     [Description("电机转速请求")]
    public int? MotorSpeed_req { get; set; }
     [Description("电机转矩请求")]
    public double MotorTq_req { get; set; }
     [Description("DCDC最大允许输出功率")]
    public int? DCDCMaxOtptPower { get; set; }
     [Description("充电机硬件故障")]
    public string ChargerHardware { get; set; }
     [Description("充电机过温报警")]
    public int? ChargerTempAlarm { get; set; }
     [Description("充电机输入电压状态")]
     public int? ONC_InputVoltage { get; set; }
     [Description("充电机输入欠压报警")]
     public double ONC_HighPreInputCurrent { get; set; }

     [Description("充电机输入欠压报警")]
    public string ChargerInputUnderVolt { get; set; }
     [Description("充电机输入过压报警")]
    public string ChargerInputOverVolt { get; set; }
     [Description("充电机输出过流报警")]
    public string ChargerOutputOverCurr { get; set; }
     [Description("电池连接故障报警")]
    public string BattConnectFault { get; set; }
     [Description("EBD故障")]
    public string EBD_Fault { get; set; }
     [Description("ABS动作标志")]
    public string ABS_Sign { get; set; }
     [Description("轮速故障")]
    public string WheelSpeed_Fault { get; set; }
     [Description("ABS功能")]
    public string ABS_Function { get; set; }
     [Description("ABS故障状态指示灯")]
    public string ABS_FaultStatusLed { get; set; }
     [Description("EBD故障指示灯")]
    public string EBD_FaultLed { get; set; }

    [Description("温度过高报警（电池）")]
    public int? BatteryFault_OverTemperature { get; set; }
    [Description("充电温度过低（电池）")]
    public int? BatteryFault_ChargeTemperatureLow { get; set; }
    [Description("放电温度过低（电池）")]
    public int? BatteryFault_DischargeTemperatureLow { get; set; }
    [Description("温差过大（电池）")]
    public int? BatteryFault_TemperatureDifferenceLarge { get; set; }
    [Description("单体电压过高（电池）")]
    public int? BatteryFault_MonomerVoltageHigh { get; set; }
    [Description("单体电压过低（电池）")]
    public int? BatteryFault_MonomerVoltageLow { get; set; }
    [Description("压差过大（电池）")]
    public int? BatteryFault_PressureDifferenceLarge { get; set; }
    [Description("SOC过低（电池）")]
    public int? BatteryFault_SOCLow { get; set; }
    [Description("总电压过高")]
    public int? BatteryFault_TotalVoltageHigh { get; set; }
    [Description("总电压过低")]
    public int? BatteryFault_TotalVoltageLow { get; set; }
    [Description("充电电流过大")]
    public int? BatteryFault_ChargingCurrentLarge { get; set; }
    [Description("放电电流过大")]
    public int? BatteryFault_DischargeCurrentLarge { get; set; }
    [Description("绝缘故障")]
    public int? BatteryFault_Insulation { get; set; }

    [Description("SOC过高（电池）")]
    public int? SOCOvertopAlarm { get; set; }
    [Description("单体温度过低")]
    public int? BMS_UnitTemperatureTooLow { get; set; }
    [Description("极柱温度过高")]
    public int? BMS_PoleTemperatureOver { get; set; }
    [Description("极柱温度差异过大")]
    public int? BMS_PoleTemperatureDifferenceOver { get; set; }
    [Description("BMS与充电桩通讯故障")]
    public int? BMSCharging_CommunicationFault { get; set; }

    [Description("BMS_Fault")]
    public int? BMS_Fault { get; set; }
     [Description("电池过热故障")]
    public int? BMS_BattOverTemp { get; set; }
     [Description("电池绝缘故障")]
    public int? BMS_BattInsulation { get; set; }
     [Description("电池单体过压故障")]
    public int? BMS_BattOverVolt { get; set; }
     [Description("电池过流故障")]
    public int? BMS_BattOverCurrent { get; set; }
     [Description("电池单体欠压")]
    public int? BMS_BattUnderVolt { get; set; }
     [Description("与VMS通讯超时故障")]
    public string BMS_VMSCommOverTime { get; set; }
     [Description("与LECU通讯超时故障")]
    public string BMS_LECUCommOverTime { get; set; }
     [Description("电池包烟雾故障")]
    public string BMS_BattPackSmoke { get; set; }
     [Description("与充电机通讯超时故障")]
    public string BMS_ChargerCommOverTime { get; set; }

    [Description("电池自检故障")]
    public string BMS_BattCheckSelf { get; set; }
     [Description("电池单体电压互差大")]
    public string BMS_BattVoltDiff { get; set; }
     [Description("总正接触器状态")]
    public int? BMS_BattPosContactor { get; set; }
     [Description("总负接触器状态")]
    public int? BMS_BattNegContactor { get; set; }
    [Description("预充电接触器状态")]
    public int? BMS_PreChargeContactor { get; set; }
     [Description("高压互锁状态")]
    public int? BMS_interlock { get; set; }

    #endregion

     [Description("内网can通讯丢失")]
     public int? IntranetCanFault { get; set; }
     [Description("整车CAN通信丢失")]
     public int? VehicleCanFault { get; set; }



    [Description("电池平均温度")]
    /// <summary>
    /// 电池平均温度
    /// </summary>
    public double BMS_Temp_Ave { get; set; }
    [Description("电池组当前容量指数")]
    /// <summary>
    /// 电池组当前容量指数 (分辨率1%) 
    /// </summary>
    public int? BatteryPackCurrentCapacityIndex { get; set; }
    [Description("电池的健康指数")]
    /// <summary>
    /// 电池的健康指数 (分辨率1%) 

    /// </summary>
    public int? BatteryHealthIndex { get; set; }
    [Description("续驶里程")]
    /// <summary>
    /// 续驶里程
    /// </summary>
    public double VCU_CruisingRange { get; set; }
    [Description("电池包能量")]
    /// <summary>
    /// 电池包能量
    /// </summary>
    public double BatteryPackEnergy { get; set; }
    [Description("剩余动力电池电量")]
    /// <summary>
    /// 剩余动力电池电量
    /// </summary>
    public double BMS_RemainingBattPower { get; set; }

    [Description("BMS当前工作模式")]
    public int? BMSMode { get; set; }
    [Description("BMS休眠请求")]
    public int? req_BMSSleep { get; set; }
    [Description("电池加热或冷却请求")]
    public int? req_HeatOrCoolBatt { get; set; }
    [Description("电池单体最高电压编号1")]
    public int? HVMaxVoltage1_Num { get; set; }
    [Description("电池单体最高电压1")]
    public double HVMaxVoltage1 { get; set; }
    [Description("电池单体最高电压编号2")]
    public int? HVMaxVoltage2_Num { get; set; }
    [Description("电池单体最高电压2")]
    public double HVMaxVoltage2 { get; set; }
    [Description("电池单体最高电压编号3")]
    public int? HVMaxVoltage3_Num { get; set; }
    [Description("电池单体最高电压3")]
    public double HVMaxVoltage3 { get; set; }
    [Description("电池单体最高电压编号4")]
    public int? HVMaxVoltage4_Num { get; set; }
    [Description("电池单体最高电压4")]
    public double HVMaxVoltage4 { get; set; }
    [Description("电池单体最高电压编号1")]
    public int? HVMinVoltage1_Num { get; set; }
    [Description("电池单体最低电压1")]
    public double HVMinVoltage1 { get; set; }
    [Description("电池单体最低电压2")]
    public int? HVMinVoltage2_Num { get; set; }
    [Description("电池单体最低电压2")]
    public double HVMinVoltage2 { get; set; }
    [Description("电池单体最低电压3")]
    public int? HVMinVoltage3_Num { get; set; }
    [Description("电池单体最低电压3")]
    public double HVMinVoltage3 { get; set; }
    [Description("电池单体最低电压4")]
    public int? HVMinVoltage4_Num { get; set; }
    [Description("电池单体最低电压4")]
    public double HVMinVoltage4 { get; set; }
    [Description("高精度电池平均温度")]
    public double BMS_TempHighPreAve { get; set; }
    [Description("电池健康状态SOH")]
    /// <summary>
    /// 电池健康状态SOH
    /// </summary>
    public int? BatteryHealthSOH { get; set; }
    [Description("电池模块个数")]
    /// <summary>
    /// 电池模块个数
    /// </summary>
    public int? BatteryModulesNumber { get; set; }
    [Description("电池最大允许放电功率")]
    /// <summary>
    /// 电池最大允许放电功率
    /// </summary>
    public double BMS_Battery_Discharge_KW_MA { get; set; }
    [Description("电池最大允许充电功率")]
    /// <summary>
    /// 电池最大允许充电功率
    /// </summary>
    public double BMS_Battery_Charge_KW_MAX { get; set; }
    [Description("电池正极继电器状态")]
    /// <summary>
    /// 电池正极继电器状态
    /// </summary>
    public int? BMS_Positive_Relay { get; set; }
    [Description("电池组充电状态报警")]
    /// <summary>
    /// 电池组充电状态报警
    /// </summary>
    public int? BatteryPackChargingStatusAlarm { get; set; }
    [Description("电池组放电状态")]
    /// <summary>
    /// 电池组放电状态
    /// </summary>
    public int? BatteryPackDischargeState { get; set; }
    [Description("电池组温度状态")]
    /// <summary>
    /// 电池组温度状态
    /// </summary>
    public int? BatteryPackTempState { get; set; }
    [Description("电池组漏电状态")]
    /// <summary>
    /// 电池组漏电状态
    /// </summary>
    public int? BatteryPackElectricLeakageState { get; set; }
    [Description("电池组电量状态")]
    /// <summary>
    /// 电池组电量状态
    /// </summary>
    public int? BatteryPackElectricQuantityState { get; set; }
    [Description("过流状态")]
    /// <summary>
    /// 过流状态
    /// </summary>
    public int? OverCurrentState { get; set; }
    [Description("电压过低报警")]
    /// <summary>
    /// 电压过低报警
    /// </summary>
    public int? PowerBatteryUnderVoltage { get; set; }
    [Description("电压过高报警")]
    /// <summary>
    /// 电压过高报警
    /// </summary>
    public int? PowerBatteryPackOverVoltage { get; set; }
    [Description("动力电池报警")]
    /// <summary>
    /// 动力电池报警
    /// </summary>
    public int? PowerBatteryWarningSign { get; set; }
    [Description("主驾驶设定温度")]
    /// <summary>
    /// 主驾驶设定温度
    /// </summary>
    public int? MasterDriveSetTemperature { get; set; }
    [Description("副驾驶设定温度")]
    /// <summary>
    /// 副驾驶设定温度
    /// </summary>
    public int? AuxiliaryDrivingSetTemperature { get; set; }
    [Description("压缩机使能请求")]
    public int? req_CMC_Enable { get; set; }
    [Description("压缩机转速请求")]
    public int? CMCMotSpeed { get; set; }
    [Description("压缩机高压输入")]
    public double CMCHVVolt { get; set; }
    [Description("压缩机当前功率")]
    public double CMCPower { get; set; }
    [Description("压缩机当前转速")]
    public double CompSpeed { get; set; }
    [Description("压缩机控制器状态")]
    public int? CMCWork { get; set; }
    [Description("压缩机控制器自检故障")]
    public string CPR_CMCSelfCheck { get; set; }
    [Description("压缩机过压故障")]
    public string CPR_CMCOverVolt { get; set; }
    [Description("压缩机欠压故障")]
    public string CPR_CMCUnderVolt { get; set; }
    [Description("压缩机过流故障")]
    public string CPR_CompOverCurrent { get; set; }
    [Description("压缩机过温故障")]
    public string CPR_CompOverTemp { get; set; }
    [Description("压缩机堵转故障")]
    public string CPR_CompLocked { get; set; }
    [Description("压缩机缺相故障")]
    public string CPR_CompFailurePhase { get; set; }
    [Description("空调系统故障")]
    public string CPR_ACSysFault { get; set; }


    [Description("动力系统就绪")]
    public int? PowerSystemReady { get; set; }
    [Description("紧急下电请求")]
    public int? EmergencyPowerRequest { get; set; }
    [Description("电池均衡激活")]
    public int? BatteryBalancedActivation { get; set; }
    [Description("车辆当前状态")]
    public int? VehicleCurrentStatus { get; set; }
    [Description("液体燃料消耗量")]
    public int? LiquidFuelConsumption { get; set; }
}
