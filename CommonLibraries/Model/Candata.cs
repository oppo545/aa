using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


public interface Candata
{

}

/// <summary>
/// s30
/// </summary>
public class Candata_s30 : Candata
{
    #region S30

    private string eco_off_switch;

    public string Eco_off_switch
    {
        get { return eco_off_switch; }
        set { eco_off_switch = value; }

    }

    private string bat_vol;

    public string Bat_vol
    {
        get { return bat_vol; }
        set { bat_vol = value; }
    }


    private string insFuelConsumed;

    public string InsFuelConsumed
    {
        get { return insFuelConsumed; }
        set { insFuelConsumed = value; }
    }

    private string error0;

    public string Error0
    {
        get { return error0; }
        set { error0 = value; }
    }
    private string error1;

    public string Error1
    {
        get { return error1; }
        set { error1 = value; }
    }
    private string error2;

    public string Error2
    {
        get { return error2; }
        set { error2 = value; }
    }


    private string err_level;

    public string Err_level
    {
        get { return err_level; }
        set { err_level = value; }
    }

    private string start_total_timers;

    public string Start_total_timers
    {
        get { return start_total_timers; }
        set { start_total_timers = value; }
    }

    private string reserved_info_1;



    public string Reserved_info_1
    {
        get { return reserved_info_1; }
        set { reserved_info_1 = value; }
    }
    private string reserved_info_2;

    public string Reserved_info_2
    {
        get { return reserved_info_2; }
        set { reserved_info_2 = value; }
    }
    private string reserved_info_3;

    public string Reserved_info_3
    {
        get { return reserved_info_3; }
        set { reserved_info_3 = value; }
    }
    private string reserved_info_4;

    public string Reserved_info_4
    {
        get { return reserved_info_4; }
        set { reserved_info_4 = value; }
    }
    private string reserved_info_5;

    public string Reserved_info_5
    {
        get { return reserved_info_5; }
        set { reserved_info_5 = value; }
    }

    #endregion
}

/// <summary>
/// 聆风 纯电动
/// </summary>
public class Candata_Ev_lf : Candata
{
    #region 聆风

    private double curent;

    public double Curent
    {
        get { return curent; }
        set { curent = value; }
    }



    private double voltage;

    public double Voltage
    {
        get { return voltage; }
        set { voltage = value; }
    }




    private double sOC;

    public double SOC
    {
        get { return sOC; }
        set { sOC = value; }
    }


    private double motorRevolution;

    public double MotorRevolution
    {
        get { return motorRevolution; }
        set { motorRevolution = value; }
    }

    private double motorTemperature;

    public double MotorTemperature
    {
        get { return motorTemperature; }
        set { motorTemperature = value; }
    }


    private double outputPower;

    public double OutputPower
    {
        get { return outputPower; }
        set { outputPower = value; }
    }


    private double aBS_VehSpd;

    public double ABS_VehSpd
    {
        get { return aBS_VehSpd; }
        set { aBS_VehSpd = value; }
    }



    private double distance;

    public double Distance
    {
        get { return distance; }
        set { distance = value; }
    }


    private double batteryTemp_Ave;

    public double BatteryTemp_Ave
    {
        get { return batteryTemp_Ave; }
        set { batteryTemp_Ave = value; }
    }




    private double batteryTemp_Max;

    public double BatteryTemp_Max
    {
        get { return batteryTemp_Max; }
        set { batteryTemp_Max = value; }
    }


    private double batteryTemp_Min;

    public double BatteryTemp_Min
    {
        get { return batteryTemp_Min; }
        set { batteryTemp_Min = value; }
    }


    #endregion
}


/// <summary>
/// s30_EJ02
/// </summary>
public class Candata_EJ02 : Candata
{
    #region EJ02

    public double IC_TotalOdmeter { get; set; }
    public double IC_Odmeter { get; set; }
    public double ONC_OutputVoltage { get; set; }
    public double ONC_OutputCurrent { get; set; }


    public string ONC_InputVoltageSt { get; set; }
    public string ONC_CommunicationSt { get; set; }

    public double ONC_ONCTemp { get; set; }

    public string ONC_Fault { get; set; }

    public double BMS_TotalVol { get; set; }
    public double BMS_MaxCellBatt { get; set; }

