using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCenterLis
{
    /// <summary>
    ///  北京地标 海马
    /// </summary>
    class ParsBLStandard
    {
        static string zcinfo1;
        static string channelNumber, packageNumber;
        /// 故障值  0正常         /// 故障等级    1	轻微故障,2	一般故障,3	严重故障,4	致命故障
        static int faultlever = 1, faultint = 0;
        public static Cls_RealInformation GetParsbyBLStandard(Telnet_Session session, string info1, int msgnumber, int dataLength, ref string canfault)
        {
            session.cls_hme = session.getClsHme();
            session.cls_vlsei = session.getClsVlsEI();
            string bit; string[] byte0;//海马
            for (int i = 0; i < msgnumber; i++)
            {
                zcinfo1 = info1.Substring(3 * dataLength * i + 18 * i, 3 * dataLength + 17);

                channelNumber = zcinfo1.Substring(0, 2);
                packageNumber = PublicMethods.GetZeroSuppression(zcinfo1.Substring(3, 11));
                dataLength = PublicMethods.Get16To10(zcinfo1.Substring(15, 2));
                byte0 = zcinfo1.Substring(18, 23).Split(' ');

                #region HM
                switch (packageNumber)
                {
                    #region 0CF401D0 √
                    case "0CF401D0":
                        {
                            //byte[0]
                            bit = PublicMethods.Get16To2(byte0[0], 1);
                            //电机正常回馈模式
                            session.cls_hme.VCU_Normal_Feedback = int.Parse(bit.Substring(1, 1));
                            //电机启停命令
                            session.cls_hme.VCU_Motor_Run_Stop = int.Parse(bit.Substring(2, 1));
                            //超速报警信号
                            session.cls_hme.VCU_OverSpeed_Alarm = int.Parse(bit.Substring(3, 1));
                            //电机正反转指令
                            session.cls_hme.VCU_Motor_Forward_Reverse = PublicMethods.Get2To10(bit.Substring(4, 2));
                            //电机控制模式
                            session.cls_hme.VCU_Control_Mode = PublicMethods.Get2To10(bit.Substring(6, 2));

                            //byte[1],byte[2]         //电机目标扭矩
                            session.cls_hme.VCU_Motor_Target_tq = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[2], 1) + PublicMethods.Get16To2(byte0[1], 1)) * 0.01d - 327, 2);
                            //byte[3],byte[4]         //电机目标转速
                            session.cls_hme.VCU_Motor_Target_rpm = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[4], 1) + PublicMethods.Get16To2(byte0[3], 1)) * 0.4d - 13107, 2);

                            //byte[6]
                            bit = PublicMethods.Get16To2(byte0[6], 1);
                            //充电线状态
                            session.cls_hme.VCU_Chargingstate = PublicMethods.Get2To10(bit.Substring(0, 1));
                            //DCDC工作指令
                            session.cls_hme.VCU_DCDC_Workorder = PublicMethods.Get2To10(bit.Substring(5, 1));
                            //DCDC故障
                            session.cls_hme.DCDC_Fault = PublicMethods.Get2To10(bit.Substring(7, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.DCDC_Fault;
                            if (faultint > 0)
                                canfault += string.Format("${0}::{1}:{2}", "DCDC_Fault", faultlever, faultint);
                            session.AddLisFault("01,05," + faultlever + "," + faultint);
                            //byte[7]
                            //剩余行驶里程
                            session.cls_hme.VCU_CruisingRange = PublicMethods.Get16To10(byte0[7]);

                            break;
                        }
                    #endregion
                    #region 18F464D0  √
                    case "18F464D0":
                        {
                            //byte[0]
                            //DCDC工作电流
                            session.cls_hme.VCU_DCDC_Current = PublicMethods.Get16To10(byte0[0]);
                            //byte[1]
                            bit = PublicMethods.Get16To2(byte0[1], 1);
                            //ready
                            session.cls_hme.VCU_Ready = int.Parse(bit.Substring(0, 1));
                            //高压器件连接状态
                            session.cls_hme.VCU_Hdevice_Connection = int.Parse(bit.Substring(3, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.VCU_Hdevice_Connection;
                            if (faultint > 0)
                                canfault += string.Format("$VCU_Hdevice_Connection::{0}:{1}", faultlever, faultint);

                            //钥匙档信号
                            session.cls_hme.VCU_Keyposition = PublicMethods.Get2To10(bit.Substring(4, 2));
                            //充电指令1
                            session.cls_hme.VCU_ChargeOrder1 = int.Parse(bit.Substring(6, 1));

                            //byte[2]
                            //百公里能耗
                            session.cls_hme.VCU_kwh = Math.Round(PublicMethods.Get16To10(byte0[2]) * 0.1d, 2);

                            //byte[5]
                            bit = PublicMethods.Get16To2(byte0[5], 1);
                            //空调系统使能
                            session.cls_hme.VCU_HCL_Run_Stop = int.Parse(bit.Substring(0, 1));
                            //电池正极继电器指令
                            session.cls_hme.VCU_Positive_Relay = int.Parse(bit.Substring(1, 1));
                            //电池负极继电器指令
                            session.cls_hme.VCU_Negative_Relay = int.Parse(bit.Substring(2, 1));
                            //电池预充电继电器指令
                            session.cls_hme.VCU_Pre_Charging_Relay = int.Parse(bit.Substring(3, 1));
                            //BMS启动状态
                            session.cls_hme.VCU_BMS_Start = int.Parse(bit.Substring(4, 1));
                            //充电继电器状态
                            session.cls_hme.VCU_Chargerelay_State = int.Parse(bit.Substring(5, 1));
                            //PTC启动状态
                            session.cls_hme.VCU_PTC_Run_Stop = int.Parse(bit.Substring(6, 1));
                            //碰撞开关状态/安全气囊状态
                            session.cls_hme.VCU_Impact_Switch = int.Parse(bit.Substring(7, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.VCU_Impact_Switch;
                            if (faultint > 0)
                                canfault += string.Format("$VCU_Impact_Switch::{0}:{1}", faultlever, faultint);

                            //byte[6]
                            bit = PublicMethods.Get16To2(byte0[6], 1);
                            //制动真空泵状态
                            session.cls_hme.VCU_Vacuum_Pump_State = int.Parse(bit.Substring(0, 1));
                            //制动真空泵报警
                            session.cls_hme.VCU_Vacuum_Pump_Fault = int.Parse(bit.Substring(1, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.VCU_Vacuum_Pump_Fault;
                            if (faultint > 0)
                                canfault += string.Format("$VCU_Vacuum_Pump_Fault::{0}:{1}", faultlever, faultint);

                            //加速踏板自检
                            session.cls_hme.VCU_Accel_SelfCheck = int.Parse(bit.Substring(2, 1));
                            break;
                        }
                    #endregion
                    #region 18F478D0  √
                    case "18F478D0":
                        {
                            //byte[0],byte[1]        
                            //外界允许的充电电流
                            session.cls_hme.VCU_OutsideAllow_ChargA = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[1], 1) + PublicMethods.Get16To2(byte0[0], 1)) * 0.1f - 400, 2);
                            //byte[3]
                            bit = PublicMethods.Get16To2(byte0[3], 1);
                            //ECO模式
                            session.cls_hme.VCU_ECOMode = int.Parse(bit.Substring(0, 1));
                            //驻车指令
                            session.cls_hme.VCU_Parking_Order = PublicMethods.Get2To10(bit.Substring(3, 2));
                            //制动状态
                            session.cls_hme.VCU_BrakePedalSt = int.Parse(bit.Substring(6, 1));

                            //byte[5]
                            bit = PublicMethods.Get16To2(byte0[5], 1);
                            //快充充电指令
                            session.cls_hme.VCU_FastCharge_Oder = int.Parse(bit.Substring(1, 1));

                            //快慢冲 逻辑添加 BMK 2035-12-7
                            if (session.cls_vls.BMS_ChargeSt.Equals("1"))
                                session.cls_hme.BMS_ChargeFS = session.cls_hme.VCU_FastCharge_Oder.Equals("1") ? 1 : 2;
                            else session.cls_hme.BMS_ChargeFS = 0;

                            //充电指示灯
                            session.cls_hme.VCU_ChargeLight = int.Parse(bit.Substring(3, 1));
                            //限功率信号
                            session.cls_hme.VCU_PowerLimit = int.Parse(bit.Substring(4, 1));

                            //byte[6]
                            //风扇占空比
                            session.cls_hme.VCU_Fan_DutyRatio = Math.Round(PublicMethods.Get16To10(byte0[6]) * 1.0, 2);

                            //byte[7]
                            //水泵占空比
                            session.cls_hme.VCU_Pump_DutyRatio = Math.Round(PublicMethods.Get16To10(byte0[7]) * 1.0, 2);
                            break;
                        }
                    #endregion
                    #region 0CF50DEF  √
                    case "0CF50DEF":
                        {
                            //byte[0]
                            bit = PublicMethods.Get16To2(byte0[0], 1);
                            //正常回馈模式
                            session.cls_hme.Motor_NormalRegen_Mode = int.Parse(bit.Substring(3, 1));
                            //电机控制器强电通断状态
                            session.cls_hme.Motor_MCU_Power_On_Off = int.Parse(bit.Substring(4, 1));
                            //电机启停状态
                            session.cls_hme.Motor_Start_Stop = int.Parse(bit.Substring(5, 1));
                            //控制模式
                            session.cls_hme.Motor_Control_Mode = PublicMethods.Get2To10(bit.Substring(6, 2));

                            //byte[1],byte[2]       BMK 2035-12-07 修改为  电机目标扭矩
                            //电机实际输出扭矩 
                            session.cls_hme.Motor_OutputTorque = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[2], 1) + PublicMethods.Get16To2(byte0[1], 1)) * 0.01f - 327, 2);
                            //byte[5]     
                            bit = PublicMethods.Get16To2(byte0[5], 1);
                            //电机正反转状态
                            session.cls_hme.Motor_Forward_Reverse = PublicMethods.Get2To10(bit.Substring(4, 2));
                            //电机牵引制动状态
                            session.cls_hme.Motor_Traction_Brake = PublicMethods.Get2To10(bit.Substring(6, 2));

                            break;
                        }
                    #endregion
                    #region 0CF50EEF  √
                    case "0CF50EEF":
                        {
                            //byte[3],byte[4]       
                            //直流电压
                            session.cls_hme.Motor_Direct_Voltage = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[4], 1) + PublicMethods.Get16To2(byte0[3], 1)) * 0.1d, 2);
                            //byte[5],byte[6]       
                            //交流电流IA
                            session.cls_hme.Motor_IA = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[6], 1) + PublicMethods.Get16To2(byte0[5], 1)) * 0.1d - 400, 2);

                            break;
                        }
                    #endregion
                    #region 18F532EF  √
                    case "18F532EF":
                        {
                            //byte[0],byte[1]       
                            //电机交流电流IB
                            session.cls_hme.Motor_IB = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[1], 1) + PublicMethods.Get16To2(byte0[0], 1)) * 0.1d - 400, 2);
                            //byte[2],byte[3]       
                            //电机交流电流IC
                            session.cls_hme.Motor_IC = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[3], 1) + PublicMethods.Get16To2(byte0[2], 1)) * 0.1d - 400, 2);
                            //byte[4],byte[5]       
                            //电机交流电压UA
                            session.cls_hme.Motor_UA = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[5], 1) + PublicMethods.Get16To2(byte0[4], 1)) * 0.1d, 2);
                            //byte[6],byte[7]       
                            //电机交流电压UB
                            session.cls_hme.Motor_UB = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[7], 1) + PublicMethods.Get16To2(byte0[6], 1)) * 0.1d, 2);
                            break;
                        }
                    #endregion
                    #region 18F53CEF  √
                    case "18F53CEF":
                        {
                            //byte[0],byte[1]       
                            //电机交流电压UC
                            session.cls_hme.Motor_UC = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[1], 1) + PublicMethods.Get16To2(byte0[0], 1)) * 0.1d, 2);
                            break;
                        }
                    #endregion
                    #region 18F665F3  √
                    case "18F665F3":
                        {
                            //byte[0]
                            bit = PublicMethods.Get16To2(byte0[0], 1);
                            //电池充放电状态
                            session.cls_hme.BMS_State_Charge_Discharge = int.Parse(bit.Substring(1, 1));
                            break;
                        }
                    #endregion
                    #region 18F666F3  √
                    case "18F666F3":
                        {
                            //byte[6],byte[7]       
                            //预充电压
                            session.cls_hme.BMS_Pre_Voltage = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[7], 1) + PublicMethods.Get16To2(byte0[6], 1)) * 0.1d, 2);
                            break;
                        }
                    #endregion
                    #region 18F669F3  √
                    case "18F669F3":
                        {
                            //byte[0]
                            //电池最大允许放电功率
                            session.cls_hme.BMS_Battery_Discharge_KW_MA = PublicMethods.Get16To10(byte0[0]);
                            //byte[1]
                            //电池最大允许充电功率
                            session.cls_hme.BMS_Battery_Charge_KW_MAX = PublicMethods.Get16To10(byte0[1]);
                            //byte[2],byte[3]       
                            //电池最大允许放电电流
                            session.cls_hme.BMS_Battery_Discharge_A_MAX = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[3], 1) + PublicMethods.Get16To2(byte0[2], 1)) * 0.1d, 2);

                            //byte[4],byte[5]       
                            //电池最大允许充电电流
                            session.cls_hme.BMS_Battery_Charge_A_MAX = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[5], 1) + PublicMethods.Get16To2(byte0[4], 1)) * 0.1d, 2);
                            //byte[6]
                            bit = PublicMethods.Get16To2(byte0[6], 1);
                            //非车载充电机充电状态
                            session.cls_hme.BMS_NOBC_State = PublicMethods.Get2To10(bit.Substring(2, 3));
                            //电池正极继电器状态
                            session.cls_hme.BMS_Positive_Relay = int.Parse(bit.Substring(5, 1));
                            //电池负极继电器状态
                            session.cls_hme.BMS_Negative_Relay = int.Parse(bit.Substring(6, 1));
                            //电池预充继电器状态
                            session.cls_hme.BMS_Pre_Charging_Relay = int.Parse(bit.Substring(7, 1));
                            break;
                        }
                    #endregion
                    #region 18F6DAF4  √
                    case "18F6DAF4":
                        {
                            //byte[0],byte[1]       
                            //最高允许充电电压
                            session.cls_hme.BMS_Battery_Charge_V_MAX = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[1], 1) + PublicMethods.Get16To2(byte0[0], 1)) * 0.1d, 2);

                            //byte[2],byte[3]       
                            //最高允许充电电流
                            session.cls_hme.BMS_Battery_Charge_A_MAX = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[3], 1) + PublicMethods.Get16To2(byte0[2], 1)) * 0.1d - 400, 2);

                            //byte[4]
                            bit = PublicMethods.Get16To2(byte0[4], 1);
                            //充电控制指令
                            session.cls_hme.BMS_Charge_ControlOrder = PublicMethods.Get2To10(bit.Substring(0, 1));

                            //byte[5],byte[6]       
                            //充电电流
                            session.cls_hme.ONC_Charge_Current = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[6], 1) + PublicMethods.Get16To2(byte0[5], 1)) * 0.1d - 400, 2);
                            break;
                        }
                    #endregion
                    #region 18F7D9E5  √
                    case "18F7D9E5":
                        {
                            //byte[0],byte[1]       
                            //充电机输出电压
                            session.cls_hme.ONC_OutputVoltage = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[1], 1) + PublicMethods.Get16To2(byte0[0], 1)) * 0.1d, 2);

                            //byte[2],byte[3]       
                            //充电机输出电流
                            session.cls_hme.ONC_OutputCurrent = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[3], 1) + PublicMethods.Get16To2(byte0[2], 1)) * 0.1d - 400, 2);

                            //byte[4]
                            bit = PublicMethods.Get16To2(byte0[4], 1);
                            //充电机温度报警
                            session.cls_hme.ONC_Temp_Alarm = PublicMethods.Get2To10(bit.Substring(1, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.ONC_Temp_Alarm;
                            if (faultint > 0)
                                canfault += string.Format("$ONC_Temp_Alarm::{0}:{1}", faultlever, faultint);

                            //充电机输入电压故障
                            session.cls_hme.ONC_InputVoltage_Fault = PublicMethods.Get2To10(bit.Substring(2, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.ONC_InputVoltage_Fault;
                            if (faultint > 0)
                                canfault += string.Format("$ONC_InputVoltage_Fault::{0}:{1}", faultlever, faultint);

                            //充电机启动状态
                            session.cls_hme.ONC_Start_Stop = PublicMethods.Get2To10(bit.Substring(3, 1));
                            //充电机通信状态
                            session.cls_hme.ONC_CommunicationSt = PublicMethods.Get2To10(bit.Substring(4, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.ONC_CommunicationSt;
                            if (faultint > 0)
                                canfault += string.Format("$ONC_CommunicationSt::{0}:{1}", faultlever, faultint);

                            //byte[5]
                            //充电机最高温度
                            session.cls_hme.ONC_Highest_Temp = PublicMethods.Get16To10(byte0[5]) - 40;

                            //byte[6],byte[7]       
                            //充电机最大充电电流
                            session.cls_hme.ONC_Charge_Maxallow_A = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[7], 1) + PublicMethods.Get16To2(byte0[6], 1)) * 0.1d - 400, 2);
                            break;
                        }
                    #endregion
                    #region 1CF7DCE5  √
                    case "1CF7DCE5":
                        {
                            //byte[2],byte[3]       
                            //充电机输入电流
                            session.cls_hme.ONC_Input_A = Math.Round(PublicMethods.Get2To10(PublicMethods.Get16To2(byte0[3], 1) + PublicMethods.Get16To2(byte0[2], 1)) * 0.1d - 400, 2);
                            //byte[4] 
                            //充电机最低温度
                            session.cls_hme.ONC_Lowest_Temp = PublicMethods.Get16To10(byte0[4]) - 40;
                            //byte[5]
                            //充电机中间温度
                            session.cls_hme.ONC_Middle_Temp = PublicMethods.Get16To10(byte0[5]) - 40;

                            //byte[6]
                            bit = PublicMethods.Get16To2(byte0[6], 1);
                            //充电机输入过压故障
                            session.cls_hme.ONC_Input_OverVoltage_Fault = PublicMethods.Get2To10(bit.Substring(0, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.ONC_Input_OverVoltage_Fault;
                            if (faultint > 0)
                                canfault += string.Format("$ONC_Input_OverVoltage_Fault::{0}:{1}", faultlever, faultint);

                            //充电机输入欠压故障
                            session.cls_hme.ONC_Input_LowVoltage_Fault = PublicMethods.Get2To10(bit.Substring(1, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.ONC_Input_LowVoltage_Fault;
                            if (faultint > 0)
                                canfault += string.Format("$ONC_Input_LowVoltage_Fault::{0}:{1}", faultlever, faultint);

                            //充电机输出过压故障
                            session.cls_hme.ONC_Output_OverVoltage_Fault = PublicMethods.Get2To10(bit.Substring(2, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.ONC_Output_OverVoltage_Fault;
                            if (faultint > 0)
                                canfault += string.Format("$ONC_Output_OverVoltage_Fault::{0}:{1}", faultlever, faultint);

                            //充电机输出欠压故障
                            session.cls_hme.ONC_Output_LowVoltage_Fault = PublicMethods.Get2To10(bit.Substring(3, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.ONC_Output_LowVoltage_Fault;
                            if (faultint > 0)
                                canfault += string.Format("$ONC_Output_LowVoltage_Fault::{0}:{1}", faultlever, faultint);

                            break;
                        }
                    #endregion
                    #region 18F918D6  √
                    case "18F918D6":
                        {
                            //byte[0]
                            bit = PublicMethods.Get16To2(byte0[0], 1);
                            //DCDC工作状态
                            session.cls_hme.DCDC_Work_State = PublicMethods.Get2To10(bit.Substring(0, 1));
                            //DCDC输出反接故障
                            session.cls_hme.DCDC_Output_Inversed_Fault = PublicMethods.Get2To10(bit.Substring(1, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.DCDC_Output_Inversed_Fault;
                            if (faultint > 0)
                                canfault += string.Format("$DCDC_Output_Inversed_Fault::{0}:{1}", faultlever, faultint);
                            session.AddLisFault("01,05," + faultlever + "," + faultint);
                            //DCDC硬件故障
                            session.cls_hme.DCDC_Hardware_Fault = PublicMethods.Get2To10(bit.Substring(2, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.DCDC_Hardware_Fault;
                            if (faultint > 0)
                                canfault += string.Format("$DCDC_Hardware_Fault::{0}:{1}", faultlever, faultint);
                            session.AddLisFault("01,05," + faultlever + "," + faultint);

                            //DCDC过温故障
                            session.cls_hme.DCDC_OverTemp_Fault = PublicMethods.Get2To10(bit.Substring(3, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.DCDC_OverTemp_Fault;
                            if (faultint > 0)
                                canfault += string.Format("$DCDC_OverTemp_Fault::{0}:{1}", faultlever, faultint);
                            session.AddLisFault("01,04," + faultlever + "," + faultint);
                            //DCDC输入故障
                            session.cls_hme.DCDC_Input_Fault = PublicMethods.Get2To10(bit.Substring(4, 2));
                            faultlever = 1;
                            faultint = session.cls_hme.DCDC_Input_Fault;
                            if (faultint > 0)
                                canfault += string.Format("$DCDC_Input_Fault::{0}:{1}", faultlever, faultint);
                            session.AddLisFault("01,05," + faultlever + "," + faultint);
                            //DCDC输出故障
                            session.cls_hme.DCDC_Output_Fault = PublicMethods.Get2To10(bit.Substring(6, 2));
                            faultlever = 1;
                            faultint = session.cls_hme.DCDC_Output_Fault;
                            if (faultint > 0)
                                canfault += string.Format("$DCDC_Output_Fault::{0}:{1}", faultlever, faultint);
                            session.AddLisFault("01,05," + faultlever + "," + faultint);
                            //byte[1]
                            //DCDC输出电流
                            session.cls_hme.DCDC_OutputCurrent = PublicMethods.Get16To10(byte0[1]);
                            //byte[2]
                            //DCDC输出电压
                            session.cls_hme.DCDC_OutputVoltage = Math.Round(PublicMethods.Get16To10(byte0[2]) * 0.14d, 2);
                            //byte[3]
                            //DCDC温度
                            session.cls_hme.DCDC_Temperature = PublicMethods.Get16To10(byte0[3]) - 40;

                            break;
                        }
                    #endregion
                    #region 18FA78F8  √
                    case "18FA78F8":
                        {
                            //byte[0]
                            //压缩机目标转速
                            session.cls_hme.HCL_Target_rpm = PublicMethods.Get16To10(byte0[0]) * 30;
                            //byte[1]
                            //蒸发器温度
                            session.cls_hme.HCL_Evaporator_Temp = PublicMethods.Get16To10(byte0[1]) - 40;
                            //byte[2]
                            //日照温度
                            session.cls_hme.HCL_Sunshine_intensity = PublicMethods.Get16To10(byte0[2]) - 40;
                            //byte[3]
                            //车内温度
                            session.cls_hme.HCL_Inside_Temp = PublicMethods.Get16To10(byte0[3]) - 40;
                            //byte[4]
                            //环境温度
                            session.cls_hme.HCL_Environment_Temp = PublicMethods.Get16To10(byte0[4]) - 40;

                            //byte[5]
                            bit = PublicMethods.Get16To2(byte0[5], 1);
                            //压缩机开关命令
                            session.cls_hme.HCL_Start_Order = PublicMethods.Get2To10(bit.Substring(0, 1));
                            //PTC温度
                            session.cls_hme.HCL_PTC_Temp = PublicMethods.Get2To10(bit.Substring(1)) + 20;

                            //byte[6]
                            //最大允许压缩机消耗的功率
                            session.cls_hme.HCL_Comp_allowMaxKW = Math.Round(PublicMethods.Get16To10(byte0[6]) * 0.1d, 2);
                            //byte[7]
                            //空调设定温度
                            session.cls_hme.HCL_Set_Temp = PublicMethods.Get16To10(byte0[7]) - 40;

                            break;
                        }
                    #endregion
                    #region 18FD7BEE  √
                    case "18FD7BEE":
                        {
                            //byte[0]
                            //压缩机当前转速
                            session.cls_hme.CCL_Speed = PublicMethods.Get16To10(byte0[0]) * 30;
                            //byte[1]
                            //压缩机当前相电流
                            session.cls_hme.CCL_Current = Math.Round(PublicMethods.Get16To10(byte0[1]) * 0.2d, 2);
                            //byte[2]
                            //压缩机当前电压
                            session.cls_hme.CCL_Voltage = PublicMethods.Get16To10(byte0[2]) * 2;
                            //byte[3]
                            //压缩机当前功率
                            session.cls_hme.CCL_KW = PublicMethods.Get16To10(byte0[3]) * 0.1f;
                            //byte[4]
                            //压缩机当前温度
                            session.cls_hme.CCL_Temp = PublicMethods.Get16To10(byte0[4]) - 40;

                            //byte[5]
                            bit = PublicMethods.Get16To2(byte0[5], 1);
                            //压缩机故障
                            session.cls_hme.CCL_Fault = PublicMethods.Get2To10(bit.Substring(1, 3));
                            //压缩机故障等级
                            session.cls_hme.CCL_Fault_Rate = PublicMethods.Get2To10(bit.Substring(4, 3));

                            faultlever = session.cls_hme.CCL_Fault_Rate;
                            if (faultlever > 4)
                            {
                                faultint = session.cls_hme.CCL_Fault;
                                if (faultint > 0)
                                    canfault += string.Format("${0}:{1}:{2}:{3}", "CCL_Fault", "", faultlever, faultint);
                            }
                            //压缩机使能状态
                            session.cls_hme.CCL_Enabled_State = PublicMethods.Get2To10(bit.Substring(7, 1));

                            break;
                        }
                    #endregion
                    #region 18FB6EF5  √
                    case "18FB6EF5":
                        {
                            //byte[0]
                            bit = PublicMethods.Get16To2(byte0[0], 1);
                            //12V蓄电池电压过低
                            session.cls_hme.PCTL_12V_L = PublicMethods.Get2To10(bit.Substring(2, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.PCTL_12V_L;
                            if (faultint > 0)
                                canfault += string.Format("$PCTL_12V_L::{0}:{1}", faultlever, faultint);
                            session.AddLisFault("01,01," + faultlever + "," + faultint);

                            //12V蓄电池电压过高
                            session.cls_hme.PCTL_12V_H = PublicMethods.Get2To10(bit.Substring(3, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.PCTL_12V_H;
                            if (faultint > 0)
                                canfault += string.Format("$PCTL_12V_H::{0}:{1}", faultlever, faultint);
                            session.AddLisFault("01,01," + faultlever + "," + faultint);
                            //P档系统当前状态
                            session.cls_hme.PCTL_State = PublicMethods.Get2To10(bit.Substring(4, 3));
                            //P档控制器初始化状态
                            session.cls_hme.PCTL_Initialization = PublicMethods.Get2To10(bit.Substring(7, 1));

                            //byte[1]
                            bit = PublicMethods.Get16To2(byte0[1], 1);
                            //执行电机开路故障
                            session.cls_hme.PCTL_Motor_Open_Circuit = PublicMethods.Get2To10(bit.Substring(0, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.PCTL_Motor_Open_Circuit;
                            if (faultint > 0)
                                canfault += string.Format("$PCTL_Motor_Open_Circuit::{0}:{1}", faultlever, faultint);
                            session.AddLisFault("01,03," + faultlever + "," + faultint);
                            //执行电机对电源短路故障
                            session.cls_hme.PCTL_Motor_Power_Short_Circuit = PublicMethods.Get2To10(bit.Substring(1, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.PCTL_Motor_Power_Short_Circuit;
                            if (faultint > 0)
                                canfault += string.Format("$PCTL_Motor_Power_Short_Circuit::{0}:{1}", faultlever, faultint);
                            session.AddLisFault("01,03," + faultlever + "," + faultint);

                            //执行电机对地短路故障
                            session.cls_hme.PCTL_Motor_Ground_Short_Circuit = PublicMethods.Get2To10(bit.Substring(2, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.PCTL_Motor_Ground_Short_Circuit;
                            if (faultint > 0)
                                canfault += string.Format("$PCTL_Motor_Ground_Short_Circuit::{0}:{1}", faultlever, faultint);
                            session.AddLisFault("01,03," + faultlever + "," + faultint);
                            //P档退档超时故障
                            session.cls_hme.PCTL_P_Back_Timeout = PublicMethods.Get2To10(bit.Substring(3, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.PCTL_P_Back_Timeout;
                            if (faultint > 0)
                                canfault += string.Format("$PCTL_P_Back_Timeout::{0}:{1}", faultlever, faultint);
                            session.AddLisFault("04,0F," + faultlever + "," + faultint);
                            //P档进档超时故障
                            session.cls_hme.PCTL_P_Into_Timeout = PublicMethods.Get2To10(bit.Substring(4, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.PCTL_P_Into_Timeout;
                            if (faultint > 0)
                                canfault += string.Format("$PCTL_P_Into_Timeout::{0}:{1}", faultlever, faultint);
                            session.AddLisFault("04,0F," + faultlever + "," + faultint);
                            //P档位置传感器短路故障
                            session.cls_hme.PCTL_P_Sensor_Short_Circuit = PublicMethods.Get2To10(bit.Substring(5, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.PCTL_P_Sensor_Short_Circuit;
                            if (faultint > 0)
                                canfault += string.Format("$PCTL_P_Sensor_Short_Circuit::{0}:{1}", faultlever, faultint);
                            session.AddLisFault("04,0F," + faultlever + "," + faultint);
                            //P档位置传感器开路故障
                            session.cls_hme.PCTL_P_Sensor_Open_Circuit = PublicMethods.Get2To10(bit.Substring(6, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.PCTL_P_Sensor_Open_Circuit;
                            if (faultint > 0)
                                canfault += string.Format("$PCTL_P_Sensor_Open_Circuit::{0}:{1}", faultlever, faultint);
                            session.AddLisFault("04,0F," + faultlever + "," + faultint);
                            //CAN通讯故障
                            session.cls_hme.PCTL_Can_Fault = PublicMethods.Get2To10(bit.Substring(7, 1));
                            faultlever = 1;
                            faultint = session.cls_hme.PCTL_Can_Fault;
                            if (faultint > 0)
                                canfault += string.Format("$PCTL_Can_Fault::{0}:{1}", faultlever, faultint);


                            break;
                        }
                    #endregion

                    #region 1C01A5F3  √-------------------供应商简码
                    case "1C01A5F3":
                        {
                            //byte[0]-byte[4]
                            //供应商简码
                            session.cls_vlsei.SupplierCode = PublicMethods.GetHexToAscii(string.Format("{0} {1} {2} {3} {4}", byte0[0], byte0[1], byte0[2], byte0[3], byte0[4]));
                            break;
                        }
                    #endregion
                    #region 1C02A5F3  √
                    case "1C02A5F3":
                        {
                            //byte[0]-byte[4]
                            //零件简码
                            session.cls_vlsei.PartCode = PublicMethods.GetHexToAscii(string.Format("{0} {1} {2} {3} {4}", byte0[0], byte0[1], byte0[2], byte0[3], byte0[4]));
                            break;
                        }
                    #endregion
                    #region 1C03A5F3  √
                    case "1C03A5F3":
                        {
                            //byte[0]-byte[4]
                            //生产日期
                            string[] months = { "0A", "0B", "0C" };
                            string month = months.Contains(byte0[2]) ? PublicMethods.GetHexToAscii(byte0[2]) : byte0[2].PadLeft(2, '0');
                            session.cls_vlsei.ProductionDate = string.Format("20{0}-{1}-{2}", byte0[3] + byte0[4], month, byte0[0] + byte0[1]);
                            //byte[5]-byte[7]       
                            //生产流水号
                            session.cls_vlsei.ProductionSerialNumber = byte0[5] + byte0[6] + byte0[7].Substring(0, 1);
                            break;
                        }
                    #endregion

                    #region 1401DCD0  √
                    case "1401DCD0":
                        {
                            //byte[0]
                            //车辆类型
                            session.cls_vlsei.VehicleType = PublicMethods.GetHexToAscii(byte0[0]);
                            //byte[1]
                            //主参数
                            session.cls_vlsei.MainParameters = PublicMethods.GetHexToAscii(byte0[1]);
                            //byte[2]
                            //车身或驾驶室类型
                            session.cls_vlsei.BodyOrCabType = PublicMethods.GetHexToAscii(byte0[2]);
                            //byte[3]
                            //轴距、驱动型式
                            session.cls_vlsei.WheelbaseDriveType = PublicMethods.GetHexToAscii(byte0[3]);
                            //byte[4]
                            //驱动电机功率
                            session.cls_vlsei.DrivingMotorPower = PublicMethods.GetHexToAscii(byte0[4]);
                            //byte[5]
                            //检验位
                            session.cls_vlsei.TestPosition = PublicMethods.GetHexToAscii(byte0[5]);
                            //byte[6]
                            //生产年份
                            session.cls_vlsei.YearOfProduction = PublicMethods.GetHexToAscii(byte0[6]);
                            //byte[7]
                            //装配线
                            session.cls_vlsei.AssemblyLine = PublicMethods.GetHexToAscii(byte0[7]);

                            break;
                        }
                    #endregion
                    #region 1402DCD0  √
                    case "1402DCD0":
                        {
                            //byte[0]
                            //生产顺序号1
                            session.cls_vlsei.ProductionOrderNumber1 = PublicMethods.GetHexToAscii(byte0[0]);
                            //byte[1]
                            //生产顺序号2
                            session.cls_vlsei.ProductionOrderNumber2 = PublicMethods.GetHexToAscii(byte0[1]);
                            //byte[2]
                            //生产顺序号3
                            session.cls_vlsei.ProductionOrderNumber3 = PublicMethods.GetHexToAscii(byte0[2]);
                            //byte[3]
                            //生产顺序号4
                            session.cls_vlsei.ProductionOrderNumber4 = PublicMethods.GetHexToAscii(byte0[3]);
                            //byte[4]
                            //生产顺序号5
                            session.cls_vlsei.ProductionOrderNumber5 = PublicMethods.GetHexToAscii(byte0[4]);
                            //byte[5]
                            //生产顺序号6
                            session.cls_vlsei.ProductionOrderNumber6 = PublicMethods.GetHexToAscii(byte0[5]);
                            break;
                        }
                    #endregion
                    #region 1403DCD0  √
                    case "1403DCD0":
                        {
                            //byte[0]
                            bit = PublicMethods.Get16To2(byte0[0], 1);
                            //VIN匹配回应请求
                            session.cls_vlsei.VINMatchingRequest = int.Parse(bit.Substring(0, 1));
                            break;
                        }
                    #endregion
                    #region 1405D0DC  √
                    case "1405D0DC":
                        {
                            //byte[0]
                            bit = PublicMethods.Get16To2(byte0[0], 1);
                            //VIN发送请求
                            session.cls_vlsei.VINSendingRequest = PublicMethods.Get2To10(bit.Substring(0, 1));
                            //VIN接收回应
                            session.cls_vlsei.VINReceivingResponse = PublicMethods.Get2To10(bit.Substring(1, 2));
                            //VIN匹配回应
                            session.cls_vlsei.VINMatchingResponse = PublicMethods.Get2To10(bit.Substring(3, 2));

                            break;
                        }
                    #endregion
                    default:
                        {
                            break;
                        }
                }
                #endregion

            }

            return session.cls_hme;
        }
    }
}
