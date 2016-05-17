using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

class Cls_ZzRc
{
}
public class Candata_ZzRuiQi : Candata
{

    #region VCU-0X08F60301
    /// <summary>
    /// 控制系统故障报警灯
    /// </summary>
    public int VCU_ControllerSystemFaultAlarmLamp { get; set; }

    /// <summary>
    /// 运行准备就绪指示灯
    /// </summary>
    public int VCU_ReadyLamp { get; set; }

    /// <summary>
    /// 动力蓄电池充电状态指示灯
    /// </summary>
    public int VCU_PowerStorageBatteryChargingStatusLamp { get; set; }

    /// <summary>
    /// 电机及控制器过热报警灯
    /// </summary>
    public int VCU_MotorControlsOverheatingAlarmLamp { get; set; }

    /// <summary>
    /// 动力蓄电池故障报警灯
    /// </summary>
    public int VCU_PowerStorageBatteryFaultAlarmLamp { get; set; }

    /// <summary>
    /// 充电线连接指示灯
    /// </summary>
    public int VCU_ChargeLight { get; set; }
    /// <summary>
    /// 动力蓄电池切断指示灯
    /// </summary>
    public int VCU_PowerStorageBatteryCutOffLamp { get; set; }
    /// <summary>
    /// 超速报警指示灯
    /// </summary>
    public int VCU_OverSpeedAlarmLamp { get; set; }
    /// <summary>
    /// 档位D指示灯
    /// </summary>
    public int VCU_GearsDLamp { get; set; }
    /// <summary>
    /// 档位R指示灯
    /// </summary>
    public int VCU_GearsRLamp { get; set; }
    /// <summary>
    /// 档位N指示灯
    /// </summary>
    public int VCU_GearsNLamp { get; set; }
    /// <summary>
    /// EPS指示灯
    /// </summary>
    public int VCU_EPSLamp { get; set; }
    /// <summary>
    /// 低压蓄电池亏电报警指示灯
    /// </summary>
    public int VCU_LowBatteryLossAlarmLamp { get; set; }
    /// <summary>
    /// 手制动报警灯
    /// </summary>
    public int VCU_HandBrakeWarningLight { get; set; }
    /// <summary>
    /// 动力蓄电池亏电指示灯
    /// </summary>
    public int VCU_PowerStorageBatteryLossLamp { get; set; }

    /// <summary>
    /// 真空度低
    /// </summary>
    public int VCU_lowvacuum { get; set; }

    /// <summary>
    /// PTC开关
    /// </summary>
    public int VCU_PTCSwitch { get; set; }
    /// <summary>
    /// A/C开关
    /// </summary>
    public int VCU_ACSwitch { get; set; }
    /// <summary>
    /// 真空泵使能
    /// </summary>
    public int VCU_Vacuumpumpcapability { get; set; }
    /// <summary>
    /// 转向使能
    /// </summary>
    public int VCU_SteeringCapability { get; set; }
    /// <summary>
    /// DC/DC使能
    /// </summary>
    public int VCU_DCDCCapability { get; set; }

    /// <summary>
    /// 预充电完成
    /// </summary>
    public int VCU_Prechargingcompleted { get; set; }
    /// <summary>
    /// 整车工作模式
    /// </summary>
    public int VCU_VehicleWorkingMode { get; set; }
    /// <summary>
    /// 刹车踏板2错误
    /// </summary>
    public int VCU_BrakePedal2Error { get; set; }
    /// <summary>
    /// 刹车踏板1错误
    /// </summary>
    public int VCU_BrakePedal1Error { get; set; }
    /// <summary>
    /// 油门2故障
    /// </summary>
    public int VCU_Throttle2Failure { get; set; }
    /// <summary>
    /// 油门1故障
    /// </summary>
    public int VCU_Throttle1Failure { get; set; }
    /// <summary>
    /// 转向故障
    /// </summary>
    public int VCU_SteeringFailure { get; set; }
    /// <summary>
    /// PTC过温
    /// </summary>
    public int VCU_PTCOverTemp { get; set; }
    #endregion

    #region VCU-0X0CF10BD0
    /// <summary>
    /// 钥匙开关状态
    /// </summary>
    public int VCU_Keyposition { get; set; }
    /// <summary>
    /// 刹车踏板状态
    /// </summary>
    public int VCU_BrakePedalStatus { get; set; }
    /// <summary>
    /// 低压蓄电池故障（预留）
    /// </summary>
    public int VCU_StorageBatteryLowFault { get; set; }
    /// <summary>
    /// 油门大小开度
    /// </summary>
    public double Motor_Throttle { get; set; }
    #endregion

    #region VCU-0X0CF10CD0
    /// <summary>
    /// 故障运行模式
    /// </summary>
    public int VCU_FaultOperationMode { get; set; }
    /// <summary>
    /// 换档允许
    /// </summary>
    public int VCU_ShiftGearsAllows { get; set; }
    /// <summary>
    /// BMU故障
    /// </summary>
    public int VCU_BMUFault { get; set; }
    /// <summary>
    /// VCU故障
    /// </summary>
    public int VCU_Fault { get; set; }
    /// <summary>
    /// MCU故障
    /// </summary>
    public int VCU_MCUFault { get; set; }
    /// <summary>
    /// 电机工作模式
    /// </summary>
    public int VCU_MotorWorkingMode { get; set; }
    /// <summary>
    /// 清除历史故障
    /// </summary>
    public int VCU_ClearHistoricalFault { get; set; }
    /// <summary>
    /// 清除当前故障	
    /// </summary>
    public int VCU_ClearCurrentFault { get; set; }
    /// <summary>
    /// 自学习允许
    /// </summary>
    public int VCU_SelfStudyAllows { get; set; }
    /// <summary>
    /// 变速箱挂空挡指令
    /// </summary>
    public int VCU_GearboxIdlingInstructions { get; set; }
    /// <summary>
    /// 离合器分离指令
    /// </summary>
    public int VCU_ClutchSeparateInstructions { get; set; }
    /// <summary>
    /// K系数
    /// </summary>
    public double VCU_KRatio { get; set; }

    #endregion

    #region VCU-0X08F54101
    /// <summary>
    /// 充电机状态
    /// </summary>
    public int ONC_Start_Stop { get; set; }

    #endregion

    #region VCU-0X0CEF00D0
    /// <summary>
    /// VCU_主电机转速
    /// </summary>
    public int VCU_Revolution { get; set; }
    /// <summary>
    /// VCU_主电机转矩
    /// </summary>
    public double VCU_MotorTorque { get; set; }
    /// <summary>
    /// VCU_电机驱动指令
    /// </summary>
    public int VCU_MotorDrivenInstruction { get; set; }
    /// <summary>
    /// VCU_电机旋转方向
    /// </summary>
    public int VCU_MotorDirectionOfRotation { get; set; }
    /// <summary>
    /// 速度力矩切换
    /// </summary>
    public int VCU_SpeedTorqueSwitch { get; set; }
    /// <summary>
    /// 电机复位
    /// </summary>
    public int VCU_MotorReset { get; set; }
    /// <summary>
    /// DCDC使能信号
    /// </summary>
    public int VCU_DCDCEnableSignal { get; set; }
    /// <summary>
    /// 转向系统控制器复位	
    /// </summary>
    public int VCU_SteeringControllerReset { get; set; }
    /// <summary>
    /// 转向系统控制器运行
    /// </summary>
    public int VCU_SteeringControllerRunning { get; set; }
    /// <summary>
    /// 制动系统控制器复位
    /// </summary>
    public int VCU_BrakingSystemControllerReset { get; set; }
    /// <summary>
    /// 制动系统控制器运行
    /// </summary>
    public int VCU_BrakingSystemControllerRunning { get; set; }

    #endregion

    #region VCU-0X1801D003
    /// <summary>
    /// 目标档位
    /// </summary>
    public int TCU_TargetGear { get; set; }
    /// <summary>
    /// 当前档位
    /// </summary>
    public int TCU_CurrentGear { get; set; }
    /// <summary>
    /// 离合器状态
    /// </summary>
    public int TCU_ClutchState { get; set; }
    /// <summary>
    /// 档位器位置
    /// </summary>
    public int TCU_GearPosition { get; set; }
    /// <summary>
    /// TCU计数器
    /// </summary>
    public int TCU_Counter { get; set; }
    /// <summary>
    /// 输入轴转速
    /// </summary>
    public int TCU_InputShaftRotatingSpeed { get; set; }
    /// <summary>
    /// 输出轴转速
    /// </summary>
    public int TCU_OutputShaftRotatingSpeed { get; set; }
    /// <summary>
    /// 车速	
    /// </summary>
    public double ABS_VehSpd { get; set; }

