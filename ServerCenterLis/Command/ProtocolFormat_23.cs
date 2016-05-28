using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Data.SqlClient;
using System.Data;


using SuperSocket.SocketEngine;
using CommonLibraries;
namespace ServerCenterLis
{
    /// <summary>
    /// 北京地标
    /// </summary>
    public class ProtocolFormat_23 : CommandBase<Telnet_Session, StringRequestInfo>
    {
        public override string Name
        {
            get { return "23"; }
        }

        /// <summary>
        /// 多电池多电压
        /// </summary>
        int[] vlstypes = { 13, 14, 15, 16, 17, 18, 19 };
        System.Diagnostics.Stopwatch stopwatch;
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="requestInfo">The request info.</param>
        public override void ExecuteCommand(Telnet_Session session, StringRequestInfo requestInfo)
        {
            /// 入库数据时间
            DateTime dtimeput;
            string sendCommand = "";
            try
            {
                /// 故障temp
                ///  fualtname:故障码:故障等级:故障描述
                string keystr = "", valuestr = "", canfault = ""; string datetimenow = "";
                /// 故障值  0正常         
                /// 故障等级    1(D)	轻微故障,2(C)	一般故障,3(B)	严重故障,4(A)	致命故障
                /// 2016-03-08 页面显示 数字倒置
                int faultlever = 1, faultint = 0;

                session.cls_bjdb = session.getClsBJDB();
                session.clsft_bjdb = session.getClsBJDBFt();

                //Body里面 包含了头部  在ReceiveFilterHM 处理了
                string canstr = requestInfo.Body.ToUpper().Trim();
                sendCommand = canstr;



                keystr = "";
                valuestr = "";


                if (!canstr.Contains("23 23"))
                {
                    return;
                }



                //容易 过滤掉 需要转义的关键字符 (有长度 无需转义)
                //string[] str = System.Text.RegularExpressions.Regex.Split(canstr, "(23 23 )", RegexOptions.IgnoreCase);

                //string se = str[2].ToString();

                string se = canstr.Substring(6);

                string strindex = se.Substring(0, 2);

                //接受的指令 直接 发送给车机(中转)
                //81 参数设置 ; 82  车载终端控制命令 ; 80 查询     ;C0 分时租赁开关门
                string[] orderstr = { "81", "82", "80", "C0" };
                if (orderstr.Contains(strindex))
                {
                    string MiddleIndex = se.Substring(se.Length - 8, 5);

                    //EVT指令下发
                    if (MiddleIndex.Equals(session.MiddleIndex))
                    {
                        session.OtherClient = "center";
                    }
                    //EVT指令测试
                    if (MiddleIndex.Equals(session.MiddleIndex1))
                    {
                        session.OtherClient = "android";
                    }
                    if (MiddleIndex.Equals(session.MiddleIndex) || MiddleIndex.Equals(session.MiddleIndex1))
                    {
                        session.isSet = true;
                        sendCommand = sendCommand.Replace(MiddleIndex + " ", "");
                        //修正数据单元长度 -2
                        string dylength = sendCommand.Substring(66, 5);
                        string dylengthn = PublicMethods.Get10To16((PublicMethods.Get16To10(dylength) - 2).ToString(), 2);
                        sendCommand = sendCommand.Replace(dylength, dylengthn).Trim();

                        string jy = PublicMethods.GetJy(sendCommand.Substring(6, sendCommand.Length - 6 - 3).Split(' '));
                        sendCommand = sendCommand.Substring(0, sendCommand.Length - 2) + jy.ToUpper();

                        WriteLog.WriteLogZLOther(session.OtherClient, sendCommand);
                    }

                }
                else
                {
                    session.isSet = false;
                }

                ////#region 判断格式完整【异或】
                //////  string aaaa = se.Substring(0, se.Length- 3).Trim();
                ////string[] infos = se.Substring(0, se.Length - 3).Trim().Split(' ');
                ////string jym = PublicMethods.GetJy(infos);
                ////string sejym = se.Substring(se.Length - 2, 2);
                //////if (!jym.ToUpper().Equals(sejym.ToUpper()))
                //////{
                //////    session.Send("yh error");
                //////    return;
                //////}

                ////#endregion



                #region //BMK 消息格式数据
                session.clsft_bjdb.Ml_bs = strindex;
                session.clsft_bjdb.Ml_yd = se.Substring(3, 2);
                string Simno = PublicMethods.GetZeroSuppression(se.Substring(6, 50));
                session.clsft_bjdb.Identifier = PublicMethods.GetHexToAscii(se.Substring(6, 50));
                //系统识别码
                session.siCode = session.clsft_bjdb.Identifier;



                session.clsft_bjdb.Jm = se.Substring(57, 2);
                session.clsft_bjdb.Unitlength = se.Substring(60, 5);

                string desc = string.Empty;

                if (!session.isSet)
                {
                    //BMK OtherClient不计入在线
                    //if (AnalysisServer.LisOnline != null && string.IsNullOrEmpty(session.OtherClient))
                    //{
                    //    //添加在线列表 
                    //    if (!AnalysisServer.LisOnline.Contains(session.siCode))
                    //    {
                    //        AnalysisServer.LisOnline.Add(session.siCode);
                    //    }
                    //}
                    //else
                    //{
                    //    //创建 中转客户端
                    //    AnalysisServer.CreateSocket();
                    //}

                    //过滤 消息体为null 的命令
                    if (se.Length > 69)
                    {
                        desc = se.Substring(66, se.Length - 66 - 3);
                    }
                }
                //string jy=
                #endregion

                ////session.num++;
                ////WriteLog.WriteLogZLOther("HM_", session.num + "__" + session.siCode + "__" + sendCommand, true);

                ////判断总长度 是否为完整流
                //int num = str[2].ToString().Length / 3 + 1 + 2;


                string temp = ""; //用来记录发送数据

                /*strindex 消息id
                 * desc 消息体
                 * 
                 * 
                 * */
                StringBuilder buffer = new StringBuilder();
                string strinfo, strnew, SignalName = "";

                //是否判断 当前指令车辆 是否不在线
                bool OrderCarIsonline = false;

                #region //web端上发数据
                try
                {
                    if (session.isSet)
                    {
                        foreach (var s in session.AppServer.GetAllSessions())
                        {
                            //BMK OtherClient 不发送指令

                            if (!string.IsNullOrEmpty(s.siCode))
                            {
                                if (s.siCode.Equals(session.siCode) && string.IsNullOrEmpty(s.OtherClient))
                                {
                                    byte[] btemp = strToToHexByte(sendCommand);
                                    s.Send(btemp, 0, btemp.Length);
                                    OrderCarIsonline = true;
                                }
                            }
                        }
                        if (!OrderCarIsonline)
                        {
                            //不在线 
                            if (strindex.Equals("C0"))
                            {
                                string ydlsh = se.Substring(69, 2);
                                buffer.Clear();
                                buffer.AppendFormat("\"systemNo\":\"{0}\",", session.siCode);
                                if (ydlsh.Equals("01")) //开门  油路控制
                                {
                                    buffer.Append("\"signalName\":\"takeCarReply\",");
                                }
                                if (ydlsh.Equals("02")) //关门    油路控制
                                {
                                    buffer.Append("\"signalName\":\"returnCarReply\",");
                                }
                                if (ydlsh.Equals("03")) //开门
                                {
                                    buffer.Append("\"signalName\":\"pushDoorReply\",");
                                }
                                if (ydlsh.Equals("04")) //关门
                                {
                                    buffer.Append("\"signalName\":\"shutDownReply\",");
                                }

                                buffer.Append("\"value\":\"4\"");
                                strinfo = Telnet_Session.FomatToJosn("client_message_lease", "2", session.siCode, "G18", buffer.ToString());
                                // 开关门 车辆不在线 回复   (以下 如果是center下发 回复)
                                if (session.OtherClient.Equals("center"))
                                {
                                    session.SendToReply(strinfo);
                                }
                                else
                                {
                                    //BMK添加其他客户端 2015-10-08
                                    foreach (var s in session.AppServer.GetAllSessions())
                                    {
                                        if (!string.IsNullOrEmpty(s.OtherClient))
                                        {
                                            if (s.OtherClient.Equals("android"))
                                            {
                                                s.Send(strinfo);
                                                WriteLog.WriteLogZLOther(s.OtherClient, strinfo, true);
                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                string info = "{\"key\":\"client_message\",\"sendType\":\"3\",\"receiver\":\"admin\",\"identifying\":\"G27\",\"data\":{\"sysnos\":\"" + session.siCode + "\",\"result\":\"4\"}}";
                                session.SendToReply(info);
                            }

                        }

                        //为下发指令后, 处理指令后,避免 后面 在线之类的干扰
                        return;

                    }
                }
                catch (Exception ex) { }

                #endregion

                //车载设备上发数据

                #region 注册时间 √
                if (strindex.Equals("01"))
                {
                    session.cls_register = session.getClsRegister();
                    session.clsshdb_register = session.getSHDBRegister();  //sb

                    session.clsshdb_register.registime = desc.Substring(0, 17);//sb
                    session.cls_register.Zc_zcsj = PublicMethods.GetGMT8Data(desc.Substring(0, 17), 1);
                    session.cls_register.Zc_zclsh = desc.Substring(18, 5);
                    session.cls_register.Zc_cph = PublicMethods.GetHexToAscii(desc.Substring(24, 23));

                    //车载终端编号
                    string czzdbh = desc.Substring(48, 35);
                    session.cls_register.Zc_zd_csdm = PublicMethods.GetHexToAscii(czzdbh.Substring(0, 11));
                    session.cls_register.Zc_zd_ph = PublicMethods.GetHexToAscii(czzdbh.Substring(12, 17));
                    session.cls_register.Zc_zd_lsh = czzdbh.Substring(30, 5);

                    //动力蓄电池包总数
                    int dldcgs = int.Parse(desc.Substring(84, 2));
                    session.cls_register.Zc_dlxdc = new List<Cls_bjdb_dlxdc>();

                    if (dldcgs > 0)
                    {
                        //总长度-前面的长度-后面的长度-3个空格=中间需要的长度 
                        //前面的长度 =startindex-1
                        string dldccode = desc.Substring(87, desc.Length - 32 - 86 - 1 - 1);
                        string onecode;
                        int n = 0;
                        for (int i = 0; i < dldcgs; i++)
                        {
                            onecode = dldccode.Substring(n, 59);
                            session.cls_vlsei = session.getClsVlsEI();
                            string Id = PublicMethods.Getdecimal(onecode.Substring(0, 2)).ToString();
                            //动力蓄电池编码定义
                            string Numberinfo = onecode.Substring(3, 41);
                            session.cls_vlsei.ProductionCode = PublicMethods.GetHexToAscii(Numberinfo.Substring(0, 11));
                            session.cls_vlsei.BatteryTypeCode = PublicMethods.Get16To10(Numberinfo.Substring(12, 2)).ToString();
                            session.cls_vlsei.RatedEnergy = PublicMethods.Get16To10(Numberinfo.Substring(15, 5)).ToString();
                            session.cls_vlsei.RatedVoltage = PublicMethods.Get16To10(Numberinfo.Substring(21, 5)).ToString();
                            session.cls_vlsei.BatteryProductionDateCode = PublicMethods.GetFormatTme(session.siCode, PublicMethods.GetGMT8Data(Numberinfo.Substring(27, 8), 0));
                            session.cls_vlsei.SerialNumber = PublicMethods.Get16To10(Numberinfo.Substring(36, 5)).ToString();

                            if (i == 0)    //上标使用了 第一个包的预留位
                            {
                                string dcyl = onecode.Substring(45, 14);
                                session.isMarkedVehicles = ParsMethod.GetParsBig(dcyl, 1, 0, 1) == 1;//sb
                                session.clsshdb_register.VehicleType = ParsMethod.GetParsBig(dcyl, 1, 2, 2);//sb
                                session.clsshdb_register.VehicleModels = "";//sb 
                                session.clsshdb_register.DrivingMotorType = ParsMethod.GetParsBig(dcyl, 1, 4, 4);//sb
                                session.clsshdb_register.DriveMotorArrangementType = ParsMethod.GetParsBig(dcyl.Substring(3), 1, 0, 4);//sb
                                session.clsshdb_register.DrivingMotorCoolingMode = ParsMethod.GetParsBig(dcyl.Substring(3), 1, 4, 2);//sb
                                session.clsshdb_register.EnergyStorageDeviceType = ParsMethod.GetParsBig(dcyl.Substring(3), 1, 6, 2);//sb
                                session.clsshdb_register.DrivingRangeElectricVehicle = ParsMethod.GetParsWholeByte(dcyl.Substring(6, 5));
                                session.clsshdb_register.MaxSpeedElectricVehicle = ParsMethod.GetParsWholeByte(dcyl.Substring(12, 2));
                            }

                            // 59 长度+1 +起始
                            n = 59 + 1 + n;
                        }

                    }
                    //预留位 11   校验位,已除
                    string yl = desc.Substring(desc.Length - 32, 32);

                    session.cls_register.Styling = session.GetVehicleStyle(yl.Substring(0, 14));
                    session.vlsType = session.cls_register.Styling;
                    int Activate = int.Parse(yl.Substring(15, 2));
                    //HM   自动加车    Activate 1表示 第一次注册                  
                    //if (session.vlsType == 0 && Activate == 1 && !session.isActivate)
                    int OrganizationID = 0; string SiOrganizationID = "";
                    int vlsTypeid = 0; string VehicleTypeID = "";
                    #region 自动加车
                    if (Activate == 1 && !session.isDBExist)
                    {
                        string InsertVehicleInfo = "";
                        string nowTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
                        WriteLog.WriteTestLog("Online", string.Format("Activate-{0}-{1}", session.siCode, nowTime), true);
                        InsertVehicleInfo = session.InsertVehicleInfo.Replace("siCode", session.siCode);
                        InsertVehicleInfo = InsertVehicleInfo.Replace("nowTime", nowTime);
                        if (session.vlsType == 0)
                        {
                            OrganizationID = 0;
                            vlsTypeid = 0;
                        }
                        if (session.vlsType == 14)
                        {
                            OrganizationID = 1;
                            vlsTypeid = 1;
                        }
                        if (session.vlsType == 13)
                        {
                            OrganizationID = 1;
                            vlsTypeid = 2;
                        }
                        if (session.vlsType == 19)
                        {
                            OrganizationID = 1;
                            vlsTypeid = 3;
                        }
                        if (session.vlsType == 7)
                        {
                            OrganizationID = 1;
                            vlsTypeid = 4;
                        }
                        SiOrganizationID = session.OrganizationID.Split(',')[OrganizationID].ToString();
                        VehicleTypeID = session.VehicleTypeID.Split(',')[vlsTypeid].ToString();
                        InsertVehicleInfo = InsertVehicleInfo.Replace("SiOrganizationID", SiOrganizationID);
                        InsertVehicleInfo = InsertVehicleInfo.Replace("vlsType", VehicleTypeID);
                        if (session.Activenum == 0)
                        {
                            session.isActivate = true;
                            session.mqs.SendMsg0(InsertVehicleInfo);
                            session.Activenum++;
                        }
                        try
                        {
                            //自动加车确认回复逻辑:
                            //1.第一次注册时,查询是否存在,存在即返回注册成功
                            //2.不存在,入库,但返回不了 注册成功,因为当时还不知道 是否入库成功(队列延时)
                            //3.再一次注册时,查询确认是否存在, 存在即返回成功.否则 返回 步骤2

                            //获取是否存在db
                            session.isDBExist = SqlDal.CreateInstend().IsDbExists(session.siCode);
                        }
                        catch (Exception ex)
                        {
                            WriteLog.WriteErrorLog("isDBExist", session.siCode + "__" + ex.ToString(), false);
                            session.isDBExist = false;
                        }
                        if (!session.isDBExist)
                        {
                            //string InsertVehicleInfo;
                            //session.isActivate = true;
                            //string nowTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
                            //WriteLog.WriteTestLog("Online", string.Format("Activate-{0}-{1}", session.siCode, nowTime), true);
                            //InsertVehicleInfo = session.InsertVehicleInfo.Replace("siCode", session.siCode);
                            //InsertVehicleInfo = InsertVehicleInfo.Replace("nowTime", nowTime);
                            session.mqs.SendMsg0(InsertVehicleInfo);
                        }
                    }
                    else
                    {
                        session.isDBExist = true;
                    }
                    #endregion
                    if (session.vlsType != -1 && session.isDBExist)
                    {
                        byte[] btemp = SendPttyyd("01", Simno);
                        session.Send(btemp, 0, btemp.Length);

                        //上线  2015/01/06 整理新逻辑  发送到中心服务器  g02  代表上线 1, 下线不变 0
                        //BMK 2015-10-10 修改为 通道打开即上线
                        //BMK 2016-01-04  修改为 注册为上线,避免车多造成数据拥挤
                        if (!session.isSet && session.clsft_bjdb != null && string.IsNullOrEmpty(session.OtherClient))
                        {
                            if (!string.IsNullOrEmpty(session.siCode))
                            {
                                //放到最后面，等session.isacc赋值
                                if (string.IsNullOrEmpty(session.isacc))
                                {
                                    session.isacc = "0";
                                }
                                strinfo = string.Format("insert into vehiclerunninginfo.onlinerecode(SystemNo,OnlineTime,Acc,IsOnline)  VALUES('{0}','{1:yyyy-MM-dd HH:mm:ss}','{2}','1');", session.siCode, DateTime.Now, session.isacc);
                                session.mqs.SendMsg(strinfo);
                                strinfo = Telnet_Session.FomatToJosnByOnLine(session.siCode, "1");
                                session.SendAsToServers(strinfo);
                                WriteLog.WriteTestLog("Online", string.Format("online-{0}-{1:yyyy-MM-dd HH:mm:ss}", session.siCode, DateTime.Now), true);
                            }
                        }
                        //自动加车成功后 推送数据发给卢
                        if (Activate == 1)
                        {
                            strinfo = "{\"key\":\"client_message\",\"sendType\":\"2\",\"receiver\":\"\",\"identifying\":\"G032\",\"data\":{\"systemNo\":\"" + session.siCode + "\",\"VehicleNo\":\"" + session.siCode + "\",\"OrganizationID\":\"" + SiOrganizationID + "\"}";
                            session.SendAsToServers(strinfo);
                        }
                    }
                    else
                    {
                        WriteLog.WriteErrorLog("NovlsType", session.siCode + "__" + sendCommand, false);
                    }

                    //foreach (System.Reflection.PropertyInfo p in session.cls_register.GetType().GetProperties())
                    //{
                    //    if (!p.PropertyType.IsGenericType)
                    //    {
                    //        keystr += string.Format(",`{0}`", p.Name);
                    //        valuestr += string.Format(",'{0}'", p.GetValue(session.cls_register, null));
                    //    }
                    //}
                }
                #endregion

                #region  查询命令回复 80, 参数设置命令回复 81
                if (strindex.Equals("80") || strindex.Equals("81"))
                {
                    string ident = strindex.Equals("80") ? "G26" : "G25";
                    string signalName = "";

                    string setdata = PublicMethods.GetGMT8Data(desc.Substring(0, 17), 1);
                    int setnum = PublicMethods.Get16To10(desc.Substring(18, 2));
                    string ParametersId = "", ParametersValue = "";
                    string info = desc.Substring(21, desc.Length - 21);
                    int blength = 0;
                    for (int i = 0; i < setnum; i++)
                    {
                        ParametersId = info.Substring(0, 2);
                        blength = 1;//参数id

                        if (ParametersId.Equals("06") || ParametersId.Equals("07"))//5个字节
                        {
                            blength += 5;
                            ParametersValue = PublicMethods.GetHexToAscii(info.Substring(3, 14));
                        }
                        else if (ParametersId.Equals("08"))//1个字节 车载终端心跳发送周期
                        {
                            blength += 1;
                            ParametersValue = PublicMethods.Get16To10(info.Substring(3, 2)).ToString();
                        }
                        else if (ParametersId.Equals("04"))//6个字节  综合平台IP 地址
                        {
                            blength += 6;
                            string[] ipv4 = info.Substring(3, 17).Substring(6).Split(' ');
                            ParametersValue = string.Format("{0}.{1}.{2}.{3}", PublicMethods.Get16To10(ipv4[0]), PublicMethods.Get16To10(ipv4[1]), PublicMethods.Get16To10(ipv4[2]), PublicMethods.Get16To10(ipv4[3]));

                            ident = "G22";
                        }
                        else//2个字节
                        {
                            blength += 2;
                            ParametersValue = PublicMethods.Get16To10(info.Substring(3, 5)).ToString();
                        }
                        string[] infodesc = { "", "LocalStorageTime", "InformationReportedTime", "ErrorInformationReportedTime", "IPAddress", "Port", "HardwareVersion", "FirmwareVersion", "HeartbeatCycle", "TerminalResponseTimeout", "PlatformResponseTimeout", "" };

                        signalName += "And" + infodesc[PublicMethods.Get16To10(ParametersId)];


                        //如果开始有and, 去掉
                        if (signalName.IndexOf("And", 0) == 0)
                        {
                            signalName = signalName.Substring(3);
                        }

                        WriteLog.WriteLogZLOther("80", ParametersId + "_" + ParametersValue);
                        if (info.Length > 3 * blength)
                        {
                            info = info.Substring(3 * blength);
                        }
                    }

                    //0成功,1失败
                    int result = (session.clsft_bjdb.Ml_yd).Equals("01") ? 0 : 1;
                    buffer.Clear();
                    buffer.AppendFormat("\"signalName\":\"{0}\",", signalName);
                    buffer.AppendFormat("\"time\":\"{0}\",", setdata);
                    buffer.AppendFormat("\"sysnos\":\"{0}\",", session.siCode);
                    buffer.AppendFormat("\"result\":\"{0}\"", result);
                    strinfo = Telnet_Session.FomatToJosn("client_message", "2", session.siCode, ident, buffer.ToString());
                    //指令 设置回执
                    session.SendToReply(strinfo);


                }
                #endregion
                #region  车载终端控制命令回复 82
                else if (strindex.Equals("82"))
                {
                    string resulttemp = session.clsft_bjdb.Ml_yd;
                    //0成功,1失败
                    int result = resulttemp.Equals("01") ? 0 : 1;

                    string setdata = PublicMethods.GetGMT8Data(desc.Substring(0, 17), 1);
                    int setnum = PublicMethods.Get16To10(desc.Substring(18, 2));

                    // 成功或失败 消息发送给平台
                    string[] infodesc = { "", "RemoteUpgrade", "TerminalShutdown", "TerminalReset", "RestoreFactorySettings", "DisconnectCommunications", "" };

                    buffer.Clear();
                    buffer.AppendFormat("\"signalName\":\"{0}\",", infodesc[setnum]);
                    buffer.AppendFormat("\"time\":\"{0:yyyy-MM-dd HH:mm:ss}\",", DateTime.Now);
                    buffer.AppendFormat("\"sysnos\":\"{0}\",", session.siCode);
                    buffer.AppendFormat("\"result\":\"{0}\"", result);
                    strinfo = Telnet_Session.FomatToJosn("client_message", "2", session.siCode, "G24", buffer.ToString());
                    //指令 设置回执
                    session.SendToReply(strinfo);
                }
                #endregion
                #region  C0 -分时租赁 回复
                else if (strindex.Equals("C0"))
                {
                    string ydlsh = desc.Substring(0, 5);
                    //地标中 01 成功,02失败 FE
                    //部标中 0：成功/确认；1：失败；2：消息有误；3：不支持
                    string result = session.clsft_bjdb.Ml_yd.Equals("01") ? "00" : "01";
                    if (!result.Equals("FE"))
                    {
                        buffer.Clear();
                        buffer.Append("\"systemNo\":\"" + session.siCode + "\",");
                        if (ydlsh.Equals("00 01") || ydlsh.Equals("01 01")) //开门     //01 01   01开头,是Other客户端发送标识
                        {
                            buffer.Append("\"signalName\":\"takeCarReply\",");
                        }
                        if (ydlsh.Equals("00 02") || ydlsh.Equals("01 02")) //关门
                        {
                            buffer.Append("\"signalName\":\"returnCarReply\",");
                        }
                        if (ydlsh.Equals("00 03") || ydlsh.Equals("01 03")) //开门
                        {
                            buffer.Append("\"signalName\":\"pushDoorReply\",");
                        }
                        if (ydlsh.Equals("00 04") || ydlsh.Equals("01 04")) //关门
                        {
                            buffer.Append("\"signalName\":\"shutDownReply\",");
                        }
                        buffer.Append("\"value\":\"" + result + "\"");
                        strinfo = Telnet_Session.FomatToJosn("client_message_lease", "2", session.siCode, "G18", buffer.ToString());

                        if (ydlsh.Contains("01 0")) //区别出客户端点击
                        {
                            //BMK添加其他客户端 2015-10-08
                            foreach (var s in session.AppServer.GetAllSessions())
                            {
                                if (!string.IsNullOrEmpty(s.OtherClient))
                                {
                                    if (s.OtherClient.Equals("android"))
                                    {
                                        s.Send(strinfo);
                                        WriteLog.WriteLogZLOther(s.OtherClient, strinfo, true);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // 开关门 回复
                            session.SendToReply(strinfo);

                            //记录指令回复
                            WriteLog.WriteLogZL("retrunRecv:" + strinfo);
                        }
                    }
                }
                #endregion
                //todotest    [正式中删除]
                // session.vlsType = 5;
                ////无车型,不解析 [正式中启用]
                if (session.vlsType == -1)
                {
                    WriteLog.WriteErrorLog("NovlsType", session.siCode, false);
                    return;
                }

                #region  终端心跳 √
                if (strindex.Equals("04"))
                {
                    //byte[] btemp = SendPttyyd(session.clsft_bjdb);
                    //session.Send(btemp, 0, btemp.Length);

                    byte[] btemp = SendPttyyd("04", Simno);
                    session.Send(btemp, 0, btemp.Length);
                }
                #endregion
                #region  实时信息上报 || 补发信息上报
                else if (strindex.Equals("02") || strindex.Equals("05"))
                {
                    //补发
                    if (strindex.Equals("05")) { session.isSupplement = true; } else { session.isSupplement = false; }
                    //ThreadPool.QueueUserWorkItem(new WaitCallback(session.ProtocolFormat), new ThreadObject { tsesion = session, index = 1, bytestr = desc,bytefrist=se.Substring(0,66) });
                    #region MyRegion
                    #region MyRegion
                    List<string> lis = new List<string>();
                    string jeams = "";
                    try
                    {
                        //采集时间
                        session.cls_bjdb.Date = PublicMethods.GetGMT8Data(desc.Substring(0, 17), 1);
                        //PublicMethods.TimeDetermine(session.siCode, session.cls_bjdb.Date, out dtimeput);

                        if (desc.Length < 18) { return; }

                        //信息
                        string reportinfos = desc.Substring(18, desc.Length - 18);
                        if (session.vlsType != -1) //系统不支持此车型
                        {
                            //标识位
                            string bsw, info = reportinfos, zcinfo;
                            bool flagJump = false;
                            ///      平台是否支持此车型
                            bool flagvlsType;//平台是否支持此车型
                            while (info.Length != 0)
                            {
                                flagvlsType = true;
                                if (flagJump) break;
                                bsw = info.Substring(0, 2);
                                zcinfo = info.Substring(3, info.Length - 3);
                                //此协议 无信息长度
                                // num = nu * 3 - 1;
                                switch (bsw)
                                {
                                    #region 01-单体蓄电池电压数据 √
                                    case "01":
                                        {
                                            int num = 2;
                                            int startindexnum = 0;
                                            session.cls_sbv = session.getClsSBV();
                                            session.cls_sbv.SingleBattery = PublicMethods.Get16To10(zcinfo.Substring(0, 5));
                                            //if (session.cls_sbv.SingleBattery > 0)
                                            //{
                                            num++;//加上单体蓄电池包总数
                                            session.cls_sbv.SingleBatteryPackTotal = PublicMethods.Get16To10(zcinfo.Substring(6, 2));
                                            session.cls_sbv.lis_vvl = session.getClsLstVvl();
                                            string tempTValue;
                                            int numlength = 0;
                                            zcinfo = zcinfo.Substring(9);
                                            for (int i = 0; i < session.cls_sbv.SingleBatteryPackTotal; i++)
                                            {
                                                zcinfo = zcinfo.Substring(startindexnum, zcinfo.Length - startindexnum);
                                                session.cls_vvl = session.getClsVvl();
                                                session.cls_vvl.PowerBatteryPackNumber = PublicMethods.Get16To10(zcinfo.Substring(0, 2));
                                                session.cls_vvl.PowerBatteryTotal = PublicMethods.Get16To10(zcinfo.Substring(3, 2));
                                                tempTValue = zcinfo.Substring(6, zcinfo.Length - 6);
                                                for (int j = 0; j < session.cls_vvl.PowerBatteryTotal; j++)
                                                {
                                                    double temperature = Math.Round(PublicMethods.Get16To10(tempTValue.Substring(0, 5)) * 0.001, 3);
                                                    //多电池多电压 
                                                    //if (vlstypes.Contains(session.vlsType)) //比亚迪 卡车-客车
                                                    //{
                                                    jeams += string.Format("${0}^{1}", string.Format("BatteryPack{0}MonomerVoltage{1}", session.cls_vvl.PowerBatteryPackNumber, (j + 1)), temperature);
                                                    //}
                                                    session.cls_vvl.BatteryVoltage += "," + temperature;
                                                    if (j < session.cls_vvl.PowerBatteryTotal - 1)
                                                    {
                                                        tempTValue = tempTValue.Substring(6, tempTValue.Length - 6);
                                                    }
                                                }
                                                //去掉开始逗号
                                                session.cls_vvl.BatteryVoltage = session.cls_vvl.BatteryVoltage.Substring(1, session.cls_vvl.BatteryVoltage.Length - 1);
                                                session.cls_sbv.lis_vvl.Add(session.cls_vvl);
                                                //每个包的字节数
                                                numlength = 2 + session.cls_vvl.PowerBatteryTotal * 2;
                                                //包长
                                                startindexnum = numlength * 3;
                                                num += numlength;
                                            }

                                            // }
                                            // 添加报文id
                                            num = (num + 1) * 3;
                                            startindexnum = num;
                                            if (info.Length > startindexnum)
                                            {
                                                info = info.Substring(startindexnum, info.Length - startindexnum);
                                            }
                                            else
                                            {
                                                info = "";
                                            }

                                            session.clsri = session.cls_sbv;
                                            break;
                                        }
                                    #endregion
                                    #region 02-动力蓄电池包温度数据 √
                                    case "02":
                                        {
                                            int num = 3;
                                            int startindexnum = 0;
                                            session.cls_pbp = session.getClsPBP();
                                            session.cls_pbp.BatteryTemperatureTotal = PublicMethods.Get16To10(zcinfo.Substring(0, 5));
                                            session.cls_pbp.PowerBatteryPackTotal = PublicMethods.Get16To10(zcinfo.Substring(6, 2));

                                            session.cls_pbp.lis_tvl = session.getClsLstTvl();
                                            string tempTValue;
                                            int numlength = 0;
                                            zcinfo = zcinfo.Substring(9);
                                            for (int i = 0; i < session.cls_pbp.PowerBatteryPackTotal; i++)
                                            {
                                                zcinfo = zcinfo.Substring(startindexnum, zcinfo.Length - startindexnum);
                                                session.cls_tvl = session.getClsTvl();
                                                session.cls_tvl.PowerBatteryPackNumber = PublicMethods.Get16To10(zcinfo.Substring(0, 2));
                                                session.cls_tvl.TemperatureProbeTotal = PublicMethods.Get16To10(zcinfo.Substring(3, 2));
                                                tempTValue = zcinfo.Substring(6, zcinfo.Length - 6);
                                                for (int j = 0; j < session.cls_tvl.TemperatureProbeTotal; j++)
                                                {
                                                    int temperature = PublicMethods.Get16To10(tempTValue.Substring(0, 2)) - 40;
                                                    //多电池多电压 
                                                    //if (vlstypes.Contains(session.vlsType)) //比亚迪 卡车-客车
                                                    //{
                                                    jeams += string.Format("${0}^{1}", string.Format("BatteryPack{0}ProbeTemp{1}", session.cls_tvl.PowerBatteryPackNumber, (j + 1)), temperature);
                                                    //}
                                                    session.cls_tvl.ProbeTemperature += "," + temperature;
                                                    if (j < session.cls_tvl.TemperatureProbeTotal - 1)
                                                    {
                                                        tempTValue = tempTValue.Substring(3, tempTValue.Length - 3);
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(session.cls_tvl.ProbeTemperature))
                                                {
                                                    //去掉开始逗号
                                                    session.cls_tvl.ProbeTemperature = session.cls_tvl.ProbeTemperature.Substring(1, session.cls_tvl.ProbeTemperature.Length - 1);
                                                }
                                                session.cls_pbp.lis_tvl.Add(session.cls_tvl);
                                                //每个包的字节数
                                                numlength = 2 + session.cls_tvl.TemperatureProbeTotal;
                                                //包长
                                                startindexnum = numlength * 3;
                                                num += numlength;
                                            }
                                            // 添加报文id
                                            num = (num + 1) * 3;
                                            startindexnum = num;
                                            if (info.Length > startindexnum)
                                            {
                                                info = info.Substring(startindexnum, info.Length - startindexnum);
                                            }
                                            else
                                            {
                                                info = "";
                                            }

                                            session.clsri = session.cls_pbp;

                                            break;
                                        }
                                    #endregion
                                    #region 03-整车数据
                                    case "03":
                                        {
                                            session.cls_vls = session.getClsVLS();
                                            session.cls_vls.ABS_VehSpd = Math.Round(PublicMethods.Get16To10(zcinfo.Substring(0, 5)) * 0.1d, 2);
                                            session.cls_vls.IC_TotalOdmeter = Math.Round(PublicMethods.Get16To10(zcinfo.Substring(6, 11)) * 0.1d, 2);
                                            string temps = PublicMethods.Get16To2(zcinfo.Substring(18, 2), 1).Substring(4, 4);
                                            //"0000"://空档,"0001"://1档,"0010"://2档,"0011"://3档,"0100"://4档,"0101"://5档,"1110"://倒档,"1111"://自动档
                                            session.cls_vls.TCU_GearPosition = PublicMethods.Get2To10(temps);

                                            session.cls_vls.AcceleratingTravelPer = PublicMethods.Get16To10(zcinfo.Substring(21, 2));
                                            session.cls_vls.BrakeTravelPer = PublicMethods.Get16To10(zcinfo.Substring(24, 2));
                                            //充放电状态
                                            temps = zcinfo.Substring(27, 2);
                                            switch (temps)
                                            {
                                                case "01": //充电
                                                    {
                                                        session.cls_vls.BMS_ChargeSt = "1";
                                                        break;
                                                    }
                                                case "02"://非充电
                                                    {
                                                        //session.cls_vls.BMS_ChargeSt = "2";
                                                        session.cls_vls.BMS_ChargeSt = "0";
                                                        break;
                                                    }
                                                case "FF"://数据库定义的无效数据标识
                                                    {
                                                        //session.cls_vls.BMS_ChargeSt = "3";
                                                        session.cls_vls.BMS_ChargeSt = "4";
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        session.cls_vls.BMS_ChargeSt = "4";
                                                        break;
                                                    }
                                            }
                                            //电机控制器温度
                                            session.cls_vls.Motor_ControllerTemp = PublicMethods.Get16To10(zcinfo.Substring(30, 2)) - 40;
                                            //电机转速
                                            session.cls_vls.Motor_Revolution = PublicMethods.Get16To10(zcinfo.Substring(33, 5));
                                            //电机温度
                                            session.cls_vls.Motor_Temperature = PublicMethods.Get16To10(zcinfo.Substring(39, 2)) - 40;
                                            //电机电压
                                            session.cls_vls.Motor_Voltage = Math.Round(PublicMethods.Get16To10(zcinfo.Substring(42, 5)) * 0.1d, 2);
                                            //电机电流
                                            session.cls_vls.Motor_Current = Math.Round(PublicMethods.Get16To10(zcinfo.Substring(48, 5)) * 0.1d - 1000, 2);
                                            //空调设定温度
                                            session.cls_vls.AirSettingTemperature = PublicMethods.Get16To10(zcinfo.Substring(54, 2));

                                            //取startindex, 以0字节为第一个字节, 总共有5个字节,取第6个字节 n=5 3n=15 
                                            //此时 26*3=78 已经包括了后面的空格
                                            //由于 26个字节未包含 报文id,所以继续添加一个  27*3=81
                                            // 00 04 00  (04 00 的startindex为81, 但这个包只传了整车 ,所以 得过滤)
                                            int startindexnum = 81;
                                            if (info.Length > startindexnum)
                                            {
                                                info = info.Substring(startindexnum, info.Length - startindexnum);
                                            }
                                            else
                                            {
                                                info = "";
                                            }

                                            session.clsri = session.cls_vls;
                                            break;
                                        }
                                    #endregion
                                    #region 04-定位数据
                                    case "04":
                                        {
                                            session.cls_ptd = session.getClsPTD();
                                            string IsLocation = PublicMethods.Get16To2(zcinfo.Substring(0, 2), 1);
                                            //因为平台中 1为定位,0为未定位
                                            //本协议 1为未定位
                                            session.cls_ptd.IsLocation = IsLocation.Substring(7, 1).Equals("0") ? "1" : "0";
                                            session.cls_ptd.SouthLatitude = IsLocation.Substring(6, 1);
                                            session.cls_ptd.EastWest = IsLocation.Substring(5, 1);
                                            session.cls_ptd.Longitude = Math.Round(PublicMethods.Get16To10(zcinfo.Substring(3, 11)) / 1000000d, 6);
                                            session.cls_ptd.Latitude = Math.Round(PublicMethods.Get16To10(zcinfo.Substring(15, 11)) / 1000000d, 6);


                                            //BMK经纬度处理
                                            if (session.cls_ptd.Longitude > 0)
                                            {
                                                session.EffectiveLong = session.cls_ptd.Longitude;
                                                session.EffectiveLat = session.cls_ptd.Latitude;
                                            }

                                            session.cls_ptd.Speed = Math.Round(PublicMethods.Get16To10(zcinfo.Substring(27, 5)) * 0.1d, 2);
                                            session.cls_ptd.Direction = PublicMethods.Get16To10(zcinfo.Substring(33, 5));
                                            //1 4 4 2 2 4=17
                                            int startindexnum = 54;
                                            if (info.Length > startindexnum)
                                            {
                                                info = info.Substring(startindexnum, info.Length - startindexnum);
                                            }
                                            else
                                            {
                                                info = "";
                                            }

                                            session.clsri = session.cls_ptd;

                                            break;
                                        }
                                    #endregion
                                    #region 05-极值数据
                                    case "05":
                                        {
                                            session.cls_exd = session.getClsEXD();
                                            session.cls_exd.MaximumVoltageBatteryPackNumber = PublicMethods.Get16To10(zcinfo.Substring(0, 2));
                                            session.cls_exd.MaximumVoltageSingleBatteryNumber = PublicMethods.Get16To10(zcinfo.Substring(3, 2));
                                            session.cls_exd.BMS_MaxCellBatt = Math.Round(PublicMethods.Get16To10(zcinfo.Substring(6, 5)) * 0.001d, 2);
                                            session.cls_exd.LowestVoltageBatteryPackNumber = PublicMethods.Get16To10(zcinfo.Substring(12, 2));
                                            session.cls_exd.LowestVoltageSingleBatteryPackNumber = PublicMethods.Get16To10(zcinfo.Substring(15, 2));
                                            session.cls_exd.BMS_MinCellBatt = Math.Round(PublicMethods.Get16To10(zcinfo.Substring(18, 5)) * 0.001d, 2);
                                            session.cls_exd.MaximumTemperatureBatteryPackNumber = PublicMethods.Get16To10(zcinfo.Substring(24, 2));
                                            session.cls_exd.MaximumTemperatureProbeNumber = PublicMethods.Get16To10(zcinfo.Substring(27, 2));

                                            session.cls_exd.BMS_Temp_Max = PublicMethods.Get16To10(zcinfo.Substring(30, 2)) - 40;
                                            session.cls_exd.LowestTemperatureBatteryPackNumber = PublicMethods.Get16To10(zcinfo.Substring(33, 2));
                                            session.cls_exd.LowestTemperatureProbeNumber = PublicMethods.Get16To10(zcinfo.Substring(36, 2));
                                            session.cls_exd.BMS_Temp_Min = PublicMethods.Get16To10(zcinfo.Substring(39, 2)) - 40;

                                            session.cls_exd.BMS_Voltage = Math.Round(PublicMethods.Get16To10(zcinfo.Substring(42, 5)) * 0.1d, 2);
                                            session.cls_exd.BMS_Current = Math.Round(PublicMethods.Get16To10(zcinfo.Substring(48, 5)) * 0.1d - 1000, 2);
                                            session.cls_exd.BMS_SOC = Math.Round(PublicMethods.Get16To10(zcinfo.Substring(54, 2)) * 0.4d, 2);
                                            session.cls_exd.ResidualCapacity = Math.Round(PublicMethods.Get16To10(zcinfo.Substring(57, 5)) * 0.1d, 2);
                                            session.cls_exd.InsulationResistance = PublicMethods.Get16To10(zcinfo.Substring(63, 5));

                                            int startindexnum = 87;
                                            if (info.Length > startindexnum)
                                            {
                                                info = info.Substring(startindexnum, info.Length - startindexnum);
                                            }
                                            else
                                            {
                                                info = "";
                                            }

                                            session.clsri = session.cls_exd;
                                            break;
                                        }
                                    #endregion
                                    #region 06-报警数据
                                    case "06":
                                        {
                                            int num = 0;
                                            session.cls_armd = session.getClsArmd();
                                            //动力蓄电池报警
                                            string PowerBatteryWarningSign = PublicMethods.Get16To2(zcinfo.Substring(0, 5), 2);
                                            session.cls_armd.PowerBatteryWarningSign = "";
                                            for (int j = 0; j < PowerBatteryWarningSign.Length; j++)
                                            {
                                                if (PowerBatteryWarningSign[j] != '0')
                                                    session.cls_armd.PowerBatteryWarningSign += PowerBatteryWarningSign.Length - 1 - j + "_";
                                            }
                                            //故障
                                            if (!string.IsNullOrEmpty(session.cls_armd.PowerBatteryWarningSign))
                                            {
                                                faultlever = 1; int aramindex = 0; string singnamefault = "";
                                                foreach (var item in session.cls_armd.PowerBatteryWarningSign.Split('_'))
                                                {
                                                    if (!string.IsNullOrEmpty(item))
                                                    {
                                                        aramindex = int.Parse(item);
                                                        if (aramindex < PowerBatteryWarningSignlistName.Length)
                                                        {
                                                            singnamefault = PowerBatteryWarningSignlistName[aramindex];
                                                            if (!string.IsNullOrEmpty(singnamefault))
                                                            {
                                                                canfault += string.Format("${0}:{1}:{2}:{3}", singnamefault, "0X" + zcinfo.Substring(0, 5).Replace(" ", "") + "-bit" + aramindex, faultlever, aramindex);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            //总值 _1_  不入库
                                            session.cls_armd.PowerBatteryWarningSign = "";

                                            #region 动力蓄电池报警标志
                                            session.cls_armd.TemperatureDifferenceAlarm = int.Parse(PowerBatteryWarningSign.Substring(15));
                                            session.cls_armd.BatteryPoleHighTemperature = int.Parse(PowerBatteryWarningSign.Substring(14, 1));
                                            session.cls_armd.BMS_VoltageTooHigh = int.Parse(PowerBatteryWarningSign.Substring(13, 1));
                                            session.cls_armd.BMS_VoltageTooLow = int.Parse(PowerBatteryWarningSign.Substring(12, 1));
                                            session.cls_armd.SOCLowAlarm = int.Parse(PowerBatteryWarningSign.Substring(11, 1));
                                            session.cls_armd.BatteryOverVoltage = int.Parse(PowerBatteryWarningSign.Substring(10, 1));
                                            session.cls_armd.MonomerBatteryUnderVoltage = int.Parse(PowerBatteryWarningSign.Substring(9, 1));
                                            session.cls_armd.SOCTooLowAlarm = int.Parse(PowerBatteryWarningSign.Substring(8, 1));
                                            session.cls_armd.SOCOvertopAlarm = int.Parse(PowerBatteryWarningSign.Substring(7, 1));
                                            session.cls_armd.BatteryPackNotMatch = int.Parse(PowerBatteryWarningSign.Substring(6, 1));
                                            session.cls_armd.BatteriesConsistencyPoor = int.Parse(PowerBatteryWarningSign.Substring(5, 1));
                                            session.cls_armd.InsulationFault = int.Parse(PowerBatteryWarningSign.Substring(4, 1));
                                            #endregion

                                            num = 2;
                                            int startindex = 9;
                                            //动力蓄电池其他故障
                                            int totalPower = PublicMethods.Get16To10(zcinfo.Substring(6, 2));
                                            session.cls_armd.OtherPowerBatteriesTotal = totalPower;
                                            if (totalPower > 0)
                                            {
                                                string[] MotorFaultList = zcinfo.Substring(9, totalPower * 3 - 1).Split(' ');
                                                startindex = 9 + totalPower * 3 - 1 + 1;
                                                //字节数
                                                num = num + totalPower;
                                                int aramtemp = 0; string araminfo;
                                                for (int i = 0; i < totalPower; i++)
                                                {
                                                    araminfo = MotorFaultList[i];
                                                    aramtemp = PublicMethods.Get16To10(araminfo);
                                                }

                                            }
                                            num = num + 1;
                                            //电机故障
                                            int totalMotor = PublicMethods.Get16To10(zcinfo.Substring(startindex, 2));
                                            startindex = startindex + 2 + 1;
                                            session.cls_armd.MotorFaultTotal = totalMotor;
                                            if (totalMotor > 0)
                                            {
                                                string[] OtherPowerBatteriesList = zcinfo.Substring(startindex, totalMotor * 3 - 1).Split(' ');
                                                startindex = startindex + totalMotor * 3 - 1 + 1;
                                                //字节数
                                                num = num + totalMotor;
                                                int aramtemp = 0;
                                                for (int i = 0; i < totalMotor; i++)
                                                {
                                                    aramtemp = PublicMethods.Get16To10(OtherPowerBatteriesList[i]);
                                                }
                                            }
                                            num = num + 1;

                                            //其他故障
                                            int totalOther = PublicMethods.Get16To10(zcinfo.Substring(startindex, 2));
                                            startindex = startindex + 2 + 1;
                                            session.cls_armd.OtherFaultTotal = totalOther;
                                            if (totalOther > 0)
                                            {
                                                string[] OtherFaultList = zcinfo.Substring(startindex, totalOther * 3 - 1).Split(' ');
                                                startindex = startindex + totalOther * 3 - 1 + 1;
                                                //字节数
                                                num = num + totalOther;
                                                int aramtemp = 0;
                                            }
                                            num = num + 1;

                                            // 添加报文id
                                            num = (num + 1) * 3;
                                            int startindexnum = num;
                                            if (info.Length > startindexnum)
                                            {
                                                info = info.Substring(startindexnum, info.Length - startindexnum);
                                            }
                                            else
                                            {
                                                info = "";
                                            }

                                            session.clsri = session.cls_armd;

                                            break;
                                        }
                                    #endregion
                                    #region 80-扩展数据(海马_A60_E30)
                                    case "80":
                                        {
                                            //标识位
                                            string info1 = zcinfo, zcinfo1;
                                            int countnum = PublicMethods.Get16To10(info1.Substring(0, 5));//包长
                                            if (countnum > 1) //包长是指整个自定义, 那么他要么为0,要么大于1,因为 子信息体个数 会占1
                                            {
                                                //子信息体个数
                                                int msgnumber = PublicMethods.Get16To10(info1.Substring(6, 2));
                                                //所有扩展
                                                info1 = info1.Substring(9, info1.Length - 9);
                                                //string channelNumber, packageNumber;
                                                int dataLength = PublicMethods.Get16To10(info1.Substring(15, 2));

                                                if (session.vlsType == 0)//海马
                                                {
                                                    //session.clsri = new ParsBLStandard().GetParsbyBLStandard(session, info1, msgnumber, dataLength, ref  canfault);
                                                    session.clsri = ParsBLStandard.GetParsbyBLStandard(session, info1, msgnumber, dataLength, ref  canfault);
                                                    // session.clsri = session.cls_hme;
                                                }
                                                else if (session.vlsType == 1)//A60
                                                {
                                                    session.clsri = ParsA60.GetParsbyA60(session, info1, msgnumber, dataLength, ref  canfault);
                                                    //session.clsri = session.cd_a60;
                                                }
                                                else if (session.vlsType == 2)//E30
                                                {
                                                    session.clsri = ParsE30.GetParsbyE30(session, info1, msgnumber, dataLength, ref  canfault);
                                                    //session.clsri = session.cd_a60;
                                                }
                                                else if (session.vlsType == 5)//DR95C
                                                {
                                                    //记录
                                                }
                                                else
                                                {
                                                    //暂不支持此车型
                                                    flagvlsType = false;
                                                }
                                            }

                                            // 添加报文id
                                            int num = 3 * (countnum + 3); //3 为 指令的长度
                                            int startindexnum = num;
                                            if (info.Length > startindexnum)
                                            {
                                                info = info.Substring(startindexnum, info.Length - startindexnum);
                                            }
                                            else
                                            {
                                                info = "";
                                            }

                                            break;
                                        }

                                    #endregion
                                    #region 81-EVT_自定义数据[不管车型]
                                    case "81":
                                        {
                                            //标识位
                                            string info1 = zcinfo;
                                            int countnum = PublicMethods.Get16To10(info1.Substring(0, 5));//包长
                                            if (countnum > 1) //包长是指整个自定义, 那么他要么为0,要么大于1,因为 子信息体个数 会占1
                                            {
                                                //子信息体个数
                                                int msgnumber = PublicMethods.Get16To10(info1.Substring(6, 2));
                                                //所有扩展
                                                info1 = info1.Substring(9, info1.Length - 9);
                                                //string channelNumber, packageNumber;
                                                //int dataLength = PublicMethods.Get16To10(info1.Substring(15, 2));

                                                //if (session.vlsType == 5)//DR95C
                                                //{
                                                session.clsri = ParsEVT.GetParsEVT(session, info1, msgnumber, ref  canfault);
                                                //}
                                                //else
                                                //{
                                                //    //暂不支持此车型
                                                //    flagvlsType = false;
                                                //}
                                            }

                                            // 添加报文id
                                            int num = 3 * (countnum + 3); //3 为 指令的长度
                                            int startindexnum = num;
                                            if (info.Length > startindexnum)
                                            {
                                                info = info.Substring(startindexnum, info.Length - startindexnum);
                                            }
                                            else
                                            {
                                                info = "";
                                            }
                                            break;
                                        }

                                    #endregion
                                    default:
                                        {
                                            flagJump = true;
                                            info = "";
                                            break;
                                        }
                                }
                                if (!bsw.Equals("81"))
                                {
                                    WriteLog.WriteLogMeaning("DB", zcinfo, session.clsri);
                                }
                                //GPS的值 不用行入库
                                string[] Filtered = { "IsLocation", "Longitude", "Latitude", "Speed", "Direction", "SouthLatitude", "EastWest" };
                                //BMK 2016-01-08 A60除自定义  其他不用入库 [因为:所有值堵在自定义里面发送了, 没有摘取到 地标中, 所以避免 2次入库]
                                bool isConform = false;
                                if ((session.vlsType == 1 || session.vlsType == 2) && (bsw.Equals("80") || bsw.Equals("81")))
                                {
                                    isConform = true;
                                }
                                if (session.vlsType != 1 && session.vlsType != 2) //除A60和E30 实时上传全部入库
                                {
                                    isConform = true;
                                }
                                //BMk 2016-01-14 A60修改 不过滤地标, 过滤自定义重复部分
                                //[嵌入式还没更新,暂且注释]
                                try
                                {
                                    //if (session.clsri != null&& flagvlsType)
                                    if (session.clsri != null && isConform && flagvlsType)
                                    {
                                        foreach (System.Reflection.PropertyInfo p in session.clsri.GetType().GetProperties())
                                        {
                                            //不是E0,却包含E0的报警,过滤
                                            if (session.vlsType != 10 && OtherFaultListStyle10.Contains(p.Name)) continue;
                                            //判断是否是 非泛型
                                            if (p.PropertyType.IsGenericType) continue;
                                            //过滤gps字段
                                            if (Filtered.Contains(p.Name)) continue;
                                            //过滤 重复 singalName 
                                            //   if (jeams.Contains(string.Format("${0}^",p.Name))) continue;
                                            //过滤为null的值
                                            if (p.GetValue(session.clsri, null) != null)
                                            {
                                                jeams += string.Format("${0}^{1}", p.Name, p.GetValue(session.clsri, null));
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                {


                                }

                            }
                            //GPS入库
                            jeams += string.Format("${0}^{1}", "Gps_Location", "");
                        }
                        else
                        {
                            //没有车型
                            WriteLog.WriteTestLog("Online", string.Format("error-{0}-{1:yyyy-MM-dd HH:mm:ss}", session.siCode, DateTime.Now), true);
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteErrorLog("Error", "______02:" + ex.ToString(), true);
                        WriteLog.WriteErrorLog("Error", "_____原始码:" + sendCommand, true);
                    }

                    try
                    {
                        IDictionary<string, string> dicRepeat = new Dictionary<string, string>();
                        if (!string.IsNullOrEmpty(jeams))
                        {
                            jeams = jeams.Substring(1, jeams.Length - 1);
                            string[] pairs = jeams.Split('$');
                            for (int i = 0; i < pairs.Count(); i++)
                            {
                                //A60全部通过扩展发送,会与地标 存在信号重合部分
                                //if (lis.Contains(pairs[i].Split('^')[0]))
                                //{
                                //    dicRepeat.Add(pairs[i].Split('^')[0], pairs[i].Split('^')[1]);
                                //    continue;
                                //}
                                if (!lis.Contains(pairs[i]))
                                {
                                    lis.Add(pairs[i]);
                                }
                            }
                        }
                        datetimenow = PublicMethods.GetFormatTme(session.siCode, session.cls_bjdb.Date);
                        #region  //BMK GPS+CAN+故障 入库ion
                        if (session.cls_ptd != null && !string.IsNullOrEmpty(session.siCode))
                        {
                            double mileage = session.cls_vls == null ? 0 : session.cls_vls.IC_TotalOdmeter;
                            string infovalue = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}'", session.siCode, session.EffectiveLong, session.EffectiveLat, session.cls_ptd.Speed, session.cls_ptd.Direction, 0, session.isacc, session.cls_ptd.IsLocation, mileage, 0, datetimenow);

                            //  string tempstr = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}'", session.siCode, session.EffectiveLong, session.EffectiveLat, session.cls_ptd.Speed, session.cls_ptd.Direction, 0, session.isacc, session.cls_ptd.IsLocation, mileage, 0, datetimenow);
                            session.InsertTable(session, infovalue, datetimenow, lis, dicRepeat, canfault);
                            //入库 gpsrecord
                            string infokey = "`SystemNo`,`Longitude`,`Latitude`,`Speed`,`Direction`,`Elevation`,`Acc`,`IsLocation`,`Mileage`,`Oil`,`CurrentTime`,`GPS_OprationLock`,`SouthLatitude`,`EastWest`";
                            infovalue = string.Format("{0},'{1}','{2}','{3}'", infovalue, "", session.cls_ptd.SouthLatitude, session.cls_ptd.EastWest);
                            StringBuilder sbstr = new StringBuilder();
                            sbstr.AppendFormat(" insert into vehiclerunninginfo.gpsrecord ( {0} ) values ( {1} );", infokey, infovalue);
                            if (!string.IsNullOrEmpty(sbstr.ToString()))
                            {
                                session.mqs.SendMsg(sbstr.ToString());
                            }
                            #region GPS位置 发送给中心服务器
                            //发送给中心服务器
                            //if (session.cls_ptd.Longitude > 0 && !session.isSupplement)
                            //BMK 2016-01-26 修改为 0发送,卢处理
                            if (!session.isSupplement)
                            {
                                SignalName = "Gps_Location";
                                strnew = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'", session.siCode, session.EffectiveLong, session.EffectiveLat, session.cls_ptd.Speed, session.cls_ptd.Direction, 0, session.isacc, session.cls_ptd.IsLocation, mileage, 0, datetimenow, SignalName, "");
                                strinfo = Telnet_Session.FomatToJosnByNew(0, "G02", strnew);
                                strinfo = strinfo.Replace("'", "");
                                //GPS位置
                                //session.SendAsToServers( strinfo);
                                //2015-04-21 修改 将大量发送的数据 与 命令 分开
                                session.SendAsToServers(strinfo);
                            }
                            #endregion
                        }
                        #endregion

                        #region 设备详细信息入库
                        session.cls_vlsei = session.getClsVlsEI();
                        string aaa = "", bbb = ""; bool isDeviceRecord = true;
                        if (session.cls_vlsei != null)
                        {
                            foreach (System.Reflection.PropertyInfo p in session.cls_vlsei.GetType().GetProperties())
                            {
                                if (p.GetValue(session.cls_vlsei, null) != null)
                                {
                                    aaa += string.Format(",`{0}`", p.Name);
                                    bbb += string.Format(",'{0}'", p.GetValue(session.cls_vlsei, null));
                                }
                                else isDeviceRecord = false;
                            }
                            if (!session.isDeviceRecord || (isDeviceRecord && !session.isCompleteRecord))
                            {
                                StringBuilder sbstr = new StringBuilder();
                                sbstr.AppendFormat(" insert into vehiclerunninginfo.vehicleattribute ( `SystemNo`{0} ) values ( '{1}'{2} );", aaa, session.siCode, bbb);
                                if (!string.IsNullOrEmpty(sbstr.ToString()))
                                {
                                    session.mqs.SendMsg(sbstr.ToString());
                                    session.isDeviceRecord = true;
                                }
                                session.isCompleteRecord = isDeviceRecord;
                            }
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteErrorLog("Error", "______02:" + ex.ToString(), true);
                        return;
                    }

                    #endregion
                    #endregion
                }
                #endregion
                #region  状态信息上报
                else if (strindex.Equals("03"))
                {
                    session.cls_vlss = session.getClsVlsS();  //4561qqq
                    //采集时间
                    session.cls_bjdb.Date = PublicMethods.GetGMT8Data(desc.Substring(0, 17), 1);
                    //状态标志
                    string StatusFlag = PublicMethods.Get16To2(desc.Substring(18, 2), 1);
                    session.cls_vlss.ElectricityStatu = StatusFlag.Substring(7, 1);
                    //相当于acc
                    session.isacc = session.cls_vlss.ElectricityStatu;

                    session.cls_vlss.PowerSupplyStatu = StatusFlag.Substring(6, 1);
                    session.cls_vlss.CommunicationStatu = StatusFlag.Substring(5, 1);
                    session.cls_vlss.OtherExceptions = StatusFlag.Substring(4, 1);

                    foreach (System.Reflection.PropertyInfo p in session.cls_bjdb.GetType().GetProperties())
                    {
                        keystr += string.Format(",`{0}`", p.Name);
                        valuestr += string.Format(",'{0}'", p.GetValue(session.cls_bjdb, null));
                    }

                    byte[] btemp = SendPttyyd("03", Simno);
                    session.Send(btemp, 0, btemp.Length);

                }
                #endregion

                //记录 从设备中发来的数据
                if (session.vlsType != -1)
                {
                    WriteLog.WritRerecvStrFromClient(session.vlsType, datetimenow + "\r\n" + canstr.ToString());
                }
                if (!string.IsNullOrEmpty(session.siCode))
                {
                    WriteLog.WriteLogrecvStr(session.siCode, datetimenow + "\r\n" + sendCommand);
                }

                #region 上标封装
                if (session.isMarkedVehicles)
                {

                }
                #endregion


            }
            catch (Exception ex)
            {
                WriteLog.WriteErrorLog("LengthError", session.siCode + "__" + sendCommand, false);
                return;
            }
        }


        // string[] PowerBatteryWarningSignlist = { "温度差异报警", "电池极柱高温报警", "动力蓄电池包过压报警", "动力蓄电池包欠压报警", "SOC 低报警", "单体蓄电池过压报警", "单体蓄电池欠压报警", "SOC太低报警", "", "动力蓄电池包不匹配报警", "动力蓄电池一致性差报警", "绝缘故障" };
        string[] PowerBatteryWarningSignlistName = { "TemperatureDifferenceAlarm", "BatteryPoleHighTemperature", "BMS_VoltageTooHigh", "BMS_VoltageTooLow", "SOCTooLowAlarm", "BatteryOverVoltage", "MonomerBatteryUnderVoltage", "SOCTooLowAlarm", "SOCOvertopAlarm", "BatteryPackNotMatch", "BatteriesConsistencyPoor", "InsulationFault" };
        string[] OtherFaultListStyle10 = { "OverTemperature", "ChargeTemperatureLow", "DisChargeTemperatureLow", "HighRangeOfTemperature", "MonomerVoltageHigh", "MonomerVoltageLow", "PressureDifferenceHigh", "TotalBatteryVoltageHigh", "TotalBatteryVoltageLow", "ChargeCurrentHigh", "DisChargeCurrentHigh" };
        string[] MotorFaultList10 = { "OverTemperature", "ChargeTemperatureLow", "DisChargeTemperatureLow", "HighRangeOfTemperature", "MonomerVoltageHigh", "MonomerVoltageLow", "PressureDifferenceHigh", "TotalBatteryVoltageHigh", "TotalBatteryVoltageLow", "ChargeCurrentHigh", "DisChargeCurrentHigh" };



        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }



        /// <summary>
        /// 得到 10进制数据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int Getdecimal(string info)
        {
            return Convert.ToInt32(info, 16);
        }
        /// <summary>
        /// 去0
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public string Gets(string[] infos)
        {
            string info = "";
            for (int i = 0; i < infos.Length; i++)
            {
                if (infos[i].Substring(0, 1).Equals("0"))
                {
                    infos[i] = infos[i].Substring(1, 1);
                }
                if (i == 0)
                {
                    info = infos[i];
                }
                else
                {
                    info += " " + infos[i];
                }
            }

            return info;
        }



        /// <summary>
        /// 转义还原  
        /// </summary>
        /// <param name="info">验证码,消息头,消息体</param>
        /// <returns></returns>
        public string GetZy(string info)
        {
            Regex reg = new Regex("7d 02", RegexOptions.IgnoreCase);
            if (reg.IsMatch(info))
            {
                info = reg.Replace(info, "7e");
            }
            reg = new Regex("7d 01", RegexOptions.IgnoreCase);
            if (reg.IsMatch(info))
            {
                info = reg.Replace(info, "7d");
            }
            return info;
        }

        /// <summary>
        /// 消息封装
        /// </summary>
        /// <param name="info">验证码,消息头,消息体</param>
        /// <returns></returns>
        public string GetXxfz(string info)
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
            return info;
        }

        /// <summary>
        /// 平台通用应答
        /// </summary>
        /// <param name="session.clsft_bjdb">格式类</param>
        /// <returns></returns>
        public byte[] SendPttyyd(string command, string identifier)
        {
            string info = string.Format("{0} 01 {1} 00 00 00", command, PublicMethods.GetFomartZK(identifier, 17));
            info = string.Format("23 23 {0} {1}", info, PublicMethods.GetJy(info.Split(' ')));
            byte[] btemp = strToToHexByte(info);
            return btemp;
        }



    }
}