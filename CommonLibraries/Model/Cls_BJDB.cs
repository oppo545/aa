using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

public class Cls_BJDB
{
    /// <summary>
    /// 采集时间
    /// </summary>
    public string Date { get; set; }

}
/// <summary>
/// 格式
/// </summary>
public class Cls_FormatBJDB
{
    /// <summary>
    /// 起始符
    /// </summary>
    public string Qsf { get; set; }

    /// <summary>
    /// 命令标识
    /// </summary>
    public string Ml_bs { get; set; }

    /// <summary>
    /// 应答标志
    /// </summary>
    public string Ml_yd { get; set; }

    /// <summary>
    /// 识别码
    /// </summary>
    public string Identifier { get; set; }

    /// <summary>
    /// 数据加密方式
    /// </summary>
    public string Jm { get; set; }

    /// <summary>
    /// 数据单元长度
    /// </summary>
    public string Unitlength { get; set; }

    /// <summary>
    /// 校验码
    /// </summary>
    public string Jym { get; set; }

}

/// <summary>
/// 注册
/// </summary>
public class Cls_Register
{
    /// <summary>
    /// 注册时间
    /// </summary>
    public string Zc_zcsj { get; set; }
    /// <summary>
    /// 流水号
    /// </summary>
    public string Zc_zclsh { get; set; }
    /// <summary>
    /// 车牌号
    /// </summary>
    public string Zc_cph { get; set; }
    /// <summary>
    /// 生产厂商代码
    /// </summary>
    public string Zc_zd_csdm { get; set; }
    /// <summary>
    /// 终端批号
    /// </summary>
    public string Zc_zd_ph { get; set; }
    /// <summary>
    /// 流水号
    /// </summary>
    public string Zc_zd_lsh { get; set; }
    public List<Cls_bjdb_dlxdc> Zc_dlxdc { get; set; }

    /// <summary>
    /// 车型
    /// </summary>
    public int Styling { get; set; }
}

/// <summary>
/// 动力蓄电池
/// </summary>
public class Cls_bjdb_dlxdc
{
    public string Id { get; set; }

    public string Number { get; set; }

    /// <summary>
    /// 生产厂商代码
    /// </summary>
    public string Sccsdm { get; set; }

    /// <summary>
    /// 电池类型代码
    /// </summary>
    public string Dclxdm { get; set; }

    /// <summary>
    /// 额定能量
    /// </summary>
    public string Ednl { get; set; }

    /// <summary>
    /// 额定电压
    /// </summary>
    public string Eddy { get; set; }


    /// <summary>
    /// 电池生产日期代码
    /// </summary>
    public string Scrq { get; set; }

    /// <summary>
    /// 流水号
    /// </summary>
    public string Lsh { get; set; }
}



#region 实时上报

/// <summary>
/// 实时上报接口
/// </summary>
public interface Cls_RealInformation
{

}

/// <summary>
/// 单体蓄电池电压数据
/// </summary>
public class Cls_SingleBatteryVoltage : Cls_RealInformation
{
    /// <summary>
    /// 单体蓄电池总数
    /// </summary>
    public double SingleBattery { get; set; }
    /// <summary>
    /// 单体蓄电池包总数
    /// </summary>
    public double SingleBatteryPackTotal { get; set; }

    public List<Cls_VoltageValueList> lis_vvl { get; set; }
}

/// <summary>
/// 电压值列
/// </summary>
public class Cls_VoltageValueList
{
    /// <summary>
    /// 动力蓄电池包序号
    /// </summary>
    public int PowerBatteryPackNumber { get; set; }
    /// <summary>
    /// 该包单体蓄电池总数
    /// </summary>
    public int PowerBatteryTotal { get; set; }
    /// <summary>
    /// 电压值
    /// </summary>
    public string BatteryVoltage { get; set; }
}

/// <summary>
/// 动力蓄电池包温度
/// </summary>
public class Cls_PowerBatteryPack : Cls_RealInformation
{
    /// <summary>
    /// 动力蓄电池包温度探针总数
    /// </summary>
    public double BatteryTemperatureTotal { get; set; }
    /// <summary>
    /// 动力蓄电池包总数
    /// </summary>
    public double PowerBatteryPackTotal { get; set; }

    public List<Cls_TemperatureValueList> lis_tvl { get; set; }
}     