    #endregion

    #region VCU-0X1802D003
    /// <summary>
    /// TCU故障
    /// </summary>
    public int TCU_Fault { get; set; }
    /// <summary>
    /// 自学习请求
    /// </summary>
    public int TCU_SelfStutyRequest { get; set; }
    /// <summary>
    /// 换档状态
    /// </summary>
    public int TCU_ShiftGearsState { get; set; }
    /// <summary>
    /// 换档模式
    /// </summary>
    public int TCU_ShiftGearsMode { get; set; }
    /// <summary>
    /// 电机自由模式请求
    /// </summary>
    public int TCU_MotorFreePatternRequest { get; set; }
    /// <summary>
    /// 转速请求有效位
    /// </summary>
    public int TCU_RotatingSpeedRequestSignificantBit { get; set; }
    /// <summary>
    /// 扭矩请求有效位	
    /// </summary>
    public int TCU_TorqueRequestSignificantBit { get; set; }
    /// <summary>
    /// 电机目标转速	
    /// </summary>
    public int VCU_Motor_Target_rpm { get; set; }
    /// <summary>
    /// 电机目标扭矩	
    /// </summary>
    public double VCU_Motor_Target_tq { get; set; }

    /// <summary>
    /// TCU故障代码	
    /// </summary>
    public int TCU_FaultCode { get; set; }
    /// <summary>
    /// 正常/运动模式	
    /// </summary>
    public int TCU_NormalMovementPatterns { get; set; }
    /// <summary>
    /// 刹车开关	
    /// </summary>
    public int TCU_BrakeSwitch { get; set; }
    /// <summary>
    /// PD开关	
    /// </summary>
    public int TCU_PDSwitch { get; set; }
    /// <summary>
    /// Sport开关	
    /// </summary>
    public int TCU_SportSwitch { get; set; }
    /// <summary>
    /// 自学习条件满足	
    /// </summary>
    public int TCU_SelfStudyConditionMet { get; set; }
    /// <summary>
    /// 自学习状态	
    /// </summary>
    public int TCU_SelfStudyStatus { get; set; }

    #endregion

    #region TCU-0X1803D003
    /// <summary>
    /// 总计里程	
    /// </summary>
    public int IC_TotalOdmeter { get; set; }
    #endregion

    [Description("主电机转速")]
    /// <summary>
    /// 主电机转速
    /// </summary>
    public int Motor_Revolution { get; set; }


    [Description("主电机转矩")]
    /// <summary>
    /// 主电机转矩
    /// </summary>
    public double Motor_OutputTorque { get; set; }


    [Description("母线电压")]
    /// <summary>
    /// 母线电压
    /// </summary>
    public double BusVoltage { get; set; }


    [Description("电机驱动指令")]
    /// <summary>
    /// 电机驱动指令
    /// </summary>
    public int MCU_MotorDrivenInstruction { get; set; }

    [Description("电机旋转方向")]
    /// <summary>
    /// 电机旋转方向
    /// </summary>
    public int MCU_MotorDirectionOfRotation { get; set; }

    [Description("电机温度1")]
    /// <summary>
    /// 电机温度1
    /// </summary>
    public int Motor_Temperature { get; set; }

    [Description("电机温度2（预留）")]
    /// <summary>
    /// 电机温度2（预留）
    /// </summary>
    public int Motor_Temperature2 { get; set; }

    [Description("控制器温度1")]
    /// <summary>
    /// 控制器温度1
    /// </summary>
    public int BMS_Temperature { get; set; }

    [Description("控制器温度2（预留）")]
    /// <summary>
    /// 控制器温度2（预留）
    /// </summary>
    public int Motor_ControllerTemp2 { get; set; }

    [Description("电机相电流")]
    /// <summary>
    /// 电机相电流
    /// </summary>
    public double Motor_PhaseCurrent { get; set; }



    [Description("电机三相电压")]
    /// <summary>
    /// 电机三相电压
    /// </summary>
    public double Motor_ThreePhaseVoltage { get; set; }


    [Description("重大故障")]
    /// <summary>
    /// 重大故障
    /// </summary>
    public int MCU_Major_Fault { get; set; }

    [Description("通信故障")]
    /// <summary>
    /// 通信故障
    /// </summary>
    public int MCU_Communication_Fault { get; set; }

    [Description("编码器状态")]
    /// <summary>
    /// 编码器状态
    /// </summary>
    public int MCU_EncoderState { get; set; }

    [Description("母线电压状态")]
    /// <summary>
    /// 母线电压状态
    /// </summary>
    public int MCU_BusVoltageState { get; set; }

    [Description("控制器过温状态")]
    /// <summary>
    /// 控制器过温状态
    /// </summary>
    public int MCU_ControllerOverTempState { get; set; }

    [Description("EEPROM异常")]
    /// <summary>
    /// EEPROM异常
    /// </summary>
    public int MCU_EEPROMException { get; set; }

    [Description("控制器过载")]
    /// <summary>
    /// 控制器过载
    /// </summary>
    public int MCU_ControllerOverload { get; set; }

    [Description("电机过载")]
    /// <summary>
    /// 电机过载
    /// </summary>
    public int MCU_MotorOverload { get; set; }

    [Description("过流状态")]
    /// <summary>
    /// 过流状态
    /// </summary>
    public int MCU_OverCurrentState { get; set; }

    [Description("CT电流传感器故障")]
    /// <summary>
    /// CT电流传感器故障
    /// </summary>
    public int MCU_CurrentSensorFault { get; set; }

    [Description("电机过温状态")]
    /// <summary>
    /// 电机过温状态
    /// </summary>
    public int MCU_MotorThermalState { get; set; }


    [Description("轻度故障")]
    /// <summary>
    /// 轻度故障
    /// </summary>
    public int MCU_MinorFault { get; set; }

    [Description("变频器过热预报")]
    /// <summary>
    /// 变频器过热预报
    /// </summary>
    public int MCU_InverterOverheatingPrediction { get; set; }

    [Description("变频器过负荷预报")]
    /// <summary>
    /// 变频器过负荷预报
    /// </summary>
    public int MCU_FrequencyLoadForecasting { get; set; }

    [Description("DC母线电压欠压预报")]
    /// <summary>
    /// DC母线电压欠压预报
    /// </summary>
    public int MCU_DCBusVoltageUnderVoltagePrediction { get; set; }

    [Description("DC母线电压过压预报")]
    /// <summary>
    /// DC母线电压过压预报
    /// </summary>
    public int MCU_DCBusVoltageOverVoltagePrediction { get; set; }

    [Description("CCW超速度")]
    /// <summary>
    /// CCW超速度
    /// </summary>
    public int MCU_CCWSupervelocity { get; set; }

    [Description("CW超速度")]
    /// <summary>
    /// CW超速度
    /// </summary>
    public int MCU_CWSupervelocity { get; set; }

    [Description("变频器准备就绪")]
    /// <summary>
    /// 变频器准备就绪
    /// </summary>
    public int MCU_InverterReady { get; set; }

    [Description("预充电完成信号")]
    /// <summary>
    /// 预充电完成信号
    /// </summary>
    public int MCU_PrechargeCompleteSignal { get; set; }

    [Description("MCU计数器")]
    /// <summary>
    /// MCU计数器
    /// </summary>
    public int MCU_Counter { get; set; }

    [Description("累计运行时间")]
    /// <summary>
    /// 累计运行时间
    /// </summary>
    public int MCU_TotalRunTime { get; set; }


    [Description("蓄电池编号")]
    /// <summary>
    /// 蓄电池编号
    /// </summary>
    public double BMS_BatteryNumber { get; set; }

    [Description("蓄电池系统模块总数量")]
    /// <summary>
    /// 蓄电池系统模块总数量
    /// </summary>
    public int BMS_TotalNumberOfBatteryModules { get; set; }

    [Description("蓄电池充电次数")]
    /// <summary>
    /// 蓄电池充电次数
    /// </summary>
    public int BMS_BatteryChargingTimes { get; set; }


    [Description("电池充电状态")]
    /// <summary>
    /// 电池充电状态
    /// </summary>
    public int BMS_ChargeSt { get; set; }

    [Description("蓄电池输入输出功率")]
    /// <summary>
    /// 蓄电池输入输出功率
    /// </summary>
    public int BMS_BatteryInputOutputPower { get; set; }


    [Description("蓄电池系统电池框数量")]
    /// <summary>
    /// 蓄电池系统电池框数量
    /// </summary>
    public int BMS_BatterySystemBatteryBoxNumber { get; set; }


