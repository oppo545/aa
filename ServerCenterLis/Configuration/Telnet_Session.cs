using System;
using System.Collections.Generic;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System.IO;

using System.Threading;
using System.Text.RegularExpressions;
using SuperSocket.SocketEngine;
using System.Configuration;


using System.Linq;
namespace ServerCenterLis
{
    public class Telnet_Session : AppSession<Telnet_Session>
    {
        /// <summary>
        /// Called when [session started].
        /// </summary>
        protected override void OnSessionStarted()
        {
            string info = string.Empty;

            //   AnalysisServer.WriteLog("Center,Welcome to ServerCenterLis!");


            if (string.IsNullOrEmpty(info))
            {
                // this.Send("Welcome to ServerCenterLis!");
            }
            else
            {
                this.Send(info);
            }


            Sts = new Status_9();

            Lasttime = "";

            //消息队列 Send客户端
            string mqIdentify = System.Configuration.ConfigurationManager.AppSettings["mqIdentify"].ToString();
            if (mqs == null)
            {
                mqs = MqFactory.CreateInstance(mqIdentify);
            }


            Lastcantime = "";
        }

        /// <summary>
        /// 自动加车,入库Sql
        /// 2016-01-22
        /// </summary>
        public string InsertVehicleInfo = System.Configuration.ConfigurationManager.AppSettings["InsertVehicleInfo"].ToString();

        /// <summary>
        /// 自动加车,入库Sql
        /// 2016-01-22   
        /// </summary>
        public string OrganizationID = System.Configuration.ConfigurationManager.AppSettings["OrganizationID"].ToString();

        /// <summary>
        /// 自动加车,入库Sql
        /// 2016-01-22   
        /// </summary>
        public string VehicleTypeID = System.Configuration.ConfigurationManager.AppSettings["VehicleTypeID"].ToString();


        /// <summary>
        /// 车型数组,可配置
        /// </summary>
        public string VehicleModels = System.Configuration.ConfigurationManager.AppSettings["VehicleModels"].ToString();

        /// <summary>
        /// 日志条数记录器 
        /// </summary>
        public int num = 0;


    
        public int Activenum = 0;

        #region 公共
        //如果每条信息来 这个参数都会被赋值,那么丢到 common中,那是可行的, 但如果不是,那么就会被干扰

        /// <summary>
        /// 是否为补传
        /// </summary>
        public bool isSupplement = false;

        /// <summary>
        /// 是否为充电状态
        /// </summary>
        public bool isCharging = false;
        /// <summary>
        /// 是否为充电桩 （暂时未用)
        /// </summary>
        public bool isChargPile = false;
        /// <summary>
        /// 是否为web下发指令
        /// </summary>
        public bool isSet = false;
        //0 关， 1  开
        public string isacc = "0";

        /// <summary>
        /// 系统识别码
        /// System identification code
        /// </summary>
        public string siCode;
        /// <summary>
        /// 车型
        /// </summary>
        public int vlsType = -1;

        /// <summary>
        ///记录 是否激活
        /// </summary>
        public bool isActivate = false;

        /// <summary>
        ///记录 设备信息 是否入库 
        /// </summary>
        public bool isDeviceRecord = false;

        /// <summary>
        ///记录 设备信息 入库 是否 完整  
        /// </summary>
        public bool isCompleteRecord = false;

        /// <summary>
        ///记录 设备信息 是否入库 
        /// </summary>
        public bool isDBExist = false;

        /// <summary>
        ///是否为上标车辆
        /// </summary>
        public bool isMarkedVehicles = false;

        public List<string> lisRegister;
        public List<string> getLisRegister()
        {
            if (lisRegister == null)
            {
                lisRegister = new List<string>();
            }
            return lisRegister;
        }
        #endregion