/// <summary>
/// 温度值列表
/// </summary>
public class Cls_TemperatureValueList
{
    /// <summary>
    /// 动力蓄电池包序号
    /// </summary>
    public int PowerBatteryPackNumber { get; set; }
    /// <summary>
    /// 该包动力蓄电池温度探针总数
    /// </summary>
    public int TemperatureProbeTotal { get; set; }
    /// <summary>
    /// 探针温度值
    /// </summary>
    public string ProbeTemperature { get; set; }
}


/// <summary>
/// 整车数据
/// </summary>
public class Cls_VehicleInfo : Cls_RealInformation
{
    [Description("车速")]
    /// <summary>
    /// 车速
    /// </summary>
    public double ABS_VehSpd { get; set; }
    [Description("总里程")]
    /// <summary>
    /// 总里程
    /// </summary>
    public double IC_TotalOdmeter { get; set; }
    [Description("档位状态")]
    /// <summary>
    /// 档位状态
    /// </summary>
    public int TCU_GearPosition { get; set; }
    [Description("加速行驶行程百分比")]

    /// <summary>
    /// 加速行驶行程百分比-
    /// </summary>
    public double AcceleratingTravelPer { get; set; }
    [Description("制动踏板行驶行程百分比")]
    /// <summary>
    /// 制动踏板行驶行程百分比-
    /// </summary>
    public double BrakeTravelPer { get; set; }

    [Description("充放电状态")]
    /// <summary>
    /// 充放电状态-     
    /// BMK 2035-12-7 BMS_ElectricityState改为 BMS_ChargeSt
    /// BMS_ElectricityState: 0x01：充电；0x02：放电；0xFF：无效数据
    /// BMS_ChargeSt: 0:非充电状态; 1:充电;2充电故障;3:充电完成;4:无效数据
    /// </summary>
    public string BMS_ChargeSt { get; set; }
    [Description("电机控制器温度")]
    /// <summary>
    /// 电机控制器温度
    /// </summary>
    public double Motor_ControllerTemp { get; set; }
    [Description("电机转速")]
    /// <summary>
    /// 电机转速
    /// </summary>
    public double Motor_Revolution { get; set; }
    [Description("电机温度")]
    /// <summary>
    /// 电机温度
    /// </summary>
    public double Motor_Temperature { get; set; }
    [Description("驱动电机电压")]
    /// <summary>
    /// 驱动电机电压-  double 为了取小数
    /// </summary>
    public double Motor_Voltage { get; set; }
    [Description("驱动电机电流")]
    /// <summary>
    /// 驱动电机电流-
    /// </summary>
    public double Motor_Current { get; set; }
    [Description("空调设定温度")]
    /// <summary>
    /// 空调设定温度-
    /// </summary>
    public double AirSettingTemperature { get; set; }

}

/// <summary>
/// 定位数据
/// </summary>
public class Cls_PositioningData : Cls_RealInformation
{
    [Description("定位状态")]
    /// <summary>
    /// 定位状态
    /// </summary>
    public string IsLocation { get; set; }
    [Description("南纬北纬")]
    /// <summary>
    /// 南纬北纬
    /// </summary>
    public string SouthLatitude { get; set; }
    [Description("东经西经")]
    /// <summary>
    /// 东经西经
    /// </summary>
    public string EastWest { get; set; }
    [Description("经度")]
    /// <summary>
    /// 经度
    /// </summary>
    public double Longitude { get; set; }
    [Description("纬度")]

    /// <summary>
    /// 纬度
    /// </summary>
    public double Latitude { get; set; }
    [Description("速度")]
    /// <summary>
    /// 速度
    /// </summary>
    public double Speed { get; set; }
    [Description("方向")]
    /// <summary>
    /// 方向
    /// </summary>
    public double Direction { get; set; }


}

/// <summary>
/// 极值数据
/// </summary>
public class Cls_ExtremeData : Cls_RealInformation
{
    [Description("最高电压动力蓄电池单体所在电池包序号")]
    /// <summary>
    /// 最高电压动力蓄电池单体所在电池包序号
    /// </summary>
    public int MaximumVoltageBatteryPackNumber { get; set; }
    [Description("最高电压单体蓄电池序号")]
    /// <summary>
    /// 最高电压单体蓄电池序号
    /// </summary>
    public int MaximumVoltageSingleBatteryNumber { get; set; }
    [Description("电池单体电压最高值")]
    /// <summary>
    /// 电池单体电压最高值
    /// </summary>
    public double BMS_MaxCellBatt { get; set; }

