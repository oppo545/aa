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
    public int VCU_Keyposition { get; set; }
    [Description("扩展档位")]
    /// <summary>
    /// 扩展档位
    /// </summary>
    public int TCU_ExtensionGearPosition { get; set; }
    [Description("ECO模式")]
    /// <summary>
    /// ECO模式
    /// </summary>
    public int VCU_ECOMode { get; set; }
    [Description("正极绝缘电阻")]
    /// <summary>
    /// 正极绝缘电阻
    /// </summary>
    public int PositiveInsulationResistance { get; set; }
    [Description("负极绝缘电阻")]
    /// <summary>
    /// 负极绝缘电阻
    /// </summary>
    public int NegativeInsulationResistance { get; set; }
    [Description("油门深度(分辨率1%)")]
    /// <summary>
    /// 油门深度(分辨率1%)
    /// </summary>
    public int Motor_ThrottleDepth { get; set; }
    [Description("制动深度(分辨率1%)")]
    /// <summary>
    /// 制动深度(分辨率1%)
    /// </summary>
    public int BrakeDepth { get; set; }
    [Description(" 充、放电系统工作状态")]
    /// <summary>
    /// 充、放电系统工作状态
    /// </summary>
    public int SystemWorkState_Charge_Discharge { get; set; }
    [Description("母线电压")]
    /// <summary>
    /// 母线电压
    /// </summary>
    public double BusVoltage { get; set; }
    [Description("IPM散热器温度（分辨率1℃)")]
    /// <summary>
    /// IPM散热器温度（分辨率1℃)
    /// </summary>
    public int IPMRadiatorTemperature { get; set; }
    [Description(" IGBT温度（分辨率1℃）")]
    /// <summary>
    /// IGBT温度（分辨率1℃）
    /// </summary>
    public int IGBTTemperature { get; set; }
    [Description("动力电机母线电压")]
    /// <summary>
    /// 动力电机母线电压
    /// </summary>
    public double Motor_PowerBusVoltage { get; set; }
    [Description("冷却液温度")]
    /// <summary>
    /// 冷却液温度
    /// </summary>
    public int CoolantTemperature { get; set; }
    [Description("目标压力")]
    /// <summary>
    /// 目标压力
    /// </summary>
    public int TargetPressure { get; set; }
    [Description("后轿左气室压力")]
    /// <summary>
    /// 后轿左气室压力
    /// </summary>
    public int BackLeftAirChamberPressure { get; set; }
    [Description("真空泵状态")]
    /// <summary>
    /// 真空泵状态
    /// </summary>
    public int VacuumPumpState { get; set; }
    [Description("助力转向油泵状态")]
    /// <summary>
    /// 助力转向油泵状态
    /// </summary>
    public int PowerSteeringOilPumpState { get; set; }

    [Description("小计里程")]
    public double IC_Odmeter { get; set; }

    [Description("漏电检测")]
    public double BMS_CreepageMonitor { get; set; }
    [Description("SOC经过计算")]
    public double BMS_SOCCalculate { get; set; }
    [Description("外部充电信号")]
    public int BMS_OutsideChargeSignal { get; set; }
    [Description("非车载充电连接指示信号")]
    public int BMS_OFCConnectSignal { get; set; }
    [Description("制动开关状")]
    public int VCU_BrakePedalSt { get; set; }
    [Description("制动踏板电压信号")]
    public double BrakeVoltageSignal { get; set; }
    [Description("制动信号")]
    public int ABS_BrakeSignal { get; set; }
    [Description("制动开关有效信号")]
    public int VCU_BrakePedalSwitchValid { get; set; }

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
    public int Motor_ControllerRequest { get; set; }
    [Description("电机工作模式反馈")]
    /// <summary>
    /// 电机工作模式反馈
    /// </summary>
    public int Motor_WorkModeFeedback { get; set; }
    [Description("电机控制器工作状态")]
    public int MCU_ElecPowerTrainMngtState { get; set; }
    [Description("电机转子温度")]
    public int MCU_InternalMachineTemp { get; set; }
    [Description("电机最大电动扭矩")]
    public int MCU_MaxMotorTorque { get; set; }
    [Description("电机最大发电扭矩")]
    public int Motor_MaxGenTorque { get; set; }
    [Description("电机控制器母线电容放电状")]
    public int MCU_ActiveDischarge { get; set; }
    [Description("电机状态及信号强度")]
    public int VCU_BrakeEnergy { get; set; }
    [Description("电机状态")]
    public int Motor_State { get; set; }
    [Description("电机反馈扭矩")]
    public double Motor_TorqueFeedback { get; set; }
    [Description("电机最大允许扭矩")]
    public double Motor_AllowMaxTorque { get; set; }
    [Description("电机功率")]
    public double Motor_OutputPower { get; set; }
    [Description("充电机输出的充电电压")]
    public double ONC_OutputVoltage { get; set; }
    [Description("充电机输出的充电电流")]
    public double ONC_OutputCurrent { get; set; }
    [Description("充电机输入电压状态")]
    public int ONC_InputVoltageSt { get; set; }
    [Description("充电机通信状态")]
    public int CCS_ChargerCommunication { get; set; }
    [Description("充电机温度")]
    public int ONC_ONCTemp { get; set; }
    [Description("交流输入状态")]
    public int BMS_ChargerACInput { get; set; }
    [Description("DCDC状态")]
    /// <summary>
    /// DCDC状态
    /// </summary>
    public int DCDC_Work_State { get; set; }

    [Description("直流母线电压")]
    public double Motor_DCVolt { get; set; }

    [Description("直流母线电流")]
    public int Motor_DCCurrent { get; set; }
    [Description("DCDC温度")]
    public int DCDC_Temperature { get; set; }
    [Description("DC-DC输出电压")]
    public double DCDC_OutputVoltage { get; set; }
    [Description("DC-DC输出电流")]
    public double DCDC_OutputCurrent { get; set; }
    [Description("DC-DC使能应答")]
    public int DCDC_EnableResponse { get; set; }
    [Description("DC-DC输入电压")]
    public double DCDC_InputVoltage { get; set; }
    [Description("DC-DC输入电流")]
    public double DCDC_InputCurrent { get; set; }


    [Description("ABS故障")]
    /// <summary>
    /// ABS故障
    /// </summary>
    public int ABS_Fault { get; set; }
    [Description("空压机故障")]
    /// <summary>
    /// 空压机故障
    /// </summary>
    public int AirCompressor_Fault { get; set; }
    [Description("DCDC故障")]
    /// <summary>
    /// DCDC故障
    /// </summary>
    public int DCDC_Fault { get; set; }
    [Description("助力转向故障")]
    /// <summary>
    /// 助力转向故障
    /// </summary>
    public int PowerSteering_Fault { get; set; }
    [Description("冷却系统故障")]
    /// <summary>
    /// 冷却系统故障
    /// </summary>
    public int CoolingSystem_Fault { get; set; }
    [Description("VCU故障")]
    public int VCU_Fault { get; set; }

    [Description("电池故障")]
    public int BMS_FaultDislpay { get; set; }

    [Description("充电机故障码")]
    public int ONC_Fault { get; set; }

    [Description("电机故障类型")]
    public int MCU_ElecMachineFault { get; set; }



    [Description("CAN网络故障")]
    public int CANNetworkFailure { get; set; }

    [Description("电机控制器电压故障")]
    public int Motor_ControllersVoltageFault { get; set; }

    [Description("电机故障")]
    public int MCU_FaultDInternalInverterMotor { get; set; }

    [Description("DC/DC内部故障")]
    public int DCDC_InternalFault { get; set; }

    [Description("DC/DC输出短路故障")]
    public int DCDC_OutputCircuitFault { get; set; }

    [Description("DCDC故障类型")]
    public int DCDC_ElecMachineFault { get; set; }
    [Description("DC/DC输出过压故障")]
    public int DCDC_OutputOverVoltageFault { get; set; }


    [Description("TCU故障码")]
    public int TCU_Fault { get; set; }
    [Description("EPS故障报警")]
    public int EPS_FaultWarning { get; set; }

    #region E0报警
    [Description("电机控制器预充故障")]
    public int MotorControllerFault_Precharge { get; set; }
    [Description("电机控制器主接故障")]
    public int MotorControllerFault_MainContactor { get; set; }
    [Description("电机控制器IGBT故障")]
    public int MotorControllerFault_IGBTDamage { get; set; }
    [Description("电机控制器过流故障")]
    public int MotorControllerFault_OverCurrent { get; set; }
    [Description("电机控制器过温故障")]
    public int MotorControllerFault_OverTemperature { get; set; }

    [Description("电机控制器过压故障")]
    public int MotorControllerFault_OverVoltage { get; set; }

    [Description("电机控制器欠压故障")]
    public int MotorControllerFault_UnderVoltage { get; set; }

    [Description("电机控制器堵转故障")]
    public int MotorControllerFault_Blocking
    { get; set; }

    [Description("电机控制器超速故障")]
    public int MotorControllerFault_OverSpeed { get; set; }
    [Description("电机控制器电压上升故障")]
    public int MotorControllerFault_VoltageRise
    { get; set; }
    [Description("电机控制器电流上升故障")]
    public int MotorControllerFault_CurrentRise { get; set; }
    [Description("电机控制器总线故障")]
    public int MotorControllerFault_Bus { get; set; }

    [Description("自检异常")]
    public int SelfCheckAbnormal { get; set; }
    [Description("短路/绝缘检测（高压）")]
    public int MotorInsulationTest { get; set; }
    [Description("断路/开路（高压）")]
    public int MotorBreakerCircuit { get; set; }
    [Description("驱动电机过载")]
    public int Motor_VoltageOver { get; set; }
    [Description("驱动电机控制器温度过高")]
    public int Motor_ControllerTemperatureOver { get; set; }
    [Description("驱动电机控制器24V欠压")]
    public int Motor_Controller24VTemperatureLow { get; set; }
    [Description("旋变故障")]
    public int Motor_ResolverFault { get; set; }
    [Description("驱动电机输出缺相")]
    public int Motor_DriveOutputPhase { get; set; }

    [Description("电机故障码")]
    public int Motor_Fault { get; set; }


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


    [Description("温度过高报警（电池）")]
    public int BatteryFault_OverTemperature { get; set; }
    [Description("充电温度过低（电池）")]
    public int BatteryFault_ChargeTemperatureLow { get; set; }
    [Description("放电温度过低（电池）")]
    public int BatteryFault_DischargeTemperatureLow { get; set; }
    [Description("温差过大（电池）")]
    public int BatteryFault_TemperatureDifferenceLarge { get; set; }
    [Description("单体电压过高（电池）")]
    public int BatteryFault_MonomerVoltageHigh { get; set; }
    [Description("单体电压过低（电池）")]
    public int BatteryFault_MonomerVoltageLow { get; set; }
    [Description("压差过大（电池）")]
    public int BatteryFault_PressureDifferenceLarge { get; set; }
    [Description("SOC过低（电池）")]
    public int BatteryFault_SOCLow { get; set; }
    [Description("总电压过高")]
    public int BatteryFault_TotalVoltageHigh { get; set; }
    [Description("总电压过低")]
    public int BatteryFault_TotalVoltageLow { get; set; }
    [Description("充电电流过大")]
    public int BatteryFault_ChargingCurrentLarge { get; set; }
    [Description("放电电流过大")]
    public int BatteryFault_DischargeCurrentLarge { get; set; }
    [Description("绝缘故障")]
    public int BatteryFault_Insulation { get; set; }

    [Description("SOC过高（电池）")]
    public int SOCOvertopAlarm { get; set; }
    [Description("单体温度过低")]
    public int BMS_UnitTemperatureTooLow { get; set; }
    [Description("极柱温度过高")]
    public int BMS_PoleTemperatureOver { get; set; }
    [Description("极柱温度差异过大")]
    public int BMS_PoleTemperatureDifferenceOver { get; set; }
    [Description("BMS与充电桩通讯故障")]
    public int BMSCharging_CommunicationFault { get; set; }

    [Description("BMS_Fault")]
    public int BMS_Fault { get; set; }

    #endregion



    [Description("电池平均温度")]
    /// <summary>
    /// 电池平均温度
    /// </summary>
    public int BMS_Temp_Ave { get; set; }
    [Description("电池组当前容量指数")]
    /// <summary>
    /// 电池组当前容量指数 (分辨率1%) 
    /// </summary>
    public int BatteryPackCurrentCapacityIndex { get; set; }
    [Description("电池的健康指数")]
    /// <summary>
    /// 电池的健康指数 (分辨率1%) 

    /// </summary>
    public int BatteryHealthIndex { get; set; }
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

    [Description("电池组充电状态报警")]
    /// <summary>
    /// 电池组充电状态报警
    /// </summary>
    public int BatteryPackChargingStatusAlarm { get; set; }
    [Description("电池组放电状态")]
    /// <summary>
    /// 电池组放电状态
    /// </summary>
    public int BatteryPackDischargeState { get; set; }
    [Description("电池组温度状态")]
    /// <summary>
    /// 电池组温度状态
    /// </summary>
    public int BatteryPackTempState { get; set; }
    [Description("电池组漏电状态")]
    /// <summary>
    /// 电池组漏电状态
    /// </summary>
    public int BatteryPackElectricLeakageState { get; set; }
    [Description("电池组电量状态")]
    /// <summary>
    /// 电池组电量状态
    /// </summary>
    public int BatteryPackElectricQuantityState { get; set; }
    [Description("过流状态")]
    /// <summary>
    /// 过流状态
    /// </summary>
    public int OverCurrentState { get; set; }
    [Description("电压过低报警")]
    /// <summary>
    /// 电压过低报警
    /// </summary>
    public int PowerBatteryUnderVoltage { get; set; }
    [Description("电压过高报警")]
    /// <summary>
    /// 电压过高报警
    /// </summary>
    public int PowerBatteryPackOverVoltage { get; set; }
    [Description("动力电池报警")]
    /// <summary>
    /// 动力电池报警
    /// </summary>
    public int PowerBatteryWarningSign { get; set; }
    [Description("主驾驶设定温度")]
    /// <summary>
    /// 主驾驶设定温度
    /// </summary>
    public int MasterDriveSetTemperature { get; set; }
    [Description("副驾驶设定温度")]
    /// <summary>
    /// 副驾驶设定温度
    /// </summary>
    public int AuxiliaryDrivingSetTemperature { get; set; }




}