    [Description("电池模块唯一编号")]
    /// <summary>
    /// 电池模块唯一编号
    /// </summary>
    public int BMS_BatteryModuleOnlyNumber { get; set; }


    [Description("所在电池组中的模块号")]
    /// <summary>
    /// 所在电池组中的模块号
    /// </summary>
    public int BMS_ModuleNumberInBatteryPack { get; set; }


    [Description("蓄电池模块号")]
    /// <summary>
    /// 蓄电池模块号
    /// </summary>
    public int BMS_BatteryModule { get; set; }

    [Description("模块内单体电池数")]
    /// <summary>
    /// 模块内单体电池数
    /// </summary>
    public int BMS_SingleBatteryModuleNumber { get; set; }

    [Description("模块内温度采样点数")]
    /// <summary>
    /// 模块内温度采样点数
    /// </summary>
    public int BMS_TheNumberOfTemperatureSamplingModule { get; set; }

    [Description("模块SOC")]
    /// <summary>
    /// 模块SOC
    /// </summary>
    public double BMS_SOCModule { get; set; }

    [Description("模块充电次数")]
    /// <summary>
    /// 模块充电次数
    /// </summary>
    public int BMS_ChargingModuleNumber { get; set; }


    [Description("模块总电流")]
    /// <summary>
    /// 模块总电流
    /// </summary>
    public double BMS_TotalCurrentModule { get; set; }


    [Description("模块总电压")]
    /// <summary>
    /// 模块总电压
    /// </summary>
    public double BMS_TotalVoltageModule { get; set; }


    [Description("模块内单体最低电压")]
    /// <summary>
    /// 模块内单体最低电压
    /// </summary>
    public double BMS_ModuleOfMonomerInTheLowestVoltage { get; set; }


    [Description("模块内单体最高电压")]
    /// <summary>
    /// 模块内单体最高电压
    /// </summary>
    public double BMS_ModuleOfMonomerInTheHighestVoltage { get; set; }


    [Description("过压")]
    /// <summary>
    /// 过压
    /// </summary>
    public int BMS_OverVoltage { get; set; }

    [Description("欠压")]
    /// <summary>
    /// 欠压
    /// </summary>
    public int BMS_UnderVoltage { get; set; }

    [Description("过温")]
    /// <summary>
    /// 过温
    /// </summary>
    public int BMS_OverTemperature { get; set; }

    [Description("欠温")]
    /// <summary>
    /// 欠温
    /// </summary>
    public int BMS_LackOfWarmth { get; set; }


    [Description("内部通讯故障")]
    /// <summary>
    /// 内部通讯故障
    /// </summary>
    public int BMS_InternalCommunicationFault { get; set; }

    [Description("模块内最低温度")]
    /// <summary>
    /// 模块内最低温度
    /// </summary>
    public int BMS_LowestTemperatureModule { get; set; }

    [Description("模块内最高温度")]
    /// <summary>
    /// 模块内最高温度
    /// </summary>
    public int BMS_HighestTemperatureModule { get; set; }

    [Description("电压最低单体号")]
    /// <summary>
    /// 电压最低单体号
    /// </summary>
    public int BMS_LowestVoltageMonomer { get; set; }

    [Description("电压最高单体号")]
    /// <summary>
    /// 电压最高单体号
    /// </summary>
    public int BMS_HighestVoltageMonomer { get; set; }

    [Description("最低温度采样点号")]
    /// <summary>
    /// 最低温度采样点号
    /// </summary>
    public int BMS_LowestTemperatureSamplingPeriod { get; set; }

    [Description("最高温度采样点号")]
    /// <summary>
    /// 最高温度采样点号
    /// </summary>
    public int BMS_HighestTemperatureSamplingPeriod { get; set; }

    [Description("蓄电池荷电状态（SOC）")]
    /// <summary>
    /// 蓄电池荷电状态（SOC）
    /// </summary>
    public double BMS_SOC { get; set; }

    [Description("动力电池总电压")]
    /// <summary>
    /// 动力电池总电压
    /// </summary>
    public double BMS_Voltage { get; set; }


    [Description("动力电池总电流")]
    /// <summary>
    /// 动力电池总电流
    /// </summary>
    public double BMS_Current { get; set; }


    [Description("BMS程序版本信息")]
    /// <summary>
    /// BMS程序版本信息
    /// </summary>
    public int BMS_VersionOfTheBMSProgramInformation { get; set; }


    [Description("电池类型")]
    /// <summary>
    /// 电池类型
    /// </summary>
    public int BMS_BatteryTypes { get; set; }

    [Description("高压漏电报警")]
    /// <summary>
    /// 高压漏电报警
    /// </summary>
    public int BMS_HighPressureLeakageAlarm { get; set; }

    [Description("高温报警")]
    /// <summary>
    /// 高温报警
    /// </summary>
    public int BMS_HighTemperatureAlarm { get; set; }

    [Description("温度差异报警")]
    /// <summary>
    /// 温度差异报警
    /// </summary>
    public int BMS_TemperatureDifferenceAlarm { get; set; }

    [Description("放电电流报警")]
    /// <summary>
    /// 放电电流报警
    /// </summary>
    public int BMS_DischargeCurrentAlarm { get; set; }

    [Description("充电电流报警")]
    /// <summary>
    /// 充电电流报警
    /// </summary>
    public int BMS_ChargingCurrentAlarm { get; set; }

    [Description("电池组过压报警")]
    /// <summary>
    /// 电池组过压报警
    /// </summary>
    public int BMS_BatteryPackOverVoltageAlarm { get; set; }

    [Description("单体过压报警")]
    /// <summary>
    /// 单体过压报警
    /// </summary>
    public int BMS_MonomerOverVoltageAlarm { get; set; }

    [Description("SOC低报警")]
    /// <summary>
    /// SOC低报警
    /// </summary>
    public int BMS_SOCLowAlarm { get; set; }

    [Description("SOC差异报警")]
    /// <summary>
    /// SOC差异报警
    /// </summary>
    public int BMS_SOCDifferenceAlarm { get; set; }

    [Description("电池组欠压")]
    /// <summary>
    /// 电池组欠压
    /// </summary>
    public int BMS_BatteryPackUnderVoltage { get; set; }

    [Description("单体欠压")]
    /// <summary>
    /// 单体欠压
    /// </summary>
    public int BMS_MonomerUnderVoltage { get; set; }

    [Description("极柱温度高报警")]
    /// <summary>
    /// 极柱温度高报警
    /// </summary>
    public int BMS_PoleHighTemperatureAlarm { get; set; }

    [Description("电池欠温报警")]
    /// <summary>
    /// 电池欠温报警
    /// </summary>
    public int BMS_BatteryLowTemperatureAlarm { get; set; }

    [Description("单体电压差异报警")]
    /// <summary>
    /// 单体电压差异报警
    /// </summary>
    public int BMS_MonomerVoltageDifferenceAlarm { get; set; }

    [Description("SOC高报警")]
    /// <summary>
    /// SOC高报警
    /// </summary>
    public int BMS_SOCHighAlarm { get; set; }

    [Description("通信生命周期计数")]
    /// <summary>
    /// 通信生命周期计数
    /// </summary>
    public int BMS_CommunicationLifeCycleCounting { get; set; }

    [Description("与LECU通信报警")]
    /// <summary>
    /// 与LECU通信报警
    /// </summary>
    public int BMS_LECUCommunicationAlarm { get; set; }

    [Description("电池不同组报警")]
    /// <summary>
    /// 电池不同组报警
    /// </summary>
    public int BMS_DifferentBatteryAlarm { get; set; }

    [Description("动力电池状态")]
    /// <summary>
    /// 动力电池状态
    /// </summary>
    public int BMS_BatteryStatus { get; set; }

    [Description("动力电池亏电状态")]
    /// <summary>
    /// 动力电池亏电状态
    /// </summary>
    public int BMS_BatteryPowerLossStatus { get; set; }

    [Description("极柱温度高状态")]
    /// <summary>
    /// 极柱温度高状态
    /// </summary>
    public int BMS_PoleHighTemperatureState { get; set; }

    [Description("均衡状态")]
    /// <summary>
    /// 均衡状态
    /// </summary>
    public int BMS_BalancedState { get; set; }

    [Description("均衡报警状态")]
    /// <summary>
    /// 均衡报警状态
    /// </summary>
    public int BMS_BalancedAlarmState { get; set; }

    [Description("加热状态")]
    /// <summary>
    /// 加热状态
    /// </summary>
    public int BMS_HeatingState { get; set; }