    [Description("最低电压动力蓄电池包序号")]
    /// <summary>
    /// 最低电压动力蓄电池包序号   
    /// </summary>
    public int LowestVoltageBatteryPackNumber { get; set; }
    [Description("最低电压单体蓄电池序号")]
    /// <summary>
    /// 最低电压单体蓄电池序号
    /// </summary>
    public int LowestVoltageSingleBatteryPackNumber { get; set; }
    [Description("电池单体电压最低值")]
    /// <summary>
    /// 电池单体电压最低值
    /// </summary>
    public double BMS_MinCellBatt { get; set; }
    [Description("最高温度动力蓄电池包序号")]
    /// <summary>
    /// 最高温度动力蓄电池包序号
    /// </summary>
    public int MaximumTemperatureBatteryPackNumber { get; set; }
    [Description("最高温度探针序号")]
    /// <summary>
    /// 最高温度探针序号
    /// </summary>
    public int MaximumTemperatureProbeNumber { get; set; }
    [Description("最高温度值")]
    /// <summary>
    /// 最高温度值
    /// </summary>
    public double BMS_Temp_Max { get; set; }
    [Description("最低温度动力蓄电池包序号")]
    /// <summary>
    /// 最低温度动力蓄电池包序号
    /// </summary>
    public int LowestTemperatureBatteryPackNumber { get; set; }
    [Description("最低温度探针序号")]
    /// <summary>
    /// 最低温度探针序号
    /// </summary>
    public int LowestTemperatureProbeNumber { get; set; }
    [Description("最低温度值")]
    /// <summary>
    /// 最低温度值
    /// </summary>
    public double BMS_Temp_Min { get; set; }
    [Description("总电压")]
    /// <summary>
    /// 总电压
    /// </summary>
    public double BMS_Voltage { get; set; }
    [Description("总电流")]
    /// <summary>
    /// 总电流
    /// </summary>
    public double BMS_Current { get; set; }
    [Description("SOC")]
    /// <summary>
    /// SOC
    /// </summary>
    public double BMS_SOC { get; set; }
    [Description("剩余电量")]
    /// <summary>
    /// 剩余电量(剩余能量)
    /// </summary>
    public double ResidualCapacity { get; set; }
    [Description("绝缘电阻")]
    /// <summary>
    /// 绝缘电阻
    /// </summary>
    public double InsulationResistance { get; set; }

}

/// <summary>
/// 报警数据
/// </summary>
public class Cls_AlarmData : Cls_RealInformation
{
    #region 动力蓄电池报警标志

    /// <summary>
    /// 动力蓄电池报警标志
    /// </summary>
    public string PowerBatteryWarningSign { get; set; }
    /// <summary>
    /// 温度差异报警
    /// </summary>
    public int TemperatureDifferenceAlarm { get; set; }
    /// <summary>
    /// 电池极柱高温报警
    /// </summary>
    public int BatteryPoleHighTemperature { get; set; }
    /// <summary>
    /// 动力蓄电池包过压报警
    /// </summary>
    public int BMS_VoltageTooHigh { get; set; }
    /// <summary>
    /// 动力蓄电池包欠压报警
    /// </summary>
    public int BMS_VoltageTooLow { get; set; }
    /// <summary>
    /// SOC 低报警
    /// </summary>
    public int SOCLowAlarm { get; set; }
    /// <summary>
    /// 单体蓄电池过压报警
    /// </summary>
    public int BatteryOverVoltage { get; set; }
    /// <summary>
    /// 单体蓄电池欠压报警
    /// </summary>
    public int MonomerBatteryUnderVoltage { get; set; }
    /// <summary>
    /// SOC 太低报警
    /// </summary>
    public int SOCTooLowAlarm { get; set; }
    /// <summary>
    /// SOC 过高报警
    /// </summary>
    public int SOCOvertopAlarm { get; set; }
    /// <summary>
    /// 动力蓄电池包不匹配报警
    /// </summary>
    public int BatteryPackNotMatch { get; set; }
    /// <summary>
    /// 动力蓄电池一致性差报
    /// </summary>
    public int BatteriesConsistencyPoor { get; set; }
    /// <summary>
    /// 绝缘故障
    /// </summary>
    public int InsulationFault { get; set; }
    #endregion