        #region 地标_定义类
        /// <summary>
        /// 获取车型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int GetVehicleStyle(string info)
        {
            int result = -1;
            string[] VclModels = VehicleModels.Split(',');
            result = Array.IndexOf(VclModels, PublicMethods.GetHexToAscii(info));//获取数组的下标
            if (info.Equals("00 00 00 00 00"))
            {
                result = 0;
            }
            return result;
        }
        /// <summary>
        /// 获取车型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int GetStyle(string info)
        {
            int result = -1;
            switch (info)
            {
                case "00 00 00 00 00"://HM
                    result = 0;
                    break;
                case "30 30 30 48 4D"://HM
                    result = 0;
                    break;
                case "44 46 41 36 30"://DFA60
                    result = 1;
                    break;
                case "44 46 45 33 30"://DFE30
                    result = 2;
                    break;
                case "52 43 4C 46 31"://RCLF1
                    result = 3;
                    break;
                case "44 46 4C 54 36"://DFLT6
                    result = 4;
                    break;
                case "44 52 39 35 43"://DR95C
                    result = 5;
                    break;
                case "30 35 41 45 56"://比亚迪5AEV
                    result = 6;
                    break;
                case "30 48 41 45 56"://比亚迪HAVE
                    result = 7;
                    break;
                case "30 56 42 45 56"://比亚迪VBEV
                    result = 8;
                    break;
                case "30 45 36 31 35"://比亚迪E615
                    result = 9;
                    break;
                case "44 46 42 45 30": //东风商用车E0
                    result = 10;
                    break;
                case "45 33 30 45 58": //E30EX
                    result = 11;
                    break;
                case "41 36 30 45 58": //A60EX
                    result = 12;
                    break;
                case "30 30 54 35 41"://比亚迪T5A
                    result = 13;
                    break;
                case "30 30 30 54 35"://比亚迪T5
                    result = 14;
                    break;
                case "30 54 38 53 41"://比亚迪T8SA
                    result = 15;
                    break;
                case "30 30 30 54 37"://比亚迪T7
                    result = 16;
                    break;
                case "30 30 30 54 34"://比亚迪T4
                    result = 17;
                    break;
                case "30 30 30 4B 38"://比亚迪K8
                    result = 19;
                    break;
                case "30 30 30 4B 39"://比亚迪K9
                    result = 18;
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// 车辆设备信息
        /// </summary>
        public Cls_VehicleEquipmentInfo cls_vlsei;

        public Cls_VehicleEquipmentInfo getClsVlsEI()
        {
            if (cls_vlsei == null)
            {
                cls_vlsei = new Cls_VehicleEquipmentInfo();
            }
            return cls_vlsei;
        }

        public Cls_BJDB cls_bjdb;
        public Cls_BJDB getClsBJDB()
        {
            if (cls_bjdb == null)
            {
                cls_bjdb = new Cls_BJDB();
            }
            return cls_bjdb;
        }

        public Cls_FormatBJDB clsft_bjdb;
        public Cls_FormatBJDB getClsBJDBFt()
        {
            if (clsft_bjdb == null)
            {
                clsft_bjdb = new Cls_FormatBJDB();
            }
            return clsft_bjdb;
        }

        /// <summary>
        /// 注册
        /// </summary>
        public Cls_Register cls_register;
        public Cls_Register getClsRegister()
        {
            if (cls_register == null)
            {
                cls_register = new Cls_Register();
            }
            return cls_register;
        }

        /// <summary>
        /// 单体蓄电池电压数据
        /// </summary>
        public Cls_SingleBatteryVoltage cls_sbv;
        public Cls_SingleBatteryVoltage getClsSBV()
        {
            if (cls_sbv == null)
            {
                cls_sbv = new Cls_SingleBatteryVoltage();
            }
            return cls_sbv;
        }

        public List<Cls_VoltageValueList> lis_vvl;
        /// <summary>
        /// 电压值列表
        /// </summary>
        /// <returns></returns>
        public List<Cls_VoltageValueList> getClsLstVvl()
        {
            if (lis_vvl == null)
            {
                lis_vvl = new List<Cls_VoltageValueList>();
            }
            return lis_vvl;
        }

        public Cls_VoltageValueList cls_vvl;
        /// <summary>
        /// 电压值
        /// </summary>
        /// <returns></returns>
        public Cls_VoltageValueList getClsVvl()
        {
            if (cls_vvl == null)
            {
                cls_vvl = new Cls_VoltageValueList();
            }
            return cls_vvl;
        }

        /// <summary>
        /// 动力蓄电池包温度
        /// </summary>
        public Cls_PowerBatteryPack cls_pbp;

        public Cls_PowerBatteryPack getClsPBP()
        {
            if (cls_pbp == null)
            {
                cls_pbp = new Cls_PowerBatteryPack();
            }
            return cls_pbp;
        }

        public List<Cls_TemperatureValueList> lis_tvl;
        /// <summary>
        /// 温度值列表
        /// </summary>
        /// <returns></returns>
        public List<Cls_TemperatureValueList> getClsLstTvl()
        {
            if (lis_tvl == null)
            {
                lis_tvl = new List<Cls_TemperatureValueList>();
            }
            return lis_tvl;
        }

        public Cls_TemperatureValueList cls_tvl;
        /// <summary>
        /// 温度值
        /// </summary>
        /// <returns></returns>
        public Cls_TemperatureValueList getClsTvl()
        {
            if (cls_tvl == null)
            {
                cls_tvl = new Cls_TemperatureValueList();
            }
            return cls_tvl;
        }

        /// <summary>
        /// 整车数据
        /// </summary>
        public Cls_VehicleInfo cls_vls;
        public Cls_VehicleInfo getClsVLS()
        {
            if (cls_vls == null)
            {
                cls_vls = new Cls_VehicleInfo();
            }
            return cls_vls;
        }
        /// <summary>
        /// 定位数据
        /// </summary>
        public Cls_PositioningData cls_ptd;
        public Cls_PositioningData getClsPTD()
        {
            if (cls_ptd == null)
            {
                cls_ptd = new Cls_PositioningData();
            }
            return cls_ptd;
        }
        /// <summary>
        /// 极值数据
        /// </summary>
        public Cls_ExtremeData cls_exd;
        public Cls_ExtremeData getClsEXD()
        {
            if (cls_exd == null)
            {
                cls_exd = new Cls_ExtremeData();
            }
            return cls_exd;
        }

        /// <summary>
        ///报警数据
        /// </summary>
        public Cls_AlarmData cls_armd;
        public Cls_AlarmData getClsArmd()
        {
            if (cls_armd == null)
            {
                cls_armd = new Cls_AlarmData();
            }
            return cls_armd;
        }


        /// <summary>
        ///车载终端状态信息上报
        /// </summary>
        public Cls_VehicleState cls_vlss;
        public Cls_VehicleState getClsVlsS()
        {
            if (cls_vlss == null)
            {
                cls_vlss = new Cls_VehicleState();
            }
            return cls_vlss;
        }

        /// <summary>
        ///海马扩展数据
        /// </summary>
        public Cls_HMExtension cls_hme;
        public Cls_HMExtension getClsHme()
        {
            if (cls_hme == null)
            {
                cls_hme = new Cls_HMExtension();
            }
            return cls_hme;
        }


        /// <summary>
        /// 实时上报接口
        /// </summary>
        public Cls_RealInformation clsri;
        #endregion
        /// <summary>
        /// 本地客户端发送指令标识,其他发送的不予处理
        /// </summary>
        public string MiddleIndex = ConfigurationManager.AppSettings["MiddleIndex"].ToString();
        /// <summary>
        /// EVT指令测试
        /// </summary>
        public string MiddleIndex1 = ConfigurationManager.AppSettings["MiddleIndex1"].ToString();


        public Mq mqs;

        #region can class
        public Candata candate;

        public Candata_s30 cd_s30;
        public Candata_s30 getClsCdS30()
        {
            if (cd_s30 == null)
            {
                cd_s30 = new Candata_s30();
            }
            return cd_s30;
        }

        public Candata_Ev_lf cd_lf;
        public Candata_Ev_lf getClsCdEvlf()
        {
            if (cd_lf == null)
            {
                cd_lf = new Candata_Ev_lf();
            }
            return cd_lf;
        }

        public Candata_EJ02 cd_ej02;
        public Candata_EJ02 getClsCdEJ02()
        {
            if (cd_ej02 == null)
            {
                cd_ej02 = new Candata_EJ02();
            }
            return cd_ej02;
        }
        public Candata_EJ04 cd_ej04;
        public Candata_EJ04 getClsCdEJ04()
        {
            if (cd_ej04 == null)
            {
                cd_ej04 = new Candata_EJ04();
            }
            return cd_ej04;
        }

        public Candata_A60 cd_a60;
        public Candata_A60 getClsCdA60()
        {
            if (cd_a60 == null)
            {
                cd_a60 = new Candata_A60();
            }
            return cd_a60;
        }
        public Cls_Evt cd_evt;
        public Cls_Evt getClsCdEVT()
        {
            if (cd_evt == null)
            {
                cd_evt = new Cls_Evt();
            }
            return cd_evt;
        }
        public Candata_ZzRuiQi cd_zzrq;
        public Candata_ZzRuiQi getClsCdZzRQ()
        {
            if (cd_zzrq == null)
            {
                cd_zzrq = new Candata_ZzRuiQi();
            }
            return cd_zzrq;
        }
        public Candata_ZzShuaiKe cd_zzsk;
        public Candata_ZzShuaiKe getClsCdZzSK()
        {
            if (cd_zzsk == null)
            {
                cd_zzsk = new Candata_ZzShuaiKe();
            }
            return cd_zzsk;
        }
        public Candata CreateInstance(int index)
        {
            if (candate == null)
            {
                switch (index)
                {
                    case 0: candate = getClsCdS30(); break;
                    case 1: candate = getClsCdEvlf(); break;
                    case 2: candate = getClsCdEJ02(); break;
                    case 3: candate = getClsCdEJ04(); break;
                    case 4: candate = getClsCdA60(); break;
                    case 5: candate = getClsCdZzRQ(); break;
                    case 6: candate = getClsCdZzSK(); break;
                    default: break;
                }

            }
            return candate;
        }

        #endregion

        /// <summary>
        /// 上一条 有效 经度
        /// </summary>
        public double EffectiveLong = 0;
        /// <summary>
        /// 上一条 有效 纬度
        /// </summary>
        public double EffectiveLat = 0;

        /// <summary>
        /// The lastcantime
        /// </summary>
        private string lastcantime;
        /// <summary>
        /// 上一个cantime，对比 如果相同 则丢弃本次
        /// </summary>
        public string Lastcantime
        {
            get { return lastcantime; }
            set { lastcantime = value; }
        }

        private string otherClient;
        /// <summary>
        /// 是否为其客户端(车载设备,内部连接对象)
        /// </summary>
        public string OtherClient
        {
            get { return otherClient; }
            set { otherClient = value; }
        }

        private string lasttime;

        /// <summary>
        /// 记录gps时间，用以入库can
        /// </summary>
        public string Lasttime
        {
            get { return lasttime; }
            set { lasttime = value; }
        }

        private bool is088;

        public bool Is088
        {
            get { return is088; }
            set { is088 = value; }
        }

        public StreamWriter writer;

        public int numcount;

        public Status_9 Sts;

        public Status_9 getClsSts()
        {
            if (Sts == null)
            {
                Sts = new Status_9();
            }
            return Sts;
        }

        public Layout Ly;

        public Layout getClsLy()
        {
            if (Ly == null)
            {
                Ly = new Layout();
            }
            return Ly;
        }
        public Position Pt1;

        public Position getClsPt1()
        {
            if (Pt1 == null)
            {
                Pt1 = new Position();
            }
            return Pt1;
        }


        public Cls_Warning Wi;

        public Cls_Warning getClsWi()
        {
            if (Wi == null)
            {
                Wi = new Cls_Warning();
            }
            return Wi;
        }

        public Cls_Status Ss;

        public Cls_Status getClsSs()
        {
            if (Ss == null)
            {
                Ss = new Cls_Status();
            }
            return Ss;
        }

        public Cls_Position Pt;
        public Cls_Position getClsPt()
        {
            if (Pt == null)
            {
                Pt = new Cls_Position();
            }
            return Pt;
        }

        public Cls_Format Ft;

        public Cls_Format getClsFt()
        {
            if (Ft == null)
            {
                Ft = new Cls_Format();
            }
            return Ft;
        }

        public Cls_Signalstate Sle;

        public Cls_Signalstate getClsSle()
        {
            if (Sle == null)
            {
                Sle = new Cls_Signalstate();
            }
            return Sle;
        }

        public Cls_SHDB_Register clsshdb_register;

        public Cls_SHDB_Register getSHDBRegister()
        {
            if (clsshdb_register == null)
            {
                clsshdb_register = new Cls_SHDB_Register();
            }
            return clsshdb_register;
        }

        private string sssim;
        /// <summary>
        /// 记录ss客户端 标识  <string, string> 
        /// 车机sim
        /// </summary>
        public string Sssim
        {
            get { return sssim; }
            set { sssim = value; }
        }


        //反馈信息
        public string msginfo = "";


        public void SendBegin()
        {
            this.Send("RepeatConnect");
        }


        private List<string> lisbs;

        public List<string> Lisbs
        {
            get { return lisbs; }
            set { lisbs = value; }
        }




        protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
        {
            this.Send("Unknow request");

        }

        protected override void HandleException(Exception e)
        {
            this.Send("Application error: {0}", e.Message);
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            //foreach (var s in this.AppServer.GetAllSessions())
            //{
            //    s.Close();
            //}

            //add you logics which will be executed after the session is closed
            base.OnSessionClosed(reason);

            //下线
            if (!string.IsNullOrEmpty(siCode) && isonline)
            {
                string strinfo = string.Format("insert into vehiclerunninginfo.onlinerecode(SystemNo,OnlineTime,Acc,IsOnline)  VALUES('{0}','{1:yyyy-MM-dd HH:mm:ss}','0','0');", siCode, DateTime.Now);
                mqs.SendMsg(strinfo);
                strinfo = FomatToJosnByOnLine(siCode, "0");
                SendAsToServersB(strinfo);
                isonline = false;
                WriteLog.WriteTestLog("Online", string.Format("Offline-{0}-{1:yyyy-MM-dd HH:mm:ss}", siCode, DateTime.Now), true);
            }

            //会话清空后, 初始化标识
            //if (!string.IsNullOrEmpty(OtherClient))
            //{
            //    if (OtherClient.Equals("android"))
            //    {
            //        AnalysisServer.IsStConnect = false;
            //        WriteLog.WriteOrdersLog("Close AnalysisServer !");
            //    }
            //}
            //WriteLog.WriteOrdersLog("Close ServerCenterLis!");
            ////AnalysisServer.WriteLog("Close ServerCenterLis!");

            ////SessionClosed
            //if (AnalysisServer.LisOnline != null)
            //{
            //    if (AnalysisServer.LisOnline.Contains(siCode))
            //    {
            //        AnalysisServer.LisOnline.Remove(siCode);
            //    }
            //}


        }

        public int GameHallId { get; internal set; }

        public int RoomId { get; internal set; }



        /// <summary>
        /// 去空格
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetZeroSuppression(string info)
        {
            string[] infos = info.Split(' ');
            string temp = "";
            for (int i = 0; i < infos.Length; i++)
            {
                temp += infos[i];
            }
            return temp;
        }

        /// <summary>
        /// 得到 10进制数据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static int Getdecimal(string info)
        {
            return Convert.ToInt32(info, 16);
        }

        /// <summary>
        /// 得到 2进制  16转2
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SixteenTrunTwo(string data)
        {
            int data1 = Convert.ToInt32(data, 16);
            data = Convert.ToString(data1, 2);
            return data;
        }

        /// <summary>
        /// 伪ip转换系统编号
        /// 龙安9.0协议版
        /// </summary>
        /// <param name="wip">伪ip</param>
        /// <returns></returns>
        public static string WipToSysNo(string wip)
        {
            string[] infos = wip.Split(' ');
            string str1 = "", str2 = "", str3 = "", str4 = "";

            //1.16-10
            str1 = Convert.ToInt32(infos[0], 16).ToString();
            str2 = Convert.ToInt32(infos[1], 16).ToString();
            str3 = Convert.ToInt32(infos[2], 16).ToString();
            str4 = Convert.ToInt32(infos[3], 16).ToString();

            //2.10-2
            str1 = Get10z2(str1);
            str2 = Get10z2(str2);
            str3 = Get10z2(str3);
            str4 = Get10z2(str4);

            string a = str1.Substring(0, 1) + str2.Substring(0, 1) + str3.Substring(0, 1) + str4.Substring(0, 1);
            //3.2-10
            str1 = Convert.ToInt32(str1.Substring(1, str1.Length - 1), 2).ToString();
            str2 = Convert.ToInt32(str2.Substring(1, str2.Length - 1), 2).ToString();
            str3 = Convert.ToInt32(str3.Substring(1, str3.Length - 1), 2).ToString();
            str4 = Convert.ToInt32(str4.Substring(1, str4.Length - 1), 2).ToString();


            a = Convert.ToInt32(a, 2).ToString();
            a = (int.Parse(a) + 30).ToString();

            str1 = str1.Length == 1 ? "0" + str1 : str1;
            str2 = str2.Length == 1 ? "0" + str2 : str2;
            str3 = str3.Length == 1 ? "0" + str3 : str3;
            str4 = str4.Length == 1 ? "0" + str4 : str4;

            string info = "1" + a + str1 + str2 + str3 + str4;
            return info;
        }

        /// <summary>
        /// 10转2
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string Get10z2(string info)
        {
            int int10 = int.Parse(info);
            string jz = Convert.ToString(int10, 2);
            int num = jz.Length;
            for (int i = 0; i < 8 - num; i++)
            {
                jz = "0" + jz;
            }
            return jz;
        }




        /// <summary>
        /// 获取 校验码
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public static string GetJy(string[] infos)
        {
            int a, b, x = 0;
            for (int i = 0; i < infos.Length; i++)
            {
                if (i != infos.Length - 1)
                {
                    if (i == 0)
                    {
                        a = Getdecimal(infos[i].ToString());
                    }
                    else
                    {
                        a = x;
                    }
                    b = Getdecimal(infos[i + 1].ToString());
                    x = a ^ b;
                }
            }

            string info = Convert.ToString(x, 16);
            if (info.Length == 1)//不足 补零
            {
                info = "0" + info;
            }
            return info.ToUpper();
        }

        /// <summary>
        ///隔偶加空格|格式化 系统编号
        ///13622222123 ->01 36 22 22 21 23
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetFomartSysno(string info)
        {
            info = "0" + info;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < info.Length; ++i)
            {
                sb.Append(info[i]);
                if ((i + 1) % 2 == 0)
                    sb.Append(" ");
            }
            return sb.ToString().Substring(0, sb.Length - 1);
        }



        /// <summary>
        /// Fomats to josn.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="type">The type.</param>
        /// <param name="info">The information.</param>
        /// <param name="identifying">The identifying.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string FomatToJosn(string key, string type, string info, string identifying, string data)
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("{\"key\":\"" + key + "\",");
            buffer.Append("\"sendType\":\"" + type + "\",");
            buffer.Append("\"receiver\":\"" + info + "\",");
            //buffer.Append("\"timestamp\":\"" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\",");
            buffer.Append("\"identifying\":\"" + identifying + "\",");
            buffer.Append("\"data\":{");
            buffer.Append(data);
            buffer.Append("}");
            buffer.Append("}");
            return buffer.ToString();
        }

