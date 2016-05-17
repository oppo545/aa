using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ServerCenterLis
{
    /// <summary>
    /// 锐骐
    /// </summary>
    public class ParsZzRQ
    {
        /// 故障值  0正常         /// 故障等级    1	轻微故障,2	一般故障,3	严重故障,4	致命故障
        static int faultlever = 1, faultint = 0;
        static string str_data1, str_data2, str_data3, str_data4, str_data;
        static Regex regex = new Regex("\\s+");

        static int data1 = 0, data2 = 0, data3 = 0;
        public static void GetPars(string[] infos, ref string canfault, ref Candata_ZzRuiQi cd_zzrq)
        {
            for (int i = 0; i < infos.Length - 1; i++)
            {
                string bs = infos[i].Substring(0, 11);
                string bjbz = "00000000";//默认全0二进制
                string str_all = infos[i].Substring(12);
                //1:起始从1开始,"02 01":包含的整个字节,1:开始字节,0:起始位,1:长度
                #region ZZRQ
                switch (bs)
                {
                    //控制系统故障报警灯//运行准备就绪指示灯//动力蓄电池充电状态指示灯//电机及控制器过热报警灯//动力蓄电池故障报警灯//充电线连接指示灯//动力蓄电池切断指示灯
                    //超速报警指示灯//档位D指示灯//档位R指示灯//档位N指示灯//EPS指示灯//低压蓄电池亏电报警指示灯//手制动报警灯//动力蓄电池亏电指示灯
                    case "08 F6 03 01":

                        str_data1 = str_all.Substring(0, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.VCU_ControllerSystemFaultAlarmLamp = int.Parse(bjbz.Substring(7, 1));
                        cd_zzrq.VCU_ReadyLamp = int.Parse(bjbz.Substring(6, 1));
                        cd_zzrq.VCU_PowerStorageBatteryChargingStatusLamp = int.Parse(bjbz.Substring(5, 1));
                        cd_zzrq.VCU_MotorControlsOverheatingAlarmLamp = int.Parse(bjbz.Substring(4, 1));
                        cd_zzrq.VCU_PowerStorageBatteryFaultAlarmLamp = int.Parse(bjbz.Substring(3, 1));
                        cd_zzrq.VCU_ChargeLight = int.Parse(bjbz.Substring(2, 1));
                        cd_zzrq.VCU_PowerStorageBatteryCutOffLamp = int.Parse(bjbz.Substring(1, 1));
                        cd_zzrq.VCU_OverSpeedAlarmLamp = int.Parse(bjbz.Substring(0, 1));

                        str_data1 = str_all.Substring(3, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.VCU_GearsDLamp = int.Parse(bjbz.Substring(7, 1));
                        cd_zzrq.VCU_GearsRLamp = int.Parse(bjbz.Substring(6, 1));
                        cd_zzrq.VCU_GearsNLamp = int.Parse(bjbz.Substring(5, 1));
                        cd_zzrq.VCU_EPSLamp = int.Parse(bjbz.Substring(4, 1));
                        cd_zzrq.VCU_LowBatteryLossAlarmLamp = int.Parse(bjbz.Substring(3, 1));
                        cd_zzrq.VCU_HandBrakeWarningLight = int.Parse(bjbz.Substring(2, 1));
                        cd_zzrq.VCU_PowerStorageBatteryLossLamp = int.Parse(bjbz.Substring(1, 1));

                        str_data1 = str_all.Substring(15, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.VCU_lowvacuum = int.Parse(bjbz.Substring(7, 1));
                        cd_zzrq.VCU_PTCSwitch = int.Parse(bjbz.Substring(6, 1));
                        cd_zzrq.VCU_ACSwitch = int.Parse(bjbz.Substring(5, 1));
                        cd_zzrq.VCU_Vacuumpumpcapability = int.Parse(bjbz.Substring(4, 1));
                        cd_zzrq.VCU_SteeringCapability = int.Parse(bjbz.Substring(3, 1));
                        cd_zzrq.VCU_DCDCCapability = int.Parse(bjbz.Substring(2, 1));

                        str_data1 = str_all.Substring(18, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.VCU_Prechargingcompleted = int.Parse(bjbz.Substring(3, 1));
                        cd_zzrq.VCU_VehicleWorkingMode = int.Parse(bjbz.Substring(2, 1));

                        str_data1 = str_all.Substring(21, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.VCU_BrakePedal2Error = int.Parse(bjbz.Substring(6, 1));
                        faultint = cd_zzrq.VCU_BrakePedal2Error;
                        faultlever = 3;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "VCU_BrakePedal2Error", "0X08F60301-0X" + str_data1 + "-bit1", faultlever, faultint);
                        cd_zzrq.VCU_BrakePedal1Error = int.Parse(bjbz.Substring(5, 1));
                        faultint = cd_zzrq.VCU_BrakePedal1Error;
                        faultlever = 3;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "VCU_BrakePedal1Error", "0X08F60301-0X" + str_data1 + "-bit2", faultlever, faultint);
                        cd_zzrq.VCU_Throttle2Failure = int.Parse(bjbz.Substring(3, 1));
                        faultint = cd_zzrq.VCU_Throttle2Failure;
                        faultlever = 3;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "VCU_Throttle2Failure", "0X08F60301-0X" + str_data1 + "-bit4", faultlever, faultint);
                        cd_zzrq.VCU_Throttle1Failure = int.Parse(bjbz.Substring(2, 1));
                        faultint = cd_zzrq.VCU_Throttle1Failure;
                        faultlever = 3;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "VCU_Throttle1Failure", "0X08F60301-0X" + str_data1 + "-bit5", faultlever, faultint);
                        cd_zzrq.VCU_SteeringFailure = int.Parse(bjbz.Substring(1, 1));
                        faultint = cd_zzrq.VCU_SteeringFailure;
                        faultlever = 3;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "VCU_SteeringFailure", "0X08F60301-0X" + str_data1 + "-bit6", faultlever, faultint);
                        cd_zzrq.VCU_PTCOverTemp = int.Parse(bjbz.Substring(0, 1));
                        faultint = cd_zzrq.VCU_PTCOverTemp;
                        faultlever = 3;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "VCU_PTCOverTemp", "0X08F60301-0X" + str_data1 + "-bit7", faultlever, faultint);
                        break;
                    case "0C F1 0B D0":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzrq.VCU_Keyposition = ParsMethod.GetParsBig(str_data1, 1, 0, 2);
                        cd_zzrq.VCU_BrakePedalStatus = ParsMethod.GetParsBig(str_data1, 1, 6, 1);

                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.VCU_StorageBatteryLowFault = ParsMethod.GetParsBig(str_data1, 2, 11, 1);
                        faultint = cd_zzrq.VCU_StorageBatteryLowFault;
                        faultlever = 1;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "VCU_StorageBatteryLowFault", "0X0CF10BD0-0X" + str_data1 + "-bit3", faultlever, faultint);
                        str_data1 = str_all.Substring(12, 2);
                        cd_zzrq.Motor_Throttle = ParsMethod.GetParsBig(str_data1, 5, 32, 8, 0.4);

                        break;
                    case "0C F1 0C D0":
                        str_data1 = str_all.Substring(0, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.VCU_FaultOperationMode = int.Parse(bjbz.Substring(7, 1));
                        cd_zzrq.VCU_ShiftGearsAllows = ParsMethod.GetValuebyBinary(str_data1, 3, 2);
                        cd_zzrq.VCU_BMUFault = int.Parse(bjbz.Substring(2, 1));
                        faultint = cd_zzrq.VCU_BMUFault;
                        faultlever = 1;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "VCU_BMUFault", "0X0CF10CD0-0X" + str_data1 + "-bit5", faultlever, faultint);
                        cd_zzrq.VCU_Fault = int.Parse(bjbz.Substring(1, 1));
                        faultint = cd_zzrq.VCU_Fault;
                        faultlever = 1;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "VCU_Fault", "0X0CF10CD0-0X" + str_data1 + "-bit6", faultlever, faultint);
                        cd_zzrq.VCU_MCUFault = int.Parse(bjbz.Substring(0, 1));
                        faultint = cd_zzrq.VCU_MCUFault;
                        faultlever = 1;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "VCU_MCUFault", "0X0CF10CD0-0X" + str_data1 + "-bit7", faultlever, faultint);

                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.VCU_MotorWorkingMode = ParsMethod.GetParsBig(str_data1, 2, 14, 2);

                        str_data1 = str_all.Substring(6, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.VCU_ClearHistoricalFault = int.Parse(bjbz.Substring(4, 1));
                        cd_zzrq.VCU_ClearCurrentFault = int.Parse(bjbz.Substring(3, 1));
                        cd_zzrq.VCU_SelfStudyAllows = int.Parse(bjbz.Substring(2, 1));
                        cd_zzrq.VCU_GearboxIdlingInstructions = int.Parse(bjbz.Substring(1, 1));
                        cd_zzrq.VCU_ClutchSeparateInstructions = int.Parse(bjbz.Substring(0, 1));

                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.VCU_KRatio = ParsMethod.GetParsBig(str_data1, 4, 24, 16, 0.1);
                        break;
                    case "08 F5 41 01":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzrq.ONC_Start_Stop = ParsMethod.GetParsBig(str_data1, 1, 0, 2);
                        break;
                    case "0C EF 00 D0":
                        str_data1 = str_all.Substring(0, 5);
                        cd_zzrq.VCU_Revolution = ParsMethod.GetParsBig(str_data1, 1, 0, 16);
                        str_data1 = str_all.Substring(6, 5);
                        cd_zzrq.VCU_MotorTorque = ParsMethod.GetParsBig(str_data1, 3, 16, 16, 0.1);

                        str_data1 = str_all.Substring(12, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.VCU_MotorDrivenInstruction = int.Parse(bjbz.Substring(7, 1));
                        cd_zzrq.VCU_MotorDirectionOfRotation = int.Parse(bjbz.Substring(5, 1));
                        cd_zzrq.VCU_SpeedTorqueSwitch = int.Parse(bjbz.Substring(3, 1));
                        cd_zzrq.VCU_MotorReset = int.Parse(bjbz.Substring(2, 1));
                        str_data1 = str_all.Substring(15, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.VCU_DCDCEnableSignal = int.Parse(bjbz.Substring(7, 1));
                        cd_zzrq.VCU_SteeringControllerReset = int.Parse(bjbz.Substring(3, 1));
                        cd_zzrq.VCU_SteeringControllerRunning = int.Parse(bjbz.Substring(2, 1));
                        cd_zzrq.VCU_BrakingSystemControllerReset = int.Parse(bjbz.Substring(1, 1));
                        cd_zzrq.VCU_BrakingSystemControllerRunning = int.Parse(bjbz.Substring(0, 1));
                        break;
                    case "18 01 D0 03":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzrq.TCU_TargetGear = ParsMethod.GetParsBig(str_data1, 1, 0, 3);
                        cd_zzrq.TCU_CurrentGear = ParsMethod.GetParsBig(str_data1, 1, 3, 3);
                        cd_zzrq.TCU_ClutchState = ParsMethod.GetParsBig(str_data1, 1, 6, 2);

                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.TCU_GearPosition = ParsMethod.GetParsBig(str_data1, 2, 8, 3);

                        str_data1 = str_all.Substring(6, 2);
                        cd_zzrq.TCU_Counter = ParsMethod.GetParsBig(str_data1, 3, 16, 8);

                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.TCU_InputShaftRotatingSpeed = ParsMethod.GetParsBig(str_data1, 4, 24, 16);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.TCU_OutputShaftRotatingSpeed = ParsMethod.GetParsBig(str_data1, 6, 40, 16);
                        str_data1 = str_all.Substring(21, 2);
                        cd_zzrq.ABS_VehSpd = ParsMethod.GetParsBig(str_data1, 8, 56, 8, 0.5);
                        break;
                    case "18 02 D0 03":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzrq.TCU_Fault = ParsMethod.GetValuebyBinary(str_data1, 0, 2);
                        faultint = cd_zzrq.TCU_Fault;
                        faultlever = 1;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "TCU_Fault", "0X1802D003-0X" + str_data1, faultlever, faultint);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.TCU_SelfStutyRequest = int.Parse(bjbz.Substring(5, 1));
                        cd_zzrq.TCU_ShiftGearsState = int.Parse(bjbz.Substring(4, 1));
                        cd_zzrq.TCU_ShiftGearsMode = int.Parse(bjbz.Substring(3, 1));
                        cd_zzrq.TCU_MotorFreePatternRequest = int.Parse(bjbz.Substring(2, 1));
                        cd_zzrq.TCU_RotatingSpeedRequestSignificantBit = int.Parse(bjbz.Substring(1, 1));
                        cd_zzrq.TCU_TorqueRequestSignificantBit = int.Parse(bjbz.Substring(0, 1));

                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.VCU_Motor_Target_rpm = ParsMethod.GetParsBig(str_data1, 2, 8, 16);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.VCU_Motor_Target_tq = ParsMethod.GetParsBig(str_data1, 4, 24, 16);
                        str_data1 = str_all.Substring(15, 2);
                        cd_zzrq.TCU_FaultCode = ParsMethod.GetParsBig(str_data1, 6, 40, 8);
                        str_data1 = str_all.Substring(18, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.TCU_NormalMovementPatterns = int.Parse(bjbz.Substring(5, 1));
                        cd_zzrq.TCU_BrakeSwitch = int.Parse(bjbz.Substring(4, 1));
                        cd_zzrq.TCU_PDSwitch = int.Parse(bjbz.Substring(3, 1));
                        cd_zzrq.TCU_SportSwitch = int.Parse(bjbz.Substring(2, 1));
                        cd_zzrq.TCU_SelfStudyConditionMet = int.Parse(bjbz.Substring(1, 1));
                        cd_zzrq.TCU_SelfStudyStatus = int.Parse(bjbz.Substring(0, 1));

                        break;
                    case "18 03 D0 03":
                        str_data1 = str_all.Substring(0, 11);
                        cd_zzrq.IC_TotalOdmeter = ParsMethod.GetParsBig(str_data1, 1, 0, 32);
                        break;
                    case "0C F0 04 EF":
                        str_data1 = str_all.Substring(0, 5);
                        cd_zzrq.Motor_Revolution = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(6, 5);
                        cd_zzrq.Motor_OutputTorque = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                        str_data1 = str_all.Substring(12, 5);
                        cd_zzrq.BusVoltage = ParsMethod.GetParsWholeByte(str_data1, 0.5);
                        str_data1 = str_all.Substring(18, 2);
                        cd_zzrq.MCU_MotorDrivenInstruction = ParsMethod.GetValuebyBinary(str_data1, 48, 2, 6);
                        cd_zzrq.MCU_MotorDirectionOfRotation = ParsMethod.GetValuebyBinary(str_data1, 50, 2, 6);
                        break;
                    case "0C F0 05 EF":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzrq.Motor_Temperature = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.Motor_Temperature2 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(6, 2);
                        cd_zzrq.BMS_Temperature = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(9, 2);
                        cd_zzrq.Motor_ControllerTemp2 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);

                        str_data1 = str_all.Substring(12, 5);
                        cd_zzrq.Motor_PhaseCurrent = ParsMethod.GetParsWholeByte(str_data1, 0.5);
                        str_data1 = str_all.Substring(18, 5);
                        cd_zzrq.Motor_ThreePhaseVoltage = ParsMethod.GetParsWholeByte(str_data1, 0.5);
                        break;
                    case "0C F0 06 EF":
                        str_data1 = str_all.Substring(0, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.MCU_Major_Fault = int.Parse(bjbz.Substring(7, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.MCU_Major_Fault;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_Major_Fault", "0X0CF006EF-0X" + str_data1 + "-bit0", faultlever, faultint);
                        cd_zzrq.MCU_Communication_Fault = int.Parse(bjbz.Substring(3, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.MCU_Communication_Fault;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_Communication_Fault", "0X0CF006EF-0X" + str_data1 + "-bit4", faultlever, faultint);
                        cd_zzrq.MCU_EncoderState = int.Parse(bjbz.Substring(2, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.MCU_EncoderState;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_EncoderState", "0X0CF006EF-0X" + str_data1 + "-bit5", faultlever, faultint);
                        cd_zzrq.MCU_BusVoltageState = int.Parse(bjbz.Substring(1, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.MCU_BusVoltageState;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_BusVoltageState", "0X0CF006EF-0X" + str_data1 + "-bit6", faultlever, faultint);

                        str_data1 = str_all.Substring(3, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.MCU_ControllerOverTempState = int.Parse(bjbz.Substring(7, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.MCU_ControllerOverTempState;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_ControllerOverTempState", "0X0CF006EF-0X" + str_data1 + "-bit0", faultlever, faultint);
                        cd_zzrq.MCU_EEPROMException = int.Parse(bjbz.Substring(6, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.MCU_EEPROMException;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_EEPROMException", "0X0CF006EF-0X" + str_data1 + "-bit1", faultlever, faultint);
                        cd_zzrq.MCU_ControllerOverload = int.Parse(bjbz.Substring(5, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.MCU_ControllerOverload;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_ControllerOverload", "0X0CF006EF-0X" + str_data1 + "-bit2", faultlever, faultint);
                        cd_zzrq.MCU_MotorOverload = int.Parse(bjbz.Substring(4, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.MCU_MotorOverload;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_MotorOverload", "0X0CF006EF-0X" + str_data1 + "-bit3", faultlever, faultint);
                        cd_zzrq.MCU_OverCurrentState = int.Parse(bjbz.Substring(3, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.MCU_OverCurrentState;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_OverCurrentState", "0X0CF006EF-0X" + str_data1 + "-bit4", faultlever, faultint);
                        cd_zzrq.MCU_CurrentSensorFault = int.Parse(bjbz.Substring(2, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.MCU_CurrentSensorFault;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_CurrentSensorFault", "0X0CF006EF-0X" + str_data1 + "-bit5", faultlever, faultint);
                        cd_zzrq.MCU_MotorThermalState = ParsMethod.GetValuebyBinary(str_data1, 14, 2, 1);
                        faultlever = 3;
                        faultint = cd_zzrq.MCU_MotorThermalState;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_MotorThermalState", "0X0CF006EF-0X" + str_data1, faultlever, faultint);

                        str_data1 = str_all.Substring(6, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.MCU_MinorFault = int.Parse(bjbz.Substring(7, 1));
                        faultlever = 1;
                        faultint = cd_zzrq.MCU_MinorFault;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_MinorFault", "0X0CF006EF-0X" + str_data1 + "-bit0", faultlever, faultint);
                        cd_zzrq.MCU_InverterOverheatingPrediction = int.Parse(bjbz.Substring(6, 1));
                        faultlever = 1;
                        faultint = cd_zzrq.MCU_InverterOverheatingPrediction;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_InverterOverheatingPrediction", "0X0CF006EF-0X" + str_data1 + "-bit1", faultlever, faultint);
                        cd_zzrq.MCU_FrequencyLoadForecasting = int.Parse(bjbz.Substring(5, 1));
                        faultlever = 1;
                        faultint = cd_zzrq.MCU_FrequencyLoadForecasting;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_FrequencyLoadForecasting", "0X0CF006EF-0X" + str_data1 + "-bit2", faultlever, faultint);
                        cd_zzrq.MCU_DCBusVoltageUnderVoltagePrediction = int.Parse(bjbz.Substring(4, 1));
                        faultlever = 1;
                        faultint = cd_zzrq.MCU_DCBusVoltageUnderVoltagePrediction;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_DCBusVoltageUnderVoltagePrediction", "0X0CF006EF-0X" + str_data1 + "-bit3", faultlever, faultint);
                        cd_zzrq.MCU_DCBusVoltageOverVoltagePrediction = int.Parse(bjbz.Substring(3, 1));
                        faultlever = 1;
                        faultint = cd_zzrq.MCU_DCBusVoltageOverVoltagePrediction;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_DCBusVoltageOverVoltagePrediction", "0X0CF006EF-0X" + str_data1 + "-bit4", faultlever, faultint);
                        cd_zzrq.MCU_CCWSupervelocity = int.Parse(bjbz.Substring(2, 1));
                        faultlever = 1;
                        faultint = cd_zzrq.MCU_CCWSupervelocity;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_CCWSupervelocity", "0X0CF006EF-0X" + str_data1 + "-bit5", faultlever, faultint);
                        cd_zzrq.MCU_CWSupervelocity = int.Parse(bjbz.Substring(1, 1));
                        faultlever = 1;
                        faultint = cd_zzrq.MCU_CWSupervelocity;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_CWSupervelocity", "0X0CF006EF-0X" + str_data1 + "-bit6", faultlever, faultint);

                        str_data1 = str_all.Substring(9, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.MCU_InverterReady = int.Parse(bjbz.Substring(7, 1));
                        cd_zzrq.MCU_PrechargeCompleteSignal = int.Parse(bjbz.Substring(6, 1));
                        cd_zzrq.MCU_Counter = ParsMethod.GetValuebyBinary(str_data1, 28, 4, 3);

                        str_data1 = str_all.Substring(12, 5);
                        cd_zzrq.MCU_TotalRunTime = ParsMethod.GetParsWholeByte(str_data1);
                        break;
                    case "18 18 17 F3":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzrq.BMS_BatteryNumber = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.BMS_TotalNumberOfBatteryModules = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(6, 5);
                        cd_zzrq.BMS_BatteryChargingTimes = ParsMethod.GetParsWholeByte(str_data1);

                        str_data1 = str_all.Substring(12, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.BMS_ChargeSt = int.Parse(bjbz.Substring(7, 1));

                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryInputOutputPower = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(21, 2);
                        cd_zzrq.BMS_BatterySystemBatteryBoxNumber = ParsMethod.GetParsWholeByte(str_data1);
                        break;
                    case "18 57 17 F3":
                        str_data1 = str_all.Substring(0, 11);
                        cd_zzrq.BMS_BatteryModuleOnlyNumber = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(12, 2);
                        cd_zzrq.BMS_ModuleNumberInBatteryPack = ParsMethod.GetParsWholeByte(str_data1);
                        break;
                    case "18 1D 17 F3":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzrq.BMS_BatteryModule = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.BMS_SingleBatteryModuleNumber = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(6, 2);
                        cd_zzrq.BMS_TheNumberOfTemperatureSamplingModule = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(9, 2);
                        cd_zzrq.BMS_SOCModule = ParsMethod.GetParsWholeByte(str_data1, 0.4);

                        str_data1 = str_all.Substring(12, 5);
                        cd_zzrq.BMS_ChargingModuleNumber = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(18, 5);
                        cd_zzrq.BMS_TotalCurrentModule = ParsMethod.GetParsWholeByte(str_data1, 0.1, -3200);
                        break;
                    case "18 1E 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_TotalVoltageModule = ParsMethod.GetParsWholeByte(str_data1, 0.02);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_ModuleOfMonomerInTheLowestVoltage = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_ModuleOfMonomerInTheHighestVoltage = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(21, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.BMS_OverVoltage = int.Parse(bjbz.Substring(7, 1));
                        cd_zzrq.BMS_UnderVoltage = int.Parse(bjbz.Substring(6, 1));
                        cd_zzrq.BMS_OverTemperature = int.Parse(bjbz.Substring(5, 1));
                        cd_zzrq.BMS_LackOfWarmth = int.Parse(bjbz.Substring(4, 1));
                        cd_zzrq.BMS_InternalCommunicationFault = int.Parse(bjbz.Substring(3, 1));
                        break;
                    case "18 1F 17 F3":
                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.BMS_LowestTemperatureModule = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(6, 2);
                        cd_zzrq.BMS_HighestTemperatureModule = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(9, 2);
                        cd_zzrq.BMS_LowestVoltageMonomer = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(12, 2);
                        cd_zzrq.BMS_HighestVoltageMonomer = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(15, 2);
                        cd_zzrq.BMS_LowestTemperatureSamplingPeriod = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(18, 2);
                        cd_zzrq.BMS_HighestTemperatureSamplingPeriod = ParsMethod.GetParsWholeByte(str_data1);
                        break;
                    case "18 F2 12 F3":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzrq.BMS_SOC = ParsMethod.GetParsWholeByte(str_data1,0.4);
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_Voltage = ParsMethod.GetParsWholeByte(str_data1, 0.02);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_Current = ParsMethod.GetParsWholeByte(str_data1, 0.1, -3200);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_VersionOfTheBMSProgramInformation = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(12, 2);
                        cd_zzrq.BMS_BatteryTypes = ParsMethod.GetValuebyBinary(str_data1, 60, 4, 7);
                        break;
                    case "18 F2 14 F3":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzrq.BMS_HighPressureLeakageAlarm = ParsMethod.GetValuebyBinary(str_data1, 0, 2);
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_HighPressureLeakageAlarm;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_HighPressureLeakageAlarm", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        cd_zzrq.BMS_HighTemperatureAlarm = ParsMethod.GetValuebyBinary(str_data1, 2, 3);
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_HighTemperatureAlarm;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_HighTemperatureAlarm", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        cd_zzrq.BMS_TemperatureDifferenceAlarm = ParsMethod.GetValuebyBinary(str_data1, 5, 3);
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_TemperatureDifferenceAlarm;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_TemperatureDifferenceAlarm", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.BMS_DischargeCurrentAlarm = ParsMethod.GetValuebyBinary(str_data1, 10, 3, 1);
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_DischargeCurrentAlarm;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_DischargeCurrentAlarm", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        cd_zzrq.BMS_ChargingCurrentAlarm = ParsMethod.GetValuebyBinary(str_data1, 13, 3, 1);
                        faultlever = 1;
                        faultint = cd_zzrq.MCU_CWSupervelocity;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_CWSupervelocity", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        str_data1 = str_all.Substring(6, 2);
                        cd_zzrq.BMS_BatteryPackOverVoltageAlarm = ParsMethod.GetValuebyBinary(str_data1, 18, 3, 2);
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_BatteryPackOverVoltageAlarm;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_BatteryPackOverVoltageAlarm", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        cd_zzrq.BMS_MonomerOverVoltageAlarm = ParsMethod.GetValuebyBinary(str_data1, 21, 3, 2);
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_MonomerOverVoltageAlarm;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_MonomerOverVoltageAlarm", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        str_data1 = str_all.Substring(9, 2);
                        cd_zzrq.BMS_SOCLowAlarm = ParsMethod.GetValuebyBinary(str_data1, 24, 2, 3);
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_SOCLowAlarm;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_SOCLowAlarm", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        cd_zzrq.BMS_SOCDifferenceAlarm = ParsMethod.GetValuebyBinary(str_data1, 26, 2, 3);
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_SOCDifferenceAlarm;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_SOCDifferenceAlarm", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        cd_zzrq.BMS_BatteryPackUnderVoltage = ParsMethod.GetValuebyBinary(str_data1, 28, 2, 3);
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_BatteryPackUnderVoltage;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_BatteryPackUnderVoltage", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        cd_zzrq.BMS_MonomerUnderVoltage = ParsMethod.GetValuebyBinary(str_data1, 30, 2, 3);
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_MonomerUnderVoltage;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_MonomerUnderVoltage", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        str_data1 = str_all.Substring(12, 2);
                        cd_zzrq.BMS_PoleHighTemperatureAlarm = ParsMethod.GetValuebyBinary(str_data1, 32, 2, 4);
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_PoleHighTemperatureAlarm;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_PoleHighTemperatureAlarm", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        cd_zzrq.BMS_BatteryLowTemperatureAlarm = ParsMethod.GetValuebyBinary(str_data1, 34, 2, 4);
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_BatteryLowTemperatureAlarm;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_BatteryLowTemperatureAlarm", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        cd_zzrq.BMS_MonomerVoltageDifferenceAlarm = ParsMethod.GetValuebyBinary(str_data1, 36, 2, 4);
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_MonomerVoltageDifferenceAlarm;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_MonomerVoltageDifferenceAlarm", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        cd_zzrq.BMS_SOCHighAlarm = ParsMethod.GetValuebyBinary(str_data1, 38, 2, 4);
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_SOCHighAlarm;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_SOCHighAlarm", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        str_data1 = str_all.Substring(15, 2);
                        cd_zzrq.BMS_CommunicationLifeCycleCounting = ParsMethod.GetValuebyBinary(str_data1, 40, 4, 5);
                        cd_zzrq.BMS_LECUCommunicationAlarm = ParsMethod.GetValuebyBinary(str_data1, 44, 1, 5);
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_LECUCommunicationAlarm;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_LECUCommunicationAlarm", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        cd_zzrq.BMS_DifferentBatteryAlarm = ParsMethod.GetValuebyBinary(str_data1, 46, 2, 5);
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_DifferentBatteryAlarm;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_DifferentBatteryAlarm", "0X18F214F3-0X" + str_data1, faultlever, faultint);

                        str_data1 = str_all.Substring(18, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.BMS_BatteryStatus = int.Parse(bjbz.Substring(7, 1));
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_BatteryStatus;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_BatteryStatus", "0X18F214F3-0X" + str_data1 + "-bit0", faultlever, faultint);

                        cd_zzrq.BMS_BatteryPowerLossStatus = int.Parse(bjbz.Substring(6, 1));
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_BatteryPowerLossStatus;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_BatteryPowerLossStatus", "0X18F214F3-0X" + str_data1 + "-bit1", faultlever, faultint);

                        cd_zzrq.BMS_PoleHighTemperatureState = int.Parse(bjbz.Substring(5, 1));
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_PoleHighTemperatureState;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_PoleHighTemperatureState", "0X18F214F3-0X" + str_data1 + "-bit2", faultlever, faultint);

                        cd_zzrq.BMS_BalancedState = int.Parse(bjbz.Substring(4, 1));
                        cd_zzrq.BMS_BalancedAlarmState = int.Parse(bjbz.Substring(3, 1));
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_BalancedAlarmState;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_BalancedAlarmState", "0X18F214F3-0X" + str_data1 + "-bit4", faultlever, faultint);

                        cd_zzrq.BMS_HeatingState = int.Parse(bjbz.Substring(2, 1));
                        cd_zzrq.BMS_HeatingAlarmState = int.Parse(bjbz.Substring(1, 1));
                        faultlever = 1;
                        faultint = cd_zzrq.BMS_HeatingAlarmState;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_HeatingAlarmState", "0X18F214F3-0X" + str_data1 + "-bit6", faultlever, faultint);

                        cd_zzrq.BMS_PowerBatteryForecast = int.Parse(bjbz.Substring(0, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_PowerBatteryForecast;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_PowerBatteryForecast", "0X18F214F3-0X" + str_data1 + "-bit7", faultlever, faultint);

                        break;
                    case "18 F2 15 F3":
                        str_data1 = str_all.Substring(0, 5);
                        cd_zzrq.BMS_DischargePower = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                        str_data1 = str_all.Substring(6, 5);
                        cd_zzrq.BMS_DischargeTime = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(12, 5);
                        cd_zzrq.BMS_DischargeCapacity = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                        str_data1 = str_all.Substring(18, 5);
                        cd_zzrq.BMS_ResidualCapacity = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                        break;
                    case "18 F2 16 F3":
                        str_data1 = str_all.Substring(0, 2);
                        //cd_zzrq.BMS_BatteryNumber = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(3, 5);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.BMS_HeatingModuleState = int.Parse(bjbz.Substring(7, 1));
                        cd_zzrq.BMS_HeatingModuleFaultState = int.Parse(bjbz.Substring(6, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_HeatingModuleFaultState;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_HeatingModuleFaultState", "0X18F216F3-0X" + str_data1 + "-bit1", faultlever, faultint);

                        cd_zzrq.BMS_InternalEquilibrium = int.Parse(bjbz.Substring(5, 1));
                        cd_zzrq.BMS_InternalEquilibriumFault = int.Parse(bjbz.Substring(4, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_InternalEquilibriumFault;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_InternalEquilibriumFault", "0X18F216F3-0X" + str_data1 + "-bit3", faultlever, faultint);

                        cd_zzrq.BMS_ExternalEquilibrium = int.Parse(bjbz.Substring(3, 1));
                        cd_zzrq.BMS_ExternalEquilibriumFault = int.Parse(bjbz.Substring(2, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ExternalEquilibriumFault;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ExternalEquilibriumFault", "0X18F216F3-0X" + str_data1 + "-bit5", faultlever, faultint);

                        break;
                    case "18 F2 17 F3":
                        str_data1 = str_all.Substring(0, 5);
                        cd_zzrq.BMS_ChargingTime = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(6, 5);
                        cd_zzrq.BMS_RemainingChargingTime = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(12, 5);
                        cd_zzrq.BMS_MaximumChargeCurrentLimit = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(18, 5);
                        cd_zzrq.BMS_MaximumDischargeCurrentLimit = ParsMethod.GetParsWholeByte(str_data1);
                        break;
                    #region 128节电池
                    case "18 24 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage1 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage2 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage3 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 25 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage4 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage5 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage6 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 26 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage7 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage8 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage9 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 27 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage10 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage11 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage12 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 28 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage13 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage14 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage15 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 29 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage16 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage17 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage18 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 2A 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage19 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage20 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage21 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 2B 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage22 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage23 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage24 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 2C 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage25 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage26 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage27 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 2D 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage28 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage29 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage30 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 2E 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage31 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage32 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage33 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 2F 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage34 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage35 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage36 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 30 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage37 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage38 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage39 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 31 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage40 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage41 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage42 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 32 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage43 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage44 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage45 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 33 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage46 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage47 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage48 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 34 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage49 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage50 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage51 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 35 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage52 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage53 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage54 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 36 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage55 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage56 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage57 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 37 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage58 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage59 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage60 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 38 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage61 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage62 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage63 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 39 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage64 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage65 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage66 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 3A 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage67 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage68 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage69 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 3B 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage70 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage71 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage72 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 3C 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage73 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage74 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage75 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 3D 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage76 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage77 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage78 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 3E 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage79 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage80 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage81 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 3F 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage82 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage83 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage84 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 40 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage85 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage86 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage87 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 41 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage88 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage89 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage90 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 42 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage91 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage92 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage93 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 43 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage94 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage95 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage96 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 44 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage97 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage98 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage99 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 45 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage100 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage101 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage102 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 46 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage103 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage104 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage105 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 47 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage106 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage107 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage108 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 48 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage109 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage110 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage111 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 49 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage112 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage113 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage114 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 4A 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage115 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage116 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage117 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 4B 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage118 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage119 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage120 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 4C 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage121 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage122 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage123 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 4D 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage124 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage125 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(15, 5);
                        cd_zzrq.BMS_BatteryVoltage126 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 4E 17 F3":
                        str_data1 = str_all.Substring(3, 5);
                        cd_zzrq.BMS_BatteryVoltage127 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(9, 5);
                        cd_zzrq.BMS_BatteryVoltage128 = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    #endregion
                    case "18 4F 17 F3":
                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.BMS_TemperatureSamplingModule1 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(6, 2);
                        cd_zzrq.BMS_TemperatureSamplingModule2 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(9, 2);
                        cd_zzrq.BMS_TemperatureSamplingModule3 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(12, 2);
                        cd_zzrq.BMS_TemperatureSamplingModule4 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(15, 2);
                        cd_zzrq.BMS_TemperatureSamplingModule5 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(18, 2);
                        cd_zzrq.BMS_TemperatureSamplingModule6 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(21, 2);
                        cd_zzrq.BMS_TemperatureSamplingModule7 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        break;
                    case "18 50 17 F3":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzrq.BMS_FramePositivePoleTemperature1 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.BMS_FrameNegativePoleTemperature1 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(6, 2);
                        cd_zzrq.BMS_FramePositivePoleTemperature2 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(9, 2);
                        cd_zzrq.BMS_FrameNegativePoleTemperature2 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(12, 2);
                        cd_zzrq.BMS_FramePositivePoleTemperature3 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(15, 2);
                        cd_zzrq.BMS_FrameNegativePoleTemperature3 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(18, 2);
                        cd_zzrq.BMS_FramePositivePoleTemperature4 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(21, 2);
                        cd_zzrq.BMS_FrameNegativePoleTemperature4 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        break;
                    case "18 51 17 F3":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzrq.BMS_FramePositivePoleTemperature5 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.BMS_FrameNegativePoleTemperature5 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(6, 2);
                        cd_zzrq.BMS_FramePositivePoleTemperature6 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(9, 2);
                        cd_zzrq.BMS_FrameNegativePoleTemperature6 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(12, 2);
                        cd_zzrq.BMS_FramePositivePoleTemperature7 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(15, 2);
                        cd_zzrq.BMS_FrameNegativePoleTemperature7 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(18, 2);
                        cd_zzrq.BMS_FramePositivePoleTemperature8 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(21, 2);
                        cd_zzrq.BMS_FrameNegativePoleTemperature8 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        break;
                    case "18 52 17 F3":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzrq.BMS_FramePositivePoleTemperature9 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.BMS_FrameNegativePoleTemperature9 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(6, 2);
                        cd_zzrq.BMS_FramePositivePoleTemperature10 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(9, 2);
                        cd_zzrq.BMS_FrameNegativePoleTemperature10 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(12, 2);
                        cd_zzrq.BMS_FramePositivePoleTemperature11 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(15, 2);
                        cd_zzrq.BMS_FrameNegativePoleTemperature11 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(18, 2);
                        cd_zzrq.BMS_FramePositivePoleTemperature12 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(21, 2);
                        cd_zzrq.BMS_FrameNegativePoleTemperature12 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        break;
                    case "18 53 17 F3":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzrq.BMS_FramePositivePoleTemperature13 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.BMS_FrameNegativePoleTemperature13 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(6, 2);
                        cd_zzrq.BMS_FramePositivePoleTemperature14 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(9, 2);
                        cd_zzrq.BMS_FrameNegativePoleTemperature14 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(12, 2);
                        cd_zzrq.BMS_FramePositivePoleTemperature15 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(15, 2);
                        cd_zzrq.BMS_FrameNegativePoleTemperature15 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(18, 2);
                        cd_zzrq.BMS_FramePositivePoleTemperature16 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        str_data1 = str_all.Substring(21, 2);
                        cd_zzrq.BMS_FrameNegativePoleTemperature16 = ParsMethod.GetParsWholeByte(str_data1, 1, -40);
                        break;
                    case "18 58 17 F3":
                        str_data1 = str_all.Substring(0, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.BMS_ModulePositivePoleOverTempAlarm1 = int.Parse(bjbz.Substring(7, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModulePositivePoleOverTempAlarm1;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModulePositivePoleOverTempAlarm1", "0X185817F3-0X" + str_data1 + "-bit0", faultlever, faultint);

                        cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm1 = int.Parse(bjbz.Substring(6, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm1;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModuleNegativePoleOverTempAlarm1", "0X185817F3-0X" + str_data1 + "-bit1", faultlever, faultint);
                        cd_zzrq.BMS_ModulePositivePoleOverTempAlarm2 = int.Parse(bjbz.Substring(5, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModulePositivePoleOverTempAlarm2;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModulePositivePoleOverTempAlarm2", "0X185817F3-0X" + str_data1 + "-bit2", faultlever, faultint);
                        cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm2 = int.Parse(bjbz.Substring(4, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm2;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModuleNegativePoleOverTempAlarm2", "0X185817F3-0X" + str_data1 + "-bit3", faultlever, faultint);
                        cd_zzrq.BMS_ModulePositivePoleOverTempAlarm3 = int.Parse(bjbz.Substring(3, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModulePositivePoleOverTempAlarm3;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModulePositivePoleOverTempAlarm3", "0X185817F3-0X" + str_data1 + "-bit4", faultlever, faultint);
                        cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm3 = int.Parse(bjbz.Substring(2, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm3;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModuleNegativePoleOverTempAlarm3", "0X185817F3-0X" + str_data1 + "-bit5", faultlever, faultint);
                        cd_zzrq.BMS_ModulePositivePoleOverTempAlarm4 = int.Parse(bjbz.Substring(1, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModulePositivePoleOverTempAlarm4;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModulePositivePoleOverTempAlarm4", "0X185817F3-0X" + str_data1 + "-bit6", faultlever, faultint);
                        cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm4 = int.Parse(bjbz.Substring(0, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm4;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModuleNegativePoleOverTempAlarm4", "0X185817F3-0X" + str_data1 + "-bit7", faultlever, faultint);
                        str_data1 = str_all.Substring(3, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.BMS_ModulePositivePoleOverTempAlarm5 = int.Parse(bjbz.Substring(7, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModulePositivePoleOverTempAlarm5;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModulePositivePoleOverTempAlarm5", "0X185817F3-0X" + str_data1 + "-bit0", faultlever, faultint);
                        cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm5 = int.Parse(bjbz.Substring(6, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm5;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModuleNegativePoleOverTempAlarm5", "0X185817F3-0X" + str_data1 + "-bit1", faultlever, faultint);
                        cd_zzrq.BMS_ModulePositivePoleOverTempAlarm6 = int.Parse(bjbz.Substring(5, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModulePositivePoleOverTempAlarm6;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModulePositivePoleOverTempAlarm6", "0X185817F3-0X" + str_data1 + "-bit2", faultlever, faultint);
                        cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm6 = int.Parse(bjbz.Substring(4, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm6;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModuleNegativePoleOverTempAlarm6", "0X185817F3-0X" + str_data1 + "-bit3", faultlever, faultint);
                        cd_zzrq.BMS_ModulePositivePoleOverTempAlarm7 = int.Parse(bjbz.Substring(3, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModulePositivePoleOverTempAlarm7;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModulePositivePoleOverTempAlarm7", "0X185817F3-0X" + str_data1 + "-bit4", faultlever, faultint);
                        cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm7 = int.Parse(bjbz.Substring(2, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm7;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModuleNegativePoleOverTempAlarm7", "0X185817F3-0X" + str_data1 + "-bit5", faultlever, faultint);
                        cd_zzrq.BMS_ModulePositivePoleOverTempAlarm8 = int.Parse(bjbz.Substring(1, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModulePositivePoleOverTempAlarm8;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModulePositivePoleOverTempAlarm8", "0X185817F3-0X" + str_data1 + "-bit6", faultlever, faultint);
                        cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm8 = int.Parse(bjbz.Substring(0, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm8;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModuleNegativePoleOverTempAlarm8", "0X185817F3-0X" + str_data1 + "-bit7", faultlever, faultint);
                        str_data1 = str_all.Substring(6, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.BMS_ModulePositivePoleOverTempAlarm9 = int.Parse(bjbz.Substring(7, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModulePositivePoleOverTempAlarm9;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModulePositivePoleOverTempAlarm9", "0X185817F3-0X" + str_data1 + "-bit0", faultlever, faultint);
                        cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm9 = int.Parse(bjbz.Substring(6, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm9;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModuleNegativePoleOverTempAlarm9", "0X185817F3-0X" + str_data1 + "-bit1", faultlever, faultint);
                        cd_zzrq.BMS_ModulePositivePoleOverTempAlarm10 = int.Parse(bjbz.Substring(5, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModulePositivePoleOverTempAlarm10;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModulePositivePoleOverTempAlarm10", "0X185817F3-0X" + str_data1 + "-bit2", faultlever, faultint);
                        cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm10 = int.Parse(bjbz.Substring(4, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm10;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModuleNegativePoleOverTempAlarm10", "0X185817F3-0X" + str_data1 + "-bit3", faultlever, faultint);
                        cd_zzrq.BMS_ModulePositivePoleOverTempAlarm11 = int.Parse(bjbz.Substring(3, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModulePositivePoleOverTempAlarm11;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModulePositivePoleOverTempAlarm11", "0X185817F3-0X" + str_data1 + "-bit4", faultlever, faultint);
                        cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm11 = int.Parse(bjbz.Substring(2, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm11;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModuleNegativePoleOverTempAlarm11", "0X185817F3-0X" + str_data1 + "-bit5", faultlever, faultint);
                        cd_zzrq.BMS_ModulePositivePoleOverTempAlarm12 = int.Parse(bjbz.Substring(1, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModulePositivePoleOverTempAlarm12;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModulePositivePoleOverTempAlarm12", "0X185817F3-0X" + str_data1 + "-bit6", faultlever, faultint);
                        cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm12 = int.Parse(bjbz.Substring(0, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm12;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModuleNegativePoleOverTempAlarm12", "0X185817F3-0X" + str_data1 + "-bit7", faultlever, faultint);
                        str_data1 = str_all.Substring(9, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.BMS_ModulePositivePoleOverTempAlarm13 = int.Parse(bjbz.Substring(7, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModulePositivePoleOverTempAlarm13;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModulePositivePoleOverTempAlarm13", "0X185817F3-0X" + str_data1 + "-bit0", faultlever, faultint);
                        cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm13 = int.Parse(bjbz.Substring(6, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm13;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModuleNegativePoleOverTempAlarm13", "0X185817F3-0X" + str_data1 + "-bit1", faultlever, faultint);
                        cd_zzrq.BMS_ModulePositivePoleOverTempAlarm14 = int.Parse(bjbz.Substring(5, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModulePositivePoleOverTempAlarm14;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModulePositivePoleOverTempAlarm14", "0X185817F3-0X" + str_data1 + "-bit2", faultlever, faultint);
                        cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm14 = int.Parse(bjbz.Substring(4, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm14;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModuleNegativePoleOverTempAlarm14", "0X185817F3-0X" + str_data1 + "-bit3", faultlever, faultint);
                        cd_zzrq.BMS_ModulePositivePoleOverTempAlarm15 = int.Parse(bjbz.Substring(3, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModulePositivePoleOverTempAlarm15;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModulePositivePoleOverTempAlarm15", "0X185817F3-0X" + str_data1 + "-bit4", faultlever, faultint);
                        cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm15 = int.Parse(bjbz.Substring(2, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm15;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModuleNegativePoleOverTempAlarm15", "0X185817F3-0X" + str_data1 + "-bit5", faultlever, faultint);
                        cd_zzrq.BMS_ModulePositivePoleOverTempAlarm16 = int.Parse(bjbz.Substring(1, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModulePositivePoleOverTempAlarm16;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModulePositivePoleOverTempAlarm16", "0X185817F3-0X" + str_data1 + "-bit6", faultlever, faultint);
                        cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm16 = int.Parse(bjbz.Substring(0, 1));
                        faultlever = 3;
                        faultint = cd_zzrq.BMS_ModuleNegativePoleOverTempAlarm16;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_ModuleNegativePoleOverTempAlarm16", "0X185817F3-0X" + str_data1 + "-bit7", faultlever, faultint);
                        break;
                    case "18 80 17 F3":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzrq.BMS_BatteryBoxQuantity = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.BMS_BatteryBoxNumber = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(6, 5);
                        cd_zzrq.BMS_AverageVoltageOfTheBatteryBox = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(12, 5);
                        cd_zzrq.BMS_AverageTemperatureOfTheBatteryBox = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 81 17 F3":
                        str_data1 = str_all.Substring(6, 5);
                        cd_zzrq.BMS_TheTerminalVoltageOfBatteryBox = ParsMethod.GetParsWholeByte(str_data1, 0.01);
                        str_data1 = str_all.Substring(12, 5);
                        cd_zzrq.BMS_BatteryBoxSOC = ParsMethod.GetParsWholeByte(str_data1, 0.002);
                        break;
                    case "18 82 17 F3":
                        str_data1 = str_all.Substring(0, 5);
                        cd_zzrq.BMS_TheMaximumVoltageMonomerSerialNumber = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(6, 5);
                        cd_zzrq.BMS_TheLargestSingleVoltageValue = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(12, 5);
                        cd_zzrq.BMS_TheMinimumVoltageMonomerSerialNumber = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(18, 5);
                        cd_zzrq.BMS_TheMinimumSingleVoltageValue = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 83 17 F3":
                        str_data1 = str_all.Substring(0, 5);
                        cd_zzrq.BMS_TheHighestTemperatureSamplingPoint = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(6, 5);
                        cd_zzrq.BMS_TheHighestTemperatureSamplingValue = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        str_data1 = str_all.Substring(12, 5);
                        cd_zzrq.BMS_TheLowestTemperatureSamplingPoint = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(18, 5);
                        cd_zzrq.BMS_TheLowestTemperatureSamplingValue = ParsMethod.GetParsWholeByte(str_data1, 0.001);
                        break;
                    case "18 84 17 F3":
                        str_data1 = str_all.Substring(0, 11);
                        cd_zzrq.BMS_PositiveCumulativePower = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                        str_data1 = str_all.Substring(12, 11);
                        cd_zzrq.BMS_NegativeCumulativePower = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                        break;
                    case "18 85 17 F3":
                        //      14:09:28 郑州日产新能源付工 2016/3/8 14:09:28 这个先不用解析了
                        break;
                    #region   CCS发送的报文
                    case "14 F2 01 41":
                        str_data1 = str_all.Substring(0, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.CCS_ChargerCommunication = int.Parse(bjbz.Substring(4, 1));
                        faultlever = 2;
                        faultint = cd_zzrq.CCS_ChargerCommunication;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "CCS_ChargerCommunication", "0X14F20141-0X" + str_data1 + "-bit3", faultlever, faultint);
                        cd_zzrq.CCS_ChargerHardwareFailure = int.Parse(bjbz.Substring(0, 1));
                        faultlever = 2;
                        faultint = cd_zzrq.CCS_ChargerHardwareFailure;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "CCS_ChargerHardwareFailure", "0X14F20141-0X" + str_data1 + "-bit7", faultlever, faultint);
                        str_data1 = str_all.Substring(6, 5);
                        cd_zzrq.CCS_ChargingVoltage = ParsMethod.GetParsWholeByte(str_data1, 0.1);
                        str_data1 = str_all.Substring(12, 5);
                        cd_zzrq.CCS_ChargingCurrent = ParsMethod.GetParsWholeByte(str_data1, 0.01);
                        str_data1 = str_all.Substring(18, 2);
                        cd_zzrq.CCS_ChargeSt = ParsMethod.GetParsWholeByte(str_data1);
                        break;
                    case "18 60 17 F3":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzrq.InsulationMonitor_StatusByte = ParsMethod.GetValuebyBinary(str_data1, 0, 4);
                        cd_zzrq.InsulationMonitor_TheInsulationResistanceOfTheAlarmState = ParsMethod.GetValuebyBinary(str_data1, 4, 2);
                        faultlever = 2;
                        faultint = cd_zzrq.InsulationMonitor_TheInsulationResistanceOfTheAlarmState;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "InsulationMonitor_TheInsulationResistanceOfTheAlarmState", "0x186017F3-0X" + str_data1, faultlever, faultint);
                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.InsulationMonitor_TheInsulationResistanceOfTheHighByte = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(6, 2);
                        cd_zzrq.InsulationMonitor_TheInsulationResistanceOfTheLowByte = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(9, 2);
                        cd_zzrq.InsulationMonitor_BatteryVoltageHighByte = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(12, 2);
                        cd_zzrq.InsulationMonitor_BatteryVoltageLowByte = ParsMethod.GetParsWholeByte(str_data1);
                        str_data1 = str_all.Substring(21, 2);
                        cd_zzrq.InsulationMonitor_LifeSignal = ParsMethod.GetParsWholeByte(str_data1);
                        break;
                    case "18 F0 00 F7":
                        str_data1 = str_all.Substring(0, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.APU_BrakeControlSystem_ControllerReady = int.Parse(bjbz.Substring(7, 1));
                        cd_zzrq.APU_BrakeControlSystem_Run = int.Parse(bjbz.Substring(6, 1));
                        cd_zzrq.APU_BrakeControlSystem_Fault = int.Parse(bjbz.Substring(5, 1));
                        faultlever = 2;
                        faultint = cd_zzrq.APU_BrakeControlSystem_Fault;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "APU_BrakeControlSystem_Fault", "0x18F000F7-0X" + str_data1, faultlever, faultint);
                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.APU_BrakeControlSystem_Counter = ParsMethod.GetValuebyBinary(str_data1, 8, 4,1);
                        break;
                    case "18 F0 01 F7":
                        str_data1 = str_all.Substring(0, 2);
                        bjbz = PublicMethods.Get16To2(str_data1, 1);
                        cd_zzrq.APU_SteeringControlSystem_ControllerReady = int.Parse(bjbz.Substring(7, 1));
                        cd_zzrq.APU_SteeringControlSystem_Run = int.Parse(bjbz.Substring(6, 1));
                        cd_zzrq.APU_SteeringControlSystem_Fault = int.Parse(bjbz.Substring(5, 1));
                        faultlever = 2;
                        faultint = cd_zzrq.APU_SteeringControlSystem_Fault;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "APU_SteeringControlSystem_Fault", "0x18F001F7-0X" + str_data1, faultlever, faultint);
                        str_data1 = str_all.Substring(3, 2);
                        cd_zzrq.APU_SteeringControlSystem_Counter = ParsMethod.GetValuebyBinary(str_data1, 8, 4,1);
                        break;
                    #endregion
                    default:
                        break;
                }
                #endregion
            }

            WriteLog.WriteLogMeaning("ZzRQ", "", cd_zzrq);

        }

    }
}