    [Description("加热报警状态")]
    /// <summary>
    /// 加热报警状态
    /// </summary>
    public int BMS_HeatingAlarmState { get; set; }

    [Description("动力电池预报")]
    /// <summary>
    /// 动力电池预报
    /// </summary>
    public int BMS_PowerBatteryForecast { get; set; }

    [Description("当前放电功率")]
    /// <summary>
    /// 当前放电功率
    /// </summary>
    public double BMS_DischargePower { get; set; }


    [Description("已放电时间")]
    /// <summary>
    /// 已放电时间
    /// </summary>
    public int BMS_DischargeTime { get; set; }


    [Description("已放电容量")]
    /// <summary>
    /// 已放电容量
    /// </summary>
    public double BMS_DischargeCapacity { get; set; }


    [Description("剩余容量")]
    /// <summary>
    /// 剩余容量
    /// </summary>
    public double BMS_ResidualCapacity { get; set; }



    [Description("加热模块状态")]
    /// <summary>
    /// 加热模块状态
    /// </summary>
    public int BMS_HeatingModuleState { get; set; }

    [Description("加热模块故障状态")]
    /// <summary>
    /// 加热模块故障状态
    /// </summary>
    public int BMS_HeatingModuleFaultState { get; set; }

    [Description("内部均衡状态")]
    /// <summary>
    /// 内部均衡状态
    /// </summary>
    public int BMS_InternalEquilibrium { get; set; }

    [Description("内部均衡故障")]
    /// <summary>
    /// 内部均衡故障
    /// </summary>
    public int BMS_InternalEquilibriumFault { get; set; }

    [Description("外部均衡状态")]
    /// <summary>
    /// 外部均衡状态
    /// </summary>
    public int BMS_ExternalEquilibrium { get; set; }

    [Description("外部均衡故障")]
    /// <summary>
    /// 外部均衡故障
    /// </summary>
    public int BMS_ExternalEquilibriumFault { get; set; }

    [Description("已充电时间")]
    /// <summary>
    /// 已充电时间
    /// </summary>
    public int BMS_ChargingTime { get; set; }


    [Description("剩余充电时间")]
    /// <summary>
    /// 剩余充电时间
    /// </summary>
    public int BMS_RemainingChargingTime { get; set; }


    [Description("最大充电电流限制")]
    /// <summary>
    /// 最大充电电流限制
    /// </summary>
    public int BMS_MaximumChargeCurrentLimit { get; set; }


    [Description("最大放电电流限制")]
    /// <summary>
    /// 最大放电电流限制
    /// </summary>
    public int BMS_MaximumDischargeCurrentLimit { get; set; }

