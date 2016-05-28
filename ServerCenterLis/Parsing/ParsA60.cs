using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ServerCenterLis
{
    public class ParsA60
    {
        static string zcinfo1;
        static string channelNumber, packageNumber;
        /// 故障值  0正常         /// 故障等级    1	轻微故障,2	一般故障,3	严重故障,4	致命故障
        static int faultlever = 1, faultint = 0;
        public static Cls_RealInformation GetParsbyA60(Telnet_Session session, string info1, int msgnumber, int dataLength, ref string canfault)
        {
            string str_all;//A60
            int data1 = 0, data2 = 0;
            string str_data1, str_data2, str_data3, str_data4, str_data;
            session.cd_a60 = session.getClsCdA60();
            Regex regex = new Regex("\\s+");
            for (int i = 0; i < msgnumber; i++)
            {
                zcinfo1 = info1.Substring(3 * dataLength * i + 18 * i, 3 * dataLength + 17);

                channelNumber = zcinfo1.Substring(0, 2);
                packageNumber = PublicMethods.GetZeroSuppression(zcinfo1.Substring(3, 11));
                dataLength = PublicMethods.Get16To10(zcinfo1.Substring(15, 2));

                //因前解析把 00 f0(2字节canid) 也带上了 
                str_all = "XX XX ";
                str_all += zcinfo1.Substring(18, 23);

                #region A60
                switch (packageNumber)
                {
                    //VCU点火开关及故障码
                    //string VCU_Keyposition;//点火开关
                    //string VCU_Fault;//故障码
                    case "000000F0":
                        str_data1 = str_all.Substring(6, 1);
                        data1 = Convert.ToInt32(str_data1, 16);
                        str_data1 = Convert.ToString(data1, 2);
                        if (str_data1.Length < 4)
                            str_data1 = str_data1.PadLeft(4, '0');
                        str_data1 = str_data1.Substring(0, 2);
                        switch (str_data1)
                        {
                            case "00":
                                session.cd_a60.VCU_Keyposition = "0";
                                // session.isacc = "0";
                                break;
                            case "01":
                                session.cd_a60.VCU_Keyposition = "1";
                                //session.isacc = "1";
                                break;
                            case "10":
                                session.cd_a60.VCU_Keyposition = "2";
                                //session.isacc = "1";
                                break;
                            case "11":
                                session.cd_a60.VCU_Keyposition = "3";
                                //session.isacc = "1";
                                break;
                            default:
                                break;
                        }


                        str_data2 = str_all.Substring(21, 2);
                        #region 故障分级后丢弃
                        //data1 = Convert.ToInt32(str_data2, 16);
                        //str_data2 = Convert.ToString(data1, 2);
                        //if (str_data2.Length < 8)
                        //    str_data2 = str_data2.PadLeft(8, '0');
                        //str_data1 = str_data2.Substring(0, 3);
                        //switch (str_data1)
                        //{
                        //    case "000":
                        //        session.cd_a60.VCU_Fault = 0;
                        //        faultlever = 0;
                        //        break;
                        //    case "001":
                        //        session.cd_a60.VCU_Fault = 1;
                        //        faultlever = 1;
                        //        break;
                        //    case "010":
                        //        session.cd_a60.VCU_Fault = 2;
                        //        faultlever = 2;
                        //        break;
                        //    case "011":
                        //        session.cd_a60.VCU_Fault = 3;
                        //        faultlever = 3;
                        //        break;
                        //    case "100":
                        //        session.cd_a60.VCU_Fault = 4;
                        //        faultlever = 4;
                        //        break;
                        //    default:
                        //        break;
                        //}
                        #endregion
                        #region 故障分级
                        switch (str_data2)
                        {
                            case "9B":
                                faultlever = 4;
                                break;
                            case "7A":
                                faultlever = 3;
                                break;
                            case "79":
                                faultlever = 3;
                                break;
                            case "78":
                                faultlever = 3;
                                break;
                            case "77":
                                faultlever = 3;
                                break;
                            case "76":
                                faultlever = 3;
                                break;
                            case "75":
                                faultlever = 2;
                                break;
                            case "74":
                                faultlever = 2;
                                break;
                            case "73":
                                faultlever = 3;
                                break;
                            case "4E":
                                faultlever = 2;
                                break;
                            case "4C":
                                faultlever = 2;
                                break;
                            case "49":
                                faultlever = 2;
                                break;
                            case "47":
                                faultlever = 2;
                                break;
                            case "21":
                                faultlever = 2;
                                break;
                            case "22":
                                faultlever = 2;
                                break;
                            case "23":
                                faultlever = 2;
                                break;
                            case "24":
                                faultlever = 2;
                                break;
                            case "25":
                                faultlever = 2;
                                break;
                        }
                        #endregion
                        session.cd_a60.VCU_Fault = ParsMethod.GetParsWholeByte(str_data2);
                        faultint = session.cd_a60.VCU_Fault;
                        if (faultint > 0)
                            canfault += string.Format("$VCU_Fault:{0}:{1}:{2}", "0X" + str_data2, faultlever, faultint);
                        break;
                    //double BMS_SOC;//电池SOC
                    //double BMS_Voltage;//电池总电压
                    //double BMS_Current;//电池充放电总电流
                    case "00000125":
                        //电池SOC
                        str_data2 = str_all.Substring(6, 4);
                        str_data1 = regex.Replace(str_data2, "");
                        data2 = Convert.ToInt32(str_data1, 16);
                        str_data3 = Convert.ToString(data2, 2);
                        if (str_data3.Length < 12)
                            str_data3 = str_data3.PadLeft(12, '0');
                        str_data = str_data3.Substring(0, 10);
                        session.cd_a60.BMS_SOC = Convert.ToInt32(str_data, 2) * 0.1;
                        //TODO Filter 范围0-100
                        session.cd_a60.BMS_SOC = session.cd_a60.BMS_SOC < 0 ? 0 : session.cd_a60.BMS_SOC;

                        //电池总电压
                        str_data2 = str_all.Substring(15, 4);
                        str_data2 = regex.Replace(str_data2, "");
                        data2 = Convert.ToInt32(str_data2, 16);
                        str_data2 = Convert.ToString(data2, 2);
                        if (str_data2.Length < 12)
                            str_data2 = str_data2.PadLeft(12, '0');
                        str_data2 = str_data2.Substring(0, 9);
                        session.cd_a60.BMS_Voltage = Convert.ToInt32(str_data2, 2);
                        //TODO Filter 范围0-500
                        session.cd_a60.BMS_Voltage = session.cd_a60.BMS_Voltage < 0 ? 0 : session.cd_a60.BMS_Voltage;

                        //电池充放电总电流
                        str_data3 = str_all.Substring(19, 4);
                        str_data3 = regex.Replace(str_data3, "");
                        session.cd_a60.BMS_Current = Convert.ToInt32(str_data3, 16) - 600;
                        break;


                    //漏电检测//BMS_CreepageMonitor============增加
                    case "00000127":
                        str_data = str_all.Substring(25, 4);
                        str_data = regex.Replace(str_data, "");
                        session.cd_a60.BMS_CreepageMonitor = Convert.ToInt32(str_data, 16);
                        //TODO Filter 范围0-2000
                        session.cd_a60.BMS_CreepageMonitor = session.cd_a60.BMS_CreepageMonitor < 0 ? 0 : session.cd_a60.BMS_CreepageMonitor;

                        break;

                    //double BMS_SOCCalculate;//电池SOC(处理后)
                    //string BMS_OutsideChargeSignal;//外部充电信号(动力电池充电指示)
                    //string BMS_FaultDislpay;//电池故障(显示用)
                    //int 信号 BMS_OFCConnectSignal 非车载充电连接指示信号
                    //double BMS_Temperature;//电池温度
                    //double BMS_Temp_Max;//电池最高温度
                    //double BMS_Temp_Min;//电池最低问题
                    case "00000358":
                        str_data1 = str_all.Substring(6, 2);
                        session.cd_a60.BMS_SOCCalculate = Convert.ToInt32(str_data1, 16);
                        //TODO Filter 范围0-100
                        session.cd_a60.BMS_SOCCalculate = session.cd_a60.BMS_SOCCalculate < 0 ? 0 : session.cd_a60.BMS_SOCCalculate;

                        str_data2 = str_all.Substring(9, 1);
                        data2 = Convert.ToInt32(str_data2, 16);
                        str_data2 = Convert.ToString(data2, 2);
                        if (str_data2.Length < 4)
                            str_data2 = str_data2.PadLeft(4, '0');
                        //外部充电信号
                        str_data3 = str_data2.Substring(0, 1);
                        if (str_data3 == "0")
                            session.cd_a60.BMS_OutsideChargeSignal = "0";
                        else
                            session.cd_a60.BMS_OutsideChargeSignal = "1";
                        //电池故障
                        str_data4 = str_data2.Substring(1, 1);
                        if (str_data4 == "1")
                        {
                            session.cd_a60.BMS_FaultDislpay = "1";
                            canfault += string.Format("BMS_FaultDislpay:{0}:{1}:", "0X358-0X01", session.cd_a60.BMS_FaultDislpay);
                        }
                        else
                            session.cd_a60.BMS_FaultDislpay = "0";

                        //非车载充电连接指示信号
                        str_data4 = str_data2.Substring(2, 1);
                        if (str_data4 == "0")
                            session.cd_a60.BMS_OFCConnectSignal = "0";
                        else
                            session.cd_a60.BMS_OFCConnectSignal = "1";
                        //电池温度
                        str_data1 = str_all.Substring(18, 2);
                        session.cd_a60.BMS_Temperature = Convert.ToInt32(str_data1, 16) - 40;
                        //电池最高温度
                        str_data2 = str_all.Substring(24, 2);
                        session.cd_a60.BMS_Temp_Max = Convert.ToInt32(str_data2, 16) - 40;
                        //电池最低温度
                        str_data3 = str_all.Substring(27, 2);
                        session.cd_a60.BMS_Temp_Min = Convert.ToInt32(str_data3, 16) - 40;
                        break;
                    //string BMS_Fault = "";//BMS故障码//============增加
                    //double BMS_MaxCellBatt = 0;//最高单体电压//============增加
                    //double BMS_MinCellBatt = 0;//最低单体电压//============增加
                    //int BMS_MaxCellBattNumber = 0;//最高单体电压电池编号//============增加
                    //int BMS_MinCellBattNumber = 0;//最低单体电压电池编号//============增加
                    //case "03 59":
                    case "00000359":
                        //BMS故障码
                        str_data = str_all.Substring(6, 2);
                        #region switch
                        switch (str_data)
                        {
                            case "00":
                                session.cd_a60.BMS_Fault = 0;
                                faultlever = 0;
                                break;
                            case "21":
                                session.cd_a60.BMS_Fault = 33;
                                faultlever = 2;
                                break;
                            case "46":
                                session.cd_a60.BMS_Fault = 70;
                                faultlever = 3;
                                break;
                            case "47":
                                session.cd_a60.BMS_Fault = 71;
                                faultlever = 3;
                                break;
                            case "48":
                                session.cd_a60.BMS_Fault = 72;
                                faultlever = 3;
                                break;
                            case "4A":
                                session.cd_a60.BMS_Fault = 74;
                                faultlever = 3;
                                break;
                            case "4B":
                                session.cd_a60.BMS_Fault = 75;
                                faultlever = 3;
                                break;
                            case "4E":
                                session.cd_a60.BMS_Fault = 78;
                                faultlever = 3;
                                break;
                            case "4F":
                                session.cd_a60.BMS_Fault = 79;
                                faultlever = 3;
                                break;
                            case "50":
                                session.cd_a60.BMS_Fault = 80;
                                faultlever = 3;
                                break;
                            case "51":
                                session.cd_a60.BMS_Fault = 81;
                                faultlever = 3;
                                break;
                            case "52":
                                session.cd_a60.BMS_Fault = 82;
                                faultlever = 3;
                                break;
                            case "54":
                                session.cd_a60.BMS_Fault = 84;
                                faultlever = 3;
                                break;
                            case "56":
                                session.cd_a60.BMS_Fault = 86;
                                faultlever = 3;
                                break;
                            case "57":
                                session.cd_a60.BMS_Fault = 87;
                                faultlever = 3;
                                break;
                            case "58":
                                session.cd_a60.BMS_Fault = 88;
                                faultlever = 3;
                                break;
                            case "59":
                                session.cd_a60.BMS_Fault = 89;
                                faultlever = 3;
                                break;
                            case "5A":
                                session.cd_a60.BMS_Fault = 90;
                                faultlever = 3;
                                break;
                            case "5B":
                                session.cd_a60.BMS_Fault = 91;
                                faultlever = 3;
                                break;
                            case "5C":
                                session.cd_a60.BMS_Fault = 92;
                                faultlever = 3;
                                break;
                            case "5D":
                                session.cd_a60.BMS_Fault = 93;
                                faultlever = 3;
                                break;
                            case "63":
                                session.cd_a60.BMS_Fault = 99;
                                faultlever = 4;
                                break;
                            case "64":
                                session.cd_a60.BMS_Fault = 100;
                                faultlever = 4;
                                break;
                            case "65":
                                session.cd_a60.BMS_Fault = 101;
                                faultlever = 4;
                                break;
                            case "66":
                                session.cd_a60.BMS_Fault = 102;
                                faultlever = 4;
                                break;
                            case "67":
                                session.cd_a60.BMS_Fault = 103;
                                faultlever = 4;
                                break;
                            case "69":
                                session.cd_a60.BMS_Fault = 105;
                                faultlever = 4;
                                break;
                            case "6A":
                                session.cd_a60.BMS_Fault = 106;
                                faultlever = 4;
                                break;
                            case "6B":
                                session.cd_a60.BMS_Fault = 107;
                                faultlever = 4;
                                break;
                            case "6C":
                                session.cd_a60.BMS_Fault = 108;
                                faultlever = 4;
                                break;
                            case "6D":
                                session.cd_a60.BMS_Fault = 109;
                                faultlever = 4;
                                break;
                            case "70":
                                session.cd_a60.BMS_Fault = 112;
                                faultlever = 4;
                                break;
                            case "71":
                                session.cd_a60.BMS_Fault = 113;
                                faultlever = 4;
                                break;
                            case "72":
                                session.cd_a60.BMS_Fault = 114;
                                faultlever = 4;
                                break;
                            case "73":
                                session.cd_a60.BMS_Fault = 115;
                                faultlever = 4;
                                break;
                            default:
                                session.cd_a60.BMS_Fault = 0;
                                break;
                        }

                        #endregion
                        faultint = session.cd_a60.BMS_Fault;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_Fault", "0X359-0X" + str_data, faultlever, faultint);


                        //最高单体电压
                        str_data1 = str_all.Substring(9, 2);
                        session.cd_a60.BMS_MaxCellBatt = Math.Round(Convert.ToInt32(str_data1, 16) * 0.02d, 2);
                        //最高单体电压
                        str_data2 = str_all.Substring(12, 2);
                        session.cd_a60.BMS_MinCellBatt = Math.Round(Convert.ToInt32(str_data2, 16) * 0.02d, 2);
                        //最高单体电压电池编号
                        str_data3 = str_all.Substring(15, 2);
                        session.cd_a60.BMS_MaxCellBattNumber = Convert.ToInt32(str_data3, 16);
                        //最高单体电压电池编号
                        str_data4 = str_all.Substring(18, 2);
                        session.cd_a60.BMS_MinCellBattNumber = Convert.ToInt32(str_data4, 16);
                        break;

                    //string BMS_SlowChargeSt;//充电状态(慢充)
                    //string BMS_FastChargeSt;//充电状态(快充)
                    case "00000361":
                        str_data1 = str_all.Substring(15, 1);
                        data1 = Convert.ToInt32(str_data1, 16);
                        str_data1 = Convert.ToString(data1, 2);
                        if (str_data1.Length < 4)
                            str_data1 = str_data1.PadLeft(4, '0');
                        str_data1 = str_data1.Substring(1, 2);
                        switch (str_data1)
                        {
                            case "00":
                                session.cd_a60.BMS_SlowChargeSt = "0";
                                break;
                            case "01":
                                session.cd_a60.BMS_SlowChargeSt = "1";
                                break;
                            case "10":
                                session.cd_a60.BMS_SlowChargeSt = "2";
                                break;
                            case "11":
                                session.cd_a60.BMS_SlowChargeSt = "3";
                                break;
                            default:
                                break;
                        }
                        //充电状态(快充)
                        str_data1 = str_all.Substring(24, 1);
                        data1 = Convert.ToInt32(str_data1, 16);
                        str_data1 = Convert.ToString(data1, 2);
                        if (str_data1.Length < 4)
                            str_data1 = str_data1.PadLeft(4, '0');
                        str_data1 = str_data1.Substring(0, 2);
                        switch (str_data1)
                        {
                            case "00":
                                session.cd_a60.BMS_FastChargeSt = "0";
                                break;
                            case "01":
                                session.cd_a60.BMS_FastChargeSt = "1";
                                break;
                            case "10":
                                session.cd_a60.BMS_FastChargeSt = "2";
                                break;
                            case "11":
                                session.cd_a60.BMS_FastChargeSt = "3";
                                break;
                            default:
                                break;
                        }
                        break;

                    //string ONC_Fault = "";//充电机故障码============增加
                    case "000003A2":
                        //充电机故障码
                        session.cd_a60.ONC_Fault = "";
                        str_data1 = str_all.Substring(24, 4);
                        str_data2 = regex.Replace(str_data1, "");
                        str_data1 = Convert.ToString(Convert.ToInt32(str_data2, 16), 2);
                        if (str_data1.Length < 12)
                            str_data1 = str_data1.PadLeft(12, '0');
                        //for (int j = 0; j < str_data4.Length; j++)
                        //{
                        //    if (str_data4[j] == '1')
                        //        //   session.cd_a60.ONC_Fault += oncfault[j] + ",";
                        //        session.cd_a60.ONC_Fault += j + ",";
                        //}

                        //if (!string.IsNullOrEmpty(session.cd_a60.ONC_Fault))
                        //{
                        //    session.cd_a60.ONC_Fault = session.cd_a60.ONC_Fault.Substring(0, session.cd_a60.ONC_Fault.Length - 1);
                        //    foreach (var item in session.cd_a60.ONC_Fault.Split(','))
                        //    {
                        //        if (int.Parse(item) == 4 || int.Parse(item) == 5)
                        //        {
                        //            faultlever = 3;
                        //        }
                        //        else
                        //        {
                        //            faultlever = 2;
                        //        }
                        //        if (!string.IsNullOrEmpty(item))
                        //        {
                        //            canfault += "$ONC_Fault:0X3A2-bit" + (11 - int.Parse(item)) + ":" + faultlever + ":" + item;
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    session.cd_a60.ONC_Fault = "0";
                        //}
                        #region 标志位分离
                        string result = str_data1.Substring(11); //无故障时 不添加running表
                        session.cd_a60.ONC_OverTempOutputPowerReduceHalf = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("ONC_OverTempOutputPowerReduceHalf", "0X" + str_data2 + "-bit0", 2, int.Parse(result));
                        result = str_data1.Substring(10, 1);
                        session.cd_a60.ONC_OverTempOutputClose = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("ONC_OverTempOutputClose", "0X" + str_data2 + "-bit1", 2, int.Parse(result));
                        result = str_data1.Substring(9, 1);
                        session.cd_a60.ONC_InputUndervoltage = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("ONC_InputUndervoltage", "0X" + str_data2 + "-bit2", 2, int.Parse(result));
                        result = str_data1.Substring(8, 1);
                        session.cd_a60.ONC_InputOvervoltage = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("ONC_InputOvervoltage", "0X" + str_data2 + "-bit3", 2, int.Parse(result));
                        result = str_data1.Substring(7, 1);
                        session.cd_a60.ONC_InnerPFCOvervoltage = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("ONC_InnerPFCOvervoltage", "0X" + str_data2 + "-bit4", 2, int.Parse(result));
                        result = str_data1.Substring(6, 1);
                        session.cd_a60.ONC_InnerPFCUndervoltage = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("ONC_InnerPFCUndervoltage", "0X" + str_data2 + "-bit5", 2, int.Parse(result));
                        result = str_data1.Substring(5, 1);
                        session.cd_a60.ONC_OutputOvercurrent = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("ONC_OutputOvercurrent", "0X" + str_data2 + "-bit6", 3, int.Parse(result));
                        result = str_data1.Substring(4, 1);
                        session.cd_a60.ONC_OutputShortcircuitOrUndervoltage = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("ONC_OutputShortcircuitOrUndervoltage", "0X" + str_data2 + "-bit7", 3, int.Parse(result));
                        result = str_data1.Substring(3, 1);
                        session.cd_a60.ONC_InnerMiddleUndervoltage = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("ONC_InnerMiddleUndervoltage", "0X" + str_data2 + "-bit8", 2, int.Parse(result));
                        result = str_data1.Substring(2, 1);
                        session.cd_a60.ONC_InputUndercurrent = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("ONC_InputUndercurrent", "0X" + str_data2 + "-bit9", 2, int.Parse(result));
                        result = str_data1.Substring(1, 1);
                        session.cd_a60.ONC_OutputOvervoltage = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("ONC_OutputOvervoltage", "0X" + str_data2 + "-bit10", 2, int.Parse(result));
                        result = str_data1.Substring(0, 1);
                        session.cd_a60.ONC_CommunicationTimeout = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("ONC_CommunicationTimeout", "0X" + str_data2 + "-bit11", 2, int.Parse(result));
                        if (str_data1.Contains("1"))
                        {
                            session.cd_a60.ONC_Fault = "1";
                        }
                        else
                        {
                            session.cd_a60.ONC_Fault = "0";
                        }
                        #endregion
                        break;
                    //int MCU_ElecPowerTrainMngtState 电机控制器工作状态
                    //double     Motor_Torqueestimation 电机实际输出扭矩
                    //int MCU_ElecMachineFault 电机故障类型
                    //double MCU_InternalMachineTemp 电机转子温度
                    case "00000201":
                        //电机控制器工作状态   取低四位
                        str_data1 = str_all.Substring(13, 1);
                        str_data2 = Convert.ToString(Convert.ToInt32(str_data1, 16), 2);
                        if (str_data2.Length < 4)
                            str_data2 = str_data2.PadLeft(4, '0');
                        str_data1 = str_data2.Substring(1, 3);
                        switch (str_data1)
                        {
                            case "000":
                                session.cd_a60.MCU_ElecPowerTrainMngtState = "0";
                                break;
                            case "010":
                                session.cd_a60.MCU_ElecPowerTrainMngtState = "2";
                                break;
                            case "100":
                                session.cd_a60.MCU_ElecPowerTrainMngtState = "4";
                                break;
                            default:
                                break;
                        }

                        //电机实际输出扭矩
                        str_data2 = str_all.Substring(15, 4);
                        str_data2 = regex.Replace(str_data2, "");
                        data2 = Convert.ToInt32(str_data2, 16);
                        str_data2 = Convert.ToString(data2, 2);
                        if (str_data2.Length < 12)
                            str_data2 = str_data2.PadLeft(12, '0');
                        str_data2 = str_data2.Substring(0, 9);

                        //TODO Filter 范围-254-256
                        session.cd_a60.Motor_Torqueestimation = Convert.ToInt32(str_data2, 2);
                        session.cd_a60.Motor_Torqueestimation = session.cd_a60.Motor_Torqueestimation < 0 ? 0 : session.cd_a60.Motor_Torqueestimation;
                        session.cd_a60.Motor_Torqueestimation = session.cd_a60.Motor_Torqueestimation - 254;

                        //电机故障类型
                        str_data1 = str_all.Substring(19, 1);
                        str_data2 = Convert.ToString(Convert.ToInt32(str_data1, 16), 2);
                        if (str_data2.Length < 4)
                            str_data2 = str_data2.PadLeft(4, '0');
                        str_data1 = str_data2.Substring(0, 2);
                        switch (str_data1)
                        {
                            case "00":
                                session.cd_a60.MCU_ElecMachineFault = "0";
                                break;
                            case "01":
                                session.cd_a60.MCU_ElecMachineFault = "1";
                                canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_ElecMachineFault", "0X359-0X1", 3, "1");
                                break;
                            case "10":
                                session.cd_a60.MCU_ElecMachineFault = "2";
                                canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_ElecMachineFault", "0X359-0X2", 4, "2");
                                break;
                            case "11":
                                session.cd_a60.MCU_ElecMachineFault = "3";
                                canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_ElecMachineFault", "0X359-0X3", 4, "3");
                                break;
                            default:
                                break;
                        }
                        //电机转子温度
                        str_data1 = str_all.Substring(21, 2);
                        data1 = Convert.ToInt32(str_data1, 16);
                        //TODO Filter 范围-40-214
                        session.cd_a60.MCU_InternalMachineTemp = data1 - 40;
                        break;
                    //MCU_MaxMotorTorque 电机最大电动扭矩
                    //   Motor_MaxGenTorque 电机最大发电扭矩
                    //Motor_Revolution 电机转速
                    //MCU_ActiveDischarge 电机控制器母线电容放电状态
                    case "00000202":
                        //电机最大电动扭矩
                        str_data1 = str_all.Substring(6, 2);
                        session.cd_a60.MCU_MaxMotorTorque = Convert.ToInt32(str_data1, 16);
                        //TODO Filter 范围0-254
                        session.cd_a60.MCU_MaxMotorTorque = session.cd_a60.MCU_MaxMotorTorque < 0 ? 0 : session.cd_a60.MCU_MaxMotorTorque;

                        //电机最大发电扭矩
                        str_data1 = str_all.Substring(9, 2);
                        session.cd_a60.Motor_MaxGenTorque = Convert.ToInt32(str_data1, 16);
                        //TODO Filter 范围0-254
                        session.cd_a60.Motor_MaxGenTorque = session.cd_a60.Motor_MaxGenTorque < 0 ? 0 : session.cd_a60.Motor_MaxGenTorque;

                        //电机转速
                        str_data1 = str_all.Substring(12, 4);
                        str_data1 = regex.Replace(str_data1, "");
                        data2 = Convert.ToInt32(str_data1, 16);
                        str_data2 = Convert.ToString(data2, 2);
                        if (str_data2.Length < 12)
                            str_data2 = str_data2.PadLeft(12, '0');
                        session.cd_a60.Motor_Revolution = Convert.ToInt32(str_data2, 2) * 10 - 20000;


                        //电机控制器母线电容放电状态
                        str_data1 = str_all.Substring(19, 1);
                        str_data2 = Convert.ToString(Convert.ToInt32(str_data1, 16), 2);
                        if (str_data2.Length < 4)
                            str_data2 = str_data2.PadLeft(4, '0');
                        str_data1 = str_data2.Substring(1, 1);
                        switch (str_data1)
                        {
                            case "0":
                                session.cd_a60.MCU_ActiveDischarge = "0";
                                break;
                            case "1":
                                session.cd_a60.MCU_ActiveDischarge = "1";
                                break;
                            default:
                                break;
                        }

                        break;

                    //Motor_DCCurrent 直流母线电压
                    //Motor_ControllerTemp 电机控制器温度
                    //Motor_Temperature 电机温度
                    case "00000204":
                        //直流母线电压
                        str_data2 = str_all.Substring(6, 4);
                        str_data2 = regex.Replace(str_data2, "");
                        data2 = Convert.ToInt32(str_data2, 16);
                        str_data2 = Convert.ToString(data2, 2);
                        if (str_data2.Length < 12)
                            str_data2 = str_data2.PadLeft(12, '0');
                        str_data2 = str_data2.Substring(0, 10);

                        //TODO Filter 范围0-511
                        session.cd_a60.Motor_DCCurrent = Convert.ToInt32(str_data2, 2) * 0.5;

                        //电机控制器温度
                        str_data2 = str_all.Substring(9, 5);
                        str_data2 = regex.Replace(str_data2, "");
                        data2 = Convert.ToInt32(str_data2, 16);
                        str_data2 = Convert.ToString(data2, 2);
                        if (str_data2.Length < 16)
                            str_data2 = str_data2.PadLeft(16, '0');
                        str_data2 = str_data2.Substring(2, 13);
                        //TODO Filter 范围0-100
                        session.cd_a60.Motor_ControllerTemp = Convert.ToInt32(str_data2, 2) * 0.02;

                        //电机温度
                        str_data1 = str_all.Substring(13, 6);
                        str_data1 = regex.Replace(str_data1, "");
                        str_data2 = Convert.ToString(Convert.ToInt32(str_data1, 16), 2);
                        if (str_data2.Length < 16)
                            str_data2 = str_data2.PadLeft(16, '0');
                        str_data1 = str_data2.Substring(3, 13);
                        //TODO Filter 范围0-100
                        session.cd_a60.Motor_Temperature = Convert.ToInt32(str_data1, 2) * 0.02;
                        break;

                    //double IC_TotalOdmeter;//总里程
                    //double IC_Odmeter;//小计里程
                    //double VCU_CruisingRange 剩余行驶里程
                    //VCU_BrakeEnergyReturnIntension 电机状态及强度信度
                    case "00000485":
                        //总里程
                        str_data1 = str_all.Substring(6, 8);
                        str_data1 = regex.Replace(str_data1, "");
                        session.cd_a60.IC_TotalOdmeter = Convert.ToInt32(str_data1, 16) * 0.1;
                        //TODO Filter 范围0-999999.9
                        session.cd_a60.IC_TotalOdmeter = session.cd_a60.IC_TotalOdmeter < 0 ? 0 : session.cd_a60.IC_TotalOdmeter;
                        session.cd_a60.IC_TotalOdmeter = Math.Round(session.cd_a60.IC_TotalOdmeter, 1);

                        //小计里程
                        str_data2 = str_all.Substring(15, 5);
                        str_data2 = regex.Replace(str_data2, "");
                        session.cd_a60.IC_Odmeter = Convert.ToInt32(str_data2, 16) * 0.1;
                        //TODO Filter 范围0-999.9
                        session.cd_a60.IC_Odmeter = session.cd_a60.IC_Odmeter < 0 ? 0 : session.cd_a60.IC_Odmeter;//小于0 取0
                        session.cd_a60.IC_Odmeter = Math.Round(session.cd_a60.IC_Odmeter, 1);//取一位小数

                        //剩余行驶里程
                        str_data2 = str_all.Substring(21, 4);
                        str_data2 = regex.Replace(str_data2, "");
                        data2 = Convert.ToInt32(str_data2, 16);
                        str_data2 = Convert.ToString(data2, 2);
                        if (str_data2.Length < 12)
                            str_data2 = str_data2.PadLeft(12, '0');
                        str_data2 = str_data2.Substring(0, 10);
                        session.cd_a60.VCU_CruisingRange = Convert.ToInt32(str_data2, 2);
                        //TODO Filter 范围0-1000
                        session.cd_a60.VCU_CruisingRange = session.cd_a60.VCU_CruisingRange < 0 ? 0 : session.cd_a60.VCU_CruisingRange;

                        //电机状态及强度信度
                        str_data2 = str_all.Substring(27, 2);
                        //TODO Filter 范围-50-150
                        data1 = Convert.ToInt32(str_data2, 16) - 50;
                        if (data1 > 0)
                            session.cd_a60.VCU_BrakeEnergy = "1";//2035-10-30修改
                        else if (data1 == 0)
                            session.cd_a60.VCU_BrakeEnergy = "0";
                        else
                            session.cd_a60.VCU_BrakeEnergy = "2";
                        break;

                    default:
                        break;
                }
                #endregion
            }
            //充电状态   
            //充电时 根据快慢冲 不同字段 显示 正在充电
            //非充电   两个值都显示 非充电状态

            if (!string.IsNullOrEmpty(session.cd_a60.BMS_SlowChargeSt) && !string.IsNullOrEmpty(session.cd_a60.BMS_FastChargeSt) && !string.IsNullOrEmpty(session.cd_a60.BMS_ChargeFS.ToString()))
            {
                //快慢充
                if (session.cd_a60.BMS_SlowChargeSt.Equals("1") || session.cd_a60.BMS_FastChargeSt.Equals("1"))
                {
                    session.cd_a60.BMS_ChargeSt = "1";
                    if (session.cd_a60.BMS_SlowChargeSt.Equals("1"))
                    {
                        //慢充
                        session.cd_a60.BMS_ChargeFS = 2;
                    }
                    if (session.cd_a60.BMS_FastChargeSt.Equals("1"))
                    {
                        //快充
                        session.cd_a60.BMS_ChargeFS = 1;
                    }
                }
                else
                {
                    //非充电
                    session.cd_a60.BMS_ChargeFS = 0;
                    session.cd_a60.BMS_ChargeSt = session.cd_a60.BMS_SlowChargeSt;
                }
            }
            return session.cd_a60;
        }
    }
}