        /// <summary>
        /// 统一发送格式
        /// </summary>
        ///  <param name="iscan">0 否</param>
        /// <param name="identifying"></param>
        /// <param name="data">systemNo,longitude,latitude,speed,direction,elevation,acc,islocation,mileage,Oil,currentTime,signalName,currentValue,maxValue,minValue</param>
        /// <returns></returns>
        public static string FomatToJosnByNew(int iscan, string identifying, string data)
        {
            string key = iscan == 0 ? "client_message" : "client_message_can";
            string[] infos = data.Split(',');
            StringBuilder buffer = new StringBuilder();
            buffer.Append("{\"key\":\"" + key + "\",");
            buffer.Append("\"sendType\":\"2\",");
            buffer.Append("\"receiver\":\"" + infos[0] + "\",");
            //buffer.Append("\"timestamp\":\"" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\",");
            buffer.Append("\"identifying\":\"" + identifying + "\",");
            buffer.Append("\"data\":{");
            buffer.Append("\"systemNo\":\"" + infos[0] + "\",");
            buffer.Append("\"longitude\":\"" + infos[1] + "\",");
            buffer.Append("\"latitude\":\"" + infos[2] + "\",");
            buffer.Append("\"speed\":\"" + infos[3] + "\",");
            buffer.Append("\"direction\":\"" + infos[4] + "\",");
            buffer.Append("\"elevation\":\"" + infos[5] + "\",");
            buffer.Append("\"acc\":\"" + infos[6] + "\",");
            buffer.Append("\"islocation\":\"" + infos[7] + "\",");
            buffer.Append("\"mileage\":\"" + infos[8] + "\",");
            buffer.Append("\"Oil\":\"" + infos[9] + "\",");
            buffer.Append("\"currentTime\":\"" + infos[10] + "\",");
            buffer.Append("\"signalName\":\"" + infos[11] + "\",");
            buffer.Append("\"currentValue\":\"" + infos[12] + "\",");
            buffer.Append("\"maxValue\":\"\",");
            buffer.Append("\"minValue\":\"\"");
            buffer.Append("}");
            buffer.Append("}");
            return buffer.ToString();
        }