    #region 128节电池电压
    [Description("第1节电池电压")]
    /// <summary>
    /// 第1节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage1 { get; set; }
    [Description("第2节电池电压")]
    /// <summary>
    /// 第2节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage2 { get; set; }
    [Description("第3节电池电压")]
    /// <summary>
    /// 第3节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage3 { get; set; }
    [Description("第4节电池电压")]
    /// <summary>
    /// 第4节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage4 { get; set; }
    [Description("第5节电池电压")]
    /// <summary>
    /// 第5节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage5 { get; set; }
    [Description("第6节电池电压")]
    /// <summary>
    /// 第6节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage6 { get; set; }
    [Description("第7节电池电压")]
    /// <summary>
    /// 第7节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage7 { get; set; }
    [Description("第8节电池电压")]
    /// <summary>
    /// 第8节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage8 { get; set; }
    [Description("第9节电池电压")]
    /// <summary>
    /// 第9节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage9 { get; set; }
    [Description("第10节电池电压")]
    /// <summary>
    /// 第10节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage10 { get; set; }
    [Description("第11节电池电压")]
    /// <summary>
    /// 第11节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage11 { get; set; }
    [Description("第12节电池电压")]
    /// <summary>
    /// 第12节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage12 { get; set; }
    [Description("第13节电池电压")]
    /// <summary>
    /// 第13节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage13 { get; set; }
    [Description("第14节电池电压")]
    /// <summary>
    /// 第14节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage14 { get; set; }
    [Description("第15节电池电压")]
    /// <summary>
    /// 第15节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage15 { get; set; }
    [Description("第16节电池电压")]
    /// <summary>
    /// 第16节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage16 { get; set; }
    [Description("第17节电池电压")]
    /// <summary>
    /// 第17节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage17 { get; set; }
    [Description("第18节电池电压")]
    /// <summary>
    /// 第18节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage18 { get; set; }
    [Description("第19节电池电压")]
    /// <summary>
    /// 第19节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage19 { get; set; }
    [Description("第20节电池电压")]
    /// <summary>
    /// 第20节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage20 { get; set; }
    [Description("第21节电池电压")]
    /// <summary>
    /// 第21节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage21 { get; set; }
    [Description("第22节电池电压")]
    /// <summary>
    /// 第22节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage22 { get; set; }
    [Description("第23节电池电压")]
    /// <summary>
    /// 第23节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage23 { get; set; }
    [Description("第24节电池电压")]
    /// <summary>
    /// 第24节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage24 { get; set; }
    [Description("第25节电池电压")]
    /// <summary>
    /// 第25节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage25 { get; set; }
    [Description("第26节电池电压")]
    /// <summary>
    /// 第26节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage26 { get; set; }
    [Description("第27节电池电压")]
    /// <summary>
    /// 第27节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage27 { get; set; }
    [Description("第28节电池电压")]
    /// <summary>
    /// 第28节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage28 { get; set; }
    [Description("第29节电池电压")]
    /// <summary>
    /// 第29节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage29 { get; set; }
    [Description("第30节电池电压")]
    /// <summary>
    /// 第30节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage30 { get; set; }
    [Description("第31节电池电压")]
    /// <summary>
    /// 第31节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage31 { get; set; }
    [Description("第32节电池电压")]
    /// <summary>
    /// 第32节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage32 { get; set; }
    [Description("第33节电池电压")]
    /// <summary>
    /// 第33节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage33 { get; set; }
    [Description("第34节电池电压")]
    /// <summary>
    /// 第34节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage34 { get; set; }
    [Description("第35节电池电压")]
    /// <summary>
    /// 第35节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage35 { get; set; }
    [Description("第36节电池电压")]
    /// <summary>
    /// 第36节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage36 { get; set; }
    [Description("第37节电池电压")]
    /// <summary>
    /// 第37节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage37 { get; set; }
    [Description("第38节电池电压")]
    /// <summary>
    /// 第38节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage38 { get; set; }
    [Description("第39节电池电压")]
    /// <summary>
    /// 第39节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage39 { get; set; }
    [Description("第40节电池电压")]
    /// <summary>
    /// 第40节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage40 { get; set; }
    [Description("第41节电池电压")]
    /// <summary>
    /// 第41节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage41 { get; set; }
    [Description("第42节电池电压")]
    /// <summary>
    /// 第42节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage42 { get; set; }
    [Description("第43节电池电压")]
    /// <summary>
    /// 第43节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage43 { get; set; }
    [Description("第44节电池电压")]
    /// <summary>
    /// 第44节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage44 { get; set; }
    [Description("第45节电池电压")]
    /// <summary>
    /// 第45节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage45 { get; set; }
    [Description("第46节电池电压")]
    /// <summary>
    /// 第46节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage46 { get; set; }
    [Description("第47节电池电压")]
    /// <summary>
    /// 第47节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage47 { get; set; }
    [Description("第48节电池电压")]
    /// <summary>
    /// 第48节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage48 { get; set; }
    [Description("第49节电池电压")]
    /// <summary>
    /// 第49节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage49 { get; set; }
    [Description("第50节电池电压")]
    /// <summary>
    /// 第50节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage50 { get; set; }
    [Description("第51节电池电压")]
    /// <summary>
    /// 第51节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage51 { get; set; }
    [Description("第52节电池电压")]
    /// <summary>
    /// 第52节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage52 { get; set; }
    [Description("第53节电池电压")]
    /// <summary>
    /// 第53节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage53 { get; set; }
    [Description("第54节电池电压")]
    /// <summary>
    /// 第54节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage54 { get; set; }
    [Description("第55节电池电压")]
    /// <summary>
    /// 第55节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage55 { get; set; }
    [Description("第56节电池电压")]
    /// <summary>
    /// 第56节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage56 { get; set; }
    [Description("第57节电池电压")]
    /// <summary>
    /// 第57节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage57 { get; set; }
    [Description("第58节电池电压")]
    /// <summary>
    /// 第58节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage58 { get; set; }
    [Description("第59节电池电压")]
    /// <summary>
    /// 第59节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage59 { get; set; }
    [Description("第60节电池电压")]
    /// <summary>
    /// 第60节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage60 { get; set; }
    [Description("第61节电池电压")]
    /// <summary>
    /// 第61节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage61 { get; set; }
    [Description("第62节电池电压")]
    /// <summary>
    /// 第62节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage62 { get; set; }
    [Description("第63节电池电压")]
    /// <summary>
    /// 第63节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage63 { get; set; }
    [Description("第64节电池电压")]
    /// <summary>
    /// 第64节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage64 { get; set; }
    [Description("第65节电池电压")]
    /// <summary>
    /// 第65节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage65 { get; set; }
    [Description("第66节电池电压")]
    /// <summary>
    /// 第66节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage66 { get; set; }
    [Description("第67节电池电压")]
    /// <summary>
    /// 第67节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage67 { get; set; }
    [Description("第68节电池电压")]
    /// <summary>
    /// 第68节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage68 { get; set; }
    [Description("第69节电池电压")]
    /// <summary>
    /// 第69节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage69 { get; set; }
    [Description("第70节电池电压")]
    /// <summary>
    /// 第70节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage70 { get; set; }
    [Description("第71节电池电压")]
    /// <summary>
    /// 第71节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage71 { get; set; }
    [Description("第72节电池电压")]
    /// <summary>
    /// 第72节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage72 { get; set; }
    [Description("第73节电池电压")]
    /// <summary>
    /// 第73节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage73 { get; set; }
    [Description("第74节电池电压")]
    /// <summary>
    /// 第74节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage74 { get; set; }
    [Description("第75节电池电压")]
    /// <summary>
    /// 第75节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage75 { get; set; }
    [Description("第76节电池电压")]
    /// <summary>
    /// 第76节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage76 { get; set; }
    [Description("第77节电池电压")]
    /// <summary>
    /// 第77节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage77 { get; set; }
    [Description("第78节电池电压")]
    /// <summary>
    /// 第78节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage78 { get; set; }
    [Description("第79节电池电压")]
    /// <summary>
    /// 第79节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage79 { get; set; }
    [Description("第80节电池电压")]
    /// <summary>
    /// 第80节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage80 { get; set; }
    [Description("第81节电池电压")]
    /// <summary>
    /// 第81节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage81 { get; set; }
    [Description("第82节电池电压")]
    /// <summary>
    /// 第82节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage82 { get; set; }
    [Description("第83节电池电压")]
    /// <summary>
    /// 第83节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage83 { get; set; }
    [Description("第84节电池电压")]
    /// <summary>
    /// 第84节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage84 { get; set; }
    [Description("第85节电池电压")]
    /// <summary>
    /// 第85节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage85 { get; set; }
    [Description("第86节电池电压")]
    /// <summary>
    /// 第86节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage86 { get; set; }
    [Description("第87节电池电压")]
    /// <summary>
    /// 第87节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage87 { get; set; }
    [Description("第88节电池电压")]
    /// <summary>
    /// 第88节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage88 { get; set; }
    [Description("第89节电池电压")]
    /// <summary>
    /// 第89节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage89 { get; set; }
    [Description("第90节电池电压")]
    /// <summary>
    /// 第90节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage90 { get; set; }
    [Description("第91节电池电压")]
    /// <summary>
    /// 第91节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage91 { get; set; }
    [Description("第92节电池电压")]
    /// <summary>
    /// 第92节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage92 { get; set; }
    [Description("第93节电池电压")]
    /// <summary>
    /// 第93节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage93 { get; set; }
    [Description("第94节电池电压")]
    /// <summary>
    /// 第94节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage94 { get; set; }
    [Description("第95节电池电压")]
    /// <summary>
    /// 第95节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage95 { get; set; }
    [Description("第96节电池电压")]
    /// <summary>
    /// 第96节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage96 { get; set; }
    [Description("第97节电池电压")]
    /// <summary>
    /// 第97节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage97 { get; set; }
    [Description("第98节电池电压")]
    /// <summary>
    /// 第98节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage98 { get; set; }
    [Description("第99节电池电压")]
    /// <summary>
    /// 第99节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage99 { get; set; }
    [Description("第100节电池电压")]
    /// <summary>
    /// 第100节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage100 { get; set; }
    [Description("第101节电池电压")]
    /// <summary>
    /// 第101节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage101 { get; set; }
    [Description("第102节电池电压")]
    /// <summary>
    /// 第102节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage102 { get; set; }
    [Description("第103节电池电压")]
    /// <summary>
    /// 第103节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage103 { get; set; }
    [Description("第104节电池电压")]
    /// <summary>
    /// 第104节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage104 { get; set; }
    [Description("第105节电池电压")]
    /// <summary>
    /// 第105节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage105 { get; set; }
    [Description("第106节电池电压")]
    /// <summary>
    /// 第106节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage106 { get; set; }
    [Description("第107节电池电压")]
    /// <summary>
    /// 第107节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage107 { get; set; }
    [Description("第108节电池电压")]
    /// <summary>
    /// 第108节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage108 { get; set; }
    [Description("第109节电池电压")]
    /// <summary>
    /// 第109节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage109 { get; set; }
    [Description("第110节电池电压")]
    /// <summary>
    /// 第110节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage110 { get; set; }
    [Description("第111节电池电压")]
    /// <summary>
    /// 第111节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage111 { get; set; }
    [Description("第112节电池电压")]
    /// <summary>
    /// 第112节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage112 { get; set; }
    [Description("第113节电池电压")]
    /// <summary>
    /// 第113节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage113 { get; set; }
    [Description("第114节电池电压")]
    /// <summary>
    /// 第114节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage114 { get; set; }
    [Description("第115节电池电压")]
    /// <summary>
    /// 第115节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage115 { get; set; }
    [Description("第116节电池电压")]
    /// <summary>
    /// 第116节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage116 { get; set; }
    [Description("第117节电池电压")]
    /// <summary>
    /// 第117节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage117 { get; set; }
    [Description("第118节电池电压")]
    /// <summary>
    /// 第118节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage118 { get; set; }
    [Description("第119节电池电压")]
    /// <summary>
    /// 第119节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage119 { get; set; }
    [Description("第120节电池电压")]
    /// <summary>
    /// 第120节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage120 { get; set; }
    [Description("第121节电池电压")]
    /// <summary>
    /// 第121节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage121 { get; set; }
    [Description("第122节电池电压")]
    /// <summary>
    /// 第122节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage122 { get; set; }
    [Description("第123节电池电压")]
    /// <summary>
    /// 第123节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage123 { get; set; }
    [Description("第124节电池电压")]
    /// <summary>
    /// 第124节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage124 { get; set; }
    [Description("第125节电池电压")]
    /// <summary>
    /// 第125节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage125 { get; set; }
    [Description("第126节电池电压")]
    /// <summary>
    /// 第126节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage126 { get; set; }
    [Description("第127节电池电压")]
    /// <summary>
    /// 第127节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage127 { get; set; }
    [Description("第128节电池电压")]
    /// <summary>
    /// 第128节电池电压
    /// <summary> 
    public double BMS_BatteryVoltage128 { get; set; }
    #endregion

    #region 模块内7个采样温度
    [Description("模块内第1个采样温度")]
    /// <summary>
    /// 模块内第1个采样温度
    /// <summary> 
    public int BMS_TemperatureSamplingModule1 { get; set; }
    [Description("模块内第2个采样温度")]
    /// <summary>
    /// 模块内第2个采样温度
    /// <summary> 
    public int BMS_TemperatureSamplingModule2 { get; set; }
    [Description("模块内第3个采样温度")]
    /// <summary>
    /// 模块内第3个采样温度
    /// <summary> 
    public int BMS_TemperatureSamplingModule3 { get; set; }
    [Description("模块内第4个采样温度")]
    /// <summary>
    /// 模块内第4个采样温度
    /// <summary> 
    public int BMS_TemperatureSamplingModule4 { get; set; }
    [Description("模块内第5个采样温度")]
    /// <summary>
    /// 模块内第5个采样温度
    /// <summary> 
    public int BMS_TemperatureSamplingModule5 { get; set; }
    [Description("模块内第6个采样温度")]
    /// <summary>
    /// 模块内第6个采样温度
    /// <summary> 
    public int BMS_TemperatureSamplingModule6 { get; set; }
    [Description("模块内第7个采样温度")]
    /// <summary>
    /// 模块内第7个采样温度
    /// <summary> 
    public int BMS_TemperatureSamplingModule7 { get; set; }
    #endregion