    /// <summary>
    /// 动力蓄电池其他故障总数
    /// </summary>
    public int OtherPowerBatteriesTotal { get; set; }
    ///// <summary>
    ///// 动力蓄电池其他故障代码列表    <<列表 是个 集合
    ///// </summary>
    //public int OtherPowerBatteriesList { get; set; }
    /// <summary>
    /// 电机故障总数
    /// </summary>
    public int MotorFaultTotal { get; set; }
    ///// <summary>
    ///// 电机故障代码列表
    ///// </summary>
    //public int MotorFaultList { get; set; }

    /// <summary>
    /// 其他故障总数
    /// </summary>
    public int OtherFaultTotal { get; set; }
    ///// <summary>
    ///// 电机故障代码列表
    ///// </summary>
    //public int OtherFaultList { get; set; }


    public int OverTemperature { get; set; }
    public int ChargeTemperatureLow { get; set; }
    public int DisChargeTemperatureLow { get; set; }
    public int HighRangeOfTemperature { get; set; }
    public int MonomerVoltageHigh { get; set; }
    public int MonomerVoltageLow { get; set; }
    public int PressureDifferenceHigh { get; set; }
    public int TotalBatteryVoltageHigh { get; set; }
    public int TotalBatteryVoltageLow { get; set; }
    public int ChargeCurrentHigh { get; set; }
    public int DisChargeCurrentHigh { get; set; }
}



/// <summary>
/// 海马扩展数据
/// </summary>
public class Cls_HMExtension : Candata, Cls_RealInformation
{


    /// <summary>
    /// 电机正常回馈模式
    /// </summary>
    public int VCU_Normal_Feedback
    { get; set; }

    /// <summary>
    /// 电机启停命令
    /// </summary>
    public int VCU_Motor_Run_Stop
    { get; set; }

    /// <summary>
    /// 超速报警信号
    /// </summary>
    public int VCU_OverSpeed_Alarm
    { get; set; }

    /// <summary>
    /// 电机正反转指令
    /// </summary>
    public int VCU_Motor_Forward_Reverse
    { get; set; }

    /// <summary>
    /// 电机控制模式
    /// </summary>
    public int VCU_Control_Mode
    { get; set; }

    /// <summary>
    /// 电机目标扭矩
    /// </summary>
    public double VCU_Motor_Target_tq
    { get; set; }

    /// <summary>
    /// 电机目标转速
    /// </summary>
    public double VCU_Motor_Target_rpm
    { get; set; }

    /// <summary>
    /// 充电线状态
    /// </summary>
    public int VCU_Chargingstate
    { get; set; }

    /// <summary>
    /// DCDC工作指令
    /// </summary>
    public int VCU_DCDC_Workorder
    { get; set; }

    /// <summary>
    /// DCDC故障
    /// </summary>
    public int DCDC_Fault
    { get; set; }

    /// <summary>
    /// 剩余行驶里程
    /// </summary>
    public double VCU_CruisingRange { get; set; }


    /// <summary>
    /// DCDC工作电流
    /// </summary>
    public double VCU_DCDC_Current
    { get; set; }



    /// <summary>
    /// ready
    /// </summary>
    public int VCU_Ready
    { get; set; }

    /// <summary>
    /// 高压器件连接状态
    /// </summary>
    public int VCU_Hdevice_Connection
    { get; set; }

    /// <summary>
    /// 钥匙档信号
    /// </summary>
    public int VCU_Keyposition
    { get; set; }

    /// <summary>
    /// 充电指令1
    /// </summary>
    public int VCU_ChargeOrder1
    { get; set; }

    /// <summary>
    /// 百公里能耗
    /// </summary>
    public double VCU_kwh
    { get; set; }

    /// <summary>
    /// 空调系统使能
    /// </summary>
    public int VCU_HCL_Run_Stop
    { get; set; }

    /// <summary>
    /// 电池正极继电器指令
    /// </summary>
    public int VCU_Positive_Relay
    { get; set; }

