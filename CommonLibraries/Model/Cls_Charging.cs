using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


public class Cls_Charging : Candata, Cls_RealInformation
{

    public string Date { get; set; }
    [Description("充电度数")]
    /// <summary>
    /// 充电度数
    /// </summary>
    public double ChargeNumber { get; set; }

    [Description("充电桩终端编号")]
    /// <summary>
    /// 充电桩终端编号
    /// </summary>
    public string Charg_TerminalNumber { get; set; }

    [Description("充电接口标识")]
    /// <summary>
    /// 充电接口标识
    /// </summary>
    public int Charg_PortIdentification { get; set; }

    [Description("连接确认开关状态")]
    /// <summary>
    /// 连接确认开关状态
    /// </summary>
    public int ConnectSwitchConfirm { get; set; }

    [Description("工作状态")]
    /// <summary>
    /// 工作状态
    /// </summary>
    public int WorkState { get; set; }

    [Description("交流输入过压告警")]
    /// <summary>
    /// 交流输入过压告警
    /// </summary>
    public int ACInputOverVoltageAlarm { get; set; }

    [Description("交流输入欠压告警")]
    /// <summary>
    /// 交流输入欠压告警
    /// </summary>
    public int ACInputUnderVoltageAlarm { get; set; }

    [Description("充电电流过负荷告警")]
    /// <summary>
    /// 充电电流过负荷告警
    /// </summary>
    public int ChargingCurrentOverloadAlarm { get; set; }

    [Description("充电输出电压")]
    /// <summary>
    /// 充电输出电压
    /// </summary>
    public double ChargingOutputVoltage { get; set; }

    [Description("充电输出电流")]
    /// <summary>
    /// 充电输出电流
    /// </summary>
    public double ChargingOutputCurrent { get; set; }

    [Description("输出继电器状态")]
    /// <summary>
    /// 输出继电器状态
    /// </summary>
    public int OutputRelayStatus { get; set; }

    [Description("有功总电度")]
    /// <summary>
    /// 有功总电度
    /// </summary>
    public double TotalActivePower { get; set; }


    /// <summary>
    /// 累计充电时间
    /// </summary>
    [Description("累计充电时间")]
    public int TotalChargeTime { get; set; }




    /// <summary>
    /// SOC
    /// </summary>
    [Description("SOC")]
    public double BMS_SOC { get; set; }

    [Description("电池组最低温度")]
    /// <summary>
    /// 电池组最低温度
    /// </summary>
    public double BatteryPackMinTemp { get; set; }

    [Description("电池组最高温度")]
    /// <summary>
    /// 电池组最高温度
    /// </summary>
    public double BatteryPackMaxTemp { get; set; }

    [Description("BMS通讯异常")]
    /// <summary>
    /// BMS通讯异常
    /// </summary>
    public int BMS_CommunicationFault { get; set; }

    [Description("直流母线输出过压告警")]
    /// <summary>
    /// 直流母线输出过压告警
    /// </summary>
    public int DCBusOutputOvervoltageAlarm { get; set; }

    [Description("直流母线输出欠压告警")]
    /// <summary>
    /// 直流母线输出欠压告警
    /// </summary>
    public int DCBusOutputUndervoltageAlarm { get; set; }

    [Description("蓄电池充电过流告警")]
    /// <summary>
    /// 蓄电池充电过流告警
    /// </summary>
    public int BatteryChargingOverCurrentAlarm { get; set; }

    [Description("蓄电池模块采样点过温告警")]
    /// <summary>
    /// 蓄电池模块采样点过温告警
    /// </summary>
    public int BatteryModuleSamplingPointOverTempAlarm { get; set; }

    [Description("是否连接电池")]
    /// <summary>
    /// 是否连接电池
    /// </summary>
    public int BatteryConnectStatue { get; set; }

    [Description("单体电池最高电压")]
    /// <summary>
    /// 单体电池最高电压
    /// </summary>
    public double BatteryMaxVoltage { get; set; }

    [Description("单体电池最低电压")]
    /// <summary>
    /// 单体电池最低电压
    /// </summary>
    public double BatteryMinVoltage { get; set; }
    [Description("急停按钮动作故障")]
    /// <summary>
    /// 急停按钮动作故障
    /// </summary>
    public int EmergencyStopButtonActionFault { get; set; }

    [Description("读卡器通讯异常故障")]
    /// <summary>
    /// 读卡器通讯异常故障
    /// </summary>
    public int CardReaderCommunicationFault { get; set; }

    [Description("直流电度表异常故障")]
    /// <summary>
    /// 直流电度表异常故障
    /// </summary>
    public int DCMeterExceptionFault { get; set; }

    [Description("绝缘监测故障")]
    /// <summary>
    /// 绝缘监测故障
    /// </summary>
    public int InsulationMonitoringFault { get; set; }
    [Description("电池反接故障")]
    /// <summary>
    /// 电池反接故障
    /// </summary>
    public int BatteryReverseFault { get; set; }
    [Description("避雷器故障")]
    /// <summary>
    /// 避雷器故障
    /// </summary>
    public int ArresterFault { get; set; }
    [Description("充电枪未连接告警")]
    /// <summary>
    /// 充电枪未连接告警
    /// </summary>
    public int ChargingGunNotConnectedAlarm { get; set; }
    [Description("充电机过温故障")]
    /// <summary>
    /// 充电机过温故障
    /// </summary>
    public int ChargerOverTempFault { get; set; }
    [Description("烟雾报警告警")]
    /// <summary>
    /// 烟雾报警告警
    /// </summary>
    public int SmokeAlarm { get; set; }

    [Description("交易记录已满告警")]
    /// <summary>
    /// 交易记录已满告警
    /// </summary>
    public int TransactionRecordFullAlarm { get; set; }



}