    #region 16框正极极柱温度
    [Description("第1框正极极柱温度")]
    /// <summary>
    /// 第1框正极极柱温度
    /// <summary> 
    public int BMS_FramePositivePoleTemperature1 { get; set; }
    [Description("第2框正极极柱温度")]
    /// <summary>
    /// 第2框正极极柱温度
    /// <summary> 
    public int BMS_FramePositivePoleTemperature2 { get; set; }
    [Description("第3框正极极柱温度")]
    /// <summary>
    /// 第3框正极极柱温度
    /// <summary> 
    public int BMS_FramePositivePoleTemperature3 { get; set; }
    [Description("第4框正极极柱温度")]
    /// <summary>
    /// 第4框正极极柱温度
    /// <summary> 
    public int BMS_FramePositivePoleTemperature4 { get; set; }
    [Description("第5框正极极柱温度")]
    /// <summary>
    /// 第5框正极极柱温度
    /// <summary> 
    public int BMS_FramePositivePoleTemperature5 { get; set; }
    [Description("第6框正极极柱温度")]
    /// <summary>
    /// 第6框正极极柱温度
    /// <summary> 
    public int BMS_FramePositivePoleTemperature6 { get; set; }
    [Description("第7框正极极柱温度")]
    /// <summary>
    /// 第7框正极极柱温度
    /// <summary> 
    public int BMS_FramePositivePoleTemperature7 { get; set; }
    [Description("第8框正极极柱温度")]
    /// <summary>
    /// 第8框正极极柱温度
    /// <summary> 
    public int BMS_FramePositivePoleTemperature8 { get; set; }
    [Description("第9框正极极柱温度")]
    /// <summary>
    /// 第9框正极极柱温度
    /// <summary> 
    public int BMS_FramePositivePoleTemperature9 { get; set; }
    [Description("第10框正极极柱温度")]
    /// <summary>
    /// 第10框正极极柱温度
    /// <summary> 
    public int BMS_FramePositivePoleTemperature10 { get; set; }
    [Description("第11框正极极柱温度")]
    /// <summary>
    /// 第11框正极极柱温度
    /// <summary> 
    public int BMS_FramePositivePoleTemperature11 { get; set; }
    [Description("第12框正极极柱温度")]
    /// <summary>
    /// 第12框正极极柱温度
    /// <summary> 
    public int BMS_FramePositivePoleTemperature12 { get; set; }
    [Description("第13框正极极柱温度")]
    /// <summary>
    /// 第13框正极极柱温度
    /// <summary> 
    public int BMS_FramePositivePoleTemperature13 { get; set; }
    [Description("第14框正极极柱温度")]
    /// <summary>
    /// 第14框正极极柱温度
    /// <summary> 
    public int BMS_FramePositivePoleTemperature14 { get; set; }
    [Description("第15框正极极柱温度")]
    /// <summary>
    /// 第15框正极极柱温度
    /// <summary> 
    public int BMS_FramePositivePoleTemperature15 { get; set; }
    [Description("第16框正极极柱温度")]
    /// <summary>
    /// 第16框正极极柱温度
    /// <summary> 
    public int BMS_FramePositivePoleTemperature16 { get; set; }
    #endregion
    #region 16框负极极柱温度
    [Description("第1框负极极柱温度")]
    /// <summary>
    /// 第1框负极极柱温度
    /// <summary> 
    public int BMS_FrameNegativePoleTemperature1 { get; set; }
    [Description("第2框负极极柱温度")]
    /// <summary>
    /// 第2框负极极柱温度
    /// <summary> 
    public int BMS_FrameNegativePoleTemperature2 { get; set; }
    [Description("第3框负极极柱温度")]
    /// <summary>
    /// 第3框负极极柱温度
    /// <summary> 
    public int BMS_FrameNegativePoleTemperature3 { get; set; }
    [Description("第4框负极极柱温度")]
    /// <summary>
    /// 第4框负极极柱温度
    /// <summary> 
    public int BMS_FrameNegativePoleTemperature4 { get; set; }
    [Description("第5框负极极柱温度")]
    /// <summary>
    /// 第5框负极极柱温度
    /// <summary> 
    public int BMS_FrameNegativePoleTemperature5 { get; set; }
    [Description("第6框负极极柱温度")]
    /// <summary>
    /// 第6框负极极柱温度
    /// <summary> 
    public int BMS_FrameNegativePoleTemperature6 { get; set; }
    [Description("第7框负极极柱温度")]
    /// <summary>
    /// 第7框负极极柱温度
    /// <summary> 
    public int BMS_FrameNegativePoleTemperature7 { get; set; }
    [Description("第8框负极极柱温度")]
    /// <summary>
    /// 第8框负极极柱温度
    /// <summary> 
    public int BMS_FrameNegativePoleTemperature8 { get; set; }
    [Description("第9框负极极柱温度")]
    /// <summary>
    /// 第9框负极极柱温度
    /// <summary> 
    public int BMS_FrameNegativePoleTemperature9 { get; set; }
    [Description("第10框负极极柱温度")]
    /// <summary>
    /// 第10框负极极柱温度
    /// <summary> 
    public int BMS_FrameNegativePoleTemperature10 { get; set; }
    [Description("第11框负极极柱温度")]
    /// <summary>
    /// 第11框负极极柱温度
    /// <summary> 
    public int BMS_FrameNegativePoleTemperature11 { get; set; }
    [Description("第12框负极极柱温度")]
    /// <summary>
    /// 第12框负极极柱温度
    /// <summary> 
    public int BMS_FrameNegativePoleTemperature12 { get; set; }
    [Description("第13框负极极柱温度")]
    /// <summary>
    /// 第13框负极极柱温度
    /// <summary> 
    public int BMS_FrameNegativePoleTemperature13 { get; set; }
    [Description("第14框负极极柱温度")]
    /// <summary>
    /// 第14框负极极柱温度
    /// <summary> 
    public int BMS_FrameNegativePoleTemperature14 { get; set; }
    [Description("第15框负极极柱温度")]
    /// <summary>
    /// 第15框负极极柱温度
    /// <summary> 
    public int BMS_FrameNegativePoleTemperature15 { get; set; }
    [Description("第16框负极极柱温度")]
    /// <summary>
    /// 第16框负极极柱温度
    /// <summary> 
    public int BMS_FrameNegativePoleTemperature16 { get; set; }
    #endregion
    #region 16#模块正极柱过温报警
    [Description("1#模块正极柱过温报警")]
    /// <summary>
    /// 1#模块正极柱过温报警
    /// <summary> 
    public int BMS_ModulePositivePoleOverTempAlarm1 { get; set; }
    [Description("2#模块正极柱过温报警")]
    /// <summary>
    /// 2#模块正极柱过温报警
    /// <summary> 
    public int BMS_ModulePositivePoleOverTempAlarm2 { get; set; }
    [Description("3#模块正极柱过温报警")]
    /// <summary>
    /// 3#模块正极柱过温报警
    /// <summary> 
    public int BMS_ModulePositivePoleOverTempAlarm3 { get; set; }
    [Description("4#模块正极柱过温报警")]
    /// <summary>
    /// 4#模块正极柱过温报警
    /// <summary> 
    public int BMS_ModulePositivePoleOverTempAlarm4 { get; set; }
    [Description("5#模块正极柱过温报警")]
    /// <summary>
    /// 5#模块正极柱过温报警
    /// <summary> 
    public int BMS_ModulePositivePoleOverTempAlarm5 { get; set; }
    [Description("6#模块正极柱过温报警")]
    /// <summary>
    /// 6#模块正极柱过温报警
    /// <summary> 
    public int BMS_ModulePositivePoleOverTempAlarm6 { get; set; }
    [Description("7#模块正极柱过温报警")]
    /// <summary>
    /// 7#模块正极柱过温报警
    /// <summary> 
    public int BMS_ModulePositivePoleOverTempAlarm7 { get; set; }
    [Description("8#模块正极柱过温报警")]
    /// <summary>
    /// 8#模块正极柱过温报警
    /// <summary> 
    public int BMS_ModulePositivePoleOverTempAlarm8 { get; set; }
    [Description("9#模块正极柱过温报警")]
    /// <summary>
    /// 9#模块正极柱过温报警
    /// <summary> 
    public int BMS_ModulePositivePoleOverTempAlarm9 { get; set; }
    [Description("10#模块正极柱过温报警")]
    /// <summary>
    /// 10#模块正极柱过温报警
    /// <summary> 
    public int BMS_ModulePositivePoleOverTempAlarm10 { get; set; }
    [Description("11#模块正极柱过温报警")]
    /// <summary>
    /// 11#模块正极柱过温报警
    /// <summary> 
    public int BMS_ModulePositivePoleOverTempAlarm11 { get; set; }
    [Description("12#模块正极柱过温报警")]
    /// <summary>
    /// 12#模块正极柱过温报警
    /// <summary> 
    public int BMS_ModulePositivePoleOverTempAlarm12 { get; set; }
    [Description("13#模块正极柱过温报警")]
    /// <summary>
    /// 13#模块正极柱过温报警
    /// <summary> 
    public int BMS_ModulePositivePoleOverTempAlarm13 { get; set; }
    [Description("14#模块正极柱过温报警")]
    /// <summary>
    /// 14#模块正极柱过温报警
    /// <summary> 
    public int BMS_ModulePositivePoleOverTempAlarm14 { get; set; }
    [Description("15#模块正极柱过温报警")]
    /// <summary>
    /// 15#模块正极柱过温报警
    /// <summary> 
    public int BMS_ModulePositivePoleOverTempAlarm15 { get; set; }
    [Description("16#模块正极柱过温报警")]
    /// <summary>
    /// 16#模块正极柱过温报警
    /// <summary> 
    public int BMS_ModulePositivePoleOverTempAlarm16 { get; set; }
    #endregion
    #region 16#模块负极柱过温报警
    [Description("1#模块负极柱过温报警")]
    /// <summary>
    /// 1#模块负极柱过温报警
    /// <summary> 
    public int BMS_ModuleNegativePoleOverTempAlarm1 { get; set; }
    [Description("2#模块负极柱过温报警")]
    /// <summary>
    /// 2#模块负极柱过温报警
    /// <summary> 
    public int BMS_ModuleNegativePoleOverTempAlarm2 { get; set; }
    [Description("3#模块负极柱过温报警")]
    /// <summary>
    /// 3#模块负极柱过温报警
    /// <summary> 
    public int BMS_ModuleNegativePoleOverTempAlarm3 { get; set; }
    [Description("4#模块负极柱过温报警")]
    /// <summary>
    /// 4#模块负极柱过温报警
    /// <summary> 
    public int BMS_ModuleNegativePoleOverTempAlarm4 { get; set; }
    [Description("5#模块负极柱过温报警")]
    /// <summary>
    /// 5#模块负极柱过温报警
    /// <summary> 
    public int BMS_ModuleNegativePoleOverTempAlarm5 { get; set; }
    [Description("6#模块负极柱过温报警")]
    /// <summary>
    /// 6#模块负极柱过温报警
    /// <summary> 
    public int BMS_ModuleNegativePoleOverTempAlarm6 { get; set; }
    [Description("7#模块负极柱过温报警")]
    /// <summary>
    /// 7#模块负极柱过温报警
    /// <summary> 
    public int BMS_ModuleNegativePoleOverTempAlarm7 { get; set; }
    [Description("8#模块负极柱过温报警")]
    /// <summary>
    /// 8#模块负极柱过温报警
    /// <summary> 
    public int BMS_ModuleNegativePoleOverTempAlarm8 { get; set; }
    [Description("9#模块负极柱过温报警")]
    /// <summary>
    /// 9#模块负极柱过温报警
    /// <summary> 
    public int BMS_ModuleNegativePoleOverTempAlarm9 { get; set; }
    [Description("10#模块负极柱过温报警")]
    /// <summary>
    /// 10#模块负极柱过温报警
    /// <summary> 
    public int BMS_ModuleNegativePoleOverTempAlarm10 { get; set; }
    [Description("11#模块负极柱过温报警")]
    /// <summary>
    /// 11#模块负极柱过温报警
    /// <summary> 
    public int BMS_ModuleNegativePoleOverTempAlarm11 { get; set; }
    [Description("12#模块负极柱过温报警")]
    /// <summary>
    /// 12#模块负极柱过温报警
    /// <summary> 
    public int BMS_ModuleNegativePoleOverTempAlarm12 { get; set; }
    [Description("13#模块负极柱过温报警")]
    /// <summary>
    /// 13#模块负极柱过温报警
    /// <summary> 
    public int BMS_ModuleNegativePoleOverTempAlarm13 { get; set; }
    [Description("14#模块负极柱过温报警")]
    /// <summary>
    /// 14#模块负极柱过温报警
    /// <summary> 
    public int BMS_ModuleNegativePoleOverTempAlarm14 { get; set; }
    [Description("15#模块负极柱过温报警")]
    /// <summary>
    /// 15#模块负极柱过温报警
    /// <summary> 
    public int BMS_ModuleNegativePoleOverTempAlarm15 { get; set; }
    [Description("16#模块负极柱过温报警")]
    /// <summary>
    /// 16#模块负极柱过温报警
    /// <summary> 
    public int BMS_ModuleNegativePoleOverTempAlarm16 { get; set; }
    #endregion

