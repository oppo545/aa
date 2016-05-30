using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ServerCenterLis
{
    public class ParsEVT
    {
        static string result;
        static string zcinfo1;
        static string packageNumber;
        static string lastPackageNumber;
        static int num = 1;//信号长度 字节数
        /// 故障值  0正常         /// 故障等级    1	轻微故障,2	一般故障,3	严重故障,4	致命故障
        static int faultlever = 1; static int? faultint = 0;
        public static Cls_RealInformation GetParsEVT(Telnet_Session session, string info1, int msgnumber, ref string canfault)
        {
            try
            {
                string str_all;
                int data1 = 0, data2 = 0;
                string str_data1, str_data2, str_data3, str_data4, str_data;
                session.cd_evt = session.getClsCdEVT();
                Regex regex = new Regex("\\s+");
                for (int i = 0; i < msgnumber; i++)
                {
                    if (i > 0)
                    {
                        zcinfo1 = zcinfo1.Substring((num + 2) * 3);
                    }
                    else
                    { zcinfo1 = info1; }
                    packageNumber = PublicMethods.GetZeroSuppression(zcinfo1.Substring(0, 5));
                    str_all = zcinfo1.Substring(6);
                    #region
                    switch (packageNumber)
                    {
                        #region 车况 0x00-0X2FF
                        #region 车况数据0x00-0X3FF
                        #region 普通车况0x00-0xFF
                        case "0001":         /// 钥匙档位
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.VCU_Keyposition = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0002":   //档位
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.TCU_ExtensionGearPosition = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0003":       /// ECO模式
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.VCU_ECOMode = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0010":             /// 正极绝缘电阻
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.PositiveInsulationResistance = ParsMethod.GetParsWholeByte(str_data1, 10);
                            break;
                        case "0011":        /// 负极绝缘电阻
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.NegativeInsulationResistance = ParsMethod.GetParsWholeByte(str_data1, 10);
                            break;
                        case "0012":            /// 油门深度(分辨率1%)
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.Motor_ThrottleDepth = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0013":           /// 制动深度(分辨率1%)
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BrakeDepth = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0014":                /// 充、放电系统工作状态
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.SystemWorkState_Charge_Discharge = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0015":                /// 母线电压
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.BusVoltage = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "0016":          /// IPM散热器温度（分辨率1℃)
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.IPMRadiatorTemperature = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                            break;
                        case "0017":            /// IGBT温度（分辨率1℃）
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.IGBTTemperature = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                            break;
                        case "0018":                  /// 动力电机母线电压
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.Motor_PowerBusVoltage = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "0019":                      /// 冷却液温度
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.CoolantTemperature = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                            break;
                        case "0020":     /// 目标压力
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.TargetPressure = ParsMethod.GetParsWholeByte(str_data1, 4);
                            break;
                        case "0021":    /// 后轿左气室压力
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BackLeftAirChamberPressure = ParsMethod.GetParsWholeByte(str_data1, 4);
                            break;
                        case "0022":        /// 真空泵状态
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.VacuumPumpState = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0023":       /// 助力转向油泵状态
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.PowerSteeringOilPumpState = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0024":       /// 小计里程
                            num = 4;
                            str_data1 = str_all.Substring(0, 11);
                            session.cd_evt.IC_Odmeter = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "0025":       /// 漏电检测
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.BMS_CreepageMonitor = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0026":       /// SOC经过计算
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.BMS_SOCCalculate = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0027":       /// 外部充电信号
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BMS_OutsideChargeSignal = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0028":       /// 非车载充电连接指示信号
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BMS_OFCConnectSignal = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0030":       /// 制动开关状
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.VCU_BrakePedalSt = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0031":       /// 制动踏板电压信号
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.BrakeVoltageSignal = ParsMethod.GetParsWholeByte(str_data1, 0.005);
                            break;
                        case "0032":       /// （ABS）制动信号
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.ABS_BrakeSignal = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0033":       /// 制动开关有效信号
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.VCU_BrakePedalSwitchValid = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0034":       /// 整车工作模式
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.VehicleState = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0035":       /// 电机控制模式请求
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.mode_Motor_req = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0036":       /// 回馈制动请求
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.req_Regen = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0037":       /// 高压上电请求
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.req_BattPackHV = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0038":       /// BMS休眠允许标志
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.flg_BMSSleepAllow = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0039":       /// 充电口连接状态
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.ChargePortConnect_VCU = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "003A":       /// CP电压值
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.CP_Voltage = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "003B":       /// 供电设备最大供电电流
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.ACCharger_Max_Cur = ParsMethod.GetParsWholeByte(str_data1, 0.5);
                            break;
                        case "003C":       /// 充电电缆容量
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.charge_cable_capacity = ParsMethod.GetParsWholeByte(str_data1, 0.5);
                            break;
                        case "003D":
                            num = 4;
                            #region byte1
                            str_data2 = str_all.Substring(0, 2);
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 8)
                                str_data1 = str_data1.PadLeft(8, '0');
                            result = str_data1.Substring(7, 1);
                            session.cd_evt.PowerBattLV = result;
                            result = str_data1.Substring(6, 1);
                            session.cd_evt.batt_sys_flt = result;
                            result = str_data1.Substring(5, 1);
                            session.cd_evt.MCUorMotor_ot_flt = result;
                            result = str_data1.Substring(4, 1);
                            session.cd_evt.EPS_flt = result;
                            result = str_data1.Substring(3, 1);
                            session.cd_evt.state_PowerReady = result;
                            result = str_data1.Substring(2, 1);
                            session.cd_evt.state_EPB_enable = result;
                            #endregion
                            #region byte2
                            str_data2 = str_all.Substring(3, 2);
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 8)
                                str_data1 = str_data1.PadLeft(8, '0');
                            result = str_data1.Substring(7, 1);
                            session.cd_evt.state_mainErrLight = result;
                            canfault += PublicMethods.GetCanFaultStr("state_mainErrLight", "0X" + str_data2 + "-bit0", 1, int.Parse(result));
                            result = str_data1.Substring(6, 1);
                            session.cd_evt.state_DCSysErrLight = result;
                            canfault += PublicMethods.GetCanFaultStr("state_DCSysErrLight", "0X" + str_data2 + "-bit1", 1, int.Parse(result));
                            result = str_data1.Substring(5, 1);
                            session.cd_evt.state_CruiseLight = result;
                            result = str_data1.Substring(4, 1);
                            session.cd_evt.state_PowerSysErrLight = result;
                            result = str_data1.Substring(3, 1);
                            session.cd_evt.state_OverSpeedAlarm = result;
                            result = str_data1.Substring(2, 1);
                            session.cd_evt.state_PTCContactorInterlock = result;
                            result = str_data1.Substring(1, 1);
                            session.cd_evt.state_ACContactorInterlock = result;
                            #endregion
                            #region byte3
                            str_data2 = str_all.Substring(6, 2);
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 8)
                                str_data1 = str_data1.PadLeft(8, '0');
                            result = str_data1.Substring(7, 1);
                            session.cd_evt.ABS_SysFault = result;
                            canfault += PublicMethods.GetCanFaultStr("ABS_SysFault", "0X" + str_data2 + "-bit0", 1, int.Parse(result));
                            result = str_data1.Substring(6, 1);
                            session.cd_evt.AcceleratorFault = result;
                            canfault += PublicMethods.GetCanFaultStr("AcceleratorFault", "0X" + str_data2 + "-bit1", 1, int.Parse(result));
                            result = str_data1.Substring(5, 1);
                            session.cd_evt.VehErrShutDown = result;
                            canfault += PublicMethods.GetCanFaultStr("VehErrShutDown", "0X" + str_data2 + "-bit2", 1, int.Parse(result));
                            result = str_data1.Substring(4, 1);
                            session.cd_evt.state_ThreehaseInterlock = result;
                            result = str_data1.Substring(3, 1);
                            session.cd_evt.state_EHPSInterlock = result;
                            result = str_data1.Substring(2, 1);
                            session.cd_evt.state_ACPowerInterlock = result;
                            result = str_data1.Substring(1, 1);
                            session.cd_evt.state_DCPowerInterlock = result;
                            result = str_data1.Substring(0, 1);
                            session.cd_evt.state_PowerBatteryInterlock = result;
                            #endregion
                            #region byte4
                            str_data2 = str_all.Substring(9, 2);
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 8)
                                str_data1 = str_data1.PadLeft(8, '0');
                            result = str_data1.Substring(7, 1);
                            session.cd_evt.state_Information1 = result;
                            result = str_data1.Substring(6, 1);
                            session.cd_evt.state_Information2 = result;
                            result = str_data1.Substring(5, 1);
                            session.cd_evt.state_Information3 = result;
                            result = str_data1.Substring(4, 1);
                            session.cd_evt.state_Information4 = result;
                            result = str_data1.Substring(3, 1);
                            session.cd_evt.state_Information5 = result;
                            result = str_data1.Substring(2, 1);
                            session.cd_evt.state_Information6 = result;
                            result = str_data1.Substring(1, 1);
                            session.cd_evt.state_Information7 = result;
                            result = str_data1.Substring(0, 1);
                            session.cd_evt.state_Information8 = result;
                            #endregion
                            break;
                        case "003E":       /// 百公里电耗
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.PowerCons = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "003F":       /// 整车功率
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.VehPowerPercent = ParsMethod.GetParsWholeByte(str_data1, 1, -100);
                            break;
                        case "0060":       /// 电耗计算重置
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.req_PowerCostReset = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0061":       /// EPB状态指示灯
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.flg_EPB_State = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0062":       /// 左前轮速
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.LeftFrontWheelSpeed = ParsMethod.GetParsWholeByte(str_data1, 0.0625);
                            break;
                        case "0063":       /// 右前轮速
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.RightFrontWheelSpeed = ParsMethod.GetParsWholeByte(str_data1, 0.0625);
                            break;
                        case "0064":       ///左后轮速
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.LeftRearWheelSpeed = ParsMethod.GetParsWholeByte(str_data1, 0.0625);
                            break;
                        case "0065":       /// 右后轮速
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.RightRearWheelSpeed = ParsMethod.GetParsWholeByte(str_data1, 0.0625);
                            break;
                        #endregion
                        #region 电机0x100-0x1FF
                        case "0101":            /// 电机扭矩
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.Motor_OutputTorque = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "0102":     /// 电机目标扭矩
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.VCU_Motor_Target_tq = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "0103":       /// 电机控制器请求
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.Motor_ControllerRequest = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0104":       /// 电机工作模式反馈
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.Motor_WorkModeFeedback = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0105":       /// 电机控制器工作状态
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MCU_ElecPowerTrainMngtState = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0106":       /// 电机转子温度
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MCU_InternalMachineTemp = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                            break;
                        case "0107":       /// 电机最大电动扭矩
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MCU_MaxMotorTorque = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0108":       /// 电机最大发电扭矩
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.Motor_MaxGenTorque = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0109":       /// 电机控制器母线电容放电状
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MCU_ActiveDischarge = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0110":       /// 电机状态及信号强度
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.VCU_BrakeEnergy = ParsMethod.GetParsWholeByte(str_data1, 1, -50);
                            break;
                        case "0111":       /// 电机状态
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.Motor_State = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0112":       /// 电机反馈扭矩
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.Motor_TorqueFeedback = ParsMethod.GetParsWholeByte(str_data1, 0.1, -300);
                            break;
                        case "0113":       /// 电机最大允许扭矩
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.Motor_AllowMaxTorque = ParsMethod.GetParsWholeByte(str_data1, 0.1, -300);
                            break;
                        case "0114":       /// 电机功率
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.Motor_OutputPower = ParsMethod.GetParsWholeByte(str_data1, 0.1, -100);
                            break;
                        case "0115":       /// 转向电机转速请求
                            num = 4;
                            str_data1 = str_all.Substring(0, 11);
                            session.cd_evt.EpsMotSpeed = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0116":       /// 电机U相电流
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.U_MotCurrent = ParsMethod.GetParsWholeByte(str_data1, 0.1, -600);
                            break;
                        case "0117":       /// 电机V相电流
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.V_MotCurrent = ParsMethod.GetParsWholeByte(str_data1, 0.1, -600);
                            break;
                        case "0118":       /// 电机W相电流
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.W_MotCurrent = ParsMethod.GetParsWholeByte(str_data1, 0.1, -600);
                            break;
                        case "0119":       /// 电机控制状态及故障信息
                            num = 4;
                            #region byte1
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MCU_MotRunMode = ParsMethod.GetValuebyBinary(str_data1, 0, 4);
                            session.cd_evt.MCU_MotFeedBackMode = ParsMethod.GetValuebyBinary(str_data1, 4, 1);
                            session.cd_evt.MCU_PreComplete = ParsMethod.GetValuebyBinary(str_data1, 5, 1);
                            session.cd_evt.MCU_InitializeCompl = ParsMethod.GetValuebyBinary(str_data1, 6, 1);
                            #endregion
                            #region byte2
                            str_data1 = str_all.Substring(3, 2);
                            faultlever = ParsMethod.GetValuebyBinary(str_data1, 8, 2, 1);
                            faultint = faultlever.Equals("0") ? 0 : 1;
                            session.cd_evt.MCU_MotOTFault = faultint;
                            canfault += PublicMethods.GetCanFaultStr("MCU_MotOTFault", "0X" + str_data1 + "-bit7", faultlever, faultint);
                            faultlever = ParsMethod.GetValuebyBinary(str_data1, 10, 2, 1);
                            faultint = faultlever.Equals("0") ? 0 : 1;
                            session.cd_evt.MCU_MotOverCurrent = faultint;
                            canfault += PublicMethods.GetCanFaultStr("MCU_MotOverCurrent", "0X" + str_data1 + "-bit5", faultlever, faultint);
                            faultlever = ParsMethod.GetValuebyBinary(str_data1, 12, 2, 1);
                            faultint = faultlever.Equals("0") ? 0 : 1;
                            session.cd_evt.MCU_IGBTTempProtect = faultint;
                            canfault += PublicMethods.GetCanFaultStr("MCU_IGBTTempProtect", "0X" + str_data1 + "-bit3", faultlever, faultint);
                            faultlever = ParsMethod.GetValuebyBinary(str_data1, 14, 2, 1);
                            faultint = faultlever.Equals("0") ? 0 : 1;
                            session.cd_evt.MCU_OverVoltage = faultint;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("MCU_OverVoltage", "0X" + str_data1 + "-bit1", faultlever, faultint);
                            #endregion
                            #region byte3
                            str_data1 = str_all.Substring(6, 2);
                            faultlever = ParsMethod.GetValuebyBinary(str_data1, 16, 2, 2);
                            faultint = faultlever.Equals("0") ? 0 : 1;
                            session.cd_evt.MCU_UnderVoltage = faultint;
                            canfault += PublicMethods.GetCanFaultStr("MCU_UnderVoltage", "0X" + str_data1 + "-bit7", faultlever, faultint);
                            faultlever = ParsMethod.GetValuebyBinary(str_data1, 18, 2, 2);
                            faultint = faultlever.Equals("0") ? 0 : 1;
                            session.cd_evt.MCU_OTFault = faultint;
                            canfault += PublicMethods.GetCanFaultStr("MCU_OTFault", "0X" + str_data1 + "-bit5", faultlever, faultint);
                            faultlever = ParsMethod.GetValuebyBinary(str_data1, 20, 2, 2);
                            faultint = faultlever.Equals("0") ? 0 : 1;
                            session.cd_evt.MCU_MotOverSpeed = faultint;
                            canfault += PublicMethods.GetCanFaultStr("MCU_MotOverSpeed", "0X" + str_data1 + "-bit3", faultlever, faultint);
                            faultlever = ParsMethod.GetValuebyBinary(str_data1, 22, 2, 2);
                            faultint = faultlever.Equals("0") ? 0 : 1;
                            session.cd_evt.MCU_OverCurrent = faultint;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("MCU_OverCurrent", "0X" + str_data1 + "-bit1", faultlever, faultint);
                            #endregion
                            #region byte4
                            str_data2 = str_all.Substring(9, 2);
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 8)
                                str_data1 = str_data1.PadLeft(8, '0');
                            result = str_data1.Substring(7, 1);
                            session.cd_evt.MCU_CANFault = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("MCU_CANFault", "0X" + str_data2 + "-bit0", int.Parse(result.Equals("1") ? "2" : "1"), result.Equals("0") ? 0 : 1);
                            result = str_data1.Substring(6, 1);
                            session.cd_evt.MCU_TSensorFault = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("MCU_TSensorFault", "0X" + str_data2 + "-bit1", int.Parse(result.Equals("1") ? "2" : "1"), result.Equals("0") ? 0 : 1);
                            result = str_data1.Substring(5, 1);
                            session.cd_evt.MCU_MotTSensorFault = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("MCU_MotTSensorFault", "0X" + str_data2 + "-bit2", int.Parse(result.Equals("1") ? "2" : "1"), result.Equals("0") ? 0 : 1);
                            result = str_data1.Substring(4, 1);
                            session.cd_evt.MCU_CSensorFault = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("MCU_CSensorFault", "0X" + str_data2 + "-bit3", int.Parse(result.Equals("1") ? "3" : "1"), result.Equals("0") ? 0 : 1);
                            result = str_data1.Substring(3, 1);
                            session.cd_evt.MCU_MotMCUCheckSelfFault = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("MCU_MotMCUCheckSelfFault", "0X" + str_data2 + "-bit4", int.Parse(result.Equals("1") ? "3" : "1"), result.Equals("0") ? 0 : 1);
                            result = str_data1.Substring(2, 1);
                            session.cd_evt.MCU_IGBTFault = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("MCU_IGBTFault", "0X" + str_data2 + "-bit5", int.Parse(result.Equals("1") ? "3" : "1"), result.Equals("0") ? 0 : 1);
                            result = str_data1.Substring(1, 1);
                            session.cd_evt.MCU_MotEncoderFault = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("MCU_MotEncoderFault", "0X" + str_data2 + "-bit6", 1, int.Parse(result));
                            result = str_data1.Substring(0, 1);
                            session.cd_evt.MCU_PreChargeFault = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("MCU_PreChargeFault", "0X" + str_data2 + "-bit7", 1, int.Parse(result));
                            #endregion
                            break;
                        case "011A":       /// 电机控制状态及故障信息2
                            num = 1;
                            str_data2 = str_all.Substring(0, 2);
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 8)
                                str_data1 = str_data1.PadLeft(8, '0');
                            result = str_data1.Substring(7, 1);
                            session.cd_evt.PhaseShortCircuit = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("PhaseShortCircuit", "0X" + str_data2 + "-bit0", 1, int.Parse(result));
                            result = str_data1.Substring(6, 1);
                            session.cd_evt.GroundShortCircuit = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("GroundShortCircuit", "0X" + str_data2 + "-bit1", 1, int.Parse(result));
                            result = str_data1.Substring(5, 1);
                            session.cd_evt.MotLackPhase = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("MotLackPhase", "0X" + str_data2 + "-bit2", 1, int.Parse(result));
                            result = str_data1.Substring(4, 1);
                            session.cd_evt.MotOverLoad = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("MotOverLoad", "0X" + str_data2 + "-bit3", 1, int.Parse(result));
                            result = str_data1.Substring(3, 1);
                            session.cd_evt.MotLoseSpeed = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("MotLoseSpeed", "0X" + str_data2 + "-bit4", 1, int.Parse(result));
                            break;
                        case "0127":       /// 电机最大输出转矩2
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.MotMaxTorque = ParsMethod.GetParsWholeByte(str_data1, 0.1,-1000);
                            break;
                        #endregion

                        #region 车况故障充电机0x200-0x27F
                        case "0201":             /// 充电机输出的充电电压
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.ONC_OutputVoltage = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "0202":             ///充电机输出的充电电流 
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.ONC_OutputCurrent = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "0203":             ///充电机输入电压状态
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.ONC_InputVoltageSt = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0204":             ///充电机通信状态
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.CCS_ChargerCommunication = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0205":             ///充电机温度
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.ONC_ONCTemp = ParsMethod.GetParsWholeByte(str_data1, 1, -100);
                            break;
                        case "0206":             ///交流输入状态
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BMS_ChargerACInput = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0208":             ///充电机当前输入电压
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.InChargingVolt = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "0209":             ///充电机当前输入电流
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.InChargingCurr = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "020A":             ///电池放电电流限值
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.HVDischargeLimit = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "020B":             ///电池充电电流限值
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.HVChargeLimit = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "020D":             ///充电接触器状态
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.state_ChargeContactor = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "020E":             ///预充电状态
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.HVPreChrgRdy = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "020F":             ///充电接触器接通请求
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.req_ChargeContactorEn = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0210":             ///充电机使能请求
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.req_Charger_Enable = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0211":             ///充电电压请求
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.req_ChargeVoltage = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0212":             ///充电电流请求
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.req_ChargeCurrent = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0213":             ///输入电流功率因数
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.InputPowerFactor = ParsMethod.GetParsWholeByte(str_data1, 0.01);
                            break;
                        case "0214":             ///充电机效率
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.ChargerEfficiency = ParsMethod.GetParsWholeByte(str_data1, 0.01);
                            break;
                        case "0215":             ///充电机温度
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.ChargerTemp = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                            break;
                        case "0216":             ///当前充电状态
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.state_Charging = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0217":             ///高精度充电机输出的充电电流 
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.ONC_HighPreOutputCurrent = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        #endregion

                        #region DC/DC 0x280-0x2FF
                        case "0281":             /// DCDC状态
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.DCDC_Work_State = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0282":             ///直流母线电压
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.Motor_DCVolt = ParsMethod.GetParsWholeByte(str_data1, 0.5);
                            break;
                        case "0283":             /// 直流母线电流
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.Motor_DCCurrent = ParsMethod.GetParsWholeByte(str_data1, 1, -800);
                            break;
                        case "0284":             ///DCDC温度
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.DCDC_Temperature = ParsMethod.GetParsWholeByte(str_data1, 1, -60);
                            break;
                        case "0285":             /// DC-DC输出电压
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.DCDC_OutputVoltage = ParsMethod.GetParsWholeByte(str_data1, 0.125);
                            break;
                        case "0286":             ///DC-DC输出电流 
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.DCDC_OutputCurrent = ParsMethod.GetParsWholeByte(str_data1, 0.125);
                            break;
                        case "0287":             ///DC-DC使能应答
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.DCDC_EnableResponse = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0288":             ///DC-DC输入电压
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.DCDC_InputVoltage = ParsMethod.GetParsWholeByte(str_data1, 0.016);
                            break;
                        case "0289":             /// DC-DC输入电流
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.DCDC_InputCurrent = ParsMethod.GetParsWholeByte(str_data1, 0.125);
                            break;
                        case "0290":             /// sk DCDC输入电压
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.DCDC_InputVoltage = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0291":             /// sk DCDC输入电流
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.DCDC_InputCurrent = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0292":             /// sk DCDC输出电压
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.DCDC_OutputVoltage = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "0293":             /// sk DCDC输出电流
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.DCDC_OutputCurrent = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0294":             /// sk DCDC效率
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.DCDC_Efficiency = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0295":             /// sk DCDC输入电压
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.DCDC_Temp = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                            break;
                        #endregion


                        #endregion
                        #region 车况故障0X600--0X7FF
                        case "0601":             /// ABS故障
                            num = 1; faultlever = 3;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.ABS_Fault = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("ABS_Fault", "0X" + packageNumber, faultlever, session.cd_evt.ABS_Fault);
                            break;
                        case "0602":          /// 空压机故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.AirCompressor_Fault = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("AirCompressor_Fault", "0X" + packageNumber, faultlever, session.cd_evt.AirCompressor_Fault);
                            break;
                        case "0603":             /// DCDC故障
                            num = 1;
                            str_data2 = str_all.Substring(0, 2);
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 8)
                                str_data1 = str_data1.PadLeft(8, '0');
                            result = str_data1.Substring(7, 1);
                            session.cd_evt.DCDC_InputOvervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("DCDC_InputOvervoltage", "0X" + str_data2 + "-bit0", 3, int.Parse(result));
                            result = str_data1.Substring(6, 1);
                            session.cd_evt.DCDC_InputUndervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("DCDC_InputUndervoltage", "0X" + str_data2 + "-bit1", 3, int.Parse(result));
                            result = str_data1.Substring(5, 1);
                            session.cd_evt.DCDC_OutputShortcircuit = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("DCDC_OutputShortcircuit", "0X" + str_data2 + "-bit2", 3, int.Parse(result));
                            result = str_data1.Substring(4, 1);
                            session.cd_evt.DCDC_OverheatFault = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("DCDC_OverheatFault", "0X" + str_data2 + "-bit3", 3, int.Parse(result));
                            result = str_data1.Substring(3, 1);
                            session.cd_evt.DCDC_OutputUndervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("DCDC_OutputUndervoltage", "0X" + str_data2 + "-bit4", 4, int.Parse(result));
                            result = str_data1.Substring(2, 1);
                            session.cd_evt.DCDC_OutputOvervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("DCDC_OutputOvervoltage", "0X" + str_data2 + "-bit5", 4, int.Parse(result));
                            if (str_data1.Contains("1"))
                            {
                                session.cd_evt.DCDC_Fault = 1;
                            }
                            else
                            {
                                session.cd_evt.DCDC_Fault = 0;
                            }
                            break;
                        case "0604":    /// 助力转向故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.PowerSteering_Fault = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("PowerSteering_Fault", "0X" + packageNumber, faultlever, session.cd_evt.PowerSteering_Fault);
                            break;
                        case "0605":     /// 冷却系统故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.CoolingSystem_Fault = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("CoolingSystem_Fault", "0X" + packageNumber, faultlever, session.cd_evt.CoolingSystem_Fault);
                            break;
                        case "0606":    /// VCU故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.VCU_Fault = ParsMethod.GetParsWholeByte(str_data1);
                            faultint = session.cd_evt.VCU_Fault;
                            if (faultint > 0)
                                canfault += string.Format("${0}:{1}:{2}:{3}", "VCU_Fault", "0X" + packageNumber, faultlever, faultint);
                            break;
                        case "0607":     /// 电池故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BMS_FaultDislpay = ParsMethod.GetParsWholeByte(str_data1);
                            faultint = session.cd_evt.BMS_FaultDislpay;
                            if (faultint > 0)
                                canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_FaultDislpay", "0X" + packageNumber, faultlever, faultint);
                            break;
                        case "0608":    // 充电机故障码
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            //session.cd_evt.ONC_Fault = ParsMethod.GetParsWholeByte(str_data1);
                            str_data2 = regex.Replace(str_data1, "");
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 12)
                                str_data1 = str_data1.PadLeft(12, '0');
                            result = str_data1.Substring(11);
                            session.cd_evt.ONC_OverTempOutputPowerReduceHalf = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_OverTempOutputPowerReduceHalf", "0X" + str_data2 + "-bit0", 2, int.Parse(result));
                            result = str_data1.Substring(10, 1);
                            session.cd_evt.ONC_OverTempOutputClose = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_OverTempOutputClose", "0X" + str_data2 + "-bit1", 2, int.Parse(result));
                            result = str_data1.Substring(9, 1);
                            session.cd_evt.ONC_InputUndervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_InputUndervoltage", "0X" + str_data2 + "-bit2", 2, int.Parse(result));
                            result = str_data1.Substring(8, 1);
                            session.cd_evt.ONC_InputOvervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_InputOvervoltage", "0X" + str_data2 + "-bit3", 2, int.Parse(result));
                            result = str_data1.Substring(7, 1);
                            session.cd_evt.ONC_InnerPFCOvervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_InnerPFCOvervoltage", "0X" + str_data2 + "-bit4", 2, int.Parse(result));
                            result = str_data1.Substring(6, 1);
                            session.cd_evt.ONC_InnerPFCUndervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_InnerPFCUndervoltage", "0X" + str_data2 + "-bit5", 2, int.Parse(result));
                            result = str_data1.Substring(5, 1);
                            session.cd_evt.ONC_OutputOvercurrent = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_OutputOvercurrent", "0X" + str_data2 + "-bit6", 3, int.Parse(result));
                            result = str_data1.Substring(4, 1);
                            session.cd_evt.ONC_OutputShortcircuitOrUndervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_OutputShortcircuitOrUndervoltage", "0X" + str_data2 + "-bit7", 3, int.Parse(result));
                            result = str_data1.Substring(3, 1);
                            session.cd_evt.ONC_InnerMiddleUndervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_InnerMiddleUndervoltage", "0X" + str_data2 + "-bit8", 2, int.Parse(result));
                            result = str_data1.Substring(2, 1);
                            session.cd_evt.ONC_InputUndercurrent = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_InputUndercurrent", "0X" + str_data2 + "-bit9", 2, int.Parse(result));
                            result = str_data1.Substring(1, 1);
                            session.cd_evt.ONC_OutputOvervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_OutputOvervoltage", "0X" + str_data2 + "-bit10", 2, int.Parse(result));
                            result = str_data1.Substring(0, 1);
                            session.cd_evt.ONC_CommunicationTimeout = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_CommunicationTimeout", "0X" + str_data2 + "-bit11", 2, int.Parse(result));
                            if (str_data1.Contains("1"))
                            {
                                session.cd_evt.ONC_Fault = 1;
                            }
                            else
                            {
                                session.cd_evt.ONC_Fault = 0;
                            }
                            break;
                        case "0609":     /// 电机故障类型
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MCU_ElecMachineFault = ParsMethod.GetParsWholeByte(str_data1);
                            faultint = session.cd_evt.MCU_ElecMachineFault;
                            if (faultint == 1)
                            {
                                faultlever = 3;
                            } if (faultint >= 2)
                            {
                                faultlever = 4;
                            }
                            if (faultint > 0)
                                canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_ElecMachineFault", "0X" + packageNumber, faultlever, faultint);
                            break;
                        case "0610":    /// CAN网络故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.CANNetworkFailure = ParsMethod.GetParsWholeByte(str_data1);
                            faultint = session.cd_evt.CANNetworkFailure;
                            if (faultint > 0)
                                canfault += string.Format("${0}:{1}:{2}:{3}", "CANNetworkFailure", "0X" + packageNumber, faultlever, faultint);
                            break;
                        case "0611":     /// 电机控制器电压故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.Motor_ControllersVoltageFault = ParsMethod.GetParsWholeByte(str_data1);
                            faultint = session.cd_evt.Motor_ControllersVoltageFault;
                            if (faultint > 0)
                                canfault += string.Format("${0}:{1}:{2}:{3}", "Motor_ControllersVoltageFault", "0X" + packageNumber, faultlever, faultint);
                            break;
                        case "0612":    /// 电机故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MCU_FaultDInternalInverterMotor = ParsMethod.GetParsWholeByte(str_data1);
                            faultint = session.cd_evt.MCU_FaultDInternalInverterMotor;
                            if (faultint > 0)
                                canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_FaultDInternalInverterMotor", "0X" + packageNumber, faultlever, faultint);
                            break;
                        case "0613":     /// DC/DC内部故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.DCDC_InternalFault = ParsMethod.GetParsWholeByte(str_data1);
                            faultint = session.cd_evt.DCDC_InternalFault;
                            if (faultint > 0)
                                canfault += string.Format("${0}:{1}:{2}:{3}", "DCDC_InternalFault", "0X" + packageNumber, faultlever, faultint);
                            break;
                        case "0614":    /// DC/DC输出短路故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.DCDC_OutputCircuitFault = ParsMethod.GetParsWholeByte(str_data1);
                            faultint = session.cd_evt.DCDC_OutputCircuitFault;
                            if (faultint > 0)
                                canfault += string.Format("${0}:{1}:{2}:{3}", "DCDC_OutputCircuitFault", "0X" + packageNumber, faultlever, faultint);
                            break;
                        case "0615":     /// DCDC故障类型
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.DCDC_ElecMachineFault = ParsMethod.GetParsWholeByte(str_data1);
                            faultint = session.cd_evt.DCDC_ElecMachineFault;
                            if (faultint > 0)
                                canfault += string.Format("${0}:{1}:{2}:{3}", "DCDC_ElecMachineFault", "0X" + packageNumber, faultlever, faultint);
                            break;
                        case "0616":    /// DC/DC输出过压故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.DCDC_OutputOverVoltageFault = ParsMethod.GetParsWholeByte(str_data1);
                            faultint = session.cd_evt.DCDC_OutputOverVoltageFault;
                            if (faultint > 0)
                                canfault += string.Format("${0}:{1}:{2}:{3}", "DCDC_OutputOverVoltageFault", "0X" + packageNumber, faultlever, faultint);
                            break;
                        case "0617":    /// TCU故障码
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.TCU_Fault = ParsMethod.GetParsWholeByte(str_data1);
                            faultint = session.cd_evt.TCU_Fault;
                            if (faultint > 0)
                                canfault += string.Format("${0}:{1}:{2}:{3}", "TCU_Fault", "0X" + packageNumber, faultlever, faultint);
                            break;
                        //case "0618":    /// ABS故障信号
                        //    num = 1;
                        //    str_data1 = str_all.Substring(0, 2);
                        //    session.cd_evt.ABS_Fault = ParsMethod.GetParsWholeByte(str_data1);
                        //    faultint = session.cd_evt.ABS_Fault;
                        //    if (faultint > 0)
                        //        canfault += string.Format("${0}:{1}:{2}:{3}", "ABS_Fault", "0X" + packageNumber, faultlever, faultint);
                        //    break;
                        case "0619":    /// EPS故障报警
                            num = 1; faultlever = 3;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.EPS_FaultWarning = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("EPS_FaultWarning", "0X" + packageNumber, faultlever, session.cd_evt.EPS_FaultWarning);
                            break;
                        case "061A":    ///电机控制器预充故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MotorControllerFault_Precharge = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("MotorControllerFault_Precharge", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.MotorControllerFault_Precharge);
                            break;
                        case "061B":    /// 电机控制器主接故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MotorControllerFault_MainContactor = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("MotorControllerFault_MainContactor", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.MotorControllerFault_MainContactor);
                            break;
                        case "061C":    /// 电机控制器IGBT故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MotorControllerFault_IGBTDamage = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("MotorControllerFault_IGBTDamage", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.MotorControllerFault_IGBTDamage);
                            break;
                        case "061D":    /// 电机控制器过流故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MotorControllerFault_OverCurrent = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("MotorControllerFault_OverCurrent", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.MotorControllerFault_OverCurrent);
                            break;
                        case "061E":    /// 电机控制器过温故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MotorControllerFault_OverTemperature = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("MotorControllerFault_OverTemperature", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.MotorControllerFault_OverTemperature);
                            break;
                        case "061F":    /// 电机控制器过压故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MotorControllerFault_OverVoltage = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("MotorControllerFault_OverVoltage", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.MotorControllerFault_OverVoltage);
                            break;
                        case "0620":    /// 电机控制器欠压故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MotorControllerFault_UnderVoltage = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("MotorControllerFault_UnderVoltage", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.MotorControllerFault_UnderVoltage);
                            break;
                        case "0621":    ///电机控制器堵转故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MotorControllerFault_Blocking = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("MotorControllerFault_Blocking", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.MotorControllerFault_Blocking);
                            break;
                        case "0622":    /// 电机控制器超速故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MotorControllerFault_OverSpeed = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("MotorControllerFault_OverSpeed", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.MotorControllerFault_OverSpeed);
                            break;
                        case "0623":    /// 电机控制器电压上升故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MotorControllerFault_VoltageRise = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("MotorControllerFault_VoltageRise", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.MotorControllerFault_VoltageRise);
                            break;
                        case "0624":    /// 电机控制器电流上升故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MotorControllerFault_CurrentRise = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("MotorControllerFault_CurrentRise", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.MotorControllerFault_CurrentRise);
                            break;
                        case "0625":    /// 电机控制器总线故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MotorControllerFault_Bus = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("MotorControllerFault_Bus", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.MotorControllerFault_Bus);
                            break;
                        case "0626":    /// 自检异常
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.SelfCheckAbnormal = int.Parse(str_data1.Substring(1, 1));
                            canfault += PublicMethods.GetCanFaultStr("SelfCheckAbnormal", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.SelfCheckAbnormal);
                            break;
                        case "0627":    /// 短路/绝缘检测（高压）
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MotorInsulationTest = int.Parse(str_data1.Substring(1, 1));
                            canfault += PublicMethods.GetCanFaultStr("MotorInsulationTest", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.MotorInsulationTest);
                            break;
                        case "0628":    /// 断路/开路（高压）
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MotorBreakerCircuit = int.Parse(str_data1.Substring(1, 1));
                            canfault += PublicMethods.GetCanFaultStr("MotorBreakerCircuit", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.MotorBreakerCircuit);
                            break;
                        case "0629":    /// 驱动电机过载
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.Motor_VoltageOver = int.Parse(str_data1.Substring(1, 1));
                            canfault += PublicMethods.GetCanFaultStr("Motor_VoltageOver", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.Motor_VoltageOver);
                            break;
                        case "062A":
                            num = 1;        //驱动电机控制器温度过高
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.Motor_ControllerTemperatureOver = int.Parse(str_data1.Substring(1, 1));
                            canfault += PublicMethods.GetCanFaultStr("Motor_TemperatureOver", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.Motor_ControllerTemperatureOver);
                            break;
                        case "062B":
                            num = 1;          //驱动电机控制器24V欠压
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.Motor_Controller24VTemperatureLow = int.Parse(str_data1.Substring(1, 1));
                            canfault += PublicMethods.GetCanFaultStr("Motor_Controller24VTemperatureLow", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.Motor_Controller24VTemperatureLow);
                            break;
                        case "062C":
                            num = 1;             //旋变故障
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.Motor_ResolverFault = int.Parse(str_data1.Substring(1, 1));
                            canfault += PublicMethods.GetCanFaultStr("Motor_ResolverFault", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.Motor_ResolverFault);
                            break;
                        case "062D":
                            num = 1;               //驱动电机输出缺相
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.Motor_DriveOutputPhase = int.Parse(str_data1.Substring(1, 1));
                            canfault += PublicMethods.GetCanFaultStr("Motor_DriveOutputPhase", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.Motor_DriveOutputPhase);
                            break;
                        case "062E":
                            num = 1;               //电机故障码
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.Motor_Fault = ParsMethod.GetParsWholeByte(str_data1);
                            #region MyRegion
                            switch (str_data1)
                            {
                                case "41":
                                    faultlever = 3;
                                    break;
                                case "81":
                                    faultlever = 3;
                                    break;
                                case "82":
                                    faultlever = 3;
                                    break;
                                case "83":
                                    faultlever = 3;
                                    break;
                                case "84":
                                    faultlever = 3;
                                    break;
                                case "85":
                                    faultlever = 3;
                                    break;
                                case "86":
                                    faultlever = 3;
                                    break;
                                case "87":
                                    faultlever = 3;
                                    break;
                                case "C1":
                                    faultlever = 2;
                                    break;
                                case "C2":
                                    faultlever = 2;
                                    break;
                                case "C3":
                                    faultlever = 2;
                                    break;
                                case "C4":
                                    faultlever = 2;
                                    break;
                                case "C5":
                                    faultlever = 2;
                                    break;
                                case "C6":
                                    faultlever = 3;
                                    break;
                                case "C7":
                                    faultlever = 3;
                                    break;
                                default:
                                    session.cd_evt.Motor_Fault = 0;
                                    break;
                            }
                            #endregion
                            canfault += PublicMethods.GetCanFaultStr("Motor_Fault", "0X" + str_data1, faultlever, session.cd_evt.Motor_Fault);
                            break;
                        case "062F":
                            num = 1;               //DCDC故障   DCDC_Fault
                            str_data2 = str_all.Substring(0, 2);
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 8)
                                str_data1 = str_data1.PadLeft(8, '0');
                            result = str_data1.Substring(7, 1);
                            session.cd_evt.DCDC_InputOvervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("DCDC_InputOvervoltage", "0X" + str_data2 + "-bit0", 3, int.Parse(result));
                            result = str_data1.Substring(6, 1);
                            session.cd_evt.DCDC_InputUndervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("DCDC_InputUndervoltage", "0X" + str_data2 + "-bit1", 3, int.Parse(result));
                            result = str_data1.Substring(5, 1);
                            session.cd_evt.DCDC_OutputOvervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("DCDC_OutputOvervoltage", "0X" + str_data2 + "-bit2", 3, int.Parse(result));
                            result = str_data1.Substring(4, 1);
                            session.cd_evt.DCDC_OutputUndervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("DCDC_OutputUndervoltage", "0X" + str_data2 + "-bit3", 3, int.Parse(result));
                            result = str_data1.Substring(3, 1);
                            session.cd_evt.DCDC_OutputOvercurrent = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("DCDC_OutputOvercurrent", "0X" + str_data2 + "-bit4", 4, int.Parse(result));
                            result = str_data1.Substring(2, 1);
                            session.cd_evt.DCDC_LowTempAlarm = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("DCDC_LowTempAlarm", "0X" + str_data2 + "-bit5", 2, int.Parse(result));
                            result = str_data1.Substring(1, 1);
                            session.cd_evt.DCDC_HighTempAlarm = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("DCDC_HighTempAlarm", "0X" + str_data2 + "-bit6", 2, int.Parse(result));

                            if (str_data1.Contains("1"))
                            {
                                session.cd_evt.DCDC_Fault = 1;
                            }
                            else
                            {
                                session.cd_evt.DCDC_Fault = 0;
                            }
                            break;
                        case "0630":                    /// DCDC使能请求
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.req_DCDC_Enable = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0631":                    /// 电机转速请求
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.MotorSpeed_req = ParsMethod.GetParsWholeByte(str_data1, 1, -20000);
                            break;
                        case "0632":                    /// 电机转矩请求
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.MotorTq_req = ParsMethod.GetParsWholeByte(str_data1, 0.1, -500);
                            break;
                        case "0633":                    /// DCDC最大允许输出功率
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.DCDCMaxOtptPower = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0640":                    /// 充电机故障信息   
                            num = 1;
                            str_data2 = str_all.Substring(0, 2);
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 8)
                                str_data1 = str_data1.PadLeft(8, '0');
                            result = str_data1.Substring(7, 1);
                            session.cd_evt.ChargerHardware = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ChargerHardware", "0X" + str_data2 + "-bit0", 1, int.Parse(result));
                            result = str_data1.Substring(6, 1);
                            session.cd_evt.ChargerTempAlarm = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ChargerTempAlarm", "0X" + str_data2 + "-bit1", 1, int.Parse(result));
                            result = str_data1.Substring(5, 1);
                            session.cd_evt.ChargerInputUnderVolt = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ChargerInputUnderVolt", "0X" + str_data2 + "-bit2", 1, int.Parse(result));
                            result = str_data1.Substring(4, 1);
                            session.cd_evt.ChargerInputOverVolt = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ChargerInputOverVolt", "0X" + str_data2 + "-bit3", 1, int.Parse(result));
                            result = str_data1.Substring(3, 1);
                            session.cd_evt.ChargerOutputOverCurr = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ChargerOutputOverCurr", "0X" + str_data2 + "-bit4", 1, int.Parse(result));
                            result = str_data1.Substring(2, 1);
                            session.cd_evt.BattConnectFault = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("BattConnectFault", "0X" + str_data2 + "-bit5", 1, int.Parse(result));
                            break;
                        case "0700":
                            num = 1;               //ABS故障信息（Inter型）
                            str_data2 = str_all.Substring(0, 2);
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 8)
                                str_data1 = str_data1.PadLeft(8, '0');
                            result = str_data1.Substring(7, 1);
                            session.cd_evt.EBD_Fault = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("EBD_Fault", "0X" + str_data2 + "-bit0", 1, int.Parse(result));
                            result = str_data1.Substring(6, 1);
                            session.cd_evt.ABS_Fault = int.Parse(result);//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ABS_Fault", "0X" + str_data2 + "-bit1", 1, int.Parse(result));
                            result = str_data1.Substring(5, 1);
                            session.cd_evt.ABS_Sign = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ABS_Sign", "0X" + str_data2 + "-bit2", 1, int.Parse(result));
                            result = str_data1.Substring(4, 1);
                            session.cd_evt.WheelSpeed_Fault = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("WheelSpeed_Fault", "0X" + str_data2 + "-bit3", 1, int.Parse(result));
                            result = str_data1.Substring(3, 1);
                            session.cd_evt.ABS_Function = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ABS_Function", "0X" + str_data2 + "-bit4", 1, int.Parse(result));
                            result = str_data1.Substring(2, 1);
                            session.cd_evt.ABS_FaultStatusLed = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ABS_FaultStatusLed", "0X" + str_data2 + "-bit5", 1, int.Parse(result));
                            result = str_data1.Substring(1, 1);
                            session.cd_evt.EBD_FaultLed = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("EBD_FaultLed", "0X" + str_data2 + "-bit6", 1, int.Parse(result));
                            break;
                        #endregion
                        #endregion
                        #region 电池 0X800-0XFFF
                        #region 电池数据0x800-0XBFF
                        case "0801":                    /// 电池平均温度
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BMS_Temp_Ave = ParsMethod.GetParsWholeByte(str_data1, 0.1, -40);
                            break;
                        case "0802":           /// 电池组当前容量指数 (分辨率1%) 
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryPackCurrentCapacityIndex = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0803":     /// 电池的健康指数 (分辨率1%) 
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryHealthIndex = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        //case "0804":      
                        //    str_data1 = str_all.Substring(0, 2);
                        //    session.cd_evt.ABS_Fault=ParsMethod.GetParsWholeByte(str_data1);
                        //break;
                        case "0805":            /// 续驶里程
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.VCU_CruisingRange = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "0806":             /// 电池包能量
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.BatteryPackEnergy = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "0807":             /// 剩余动力电池电量
                            num = 4;
                            str_data1 = str_all.Substring(0, 11);
                            session.cd_evt.BMS_RemainingBattPower = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                            break;
                        case "0808":             /// BMS当前工作模式
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BMSMode = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0809":             /// BMS休眠请求
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.req_BMSSleep = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0810":             /// 电池加热或冷却请求
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.req_HeatOrCoolBatt = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0900":             /// 电池单体最高电压编号1
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.HVMaxVoltage1_Num = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0901":             /// 电池单体最高电压1
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.HVMaxVoltage1 = ParsMethod.GetParsWholeByte(str_data1, 0.01);
                            break;
                        case "0902":             /// 电池单体最高电压编号2
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.HVMaxVoltage2_Num = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0903":             /// 电池单体最高电压2
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.HVMaxVoltage2 = ParsMethod.GetParsWholeByte(str_data1, 0.01);
                            break;
                        case "0904":             /// 电池单体最高电压编号3
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.HVMaxVoltage3_Num = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0905":             /// 电池单体最高电压3
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.HVMaxVoltage3 = ParsMethod.GetParsWholeByte(str_data1, 0.01);
                            break;
                        case "0906":             /// 电池单体最高电压编号4
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.HVMaxVoltage4_Num = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0907":             /// 电池单体最高电压4
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.HVMaxVoltage4 = ParsMethod.GetParsWholeByte(str_data1, 0.01);
                            break;
                        case "0909":             /// 电池单体最低电压编号1
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.HVMinVoltage1_Num = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "090A":             /// 电池单体最低电压1
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.HVMinVoltage1 = ParsMethod.GetParsWholeByte(str_data1, 0.01);
                            break;
                        case "090B":             /// 电池单体最低电压编号2
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.HVMinVoltage2_Num = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "090C":             /// 电池单体最低电压2
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.HVMinVoltage2 = ParsMethod.GetParsWholeByte(str_data1, 0.01);
                            break;
                        case "090D":             /// 电池单体最低电压编号3
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.HVMinVoltage3_Num = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "090E":             /// 电池单体最低电压3
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.HVMinVoltage3 = ParsMethod.GetParsWholeByte(str_data1, 0.01);
                            break;
                        case "090F":             /// 电池单体最低电压编号4
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.HVMinVoltage4_Num = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0910":             /// 电池单体最低电压4
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.HVMinVoltage4 = ParsMethod.GetParsWholeByte(str_data1, 0.01);
                            break;
                        case "0920":             /// 高精度电池平均温度
                            num = 2;
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.BMS_TempHighPreAve = ParsMethod.GetParsWholeByte(str_data1, 0.1, -40);
                            break;
                        #endregion
                        #region 电池报警0XC00--0XDFF
                        case "0C01":
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            faultint = ParsMethod.GetParsWholeByte(str_data1);
                            session.cd_evt.BatteryPackChargingStatusAlarm = faultint;
                            if (faultint > 0)
                                canfault += string.Format("${0}:{1}:{2}:{3}", "BatteryPackChargingStatusAlarm", "0X" + packageNumber, faultlever, faultint);
                            break;
                        case "0C02":
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryPackDischargeState = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0C03":
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryPackTempState = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0C04":
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryPackElectricLeakageState = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0C05":
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryPackElectricQuantityState = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0C06":
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.OverCurrentState = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "0C07":
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            faultint = ParsMethod.GetParsWholeByte(str_data1);
                            session.cd_evt.PowerBatteryUnderVoltage = faultint;
                            if (faultint > 0)
                                canfault += string.Format("${0}:{1}:{2}:{3}", "PowerBatteryUnderVoltage", "0X" + packageNumber, faultlever, faultint);
                            break;
                        case "0C08":
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            faultint = ParsMethod.GetParsWholeByte(str_data1);
                            session.cd_evt.PowerBatteryPackOverVoltage = faultint;
                            if (faultint > 0)
                                canfault += string.Format("${0}:{1}:{2}:{3}", "PowerBatteryPackOverVoltage", "0X" + packageNumber, faultlever, faultint);
                            break;
                        case "0C09":
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            faultint = ParsMethod.GetParsWholeByte(str_data1);
                            session.cd_evt.PowerBatteryWarningSign = faultint;
                            if (faultint > 0)
                                canfault += string.Format("${0}:{1}:{2}:{3}", "PowerBatteryWarningSign", "0X" + packageNumber, faultlever, faultint);
                            break;
                        case "0C0A":    /// 温度过高报警（电池）
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryFault_OverTemperature = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("BatteryFault_OverTemperature", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BatteryFault_OverTemperature);
                            break;
                        case "0C0B":    /// 充电温度过低（电池）
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryFault_ChargeTemperatureLow = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("BatteryFault_ChargeTemperatureLow", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BatteryFault_ChargeTemperatureLow);
                            break;
                        case "0C0C":    /// 放电温度过低
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryFault_DischargeTemperatureLow = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("BatteryFault_DischargeTemperatureLow", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BatteryFault_DischargeTemperatureLow);
                            break;
                        case "0C0D":    /// 温差过大
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryFault_TemperatureDifferenceLarge = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("BatteryFault_TemperatureDifferenceLarge", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BatteryFault_TemperatureDifferenceLarge);
                            break;
                        case "0C0E":    /// 单体电压过高
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryFault_MonomerVoltageHigh = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("BatteryFault_MonomerVoltageHigh", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BatteryFault_MonomerVoltageHigh);
                            break;
                        case "0C0F":    /// 单体电压过低
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryFault_MonomerVoltageLow = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("BatteryFault_MonomerVoltageLow", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BatteryFault_MonomerVoltageLow);
                            break;
                        case "0C10":    /// 压差过大
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryFault_PressureDifferenceLarge = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("BatteryFault_PressureDifferenceLarge", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BatteryFault_PressureDifferenceLarge);
                            break;
                        case "0C11":    /// SOC过低
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryFault_SOCLow = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("BatteryFault_SOCLow", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BatteryFault_SOCLow);
                            break;
                        case "0C12":    /// 总电压过高
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryFault_TotalVoltageHigh = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("BatteryFault_TotalVoltageHigh", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BatteryFault_TotalVoltageHigh);
                            break;
                        case "0C13":    /// 总电压过低
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryFault_TotalVoltageLow = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("BatteryFault_TotalVoltageLow", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BatteryFault_TotalVoltageLow);
                            break;
                        case "0C14":    /// 充电电流过大
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryFault_ChargingCurrentLarge = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("BatteryFault_ChargingCurrentLarge", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BatteryFault_ChargingCurrentLarge);
                            break;
                        case "0C15":    /// 放电电流过大
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryFault_DischargeCurrentLarge = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("BatteryFault_DischargeCurrentLarge", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BatteryFault_DischargeCurrentLarge);
                            break;
                        case "0C16":    /// 绝缘故障
                            num = 1;
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BatteryFault_Insulation = ParsMethod.GetParsWholeByte(str_data1);
                            canfault += PublicMethods.GetCanFaultStr("BatteryFault_Insulation", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BatteryFault_Insulation);
                            break;
                        case "0C17":
                            num = 1;       //SOC过高（电池）
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.SOCOvertopAlarm = int.Parse(str_data1.Substring(1, 1));
                            canfault += PublicMethods.GetCanFaultStr("SOCOvertopAlarm", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.SOCOvertopAlarm);
                            break;
                        case "0C18":
                            num = 1;       //单体温度过低
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BMS_UnitTemperatureTooLow = int.Parse(str_data1.Substring(1, 1));
                            canfault += PublicMethods.GetCanFaultStr("BMS_UnitTemperatureTooLow", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BMS_UnitTemperatureTooLow);
                            break;
                        case "0C19":
                            num = 1;       //极柱温度过高
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BMS_PoleTemperatureOver = int.Parse(str_data1.Substring(1, 1));
                            canfault += PublicMethods.GetCanFaultStr("BMS_PoleTemperatureOver", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BMS_PoleTemperatureOver);
                            break;
                        case "0C1A":
                            num = 1;       //极柱温度差异过大
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BMS_PoleTemperatureDifferenceOver = int.Parse(str_data1.Substring(1, 1));
                            canfault += PublicMethods.GetCanFaultStr("BMS_PoleTemperatureDifferenceOver", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BMS_PoleTemperatureDifferenceOver);
                            break;
                        case "0C1B":
                            num = 1;       //BMS与充电桩通讯故障
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BMSCharging_CommunicationFault = int.Parse(str_data1.Substring(1, 1));
                            canfault += PublicMethods.GetCanFaultStr("BMSCharging_CommunicationFault", "0X" + packageNumber, ParsMethod.GetFaultlever(str_data1), session.cd_evt.BMSCharging_CommunicationFault);
                            break;
                        case "0C1C":
                            num = 1;       //BMS电池故障码
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.BMS_Fault = ParsMethod.GetParsWholeByte(str_data1);
                            #region switch
                            //BMK 2035-10-21 根据 2035-10-20 才总发送的邮件 <<【东电】安全预警需求>> 添加  52,51,50,4f,4e,4b,4A,48,47,46,21
                            switch (str_data1)
                            {
                                case "00":
                                    faultlever = 0;
                                    break;
                                case "21":
                                    faultlever = 2;
                                    break;
                                case "46":
                                    faultlever = 3;
                                    break;
                                case "47":
                                    faultlever = 3;
                                    break;
                                case "48":
                                    faultlever = 3;
                                    break;
                                case "4A":
                                    faultlever = 3;
                                    break;
                                case "4B":
                                    faultlever = 3;
                                    break;
                                case "4C":
                                    faultlever = 3;
                                    break;
                                case "4D":
                                    faultlever = 3;
                                    break;
                                case "4E":
                                    faultlever = 3;
                                    break;
                                case "4F":
                                    faultlever = 3;
                                    break;
                                case "50":
                                    faultlever = 3;
                                    break;
                                case "51":
                                    faultlever = 3;
                                    break;
                                case "52":
                                    faultlever = 3;
                                    break;
                                case "54":
                                    faultlever = 3;
                                    break;
                                case "56":
                                    faultlever = 3;
                                    break;
                                case "57":
                                    faultlever = 3;
                                    break;
                                case "58":
                                    faultlever = 3;
                                    break;
                                case "59":
                                    faultlever = 3;
                                    break;
                                case "5A":
                                    faultlever = 3;
                                    break;
                                case "5B":
                                    faultlever = 3;
                                    break;
                                case "5C":
                                    faultlever = 3;
                                    break;
                                case "5D":
                                    faultlever = 3;
                                    break;
                                case "63":
                                    faultlever = 4;
                                    break;
                                case "64":
                                    faultlever = 4;
                                    break;
                                case "65":
                                    faultlever = 4;
                                    break;
                                case "66":
                                    faultlever = 4;
                                    break;
                                case "67":
                                    faultlever = 4;
                                    break;
                                case "69":
                                    faultlever = 4;
                                    break;
                                case "6A":
                                    faultlever = 4;
                                    break;
                                case "6B":
                                    faultlever = 4;
                                    break;
                                case "6C":
                                    faultlever = 4;
                                    break;
                                case "6D":
                                    faultlever = 4;
                                    break;
                                case "70":
                                    faultlever = 4;
                                    break;
                                case "71":
                                    faultlever = 4;
                                    break;
                                case "72":
                                    faultlever = 4;
                                    break;
                                case "73":
                                    faultlever = 4;
                                    break;
                                case "74":
                                    faultlever = 4;
                                    break;
                                case "75":
                                    faultlever = 4;
                                    break;
                                case "76":
                                    faultlever = 4;
                                    break;
                                case "81":
                                    faultlever = 4;
                                    break;
                                case "82":
                                    faultlever = 4;
                                    break;
                                default:
                                    session.cd_evt.BMS_Fault = 0;
                                    faultlever = 0;
                                    break;
                            }

                            #endregion
                            canfault += PublicMethods.GetCanFaultStr("BMS_Fault", "0X" + str_data1, faultlever, session.cd_evt.BMS_Fault);
                            break;
                        case "0C30":
                            num = 4;    //电池故障信息
                            #region byte1
                            str_data2 = str_all.Substring(0, 2);
                            faultlever = ParsMethod.GetValuebyBinary(str_data2, 0, 2);
                            faultint = faultlever.Equals("0") ? 0 : 1;
                            session.cd_evt.BMS_BattOverTemp = faultint;
                            canfault += PublicMethods.GetCanFaultStr("BMS_BattOverTemp", "0X" + str_data2, faultlever, faultint);
                            faultlever = ParsMethod.GetValuebyBinary(str_data2, 2, 2);
                            faultint = faultlever.Equals("0") ? 0 : 1;
                            session.cd_evt.BMS_BattInsulation = faultlever;
                            canfault += PublicMethods.GetCanFaultStr("BMS_BattInsulation", "0X" + str_data2, faultlever, faultint);
                            faultlever = ParsMethod.GetValuebyBinary(str_data2, 4, 2);
                            faultint = faultlever.Equals("0") ? 0 : 1;
                            session.cd_evt.BMS_BattOverVolt = faultint;
                            canfault += PublicMethods.GetCanFaultStr("BMS_BattOverVolt", "0X" + str_data2, faultlever, faultint);
                            faultlever = ParsMethod.GetValuebyBinary(str_data2, 6, 2);
                            faultint = faultlever.Equals("0") ? 0 : 1;
                            session.cd_evt.BMS_BattOverCurrent = faultint;
                            canfault += PublicMethods.GetCanFaultStr("BMS_BattOverCurrent", "0X" + str_data2, faultlever, faultint);
                            #endregion
                            #region byte2
                            str_data2 = str_all.Substring(3, 2);
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 8)
                                str_data1 = str_data1.PadLeft(8, '0');
                            result = str_data1.Substring(6, 2);
                            faultlever = PublicMethods.Get2To10(result);
                            faultint = faultlever.Equals("0") ? 0 : 1;
                            session.cd_evt.BMS_BattUnderVolt = faultint;
                            canfault += PublicMethods.GetCanFaultStr("BMS_BattUnderVolt", "0X" + str_data2 + "-bit1", faultlever, faultint);
                            result = str_data1.Substring(5, 1);
                            session.cd_evt.BMS_VMSCommOverTime = result;
                            canfault += PublicMethods.GetCanFaultStr("BMS_VMSCommOverTime", "0X" + str_data2 + "-bit2", int.Parse(result.Equals("1") ? "3" : "1"), result.Equals("0") ? 0 : 1);
                            result = str_data1.Substring(4, 1);
                            session.cd_evt.BMS_LECUCommOverTime = result;
                            canfault += PublicMethods.GetCanFaultStr("BMS_LECUCommOverTime", "0X" + str_data2 + "-bit3", int.Parse(result.Equals("1") ? "3" : "1"), result.Equals("0") ? 0 : 1);
                            result = str_data1.Substring(3, 1);
                            session.cd_evt.BMS_BattPackSmoke = result;
                            canfault += PublicMethods.GetCanFaultStr("BMS_BattPackSmoke", "0X" + str_data2 + "-bit4", int.Parse(result.Equals("1") ? "3" : "1"), result.Equals("0") ? 0 : 1);
                            result = str_data1.Substring(2, 1);
                            session.cd_evt.BMS_ChargerCommOverTime = result;
                            canfault += PublicMethods.GetCanFaultStr("BMS_ChargerCommOverTime", "0X" + str_data2 + "-bit5", int.Parse(result.Equals("1") ? "3" : "1"), result.Equals("0") ? 0 : 1);
                            #endregion
                            #region byte3
                            str_data2 = str_all.Substring(6, 2);
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 8)
                                str_data1 = str_data1.PadLeft(8, '0');
                            result = str_data1.Substring(6, 1);
                            session.cd_evt.BMS_BattCheckSelf = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("BMS_BattCheckSelf", "0X" + str_data2 + "-bit0", 1, int.Parse(result));
                            result = str_data1.Substring(5, 1);
                            session.cd_evt.BMS_BattVoltDiff = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("BMS_BattVoltDiff", "0X" + str_data2 + "-bit1", 1, int.Parse(result));
                            #endregion
                            #region byte4
                            str_data2 = str_all.Substring(9, 2);
                            faultint = ParsMethod.GetValuebyBinary(str_data2, 24, 2, 3);
                            session.cd_evt.BMS_BattPosContactor = faultint;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("BMS_BattPosContactor", "0X" + str_data2 + "-bit1", 1, faultint);
                            faultint = ParsMethod.GetValuebyBinary(str_data2, 26, 2, 3);
                            session.cd_evt.BMS_BattNegContactor = faultint;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("BMS_BattNegContactor", "0X" + str_data2 + "-bit3", 1, faultint);
                            faultint = ParsMethod.GetValuebyBinary(str_data2, 28, 2, 3);
                            session.cd_evt.BMS_PreChargeContactor = faultint;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("BMS_PreChargeContactor", "0X" + str_data2 + "-bit5", 1, faultint);
                            faultint = ParsMethod.GetValuebyBinary(str_data2, 30, 2, 3);
                            session.cd_evt.BMS_interlock = faultint;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("BMS_interlock", "0X" + str_data2 + "-bit7", 1, faultint);
                            #endregion
                            break;
                        #endregion
                        #endregion
                        #region 空调 0X1000-0X1FFF
                        #region 空调数据0x1000-0X1BFF
                        case "1000":
                            num = 1;       //主驾驶设定温度
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.MasterDriveSetTemperature = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "1001":
                            num = 1;       //副驾驶设定温度 
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.AuxiliaryDrivingSetTemperature = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "1002":
                            num = 1;       //压缩机使能请求 
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.req_CMC_Enable = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "1003":
                            num = 2;       //压缩机转速请求 
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.CMCMotSpeed = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        case "1004":
                            num = 2;       //压缩机高压输入 
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.CMCHVVolt = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "1005":
                            num = 2;       //压缩机当前功率 
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.CMCPower = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "1006":
                            num = 2;       //压缩机当前转速 
                            str_data1 = str_all.Substring(0, 5);
                            session.cd_evt.CompSpeed = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                            break;
                        case "1007":
                            num = 1;       //压缩机控制器状态 
                            str_data1 = str_all.Substring(0, 2);
                            session.cd_evt.CMCWork = ParsMethod.GetParsWholeByte(str_data1);
                            break;
                        #endregion
                        #region 空调故障0X1E00--0X1FFF
                        case "1E00":
                            num = 1;               //压缩机故障信息
                            str_data2 = str_all.Substring(0, 2);
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 8)
                                str_data1 = str_data1.PadLeft(8, '0');
                            result = str_data1.Substring(7, 1);
                            session.cd_evt.CPR_CMCSelfCheck = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("CPR_CMCSelfCheck", "0X" + str_data2 + "-bit0", 1, int.Parse(result));
                            result = str_data1.Substring(6, 1);
                            session.cd_evt.CPR_CMCOverVolt = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("CPR_CMCOverVolt", "0X" + str_data2 + "-bit1", 1, int.Parse(result));
                            result = str_data1.Substring(5, 1);
                            session.cd_evt.CPR_CMCUnderVolt = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("CPR_CMCUnderVolt", "0X" + str_data2 + "-bit2", 1, int.Parse(result));
                            result = str_data1.Substring(4, 1);
                            session.cd_evt.CPR_CompOverCurrent = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("CPR_CompOverCurrent", "0X" + str_data2 + "-bit3", 1, int.Parse(result));
                            result = str_data1.Substring(3, 1);
                            session.cd_evt.CPR_CompOverTemp = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("CPR_CompOverTemp", "0X" + str_data2 + "-bit4", 1, int.Parse(result));
                            result = str_data1.Substring(2, 1);
                            session.cd_evt.CPR_CompLocked = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("CPR_CompLocked", "0X" + str_data2 + "-bit5", 1, int.Parse(result));
                            result = str_data1.Substring(1, 1);
                            session.cd_evt.CPR_CompFailurePhase = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("CPR_CompFailurePhase", "0X" + str_data2 + "-bit6", 1, int.Parse(result));
                            result = str_data1.Substring(0, 1);
                            session.cd_evt.CPR_ACSysFault = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("CPR_ACSysFault", "0X" + str_data2 + "-bit7", 1, int.Parse(result));
                            break;
                        #endregion
                        #endregion
                        default:
                            WriteLog.WriteTestLog("ZDY", "Not Found:" + packageNumber + "___Last:" + lastPackageNumber, true);
                            break;
                    }
                    #endregion
                    lastPackageNumber = packageNumber;
                }
                WriteLog.WriteLogMeaning("ZDY", info1, session.cd_evt);
            }
            catch (Exception ex)
            {
                WriteLog.WriteErrorLog("ZDY", "Ex:" + ex.ToString() + "___Last:" + lastPackageNumber, true);
            }
            return session.cd_evt;
        }
    }
}
