using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ServerCenterLis
{
    /// <summary>
    /// 帅客
    /// </summary>
    public class ParsZzSK
    {
        /// 故障值  0正常         /// 故障等级    1	轻微故障,2	一般故障,3	严重故障,4	致命故障
        static int faultlever = 1, faultint = 0;
        static string str_data1, str_data2, str_data3, str_data4, str_data;
        static Regex regex = new Regex("\\s+");

        static int data1 = 0, data2 = 0, data3 = 0;
        public static void GetPars(string[] infos, ref string canfault, ref Candata_ZzShuaiKe cd_zzsk)
        {
            for (int i = 0; i < infos.Length; i++)
            {
                string bs = infos[i].Substring(0, 11);
                string bjbz = "00000000"; //默认全0二进制
                string str_all = infos[i].Substring(12);
                //1:起始从1开始,"02 01":包含的整个字节,1:开始字节,0:起始位,1:长度
                #region ZZRQ
                switch (bs)
                {
                    case "05 1E":

                        break;
                    case "05 0C":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzsk.TCU_LockingStatus = ParsMethod.GetValuebyBinary(str_data1, 0, 3);
                        cd_zzsk.TCU_Parking = ParsMethod.GetValuebyBinary(str_data1, 6, 1);
                        cd_zzsk.TCU_DriveState = ParsMethod.GetValuebyBinary(str_data1, 7, 1);

                        break;
                    case "05 80":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzsk.TCU_MotorParkingUnplugged = ParsMethod.GetValuebyBinary(str_data1, 4, 1);
                        cd_zzsk.TCU_ParkingMotorStall = ParsMethod.GetValuebyBinary(str_data1, 5, 1);
                        cd_zzsk.TCU_NotParkingMotorStall = ParsMethod.GetValuebyBinary(str_data1, 6, 1);
                        cd_zzsk.TCU_ParkingOrNotMotorStall = ParsMethod.GetValuebyBinary(str_data1, 7, 1);
                        str_data1 = str_all.Substring(3, 2);
                        cd_zzsk.TCU_ShiftMotorAngleSensorFault = ParsMethod.GetValuebyBinary(str_data1, 2, 1,1);
                        cd_zzsk.TCU_DMCLost = ParsMethod.GetValuebyBinary(str_data1, 6, 1,1);
                        str_data1 = str_all.Substring(6, 2);
                        cd_zzsk.TCU_CANFault = ParsMethod.GetValuebyBinary(str_data1, 1, 1,2);
                        cd_zzsk.TCU_GearboxOverheating = ParsMethod.GetValuebyBinary(str_data1, 3, 1,2);

                        break;
                    case "05 90":
                        str_data1 = str_all.Substring(0, 2);
                        cd_zzsk.TCU_WithTheCorresponding0X580Fault = ParsMethod.GetValuebyBinary(str_data1, 0, 3);

                        break;
                    case "05 00":

                        break;
                    case "05 3A":

                        break;
                    case "07 1A":

                        break;
                    case "03 08":

                        break;
                    case "00 8C":

                        break;
                    case "05 2A":

                        break;
                    case "01 81":

                        break;
                    case "02 81":

                        break;
                    case "03 81":

                        break;
                    case "04 81":

                        break;
                    case "04 82":

                        break;
                    default:
                        break;
                }
                #endregion
            }



        }

    }
}