    [Description("电池箱数量")]
    /// <summary>
    /// 电池箱数量
    /// </summary>
    public int BMS_BatteryBoxQuantity { get; set; }
    [Description("电池箱序号（保留）")]
    /// <summary>
    /// 电池箱序号（保留）
    /// </summary>
    public int BMS_BatteryBoxNumber { get; set; }
    [Description("电池箱平均电压")]
    /// <summary>
    /// 电池箱平均电压
    /// </summary>
    public double BMS_AverageVoltageOfTheBatteryBox { get; set; }
    [Description("电池箱均温度")]
    /// <summary>
    /// 电池箱均温度
    /// </summary>
    public double BMS_AverageTemperatureOfTheBatteryBox { get; set; }
    [Description("电池箱端电压")]
    /// <summary>
    /// 电池箱端电压
    /// </summary>
    public double BMS_TheTerminalVoltageOfBatteryBox { get; set; }
    [Description("电池箱SOC")]
    /// <summary>
    /// 电池箱SOC
    /// </summary>
    public double BMS_BatteryBoxSOC { get; set; }
    [Description("最大电压单体序号")]
    /// <summary>
    /// 最大电压单体序号
    /// </summary>
    public int BMS_TheMaximumVoltageMonomerSerialNumber { get; set; }
    [Description("最大单体电压值")]
    /// <summary>
    /// 最大单体电压值
    /// </summary>
    public double BMS_TheLargestSingleVoltageValue { get; set; }
    [Description("最小电压单体序号")]
    /// <summary>
    /// 最小电压单体序号
    /// </summary>
    public int BMS_TheMinimumVoltageMonomerSerialNumber { get; set; }
    [Description("最小单体电压值")]
    /// <summary>
    /// 最小单体电压值
    /// </summary>
    public double BMS_TheMinimumSingleVoltageValue { get; set; }
    [Description("最高温度采样点")]
    /// <summary>
    /// 最高温度采样点
    /// </summary>
    public int BMS_TheHighestTemperatureSamplingPoint { get; set; }
    [Description("最高温度采样值")]
    /// <summary>
    /// 最高温度采样值
    /// </summary>
    public double BMS_TheHighestTemperatureSamplingValue { get; set; }
    [Description("最低温度采样点")]
    /// <summary>
    /// 最低温度采样点
    /// </summary>
    public int BMS_TheLowestTemperatureSamplingPoint { get; set; }
    [Description("最低温度采样值")]
    /// <summary>
    /// 最低温度采样值
    /// </summary>
    public double BMS_TheLowestTemperatureSamplingValue { get; set; }
    [Description("正向累计电量")]
    /// <summary>
    /// 正向累计电量
    /// </summary>
    public double BMS_PositiveCumulativePower { get; set; }
    [Description("反向累计电量")]
    /// <summary>
    /// 反向累计电量
    /// </summary>
    public double BMS_NegativeCumulativePower { get; set; }
    [Description("电池组编码字节序列-暂不解析")]
    /// <summary>
    /// 电池组编码字节序列
    /// </summary>
    public int BMS_BatteryGroupEncodingByteSequence { get; set; }
    [Description("电池组编码（BCD码）-暂不解析")]
    /// <summary>
    /// 电池组编码（BCD码）
    /// </summary>
    public int BMS_BatteryGroupEncoding { get; set; }
    [Description("充电机通信")]
    /// <summary>
    /// 充电机通信
    /// </summary>
    public int CCS_ChargerCommunication { get; set; }
    [Description("充电机硬件故障")]
    /// <summary>
    /// 充电机硬件故障
    /// </summary>
    public int CCS_ChargerHardwareFailure { get; set; }
    [Description("充电电压")]
    /// <summary>
    /// 充电电压
    /// </summary>
    public double CCS_ChargingVoltage { get; set; }
    [Description("充电电流")]
    /// <summary>
    /// 充电电流
    /// </summary>
    public double CCS_ChargingCurrent { get; set; }
    [Description("CCS_充电状态")]
    /// <summary>
    /// CCS_充电状态
    /// </summary>
    public int CCS_ChargeSt { get; set; }
    [Description("绝缘监测仪状态字节")]
    /// <summary>
    /// 绝缘监测仪状态字节
    /// </summary>
    public int InsulationMonitor_StatusByte { get; set; }
    [Description("绝缘电阻告警状态")]
    /// <summary>
    /// 绝缘电阻告警状态
    /// </summary>
    public int InsulationMonitor_TheInsulationResistanceOfTheAlarmState { get; set; }
    [Description("绝缘电阻高字节")]
    /// <summary>
    /// 绝缘电阻高字节
    /// </summary>
    public int InsulationMonitor_TheInsulationResistanceOfTheHighByte { get; set; }
    [Description("绝缘电阻低字节")]
    /// <summary>
    /// 绝缘电阻低字节
    /// </summary>
    public int InsulationMonitor_TheInsulationResistanceOfTheLowByte { get; set; }
    [Description("电池电压高字节")]
    /// <summary>
    /// 电池电压高字节
    /// </summary>
    public int InsulationMonitor_BatteryVoltageHighByte { get; set; }
    [Description("电池电压低字节")]
    /// <summary>
    /// 电池电压低字节
    /// </summary>
    public int InsulationMonitor_BatteryVoltageLowByte { get; set; }
    [Description("Life信号")]
    /// <summary>
    /// Life信号
    /// </summary>
    public int InsulationMonitor_LifeSignal { get; set; }
    [Description("控制器准备就绪")]
    /// <summary>
    /// 控制器准备就绪
    /// </summary>
    public int APU_BrakeControlSystem_ControllerReady { get; set; }
    [Description("运行")]
    /// <summary>
    /// 运行
    /// </summary>
    public int APU_BrakeControlSystem_Run { get; set; }
    [Description("故障")]
    /// <summary>
    /// 故障
    /// </summary>
    public int APU_BrakeControlSystem_Fault { get; set; }
    [Description("计数器")]
    /// <summary>
    /// 计数器
    /// </summary>
    public int APU_BrakeControlSystem_Counter { get; set; }
    [Description("控制器准备就绪")]
    /// <summary>
    /// 控制器准备就绪
    /// </summary>
    public int APU_SteeringControlSystem_ControllerReady { get; set; }
    [Description("运行")]
    /// <summary>
    /// 运行
    /// </summary>
    public int APU_SteeringControlSystem_Run { get; set; }
    [Description("故障")]
    /// <summary>
    /// 故障
    /// </summary>
    public int APU_SteeringControlSystem_Fault { get; set; }
    [Description("计数器")]
    /// <summary>
    /// 计数器
    /// </summary>
    public int APU_SteeringControlSystem_Counter { get; set; }




}