        public static string FomatToJosnByUnified(int iscan, string identifying, string data)
        {
            string key = iscan == 0 ? "client_message" : "client_message_can";
            string[] infos = data.Split(',');
            StringBuilder buffer = new StringBuilder();
            buffer.Append("{\"key\":\"" + key + "\",");
            buffer.Append("\"sendType\":\"2\",");
            buffer.Append("\"receiver\":\"" + infos[0] + "\",");
            buffer.Append("\"identifying\":\"" + identifying + "\",");
            buffer.Append("\"data\":{");
            buffer.Append("\"systemNo\":\"" + infos[0] + "\",");
            buffer.Append("\"currentTime\":\"" + infos[1] + "\"},");
            buffer.Append("\"signalNames\":{");
            for (int i = 0; i < infos[2].Split(';').Count()-1; i++)
            {
                if (i != infos[2].Split(';').Count() - 2)
                {
                    buffer.Append("\"" + infos[2].Split(';')[i].Split('~')[0] + "\":");
                    buffer.Append("\"" + infos[2].Split(';')[i].Split('~')[1] + "\",");
                }
                else
                {
                    buffer.Append("\"" + infos[2].Split(';')[i].Split('~')[0] + "\":");
                    buffer.Append("\"" + infos[2].Split(';')[i].Split('~')[1] + "\"");
                }
            }
            buffer.Append("}}");
            return buffer.ToString();
        }