    /// <summary>
    /// 电池负极继电器指令
    /// </summary>
    public int VCU_Negative_Relay
    { get; set; }

    /// <summary>
    /// 电池预充电继电器指令
    /// </summary>
    public int VCU_Pre_Charging_Relay
    { get; set; }

    /// <summary>
    /// BMS启动状态
    /// </summary>
    public int VCU_BMS_Start
    { get; set; }

    /// <summary>
    /// 充电继电器状态
    /// </summary>
    public int VCU_Chargerelay_State
    { get; set; }

    /// <summary>
    /// PTC启动状态
    /// </summary>
    public int VCU_PTC_Run_Stop
    { get; set; }

    /// <summary>
    /// 碰撞开关状态/安全气囊状态
    /// </summary>
    public int VCU_Impact_Switch
    { get; set; }

    /// <summary>
    /// 制动真空泵状态
    /// </summary>
    public int VCU_Vacuum_Pump_State
    { get; set; }

    /// <summary>
    /// 制动真空泵报警
    /// </summary>
    public int VCU_Vacuum_Pump_Fault
    { get; set; }

    /// <summary>
    /// 加速踏板自检
    /// </summary>
    public int VCU_Accel_SelfCheck
    { get; set; }

    /// <summary>
    /// 外界允许的充电电流
    /// </summary>
    public double VCU_OutsideAllow_ChargA
    { get; set; }

    /// <summary>
    /// ECO模式
    /// </summary>
    public int VCU_ECOMode
    { get; set; }

    /// <summary>
    /// 驻车指令
    /// </summary>
    public int VCU_Parking_Order
    { get; set; }

    /// <summary>
    /// 制动状态
    /// </summary>
    public int VCU_BrakePedalSt
    { get; set; }

    /// <summary>
    /// 快充充电指令
    /// 0-停止 1-启动
    /// </summary>
    public int VCU_FastCharge_Oder
    { get; set; }

    /// <summary>
    /// 快慢充 逻辑添加
    /// 0.非充电状态 1.快充 2.慢充
    /// </summary>
    public int BMS_ChargeFS { get; set; }

    /// <summary>
    /// 充电指示灯
    /// </summary>
    public int VCU_ChargeLight
    { get; set; }

    /// <summary>
    /// 限功率信号
    /// </summary>
    public int VCU_PowerLimit
    { get; set; }

    /// <summary>
    /// 风扇占空比
    /// </summary>
    public double VCU_Fan_DutyRatio
    { get; set; }

    /// <summary>
    /// 水泵占空比
    /// </summary>
    public double VCU_Pump_DutyRatio
    { get; set; }

    /// <summary>
    /// 正常回馈模式
    /// </summary>
    public int Motor_NormalRegen_Mode
    { get; set; }

    /// <summary>
    /// 电机控制器强电通断状态
    /// </summary>
    public int Motor_MCU_Power_On_Off
    { get; set; }

    /// <summary>
    /// 电机启停状态
    /// </summary>
    public int Motor_Start_Stop
    { get; set; }

    /// <summary>
    /// 控制模式
    /// </summary>
    public int Motor_Control_Mode
    { get; set; }

    /// <summary>
    /// 电机实际输出扭矩
    /// </summary>
    public double Motor_OutputTorque
    { get; set; }

    /// <summary>
    /// 电机正反转状态
    /// </summary>
    public int Motor_Forward_Reverse
    { get; set; }

    /// <summary>
    /// 电机牵引制动状态
    /// </summary>
    public int Motor_Traction_Brake
    { get; set; }

    /// <summary>
    /// 直流电压
    /// </summary>
    public double Motor_Direct_Voltage
    { get; set; }

    /// <summary>
    /// 交流电流IA
    /// </summary>
    public double Motor_IA
    { get; set; }

    /// <summary>
    /// 电机交流电流IB
    /// </summary>
    public double Motor_IB
    { get; set; }

    /// <summary>
    /// 电机交流电流IC
    /// </summary>
    public double Motor_IC
    { get; set; }

    /// <summary>
    /// 电机交流电压UA
    /// </summary>
    public double Motor_UA
    { get; set; }

    /// <summary>
    /// 电机交流电压UB
    /// </summary>
    public double Motor_UB
    { get; set; }

    /// <summary>
    /// 电机交流电压UC
    /// </summary>
    public double Motor_UC
    { get; set; }