public class Candata_ZzShuaiKe : Candata
{
    /// <summary>
    /// CCP_DTO_DATA
    /// </summary>
    public int TCU_CCP_DTO_DATA { get; set; }
    /// <summary>
    /// 锁止状态
    /// </summary>
    public int TCU_LockingStatus { get; set; }
    /// <summary>
    /// 驻车
    /// </summary>
    public int TCU_Parking { get; set; }
    /// <summary>
    /// 驱动状态
    /// </summary>
    public int TCU_DriveState { get; set; }
    /// <summary>
    /// 驻车锁止电机不插电（开路）
    /// </summary>
    public int TCU_MotorParkingUnplugged { get; set; }
    /// <summary>
    /// 驻车电机失速
    /// </summary>
    public int TCU_ParkingMotorStall { get; set; }
    /// <summary>
    /// 未驻车电机失速
    /// </summary>
    public int TCU_NotParkingMotorStall { get; set; }
    /// <summary>
    /// 驻车和非驻车中间位置电机失速
    /// </summary>
    public int TCU_ParkingOrNotMotorStall { get; set; }
    /// <summary>
    /// 换档电机角度传感器故障
    /// </summary>
    public int TCU_ShiftMotorAngleSensorFault { get; set; }
    /// <summary>
    /// DMC丢失
    /// </summary>
    public int TCU_DMCLost { get; set; }
    /// <summary>
    /// CAN故障
    /// </summary>
    public int TCU_CANFault { get; set; }
    /// <summary>
    /// 变速箱过热
    /// </summary>
    public int TCU_GearboxOverheating { get; set; }
    /// <summary>
    /// 与0X580故障对应
    /// </summary>
    public int TCU_WithTheCorresponding0X580Fault { get; set; }
    /// <summary>
    /// TCM_主板本（APP)
    /// </summary>
    public int TCU_TCM_MainVersionAPP { get; set; }
    /// <summary>
    /// TCM_标准版本（APP）
    /// </summary>
    public int TCU_TCM_StandardVersionAPP { get; set; }
    /// <summary>
    /// TCM_精简版本（APP）
    /// </summary>
    public int TCU_TCM_SimplifiedVersionAPP { get; set; }
    /// <summary>
    /// TCM_主版本（Boot）
    /// </summary>
    public int TCU_TCM_MainVersionBoot { get; set; }
    /// <summary>
    /// TCM_标准版本（Boot）
    /// </summary>
    public int TCU_TCM_StandardVersionBoot { get; set; }
    /// <summary>
    /// TCM_精简版本（Boot）
    /// </summary>
    public int TCU_TCM_SimplifiedVersionBoot { get; set; }
    /// <summary>
    /// TCM_标识高子节
    /// </summary>
    public int TCU_TCM_IdentifyTheHighByte { get; set; }
    /// <summary>
    /// TCM_标识低字节
    /// </summary>
    public int TCU_TCM_IdentifyTheLowByte { get; set; }
    /// <summary>
    /// 应答
    /// </summary>
    public int TCU_Response { get; set; }
    /// <summary>
    /// CCP_CRO_DATA
    /// </summary>
    public int TCU_CCP_CRO_DATA { get; set; }
    /// <summary>
    /// 停车请求
    /// </summary>
    public int TCU_ParkingRequest { get; set; }
    /// <summary>
    /// 电机转速	
    /// </summary>
    public int Motor_Revolution { get; set; }
    /// <summary>
    /// 清除故障
    /// </summary>
    public int TCU_ClearFault { get; set; }
    /// <summary>
    /// 广播
    /// </summary>
    public int TCU_Broadcast { get; set; }
    /// <summary>
    /// 最大限制转矩
    /// </summary>
    public int Motor_MaximumLimitTorque { get; set; }
    /// <summary>
    /// 驱动状态
    /// </summary>
    public int Motor_DriveState { get; set; }
    /// <summary>
    /// 母线电压
    /// </summary>
    public int Motor_BusbarVoltage { get; set; }
    /// <summary>
    /// 电池电流
    /// </summary>
    public int BMS_Current { get; set; }
    /// <summary>
    /// 实际Id
    /// </summary>
    public int Motor_RealId { get; set; }
    /// <summary>
    /// 目标Id
    /// </summary>
    public int Motor_TargetId { get; set; }
    /// <summary>
    /// 转矩
    /// </summary>
    public int Motor_Torque { get; set; }
    /// <summary>
    /// 控制器温度
    /// </summary>
    public int Motor_ControllerTemp { get; set; }
    /// <summary>
    /// 电压控制（电机SVM因数）
    /// </summary>
    public int Motor_VoltageControl { get; set; }
    /// <summary>
    /// 电机实际电流
    /// </summary>
    public int Motor_Current { get; set; }
    /// <summary>
    /// 实际电机uq（ua）
    /// </summary>
    public int Motor_ActualMotorUQ { get; set; }
    /// <summary>
    /// 实际电机ud（uf）
    /// </summary>
    public int Motor_ActualMotorUD { get; set; }
    /// <summary>
    /// 实际iq
    /// </summary>
    public int Motor_ActualIQ { get; set; }
    /// <summary>
    /// 目标iq
    /// </summary>
    public int Motor_ActualTargetIQ { get; set; }
    /// <summary>
    /// 目标转矩
    /// </summary>
    public int Motor_TargetTorque { get; set; }
    /// <summary>
    /// 冷却液温度
    /// </summary>
    public int Motor_CoolantTemperature { get; set; }
    /// <summary>
    /// 油门值
    /// </summary>
    public int Motor_ThrottleValue { get; set; }
    /// <summary>
    /// 电池最大放电电流
    /// </summary>
    public int BMS_Battery_Discharge_A_MAX { get; set; }


}