        //当前session是否为null
        public static bool isonline = false;


        /// <summary>
        /// 推送 上线 下线    //BMK 2015-10-20 卢让不发送
        /// </summary>
        /// <param name="sysno"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string FomatToJosnByOnLine(string sysno, string index)
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("{\"key\":\"client_message\",");
            buffer.Append("\"sendType\":\"2\",");
            buffer.Append("\"receiver\":\"\",");
            buffer.Append("\"identifying\":\"G031\",");
            buffer.Append("\"data\":{");
            buffer.Append("\"systemNo\":\"" + sysno + "\",");
            buffer.Append("\"signalName\":\"G031\",");
            buffer.Append("\"value\":\"" + index + "\"");
            buffer.Append("}");
            buffer.Append("}");
            return buffer.ToString();
        }

        /// <summary>
        /// Sends as to .   gps+上线+回复
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="strinfo">The strinfo.</param>
        public void SendAsToServers(string strinfo)
        {
            mqs.SendMsg3(strinfo);
        }
        /// <summary>
        /// activemq 区分   SendMsg2:  can
        /// </summary>
        /// <param name="strinfo"></param>
        public void SendAsToServersB(string strinfo)
        {
            mqs.SendMsg2(strinfo);
        }

        public void SendToReply(string strinfo)
        {
            mqs.SendMsg4(strinfo);
        }
        /// <summary>
        /// activemq 区分   SendMsgByAlarm:  Alarm
        /// </summary>
        /// <param name="strinfo"></param>
        public void SendAsToServersByAlarm(string strinfo)
        {
            mqs.SendMsgByAlarm(strinfo);
        }
        public void SendAsToServersByForwardToSHDB(string strinfo)    
        {
            mqs.SendMsgByForwardToSHDB(strinfo);
        }
        public static int GetAlarmType(string alarmtypename)
        {
            int result = 0;
            switch (alarmtypename)
            {
                case "无故障":
                    result = 0;
                    break;
                case "轻微故障":
                    result = 1;
                    break;
                case "一般故障":
                    result = 2;
                    break;
                case "严重故障":
                    result = 3;
                    break;
                case "致命故障":
                    result = 4;
                    break;
                default:
                    break;
            }
            return result;
        }
        /// <summary>
        /// 消息封装
        /// </summary>
        /// <param name="info">验证码,消息头,消息体</param>
        /// <returns></returns>
        public static string GetXxfz(string info)
        {
            Regex reg = new Regex("7d", RegexOptions.IgnoreCase);
            if (reg.IsMatch(info))
            {
                info = reg.Replace(info, "7d 01");
            }
            reg = new Regex("7e", RegexOptions.IgnoreCase);
            if (reg.IsMatch(info))
            {
                info = reg.Replace(info, "7d 02");
            }
            return info.ToUpper();
        }
        string[] batteryrecord = { "BMS_BattNo", "BMS_BattPackNo", "BMS_ChargeFS", "BMS_ChargerACInput", "BMS_ChargerDCInput", "BMS_ChargeSt", "BMS_CreepageMonitor", "BMS_Current", "BMS_FastChargeSt", "BMS_Fault", "BMS_FaultDislpay", "BMS_FaultState", "BMS_HighVolSt", "BMS_MaxCellBatt", "BMS_MaxCellBattNumber", "BMS_MaxTempNumber", "BMS_MinCellBatt", "BMS_MinCellBattNumber", "BMS_MinTempNumber", "BMS_OFCConnectSignal", "BMS_OutsideChargeSignal", "BMS_SlowChargeSt", "BMS_SOC", "BMS_SOCCalculate", "BMS_Temperature", "BMS_Temp_Ave", "BMS_Temp_Max", "BMS_Temp_Min", "BMS_Voltage", "IC_TotalOdmeter", "VCU_CruisingRange", "VCU_Keyposition" };
        string[] mileagerecord = { "IC_Odmeter", "IC_TotalOdmeter", "VCU_CruisingRange", "VCU_BrakeEnergy", "VCU_BrakePedalSt", "VCU_Fault", "VCU_Keyposition", "BMS_SOC" };
        string[] motorrecord = { "MCU_ElecMachineFault", "MCU_ElecPowerTrainMngtState", "MCU_InternalMachineTemp", "Motor_MaxGenTorque", "MCU_MaxMotorTorque", "Motor_Torqueestimation", "MCU_ActiveDischarge", "Motor_AllowMaxTorque", "Motor_ControllerTemp", "Motor_DCCurrent", "Motor_DCVolt", "Motor_Fault", "Motor_OutputPower", "Motor_OutputTorque", "Motor_Revolution", "Motor_RunDirection", "Motor_State", "Motor_Temperature", "Motor_TorqueFeedback", "Motor_WaterTemp", "VCU_Keyposition" };
        string[] otherrecord = { "ABS_BrakeSignal", "ABS_VehSpd", "ONC_CommunicationSt", "ONC_Fault", "ONC_ONCTemp", "ONC_InputVoltageSt", "ONC_OutputCurrent", "ONC_OutputVoltage", "TCU_GearPosition", "DCDC_EnableResponse", "DCDC_Fault", "DCDC_InputCurrent", "DCDC_InputVoltage", "DCDC_OutputCurrent", "DCDC_OutputVoltage", "DCDC_Temperature" };