    /// <summary>
    /// 电池充放电状态
    /// </summary>
    public int BMS_State_Charge_Discharge
    { get; set; }

    /// <summary>
    /// 预充电压
    /// </summary>
    public double BMS_Pre_Voltage
    { get; set; }

    /// <summary>
    /// 电池最大允许放电功率
    /// </summary>
    public double BMS_Battery_Discharge_KW_MA
    { get; set; }

    /// <summary>
    /// 电池最大允许充电功率
    /// </summary>
    public double BMS_Battery_Charge_KW_MAX
    { get; set; }

    /// <summary>
    /// 电池最大允许放电电流
    /// </summary>
    public double BMS_Battery_Discharge_A_MAX
    { get; set; }

    /// <summary>
    /// 电池最大允许充电电流
    /// </summary>
    public double BMS_Battery_Charge_A_MAX
    { get; set; }

    /// <summary>
    /// 非车载充电机充电状态
    /// </summary>
    public int BMS_NOBC_State
    { get; set; }

    /// <summary>
    /// 电池正极继电器状态
    /// </summary>
    public int BMS_Positive_Relay
    { get; set; }

    /// <summary>
    /// 电池负极继电器状态
    /// </summary>
    public int BMS_Negative_Relay
    { get; set; }

    /// <summary>
    /// 电池预充继电器状态
    /// </summary>
    public int BMS_Pre_Charging_Relay
    { get; set; }

    /// <summary>
    /// 最高允许充电电压
    /// </summary>
    public double BMS_Battery_Charge_V_MAX
    { get; set; }

    /// <summary>
    /// 充电控制指令
    /// </summary>
    public int BMS_Charge_ControlOrder
    { get; set; }

    /// <summary>
    /// 充电电流
    /// </summary>
    public double ONC_Charge_Current
    { get; set; }

    /// <summary>
    /// 充电机输出电压
    /// </summary>
    public double ONC_OutputVoltage
    { get; set; }

    /// <summary>
    /// 充电机输出电流
    /// </summary>
    public double ONC_OutputCurrent
    { get; set; }

    /// <summary>
    /// 充电机温度报警
    /// </summary>
    public int ONC_Temp_Alarm
    { get; set; }

    /// <summary>
    /// 充电机输入电压故障
    /// </summary>
    public int ONC_InputVoltage_Fault
    { get; set; }

    /// <summary>
    /// 充电机启动状态
    /// </summary>
    public int ONC_Start_Stop
    { get; set; }

    /// <summary>
    /// 充电机通信状态
    /// </summary>
    public int ONC_CommunicationSt
    { get; set; }

    /// <summary>
    /// 充电机最高温度
    /// </summary>
    public double ONC_Highest_Temp
    { get; set; }

    /// <summary>
    /// 充电机最大充电电流
    /// </summary>
    public double ONC_Charge_Maxallow_A
    { get; set; }

    /// <summary>
    /// 充电机输入电流
    /// </summary>
    public double ONC_Input_A
    { get; set; }

    /// <summary>
    /// 充电机最低温度
    /// </summary>
    public double ONC_Lowest_Temp
    { get; set; }

    /// <summary>
    /// 充电机中间温度
    /// </summary>
    public double ONC_Middle_Temp
    { get; set; }

    /// <summary>
    /// 充电机输入过压故障
    /// </summary>
    public int ONC_Input_OverVoltage_Fault
    { get; set; }

    /// <summary>
    /// 充电机输入欠压故障
    /// </summary>
    public int ONC_Input_LowVoltage_Fault
    { get; set; }

    /// <summary>
    /// 充电机输出过压故障
    /// </summary>
    public int ONC_Output_OverVoltage_Fault
    { get; set; }

    /// <summary>
    /// 充电机输出欠压故障
    /// </summary>
    public int ONC_Output_LowVoltage_Fault
    { get; set; }

    /// <summary>
    /// DCDC工作状态
    /// </summary>
    public int DCDC_Work_State
    { get; set; }

    /// <summary>
    /// DCDC输出反接故障
    /// </summary>
    public int DCDC_Output_Inversed_Fault
    { get; set; }

    /// <summary>
    /// DCDC硬件故障
    /// </summary>
    public int DCDC_Hardware_Fault
    { get; set; }