    public int BMS_MaxCellBattNumber { get; set; }
    public double BMS_MinCellBatt { get; set; }
    public int BMS_MinCellBattNumber { get; set; }
    public double BMS_MaxTemp { get; set; }
    public int BMS_MaxTempNumber { get; set; }
    public double BMS_MinTemp { get; set; }
    public int BMS_MinTempNumber { get; set; }
    public double BMS_BattTempAvg { get; set; }
    public double BMS_ChargeQuantity { get; set; }

    public string BMS_OnBoardChargerEnable { get; set; }
    public string BMS_ChargeSt { get; set; }
    public string BMS_ChargerACInput { get; set; }
    public string BMS_ChargerDCInput { get; set; }

    public double BMS_SOC { get; set; }
    public double BMS_Current { get; set; }

    public string BMS_BattSelfCheckStatus { get; set; }
    public string BMS_HighVolSt { get; set; }
    public string BMS_FaultState { get; set; }
    public string BMS_Fault { get; set; }

    public double BMS_CruisingRange { get; set; }
    public double Motor_DCVolt { get; set; }
    public double Motor_DCCurrent { get; set; }
    public double Motor_Temperature { get; set; }
    public string Motor_MotorStatus { get; set; }

    public double Motor_OutputPower { get; set; }

    public string MCU1_FaultNum { get; set; }
    public string MCU2_FaultNum2 { get; set; }

    public double MCU2_RadiatorTemp { get; set; }

    public string TCU_TransRealPosition { get; set; }
    public string VCU_MotorStCtrl { get; set; }
    public string ABS_BrakeSignal { get; set; }
    public double ABS_VehSpd { get; set; }



    #endregion
}


/// <summary>
/// s30_EJ04
/// </summary>
public class Candata_EJ04 : Candata, Cls_RealInformation
{

    #region EJ04
    [Description("点火钥匙信号")]
    public string VCU_Keyposition { get; set; }
    [Description("VCU故障码")]
    public int VCU_Fault { get; set; }
    [Description("电机状态及信号强度")]
    public string VCU_BrakeEnergy { get; set; }
    [Description("剩余行驶里程")]
    public double VCU_CruisingRange { get; set; }

    [Description("制动开关状态")]
    public string VCU_BrakePedalSt { get; set; }
    [Description("换挡手柄位置")]
    public string TCU_GearPosition { get; set; }
    [Description("电机控制器母线高压")]
    public double Motor_DCVolt { get; set; }
    [Description("电机控制器母线电流")]
    public double Motor_DCCurrent { get; set; }
    [Description("电机状态")]
    public string Motor_State { get; set; }
    [Description("电机转速")]
    public double Motor_Revolution { get; set; }
    [Description("电机输出扭矩")]
    public double Motor_OutputTorque { get; set; }
    [Description("电机最大允许扭矩")]
    public double Motor_AllowMaxTorque { get; set; }
    [Description("电机故障码")]
    public int Motor_Fault { get; set; }
    [Description("电机温度")]
    public double Motor_Temperature { get; set; }
    [Description("电机控制器温度")]
    public double Motor_ControllerTemp { get; set; }
    [Description("电机输出功率")]
    public double Motor_OutputPower { get; set; }
    [Description("电池SOC")]
    public double BMS_SOC { get; set; }
    [Description("电池电压")]
    public double BMS_Voltage { get; set; }
    [Description("电池电流")]
    public double BMS_Current { get; set; }
    [Description("漏电检测")]
    public double BMS_CreepageMonitor { get; set; }
    [Description("电池SOC（处理后）")]
    public double BMS_SOCCalculate { get; set; }
    [Description("外部充电信号（动力电池充电指示）")]
    public string BMS_OutsideChargeSignal { get; set; }
    [Description("电池故障（显示用）")]
    public string BMS_FaultDislpay { get; set; }

    //原BMS_BattTemp
    [Description("电池温度")]
    public double BMS_Temperature { get; set; }
    //BMS_MaxBattTemp
    [Description("电池最高温度")]
    public double BMS_Temp_Max { get; set; }
    //BMS_MinBattTemp
    [Description("电池最低温度")]
    public double BMS_Temp_Min { get; set; }
    [Description("电池故障码")]
    public int BMS_Fault { get; set; }
    [Description("最高单体电压")]
    public double BMS_MaxCellBatt { get; set; }
    [Description("最低单体电压")]
    public double BMS_MinCellBatt { get; set; }
    [Description("最高单体电压电池编号")]
    public int BMS_MaxCellBattNumber { get; set; }
    [Description("最低单体电压电池编号")]
    public int BMS_MinCellBattNumber { get; set; }

