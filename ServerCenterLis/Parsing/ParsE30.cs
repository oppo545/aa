﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ServerCenterLis
{
    public class ParsE30
    {
        static string zcinfo1, result;
        static string channelNumber, packageNumber;
        /// 故障值  0正常         /// 故障等级    1	轻微故障,2	一般故障,3	严重故障,4	致命故障
        static int faultlever = 1, faultint = 0;
        public static Cls_RealInformation GetParsbyE30(Telnet_Session session, string info1, int msgnumber, int dataLength, ref string canfault)
        {
            string str_all;//A60
            int data1 = 0, data2 = 0;
            string str_data1, str_data2, str_data3, str_data4, str_data;
            session.cd_ej04 = session.getClsCdEJ04();
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

                #region MyRegion
                switch (packageNumber)
                {   //VCU点火开关及故障码
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
                                // session.cd_ej04.VCU_Keyposition = "OFF";
                                session.cd_ej04.VCU_Keyposition = "0";
                                //session.isacc = "0";
                                break;
                            case "01":
                                //session.cd_ej04.VCU_Keyposition = "ACC";
                                session.cd_ej04.VCU_Keyposition = "1";
                                // session.isacc = "1";
                                break;
                            case "10":
                                //  session.cd_ej04.VCU_Keyposition = "ON";
                                session.cd_ej04.VCU_Keyposition = "2";
                                //session.isacc = "1";
                                break;
                            case "11":
                                //   session.cd_ej04.VCU_Keyposition = "START";
                                session.cd_ej04.VCU_Keyposition = "3";
                                //session.isacc = "1";
                                break;
                            default:
                                break;
                        }
                        str_data2 = str_all.Substring(21, 2);
                        //data1 = Convert.ToInt32(str_data2, 16);
                        //str_data2 = Convert.ToString(data1, 2);
                        //if (str_data2.Length < 8)
                        //    str_data2 = str_data2.PadLeft(8, '0');
                        //str_data1 = str_data2.Substring(0, 3);
                        switch (str_data2)
                        {
                            case "36":
                                faultlever = 2;
                                break;
                            case "65":
                            case "66":
                                faultlever = 2;
                                break;
                            case "74":
                            case "75":
                                faultlever = 3;
                                break;
                            case "97":
                            case "98":
                            case "99":
                            case "100":
                            case "101":
                                faultlever = 4;
                                break;
                            default:
                                break;
                        }
                        session.cd_ej04.VCU_Fault = ParsMethod.GetParsWholeByte(str_data2);
                        faultint = session.cd_ej04.VCU_Fault;
                        if (faultint > 0)
                            canfault += "$VCU_Fault:" + "0X" + str_data2 + ":" + faultlever + ":" + faultint;
                        break;

                    //int VCU_BrakeEnergy;//电机状态及信号强度
                    //double VCU_CruisingRange;//剩余行驶里程
                    case "00000355":
                        str_data1 = str_all.Substring(7, 1);
                        data1 = Convert.ToInt32(str_data1, 16) - 5;
                        if (data1 > 0)
                            session.cd_ej04.VCU_BrakeEnergy = "1";//=======================修改
                        else if (data1 == 0)
                            session.cd_ej04.VCU_BrakeEnergy = "0";
                        else
                            session.cd_ej04.VCU_BrakeEnergy = "2";
                        str_data3 = str_all.Substring(12, 2);
                        data1 = Convert.ToInt32(str_data3, 16);
                        str_data3 = Convert.ToString(data1, 2);
                        if (str_data3.Length < 8)
                            str_data3 = str_data3.PadLeft(8, '0');
                        str_data3 = str_data3.Substring(2, 6);

                        str_data4 = str_all.Substring(15, 1);
                        data2 = Convert.ToInt32(str_data4, 16);
                        str_data4 = Convert.ToString(data2, 2);

                        //高位不足 补0 这里仅取 bit4-bit7 邰工的是 不足4位亦未补0
                        if (str_data4.Length < 4)
                            str_data4 = str_data4.PadLeft(4, '0');

                        str_data = string.Format("{0}{1}", str_data3, str_data4);
                        session.cd_ej04.VCU_CruisingRange = Convert.ToInt32(str_data, 2);
                        //TODO Filter 范围0-1000
                        session.cd_ej04.VCU_CruisingRange = session.cd_ej04.VCU_CruisingRange < 0 ? 0 : session.cd_ej04.VCU_CruisingRange;
                        break;
                    // string VCU_BrakePedalSt;//制动开关状态
                    case "000000E4":
                        str_data1 = str_all.Substring(6, 1);
                        data1 = Convert.ToInt32(str_data1, 16);
                        str_data1 = Convert.ToString(data1, 2);
                        str_data1 = str_data1.Substring(str_data1.Length - 1, 1);
                        if (str_data1 == "0")
                            session.cd_ej04.VCU_BrakePedalSt = "0";
                        else
                            session.cd_ej04.VCU_BrakePedalSt = "1";
                        break;
                    //string TCU_GearPosition;//换挡手柄位置
                    case "0000009A":
                        str_data1 = str_all.Substring(6, 1);
                        data1 = Convert.ToInt32(str_data1, 16);
                        str_data1 = Convert.ToString(data1, 2);
                        if (str_data1.Length < 4)
                            str_data1 = str_data1.PadLeft(4, '0');
                        str_data1 = str_data1.Substring(0, 3);
                        switch (str_data1)
                        {
                            case "000":
                                session.cd_ej04.TCU_GearPosition = "0";
                                break;
                            case "001":
                                session.cd_ej04.TCU_GearPosition = "1";
                                break;
                            case "010":
                                session.cd_ej04.TCU_GearPosition = "2";
                                break;
                            //case "011":
                            //    session.cd_ej04.TCU_GearPosition = "预留";
                            //    break;
                            //case "100":
                            //    session.cd_ej04.TCU_GearPosition = "预留";
                            //    break;
                            //case "101":
                            //    session.cd_ej04.TCU_GearPosition = "预留";
                            //    break;
                            case "110":
                                session.cd_ej04.TCU_GearPosition = "6";
                                break;
                            default:
                                break;
                        }
                        //double Motor_DCVolt;//直流母线电压
                        //double Motor_DCCurrent;//直流母线电流
                        break;
                    case "00000112":
                        str_data1 = str_all.Substring(6, 4);
                        str_data1 = regex.Replace(str_data1, "");
                        data1 = Convert.ToInt32(str_data1, 16);
                        str_data = Convert.ToString(data1, 2);
                        if (str_data.Length < 12)
                            str_data = str_data.PadLeft(12, '0');
                        str_data = str_data.Substring(0, 10);
                        session.cd_ej04.Motor_DCVolt = Convert.ToInt32(str_data, 2);
                        //TODO Filter 范围0-600
                        session.cd_ej04.Motor_DCVolt = session.cd_ej04.Motor_DCVolt < 0 ? 0 : session.cd_ej04.Motor_DCVolt;

                        str_data3 = str_all.Substring(9, 5);
                        str_data1 = regex.Replace(str_data3, "");
                        data2 = Convert.ToInt32(str_data1, 16);
                        str_data = Convert.ToString(data2, 2);
                        if (str_data.Length < 16)
                            str_data = str_data.PadLeft(16, '0');
                        str_data = str_data.Substring(3, 11);
                        session.cd_ej04.Motor_DCCurrent = Convert.ToInt32(str_data, 2) - 800;
                        break;
                    //double Motor_Revolution;//电机转速
                    //double Motor_OutputTorque;//电机反馈扭矩
                    //string Motor_State = "";//电机状态//=============增加
                    //double Motor_AllowMaxTorque = 0;//电机允许最大扭矩===============增加
                    case "000000C0":
                        //电机状态
                        str_data3 = str_all.Substring(6, 1);
                        //str_data3 = Convert.ToString(Convert.ToInt32(str_data3, 16), 2);
                        str_data3 = Telnet_Session.SixteenTrunTwo(str_data3);
                        if (str_data3.Length < 4)
                            str_data3 = str_data3.PadLeft(4, '0');
                        str_data3 = str_data3.Substring(0, 3);
                        switch (str_data3)
                        {
                            case "000":
                                session.cd_ej04.Motor_State = "0";
                                break;
                            case "001":
                                session.cd_ej04.Motor_State = "1";
                                break;
                            case "010":
                                session.cd_ej04.Motor_State = "2";
                                break;
                            case "011":
                                session.cd_ej04.Motor_State = "3";
                                break;
                            case "100":
                                session.cd_ej04.Motor_State = "4";
                                break;
                            //case "101":
                            //    session.cd_ej04.Motor_State = "预留";
                            //    break;
                            //case "110":
                            //    session.cd_ej04.Motor_State = "预留";
                            //    break;
                            default:
                                break;
                        }
                        //电机转速
                        str_data1 = str_all.Substring(9, 7);
                        str_data1 = regex.Replace(str_data1, "");
                        data1 = Convert.ToInt32(str_data1, 16);
                        str_data1 = Convert.ToString(data1, 2);
                        if (str_data1.Length < 20)
                            str_data1 = str_data1.PadLeft(20, '0');
                        str_data1 = str_data1.Substring(0, 18);
                        session.cd_ej04.Motor_Revolution = Convert.ToInt32(str_data1, 2) * 0.1 - 12000;
                        //TODO Filter 范围-12000-12000
                        session.cd_ej04.Motor_Revolution = Math.Round(session.cd_ej04.Motor_Revolution, 0);

                        //电机反馈扭矩
                        str_data2 = str_all.Substring(15, 5);
                        str_data2 = regex.Replace(str_data2, "");
                        data2 = Convert.ToInt32(str_data2, 16);
                        str_data2 = Convert.ToString(data2, 2);
                        if (str_data2.Length < 16)
                            str_data2 = str_data2.PadLeft(16, '0');
                        str_data2 = str_data2.Substring(2, 14);
                        session.cd_ej04.Motor_OutputTorque = Math.Round(Convert.ToInt32(str_data2, 2) * 0.1 - 300, 2);

                        //电机允许最大扭矩
                        str_data4 = str_all.Substring(21, 5);
                        str_data4 = regex.Replace(str_data4, "");
                        str_data4 = Convert.ToString(Convert.ToInt32(str_data4, 16), 2);
                        if (str_data4.Length < 16)
                            str_data4 = str_data4.PadLeft(16, '0');
                        str_data4 = str_data4.Substring(2, 14);
                        session.cd_ej04.Motor_AllowMaxTorque = Math.Round(Convert.ToInt32(str_data4, 2) * 0.1 - 300, 2);
                        break;

                    //string Motor_Fault;//电机故障码
                    //double Motor_Temperature;//电机本体温度
                    //double Motor_ControllerTemp;//电机控制器温度
                    //double Motor_OutputPower;//电机功率
                    //case "03 60":
                    case "00000360":
                        str_data1 = str_all.Substring(6, 2);
                        session.cd_ej04.Motor_Fault = ParsMethod.GetParsWholeByte(str_data1);
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
                                session.cd_ej04.Motor_Fault = 0;
                                break;
                        }

                        faultint = session.cd_ej04.Motor_Fault;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "Motor_Fault", "0X360-0X" + str_data1, faultlever, faultint);

                        //电机本体温度
                        str_data2 = str_all.Substring(9, 2);
                        session.cd_ej04.Motor_Temperature = Convert.ToInt32(str_data2, 16) - 40;
                        //电机控制器温度
                        str_data3 = str_all.Substring(15, 2);
                        session.cd_ej04.Motor_ControllerTemp = Convert.ToInt32(str_data3, 16) - 40;
                        //电机功率
                        str_data4 = str_all.Substring(19, 4);
                        str_data4 = regex.Replace(str_data4, "");
                        data1 = Convert.ToInt32(str_data4, 16);
                        str_data4 = Convert.ToString(data1, 2);
                        if (str_data4.Length < 12)
                            str_data4 = str_data4.PadLeft(12, '0');
                        str_data4 = str_data4.Substring(1, 11);
                        session.cd_ej04.Motor_OutputPower = Math.Round(Convert.ToInt32(str_data4, 2) * 0.1 - 100, 3);
                        break;
                    //double BMS_SOC;//电池SOC
                    //double BMS_Voltage;//电池总电压
                    //double BMS_Current;//电池充放电总电流
                    case "00000125":
                        //电池SOC
                        str_data1 = str_all.Substring(6, 2);
                        session.cd_ej04.BMS_SOC = Convert.ToInt32(str_data1, 16);
                        //TODO Filter 范围0-100
                        session.cd_ej04.BMS_SOC = session.cd_ej04.BMS_SOC < 0 ? 0 : session.cd_ej04.BMS_SOC;

                        //电池总电压
                        str_data2 = str_all.Substring(15, 4);
                        str_data2 = regex.Replace(str_data2, "");
                        data2 = Convert.ToInt32(str_data2, 16);
                        str_data2 = Convert.ToString(data2, 2);
                        if (str_data2.Length < 12)
                            str_data2 = str_data2.PadLeft(12, '0');
                        str_data2 = str_data2.Substring(0, 9);
                        session.cd_ej04.BMS_Voltage = Convert.ToInt32(str_data2, 2);
                        //TODO Filter 范围0-500
                        session.cd_ej04.BMS_Voltage = session.cd_ej04.BMS_Voltage < 0 ? 0 : session.cd_ej04.BMS_Voltage;

                        //电池充放电总电流
                        str_data3 = str_all.Substring(19, 4);
                        str_data3 = regex.Replace(str_data3, "");
                        session.cd_ej04.BMS_Current = Convert.ToInt32(str_data3, 16) - 600;
                        break;

                    //漏电检测//BMS_CreepageMonitor============增加
                    case "00000127":
                        str_data = str_all.Substring(25, 4);
                        str_data = regex.Replace(str_data, "");
                        session.cd_ej04.BMS_CreepageMonitor = Convert.ToInt32(str_data, 16);
                        //TODO Filter 范围0-2000
                        session.cd_ej04.BMS_CreepageMonitor = session.cd_ej04.BMS_CreepageMonitor < 0 ? 0 : session.cd_ej04.BMS_CreepageMonitor;

                        break;

                    //double BMS_SOCCalculate;//电池SOC(处理后)
                    //string BMS_OutsideChargeSignal;//外部充电信号(动力电池充电指示)
                    //string BMS_FaultDislpay;//电池故障显示用
                    //int 信号 BMS_OFCConnectSignal 非车载充电连接指示信号
                    //double BMS_Temperature;//电池温度
                    //double BMS_Temp_Max;//电池最高温度
                    //double BMS_Temp_Min;//电池最低问题
                    case "00000358":
                        str_data1 = str_all.Substring(6, 2);
                        session.cd_ej04.BMS_SOCCalculate = Convert.ToInt32(str_data1, 16);
                        //TODO Filter 范围0-100
                        session.cd_ej04.BMS_SOCCalculate = session.cd_ej04.BMS_SOCCalculate < 0 ? 0 : session.cd_ej04.BMS_SOCCalculate;

                        str_data2 = str_all.Substring(9, 1);
                        data2 = Convert.ToInt32(str_data2, 16);
                        str_data2 = Convert.ToString(data2, 2);
                        if (str_data2.Length < 4)
                            str_data2 = str_data2.PadLeft(4, '0');
                        str_data3 = str_data2.Substring(0, 1);
                        if (str_data3 == "0")
                            session.cd_ej04.BMS_OutsideChargeSignal = "0";
                        else
                            session.cd_ej04.BMS_OutsideChargeSignal = "1";
                        //电池故障
                        str_data4 = str_data2.Substring(1, 1);
                        if (str_data4 == "0")
                            session.cd_ej04.BMS_FaultDislpay = "0";
                        else
                            session.cd_ej04.BMS_FaultDislpay = "1";
                        //非车载充电连接指示信号
                        str_data4 = str_data2.Substring(2, 1);
                        if (str_data4 == "0")
                            session.cd_ej04.BMS_OFCConnectSignal = "0";
                        else
                            session.cd_ej04.BMS_OFCConnectSignal = "1";
                        //电池温度
                        str_data1 = str_all.Substring(18, 2);
                        session.cd_ej04.BMS_Temperature = Convert.ToInt32(str_data1, 16) - 40;
                        //电池最高温度
                        str_data2 = str_all.Substring(24, 2);
                        session.cd_ej04.BMS_Temp_Max = Convert.ToInt32(str_data2, 16) - 40;
                        //电池最低温度
                        str_data3 = str_all.Substring(27, 2);
                        session.cd_ej04.BMS_Temp_Min = Convert.ToInt32(str_data3, 16) - 40;
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
                        session.cd_ej04.BMS_Fault = ParsMethod.GetParsWholeByte(str_data);
                        #region switch
                        //BMK 2035-10-21 根据 2035-10-20 才总发送的邮件 <<【东电】安全预警需求>> 添加  52,51,50,4f,4e,4b,4A,48,47,46,21
                        switch (str_data)
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
                                session.cd_ej04.BMS_Fault = 0;
                                faultlever = 0;
                                break;
                        }

                        #endregion

                        faultint = session.cd_ej04.BMS_Fault;
                        if (faultint > 0)
                            canfault += "$BMS_Fault:" + "0X359-0X" + str_data + ":" + faultlever + ":" + session.cd_ej04.BMS_Fault;

                        //最高单体电压
                        str_data1 = str_all.Substring(9, 2);
                        session.cd_ej04.BMS_MaxCellBatt = Convert.ToInt32(str_data1, 16) / 64;
                        //最高单体电压
                        str_data2 = str_all.Substring(12, 2);
                        session.cd_ej04.BMS_MinCellBatt = Convert.ToInt32(str_data2, 16) / 64;
                        //最高单体电压电池编号
                        str_data3 = str_all.Substring(15, 2);
                        session.cd_ej04.BMS_MaxCellBattNumber = Convert.ToInt32(str_data3, 16);
                        //最高单体电压电池编号
                        str_data4 = str_all.Substring(18, 2);
                        session.cd_ej04.BMS_MinCellBattNumber = Convert.ToInt32(str_data4, 16);
                        break;

                    //string BMS_ChargeSt;//充电状态
                    //string BMS_ChargerACInput 交流输入状态
                    case "00000361":
                        str_data1 = str_all.Substring(15, 1);
                        data1 = Convert.ToInt32(str_data1, 16);
                        str_data1 = Convert.ToString(data1, 2);
                        if (str_data1.Length < 4)
                            str_data1 = str_data1.PadLeft(4, '0');
                        str_data2 = str_data1.Substring(1, 2);
                        switch (str_data2)
                        {
                            case "00":
                                session.cd_ej04.BMS_ChargeSt = "0";
                                break;
                            case "01":
                                session.cd_ej04.BMS_ChargeSt = "1";
                                break;
                            case "10":
                                session.cd_ej04.BMS_ChargeSt = "2";
                                break;
                            case "11":
                                session.cd_ej04.BMS_ChargeSt = "3";
                                break;
                            default:
                                break;
                        }

                        //交流输入状态
                        str_data1 = str_data1.Substring(3, 1);
                        switch (str_data1)
                        {
                            case "0":
                                session.cd_ej04.BMS_ChargerACInput = "0";
                                break;
                            case "1":
                                session.cd_ej04.BMS_ChargerACInput = "1";
                                break;
                            default:
                                break;
                        }

                        break;
                    //double ONC_OutputVoltage;//充电机输出的充电电压
                    //double ONC_OutputCurrent;//充电机输出的充电电流
                    //double ONC_ONCTemp;//充电机温度
                    //string ONC_InputVoltageSt = "";//输入电压状态============增加
                    //string ONC_CommunicationSt = "";//通讯状态============增加
                    //string ONC_Fault = "";//充电机故障码============增加
                    case "000003A2":
                        str_data = str_all.Substring(6, 23);
                        if (str_data == "00 00 00 00 00 00 00 00")
                        {
                            session.cd_ej04.ONC_OutputVoltage = 0;
                            session.cd_ej04.ONC_OutputCurrent = 0;
                            session.cd_ej04.ONC_ONCTemp = 0;
                            break;
                        }
                        else
                        {
                            //充电机输出的充电电压
                            str_data1 = str_all.Substring(6, 5);
                            str_data1 = regex.Replace(str_data1, "");
                            session.cd_ej04.ONC_OutputVoltage = Convert.ToInt32(str_data1, 16) * 0.1 - 3276.7;
                            //TODO Filter 范围-3276.7-3276.7
                            session.cd_ej04.ONC_OutputVoltage = Math.Round(session.cd_ej04.ONC_OutputVoltage, 1);
                            //充电机输出的充电电流
                            str_data2 = str_all.Substring(12, 2);
                            session.cd_ej04.ONC_OutputCurrent = Convert.ToInt32(str_data2, 16) * 0.1;
                            //TODO Filter 范围0-25.5
                            session.cd_ej04.ONC_OutputCurrent = session.cd_ej04.ONC_OutputCurrent < 0 ? 0 : session.cd_ej04.ONC_OutputCurrent;
                            session.cd_ej04.ONC_OutputCurrent = Math.Round(session.cd_ej04.ONC_OutputCurrent, 1);

                            //充电机温度
                            str_data3 = str_all.Substring(18, 2);
                            session.cd_ej04.ONC_ONCTemp = Convert.ToInt32(str_data3, 16) - 100;
                            //输入电压状态
                            str_data2 = str_all.Substring(15, 1);
                            str_data2 = Convert.ToString(Convert.ToInt32(str_data2, 16), 2);
                            if (str_data2.Length < 4)
                                str_data2 = str_data2.PadLeft(4, '0');
                            str_data2 = str_data2.Substring(2, 1);
                            if (str_data2 == "0")
                                session.cd_ej04.ONC_InputVoltageSt = "0";
                            else
                                session.cd_ej04.ONC_InputVoltageSt = "1";
                            //通信状态
                            str_data3 = str_all.Substring(16, 1);
                            str_data3 = Convert.ToString(Convert.ToInt32(str_data3, 16), 2);
                            if (str_data3.Length < 4)
                                str_data3 = str_data3.PadLeft(4, '0');
                            str_data3 = str_data3.Substring(0, 1);
                            if (str_data3 == "0")
                                session.cd_ej04.ONC_CommunicationSt = "0";
                            else
                                session.cd_ej04.ONC_CommunicationSt = "1";

                            //充电机故障码
                            //session.cd_ej04.ONC_Fault = "";
                            str_data1 = str_all.Substring(24, 4);
                            str_data2 = regex.Replace(str_data1, "");
                            str_data1 = Convert.ToString(Convert.ToInt32(str_data2, 16), 2);
                            if (str_data1.Length < 12)
                                str_data1 = str_data1.PadLeft(12, '0');
                            //for (int j = 0; j < str_data4.Length; j++)
                            //{
                            //    if (str_data4[j] == '1')
                            //        //session.cd_ej04.ONC_Fault += oncfault[j] + ",";
                            //        session.cd_ej04.ONC_Fault += j + ",";
                            //}
                            //if (!string.IsNullOrEmpty(session.cd_ej04.ONC_Fault))
                            //{
                            //    session.cd_ej04.ONC_Fault = session.cd_ej04.ONC_Fault.Substring(0, session.cd_ej04.ONC_Fault.Length - 1);
                            //    foreach (var item in session.cd_ej04.ONC_Fault.Split(','))
                            //    {
                            //        if (int.Parse(item)==4 ||int.Parse(item)==5)
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
                            //    session.cd_ej04.ONC_Fault = "0";
                            //}
                            #region 标志位分离
                            result = str_data1.Substring(11);
                            session.cd_ej04.ONC_OverTempOutputPowerReduceHalf = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_OverTempOutputPowerReduceHalf", "0X" + str_data2 + "-bit0", 2, int.Parse(result));
                            result = str_data1.Substring(10, 1);
                            session.cd_ej04.ONC_OverTempOutputClose = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_OverTempOutputClose", "0X" + str_data2 + "-bit1", 2, int.Parse(result));
                            result = str_data1.Substring(9, 1);
                            session.cd_ej04.ONC_InputUndervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_InputUndervoltage", "0X" + str_data2 + "-bit2", 2, int.Parse(result));
                            result = str_data1.Substring(8, 1);
                            session.cd_ej04.ONC_InputOvervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_InputOvervoltage", "0X" + str_data2 + "-bit3", 2, int.Parse(result));
                            result = str_data1.Substring(7, 1);
                            session.cd_ej04.ONC_InnerPFCOvervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_InnerPFCOvervoltage", "0X" + str_data2 + "-bit4", 2, int.Parse(result));
                            result = str_data1.Substring(6, 1);
                            session.cd_ej04.ONC_InnerPFCUndervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_InnerPFCUndervoltage", "0X" + str_data2 + "-bit5", 2, int.Parse(result));
                            result = str_data1.Substring(5, 1);
                            session.cd_ej04.ONC_OutputOvercurrent = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_OutputOvercurrent", "0X" + str_data2 + "-bit6", 3, int.Parse(result));
                            result = str_data1.Substring(4, 1);
                            session.cd_ej04.ONC_OutputShortcircuitOrUndervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_OutputShortcircuitOrUndervoltage", "0X" + str_data2 + "-bit7", 3, int.Parse(result));
                            result = str_data1.Substring(3, 1);
                            session.cd_ej04.ONC_InnerMiddleUndervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_InnerMiddleUndervoltage", "0X" + str_data2 + "-bit8", 2, int.Parse(result));
                            result = str_data1.Substring(2, 1);
                            session.cd_ej04.ONC_InputUndercurrent = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_InputUndercurrent", "0X" + str_data2 + "-bit9", 2, int.Parse(result));
                            result = str_data1.Substring(1, 1);
                            session.cd_ej04.ONC_OutputOvervoltage = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_OutputOvervoltage", "0X" + str_data2 + "-bit10", 2, int.Parse(result));
                            result = str_data1.Substring(0, 1);
                            session.cd_ej04.ONC_CommunicationTimeout = result;//.Equals("0") ? "" : result;
                            canfault += PublicMethods.GetCanFaultStr("ONC_CommunicationTimeout", "0X" + str_data2 + "-bit11", 2, int.Parse(result));
                            if (str_data1.Contains("1"))
                            {
                                session.cd_ej04.ONC_Fault = "1";
                            }
                            else
                            {
                                session.cd_ej04.ONC_Fault = "0";
                            }
                            #endregion
                            break;
                        }

                    //double IC_TotalOdmeter;//总里程
                    //double IC_Odmeter;//小计里程
                    case "00000320":
                        //总里程
                        str_data1 = str_all.Substring(6, 8);
                        str_data1 = regex.Replace(str_data1, "");
                        session.cd_ej04.IC_TotalOdmeter = Convert.ToInt32(str_data1, 16) * 0.1;
                        //TODO Filter 范围0-999999.9
                        session.cd_ej04.IC_TotalOdmeter = session.cd_ej04.IC_TotalOdmeter < 0 ? 0 : session.cd_ej04.IC_TotalOdmeter;
                        session.cd_ej04.IC_TotalOdmeter = Math.Round(session.cd_ej04.IC_TotalOdmeter, 1);

                        //小计里程
                        str_data2 = str_all.Substring(15, 5);
                        str_data2 = regex.Replace(str_data2, "");
                        session.cd_ej04.IC_Odmeter = Convert.ToInt32(str_data2, 16) * 0.1;
                        //TODO Filter 范围0-999.9
                        session.cd_ej04.IC_Odmeter = session.cd_ej04.IC_Odmeter < 0 ? 0 : session.cd_ej04.IC_Odmeter;//小于0 取0
                        session.cd_ej04.IC_Odmeter = Math.Round(session.cd_ej04.IC_Odmeter, 1);//取一位小数

                        break;
                    //string ABS_BrakeSignal;//制动信号
                    //double ABS_VehSpd;//车速信号
                    case "000000A0":
                        str_data1 = str_all.Substring(13, 1);
                        data1 = Convert.ToInt32(str_data1, 16);
                        str_data1 = Convert.ToString(data1, 2);
                        if (str_data1.Length < 4)
                            str_data1 = str_data1.PadLeft(4, '0');
                        str_data1 = str_data1.Substring(1, 1);
                        //车速信号
                        if (str_data1 == "0")
                            session.cd_ej04.ABS_BrakeSignal = "0";
                        else
                            session.cd_ej04.ABS_BrakeSignal = "1";
                        str_data2 = str_all.Substring(24, 5);
                        str_data2 = regex.Replace(str_data2, "");
                        session.cd_ej04.ABS_VehSpd = Convert.ToInt32(str_data2, 16) * 0.01;
                        //TODO Filter 范围0-300
                        session.cd_ej04.ABS_VehSpd = session.cd_ej04.ABS_VehSpd < 0 ? 0 : session.cd_ej04.ABS_VehSpd;//小于0 取0
                        session.cd_ej04.ABS_VehSpd = Math.Round(session.cd_ej04.ABS_VehSpd, 0);//取小数
                        break;
                    //double DCDC_Temperature;//DCDC温度
                    //double DCDC_OutputVoltage;//DCDC输出电压
                    //double DCDC_OutputCurrent;//DCDC输出电流
                    //string DCDC_EnableResponse;//DCDC使能应答
                    //string DCDC_Fault;//DCDC故障
                    //double DCDC_InputVoltage;//DCDC输入电压
                    //double DCDC_InputCurrent;//DCDC输入电流
                    case "00000345"://===============================修改
                        str_data1 = str_all.Substring(6, 2);
                        session.cd_ej04.DCDC_Temperature = Convert.ToInt32(str_data1, 16) - 60;
                        str_data2 = str_all.Substring(9, 2);
                        session.cd_ej04.DCDC_OutputVoltage = Convert.ToInt32(str_data2, 16) * 0.125;
                        //DC-DC输出电流
                        str_data3 = str_all.Substring(12, 4);
                        str_data3 = regex.Replace(str_data3, "");
                        session.cd_ej04.DCDC_OutputCurrent = Convert.ToInt32(str_data3, 16) * 0.125;
                        //DC-DC使能应答
                        str_data4 = str_all.Substring(16, 1);
                        data2 = Convert.ToInt32(str_data4, 16);
                        str_data4 = Convert.ToString(data2, 2);
                        if (str_data4.Length < 4)
                            str_data4 = str_data4.PadLeft(4, '0');
                        str_data4 = str_data4.Substring(0, 1);
                        if (str_data4 == "0")
                            session.cd_ej04.DCDC_EnableResponse = "0";
                        else
                            session.cd_ej04.DCDC_EnableResponse = "1";
                        //DC-DC故障
                        session.cd_ej04.DCDC_Fault = "";
                        str_data = str_all.Substring(18, 2);
                        WriteLog.WriteOrdersLog("str_data:" + str_data);
                        data1 = Convert.ToInt32(str_data, 16);
                        str_data1 = Convert.ToString(data1, 2);
                        if (str_data1.Length < 8)
                            str_data1 = str_data1.PadLeft(8, '0');
                        //for (int j = 0; j < str_data.Length; j++)
                        //{
                        //    if (str_data[j] != '0')
                        //        //  session.cd_ej04.DCDC_Fault += dcdcfault[j] + ",";
                        //        session.cd_ej04.DCDC_Fault += j + ",";
                        //}
                        //if (!string.IsNullOrEmpty(session.cd_ej04.DCDC_Fault))
                        //{
                        //    session.cd_ej04.DCDC_Fault = session.cd_ej04.DCDC_Fault.Substring(0, session.cd_ej04.DCDC_Fault.Length - 1);
                        //    foreach (var item in session.cd_ej04.DCDC_Fault.Split(','))
                        //    {
                        //        if (int.Parse(item) == 3)
                        //        {
                        //            faultlever = 4;
                        //        }
                        //        else if (int.Parse(item) > 3)
                        //        {
                        //            faultlever = 3;
                        //        }
                        //        if (!string.IsNullOrEmpty(item))
                        //        {
                        //            canfault += "$DCDC_Fault:0X345-bit" + (7 - int.Parse(item)) + ":" + faultlever + ":" + item;
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    session.cd_ej04.DCDC_Fault = "-1";
                        //}
                        #region  标志位分离
                        result = str_data1.Substring(7, 1);
                        session.cd_ej04.DCDC_InputOvervoltage = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("DCDC_InputOvervoltage", "0X" + str_data + "-bit0", 3, int.Parse(result));
                        result = str_data1.Substring(6, 1);
                        session.cd_ej04.DCDC_InputUndervoltage = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("DCDC_InputUndervoltage", "0X" + str_data + "-bit1", 3, int.Parse(result));
                        result = str_data1.Substring(5, 1);
                        session.cd_ej04.DCDC_OutputOvervoltage = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("DCDC_OutputOvervoltage", "0X" + str_data + "-bit2", 3, int.Parse(result));
                        result = str_data1.Substring(4, 1);
                        session.cd_ej04.DCDC_OutputUndervoltage = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("DCDC_OutputUndervoltage", "0X" + str_data + "-bit3", 3, int.Parse(result));
                        result = str_data1.Substring(3, 1);
                        session.cd_ej04.DCDC_OutputOvercurrent = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("DCDC_OutputOvercurrent", "0X" + str_data + "-bit4", 4, int.Parse(result));
                        result = str_data1.Substring(2, 1);
                        session.cd_ej04.DCDC_LowTempAlarm = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("DCDC_LowTempAlarm", "0X" + str_data + "-bit5", 2, int.Parse(result));
                        result = str_data1.Substring(1, 1);
                        session.cd_ej04.DCDC_HighTempAlarm = result;//.Equals("0") ? "" : result;
                        canfault += PublicMethods.GetCanFaultStr("DCDC_HighTempAlarm", "0X" + str_data + "-bit6", 2, int.Parse(result));
                        if (str_data1.Contains("1"))
                        {
                            session.cd_ej04.DCDC_Fault = "1";
                        }
                        else
                        {
                            session.cd_ej04.DCDC_Fault = "0";
                        }
                        #endregion

                       // WriteLog.WriteOrdersLog("str_data:" + session.cd_ej04.DCDC_Fault + "----->");
                        //DC-DC输入电压
                        str_data1 = str_all.Substring(21, 5);
                        str_data1 = regex.Replace(str_data1, "");
                        session.cd_ej04.DCDC_InputVoltage = Convert.ToInt32(str_data1, 16) * 0.01563;
                        //TODO Filter 范围0-500
                        session.cd_ej04.DCDC_InputVoltage = session.cd_ej04.DCDC_InputVoltage < 0 ? 0 : session.cd_ej04.DCDC_InputVoltage;//小于0 取0
                        session.cd_ej04.DCDC_InputVoltage = Math.Round(session.cd_ej04.DCDC_InputVoltage, 0);//取小数

                        //DC-DC输入电流
                        str_data2 = str_all.Substring(27, 2);
                        session.cd_ej04.DCDC_InputCurrent = Convert.ToInt32(str_data2, 16) * 0.125;
                        //TODO Filter 范围0-25
                        session.cd_ej04.DCDC_InputCurrent = Math.Round(session.cd_ej04.DCDC_InputCurrent, 0);

                        break;
                    default:
                        break;
                }
                #endregion
            }

            if (!string.IsNullOrEmpty(session.cd_ej04.BMS_ChargeSt))
            {
                //快慢充
                if (session.cd_ej04.BMS_ChargeSt.Equals("1"))
                {
                    if (session.cd_ej04.BMS_OutsideChargeSignal.Equals("1"))
                    {
                        //慢充
                        session.cd_ej04.BMS_ChargeFS = 2;
                    }
                    if (session.cd_ej04.BMS_OFCConnectSignal.Equals("1"))
                    {
                        //快充
                        session.cd_ej04.BMS_ChargeFS = 1;
                    }
                }
                else
                {
                    //非充电
                    session.cd_ej04.BMS_ChargeFS = 0;
                }
            }
            WriteLog.WriteLogMeaning("E30", info1, session.cd_ej04);
            return session.cd_ej04;
        }
    }
}