    /// <summary>
    /// DCDC过温故障
    /// </summary>
    public int DCDC_OverTemp_Fault
    { get; set; }

    /// <summary>
    ///DCDC输入故障
    /// </summary>
    public int DCDC_Input_Fault
    { get; set; }

    /// <summary>
    /// DCDC输出故障
    /// </summary>
    public int DCDC_Output_Fault
    { get; set; }

    /// <summary>
    /// DCDC输出电流
    /// </summary>
    public double DCDC_OutputCurrent
    { get; set; }

    /// <summary>
    /// DCDC输出电压
    /// </summary>
    public double DCDC_OutputVoltage
    { get; set; }

    /// <summary>
    /// DCDC温度
    /// </summary>
    public double DCDC_Temperature
    { get; set; }

    /// <summary>
    /// 压缩机目标转速
    /// </summary>
    public double HCL_Target_rpm
    { get; set; }

    /// <summary>
    /// 蒸发器温度
    /// </summary>
    public double HCL_Evaporator_Temp
    { get; set; }

    /// <summary>
    /// 日照温度
    /// </summary>
    public double HCL_Sunshine_intensity
    { get; set; }

    /// <summary>
    /// 车内温度
    /// </summary>
    public double HCL_Inside_Temp
    { get; set; }

    /// <summary>
    /// 环境温度
    /// </summary>
    public double HCL_Environment_Temp
    { get; set; }

    /// <summary>
    /// 压缩机开关命令
    /// </summary>
    public int HCL_Start_Order
    { get; set; }

    /// <summary>
    /// PTC温度
    /// </summary>
    public double HCL_PTC_Temp
    { get; set; }

    /// <summary>
    /// 最大允许压缩机消耗的功率
    /// </summary>
    public double HCL_Comp_allowMaxKW
    { get; set; }

    /// <summary>
    /// 空调设定温度
    /// </summary>
    public double HCL_Set_Temp
    { get; set; }

    /// <summary>
    /// 压缩机当前转速
    /// </summary>
    public double CCL_Speed
    { get; set; }

    /// <summary>
    /// 压缩机当前相电流
    /// </summary>
    public double CCL_Current
    { get; set; }

    /// <summary>
    /// 压缩机当前电压
    /// </summary>
    public double CCL_Voltage
    { get; set; }

    /// <summary>
    /// 压缩机当前功率
    /// </summary>
    public double CCL_KW
    { get; set; }

    /// <summary>
    /// 压缩机当前温度
    /// </summary>
    public double CCL_Temp
    { get; set; }

    /// <summary>
    /// 压缩机故障
    /// </summary>
    public int CCL_Fault
    { get; set; }

    /// <summary>
    /// 压缩机故障等级
    /// </summary>
    public int CCL_Fault_Rate
    { get; set; }

    /// <summary>
    /// 压缩机使能状态
    /// </summary>
    public int CCL_Enabled_State
    { get; set; }

    /// <summary>
    /// 12V蓄电池电压过低
    /// </summary>
    public int PCTL_12V_L
    { get; set; }

    /// <summary>
    /// 12V蓄电池电压过高
    /// </summary>
    public int PCTL_12V_H
    { get; set; }

    /// <summary>
    /// P档系统当前状态
    /// </summary>
    public int PCTL_State
    { get; set; }

    /// <summary>
    /// P档控制器初始化状态
    /// </summary>
    public int PCTL_Initialization
    { get; set; }

    /// <summary>
    /// 执行电机开路故障
    /// </summary>
    public int PCTL_Motor_Open_Circuit
    { get; set; }

    /// <summary>
    /// 执行电机对电源短路故障
    /// </summary>
    public int PCTL_Motor_Power_Short_Circuit
    { get; set; }

    /// <summary>
    /// 执行电机对地短路故障
    /// </summary>
    public int PCTL_Motor_Ground_Short_Circuit
    { get; set; }

    /// <summary>
    /// P档退档超时故障
    /// </summary>
    public int PCTL_P_Back_Timeout
    { get; set; }

    /// <summary>
    /// P档进档超时故障
    /// </summary>
    public int PCTL_P_Into_Timeout
    { get; set; }

    /// <summary>
    /// P档位置传感器短路故障
    /// </summary>
    public int PCTL_P_Sensor_Short_Circuit
    { get; set; }