        public int DecompositionNum = int.Parse(ConfigurationManager.AppSettings["DecompositionNum"].ToString());

        /// <summary>
        /// 允许空值
        /// </summary>
        string[] AdmitNull = { "Gps_Location" };
        /// <summary>
        /// 将数据添加到表 
        /// batteryrecord,mileagerecord,motorrecord,otherrecord,
        /// runningrecord,alarmrecord
        /// </summary>
        /// <param name="session"></param>
        /// <param name="infovalue"></param>
        /// <param name="datetime"></param>
        /// <param name="lisinfo"></param>
        ///  <param name="dicRepeat"></param>
        /// <param name="canfault"></param>
        public void InsertTable(Telnet_Session session, string infovalue, string datetime, List<string> lisinfo, IDictionary<string, string> dicRepeat, string canfault)
        {
            try
            {
                string infoa = ""; string infob = "";
                string tempstr = ""; string temp = "", dtimeput = "";
                string[] cans;
                StringBuilder buffer = new StringBuilder();

                #region BMK 入库数据处理规则  info + 故障
                string sendmsgtol = "", sendtomysql = "";

                if (!session.Lastcantime.Equals(datetime))
                {
                    string infokey = "`SystemNo`,`Longitude`,`Latitude`,`Speed`,`Direction`,`Elevation`,`Acc`,`IsLocation`,`Mileage`,`Oil`,`CurrentTime`";
                    StringBuilder sbbd = new StringBuilder();
                    sbbd.Append(" insert into vehiclerunninginfo.batteryrecord ( " + infokey);
                    StringBuilder sbmd = new StringBuilder();
                    sbmd.Append(" insert into vehiclerunninginfo.mileagerecord ( " + infokey);
                    StringBuilder sbmrd = new StringBuilder();
                    sbmrd.Append(" insert into vehiclerunninginfo.motorrecord ( " + infokey);
                    StringBuilder sbod = new StringBuilder();
                    sbod.Append(" insert into vehiclerunninginfo.otherrecord ( " + infokey);
                    string keybd = "", valuebd = "", keymd = "", valuemd = "", keymrd = "", valuemrd = "", keyod = "", valueod = "";

                    int num = 0;
                    for (int i = 0; i < lisinfo.Count; i++)
                    {
                        if (session.siCode != null)
                        {
                            infoa = lisinfo[i].Split('^')[0];
                            infob = lisinfo[i].Split('^')[1];
                            if (string.IsNullOrEmpty(infob)) infob = "";
                            ////将重复的信号值(不为0的)替换
                            //if (dicRepeat != null && infob == "0")
                            //{
                            //    if (dicRepeat.Keys.Contains(infoa))
                            //    {
                            //        infob = dicRepeat[infoa];
                            //    }
                            //}

                            if (!string.IsNullOrEmpty(infob) || AdmitNull.Contains(infoa))
                            {

                                //TODO Filter  过滤 时间超过服务器时间的数据  
                                //dtimeput = PublicMethods.GetFormatTme(session.siCode, datetime);
                                tempstr = string.Format("{0},'{1}','{2}'", infovalue, infoa, infob);
                                if (i == DecompositionNum * num)
                                {
                                    //temp = string.Format("insert into vehiclerunninginfo.runningrecord({0},`SignalName`,`CurrentValue`,`MaxValue`,`MinValue`) values ", infokey);
                                    temp = string.Format(string.Format("insert into vehiclerunninginfo.runningrecord({0},`SignalName`,`CurrentValue`) values({{0}}),", infokey), tempstr);
                                    num++;
                                }
                                else
                                {
                                    if (i == lisinfo.Count - 1) { temp = string.Format("({0});", tempstr); }
                                    else
                                    {
                                        if (i == DecompositionNum * num - 1)
                                        {
                                            temp = string.Format("({0});", tempstr);
                                        }
                                        else
                                        {
                                            temp = string.Format("({0}),", tempstr);
                                        }
                                    }
                                }
                                //temp = string.Format(string.Format("insert into vehiclerunninginfo.runningrecord({0},`SignalName`,`CurrentValue`) values({{0}});", infokey), tempstr);
                                //入库队列
                                if (!string.IsNullOrEmpty(temp))
                                {
                                    sendtomysql += temp;
                                }

                                //推送给中心服务器    TODOnotPush
                                if (!string.IsNullOrEmpty(tempstr) && !session.isSupplement && !infoa.Equals("Gps_Location"))// && isEnabled(infoa) 不判断,传所有can到中心服务器
                                {
                                    //tempstr = Telnet_Session.FomatToJosnByNew(1, "", tempstr);
                                    //tempstr = tempstr.Replace("'", "");
                                    //sendmsgtol += tempstr + "\b";

                                    sendmsgtol += infoa + "~" + infob + ";";


                                }

                                //BMK 2015-10-29 分模块入库测试
                                if (batteryrecord.Contains(infoa))
                                {
                                    keybd += string.Format(",`{0}`", infoa);
                                    valuebd += string.Format(",'{0}'", infob);
                                }
                                if (mileagerecord.Contains(infoa))
                                {
                                    keymd += string.Format(",`{0}`", infoa);
                                    valuemd += string.Format(",'{0}'", infob);
                                }
                                if (motorrecord.Contains(infoa))
                                {
                                    keymrd += string.Format(",`{0}`", infoa);
                                    valuemrd += string.Format(",'{0}'", infob);
                                }
                                if (otherrecord.Contains(infoa))
                                {
                                    keyod += string.Format(",`{0}`", infoa);
                                    valueod += string.Format(",'{0}'", infob);
                                }
                            }
                        }
                    }

                    sbbd.AppendFormat(" {0} ) ", keybd);
                    sbbd.AppendFormat(" values ( {0} {1} ) ;", infovalue, valuebd);

                    sbmd.AppendFormat(" {0} ) ", keymd);
                    sbmd.AppendFormat(" values ( {0} {1} ) ;", infovalue, valuemd);

                    sbmrd.AppendFormat(" {0} ) ", keymrd);
                    sbmrd.AppendFormat(" values ( {0} {1} ) ;", infovalue, valuemrd);

                    sbod.AppendFormat(" {0} ) ", keyod);
                    sbod.AppendFormat(" values ( {0} {1} ) ;", infovalue, valueod);

                    if (!string.IsNullOrEmpty(keybd))
                    {
                        session.mqs.SendMsg(sbbd.ToString());
                    }
                    if (!string.IsNullOrEmpty(keymd))
                    {
                        session.mqs.SendMsg(sbmd.ToString());
                    }
                    if (!string.IsNullOrEmpty(keymrd))
                    {
                        session.mqs.SendMsg(sbmrd.ToString());
                    }
                    if (!string.IsNullOrEmpty(keyod))
                    {
                        session.mqs.SendMsg(sbod.ToString());
                    }

                    //TO 中心服务器    TODOnotPush
                    if (!string.IsNullOrEmpty(sendmsgtol) && !session.isSupplement)
                    {
                        session.SendAsToServersB(FomatToJosnByUnified(1, "", infovalue.Split(',')[0].Replace('\'', ' ').Trim() + "," + infovalue.Split(',')[10].Replace('\'', ' ').Trim() + "," + sendmsgtol) + "\b");
                    }

                    //TO Mysql数据库
                    if (!string.IsNullOrEmpty(sendtomysql))
                    {
                        sendtomysql = sendtomysql.Replace(",insert", ";insert");
                        string[] mysqlstr = sendtomysql.Split(';');
                        for (int i = 0; i < mysqlstr.Count() - 1; i++)
                        {
                            session.mqs.SendMsg(mysqlstr[i] + ";");
                        }
                    }

                    //记录上一个datetime
                    session.Lastcantime = datetime;
                }

                //故障
                if (!string.IsNullOrEmpty(canfault))
                {
                    sendmsgtol = "";
                    canfault = canfault.Substring(1, canfault.Length - 1);
                    for (int i = 0; i < canfault.Split('$').Count(); i++)
                    {
                        if (session.siCode != null)
                        {
                            cans = canfault.Split('$')[i].Split(':');

                            //int alarmLevel = Telnet_Session.GetAlarmType(cans[2].ToString());
                            int alarmLevel = int.Parse(cans[2]);
                            if (alarmLevel != 0)
                            {
                                if (!string.IsNullOrEmpty(cans[0]))
                                {
                                    tempstr = string.Format("'{0}','{1}','{2}',{3},'{4}','{5}'", session.siCode, datetime, cans[0], alarmLevel, cans[1], cans[3]);
                                    temp = string.Format("insert into vehiclerunninginfo.alarmrecord(`SystemNo`,`AlarmTime`,`AlarmName`,`AlarmLevel`,`AlarmCode`,`AlarmDesc`,`IsSend`) values({0},0);", tempstr);
                                    //入库队列
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        session.mqs.SendMsg(temp);
                                    }

                                    //推送给中心服务器
                                    if (!string.IsNullOrEmpty(tempstr) && !session.isSupplement)// && isEnabled(infoa) 不判断,传所有can到中心服务器
                                    {
                                        buffer.Clear();
                                        buffer.AppendFormat("\"systemNo\":\"{0}\",", tempstr.Split(',')[0]);
                                        buffer.AppendFormat("\"currentTime\":\"{0}\",", tempstr.Split(',')[1]);
                                        buffer.AppendFormat("\"AlarmName\":\"{0}\",", tempstr.Split(',')[2]);
                                        buffer.AppendFormat("\"AlarmType\":\"{0}\",", tempstr.Split(',')[3]);
                                        buffer.AppendFormat("\"AlarmCode\":\"{0}\",", tempstr.Split(',')[4]);
                                        buffer.AppendFormat("\"AlarmDesc\":\"{0}\"", tempstr.Split(',')[5]);
                                        tempstr = Telnet_Session.FomatToJosn("client_message", "2", session.siCode, "G21", buffer.ToString());
                                        tempstr = tempstr.Replace("'", "");

                                        sendmsgtol += tempstr + "\b";
                                    }

                                }
                            }
                        }
                    }

                    //TO 中心服务器
                    if (!string.IsNullOrEmpty(sendmsgtol) && !session.isSupplement)
                    {
                        session.SendAsToServersByAlarm(sendmsgtol);
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog.WriteErrorLog("Error", "______02:" + ex.ToString(), true);
            }

        }

        /// <summary>
        ///  上标车辆注册列表
        ///  0,00 01 --原始插入
        ///  1,23   --一个字节,23-> 17
        /// </summary>
        /// <param name="info"></param>
        public void AddLisRegister(string info)
        {
            if (lisRegister == null)
            {
                lisRegister = new List<string>();
            }
            lisRegister.Add(info);
        }

    }
}