    //充电状态
    [Description("充电状态")]
    public string BMS_ChargeSt { get; set; }

    [Description("充电机输出的充电电压")]
    public double ONC_OutputVoltage { get; set; }
    [Description("充电机输出的充电电流")]
    public double ONC_OutputCurrent { get; set; }
    [Description("输入电压状态")]
    public string ONC_InputVoltageSt { get; set; }
    [Description("通信状态")]
    public string ONC_CommunicationSt { get; set; }

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

    [Description("充电机温度")]
    public double ONC_ONCTemp { get; set; }
    [Description("充电机故障描述")]
    public string ONC_Fault { get; set; }

    [Description("总里程")]
    public double IC_TotalOdmeter { get; set; }
    [Description("小计里程")]
    public double IC_Odmeter { get; set; }
    [Description("制动信号")]
    public string ABS_BrakeSignal { get; set; }
    [Description("车速")]
    public double ABS_VehSpd { get; set; }
    [Description("直流电转换器温度")]
    public double DCDC_Temperature { get; set; }
    [Description("直流电转换器输出电压")]
    public double DCDC_OutputVoltage { get; set; }
    [Description("直流电转换器输出电流")]
    public double DCDC_OutputCurrent { get; set; }
    [Description("直流电转换器使能应答")]
    public string DCDC_EnableResponse { get; set; }
    [Description("DCDC故障")]
    public string DCDC_Fault { get; set; }
    [Description("直流电转换器输入电压")]
    public double DCDC_InputVoltage { get; set; }
    [Description("直流电转换器输入电流")]
    public double DCDC_InputCurrent { get; set; }

    [Description("交流输入状态")]
    //2035-10-12 添加 交流输入状态
    public string BMS_ChargerACInput { get; set; }
    [Description("非车载充电连接指示信号")]
    public string BMS_OFCConnectSignal { get; set; }

    [Description("快慢充")]
    public int BMS_ChargeFS { get; set; }
    #endregion
}

/// <summary>
///A60
/// </summary>
public class Candata_A60 : Candata, Cls_RealInformation
{

    #region A60

    public string VCU_Keyposition { get; set; }
    public int VCU_Fault { get; set; }

    public double BMS_SOC { get; set; }
    public double BMS_Voltage { get; set; }
    public double BMS_Current { get; set; }
    public double BMS_CreepageMonitor { get; set; }
    public double BMS_SOCCalculate { get; set; }

    public string BMS_OutsideChargeSignal { get; set; }
    public string BMS_FaultDislpay { get; set; }
    public string BMS_OFCConnectSignal { get; set; }


    //原BMS_BattTemp
    public double BMS_Temperature { get; set; }
    //BMS_MaxBattTemp
    public double BMS_Temp_Max { get; set; }
    //BMS_MinBattTemp
    public double BMS_Temp_Min { get; set; }

    public int BMS_Fault { get; set; }

    public double BMS_MaxCellBatt { get; set; }
    public double BMS_MinCellBatt { get; set; }

    public int BMS_MaxCellBattNumber { get; set; }
    public int BMS_MinCellBattNumber { get; set; }

    //充电状态
    public string BMS_ChargeSt { get; set; }

    //BMK 2035-10-20 为统一,将BMS_ChargeSt 设为充电状态 ,  BMS_SlowChargeSt 为慢充
    //充电状态(慢充)
    public string BMS_SlowChargeSt { get; set; }
    //充电状态(快充)
    public string BMS_FastChargeSt { get; set; }

    //快慢充 逻辑添加
    public int BMS_ChargeFS { get; set; }

    public string ONC_Fault { get; set; }

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


    public string MCU_ElecPowerTrainMngtState { get; set; }
    public double Motor_Torqueestimation { get; set; }
    public string MCU_ElecMachineFault { get; set; }
    public double MCU_InternalMachineTemp { get; set; }
    public double MCU_MaxMotorTorque { get; set; }
    public double Motor_MaxGenTorque { get; set; }

    public double Motor_Revolution { get; set; }

    public string MCU_ActiveDischarge { get; set; }

    public double Motor_DCCurrent { get; set; }
    public double Motor_ControllerTemp { get; set; }
    public double Motor_Temperature { get; set; }
    public double IC_TotalOdmeter { get; set; }
    public double IC_Odmeter { get; set; }
    public double VCU_CruisingRange { get; set; }

    public string VCU_BrakeEnergy { get; set; }
    #endregion
}