    /// <summary>
    /// P档位置传感器开路故障
    /// </summary>
    public int PCTL_P_Sensor_Open_Circuit
    { get; set; }

    /// <summary>
    /// CAN通讯故障
    /// </summary>
    public int PCTL_Can_Fault { get; set; }

}


#endregion

#region 车辆设备信息
/// <summary>
/// 车辆设备信息
/// </summary>
public class Cls_VehicleEquipmentInfo : Cls_RealInformation
{
    /// <summary>
    /// 供应商简码
    /// </summary>
    public string SupplierCode { get; set; }
    /// <summary>
    /// 零件简码
    /// </summary>
    public string PartCode { get; set; }
    /// <summary>
    /// 生产日期
    /// </summary>
    public string ProductionDate { get; set; }
    /// <summary>
    /// 生产流水号
    /// </summary>
    public string ProductionSerialNumber { get; set; }
    /// <summary>
    /// 生产厂商代码
    /// </summary>
    public string ProductionCode { get; set; }
    /// <summary>
    /// 电池类型代码
    /// </summary>
    public string BatteryTypeCode { get; set; }
    /// <summary>
    /// 额定能量
    /// </summary>
    public string RatedEnergy { get; set; }
    /// <summary>
    /// 额定电压
    /// </summary>
    public string RatedVoltage { get; set; }
    /// <summary>
    /// 电池生产日期
    /// </summary>
    public string BatteryProductionDateCode { get; set; }
    /// <summary>
    /// 流水号
    /// </summary>
    public string SerialNumber { get; set; }
    /// <summary>
    /// 车辆类型
    /// </summary>
    public string VehicleType { get; set; }
    /// <summary>
    /// 主参数
    /// </summary>
    public string MainParameters { get; set; }
    /// <summary>
    /// 车身或驾驶室类型
    /// </summary>
    public string BodyOrCabType { get; set; }
    /// <summary>
    /// 轴距、驱动型式
    /// </summary>
    public string WheelbaseDriveType { get; set; }
    /// <summary>
    /// 驱动电机功率
    /// </summary>
    public string DrivingMotorPower { get; set; }
    /// <summary>
    /// 检验位
    /// </summary>
    public string TestPosition { get; set; }
    /// <summary>
    /// 生产年份
    /// </summary>
    public string YearOfProduction { get; set; }
    /// <summary>
    /// 装配线
    /// </summary>
    public string AssemblyLine { get; set; }
    /// <summary>
    /// 生产顺序号1
    /// </summary>
    public string ProductionOrderNumber1 { get; set; }
    /// <summary>
    /// 生产顺序号2
    /// </summary>
    public string ProductionOrderNumber2 { get; set; }
    /// <summary>
    /// 生产顺序号3
    /// </summary>
    public string ProductionOrderNumber3 { get; set; }
    /// <summary>
    /// 生产顺序号4
    /// </summary>
    public string ProductionOrderNumber4 { get; set; }
    /// <summary>
    /// 生产顺序号5
    /// </summary>
    public string ProductionOrderNumber5 { get; set; }
    /// <summary>
    /// 生产顺序号6
    /// </summary>
    public string ProductionOrderNumber6 { get; set; }
    /// <summary>
    /// VIN匹配回应请求
    /// </summary>
    public int VINMatchingRequest { get; set; }
    /// <summary>
    /// VIN发送请求
    /// </summary>
    public int VINSendingRequest { get; set; }
    /// <summary>
    /// VIN接收回应
    /// </summary>
    public int VINReceivingResponse { get; set; }
    /// <summary>
    /// VIN匹配回应
    /// </summary>
    public int VINMatchingResponse { get; set; }
}

#endregion

/// <summary>
/// 车载终端状态信息上报
/// </summary>
public class Cls_VehicleState
{
    /// <summary>
    /// 通电状态
    /// </summary>
    public string ElectricityStatu { get; set; }
    /// <summary>
    /// 电源状态
    /// </summary>
    public string PowerSupplyStatu { get; set; }
    /// <summary>
    /// 通信传输标志
    /// </summary>
    public string CommunicationStatu { get; set; }
    /// <summary>
    /// 其他异常
    /// </summary>
    public string OtherExceptions { get; set; }

}
