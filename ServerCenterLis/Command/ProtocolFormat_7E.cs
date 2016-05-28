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
namespace ServerCenterLis
{
    /// <summary>                                                  D:\01.Data\vls.ServerCenterLis\3.0\ServerCenterLis_3.1_20160308_EVT自定义_改故障码\ServerCenterLis\Command\ProtocolFormat_7E.cs
    /// 088/2011,2013
    /// </summary>
    public class ProtocolFormat_7E : CommandBase<Telnet_Session, StringRequestInfo>
    {
        public override string Name
        {
            get { return "__"; }
        }



        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="requestInfo">The request info.</param>
        public override void ExecuteCommand(Telnet_Session session, StringRequestInfo requestInfo)
        {
            /// 故障值  0正常         /// 故障等级    1	轻微故障,2	一般故障,3	严重故障,4	致命故障
            int faultlever = 1, faultint = 0;

            DateTime dtimeput = DateTime.Now;
            //存入文本
            // session.Send("requestInfo.Key:" + requestInfo.Key + "requestInfo.Body:" + requestInfo.Body+"--");


            //string canstr = string.Format("{0} {1}", requestInfo.Key, requestInfo.Body);

            string sendCommand = requestInfo.Body.ToUpper();
            string canstr = sendCommand.Trim();

            //记录 从设备中发来的数据
            if (session.vlsType != -1)
            {
                WriteLog.WritRerecvStrFromClient(session.vlsType, canstr.ToString());
            }

            //  numlog++;
            // WriteLog.WriteLogZLOther("7E", numlog + "_" + canstr, false);

            if (!canstr.Contains("7E"))
            {
                return;
            }

            //string[] str = System.Text.RegularExpressions.Regex.Split(canstr, "(7E )", RegexOptions.IgnoreCase);

            // string se = str[2].ToString().Trim();

             string se = canstr.Substring(3, canstr.Length - 3);

            string strindex = se.Substring(0, 5);

            /**
                SendStr1：7E 02 00 00 30 01 39 71 52 51 94 00 A9 00 00 00 00 00 0C 00 03 01 D0 D3 80 06 D1 FA 06 00 21 00 09 00 00 14 05 05 11 23 44 01 04 00 00 00 00 02 02 00 00 03 02 00 00 25 04 00 00 00 00 8C 7E 
                SendStr1aaa：02 00 00 30 01 39 71 52 51 94 00 A9 00 00 00 00 00 0C 00 03 01 D0 D3 80 06 D1 FA 06 00 21 00 09 00 00 14 05 05 11 23 44 01 04 00 00 00 00 02 02 00 00 03 02 00 00 25 04 00 00 00 00
                 SendStr1se：02 00 00 30 01 39 71 52 51 94 00 A9 00 00 00 00 00 0C 00 03 01 D0 D3 80 06 D1 FA 06 00 21 00 09 00 00 14 05 05 11 23 44 01 04 00 00 00 00 02 02 00 00 03 02 00 00 25 04 00 00 00 00 8C

             **/
            //接受的指令 直接 发送给车机(中转)
            //8201 点名,82 04 ,82 03,8F 00 开门关门,81 03 设置终端参数 ,00 00 为内部客户端发送的空包
            // 00 00 为内部客户端发送的空包,作用:  因为,开启了 空闲清理, 那么内部client 会被清理掉,然后 重连(重连方法也就是启动方法里面带了client的初始化) 被清理,
            //有数据上传时,会根据标识判断是否断开,   发送一个空包 创造出标示 (OtherClient)
            //BMK 2035-12-02 ,去掉  "00 00" 
            string[] orderstr = { "82 01", "82 04", "82 03", "8F 00", "81 03" };
            if (orderstr.Contains(strindex))
            //if (strindex.Equals("82 01") || strindex.Equals("82 04") || strindex.Equals("82 03") || strindex.Equals("8F 00") || strindex.Equals("81 03"))
            {
                string MiddleIndex = se.Substring(se.Length - 8, 5);
                if (MiddleIndex.Equals(session.MiddleIndex))
                {
                    session.OtherClient = "center";
                    session.isSet = true;
                    sendCommand = sendCommand.Replace(MiddleIndex + " ", "");
                    WriteLog.WriteLogZLOther(session.OtherClient, sendCommand, true);
                }
                //EVT指令测试
                if (MiddleIndex.Equals(session.MiddleIndex1))
                {
                    session.OtherClient = "android";
                    session.isSet = true;
                    sendCommand = sendCommand.Replace(MiddleIndex + " ", "");
                    WriteLog.WriteLogZLOther(session.OtherClient, sendCommand, true);
                }
            }
            else
            {
                session.isSet = false;
            }


            #region 转义还原
            //接受信息: 转义->验证码->解析
            //发送消息: 封装->计算校验->转义
            if (!session.isSet)
            {
                se = GetZy(se);
            }
            #endregion


            #region 判断格式完整【异或】
            //  string aaaa = se.Substring(0, se.Length- 3).Trim();
            string[] infos = se.Substring(0, se.Length - 3).Trim().Split(' ');
            string jym = Telnet_Session.GetJy(infos);
            string sejym = se.Substring(se.Length - 2, 2);
            //if (!jym.ToUpper().Equals(sejym.ToUpper()))
            //{
            //    session.Send("yh error");
            //    return;
            //}

            #endregion




            #region //BMK 消息格式数据
            session.Ft = session.getClsFt();
            session.Ft.Xxid = strindex;
            session.Ft.Xxtcd = se.Substring(6, 5);
            session.Ft.Simno_y = se.Substring(12, 17);
            string iii = GetZeroSuppression(se.Substring(12, 17));
            session.Ft.Simno = iii.Substring(1, iii.Length - 1);

            //系统识别码
            session.siCode = session.Ft.Simno;

            session.Ft.Xxlsh = se.Substring(30, 5);

            string desc = string.Empty;

            if (!session.isSet)
            {
                ////BMK OtherClient不计入在线
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
                if (se.Length > 39)
                {
                    desc = se.Substring(36, se.Length - 36 - 3);
                }
            }
            //string jy=
            #endregion



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


            //web端上发数据
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
                        if (strindex.Equals("8F 00"))
                        {
                            string ydlsh = se.Substring(36, 2);
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
                            // 开关门 车辆不在线 回复
                            if (session.OtherClient.Equals("center"))
                            {
                                session.SendAsToServers(strinfo);
                            }
                            else
                            {
                                //BMK添加其他客户端 2035-10-08
                                foreach (var s in session.AppServer.GetAllSessions())
                                {
                                    if (!string.IsNullOrEmpty(s.OtherClient))
                                    {
                                        //发送给 以socket连接服务器的  客户端(非设备)
                                        if (s.OtherClient.Equals("android"))
                                        {
                                            s.Send(strinfo);
                                            WriteLog.WriteLogZLOther(s.OtherClient, strinfo, true);
                                        }
                                    }
                                }
                            }

                        }
                    }
                    //为下发指令后, 处理指令后,避免 后面 在线之类的干扰
                    return;


                }
            }
            catch (Exception ex) { }




            //车载设备上发数据
            if (strindex.Equals("01 00"))
            {

                //注册

                string info = string.Format("81 00 00 0b {0} 00 01 {1} 00 31 32 33 34 35 36 37 38", Telnet_Session.GetFomartSysno(session.siCode), session.Ft.Xxlsh);
                info = "7E " + info + " " + Telnet_Session.GetJy(info.Split(' ')) + " 7E";
                byte[] btemp = strToToHexByte(info);
                session.Send(btemp, 0, btemp.Length);

                session.Is088 = true;

            }
            else if (strindex.Equals("01 02") || strindex.Equals("00 02"))
            {
                //鉴权 || 终端心跳
                byte[] btemp = SendPttyyd(session.Ft);
                session.Send(btemp, 0, btemp.Length);

                //上线  2035/01/06 整理新逻辑  发送到中心服务器  g02  代表上线 1, 下线不变 0
                //BMK 2035-10-10 修改为 通道打开即上线
                //BMK 2016-01-04  修改为 注册为上线,避免车多造成数据拥挤
                if (!session.isSet && session.Ft != null && string.IsNullOrEmpty(session.OtherClient))
                {
                    //&& !Telnet_Session.isonline 不作处理, 因为下线 是正常下线才能显示正确, 如果 服务器关闭 那么车辆就下不了线
                    //上线 一直上传数据库临时表(可清理),触发器到一车一数据  上线跟当前时间对比
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
                        session.SendAsToServersB(strinfo);
                        Telnet_Session.isonline = true;
                        WriteLog.WriteTestLog("Online", string.Format("online-{0}-{1:yyyy-MM-dd HH:mm:ss}", session.siCode, DateTime.Now), true);

                    }
                }
            }
            #region 终端通用应答[平台发送指令的回执 ] -参数设置回馈(缺省时间汇报间隔),开关车门的回复
            else if (strindex.Equals("00 01"))
            {
                string ydlsh = desc.Substring(0, 5);//对应的平台消息的流水号
                string ydid = desc.Substring(6, 5);//对应的平台消息的id
                string result = desc.Substring(12, 2);//结果 0:成功/确认 1:失败 2:消息有误 3:不支持


                string fg = string.Format("{0} {1}", ydid, result);
                strinfo = "";
                if (ydid.Equals("81 03"))//参数设置回馈
                {
                    //todo 2
                    temp = "14|" + session.siCode + "," + int.Parse(result);
                    // ((Telnet_Server)session.AppServer).DespatchMessage(session.siCode + "|" + ydid, temp);


                    if (ydlsh.Equals("00 29"))//缺省时间汇报间隔
                    {
                        buffer.Clear();
                        buffer.Append("\"signalName\":\"UploadIntervalReply\",");
                        buffer.Append("\"time\":\"" + session.Pt.P_time + "\",");
                        buffer.Append("\"result\":\"" + result + "\"");
                        strinfo = Telnet_Session.FomatToJosn("client_message", "2", session.siCode, "G19", buffer.ToString());
                    }
                    if (ydlsh.Equals("00 80"))//里程重设回复
                    {
                        buffer.Clear();
                        buffer.Append("\"signalName\":\"ResetMileageReply\",");
                        buffer.Append("\"time\":\"" + session.Pt.P_time + "\",");
                        buffer.Append("\"result\":\"" + result + "\"");
                        strinfo = Telnet_Session.FomatToJosn("client_message", "2", session.siCode, "G10", buffer.ToString());
                    }
                    if (ydlsh.Equals("00 5A"))//设置停车报警
                    {
                        buffer.Clear();
                        buffer.Append("\"signalName\":\"ParkingAlarmReply\",");
                        buffer.Append("\"time\":\"" + session.Pt.P_time + "\",");
                        buffer.Append("\"result\":\"" + result + "\"");
                        strinfo = Telnet_Session.FomatToJosn("client_message", "2", session.siCode, "G12", buffer.ToString());
                    }
                    if (ydlsh.Equals("00 55"))//设置超速报警
                    {
                        buffer.Clear();
                        buffer.Append("\"signalName\":\"OverspeedAlarmReply\",");
                        buffer.Append("\"time\":\"" + session.Pt.P_time + "\",");
                        buffer.Append("\"result\":\"" + result + "\"");
                        strinfo = Telnet_Session.FomatToJosn("client_message", "2", session.siCode, "G13", buffer.ToString());
                    }
                    if (ydlsh.Equals("00 57"))//疲劳驾驶
                    {
                        buffer.Clear();
                        buffer.Append("\"signalName\":\"FatigueDrivingReply\",");
                        buffer.Append("\"time\":\"" + session.Pt.P_time + "\",");
                        buffer.Append("\"result\":\"" + result + "\"");
                        strinfo = Telnet_Session.FomatToJosn("client_message", "2", session.siCode, "G11", buffer.ToString());
                    }

                    //指令 设置回执
                    session.SendAsToServers(strinfo);


                }
                if (ydid.Equals("8F 00")) //开关车门的回复
                {
                    buffer.Clear();
                    buffer.Append("\"systemNo\":\"" + session.siCode + "\",");
                    if (ydlsh.Equals("00 01") || ydlsh.Equals("01 01")) //开门      //01 01   01开头,是Other客户端发送标识
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
                        //BMK添加其他客户端 2035-10-08
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
                        session.SendAsToServers(strinfo);

                        //记录指令回复
                        WriteLog.WriteLogZL("retrunRecv:" + strinfo);
                    }


                }


                //string path = System.AppDomain.CurrentDomain.BaseDirectory + "zl.txt";
                //writer = File.AppendText(path);
                //writer.WriteLine(DateTime.Now.ToString() + " SendStr：");
                //writer.WriteLine("liushuihao：--" + ydlsh + "--");
                //writer.WriteLine("fasong：--" + fg + "--");
                //writer.WriteLine("------------------------------------------------");
                //writer.Close();
            }
            #endregion
            #region 摄像头立即拍摄命令应答
            else if (strindex.Equals("08 05"))
            {
                string ydlsh = desc.Substring(0, 5);//应答流水号
                string result = desc.Substring(6, 2);//结果 0:成功 1:失败 2:通道不支持
                //以下字段 ,结果为0才有 
                if (result.Equals("0"))
                {
                    string geshu = desc.Substring(9, 2);//拍摄成功的多媒体个数
                    int num = int.Parse(geshu);
                    for (int i = 0; i < num; i++)
                    {
                        //4个字节 多媒体id列表

                    }

                }

            }
            #endregion
            #region 查询终端参数应答[暂时无车机有此功能,/查看状态]
            else if (strindex.Equals("01 04"))
            {


            }
            #endregion
            #region //BMK数据上行透传
            else if (strindex.Equals("09 00"))
            {
                //desc  01 ED 91 08 01...ED 91 08 01...ED 91 08 01...
                string canstyle = desc.Substring(3, 5);
                string bs = "";
                int index = -1;

                byte[] btemp = SendPttyyd(session.Ft);
                session.Send(btemp, 0, btemp.Length);

                Candata cd = session.candate;
                if (canstyle.Equals("ED 93"))//东风S30
                {
                    bs = " ED 93 08 ";
                    index = 0;
                    // session.cd_s30 = new Candata_s30();
                    //cd = session.cd_s30;                           
                }
                else if (canstyle.Equals("ED 91"))//聆风
                {
                    bs = " ED 91 ";
                    index = 1;
                    //session.cd_lf = new Candata_Ev_lf();
                    //cd = session.cd_lf;
                }
                else if (canstyle.Equals("ED 98"))//EJ02EMT型
                {
                    bs = " ED 98 08 ";
                    index = 2;
                    //session.cd_ej02 = new Candata_EJ02();
                    //cd = session.cd_ej02;
                }
                else if (canstyle.Equals("ED 99"))//EJ04型      //转向地标 
                {
                    bs = " ED 99 08 ";
                    index = 3;
                    //session.cd_ej04 = new Candata_EJ04();
                    //cd = session.cd_ej04;
                    session.vlsType = 1000;
                }
                else if (canstyle.Equals("ED 90"))//A60型      //转向地标
                {
                    bs = " ED 90 08 ";
                    index = 4;
                    session.vlsType = 1003;
                }
                else if (canstyle.Equals("ED B0"))//郑州日产_锐骐型
                {
                    bs = " ED B0 08 ";
                    index = 5;
                    session.vlsType = 1001;
                }
                else if (canstyle.Equals("ED B1"))//郑州日产_帅客型
                {
                    bs = " ED B1 08 ";
                    index = 6;
                    session.vlsType = 1002;
                }
                if (index != -1 && session.Pt!=null) //当前没有有效的定位数据,丢弃当前工况数据
                {

                    //BMK 2035-10-12 canmodel修改为按需新建
                    cd = session.CreateInstance(index);

                    string yh = "", alarm = "";//油耗,报警
                    //can data
                    string data = "", sqlstr = "", cantime = "", canfault = "";//故障

                    string[] cans = GetFormatCanData(desc, bs, index);

                    // BMK 入库can
                    List<string> liscan = GetCanInfos(cans, cd, index, session.Lasttime, session.siCode, out yh, out alarm, out data, out sqlstr, out cantime, out canfault);
                    string tempstr = "", infoa = "", infob = "", oil = "", direction = "", speed = "0", mileage = "0", Elevation="0";
                    oil = string.IsNullOrEmpty(session.Pt.P_oil) == true ? "0" : session.Pt.P_oil;
                    direction = string.IsNullOrEmpty(session.Pt.P_direction) == true ? "0" : session.Pt.P_direction;
                    speed = string.IsNullOrEmpty(session.Pt.P_speed) == true ? "0" : session.Pt.P_speed;
                    mileage = string.IsNullOrEmpty(session.Pt.P_mileage) == true ? "0" : session.Pt.P_mileage;
                    Elevation = string.IsNullOrEmpty(session.Pt.P_elevation) == true ? "0" : session.Pt.P_elevation;


                    WriteLog.WriteOrdersLog("liscan:" + liscan.Count + "_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //InsFuelConsumed_123,soc_123
                    //BMK 2035-10-26 添加 cantime对比，记录上一个cantime，过滤同样的can项

                    string datetimenow = dtimeput.ToString("yyyy-MM-dd HH:mm:ss");
                    string infovalue = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}'", session.siCode, session.EffectiveLong, session.EffectiveLat, speed, direction, Elevation, session.isacc, session.Ss.Cs_islocation, mileage, oil, datetimenow);

                    //tempstr = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}'", session.siCode, session.EffectiveLong, session.EffectiveLat, session.Pt.P_speed, direction, Elevation, session.isacc, session.Ss.Cs_islocation, session.Pt.P_mileage, oil, datetimenow);
                    session.InsertTable(session, infovalue, datetimenow, liscan, null, canfault);
                }
            }
            #endregion
            #region //BMK 位置数据
            else if (strindex.Equals("02 00") || strindex.Equals("02 01"))
            {

                //位置信息||位置信息查询应答
                bool wzyd = false;
                if (strindex.Equals("02 01"))
                {
                    wzyd = true;
                    //位置信息查询应答 比汇报多个答流水号
                    desc = desc.Substring(6, desc.Length - 6);
                }

                if (!wzyd)
                {

                    //记录 从设备中发来的gps定位数据
                    // WriteLog.WritRerecvStrFromClientLog(canstr.ToString());


                }

                session.Pt = session.getClsPt();
                session.Pt.P_mark = desc.Substring(0, 11);//报警标志 4个字节
                string bjbz = GetTwo(desc.Substring(0, 11));


                //报警入库字段集
                string signalstrs = "";
                //temp float
                float aa = 0;

                #region 报警标志
                //00000000000011000000000000000011
                session.Wi = session.getClsWi();
                session.Wi.Cw_ffkm = bjbz.Substring(0, 1);
                signalstrs += session.Wi.Cw_ffkm.Equals("1") ? "GPS_IllegalOpenDoor," : "";

                session.Wi.Cw_cfyj = bjbz.Substring(1, 1);
                signalstrs += session.Wi.Cw_cfyj.Equals("1") ? "GPS_RolloverWarning," : "";

                session.Wi.Cw_pzyj = bjbz.Substring(2, 1);
                signalstrs += session.Wi.Cw_pzyj.Equals("1") ? "GPS_CollisionWarning," : "";

                session.Wi.Cw_clffwy = bjbz.Substring(3, 1);
                signalstrs += session.Wi.Cw_clffwy.Equals("1") ? "GPS_IllegalDisplacement," : "";

                session.Wi.Cw_clffdh = bjbz.Substring(4, 1);
                signalstrs += session.Wi.Cw_clffdh.Equals("1") ? "GPS_IllegalIgnition," : "";

                session.Wi.Cw_clbd = bjbz.Substring(5, 1);
                signalstrs += session.Wi.Cw_clbd.Equals("1") ? "GPS_VehicleStolen," : "";

                session.Wi.Cw_clyl = bjbz.Substring(6, 1);
                signalstrs += session.Wi.Cw_clyl.Equals("1") ? "GPS_AbnormalOil," : "";

                session.Wi.Cw_vss = bjbz.Substring(7, 1);
                signalstrs += session.Wi.Cw_vss.Equals("1") ? "GPS_Vss_Fault," : "";

                session.Wi.Cw_lxpl = bjbz.Substring(8, 1);
                signalstrs += session.Wi.Cw_lxpl.Equals("1") ? "GPS_LaneDepartureWarning," : "";

                session.Wi.Cw_ldxs = bjbz.Substring(9, 1);
                signalstrs += session.Wi.Cw_ldxs.Equals("1") ? "GPS_InsufficientTimeTooLong," : "";

                session.Wi.Cw_jclx = bjbz.Substring(10, 1);
                signalstrs += session.Wi.Cw_jclx.Equals("1") ? "GPS_OutLine," : "";

                session.Wi.Cw_jcqy = bjbz.Substring(11, 1);
                signalstrs += session.Wi.Cw_jcqy.Equals("1") ? "GPS_OutArea," : "";

                session.Wi.Cw_cstc = bjbz.Substring(12, 1);
                signalstrs += session.Wi.Cw_cstc.Equals("1") ? "GPS_OvertimeParking," : "";


                session.Wi.Cw_cslj = bjbz.Substring(13, 1);
                signalstrs += session.Wi.Cw_cslj.Equals("1") ? "GPS_DrivingTimeout," : "";

                session.Wi.Cw_pljsyj = bjbz.Substring(14, 1);
                signalstrs += session.Wi.Cw_pljsyj.Equals("1") ? "GPS_FatigueDriving_Warning," : "";

                session.Wi.Cw_csyj = bjbz.Substring(18, 1);
                signalstrs += session.Wi.Cw_csyj.Equals("1") ? "GPS_OverspeedWarning," : "";

                session.Wi.Cw_icmk = bjbz.Substring(19, 1);
                signalstrs += session.Wi.Cw_icmk.Equals("1") ? "GPS_IC_Fault," : "";

                session.Wi.Cw_sxdgz = bjbz.Substring(20, 1);
                signalstrs += session.Wi.Cw_sxdgz.Equals("1") ? "GPS_CameraFault," : "";

                session.Wi.Cw_tts = bjbz.Substring(21, 1);
                signalstrs += session.Wi.Cw_tts.Equals("1") ? "GPS_TTS_Fault," : "";

                session.Wi.Cw_zdxs = bjbz.Substring(22, 1);
                signalstrs += session.Wi.Cw_zdxs.Equals("1") ? "GPS_DisplayFailure," : "";

                session.Wi.Cw_zdzdy_dd = bjbz.Substring(23, 1);
                signalstrs += session.Wi.Cw_zdzdy_dd.Equals("1") ? "GPS_TerminalMainsPower," : "";

                session.Wi.Cw_zdzdy_qy = bjbz.Substring(24, 1);
                signalstrs += session.Wi.Cw_zdzdy_qy.Equals("1") ? "GPS_TerminalSupplyOltage," : "";

                session.Wi.Cw_gnns_tx_dl = bjbz.Substring(25, 1);
                signalstrs += session.Wi.Cw_gnns_tx_dl.Equals("1") ? "GPS_GNNS_Antenna_ShortCircuit," : "";

                session.Wi.Cw_gnns_tx_wjjd = bjbz.Substring(26, 1);
                signalstrs += session.Wi.Cw_gnns_tx_wjjd.Equals("1") ? "GPS_GNNS_AntennaCut," : "";

                session.Wi.Cw_gnns_gz = bjbz.Substring(27, 1);
                signalstrs += session.Wi.Cw_gnns_gz.Equals("1") ? "GPS_GNNS_ModuleFailure," : "";

                session.Wi.Cw_wxyj = bjbz.Substring(28, 1);
                signalstrs += session.Wi.Cw_wxyj.Equals("1") ? "GPS_DangerAlarm," : "";

                session.Wi.Cw_pljs = bjbz.Substring(29, 1);
                signalstrs += session.Wi.Cw_pljs.Equals("1") ? "GPS_FatigueDriving," : "";

                session.Wi.Cw_csbj = bjbz.Substring(30, 1);
                signalstrs += session.Wi.Cw_csbj.Equals("1") ? "GPS_OverspeedAlarm," : "";

                session.Wi.Cw_jjbj = bjbz.Substring(31, 1);
                signalstrs += session.Wi.Cw_jjbj.Equals("1") ? "GPS_EmergencyAlarm" : "";


                #endregion




                session.Pt.P_state = desc.Substring(12, 11);//状态   4
                string status = GetTwo(desc.Substring(12, 11));
                // 00000000000011000000000000000001

                //状态入库字段集
                string signalstatu = "";
                //状态入库字段值
                string signalstatuvalue = "";

                #region 状态
                session.Ss=session.getClsSs();
                session.Ss.Cs_isgaliloca = status.Substring(10, 1);
                session.Ss.Cs_isnassloca = status.Substring(11, 1);
                session.Ss.Cs_isbdwxloca = status.Substring(12, 1);
                session.Ss.Cs_isgpsloca = status.Substring(13, 1);
                session.Ss.Cs_dool5 = status.Substring(14, 1);
                session.Ss.Cs_dool4 = status.Substring(15, 1);
                session.Ss.Cs_dool3 = status.Substring(18, 2);
                session.Ss.Cs_dool2 = status.Substring(20, 1);
                session.Ss.Cs_dool1 = status.Substring(21, 1);

                //车门状态 用以 分时租赁中 取还车 开门关门指令 验证
                //signalstatu += "GPS_DoorStatus,";
                //signalstatuvalue += session.Ss.Cs_dool1 + ",";

                session.Ss.Cs_dool = status.Substring(22, 1);
                signalstatu += "GPS_OprationLock,";
                signalstatuvalue += session.Ss.Cs_dool + ",";

                session.Ss.Cs_circuitry = status.Substring(23, 1);
                session.Ss.Cs_oil = status.Substring(24, 1);
                session.Ss.Cs_loading = status.Substring(25, 1);
                session.Ss.Cs_Latilongencry = status.Substring(26, 1);
                session.Ss.Cs_operate = status.Substring(27, 1);
                session.Ss.Cs_eastorwest = status.Substring(28, 1);
                session.Ss.Cs_northorsouth = status.Substring(29, 1);
                session.Ss.Cs_islocation = status.Substring(30, 1);


                session.Ss.Cs_acc = status.Substring(31, 1);
                //0 关,1 开
                //session.Ss.Cs_acc = session.isacc;
                session.isacc = session.Ss.Cs_acc;

                #endregion


                //纬度   4
                int tennum = Getdecimal(GetZeroSuppression(desc.Substring(24, 11)));
                session.Pt.P_latitude = Math.Round(tennum / 1000000d, 6);



                //经度   4
                tennum = Getdecimal(GetZeroSuppression(desc.Substring(36, 11)));
                session.Pt.P_longitude = Math.Round(tennum / 1000000d, 6);

                //BMK经纬度处理
                if (session.Pt.P_latitude > 0)
                {
                    session.EffectiveLong = session.Pt.P_longitude;
                    session.EffectiveLat = session.Pt.P_latitude;
                }

                session.Pt.P_elevation = desc.Substring(48, 5);//高程   2
                tennum = Getdecimal(GetZeroSuppression(desc.Substring(48, 5)));
                session.Pt.P_elevation = tennum.ToString();


                session.Pt.P_speed = desc.Substring(54, 5);//速度   2
                tennum = Getdecimal(GetZeroSuppression(desc.Substring(54, 5)));
                session.Pt.P_speed = (tennum / 10).ToString();


                session.Pt.P_direction = desc.Substring(60, 5);//方向   2
                tennum = Getdecimal(GetZeroSuppression(desc.Substring(60, 5)));
                session.Pt.P_direction = tennum.ToString();

                session.Pt.P_time = desc.Substring(66, 17);//时间   6
                string[] sis = desc.Substring(66, 17).Split(' ');
                session.Pt.P_time = string.Format("20{0}-{1}-{2} {3}:{4}:{5}", sis[0], sis[1], sis[2], sis[3], sis[4], sis[5]);
                session.Lasttime = session.Pt.P_time;

                //存储列名
                string aaa = "";
                //存储列值
                string bbb = "";
                if (desc.Length > 84)//是否包含位置附加信息
                {
                    try
                    {
                        #region 位置附加信息
                        string canstrfj = desc.Substring(84, desc.Length - 84);
                        desc = canstrfj;
                        //标识位
                        string bsw, info;
                        info = canstrfj;

                        session.Sle = session.getClsSle();
                        int num, nu;
                        while (info.Length != 0)
                        {
                            bsw = info.Substring(0, 2);
                            nu = int.Parse(info.Substring(3, 2));
                            num = nu * 3 - 1;
                            switch (bsw)
                            {

                                case "01": //里程
                                    {
                                        // num = 11;
                                        tennum = Getdecimal(GetZeroSuppression(GetZd(num, info, out info)));
                                        aa = tennum / 10f;
                                        session.Pt.P_mileage = aa.ToString();
                                        break;
                                    }
                                case "02": //油量
                                    {
                                        //num = 5;
                                        tennum = Getdecimal(GetZeroSuppression(GetZd(num, info, out info)));
                                        // aa = tennum / 10f;
                                        session.Pt.P_oil = tennum.ToString();
                                        break;
                                    }
                                case "03": //行驶记录功能获取的速度
                                    {
                                        //num = 5;
                                        tennum = Getdecimal(GetZeroSuppression(GetZd(num, info, out info)));
                                        aa = tennum / 10f;
                                        session.Pt.P_speedx = aa.ToString();
                                        break;
                                    }
                                case "04": // 需要人工确认报警事件的id
                                    {
                                        //num = 5;
                                        // 龙安-张工:088(日产)的位置附加信息id位 04的,与固标不同,自定义了一些参数,088协议上没有解说.具体查看文档 <<088车机>>

                                        if (num == 5)
                                        {
                                            tennum = Getdecimal(GetZeroSuppression(GetZd(num, info, out info)));
                                            session.Pt.P_rid = tennum.ToString();
                                        }
                                        else if (num == 23) //日产
                                        {
                                           // info = info.Substring(29, info.Length - 29);
                                            info = "";
                                        }
                                        break;
                                    }
                                case "11": //超速报警附加信息  1或者5
                                    {
                                        string sjlx = info.Substring(3, 2);

                                        if (sjlx.Equals("01"))
                                        {
                                            //一个字节
                                            //num = 2;
                                            //info = info.Substring(3, info.Length - 3);
                                            session.Pt.P_csspeend = "_" + "0";//无特定位置
                                        }
                                        else if (sjlx.Equals("05"))
                                        {
                                            //五个字节
                                            //num = 11;
                                            info = info.Substring(3, info.Length - 3);
                                            string Csbjfj = GetZd(num, info, out info);
                                            int wzlx = int.Parse(Csbjfj.Substring(0, 2));//位置类型
                                            string qyhlx = Csbjfj.Substring(3, Csbjfj.Length - 3);//区域或线路id   xx xx xx xx [4个字节]
                                            session.Pt.Csspeend_wzlx = wzlx.ToString();
                                            session.Pt.Csspeend_qyid = qyhlx;

                                            session.Pt.P_csspeend = "_" + Csbjfj;
                                        }

                                        break;
                                    }
                                case "12": //进出区域/路线报警 [6]
                                    {
                                        //num = 17;
                                        string jclx = GetZd(num, info, out info);
                                        int wzlx = int.Parse(jclx.Substring(0, 2)); //位置类型 [1个字节]
                                        string qyhlx = jclx.Substring(3, 11); //区域或线路id [4个字节]
                                        int fx = int.Parse(jclx.Substring(15, 2)); //方向 [1个字节]

                                        session.Pt.Jclx_wzlx = wzlx.ToString();
                                        session.Pt.Jclx_qyid = qyhlx;
                                        session.Pt.Jclx_Angle = fx.ToString();

                                        session.Pt.P_jclx = "_" + jclx;
                                        break;
                                    }
                                case "13": //路段行驶时间不足/过长报警 [7]
                                    {
                                        //num = 20;
                                        string ldgcbj = GetZd(num, info, out info);
                                        string ldid = ldgcbj.Substring(0, 11);//路段id   [4个字节]
                                        string dlxsjl = ldgcbj.Substring(12, 5);//路段行驶时间  [两个字节]
                                        string jg = ldgcbj.Substring(18, 2);//结果 [1个字节]

                                        session.Pt.Ldgc_ldid = ldid;
                                        session.Pt.Ldgc_time = dlxsjl;
                                        session.Pt.Ldgc_result = jg;

                                        session.Pt.P_ldgc = "_" + ldgcbj;
                                        break;
                                    }
                                case "25": //扩展车辆信号状态位 [4]
                                    {
                                        // num = 11;
                                        string kzclxh = GetZd(num, info, out info);
                                        session.Pt.P_kzcl = "_" + kzclxh;
                                        status = GetTwo(kzclxh);
                                        #region MyRegion
                                        
                                        session.Sle.Cs_stateoftheclutch = status.Substring(17, 1);
                                        session.Sle.Cs_heaterwork = status.Substring(18, 1);
                                        session.Sle.Cs_abswork = status.Substring(19, 1);
                                        session.Sle.Cs_slowmachinework = status.Substring(20, 1);
                                        session.Sle.Cs_neutralsignal = status.Substring(21, 1);
                                        session.Sle.Cs_airconditionstatus = status.Substring(22, 1);
                                        session.Sle.Cs_loudspeakersignal = status.Substring(23, 1);
                                        session.Sle.Cs_incorridorlamp = status.Substring(24, 1);
                                        session.Sle.Cs_foglightsignal = status.Substring(25, 1);
                                        session.Sle.Cs_reversesignals = status.Substring(26, 1);
                                        session.Sle.Cs_brakesignal = status.Substring(27, 1);
                                        session.Sle.Cs_leftturnsignal = status.Substring(28, 1);
                                        session.Sle.Cs_rightturnsignal = status.Substring(29, 1);
                                        session.Sle.Cs_highbeam = status.Substring(30, 1);
                                        session.Sle.Cs_dippedheadlight = status.Substring(31, 1);



                                        #endregion

                                        break;
                                    }
                                case "2a": //Io状态位 [2]
                                    {
                                        // num = 5;
                                        string io = GetZd(num, info, out info);
                                        status = GetTwo(io);
                                        string sdxmzt = status.Substring(30, 1);//深度休眠状态
                                        string xmzt = status.Substring(31, 1);//休眠状态

                                        session.Pt.Io_sdxm = sdxmzt;
                                        session.Pt.Io_xm = xmzt;

                                        session.Pt.P_ios = "_" + io;
                                        break;
                                    }
                                case "2b": //模拟量
                                    {
                                        //num = 5;
                                        string mnl = GetZd(num, info, out info);
                                        string Mol_ad0 = mnl.Substring(0, 5);
                                        string Mol_ad1 = mnl.Substring(6, 5);

                                        session.Pt.Mol_ad0 = Getdecimal(GetZeroSuppression(Mol_ad0)).ToString();
                                        session.Pt.Mol_ad1 = Getdecimal(GetZeroSuppression(Mol_ad1)).ToString();

                                        session.Pt.P_mnl = "_" + mnl;
                                        break;
                                    }
                                case "30": //无线通信网络信号强度
                                    {
                                        // num = 2;
                                        session.Pt.P_wxqd = Getdecimal(GetZeroSuppression(GetZd(num, info, out info))).ToString();

                                        break;
                                    }
                                case "31": // GNSS定位卫星数
                                    {
                                        //  num = 2;
                                        session.Pt.P_gnss = Getdecimal(GetZeroSuppression(GetZd(num, info, out info))).ToString();
                                        break;
                                    }

                                default:
                                    {

                                        info = "canstrfj" + canstrfj + ";bsw" + bsw;
                                        info = "";
                                        break;
                                    }
                            }
                        }

                        #endregion

                        foreach (System.Reflection.PropertyInfo p in session.Sle.GetType().GetProperties())
                        {
                            aaa += string.Format(",`{0}`", p.Name);
                            bbb += string.Format(",'{0}'", p.GetValue(session.Sle, null));
                        }
                    }
                    catch (Exception)
                    {

                    }
                }

                session.Pt.P_mileage = string.IsNullOrEmpty(session.Pt.P_mileage) ? "0" : session.Pt.P_mileage;

         

                foreach (System.Reflection.PropertyInfo p in session.Wi.GetType().GetProperties())
                {
                    aaa += string.Format(",`{0}`", p.Name);
                    bbb += string.Format(",'{0}'", p.GetValue(session.Wi, null));
                }
                foreach (System.Reflection.PropertyInfo p in session.Ss.GetType().GetProperties())
                {
                    aaa += string.Format(",`{0}`", p.Name);
                    bbb += string.Format(",'{0}'", p.GetValue(session.Ss, null));
                }


                //buffer.Clear();
                //buffer.Append("\"time\":\"" + session.Pt.P_time + "\",");
                //buffer.Append("\"latitude\":\"" + session.EffectiveLat + "\",");
                //buffer.Append("\"longitude\":\"" + session.EffectiveLong + "\",");
                //buffer.Append("\"speed\":\"" + session.Pt.P_speed + "\",");
                //buffer.Append("\"direction\":\"" + direction + "\",");
                //buffer.Append("\"acc\":\"" + session.isacc + "\",");
                //buffer.Append("\"islocation\":\"" + session.Ss.Cs_islocation + "\",");
                //buffer.Append("\"mileage\":\"" + session.Pt.P_mileage + "\"");

                string oil, direction, speed, mileage,Elevation;
                oil = string.IsNullOrEmpty(session.Pt.P_oil) == true ? "0" : session.Pt.P_oil;
                direction = string.IsNullOrEmpty(session.Pt.P_direction) == true ? "0" : session.Pt.P_direction;
                speed = string.IsNullOrEmpty(session.Pt.P_speed) == true ? "0" : session.Pt.P_speed;
                mileage = string.IsNullOrEmpty(session.Pt.P_mileage) == true ? "0" : session.Pt.P_mileage;
                 Elevation = string.IsNullOrEmpty(session.Pt.P_elevation) == true ? "0" : session.Pt.P_elevation;
                             
                if (wzyd)
                {
                    //位置信息查询应答 [反馈web]
                    //点名回执
                    //todo 2
                    strinfo = string.Format("04|{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}", session.siCode, session.Pt.P_time, session.EffectiveLat, session.EffectiveLong, speed, direction, session.isacc, session.Ss.Cs_islocation, mileage);
                    // ((Telnet_Server)session.AppServer).DespatchMessage(session.siCode + "|82 01", strinfo);

                    //  strinfo = Telnet_Session.FomatToJosn("client_message", "2", session.siCode, "G04", buffer.ToString());
                    SignalName = "Gps_Replay";
                    strnew = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'", session.siCode, session.EffectiveLong, session.EffectiveLat, speed, direction, Elevation, session.isacc, session.Ss.Cs_islocation, mileage, oil, session.Pt.P_time, SignalName, "");
                    strinfo = Telnet_Session.FomatToJosnByNew(0, "G04", strnew);
                    strinfo = strinfo.Replace("'", "");
                    //点名
                    session.SendAsToServers(strinfo);

                }
                else
                {
                    //bmk 位置数据_定时上传


                    string sqlstr = string.Format("insert into ev_information.ev_{0} (`Time`,P_Longitude,P_Latitude,P_direction,P_speed,P_mileage,P_Elevation,`P_Oil`,`P_Speedx`,`P_rid`,`P_wxqd`,`P_gnss`{1},`Csspeend_wzlx`,`Csspeend_qyid`,`Jclx_wzlx`,`Jclx_qyid`,`Jclx_Angle`,`Ldgc_ldid`,`Ldgc_time`,`Ldgc_result`,`Io_sdxm`,`Io_xm`,`Mol_ad0`,`Mol_ad1`) values ('{2}',{3},{4},{5},{6},{7},{8},'{9}','{10}','{11}','{12}','{13}'{14},'{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}')", session.siCode, aaa, session.Pt.P_time, session.EffectiveLong, session.EffectiveLat, direction, session.Pt.P_speed, session.Pt.P_mileage, session.Pt.P_elevation, session.Pt.P_oil, session.Pt.P_speedx, session.Pt.P_rid, session.Pt.P_wxqd, session.Pt.P_gnss, bbb, session.Pt.Csspeend_wzlx, session.Pt.Csspeend_qyid, session.Pt.Jclx_wzlx, session.Pt.Jclx_qyid, session.Pt.Jclx_Angle, session.Pt.Ldgc_ldid, session.Pt.Ldgc_time, session.Pt.Ldgc_result, session.Pt.Io_sdxm, session.Pt.Io_xm, session.Pt.Mol_ad0, session.Pt.Mol_ad1);

                    #region GPS位置入库
                    //老数据库结构入库
                    //session.mqs.SendMsg(sqlstr);

                    double P_longitude = session.EffectiveLong;
                    double P_Latitude = session.EffectiveLat;
                    if (P_longitude > 0 && P_Latitude > 0) //过滤经纬度为0的数据 或负
                    {
                        //TODO Filter  过滤 时间超过服务器时间的数据  
                        if (PublicMethods.TimeDetermine(session.siCode, session.Pt.P_time, out dtimeput))
                        {
                            //新结构
                            temp = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'", session.siCode, session.EffectiveLong, session.EffectiveLat, speed, direction, session.Pt.P_elevation, session.isacc, session.Ss.Cs_islocation, mileage, oil, dtimeput.ToString("yyyy-MM-dd HH:mm:ss"), "Gps_Location", "");
                            temp = string.Format("insert into vehiclerunninginfo.runningrecord (`SystemNo`,`Longitude`,`Latitude`,`Speed`,`Direction`,`Elevation`,`Acc`,`IsLocation`,`Mileage`,`Oil`,`CurrentTime`,`SignalName`,`CurrentValue`)  values({0});", temp);
                            //入库队列
                            if (!string.IsNullOrEmpty(temp))
                            {
                                session.mqs.SendMsg(temp);
                            }

                            string infokey = "`SystemNo`,`CurrentTime`,`Longitude`,`Latitude`,`Speed`,`Direction`,`Elevation`,`Acc`,`IsLocation`,`Mileage`,`Oil`,`GPS_OprationLock`";
                            string infovalue = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'", session.siCode, dtimeput.ToString("yyyy-MM-dd HH:mm:ss"), session.EffectiveLong, session.EffectiveLat, speed, direction, session.Pt.P_elevation, session.isacc, session.Ss.Cs_islocation, mileage, oil, session.Ss.Cs_dool);
                            StringBuilder sbstr = new StringBuilder();
                            sbstr.AppendFormat(" insert into vehiclerunninginfo.gpsrecord ( {0} ) values ( {1} );", infokey, infovalue);
                            if (!string.IsNullOrEmpty(sbstr.ToString()))
                            {
                                session.mqs.SendMsg(sbstr.ToString());
                            }

                        }

                        #region GPS位置 发送给中心服务器

                        //发送给中心服务器
                        //TODO Filter  过滤 时间超过服务器时间的数据  
                        PublicMethods.TimeDetermine(session.siCode, session.Pt.P_time, out dtimeput);

                        ////  strinfo = Telnet_Session.FomatToJosn("client_message", "2", session.siCode, "G02", buffer.ToString());
                        SignalName = "Gps_Location";
                        strnew = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'", session.siCode, session.EffectiveLong, session.EffectiveLat, speed, direction, session.Pt.P_elevation, session.isacc, session.Ss.Cs_islocation, mileage, oil, dtimeput.ToString("yyyy-MM-dd HH:mm:ss"), SignalName, "");
                        strinfo = Telnet_Session.FomatToJosnByNew(0, "G02", strnew);
                        strinfo = strinfo.Replace("'", "");
                        //GPS位置
                        //session.SendAsToServers( strinfo);
                        //2035-04-21 修改 将大量发送的数据 与 命令 分开


                        session.SendAsToServers(strinfo);


                        #endregion

                    }
                    else
                    {
                        WriteLog.WriteFilterLog(session.siCode + "不是正确的经纬度：" + session.EffectiveLong);
                    }

                    #endregion

                    //todo 2  发送到WebSocket
                    // strinfo = string.Format("02|{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}", session.siCode, session.Pt.P_time, session.EffectiveLat, session.EffectiveLong, session.Pt.P_speed, direction, session.isacc, session.Ss.Cs_islocation, session.Pt.P_mileage);
                    //((Telnet_Server)session.AppServer).DespatchMessage(strinfo);




                    //2014-11-07现转入到中心服务器处理
                    #region //BMK区域报警逻辑 （废弃）
                    //string localar = GetlocalAlarm(session.Pt.P_time, session.siCode, session.EffectiveLong, session.EffectiveLat);
                    //if (!string.IsNullOrEmpty(localar))
                    //{
                    //    //todo 2
                    //    strinfo = "07|" + session.siCode + "_" + localar;
                    //    ((Telnet_Server)session.AppServer).DespatchMessage(strinfo);

                    //    buffer.Clear();
                    //    buffer.Append("\"time\":\"" + session.Pt.P_time + "\",");
                    //    buffer.Append("\"localar\":\"" + localar + "\"");

                    //    strinfo = Telnet_Session.FomatToJosn("client_message", "2", session.siCode, "G07", buffer.ToString());
                    //    //区域报警
                    //    session.SendAsToServers( strinfo);
                    //}
                    #endregion

                    //超速报警逻辑[]
                    //string speedar = GetSpeedAlarm(session.siCode, session.Pt.P_time, session.Pt.P_speed);
                    //if (!string.IsNullOrEmpty(speedar))
                    //{
                    //    strinfo = "09|" + speedar;
                    //    ((Telnet_Server)session.AppServer).DespatchMessage(strinfo);
                    //}

                    #region //BMK GPS报警 状态


                    string errorstr, identifying = "G15", strtoc = "1"; // 1 有当前条数据 代表有报警

                    #region Old （废弃）
                    ////if (wi.Cw_pljs.Equals("1"))
                    ////{
                    ////    #region MyRegion
                    ////    //errorstr = "车辆处于疲劳驾驶状态";
                    ////    ////todo 2
                    ////    //strinfo = "11|" + session.siCode + "," + session.Pt.P_time + "," + errorstr;
                    ////    //((Telnet_Server)session.AppServer).DespatchMessage(strinfo);
                    ////    //SetAlarm("11", session.siCode, session.Pt.P_time, errorstr);

                    ////    //buffer.Clear();
                    ////    //buffer.Append("\"time\":\"" + session.Pt.P_time + "\"");
                    ////    //strinfo = Telnet_Session.FomatToJosn("client_message", "2", session.siCode, "G11", buffer.ToString());
                    ////    ////疲劳报警
                    ////    //session.SendAsToServers( strinfo);
                    ////    #endregion
                    ////    identifying = "G11";
                    ////    SignalName = "Fatigue_Driving";
                    ////}
                    ////if (wi.Cw_cstc.Equals("1"))
                    ////{
                    ////    #region MyRegion
                    ////    //errorstr = "车辆处于超时停车状态";
                    ////    ////todo 2
                    ////    //strinfo = "12|" + session.siCode + "," + session.Pt.P_time + "," + errorstr;
                    ////    //((Telnet_Server)session.AppServer).DespatchMessage(strinfo);
                    ////    //SetAlarm("12", session.siCode, session.Pt.P_time, errorstr);

                    ////    //buffer.Clear();
                    ////    //buffer.Append("\"time\":\"" + session.Pt.P_time + "\"");
                    ////    //strinfo = Telnet_Session.FomatToJosn("client_message", "2", session.siCode, "G12", buffer.ToString());
                    ////    ////超时报警
                    ////    //session.SendAsToServers( strinfo);
                    ////    #endregion
                    ////    identifying = "G12";
                    ////    SignalName = "Fatigue_Driving";
                    ////}
                    ////if (wi.Cw_csbj.Equals("1"))
                    ////{
                    ////    #region MyRegion
                    ////    //errorstr = "车辆处于超速报警状态";
                    ////    ////todo 2
                    ////    //strinfo = "13|" + session.siCode + "," + session.Pt.P_time + "," + errorstr;
                    ////    //((Telnet_Server)session.AppServer).DespatchMessage(strinfo);
                    ////    //SetAlarm("13", session.siCode, session.Pt.P_time, errorstr);

                    ////    //buffer.Clear();
                    ////    //buffer.Append("\"time\":\"" + session.Pt.P_time + "\"");
                    ////    //strinfo = Telnet_Session.FomatToJosn("client_message", "2", session.siCode, "G13", buffer.ToString());
                    ////    ////超速报警
                    ////    //session.SendAsToServers( strinfo);
                    ////    #endregion
                    ////    identifying = "G13";
                    ////    SignalName = "Overspeed_Alarm";
                    ////}
                    #endregion

                    #region  GPS报警 入库+推送
                    for (int i = 0; i < signalstrs.Split(',').Count(); i++)
                    {
                        SignalName = signalstrs.Split(',')[i];
                        //报警入库+推送
                        if (!string.IsNullOrEmpty(SignalName))
                        {
                            //TODO Filter  过滤 时间超过服务器时间的数据  
                            if (PublicMethods.TimeDetermine(session.siCode, session.Pt.P_time, out dtimeput))
                            {
                                strtoc = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'", session.siCode, session.EffectiveLong, session.EffectiveLat, speed, direction, session.Pt.P_elevation, session.isacc, session.Ss.Cs_islocation, mileage, oil, dtimeput.ToString("yyyy-MM-dd HH:mm:ss"), SignalName, "");

                                temp = "insert into vehiclerunninginfo.runningrecord(`SystemNo`,`Longitude`,`Latitude`,`Speed`,`Direction`,`Elevation`,`Acc`,`IsLocation`,`Mileage`,`Oil`,`CurrentTime`,`SignalName`,`CurrentValue`)  values(" + strtoc + ");";
                                //入库队列
                                if (!string.IsNullOrEmpty(temp))
                                {

                                    session.mqs.SendMsg(temp);




                                }

                                //推送给中心服务器
                                if (!string.IsNullOrEmpty(strtoc))
                                {
                                    strtoc = Telnet_Session.FomatToJosnByNew(1, identifying, strtoc);
                                    strtoc = strtoc.Replace("'", "");
                                    //GPS报警数据
                                    //session.SendAsToServers( strtoc); //指令队列
                                    session.SendAsToServers(strtoc); //大量数据（can，gps报警）
                                }
                            }
                        }


                    }
                    #endregion
                    #region  状态 入库
                    string statusvalue = "";
                    for (int i = 0; i < signalstatu.Split(',').Count(); i++)
                    {
                        SignalName = signalstatu.Split(',')[i];
                        statusvalue = signalstatuvalue.Split(',')[i];
                        //报警入库+推送
                        if (!string.IsNullOrEmpty(SignalName))
                        {
                            //TODO Filter  过滤 时间超过服务器时间的数据  
                            if (PublicMethods.TimeDetermine(session.siCode, session.Pt.P_time, out dtimeput))
                            {
                                strtoc = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'", session.siCode, session.EffectiveLong, session.EffectiveLat, speed, direction, session.Pt.P_elevation, session.isacc, session.Ss.Cs_islocation, mileage, oil, dtimeput.ToString("yyyy-MM-dd HH:mm:ss"), SignalName, statusvalue);

                                temp = "insert into vehiclerunninginfo.runningrecord(`SystemNo`,`Longitude`,`Latitude`,`Speed`,`Direction`,`Elevation`,`Acc`,`IsLocation`,`Mileage`,`Oil`,`CurrentTime`,`SignalName`,`CurrentValue`)  values(" + strtoc + ");";
                                //入库队列
                                if (!string.IsNullOrEmpty(temp))
                                {

                                    session.mqs.SendMsg(temp);
                                }
                            }
                        }
                    }
                    #endregion

                    #endregion
                    //writer = File.AppendText(System.AppDomain.CurrentDomain.BaseDirectory + "SendStr_localar.txt");
                    //writer.WriteLine(DateTime.Now.ToString() + " SendStr1：" + localar);
                    //writer.WriteLine(DateTime.Now.ToString() + " SendStr2：" + speedar);
                    //writer.Close();


                    byte[] btemp = SendPttyyd(session.Ft);
                    session.Send(btemp, 0, btemp.Length);
                }



            }

            #endregion

            //writer = File.AppendText(System.AppDomain.CurrentDomain.BaseDirectory + "SendStr.txt");
            //writer.WriteLine(DateTime.Now.ToString() + " SendStr2：" + temp);
            // writer.Close();
            // writer = File.AppendText(System.AppDomain.CurrentDomain.BaseDirectory + "SendStr.txt");
            //  writer.WriteLine(DateTime.Now.ToString() + " SendStr3：" + desc);
            // writer.Close();





        }



        /// <summary>
        /// 获取附加位置信息的子项
        /// </summary>
        /// <param name="num"></param>
        /// <param name="info"></param>
        /// <param name="infos"></param>
        /// <returns></returns>
        private static string GetZd(int num, string info, out string infos)
        {
            //01 04 00 00 00 00 xx 
            string aaa = info.Substring(6, num);
            if (num + 6 < info.Length)
            {
                infos = info.Substring(num + 1 + 3 + 3, info.Length - num - 1 - 3 - 3);
            }
            else
            {
                infos = "";
            }
            return aaa;
        }

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
        /// 得到二进制数据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetTwo(string infos)
        {
            string temp = GetZeroSuppression(infos);
            int len = temp.Length / 2;
            int num = Getdecimal(temp);
            string info = Convert.ToString(num, 2);
            string aa = "";
            for (int i = 0; i < len * 8 - info.Length; i++)
            {
                aa += "0";
            }
            return aa + info;
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
        /// 去空格
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetZeroSuppression(string info)
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
        /// <param name="session.Ft">格式类</param>
        /// <returns></returns>
        public byte[] SendPttyyd(Cls_Format ft)
        {
            string info = string.Format("80 01 00 05 {0} 00 01 {1} {2} 00", ft.Simno_y, ft.Xxlsh, ft.Xxid);

            string jy = Telnet_Session.GetJy(info.Substring(0, info.Length).Split(' '));
            info = string.Format("{0} {1}", info, jy).ToUpper();


            //消息封装
            info = GetXxfz(info);

            //加上标示位
            info = string.Format("{0} {1} {2}", "7E", info, "7E").ToUpper();

            byte[] btemp = strToToHexByte(info);

            return btemp;
        }



        /// <summary>
        ///  获取can的数据字节数组
        ///  str[0]CAN数据透传子信令标志
        ///  str[1]~str[?] can数据
        ///  ED 91 08 05 5B DB C0 55 00 E7 40 10 2E 00 00 0E 06 10 10 00 2E
        ///  str[1]:05 5B DB C0 55 00 E7 40 10 2E 00 00 0E 06 10 10 00 2E
        /// </summary>
        /// <param name="canstr"></param>
        /// <param name="reg"></param>
        /// <param name="index">can部分发送方式不一样,郑州日产 因内存不够 将每个can带的车型去掉了</param>
        /// <returns></returns>
        public string[] GetFormatCanData(string canstr, string reg, int index)
        {

            if (index == 5)
            {
                return GetFormatCanData(canstr);
            }
            else
            {
                return Regex.Split(canstr, reg, RegexOptions.IgnoreCase);
            }

        }
        public string[] GetFormatCanData(string info)
        {
            string infob = info.Substring(12, info.Length - 20);
            List<string> aaa = PublicMethods.GetContentByReg(infob, @"(\w{2}\s){12}");
            aaa.Add(info.Substring(info.Length - 20, 17));
            return aaa.ToArray();
        }
        //string[] oncfault = new string[12] { "通讯超时", "输出过压", "输出欠流", "内部中间电压欠压＜10V", "输出短路/欠压", "输出过流", "内部PFC欠压", "内部PFC过压", "输入过压", "输入欠压", "过温输出关闭", "过温输出功率减半" };//故障码
        string[] oncfault = new string[12] { "过温输出功率减半", "过温输出关闭", "输入欠压", "输入过压", "内部PFC过压", "内部PFC欠压", "输出过流", "输出短路/欠压", "内部中间电压欠压＜10V", "输出欠流", "输出过压", "通讯超时" };//故障码
        // string[] dcdcfault = new string[8] { "通讯故障", "高温报警", "低温报警", "输出过流", "输出欠压", "输出过压", "输入欠压", "输入过压" };
        //BMK 2035-10-28 用数字标识
        string[] dcdcfault = new string[8] { "输入过压", "输入欠压", "输出过压", "输出欠压", "输出过流", "低温报警", "高温报警", "通讯故障" };

        /// 解析can
        /// </summary>
        /// <param name="infos">数据数组</param>
        /// <param name="cd"></param>
        /// <param name="index">车辆型号标示</param>
        /// <param name="ltime">上一个位置数据时间, 现改为can入库存储can时间</param>
        /// <param name="sysno">系统编号</param>
        /// <param name="yh"></param>
        /// <param name="alarm"></param>
        /// <param name="data"></param>
        /// <param name="sqlstr"></param>
        /// <param name="cantime">cantime</param>
        /// <param name="canfault">canfault</param>
        /// <returns></returns>
        public List<string> GetCanInfos(string[] infos, Candata cd, int index, string ltime, string sysno, out string yh, out string alarm, out string data, out string sqlstr, out string cantime, out string canfault)
        {
            ///"InsFuelConsumed_" + cd30.InsFuelConsumed
            List<string> lis = new List<string>();
            sqlstr = "";
            yh = ""; alarm = "";
            data = "";
            //bmk 2035-09-14 修改 入库存储 can的时间
            cantime = "";
            string str_time;
            //bmk 2035-10-22 添加can故障表
            canfault = "";

            /// 故障值  0正常         /// 故障等级    1	轻微故障,2	一般故障,3	严重故障,4	致命故障
            int faultlever = 1, faultint = 0;

            //soc_30,Curent_60,...
            //新版本入库 2035-02-06 runningrecord
            string jeams = "";

            string str1 = "", str2 = "", str3 = "";
            int data1 = 0, data2 = 0, data3 = 0;

            string str_data1, str_data2, str_data3, str_data4, str_data;

            Regex regex = new Regex("\\s+");
            if (index == 0)
            {
                #region 东风S30
                str_time = infos[1].Substring(36, 17);
                cantime = PublicMethods.AnalysisTime(str_time).ToString("yyyy-MM-dd HH:mm:ss");
                Candata_s30 cd30 = cd as Candata_s30;
                for (int i = 1; i < infos.Length; i++)
                {
                    string bs = infos[i].Substring(0, 5);

                    string str_all = infos[i].ToString();

                    switch (bs)
                    {
                        //瞬时油耗
                        case "04 88":
                            str1 = str_all.Substring(9, 2);
                            data1 = Convert.ToInt32(str1, 16);
                            cd30.InsFuelConsumed = (data1 * 80).ToString();
                            yh = cd30.InsFuelConsumed;
                            jeams += ",InsFuelConsumed-" + cd30.InsFuelConsumed;
                            break;

                        //系统故障代码//
                        case "05 22":
                            str1 = str_all.Substring(18, 2);
                            cd30.Error0 = Convert.ToInt32(str1, 16).ToString(); //故障0
                            jeams += ",Error0" + "-" + cd30.Error0;

                            str2 = str_all.Substring(21, 2);
                            cd30.Error1 = Convert.ToInt32(str2, 16).ToString(); //故障1
                            jeams += ",Error1" + "-" + cd30.Error1;

                            str3 = str_all.Substring(24, 2);
                            cd30.Error2 = Convert.ToInt32(str3, 16).ToString(); //故障2
                            jeams += ",Error2" + "-" + cd30.Error2;


                            str1 = str_all.Substring(27, 2);
                            data1 = Convert.ToInt32(str1, 16);
                            str1 = Convert.ToString(data1, 2);
                            if (str1.Length < 8)
                                str1 = str1.PadLeft(8, '0');
                            str2 = str1.Substring(6, 2);
                            cd30.Err_level = Convert.ToInt32(str2).ToString(); //故障等级//
                            jeams += ",Err_level" + "-" + cd30.Err_level;

                            break;

                        //预留信息//
                        case "05 13":
                            str1 = str_all.Substring(6, 2);
                            data1 = Convert.ToInt32(str1, 16);
                            str2 = str_all.Substring(9, 2);
                            data2 = Convert.ToInt32(str2, 16);
                            str3 = str_all.Substring(12, 2);
                            data3 = Convert.ToInt32(str3, 16);
                            cd30.Start_total_timers = (data1 + data2 * 256 + data3 * 65536).ToString();//启停总次数//
                            jeams += ",Start_total_timers" + "-" + cd30.Start_total_timers;


                            str1 = str_all.Substring(15, 2);
                            cd30.Reserved_info_1 = Convert.ToInt32(str1, 16).ToString();
                            jeams += ",Reserved_info1" + "-" + cd30.Reserved_info_1;

                            str1 = str_all.Substring(18, 2);
                            cd30.Reserved_info_2 = Convert.ToInt32(str1, 16).ToString();
                            jeams += ",Reserved_info2" + "-" + cd30.Reserved_info_2;

                            str1 = str_all.Substring(21, 2);
                            cd30.Reserved_info_3 = Convert.ToInt32(str1, 16).ToString();
                            jeams += ",Reserved_info3" + "-" + cd30.Reserved_info_3;

                            str1 = str_all.Substring(24, 2);
                            cd30.Reserved_info_4 = Convert.ToInt32(str1, 16).ToString();
                            jeams += ",Reserved_info4" + "-" + cd30.Reserved_info_4;

                            str1 = str_all.Substring(27, 2);
                            cd30.Reserved_info_5 = Convert.ToInt32(str1, 16).ToString();
                            jeams += ",Reserved_info5" + "-" + cd30.Reserved_info_5;

                            break;

                        case "0C F5":
                            //功能切换开关
                            str1 = str_all.Substring(12, 2);
                            data1 = Convert.ToInt32(str1, 16);
                            str1 = Convert.ToString(data1, 2);
                            if (str1.Length < 8)
                                str1 = str1.PadLeft(8, '0');
                            str2 = str1.Substring(5, 1);
                            cd30.Eco_off_switch = Convert.ToInt32(str2).ToString();
                            jeams += ",Eco_off_switch" + "-" + cd30.Eco_off_switch;

                            //电池电压
                            str1 = str_all.Substring(27, 2);
                            data1 = Convert.ToInt32(str1, 16);
                            str2 = str_all.Substring(30, 2);
                            data2 = Convert.ToInt32(str2, 16);
                            cd30.Bat_vol = ((data1 + data2 * 256) * 0.01).ToString();
                            jeams += ",Battery_Voltage" + "-" + cd30.Bat_vol;

                            break;
                        default:
                            break;
                    }
                }
                #endregion

                //  sqlstr = string.Format("update ev_{0} set eco_off_switch={1},bat_vol={2},InsFuelConsumed={3},error0={4},error1={5},error2={6},err_level={7},start_total_timers={8},reserved_info5={9},reserved_info1={10},reserved_info2={11},reserved_info3={12},reserved_info4={13} where time='{14}'", sysno, cd30.Eco_off_switch, cd30.Bat_vol, cd30.InsFuelConsumed, cd30.Error0, cd30.Error1, cd30.Error2, cd30.Err_level, cd30.Start_total_timers, cd30.Reserved_info_5, cd30.Reserved_info_1, cd30.Reserved_info_2, cd30.Reserved_info_3, cd30.Reserved_info_4, ltime);

                //alarm = string.Format("0,{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}", cd.Eco_off_switch, cd.Bat_vol, cd.InsFuelConsumed, cd.Error0, cd.Error1, cd.Error2, cd.Err_level, cd.Start_total_timers, cd.Reserved_info_5, cd.Reserved_info_1, cd.Reserved_info_2, cd.Reserved_info_3, cd.Reserved_info_4);

                //alarm = "{\"index\":\"0\",\"Sysno\":\"" + sysno + "\",\"time\":\"" + ltime + "\",\"Eco_off_switch\":\"" + cd30.Eco_off_switch + "\",\"Bat_vol\":\"" + cd30.Bat_vol + "\",\"InsFuelConsumed\":\"" + cd30.InsFuelConsumed + "\",\"Error0\":\"" + cd30.Error0 + "\",\"Error1\":\"" + cd30.Error1 + "\",\"Error2\":\"" + cd30.Error2 + "\",\"Err_level\":\"" + cd30.Err_level + "\",\"Start_total_timers\":\"" + cd30.Start_total_timers + "\",\"Reserved_info_5\":\"" + cd30.Reserved_info_5 + "\",\"Reserved_info_1\":\"" + cd30.Reserved_info_1 + "\",\"Reserved_info_2\":\"" + cd30.Reserved_info_2 + "\",\"Reserved_info_3\":\"" + cd30.Reserved_info_3 + "\",\"Reserved_info_4\":\"" + cd30.Reserved_info_4 + "\"}";

                // data = "\"Sysno\":\"" + sysno + "\",\"Time\":\"" + ltime + "\",\"Eco_Off_Switch\":\"" + cd30.Eco_off_switch + "\",\"Bat_Vol\":\"" + cd30.Bat_vol + "\",\"InsFuelConsumed\":\"" + cd30.InsFuelConsumed + "\",\"Error0\":\"" + cd30.Error0 + "\",\"Error1\":\"" + cd30.Error1 + "\",\"Error2\":\"" + cd30.Error2 + "\",\"Err_Level\":\"" + cd30.Err_level + "\",\"Start_Total_Timers\":\"" + cd30.Start_total_timers + "\",\"Reserved_Info_5\":\"" + cd30.Reserved_info_5 + "\",\"Reserved_Info_1\":\"" + cd30.Reserved_info_1 + "\",\"Reserved_Info_2\":\"" + cd30.Reserved_info_2 + "\",\"Reserved_Info_3\":\"" + cd30.Reserved_info_3 + "\",\"Reserved_Info_4\":\"" + cd30.Reserved_info_4 + "\"";


            }
            else if (index == 1)
            {
                #region 聆风
                Candata_Ev_lf cdlf = cd as Candata_Ev_lf;
                str_time = infos[1].Substring(39, 17);
                cantime = PublicMethods.AnalysisTime(str_time).ToString("yyyy-MM-dd HH:mm:ss");
                for (int i = 1; i < infos.Length; i++)
                {
                    string bs = infos[i].Substring(0, 5);

                    string str_all = infos[i].ToString();

                    switch (bs)
                    {
                        //电流---使用补码
                        case "01 DB":
                            str1 = str_all.Substring(9, 2);
                            data1 = Convert.ToInt32(str1, 16);
                            str1 = Convert.ToString(data1, 2);
                            if (str1.Length < 8)
                                str1 = str1.PadLeft(8, '0');

                            str2 = str_all.Substring(12, 2);
                            data2 = Convert.ToInt32(str2, 16);
                            str2 = Convert.ToString(data2, 2);
                            if (str2.Length < 8)
                                str2 = str2.PadLeft(8, '0');
                            str2 = str2.Substring(0, 3);

                            str3 = string.Format("{0}{1}", str1, str2);
                            string fuhao = str3.Substring(0, 1);
                            data3 = Convert.ToInt32(str3.Substring(1, 10), 2);
                            if (fuhao == "1")
                            {
                                int temp1 = Convert.ToInt32("1111111111", 2);
                                data3 = -((data3 ^ temp1) + 1);
                            }
                            cdlf.Curent = data3 * 0.5;


                            //电压
                            str1 = str_all.Substring(15, 2);
                            data1 = Convert.ToInt32(str1, 16);
                            str1 = Convert.ToString(data1, 2);

                            str2 = str_all.Substring(18, 2);
                            data2 = Convert.ToInt32(str2, 16);
                            str2 = Convert.ToString(data2, 2);
                            if (str2.Length < 8)
                                str2 = str2.PadLeft(8, '0');
                            str2 = str2.Substring(0, 2);

                            str3 = string.Format("{0}{1}", str1, str2);
                            data3 = Convert.ToInt32(str3, 2);
                            cdlf.Voltage = data3 * 0.5;
                            break;

                        //SOC
                        case "05 5B":
                            str1 = str_all.Substring(9, 2);
                            data1 = Convert.ToInt32(str1, 16);
                            str1 = Convert.ToString(data1, 2);

                            str2 = str_all.Substring(12, 2);
                            data2 = Convert.ToInt32(str2, 16);
                            str2 = Convert.ToString(data2, 2);
                            if (str2.Length < 8)
                                str2 = str2.PadLeft(8, '0');
                            str2 = str2.Substring(0, 2);

                            str3 = string.Format("{0}{1}", str1, str2);
                            data3 = Convert.ToInt32(str3, 2);
                            cdlf.SOC = data3 * 0.1;
                            break;

                        //电机转速---使用补码
                        case "01 DA":
                            str1 = str_all.Substring(21, 2);
                            data1 = Convert.ToInt32(str1, 16);
                            str1 = Convert.ToString(data1, 2);
                            if (str1.Length < 8)
                                str1 = str1.PadLeft(8, '0');

                            str2 = str_all.Substring(24, 2);
                            data2 = Convert.ToInt32(str2, 16);
                            str2 = Convert.ToString(data2, 2);
                            if (str2.Length < 8)
                                str2 = str2.PadLeft(8, '0');
                            str2 = str2.Substring(0, 7);

                            str3 = string.Format("{0}{1}", str1, str2);
                            string fuhao2 = str3.Substring(0, 1);
                            data3 = Convert.ToInt32(str3.Substring(1, 14), 2);
                            if (fuhao2 == "1")
                            {
                                int temp2 = Convert.ToInt32("11111111111111", 2);
                                data3 = -((data3 ^ temp2) + 1);
                            }
                            cdlf.MotorRevolution = data3;
                            break;

                        //电机温度
                        case "05 5A":
                            str1 = str_all.Substring(12, 2);
                            data1 = Convert.ToInt32(str1, 16);
                            cdlf.MotorTemperature = data1 - 40;
                            break;

                        //充电器输出功率
                        case "03 80":
                            str1 = str_all.Substring(21, 2);
                            data1 = Convert.ToInt32(str1, 16);
                            str1 = Convert.ToString(data1, 2);
                            if (str1.Length < 8)
                                str1 = str1.PadLeft(8, '0');
                            str1 = str1.Substring(4, 4);

                            str2 = str_all.Substring(24, 2);
                            data2 = Convert.ToInt32(str2, 16);
                            str2 = Convert.ToString(data2, 2);
                            if (str2.Length < 8)
                                str2 = str2.PadLeft(8, '0');
                            str2 = str2.Substring(0, 5);

                            str3 = string.Format("{0}{1}", str1, str2);
                            data3 = Convert.ToInt32(str3, 2);
                            cdlf.OutputPower = data3 * 0.1;
                            break;

                        //车速
                        case "03 55":
                            str1 = str_all.Substring(15, 2);
                            str2 = str_all.Substring(18, 2);

                            str3 = string.Format("{0}{1}", str1, str2);
                            data3 = Convert.ToInt32(str3, 16);
                            cdlf.ABS_VehSpd = data3 * 0.01;
                            break;

                        //行驶总里程
                        case "05 C5":
                            str1 = str_all.Substring(12, 2);
                            str2 = str_all.Substring(15, 2);
                            str3 = str_all.Substring(18, 2);

                            str3 = string.Format("{0}{1}{2}", str1, str2, str3);
                            data3 = Convert.ToInt32(str3, 16);
                            cdlf.Distance = data3;
                            break;

                        //电池组温度
                        case "05 C0":
                            str1 = str_all.Substring(9, 2);
                            data1 = Convert.ToInt32(str1, 16);
                            str1 = Convert.ToString(data1, 2);
                            if (str1.Length < 8)
                                str1 = str1.PadLeft(8, '0');
                            str1 = str1.Substring(0, 2);

                            str2 = str_all.Substring(15, 2);
                            data2 = Convert.ToInt32(str2, 16);
                            str2 = Convert.ToString(data2, 2);
                            if (str2.Length < 8)
                                str2 = str2.PadLeft(8, '0');
                            str2 = str2.Substring(0, 7);
                            data2 = Convert.ToInt32(str2, 2);
                            if (str1 == "00")//无效
                                break;
                            if (str1 == "01")//最大
                            {
                                cdlf.BatteryTemp_Max = data2 - 40;
                            }

                            if (str1 == "10")//平均
                            {
                                cdlf.BatteryTemp_Ave = data2 - 40;
                            }

                            if (str1 == "11")//最小
                            {
                                cdlf.BatteryTemp_Min = data2 - 40;
                            }
                            break;
                        default:
                            break;
                    }
                }

                #endregion
                //  sqlstr = string.Format("update ev_{0} set Curent={1},Voltage={2},SOC={3},MotorRevolution={4},MotorTemperature={5},OutputPower={6},ABS_VehSpd={7},Distance={8},BatteryTemp_Ave={9},BatteryTemp_Max={10},BatteryTemp_Min={11} where time='{12}'", sysno, cdlf.Curent, cdlf.Voltage, cdlf.SOC, cdlf.MotorRevolution, cdlf.MotorTemperature, cdlf.OutputPower, cdlf.ABS_VehSpd, cdlf.Distance, cdlf.BatteryTemp_Ave, cdlf.BatteryTemp_Max, cdlf.BatteryTemp_Min, ltime);
                // alarm = "{\"index\":\"1\",\"Sysno\":\"" + sysno + "\",\"time\":\"" + ltime + "\",\"Curent\":\"" + cdlf.Curent + "\",\"Voltage\":\"" + cdlf.Voltage + "\",\"SOC\":\"" + cdlf.SOC + "\",\"MotorRevolution\":\"" + cdlf.MotorRevolution + "\",\"MotorTemperature\":\"" + cdlf.MotorTemperature + "\",\"OutputPower\":\"" + cdlf.OutputPower + "\",\"ABS_VehSpd\":\"" + cdlf.ABS_VehSpd + "\",\"Distance\":\"" + cdlf.Distance + "\",\"BatteryTemp_Ave\":\"" + cdlf.BatteryTemp_Ave + "\",\"BatteryTemp_Max\":\"" + cdlf.BatteryTemp_Max + "\",\"BatteryTemp_Min\":\"" + cdlf.BatteryTemp_Min + "\"}";

                //  data = "\"Sysno\":\"" + sysno + "\",\"time\":\"" + ltime + "\",\"Curent\":\"" + cdlf.Curent + "\",\"Voltage\":\"" + cdlf.Voltage + "\",\"SOC\":\"" + cdlf.SOC + "\",\"MotorRevolution\":\"" + cdlf.MotorRevolution + "\",\"MotorTemperature\":\"" + cdlf.MotorTemperature + "\",\"OutputPower\":\"" + cdlf.OutputPower + "\",\"ABS_VehSpd\":\"" + cdlf.ABS_VehSpd + "\",\"Distance\":\"" + cdlf.Distance + "\",\"BatteryTemp_Ave\":\"" + cdlf.BatteryTemp_Ave + "\",\"BatteryTemp_Max\":\"" + cdlf.BatteryTemp_Max + "\",\"BatteryTemp_Min\":\"" + cdlf.BatteryTemp_Min + "\"";

            }
            else if (index == 2)
            {
                #region EJ02
                Candata_EJ02 cd_ej02 = cd as Candata_EJ02;
                str_time = infos[1].Substring(36, 17);
                cantime = PublicMethods.AnalysisTime(str_time).ToString("yyyy-MM-dd HH:mm:ss");
                for (int i = 1; i < infos.Length; i++)
                {
                    string bs = infos[i].Substring(0, 5);

                    string str_all = infos[i].ToString();

                    switch (bs)
                    {
                        //总里程//小计里程
                        case "FF 31":

                            str_data1 = str_all.Substring(12, 2);
                            str_data2 = str_all.Substring(15, 2);
                            str_data3 = str_all.Substring(18, 2);
                            str_data4 = str_all.Substring(21, 2);
                            str_data = string.Format("{0}{1}{2}{3}", str_data4, str_data3, str_data2, str_data1);
                            cd_ej02.IC_TotalOdmeter = Convert.ToInt32(str_data, 16) / 10;

                            str_data1 = str_all.Substring(24, 2);
                            str_data2 = str_all.Substring(27, 2);
                            str_data3 = str_all.Substring(30, 2);
                            str_data4 = str_all.Substring(33, 2);
                            str_data = string.Format("{0}{1}{2}{3}", str_data4, str_data3, str_data2, str_data1);
                            cd_ej02.IC_Odmeter = Convert.ToInt32(str_data, 16) / 10;
                            break;

                        //充电机
                        case "FF 80":
                            str_data1 = str_all.Substring(12, 2);//电压
                            str_data2 = str_all.Substring(15, 2);
                            str_data = string.Format("{0}{1}", str_data2, str_data1);
                            cd_ej02.ONC_OutputVoltage = (Convert.ToInt32(str_data, 16)) / 10 - 3276.8;

                            str_data3 = str_all.Substring(18, 2);
                            str_data4 = str_all.Substring(21, 2);//电流
                            str_data = string.Format("{0}{1}", str_data4, str_data3);
                            cd_ej02.ONC_OutputCurrent = Convert.ToInt32(str_data, 16) / 10;

                            str_data1 = str_all.Substring(27, 2);//温度
                            str_data1 = Convert.ToString(Convert.ToInt32(str_data1, 16), 2);
                            str_data1 = str_data1.PadLeft(8, '0');
                            str_data1 = str_data1.Substring(2, 6);
                            cd_ej02.ONC_ONCTemp = Convert.ToInt32(str_data1, 2) - 100;

                            str_data2 = str_all.Substring(24, 2);//输入电压状态
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data2 = Convert.ToString(data1, 2);
                            if (str_data2.Length < 8)
                                str_data2 = str_data2.PadLeft(8, '0');
                            str_data3 = str_data2.Substring(5, 1);
                            if (str_data3 == "0")
                                cd_ej02.ONC_InputVoltageSt = "正常";
                            else
                                cd_ej02.ONC_InputVoltageSt = "移除";

                            str_data4 = str_data2.Substring(3, 1);//通讯状态
                            if (str_data4 == "0")
                                cd_ej02.ONC_CommunicationSt = "正常";
                            else
                                cd_ej02.ONC_CommunicationSt = "超时";

                            cd_ej02.ONC_Fault = "";
                            str_data1 = str_all.Substring(30, 1);//故障码
                            str_data2 = str_all.Substring(33, 2);
                            str_data = string.Format("{0}{1}", str_data2, str_data1);
                            str_data3 = Convert.ToString(Convert.ToInt32(str_data, 16), 2);
                            for (int j = 0; j < str_data3.Length; j++)
                            {
                                if (str_data3[j] == '1')
                                    cd_ej02.ONC_Fault += oncfault[j] + ",";
                            }
                            break;

                        //电池组
                        case "FF 20":
                            str_data1 = str_all.Substring(12, 2);
                            str_data2 = str_all.Substring(15, 2);
                            str_data = string.Format("{0}{1}", str_data2, str_data1);
                            cd_ej02.BMS_TotalVol = Convert.ToInt32(str_data, 16) / 64;
                            str_data2 = str_all.Substring(18, 2);
                            cd_ej02.BMS_MaxCellBatt = Convert.ToInt32(str_data2, 16) / 64;
                            str_data1 = str_all.Substring(21, 2);
                            cd_ej02.BMS_MaxCellBattNumber = Convert.ToInt32(str_data1, 16);
                            str_data2 = str_all.Substring(24, 2);
                            cd_ej02.BMS_MinCellBatt = Convert.ToInt32(str_data2, 16) * 0.02;
                            str_data1 = str_all.Substring(27, 2);
                            cd_ej02.BMS_MinCellBattNumber = Convert.ToInt32(str_data1, 16);
                            break;
                        //电池包温度
                        case "FF 21":
                            str_data1 = str_all.Substring(18, 2);
                            cd_ej02.BMS_MaxTemp = Convert.ToInt32(str_data1, 16) - 40;
                            str_data2 = str_all.Substring(24, 2);
                            cd_ej02.BMS_MinTemp = Convert.ToInt32(str_data2, 16) - 40;
                            str_data = str_all.Substring(30, 2);
                            cd_ej02.BMS_BattTempAvg = Convert.ToInt32(str_data, 16) - 40;
                            str_data3 = str_all.Substring(21, 2);
                            cd_ej02.BMS_MaxTempNumber = Convert.ToInt32(str_data3, 16);
                            str_data4 = str_all.Substring(27, 2);
                            cd_ej02.BMS_MinTempNumber = Convert.ToInt32(str_data4, 16);
                            break;
                        //单次充电量
                        case "FF 22":
                            str_data = str_all.Substring(18, 2);
                            cd_ej02.BMS_ChargeQuantity = Convert.ToInt32(str_data, 16) * 0.5;
                            break;
                        //充电状态
                        case "FF 23":
                            str_data = str_all.Substring(28, 1);
                            data1 = Convert.ToInt32(str_data, 16);
                            str_data = Convert.ToString(data1, 2);
                            if (str_data.Length < 4)
                                str_data = str_data.PadLeft(4, '0');
                            str_data1 = str_data.Substring(2, 2);
                            data1 = Convert.ToInt32(str_data1, 2);
                            switch (data1)
                            {
                                case 0:
                                    cd_ej02.BMS_ChargeSt = "非充电状态";
                                    break;
                                case 1:
                                    cd_ej02.BMS_ChargeSt = "正在充电";
                                    break;
                                case 2:
                                    cd_ej02.BMS_ChargeSt = "充电故障";
                                    break;
                                case 3:
                                    cd_ej02.BMS_ChargeSt = "充电完成";
                                    break;
                                default:
                                    break;
                            }
                            str_data2 = str_data.Substring(1, 1);
                            if (str_data2 == "0")
                                cd_ej02.BMS_ChargerACInput = "正常";
                            else
                                cd_ej02.BMS_ChargerACInput = "移除";
                            str_data3 = str_data.Substring(0, 1);
                            if (str_data3 == "0")
                                cd_ej02.BMS_ChargerDCInput = "正常";
                            else
                                cd_ej02.BMS_ChargerDCInput = "移除";
                            str_data4 = str_all.Substring(25, 1);
                            str_data4 = Convert.ToString(Convert.ToInt32(str_data4, 16), 2);
                            str_data4 = str_data4.Substring(str_data4.Length - 1, 1);
                            if (str_data4 == "0")
                                cd_ej02.BMS_OnBoardChargerEnable = "允许充电";
                            else
                                cd_ej02.BMS_OnBoardChargerEnable = "禁止充电";
                            break;

                        //动力电池状态
                        case "FF 24":
                            str_data1 = str_all.Substring(12, 2);
                            cd_ej02.BMS_SOC = Convert.ToInt32(str_data1, 16) * 0.4;
                            str_data3 = str_all.Substring(15, 2);
                            str_data4 = str_all.Substring(18, 2);
                            str_data = string.Format("{0}{1}", str_data4, str_data3);
                            cd_ej02.BMS_Current = Convert.ToInt32(str_data, 16) / 8 - 4016;
                            str_data2 = str_all.Substring(24, 1);
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data2 = Convert.ToString(data1, 2);
                            if (str_data2.Length < 4)
                                str_data2 = str_data2.PadLeft(4, '0');
                            str_data2 = str_data2.Substring(2, 2);
                            switch (str_data2)
                            {
                                case "00":
                                    cd_ej02.BMS_FaultState = "正常";
                                    break;
                                case "01":
                                    cd_ej02.BMS_FaultState = "一级故障";
                                    break;
                                case "10":
                                    cd_ej02.BMS_FaultState = "二级故障";
                                    break;
                                default:
                                    break;
                            }
                            str_data3 = str_all.Substring(21, 2);//电池自检状态
                            str_data3 = Convert.ToString(Convert.ToInt32(str_data3, 16), 2);
                            if (str_data3.Length < 8)
                                str_data3 = str_data3.PadLeft(8, '0');
                            str_data4 = str_data3.Substring(6, 2);
                            switch (str_data4)
                            {
                                case "00":
                                    cd_ej02.BMS_BattSelfCheckStatus = "自检未完成";
                                    break;
                                case "01":
                                    cd_ej02.BMS_BattSelfCheckStatus = "自检通过";
                                    break;
                                case "10":
                                    cd_ej02.BMS_BattSelfCheckStatus = "自检失败";
                                    break;
                                case "11":
                                    cd_ej02.BMS_BattSelfCheckStatus = "无效";
                                    break;
                                default:
                                    break;
                            }
                            str_data4 = str_data3.Substring(0, 2);//电池高压状态
                            switch (str_data4)
                            {
                                case "00":
                                    cd_ej02.BMS_HighVolSt = "高压电断开";
                                    break;
                                case "01":
                                    cd_ej02.BMS_HighVolSt = "高压电接通";
                                    break;
                                case "11":
                                    cd_ej02.BMS_HighVolSt = "无效";
                                    break;
                                default:
                                    break;
                            }
                            break;

                        //BMS故障码和剩余里程  
                        case "FF E1":
                            //BMS故障码
                            str_data1 = str_all.Substring(12, 2);
                            switch (str_data1)
                            {
                                case "00":
                                    cd_ej02.BMS_Fault = "无故障";
                                    break;
                                case "21":
                                    cd_ej02.BMS_Fault = "轻微故障：辅助电瓶电压预警";
                                    break;
                                case "41":
                                    cd_ej02.BMS_Fault = "一般故障：系统过压预警";
                                    break;
                                case "42":
                                    cd_ej02.BMS_Fault = "一般故障：单体过压预警";
                                    break;
                                case "43":
                                    cd_ej02.BMS_Fault = "一般故障：系统欠压预警";
                                    break;
                                case "44":
                                    cd_ej02.BMS_Fault = "一般故障：单体欠压预警";
                                    break;
                                case "45":
                                    cd_ej02.BMS_Fault = "一般故障：慢充连接异常";
                                    break;
                                case "46":
                                    cd_ej02.BMS_Fault = "一般故障：快充连接异常";
                                    break;
                                case "47":
                                    cd_ej02.BMS_Fault = "一般故障：预充电故障";
                                    break;
                                case "48":
                                    cd_ej02.BMS_Fault = "一般故障：温度差异大预警";
                                    break;
                                case "49":
                                    cd_ej02.BMS_Fault = "一般故障：SOC低警告";
                                    break;
                                case "4A":
                                    cd_ej02.BMS_Fault = "一般故障：电压不均衡预警";
                                    break;
                                case "4B":
                                    cd_ej02.BMS_Fault = "一般故障：绝缘过低预警";
                                    break;
                                case "4C":
                                    cd_ej02.BMS_Fault = "一般故障：放电电流大预警";
                                    break;
                                case "4D":
                                    cd_ej02.BMS_Fault = "一般故障：充电电流大预警";
                                    break;
                                case "4E":
                                    cd_ej02.BMS_Fault = "一般故障：子网通讯异常报警";
                                    break;
                                case "4F":
                                    cd_ej02.BMS_Fault = "一般故障：SOC低报警";
                                    break;
                                case "50":
                                    cd_ej02.BMS_Fault = "一般故障：温度差异大报警";
                                    break;
                                case "51":
                                    cd_ej02.BMS_Fault = "一般故障：单体欠压报警";
                                    break;
                                case "52":
                                    cd_ej02.BMS_Fault = "一般故障：系统欠压报警";
                                    break;
                                case "61":
                                    cd_ej02.BMS_Fault = "严重故障：充电电流大报警";
                                    break;
                                case "62":
                                    cd_ej02.BMS_Fault = "严重故障：系统过压报警";
                                    break;
                                case "63":
                                    cd_ej02.BMS_Fault = "严重故障：单体过压报警";
                                    break;
                                case "64":
                                    cd_ej02.BMS_Fault = "严重故障：单体温度过低";
                                    break;
                                case "65":
                                    cd_ej02.BMS_Fault = "严重故障：单体温度过高";
                                    break;
                                case "66":
                                    cd_ej02.BMS_Fault = "严重故障：放电电流大报警";
                                    break;
                                case "67":
                                    cd_ej02.BMS_Fault = "严重故障：绝缘过低报警";
                                    break;
                                case "68":
                                    cd_ej02.BMS_Fault = "严重故障：充电器通讯故障";
                                    break;
                                case "69":
                                    cd_ej02.BMS_Fault = "严重故障：放电回路高压互锁故障";
                                    break;
                                case "6A":
                                    cd_ej02.BMS_Fault = "严重故障：充电回路高压互锁故障";
                                    break;
                                case "6B":
                                    cd_ej02.BMS_Fault = "严重故障：辅助电瓶电压过低警告";
                                    break;
                                case "81":
                                    cd_ej02.BMS_Fault = "致命故障：继电器粘连";
                                    break;
                                case "82":
                                    cd_ej02.BMS_Fault = "致命故障：碰撞断电";
                                    break;
                                default:
                                    cd_ej02.BMS_Fault = "无故障";
                                    break;
                            }
                            //剩余行驶里程
                            str_data2 = str_all.Substring(21, 2);
                            str_data3 = str_all.Substring(25, 1);
                            str_data = string.Format("{0}{1}", str_data3, str_data2);
                            str_data = Convert.ToString(Convert.ToInt32(str_data, 16), 2);
                            if (str_data.Length < 12)
                                str_data = str_data.PadLeft(12, '0');
                            str_data = str_data.Substring(2, 10);
                            cd_ej02.BMS_CruisingRange = Convert.ToInt32(str_data, 2) * 1.0;
                            break;

                        //电机状态
                        case "FF 10":
                            str_data1 = str_all.Substring(12, 2);
                            str_data2 = str_all.Substring(15, 2);
                            str_data = string.Format("{0}{1}", str_data2, str_data1);
                            cd_ej02.Motor_DCVolt = Convert.ToInt32(str_data, 16) * 0.5 - 32127.5;
                            str_data3 = str_all.Substring(18, 2);
                            str_data4 = str_all.Substring(21, 2);
                            str_data = string.Format("{0}{1}", str_data4, str_data3);
                            cd_ej02.Motor_DCCurrent = Convert.ToInt32(str_data, 16) * 0.125 - 4015.94;
                            str_data2 = str_all.Substring(27, 2);
                            cd_ej02.Motor_Temperature = Convert.ToInt32(str_data2, 16) - 40;
                            break;

                        //电机状态//==============增加
                        case "FF 11":
                            str_data = str_all.Substring(30, 1);
                            str_data = Convert.ToString(Convert.ToInt32(str_data, 16), 2);
                            if (str_data.Length < 4)
                                str_data = str_data.PadLeft(4, '0');
                            str_data = str_data.Substring(2, 2);
                            switch (str_data)
                            {
                                case "00":
                                    cd_ej02.Motor_MotorStatus = "未就绪";
                                    break;
                                case "01":
                                    cd_ej02.Motor_MotorStatus = "就绪";
                                    break;
                                case "10":
                                    cd_ej02.Motor_MotorStatus = "无效";
                                    break;
                                case "11":
                                    cd_ej02.Motor_MotorStatus = "无效";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        //实时电机功率
                        //double Motor_OutputPower;//实时电机功率
                        //string _DiagLevel;//故障等级
                        case "FF 13":
                            str_data1 = str_all.Substring(12, 2);
                            str_data2 = str_all.Substring(15, 2);
                            str_data = string.Format("{0}{1}", str_data2, str_data1);
                            cd_ej02.Motor_OutputPower = Convert.ToInt32(str_data, 16) * 0.1;
                            break;

                        //故障码 MCU1_FaultNum//MCU1故障码
                        case "FF 43":
                            str_data = str_all.Substring(24, 2);
                            data1 = Convert.ToInt32(str_data, 16);
                            switch (data1)
                            {
                                case 85:
                                    cd_ej02.MCU1_FaultNum = "母线欠压故障";
                                    break;
                                case 87:
                                    cd_ej02.MCU1_FaultNum = "温度传感器故障";
                                    break;
                                case 109:
                                    cd_ej02.MCU1_FaultNum = "低压电过压故障";
                                    break;
                                case 110:
                                    cd_ej02.MCU1_FaultNum = "IPM故障";
                                    break;
                                case 111:
                                    cd_ej02.MCU1_FaultNum = "低压电欠压故障";
                                    break;
                                case 112:
                                    cd_ej02.MCU1_FaultNum = "霍尔故障";
                                    break;
                                case 115:
                                    cd_ej02.MCU1_FaultNum = "相电流过流故障";
                                    break;
                                case 116:
                                    cd_ej02.MCU1_FaultNum = "控制器温度过高";
                                    break;
                                case 117:
                                    cd_ej02.MCU1_FaultNum = "电机温度过高";
                                    break;
                                case 118:
                                    cd_ej02.MCU1_FaultNum = "CAN通信故障";
                                    break;
                                case 129:
                                    cd_ej02.MCU1_FaultNum = "母线电压过压故障";
                                    break;
                                case 130:
                                    cd_ej02.MCU1_FaultNum = "母线电流过流故障";
                                    break;
                                default:
                                    cd_ej02.MCU1_FaultNum = "无故障";
                                    break;
                            }
                            break;


                        //故障码
                        //string MCU2_FaultNum2;//MCU2故障码
                        case "FF 15":
                            str_data = str_all.Substring(21, 2);
                            data1 = Convert.ToInt32(str_data, 16);
                            switch (data1)
                            {
                                case 85:
                                    cd_ej02.MCU2_FaultNum2 = "母线欠压故障";
                                    break;
                                case 87:
                                    cd_ej02.MCU2_FaultNum2 = "温度传感器故障";
                                    break;
                                case 109:
                                    cd_ej02.MCU2_FaultNum2 = "低压电过压故障";
                                    break;
                                case 110:
                                    cd_ej02.MCU2_FaultNum2 = "IPM故障";
                                    break;
                                case 111:
                                    cd_ej02.MCU2_FaultNum2 = "低压电欠压故障";
                                    break;
                                case 112:
                                    cd_ej02.MCU2_FaultNum2 = "霍尔故障";
                                    break;
                                case 115:
                                    cd_ej02.MCU2_FaultNum2 = "相电流过流故障";
                                    break;
                                case 116:
                                    cd_ej02.MCU2_FaultNum2 = "控制器温度过高";
                                    break;
                                case 118:
                                    cd_ej02.MCU2_FaultNum2 = "CAN通信故障";
                                    break;
                                case 129:
                                    cd_ej02.MCU2_FaultNum2 = "母线电压过压故障";
                                    break;
                                case 130:
                                    cd_ej02.MCU2_FaultNum2 = "母线电流过流故障";
                                    break;
                                default:
                                    cd_ej02.MCU2_FaultNum2 = "无故障";
                                    break;
                            }
                            break;

                        //电机控制器温度MCU2_RadiatorTemp////===========增加
                        case "FF 17":
                            str_data = str_all.Substring(12, 2);
                            cd_ej02.MCU2_RadiatorTemp = Convert.ToInt32(str_data, 16) - 40;
                            break;

                        //档位
                        //string TCU_GearPosition;//换挡手柄位置
                        //string TCU_TransRealPosition;//变速箱实际档位
                        case "FF 01":
                            str_data = str_all.Substring(12, 2);
                            data1 = Convert.ToInt32(str_data, 16);
                            str_data = Convert.ToString(data1, 2);
                            if (str_data.Length < 8)
                                str_data = str_data.PadLeft(8, '0');
                            str_data2 = str_data.Substring(3, 3);
                            switch (str_data2)
                            {
                                case "000":
                                    cd_ej02.TCU_TransRealPosition = "N";
                                    break;
                                case "001":
                                    cd_ej02.TCU_TransRealPosition = "L";
                                    break;
                                case "010":
                                    cd_ej02.TCU_TransRealPosition = "D";
                                    break;
                                default:
                                    cd_ej02.TCU_TransRealPosition = "预留";
                                    break;
                            }
                            break;


                        //电机状态控制VCU_MotorStCtrl////============增加
                        case "FF 06":
                            str_data = str_all.Substring(12, 1);
                            str_data = Convert.ToString(Convert.ToInt32(str_data, 16), 2);
                            if (str_data.Length < 4)
                                str_data = str_data.PadLeft(4, '0');
                            str_data = str_data.Substring(1, 3);
                            switch (str_data)
                            {
                                case "000":
                                    cd_ej02.VCU_MotorStCtrl = "空转";
                                    break;
                                case "001":
                                    cd_ej02.VCU_MotorStCtrl = "正向旋转扭矩控制";
                                    break;
                                case "010":
                                    cd_ej02.VCU_MotorStCtrl = "反向旋转扭矩控制";
                                    break;
                                case "011":
                                    cd_ej02.VCU_MotorStCtrl = "正向旋转转速控制";
                                    break;
                                case "100":
                                    cd_ej02.VCU_MotorStCtrl = "反向旋转转速控制";
                                    break;
                                case "101":
                                    cd_ej02.VCU_MotorStCtrl = "预留";
                                    break;
                                case "110":
                                    cd_ej02.VCU_MotorStCtrl = "预留";
                                    break;
                                default:
                                    break;
                            }
                            break;



                        //制动信号
                        //string ABS_BrakeSignal;//制动信号
                        //double ABS_VehSpd;//车速信号
                        case "FF 19":
                            str_data1 = str_all.Substring(19, 1);
                            data1 = Convert.ToInt32(str_data1, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 4)
                                str_data1 = str_data1.PadLeft(4, '0');
                            str_data1 = str_data1.Substring(1, 1);
                            switch (str_data1)
                            {
                                case "0":
                                    cd_ej02.ABS_BrakeSignal = "无制动操作";
                                    break;
                                case "1":
                                    cd_ej02.ABS_BrakeSignal = "有制动操作";
                                    break;
                                default:
                                    break;
                            }
                            str_data1 = str_all.Substring(30, 2);
                            str_data2 = str_all.Substring(33, 2);
                            str_data = string.Format("{0}{1}", str_data2, str_data1);
                            cd_ej02.ABS_VehSpd = Convert.ToInt32(str_data, 16) * 0.01;
                            break;
                        default:
                            break;
                    }
                }
                #endregion
                //sqlstr = string.Format("update ev_{0} set IC_TotalOdmeter={1},IC_Odmeter={2},ONC_OutputVoltage={3},ONC_OutputCurrent={4},ONC_InputVoltageSt={5},ONC_CommunicationSt={6},ONC_ONCTemp={7},ONC_Fault={8},BMS_TotalVol={9},BMS_MaxCellBatt={10},BMS_MaxCellBattNumber={11} where time='{12}'", sysno, cd_ej02.Curent, cd_ej02.Voltage, cd_ej02.SOC, cd_ej02.MotorRevolution, cd_ej02.MotorTemperature, cd_ej02.OutputPower, cd_ej02.ABS_VehSpd, cd_ej02.Distance, cd_ej02.BatteryTemp_Ave, cd_ej02.BatteryTemp_Max, cd_ej02.BatteryTemp_Min, ltime);
                //alarm = "{\"index\":\"1\",\"Sysno\":\"" + sysno + "\",\"time\":\"" + ltime + "\",\"IC_TotalOdmeter\":\"" + cd_ej02.IC_TotalOdmeter + "\",\"IC_Odmeter\":\"" + cd_ej02.IC_Odmeter + "\",\"ONC_OutputVoltage\":\"" + cd_ej02.ONC_OutputVoltage + "\",\"ONC_OutputCurrent\":\"" + cd_ej02.ONC_OutputCurrent + "\",\"ONC_InputVoltageSt\":\"" + cd_ej02.ONC_InputVoltageSt + "\",\"ONC_CommunicationSt\":\"" + cd_ej02.ONC_CommunicationSt + "\",\"ONC_ONCTemp\":\"" + cd_ej02.ONC_ONCTemp + "\",\"ONC_Fault\":\"" + cd_ej02.ONC_Fault + "\",\"BMS_TotalVol\":\"" + cd_ej02.BMS_TotalVol + "\",\"BMS_MaxCellBatt\":\"" + cd_ej02.BMS_MaxCellBatt + "\",\"BMS_MaxCellBattNumber\":\"" + cd_ej02.BMS_MaxCellBattNumber + "\"}";
                //data = "";
            }
            else if (index == 3)
            {
                #region EJ04
                Candata_EJ04 cd_ej04 = cd as Candata_EJ04;
                str_time = infos[1].Substring(30, 17);
                cantime = PublicMethods.AnalysisTime(str_time).ToString("yyyy-MM-dd HH:mm:ss");
                for (int i = 1; i < infos.Length; i++)
                {
                    string bs = infos[i].Substring(0, 5);

                    string str_all = infos[i].ToString();

                    switch (bs)
                    {   //VCU点火开关及故障码
                        //string VCU_Keyposition;//点火开关
                        //string VCU_Fault;//故障码
                        case "00 F0":
                            str_data1 = str_all.Substring(6, 1);
                            data1 = Convert.ToInt32(str_data1, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 4)
                                str_data1 = str_data1.PadLeft(4, '0');
                            str_data1 = str_data1.Substring(0, 2);
                            switch (str_data1)
                            {
                                case "00":
                                    // cd_ej04.VCU_Keyposition = "OFF";
                                    cd_ej04.VCU_Keyposition = "0";
                                    //session.isacc = "0";
                                    break;
                                case "01":
                                    //cd_ej04.VCU_Keyposition = "ACC";
                                    cd_ej04.VCU_Keyposition = "1";
                                    // session.isacc = "1";
                                    break;
                                case "10":
                                    //  cd_ej04.VCU_Keyposition = "ON";
                                    cd_ej04.VCU_Keyposition = "2";
                                    //session.isacc = "1";
                                    break;
                                case "11":
                                    //   cd_ej04.VCU_Keyposition = "START";
                                    cd_ej04.VCU_Keyposition = "3";
                                    //session.isacc = "1";
                                    break;
                                default:
                                    break;
                            }
                            str_data2 = str_all.Substring(21, 2);
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data2 = Convert.ToString(data1, 2);
                            if (str_data2.Length < 8)
                                str_data2 = str_data2.PadLeft(8, '0');
                            str_data1 = str_data2.Substring(0, 3);
                            switch (str_data1)
                            {
                                case "000":
                                    cd_ej04.VCU_Fault = 0;
                                    faultlever = 0;
                                    break;
                                case "001":
                                    cd_ej04.VCU_Fault = 1;
                                    faultlever = 1;
                                    //方便统一 ,警告变为 轻微故障
                                    break;
                                case "010":
                                    cd_ej04.VCU_Fault = 2;
                                    faultlever = 2;
                                    break;
                                case "011":
                                    cd_ej04.VCU_Fault = 3;
                                    faultlever = 3;
                                    break;
                                case "100":
                                    cd_ej04.VCU_Fault = 4;
                                    faultlever = 4;
                                    break;
                                //case "101":
                                //    cd_ej04.VCU_Fault = "预留";
                                //    break;
                                //case "110":
                                //    cd_ej04.VCU_Fault = "预留";
                                //    break;
                                //case "111":
                                //    cd_ej04.VCU_Fault = "预留";
                                //    break;
                                default:
                                    break;
                            }
                             faultint = cd_ej04.VCU_Fault;
                             if (faultint > 0)
                                 canfault += "$VCU_Fault:" + "0X" + str_all.Substring(21, 2) + ":" + faultlever + ":" + faultint;
                            break;

                        //int VCU_BrakeEnergy;//电机状态及信号强度
                        //double VCU_CruisingRange;//剩余行驶里程
                        case "03 55":
                            str_data1 = str_all.Substring(7, 1);
                            data1 = Convert.ToInt32(str_data1, 16) - 5;
                            if (data1 > 0)
                                cd_ej04.VCU_BrakeEnergy = "1";//=======================修改
                            else if (data1 == 0)
                                cd_ej04.VCU_BrakeEnergy = "0";
                            else
                                cd_ej04.VCU_BrakeEnergy = "2";
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
                            cd_ej04.VCU_CruisingRange = Convert.ToInt32(str_data, 2);
                            //TODO Filter 范围0-1000
                            cd_ej04.VCU_CruisingRange = cd_ej04.VCU_CruisingRange < 0 ? 0 : cd_ej04.VCU_CruisingRange;
                            break;
                        // string VCU_BrakePedalSt;//制动开关状态
                        case "00 E4":
                            str_data1 = str_all.Substring(6, 1);
                            data1 = Convert.ToInt32(str_data1, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            str_data1 = str_data1.Substring(str_data1.Length - 1, 1);
                            if (str_data1 == "0")
                                cd_ej04.VCU_BrakePedalSt = "0";
                            else
                                cd_ej04.VCU_BrakePedalSt = "1";
                            break;
                        //string TCU_GearPosition;//换挡手柄位置
                        case "00 9A":
                            str_data1 = str_all.Substring(6, 1);
                            data1 = Convert.ToInt32(str_data1, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 4)
                                str_data1 = str_data1.PadLeft(4, '0');
                            str_data1 = str_data1.Substring(0, 3);
                            switch (str_data1)
                            {
                                case "000":
                                    cd_ej04.TCU_GearPosition = "0";
                                    break;
                                case "001":
                                    cd_ej04.TCU_GearPosition = "1";
                                    break;
                                case "010":
                                    cd_ej04.TCU_GearPosition = "2";
                                    break;
                                //case "011":
                                //    cd_ej04.TCU_GearPosition = "预留";
                                //    break;
                                //case "100":
                                //    cd_ej04.TCU_GearPosition = "预留";
                                //    break;
                                //case "101":
                                //    cd_ej04.TCU_GearPosition = "预留";
                                //    break;
                                case "110":
                                    cd_ej04.TCU_GearPosition = "6";
                                    break;
                                default:
                                    break;
                            }
                            //double Motor_DCVolt;//直流母线电压
                            //double Motor_DCCurrent;//直流母线电流
                            break;
                        case "01 12":
                            str_data1 = str_all.Substring(6, 4);
                            str_data1 = regex.Replace(str_data1, "");
                            data1 = Convert.ToInt32(str_data1, 16);
                            str_data = Convert.ToString(data1, 2);
                            if (str_data.Length < 12)
                                str_data = str_data.PadLeft(12, '0');
                            str_data = str_data.Substring(0, 10);
                            cd_ej04.Motor_DCVolt = Convert.ToInt32(str_data, 2);
                            //TODO Filter 范围0-600
                            cd_ej04.Motor_DCVolt = cd_ej04.Motor_DCVolt < 0 ? 0 : cd_ej04.Motor_DCVolt;

                            str_data3 = str_all.Substring(9, 5);
                            str_data1 = regex.Replace(str_data3, "");
                            data2 = Convert.ToInt32(str_data1, 16);
                            str_data = Convert.ToString(data2, 2);
                            if (str_data.Length < 16)
                                str_data = str_data.PadLeft(16, '0');
                            str_data = str_data.Substring(3, 11);
                            cd_ej04.Motor_DCCurrent = Convert.ToInt32(str_data, 2) - 800;
                            break;
                        //double Motor_Revolution;//电机转速
                        //double Motor_OutputTorque;//电机反馈扭矩
                        //string Motor_State = "";//电机状态//=============增加
                        //double Motor_AllowMaxTorque = 0;//电机允许最大扭矩===============增加
                        case "00 C0":
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
                                    cd_ej04.Motor_State = "0";
                                    break;
                                case "001":
                                    cd_ej04.Motor_State = "1";
                                    break;
                                case "010":
                                    cd_ej04.Motor_State = "2";
                                    break;
                                case "011":
                                    cd_ej04.Motor_State = "3";
                                    break;
                                case "100":
                                    cd_ej04.Motor_State = "4";
                                    break;
                                //case "101":
                                //    cd_ej04.Motor_State = "预留";
                                //    break;
                                //case "110":
                                //    cd_ej04.Motor_State = "预留";
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
                            cd_ej04.Motor_Revolution = Convert.ToInt32(str_data1, 2) * 0.1 - 12000;
                            //TODO Filter 范围-12000-12000
                            cd_ej04.Motor_Revolution = Math.Round(cd_ej04.Motor_Revolution, 0);

                            //电机反馈扭矩
                            str_data2 = str_all.Substring(15, 5);
                            str_data2 = regex.Replace(str_data2, "");
                            data2 = Convert.ToInt32(str_data2, 16);
                            str_data2 = Convert.ToString(data2, 2);
                            if (str_data2.Length < 16)
                                str_data2 = str_data2.PadLeft(16, '0');
                            str_data2 = str_data2.Substring(2, 14);
                            cd_ej04.Motor_OutputTorque = Math.Round(Convert.ToInt32(str_data2, 2) * 0.1 - 300, 2);

                            //电机允许最大扭矩
                            str_data4 = str_all.Substring(21, 5);
                            str_data4 = regex.Replace(str_data4, "");
                            str_data4 = Convert.ToString(Convert.ToInt32(str_data4, 16), 2);
                            if (str_data4.Length < 16)
                                str_data4 = str_data4.PadLeft(16, '0');
                            str_data4 = str_data4.Substring(2, 14);
                            cd_ej04.Motor_AllowMaxTorque = Math.Round(Convert.ToInt32(str_data4, 2) * 0.1 - 300, 2);
                            break;

                        //string Motor_Fault;//电机故障码
                        //double Motor_Temperature;//电机本体温度
                        //double Motor_ControllerTemp;//电机控制器温度
                        //double Motor_OutputPower;//电机功率
                        //case "03 60":
                        case "03 60":
                            str_data1 = str_all.Substring(6, 2);
                            switch (str_data1)
                            {
                                case "00":
                                    cd_ej04.Motor_Fault =0;
                                    faultlever = 0;
                                    break;
                                case "41":
                                    cd_ej04.Motor_Fault =65;
                                    faultlever = 4;
                                    break;
                                case "81":
                                    cd_ej04.Motor_Fault =129;
                                    faultlever = 3;
                                    break;
                                case "82":
                                    cd_ej04.Motor_Fault =130;
                                    faultlever = 3;
                                    break;
                                case "83":
                                    cd_ej04.Motor_Fault =131;
                                    faultlever = 3;
                                    break;
                                case "84":
                                    cd_ej04.Motor_Fault =132;
                                    faultlever = 3;
                                    break;
                                case "85":
                                    cd_ej04.Motor_Fault =133;
                                    faultlever = 3;
                                    break;
                                case "86":
                                    cd_ej04.Motor_Fault =134;
                                    faultlever = 3;
                                    break;
                                case "87":
                                    cd_ej04.Motor_Fault =135;
                                    faultlever = 3;
                                    break;
                                case "C1":
                                    cd_ej04.Motor_Fault =193;
                                    faultlever = 2;
                                    break;
                                case "C2":
                                    cd_ej04.Motor_Fault =194;
                                    faultlever = 2;
                                    break;
                                case "C3":
                                    cd_ej04.Motor_Fault =195;
                                    faultlever = 2;
                                    break;
                                case "C4":
                                    cd_ej04.Motor_Fault =196;
                                    faultlever = 2;
                                    break;
                                case "C5":
                                    cd_ej04.Motor_Fault =197;
                                    faultlever = 2;
                                    break;
                                case "C6":
                                    cd_ej04.Motor_Fault =198;
                                    faultlever = 3;
                                    break;
                                case "C7":
                                    cd_ej04.Motor_Fault =199;
                                    faultlever = 3;
                                    break;
                                default:
                                    cd_ej04.Motor_Fault =0;
                                    faultlever = 0;
                                    break;
                            }
                            faultint = cd_ej04.Motor_Fault;
                        if (faultint > 0)
                            canfault += string.Format("${0}:{1}:{2}:{3}", "Motor_Fault", "0X360-0X" + str_data1, faultlever, faultint);

                            //电机本体温度
                            str_data2 = str_all.Substring(9, 2);
                            cd_ej04.Motor_Temperature = Convert.ToInt32(str_data2, 16) - 40;
                            //电机控制器温度
                            str_data3 = str_all.Substring(15, 2);
                            cd_ej04.Motor_ControllerTemp = Convert.ToInt32(str_data3, 16) - 40;
                            //电机功率
                            str_data4 = str_all.Substring(19, 4);
                            str_data4 = regex.Replace(str_data4, "");
                            data1 = Convert.ToInt32(str_data4, 16);
                            str_data4 = Convert.ToString(data1, 2);
                            if (str_data4.Length < 12)
                                str_data4 = str_data4.PadLeft(12, '0');
                            str_data4 = str_data4.Substring(1, 11);
                            cd_ej04.Motor_OutputPower = Math.Round(Convert.ToInt32(str_data4, 2) * 0.1 - 100, 3);
                            break;
                        //double BMS_SOC;//电池SOC
                        //double BMS_Voltage;//电池总电压
                        //double BMS_Current;//电池充放电总电流
                        case "01 25":
                            //电池SOC
                            str_data1 = str_all.Substring(6, 2);
                            cd_ej04.BMS_SOC = Convert.ToInt32(str_data1, 16);
                            //TODO Filter 范围0-100
                            cd_ej04.BMS_SOC = cd_ej04.BMS_SOC < 0 ? 0 : cd_ej04.BMS_SOC;

                            //电池总电压
                            str_data2 = str_all.Substring(15, 4);
                            str_data2 = regex.Replace(str_data2, "");
                            data2 = Convert.ToInt32(str_data2, 16);
                            str_data2 = Convert.ToString(data2, 2);
                            if (str_data2.Length < 12)
                                str_data2 = str_data2.PadLeft(12, '0');
                            str_data2 = str_data2.Substring(0, 9);
                            cd_ej04.BMS_Voltage = Convert.ToInt32(str_data2, 2);
                            //TODO Filter 范围0-500
                            cd_ej04.BMS_Voltage = cd_ej04.BMS_Voltage < 0 ? 0 : cd_ej04.BMS_Voltage;

                            //电池充放电总电流
                            str_data3 = str_all.Substring(19, 4);
                            str_data3 = regex.Replace(str_data3, "");
                            cd_ej04.BMS_Current = Convert.ToInt32(str_data3, 16) - 600;
                            break;

                        //漏电检测//BMS_CreepageMonitor============增加
                        case "01 27":
                            str_data = str_all.Substring(25, 4);
                            str_data = regex.Replace(str_data, "");
                            cd_ej04.BMS_CreepageMonitor = Convert.ToInt32(str_data, 16);
                            //TODO Filter 范围0-2000
                            cd_ej04.BMS_CreepageMonitor = cd_ej04.BMS_CreepageMonitor < 0 ? 0 : cd_ej04.BMS_CreepageMonitor;

                            break;

                        //double BMS_SOCCalculate;//电池SOC(处理后)
                        //string BMS_OutsideChargeSignal;//外部充电信号(动力电池充电指示)
                        //string BMS_FaultDislpay;//电池故障显示用
                        //int 信号 BMS_OFCConnectSignal 非车载充电连接指示信号
                        //double BMS_Temperature;//电池温度
                        //double BMS_Temp_Max;//电池最高温度
                        //double BMS_Temp_Min;//电池最低问题
                        case "03 58":
                            str_data1 = str_all.Substring(6, 2);
                            cd_ej04.BMS_SOCCalculate = Convert.ToInt32(str_data1, 16);
                            //TODO Filter 范围0-100
                            cd_ej04.BMS_SOCCalculate = cd_ej04.BMS_SOCCalculate < 0 ? 0 : cd_ej04.BMS_SOCCalculate;

                            str_data2 = str_all.Substring(9, 1);
                            data2 = Convert.ToInt32(str_data2, 16);
                            str_data2 = Convert.ToString(data2, 2);
                            if (str_data2.Length < 4)
                                str_data2 = str_data2.PadLeft(4, '0');
                            str_data3 = str_data2.Substring(0, 1);
                            if (str_data3 == "0")
                                cd_ej04.BMS_OutsideChargeSignal = "0";
                            else
                                cd_ej04.BMS_OutsideChargeSignal = "1";
                            //电池故障
                            str_data4 = str_data2.Substring(1, 1);
                            if (str_data4 == "0")
                                cd_ej04.BMS_FaultDislpay = "0";
                            else
                                cd_ej04.BMS_FaultDislpay = "1";
                            //非车载充电连接指示信号
                            str_data4 = str_data2.Substring(2, 1);
                            if (str_data4 == "0")
                                cd_ej04.BMS_OFCConnectSignal = "0";
                            else
                                cd_ej04.BMS_OFCConnectSignal = "1";
                            //电池温度
                            str_data1 = str_all.Substring(18, 2);
                            cd_ej04.BMS_Temperature = Convert.ToInt32(str_data1, 16) - 40;
                            //电池最高温度
                            str_data2 = str_all.Substring(24, 2);
                            cd_ej04.BMS_Temp_Max = Convert.ToInt32(str_data2, 16) - 40;
                            //电池最低温度
                            str_data3 = str_all.Substring(27, 2);
                            cd_ej04.BMS_Temp_Min = Convert.ToInt32(str_data3, 16) - 40;
                            break;

                        //string BMS_Fault = "";//BMS故障码//============增加
                        //double BMS_MaxCellBatt = 0;//最高单体电压//============增加
                        //double BMS_MinCellBatt = 0;//最低单体电压//============增加
                        //int BMS_MaxCellBattNumber = 0;//最高单体电压电池编号//============增加
                        //int BMS_MinCellBattNumber = 0;//最低单体电压电池编号//============增加
                        //case "03 59":

                        case "03 59":
                            //BMS故障码
                            str_data = str_all.Substring(6, 2);
                            #region switch
                            //BMK 2035-10-21 根据 2035-10-20 才总发送的邮件 <<【东电】安全预警需求>> 添加  52,51,50,4f,4e,4b,4A,48,47,46,21
                            switch (str_data)
                            {
                                case "00":
                                    cd_ej04.BMS_Fault = 0;
                                    faultlever = 0;
                                    break;
                                case "21":
                                    cd_ej04.BMS_Fault = 33;
                                    faultlever = 1;
                                    break;
                                //case "41":
                                //    cd_ej04.BMS_Fault = "一般故障：系统过压预警";
                                //    break;
                                //case "42":
                                //    cd_ej04.BMS_Fault = "一般故障：单体过压预警";
                                //    break;
                                //case "43":
                                //    cd_ej04.BMS_Fault = "一般故障：系统欠压预警";
                                //    break;
                                //case "44":
                                //    cd_ej04.BMS_Fault = "一般故障：单体欠压预警";
                                //    break;
                                //case "45":
                                //    cd_ej04.BMS_Fault = "一般故障：慢充连接异常";
                                //    break;
                                case "46":
                                    cd_ej04.BMS_Fault = 70;
                                    faultlever = 2;
                                    break;
                                case "47":
                                    cd_ej04.BMS_Fault = 71;
                                    faultlever = 2;
                                    break;
                                case "48":
                                    cd_ej04.BMS_Fault = 72;
                                    faultlever = 2;
                                    break;
                                //case "49":
                                //    cd_ej04.BMS_Fault = "一般故障：SOC低警告";
                                //    break;
                                case "4A":
                                    cd_ej04.BMS_Fault = 74;
                                    faultlever = 2;
                                    break;
                                case "4B":
                                    cd_ej04.BMS_Fault = 75;
                                    faultlever = 2;
                                    break;
                                case "4C":
                                    cd_ej04.BMS_Fault = 76;
                                    faultlever = 2;
                                    break;
                                case "4D":
                                    cd_ej04.BMS_Fault = 77;
                                    faultlever = 2;
                                    break;
                                case "4E":
                                    cd_ej04.BMS_Fault = 78;
                                    faultlever = 2;
                                    break;
                                case "4F":
                                    cd_ej04.BMS_Fault = 79;
                                    faultlever = 2;
                                    break;
                                case "50":
                                    cd_ej04.BMS_Fault = 80;
                                    faultlever = 2;
                                    break;
                                case "51":
                                    cd_ej04.BMS_Fault = 81;
                                    faultlever = 2;
                                    break;
                                case "52":
                                    cd_ej04.BMS_Fault = 82;
                                    faultlever = 2;
                                    break;
                                case "54":
                                    cd_ej04.BMS_Fault = 84;
                                    faultlever = 2;
                                    break;
                                case "56":
                                    cd_ej04.BMS_Fault = 86;
                                    faultlever = 2;
                                    break;
                                case "57":
                                    cd_ej04.BMS_Fault = 87;
                                    faultlever = 2;
                                    break;
                                case "58":
                                    cd_ej04.BMS_Fault = 88;
                                    faultlever = 2;
                                    break;
                                case "59":
                                    cd_ej04.BMS_Fault = 89;
                                    faultlever = 2;
                                    break;
                                case "5A":
                                    cd_ej04.BMS_Fault = 90;
                                    faultlever = 2;
                                    break;
                                case "5B":
                                    cd_ej04.BMS_Fault = 91;
                                    faultlever = 2;
                                    break;
                                case "5C":
                                    cd_ej04.BMS_Fault = 92;
                                    faultlever = 2;
                                    break;
                                case "5D":
                                    cd_ej04.BMS_Fault = 93;
                                    faultlever = 2;
                                    break;
                                //case "61":
                                //    cd_ej04.BMS_Fault = "严重故障：充电电流大报警";
                                //    break;
                                //case "62":
                                //    cd_ej04.BMS_Fault = "严重故障：系统过压报警";
                                //    break;
                                case "63":
                                    cd_ej04.BMS_Fault = 99;
                                    faultlever = 3;
                                    break;
                                case "64":
                                    cd_ej04.BMS_Fault = 100;
                                    faultlever = 3;
                                    break;
                                case "65":
                                    cd_ej04.BMS_Fault = 101;
                                    faultlever = 3;
                                    break;
                                case "66":
                                    cd_ej04.BMS_Fault = 102;
                                    faultlever = 3;
                                    break;
                                case "67":
                                    cd_ej04.BMS_Fault = 103;
                                    faultlever = 3;
                                    break;
                                //case "68":
                                //    cd_ej04.BMS_Fault = "严重故障：充电器通讯故障";
                                //    break;
                                case "69":
                                    cd_ej04.BMS_Fault = 105;
                                    faultlever = 3;
                                    break;
                                case "6A":
                                    cd_ej04.BMS_Fault = 106;
                                    faultlever = 3;
                                    break;
                                case "6B":
                                    cd_ej04.BMS_Fault = 107;
                                    faultlever = 3;
                                    break;
                                case "6C":
                                    cd_ej04.BMS_Fault = 108;
                                    faultlever = 3;
                                    break;
                                case "6D":
                                    cd_ej04.BMS_Fault = 109;
                                    faultlever = 3;
                                    break;
                                case "70":
                                    cd_ej04.BMS_Fault = 112;
                                    faultlever = 3;
                                    break;
                                case "71":
                                    cd_ej04.BMS_Fault = 113;
                                    faultlever = 3;
                                    break;
                                case "72":
                                    cd_ej04.BMS_Fault = 114;
                                    faultlever = 3;
                                    break;
                                case "73":
                                    cd_ej04.BMS_Fault = 115;
                                    faultlever = 3;
                                    break;
                                case "74":
                                    cd_ej04.BMS_Fault = 116;
                                    faultlever = 3;
                                    break;
                                case "75":
                                    cd_ej04.BMS_Fault = 117;
                                    faultlever = 3;
                                    break;
                                case "76":
                                    cd_ej04.BMS_Fault = 118;
                                    faultlever = 3;
                                    break;
                                case "81":
                                    cd_ej04.BMS_Fault = 129;
                                    faultlever = 4;
                                    break;
                                case "82":
                                    cd_ej04.BMS_Fault = 130;
                                    faultlever = 4;
                                    break;
                                default:
                                    cd_ej04.BMS_Fault = 0;
                                    faultlever = 0;
                                    break;
                            }

                            #endregion
                            if (faultlever>0)
                                canfault += "$BMS_Fault:" + "0X359-0X" + str_data + ":" + faultlever + ":" + cd_ej04.BMS_Fault;

                            //最高单体电压
                            str_data1 = str_all.Substring(9, 2);
                            cd_ej04.BMS_MaxCellBatt = Convert.ToInt32(str_data1, 16) / 64;
                            //最高单体电压
                            str_data2 = str_all.Substring(12, 2);
                            cd_ej04.BMS_MinCellBatt = Convert.ToInt32(str_data2, 16) / 64;
                            //最高单体电压电池编号
                            str_data3 = str_all.Substring(15, 2);
                            cd_ej04.BMS_MaxCellBattNumber = Convert.ToInt32(str_data3, 16);
                            //最高单体电压电池编号
                            str_data4 = str_all.Substring(18, 2);
                            cd_ej04.BMS_MinCellBattNumber = Convert.ToInt32(str_data4, 16);
                            break;

                        //string BMS_ChargeSt;//充电状态
                        //string BMS_ChargerACInput 交流输入状态
                        case "03 61":
                            str_data1 = str_all.Substring(15, 1);
                            data1 = Convert.ToInt32(str_data1, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 4)
                                str_data1 = str_data1.PadLeft(4, '0');
                            str_data2 = str_data1.Substring(1, 2);
                            switch (str_data2)
                            {
                                case "00":
                                    cd_ej04.BMS_ChargeSt = "0";
                                    break;
                                case "01":
                                    cd_ej04.BMS_ChargeSt = "1";
                                    break;
                                case "10":
                                    cd_ej04.BMS_ChargeSt = "2";
                                    break;
                                case "11":
                                    cd_ej04.BMS_ChargeSt = "3";
                                    break;
                                default:
                                    break;
                            }

                            //交流输入状态
                            str_data1 = str_data1.Substring(3, 1);
                            switch (str_data1)
                            {
                                case "0":
                                    cd_ej04.BMS_ChargerACInput = "0";
                                    break;
                                case "1":
                                    cd_ej04.BMS_ChargerACInput = "1";
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
                        case "03 A2":
                            str_data = str_all.Substring(6, 23);
                            if (str_data == "00 00 00 00 00 00 00 00")
                            {
                                cd_ej04.ONC_OutputVoltage = 0;
                                cd_ej04.ONC_OutputCurrent = 0;
                                cd_ej04.ONC_ONCTemp = 0;
                                break;
                            }
                            else
                            {
                                //充电机输出的充电电压
                                str_data1 = str_all.Substring(6, 5);
                                str_data1 = regex.Replace(str_data1, "");
                                cd_ej04.ONC_OutputVoltage = Convert.ToInt32(str_data1, 16) * 0.1 - 3276.7;
                                //TODO Filter 范围-3276.7-3276.7
                                cd_ej04.ONC_OutputVoltage = Math.Round(cd_ej04.ONC_OutputVoltage, 1);
                                //充电机输出的充电电流
                                str_data2 = str_all.Substring(12, 2);
                                cd_ej04.ONC_OutputCurrent = Convert.ToInt32(str_data2, 16) * 0.1;
                                //TODO Filter 范围0-25.5
                                cd_ej04.ONC_OutputCurrent = cd_ej04.ONC_OutputCurrent < 0 ? 0 : cd_ej04.ONC_OutputCurrent;
                                cd_ej04.ONC_OutputCurrent = Math.Round(cd_ej04.ONC_OutputCurrent, 1);

                                //充电机温度
                                str_data3 = str_all.Substring(18, 2);
                                cd_ej04.ONC_ONCTemp = Convert.ToInt32(str_data3, 16) - 100;
                                //输入电压状态
                                str_data2 = str_all.Substring(15, 1);
                                str_data2 = Convert.ToString(Convert.ToInt32(str_data2, 16), 2);
                                if (str_data2.Length < 4)
                                    str_data2 = str_data2.PadLeft(4, '0');
                                str_data2 = str_data2.Substring(2, 1);
                                if (str_data2 == "0")
                                    cd_ej04.ONC_InputVoltageSt = "0";
                                else
                                    cd_ej04.ONC_InputVoltageSt = "1";
                                //通信状态
                                str_data3 = str_all.Substring(16, 1);
                                str_data3 = Convert.ToString(Convert.ToInt32(str_data3, 16), 2);
                                if (str_data3.Length < 4)
                                    str_data3 = str_data3.PadLeft(4, '0');
                                str_data3 = str_data3.Substring(0, 1);
                                if (str_data3 == "0")
                                    cd_ej04.ONC_CommunicationSt = "0";
                                else
                                    cd_ej04.ONC_CommunicationSt = "1";

                                //充电机故障码
                                cd_ej04.ONC_Fault = "";
                                str_data4 = str_all.Substring(24, 4);
                                str_data4 = regex.Replace(str_data4, "");
                                str_data4 = Convert.ToString(Convert.ToInt32(str_data4, 16), 2);
                                if (str_data4.Length < 12)
                                    str_data4 = str_data4.PadLeft(12, '0');
                                for (int j = 0; j < str_data4.Length; j++)
                                {
                                    if (str_data4[j] == '1')
                                        //cd_ej04.ONC_Fault += oncfault[j] + ",";
                                        cd_ej04.ONC_Fault += j + ",";
                                }
                                if (!string.IsNullOrEmpty(cd_ej04.ONC_Fault))
                                {
                                    cd_ej04.ONC_Fault = cd_ej04.ONC_Fault.Substring(0, cd_ej04.ONC_Fault.Length - 1);
                                    foreach (var item in cd_ej04.ONC_Fault.Split(','))
                                    {
                                        if (!string.IsNullOrEmpty(item))
                                        {
                                            canfault += "$ONC_Fault:0X3A2-bit" + (11 - int.Parse(item)) + ":2:" + item;
                                        }
                                    }
                                }
                                else
                                {
                                    cd_ej04.ONC_Fault = "0";
                                }
                                break;
                            }

                        //double IC_TotalOdmeter;//总里程
                        //double IC_Odmeter;//小计里程
                        case "03 20":
                            //总里程
                            str_data1 = str_all.Substring(6, 8);
                            str_data1 = regex.Replace(str_data1, "");
                            cd_ej04.IC_TotalOdmeter = Convert.ToInt32(str_data1, 16) * 0.1;
                            //TODO Filter 范围0-999999.9
                            cd_ej04.IC_TotalOdmeter = cd_ej04.IC_TotalOdmeter < 0 ? 0 : cd_ej04.IC_TotalOdmeter;
                            cd_ej04.IC_TotalOdmeter = Math.Round(cd_ej04.IC_TotalOdmeter, 1);

                            //小计里程
                            str_data2 = str_all.Substring(15, 5);
                            str_data2 = regex.Replace(str_data2, "");
                            cd_ej04.IC_Odmeter = Convert.ToInt32(str_data2, 16) * 0.1;
                            //TODO Filter 范围0-999.9
                            cd_ej04.IC_Odmeter = cd_ej04.IC_Odmeter < 0 ? 0 : cd_ej04.IC_Odmeter;//小于0 取0
                            cd_ej04.IC_Odmeter = Math.Round(cd_ej04.IC_Odmeter, 1);//取一位小数

                            break;
                        //string ABS_BrakeSignal;//制动信号
                        //double ABS_VehSpd;//车速信号
                        case "00 A0":
                            str_data1 = str_all.Substring(13, 1);
                            data1 = Convert.ToInt32(str_data1, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 4)
                                str_data1 = str_data1.PadLeft(4, '0');
                            str_data1 = str_data1.Substring(1, 1);
                            //车速信号
                            if (str_data1 == "0")
                                cd_ej04.ABS_BrakeSignal = "0";
                            else
                                cd_ej04.ABS_BrakeSignal = "1";
                            str_data2 = str_all.Substring(24, 5);
                            str_data2 = regex.Replace(str_data2, "");
                            cd_ej04.ABS_VehSpd = Convert.ToInt32(str_data2, 16) * 0.01;
                            //TODO Filter 范围0-300
                            cd_ej04.ABS_VehSpd = cd_ej04.ABS_VehSpd < 0 ? 0 : cd_ej04.ABS_VehSpd;//小于0 取0
                            cd_ej04.ABS_VehSpd = Math.Round(cd_ej04.ABS_VehSpd, 0);//取小数
                            break;
                        //double DCDC_Temperature;//DCDC温度
                        //double DCDC_OutputVoltage;//DCDC输出电压
                        //double DCDC_OutputCurrent;//DCDC输出电流
                        //string DCDC_EnableResponse;//DCDC使能应答
                        //string DCDC_Fault;//DCDC故障
                        //double DCDC_InputVoltage;//DCDC输入电压
                        //double DCDC_InputCurrent;//DCDC输入电流
                        case "03 45"://===============================修改
                            str_data1 = str_all.Substring(6, 2);
                            cd_ej04.DCDC_Temperature = Convert.ToInt32(str_data1, 16) - 60;
                            str_data2 = str_all.Substring(9, 2);
                            cd_ej04.DCDC_OutputVoltage = Convert.ToInt32(str_data2, 16) * 0.125;
                            //DC-DC输出电流
                            str_data3 = str_all.Substring(12, 4);
                            str_data3 = regex.Replace(str_data3, "");
                            cd_ej04.DCDC_OutputCurrent = Convert.ToInt32(str_data3, 16) * 0.125;
                            //DC-DC使能应答
                            str_data4 = str_all.Substring(16, 1);
                            data2 = Convert.ToInt32(str_data4, 16);
                            str_data4 = Convert.ToString(data2, 2);
                            if (str_data4.Length < 4)
                                str_data4 = str_data4.PadLeft(4, '0');
                            str_data4 = str_data4.Substring(0, 1);
                            if (str_data4 == "0")
                                cd_ej04.DCDC_EnableResponse = "0";
                            else
                                cd_ej04.DCDC_EnableResponse = "1";
                            //DC-DC故障
                            cd_ej04.DCDC_Fault = "";
                            str_data = str_all.Substring(18, 2);
                            WriteLog.WriteOrdersLog("str_data:" + str_data);
                            data1 = Convert.ToInt32(str_data, 16);
                            str_data = Convert.ToString(data1, 2);
                            if (str_data.Length < 8)
                                str_data = str_data.PadLeft(8, '0');
                            for (int j = 0; j < str_data.Length; j++)
                            {
                                if (str_data[j] != '0')
                                    //  cd_ej04.DCDC_Fault += dcdcfault[j] + ",";
                                    cd_ej04.DCDC_Fault += j + ",";
                            }
                            if (!string.IsNullOrEmpty(cd_ej04.DCDC_Fault))
                            {
                                cd_ej04.DCDC_Fault = cd_ej04.DCDC_Fault.Substring(0, cd_ej04.DCDC_Fault.Length - 1);
                                foreach (var item in cd_ej04.DCDC_Fault.Split(','))
                                {
                                    if (!string.IsNullOrEmpty(item))
                                    {
                                        canfault += "$DCDC_Fault::2:" + item;
                                    }
                                }
                            }
                            else
                            {
                                cd_ej04.DCDC_Fault = "-1";
                            }

                            WriteLog.WriteOrdersLog("str_data:" + cd_ej04.DCDC_Fault + "----->");
                            //DC-DC输入电压
                            str_data1 = str_all.Substring(21, 5);
                            str_data1 = regex.Replace(str_data1, "");
                            cd_ej04.DCDC_InputVoltage = Convert.ToInt32(str_data1, 16) * 0.01563;
                            //TODO Filter 范围0-500
                            cd_ej04.DCDC_InputVoltage = cd_ej04.DCDC_InputVoltage < 0 ? 0 : cd_ej04.DCDC_InputVoltage;//小于0 取0
                            cd_ej04.DCDC_InputVoltage = Math.Round(cd_ej04.DCDC_InputVoltage, 0);//取小数

                            //DC-DC输入电流
                            str_data2 = str_all.Substring(27, 2);
                            cd_ej04.DCDC_InputCurrent = Convert.ToInt32(str_data2, 16) * 0.125;
                            //TODO Filter 范围0-25
                            cd_ej04.DCDC_InputCurrent = Math.Round(cd_ej04.DCDC_InputCurrent, 0);

                            break;
                        default:
                            break;
                    }
                }
                #endregion

                if (!string.IsNullOrEmpty(cd_ej04.BMS_ChargeSt))
                {
                    //快慢充
                    if (cd_ej04.BMS_ChargeSt.Equals("1"))
                    {
                        if (cd_ej04.BMS_OutsideChargeSignal.Equals("1"))
                        {
                            //慢充
                            cd_ej04.BMS_ChargeFS = 2;
                        }
                        if (cd_ej04.BMS_OFCConnectSignal.Equals("1"))
                        {
                            //快充
                            cd_ej04.BMS_ChargeFS = 1;
                        }
                    }
                    else
                    {
                        //非充电
                        cd_ej04.BMS_ChargeFS = 0;
                    }
                }

                WriteLog.WriteLogMeaning("Ej04", "", cd_ej04);

                //sqlstr = string.Format("update ev_{0} set Curent={1},Voltage={2},SOC={3},MotorRevolution={4},MotorTemperature={5},OutputPower={6},ABS_VehSpd={7},Distance={8},BatteryTemp_Ave={9},BatteryTemp_Max={10},BatteryTemp_Min={11} where time='{12}'", sysno, cd_ej04.Curent, cd_ej04.Voltage, cd_ej04.SOC, cd_ej04.MotorRevolution, cd_ej04.MotorTemperature, cd_ej04.OutputPower, cd_ej04.ABS_VehSpd, cd_ej04.Distance, cd_ej04.BatteryTemp_Ave, cd_ej04.BatteryTemp_Max, cd_ej04.BatteryTemp_Min, ltime);
                //alarm = "{\"index\":\"1\",\"Sysno\":\"" + sysno + "\",\"time\":\"" + ltime + "\",\"Curent\":\"" + cd_ej04.Curent + "\",\"Voltage\":\"" + cd_ej04.Voltage + "\",\"SOC\":\"" + cd_ej04.SOC + "\",\"MotorRevolution\":\"" + cd_ej04.MotorRevolution + "\",\"MotorTemperature\":\"" + cd_ej04.MotorTemperature + "\",\"OutputPower\":\"" + cd_ej04.OutputPower + "\",\"ABS_VehSpd\":\"" + cd_ej04.ABS_VehSpd + "\",\"Distance\":\"" + cd_ej04.Distance + "\",\"BatteryTemp_Ave\":\"" + cd_ej04.BatteryTemp_Ave + "\",\"BatteryTemp_Max\":\"" + cd_ej04.BatteryTemp_Max + "\",\"BatteryTemp_Min\":\"" + cd_ej04.BatteryTemp_Min + "\"}";
                //data = "";
            }
            else if (index == 4)
            {
                #region A60
                Candata_A60 cd_a60 = cd as Candata_A60;
                str_time = infos[1].Substring(30, 17);
                cantime = PublicMethods.AnalysisTime(str_time).ToString("yyyy-MM-dd HH:mm:ss");
                for (int i = 1; i < infos.Length; i++)
                {
                    string bs = infos[i].Substring(0, 5);

                    string str_all = infos[i].ToString();

                    switch (bs)
                    {
                        //VCU点火开关及故障码
                        //string VCU_Keyposition;//点火开关
                        //string VCU_Fault;//故障码
                        case "00 F0":
                            str_data1 = str_all.Substring(6, 1);
                            data1 = Convert.ToInt32(str_data1, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 4)
                                str_data1 = str_data1.PadLeft(4, '0');
                            str_data1 = str_data1.Substring(0, 2);
                            switch (str_data1)
                            {
                                case "00":
                                    cd_a60.VCU_Keyposition = "0";
                                    // session.isacc = "0";
                                    break;
                                case "01":
                                    cd_a60.VCU_Keyposition = "1";
                                    //session.isacc = "1";
                                    break;
                                case "10":
                                    cd_a60.VCU_Keyposition = "2";
                                    //session.isacc = "1";
                                    break;
                                case "11":
                                    cd_a60.VCU_Keyposition = "3";
                                    //session.isacc = "1";
                                    break;
                                default:
                                    break;
                            }


                            str_data2 = str_all.Substring(21, 2);
                            data1 = Convert.ToInt32(str_data2, 16);
                            str_data2 = Convert.ToString(data1, 2);
                            if (str_data2.Length < 8)
                                str_data2 = str_data2.PadLeft(8, '0');
                            str_data1 = str_data2.Substring(0, 3);
                            switch (str_data1)
                            {
                                case "000":
                                    cd_a60.VCU_Fault = 0;
                                    faultlever = 0;
                                    break;
                                case "001":
                                    cd_a60.VCU_Fault = 1;
                                    faultlever = 1;
                                    break;
                                case "010":
                                    cd_a60.VCU_Fault = 2;
                                    faultlever = 2;
                                    break;
                                case "011":
                                    cd_a60.VCU_Fault = 3;
                                    faultlever = 3;
                                    break;
                                case "100":
                                    cd_a60.VCU_Fault = 4;
                                    faultlever = 4;
                                    break;
                                //case "101":
                                //    cd_a60.VCU_Fault = "预留";
                                //    break;
                                //case "110":
                                //    cd_a60.VCU_Fault = "预留";
                                //    break;
                                //case "111":
                                //    cd_a60.VCU_Fault = "预留";
                                //    break;
                                default:
                                    break;
                            }
                            faultint = cd_a60.VCU_Fault;
                        if (faultint > 0)
                            canfault += "$VCU_Fault:" + "0X" + str_all.Substring(21, 2) + ":" + faultlever + ":" + faultint;
                            break;
                        //double BMS_SOC;//电池SOC
                        //double BMS_Voltage;//电池总电压
                        //double BMS_Current;//电池充放电总电流
                        case "01 25":
                            //电池SOC
                            str_data2 = str_all.Substring(6, 4);
                            str_data1 = regex.Replace(str_data2, "");
                            data2 = Convert.ToInt32(str_data1, 16);
                            str_data3 = Convert.ToString(data2, 2);
                            if (str_data3.Length < 12)
                                str_data3 = str_data3.PadLeft(12, '0');
                            str_data = str_data3.Substring(0, 10);
                            cd_a60.BMS_SOC = Convert.ToInt32(str_data, 2) * 0.1;
                            //TODO Filter 范围0-100
                            cd_a60.BMS_SOC = cd_a60.BMS_SOC < 0 ? 0 : cd_a60.BMS_SOC;

                            //电池总电压
                            str_data2 = str_all.Substring(15, 4);
                            str_data2 = regex.Replace(str_data2, "");
                            data2 = Convert.ToInt32(str_data2, 16);
                            str_data2 = Convert.ToString(data2, 2);
                            if (str_data2.Length < 12)
                                str_data2 = str_data2.PadLeft(12, '0');
                            str_data2 = str_data2.Substring(0, 9);
                            cd_a60.BMS_Voltage = Convert.ToInt32(str_data2, 2);
                            //TODO Filter 范围0-500
                            cd_a60.BMS_Voltage = cd_a60.BMS_Voltage < 0 ? 0 : cd_a60.BMS_Voltage;

                            //电池充放电总电流
                            str_data3 = str_all.Substring(19, 4);
                            str_data3 = regex.Replace(str_data3, "");
                            cd_a60.BMS_Current = Convert.ToInt32(str_data3, 16) - 600;
                            break;


                        //漏电检测//BMS_CreepageMonitor============增加
                        case "01 27":
                            str_data = str_all.Substring(25, 4);
                            str_data = regex.Replace(str_data, "");
                            cd_a60.BMS_CreepageMonitor = Convert.ToInt32(str_data, 16);
                            //TODO Filter 范围0-2000
                            cd_a60.BMS_CreepageMonitor = cd_a60.BMS_CreepageMonitor < 0 ? 0 : cd_a60.BMS_CreepageMonitor;

                            break;

                        //double BMS_SOCCalculate;//电池SOC(处理后)
                        //string BMS_OutsideChargeSignal;//外部充电信号(动力电池充电指示)
                        //string BMS_FaultDislpay;//电池故障(显示用)
                        //int 信号 BMS_OFCConnectSignal 非车载充电连接指示信号
                        //double BMS_Temperature;//电池温度
                        //double BMS_Temp_Max;//电池最高温度
                        //double BMS_Temp_Min;//电池最低问题
                        case "03 58":
                            str_data1 = str_all.Substring(6, 2);
                            cd_a60.BMS_SOCCalculate = Convert.ToInt32(str_data1, 16);
                            //TODO Filter 范围0-100
                            cd_a60.BMS_SOCCalculate = cd_a60.BMS_SOCCalculate < 0 ? 0 : cd_a60.BMS_SOCCalculate;

                            str_data2 = str_all.Substring(9, 1);
                            data2 = Convert.ToInt32(str_data2, 16);
                            str_data2 = Convert.ToString(data2, 2);
                            if (str_data2.Length < 4)
                                str_data2 = str_data2.PadLeft(4, '0');
                            //外部充电信号
                            str_data3 = str_data2.Substring(0, 1);
                            if (str_data3 == "0")
                                cd_a60.BMS_OutsideChargeSignal = "0";
                            else
                                cd_a60.BMS_OutsideChargeSignal = "1";
                            //电池故障
                            str_data4 = str_data2.Substring(1, 1);
                            if (str_data4 == "1")
                            {
                                cd_a60.BMS_FaultDislpay = "1";
                                canfault += string.Format("BMS_FaultDislpay:{0}:{1}:", "0X358-0X01", cd_a60.BMS_FaultDislpay);
                            }
                            else
                                cd_a60.BMS_FaultDislpay = "0";
                            //非车载充电连接指示信号
                            str_data4 = str_data2.Substring(2, 1);
                            if (str_data4 == "0")
                                cd_a60.BMS_OFCConnectSignal = "0";
                            else
                                cd_a60.BMS_OFCConnectSignal = "1";
                            //电池温度
                            str_data1 = str_all.Substring(18, 2);
                            cd_a60.BMS_Temperature = Convert.ToInt32(str_data1, 16) - 40;
                            //电池最高温度
                            str_data2 = str_all.Substring(24, 2);
                            cd_a60.BMS_Temp_Max = Convert.ToInt32(str_data2, 16) - 40;
                            //电池最低温度
                            str_data3 = str_all.Substring(27, 2);
                            cd_a60.BMS_Temp_Min = Convert.ToInt32(str_data3, 16) - 40;
                            break;
                        //string BMS_Fault = "";//BMS故障码//============增加
                        //double BMS_MaxCellBatt = 0;//最高单体电压//============增加
                        //double BMS_MinCellBatt = 0;//最低单体电压//============增加
                        //int BMS_MaxCellBattNumber = 0;//最高单体电压电池编号//============增加
                        //int BMS_MinCellBattNumber = 0;//最低单体电压电池编号//============增加
                        //case "03 59":
                        case "03 59":
                            //BMS故障码
                            str_data = str_all.Substring(6, 2);
                            #region switch
                            switch (str_data)
                            {
                                case "00":
                                    cd_a60.BMS_Fault = 0;
                                    faultlever = 0;
                                    break;
                                case "21":
                                    cd_a60.BMS_Fault = 33;
                                    faultlever = 1;
                                    break;
                                //case "41":
                                //    cd_a60.BMS_Fault = "一般故障：系统过压预警";
                                //    break;
                                //case "42":
                                //    cd_a60.BMS_Fault = "一般故障：单体过压预警";
                                //    break;
                                //case "43":
                                //    cd_a60.BMS_Fault = "一般故障：系统欠压预警";
                                //    break;
                                //case "44":
                                //    cd_a60.BMS_Fault = "一般故障：单体欠压预警";
                                //    break;
                                //case "45":
                                //    cd_a60.BMS_Fault = "一般故障：慢充连接异常";
                                //    break;
                                case "46":
                                    cd_a60.BMS_Fault = 70;
                                    faultlever = 2;
                                    break;
                                case "47":
                                    cd_a60.BMS_Fault = 71;
                                    faultlever = 2;
                                    break;
                                case "48":
                                    cd_a60.BMS_Fault = 72;
                                    faultlever = 2;
                                    break;
                                //case "49":
                                //    cd_a60.BMS_Fault = "一般故障：SOC低警告";
                                //    break;
                                case "4A":
                                    cd_a60.BMS_Fault = 74;
                                    faultlever = 2;
                                    break;
                                case "4B":
                                    cd_a60.BMS_Fault = 75;
                                    faultlever = 2;
                                    break;
                                //case "4C":
                                //    cd_a60.BMS_Fault = "一般故障：放电电流大预警";
                                //    break;
                                //case "4D":
                                //    cd_a60.BMS_Fault = "一般故障：充电电流大预警";
                                //    break;
                                case "4E":
                                    cd_a60.BMS_Fault = 78;
                                    faultlever = 2;
                                    break;
                                case "4F":
                                    cd_a60.BMS_Fault = 79;
                                    faultlever = 2;
                                    break;
                                case "50":
                                    cd_a60.BMS_Fault = 80;
                                    faultlever = 2;
                                    break;
                                case "51":
                                    cd_a60.BMS_Fault = 81;
                                    faultlever = 2;
                                    break;
                                case "52":
                                    cd_a60.BMS_Fault = 82;
                                    faultlever = 2;
                                    break;
                                case "54":
                                    cd_a60.BMS_Fault = 84;
                                    faultlever = 2;
                                    break;
                                case "56":
                                    cd_a60.BMS_Fault = 86;
                                    faultlever = 2;
                                    break;
                                case "57":
                                    cd_a60.BMS_Fault = 87;
                                    faultlever = 2;
                                    break;
                                case "58":
                                    cd_a60.BMS_Fault = 88;
                                    faultlever = 2;
                                    break;
                                case "59":
                                    cd_a60.BMS_Fault = 89;
                                    faultlever = 2;
                                    break;
                                case "5A":
                                    cd_a60.BMS_Fault = 90;
                                    faultlever = 2;
                                    break;
                                case "5B":
                                    cd_a60.BMS_Fault = 91;
                                    faultlever = 2;
                                    break;
                                case "5C":
                                    cd_a60.BMS_Fault = 92;
                                    faultlever = 2;
                                    break;
                                case "5D":
                                    cd_a60.BMS_Fault = 93;
                                    faultlever = 2;
                                    break;
                                //case "61":
                                //    cd_a60.BMS_Fault = "严重故障：充电电流大报警";
                                //    break;
                                //case "62":
                                //    cd_a60.BMS_Fault = "严重故障：系统过压报警";
                                //    break;
                                case "63":
                                    cd_a60.BMS_Fault = 99;
                                    faultlever = 3;
                                    break;
                                case "64":
                                    cd_a60.BMS_Fault = 100;
                                    faultlever = 3;
                                    break;
                                case "65":
                                    cd_a60.BMS_Fault = 101;
                                    faultlever = 3;
                                    break;
                                case "66":
                                    cd_a60.BMS_Fault = 102;
                                    faultlever = 3;
                                    break;
                                case "67":
                                    cd_a60.BMS_Fault = 103;
                                    faultlever = 3;
                                    break;
                                //case "68":
                                //    cd_a60.BMS_Fault = "严重故障：充电器通讯故障";
                                //    break;
                                case "69":
                                    cd_a60.BMS_Fault = 105;
                                    faultlever = 3;
                                    break;
                                case "6A":
                                    cd_a60.BMS_Fault = 106;
                                    faultlever = 3;
                                    break;
                                case "6B":
                                    cd_a60.BMS_Fault = 107;
                                    faultlever = 3;
                                    break;
                                case "6C":
                                    cd_a60.BMS_Fault = 108;
                                    faultlever = 3;
                                    break;
                                case "6D":
                                    cd_a60.BMS_Fault = 109;
                                    faultlever = 3;
                                    break;
                                case "70":
                                    cd_a60.BMS_Fault = 112;
                                    faultlever = 3;
                                    break;
                                case "71":
                                    cd_a60.BMS_Fault = 113;
                                    faultlever = 3;
                                    break;
                                case "72":
                                    cd_a60.BMS_Fault = 114;
                                    faultlever = 3;
                                    break;
                                case "73":
                                    cd_a60.BMS_Fault = 115;
                                    faultlever = 3;
                                    break;
                                //case "74":
                                //    cd_a60.BMS_Fault = "严重故障：放电回路高压互锁故障";
                                //    break;
                                //case "75":
                                //    cd_a60.BMS_Fault = "严重故障：充电回路高压互锁故障";
                                //    break;
                                //case "76":
                                //    cd_a60.BMS_Fault = "严重故障：辅助电瓶电压过低警告";
                                //    break;
                                //case "81":
                                //    cd_a60.BMS_Fault = "致命故障：继电器粘连";
                                //    break;
                                //case "82":
                                //    cd_a60.BMS_Fault = "致命故障：碰撞断电";
                                //    break;
                                default:
                                    cd_a60.BMS_Fault = 0;
                                    break;
                            }

                            #endregion
                            faultint = cd_a60.BMS_Fault;
                            if (faultint > 0)
                                canfault += string.Format("${0}:{1}:{2}:{3}", "BMS_Fault", "0X359-0X" + str_data, faultlever, faultint);

                          
                            //最高单体电压
                            str_data1 = str_all.Substring(9, 2);
                            cd_a60.BMS_MaxCellBatt = Convert.ToInt32(str_data1, 16) / 64;
                            //最高单体电压
                            str_data2 = str_all.Substring(12, 2);
                            cd_a60.BMS_MinCellBatt = Convert.ToInt32(str_data2, 16) / 64;
                            //最高单体电压电池编号
                            str_data3 = str_all.Substring(15, 2);
                            cd_a60.BMS_MaxCellBattNumber = Convert.ToInt32(str_data3, 16);
                            //最高单体电压电池编号
                            str_data4 = str_all.Substring(18, 2);
                            cd_a60.BMS_MinCellBattNumber = Convert.ToInt32(str_data4, 16);
                            break;

                        //string BMS_SlowChargeSt;//充电状态(慢充)
                        //string BMS_FastChargeSt;//充电状态(快充)
                        case "03 61":
                            str_data1 = str_all.Substring(15, 1);
                            data1 = Convert.ToInt32(str_data1, 16);
                            str_data1 = Convert.ToString(data1, 2);
                            if (str_data1.Length < 4)
                                str_data1 = str_data1.PadLeft(4, '0');
                            str_data1 = str_data1.Substring(1, 2);
                            switch (str_data1)
                            {
                                case "00":
                                    cd_a60.BMS_SlowChargeSt = "0";
                                    break;
                                case "01":
                                    cd_a60.BMS_SlowChargeSt = "1";
                                    break;
                                case "10":
                                    cd_a60.BMS_SlowChargeSt = "2";
                                    break;
                                case "11":
                                    cd_a60.BMS_SlowChargeSt = "3";
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
                                    cd_a60.BMS_FastChargeSt = "0";
                                    break;
                                case "01":
                                    cd_a60.BMS_FastChargeSt = "1";
                                    break;
                                case "10":
                                    cd_a60.BMS_FastChargeSt = "2";
                                    break;
                                case "11":
                                    cd_a60.BMS_FastChargeSt = "3";
                                    break;
                                default:
                                    break;
                            }
                            break;

                        //string ONC_Fault = "";//充电机故障码============增加
                        case "03 A2":
                            //充电机故障码
                            cd_a60.ONC_Fault = "";
                            str_data4 = str_all.Substring(24, 4);
                            str_data4 = regex.Replace(str_data4, "");
                            str_data4 = Convert.ToString(Convert.ToInt32(str_data4, 16), 2);
                            if (str_data4.Length < 12)
                                str_data4 = str_data4.PadLeft(12, '0');
                            for (int j = 0; j < str_data4.Length; j++)
                            {
                                if (str_data4[j] == '1')
                                    //   cd_a60.ONC_Fault += oncfault[j] + ",";
                                    cd_a60.ONC_Fault += j + ",";
                            }
                            if (!string.IsNullOrEmpty(cd_a60.ONC_Fault))
                            {
                                cd_a60.ONC_Fault = cd_a60.ONC_Fault.Substring(0, cd_a60.ONC_Fault.Length - 1);
                                foreach (var item in cd_a60.ONC_Fault.Split(','))
                                {
                                    if (!string.IsNullOrEmpty(item))
                                    {
                                        canfault += "$ONC_Fault:0X3A2-bit" + (11 - int.Parse(item)) + ":2:" + item;
                                    }
                                }
                            }
                            else
                            {
                                cd_a60.ONC_Fault = "0";
                            }
                            break;

                        //int MCU_ElecPowerTrainMngtState 电机控制器工作状态
                        //double     Motor_Torqueestimation 电机实际输出扭矩
                        //int MCU_ElecMachineFault 电机故障类型
                        //double MCU_InternalMachineTemp 电机转子温度
                        case "02 01":
                            //电机控制器工作状态   取低四位
                            str_data1 = str_all.Substring(13, 1);
                            str_data2 = Convert.ToString(Convert.ToInt32(str_data1, 16), 2);
                            if (str_data2.Length < 4)
                                str_data2 = str_data2.PadLeft(4, '0');
                            str_data1 = str_data2.Substring(1, 3);
                            switch (str_data1)
                            {
                                case "000":
                                    cd_a60.MCU_ElecPowerTrainMngtState = "0";
                                    break;
                                case "010":
                                    cd_a60.MCU_ElecPowerTrainMngtState = "2";
                                    break;
                                case "100":
                                    cd_a60.MCU_ElecPowerTrainMngtState = "4";
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
                            cd_a60.Motor_Torqueestimation = Convert.ToInt32(str_data2, 2);
                            cd_a60.Motor_Torqueestimation = cd_a60.Motor_Torqueestimation < 0 ? 0 : cd_a60.Motor_Torqueestimation;
                            cd_a60.Motor_Torqueestimation = cd_a60.Motor_Torqueestimation - 254;

                            //电机故障类型
                            str_data1 = str_all.Substring(19, 1);
                            str_data2 = Convert.ToString(Convert.ToInt32(str_data1, 16), 2);
                            if (str_data2.Length < 4)
                                str_data2 = str_data2.PadLeft(4, '0');
                            str_data1 = str_data2.Substring(0, 2);
                            switch (str_data1)
                            {
                                case "00":
                                    cd_a60.MCU_ElecMachineFault = "0";
                                    break;
                                case "01":
                                    cd_a60.MCU_ElecMachineFault = "1";
                                    canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_ElecMachineFault", "0X359-0X1", 3, "1");
                                    break;
                                case "10":
                                    cd_a60.MCU_ElecMachineFault = "2";
                                    canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_ElecMachineFault", "0X359-0X2", 4, "2");
                                    break;
                                case "11":
                                    cd_a60.MCU_ElecMachineFault = "3";
                                    canfault += string.Format("${0}:{1}:{2}:{3}", "MCU_ElecMachineFault", "0X359-0X3", 4, "3");
                                    break;
                                default:
                                    break;
                            }
                            //电机转子温度
                            str_data1 = str_all.Substring(21, 2);
                            data1 = Convert.ToInt32(str_data1, 16);
                            //TODO Filter 范围-40-214
                            cd_a60.MCU_InternalMachineTemp = data1 - 40;
                            break;
                        //MCU_MaxMotorTorque 电机最大电动扭矩
                        //   Motor_MaxGenTorque 电机最大发电扭矩
                        //Motor_Revolution 电机转速
                        //MCU_ActiveDischarge 电机控制器母线电容放电状态
                        case "02 02":
                            //电机最大电动扭矩
                            str_data1 = str_all.Substring(6, 2);
                            cd_a60.MCU_MaxMotorTorque = Convert.ToInt32(str_data1, 16);
                            //TODO Filter 范围0-254
                            cd_a60.MCU_MaxMotorTorque = cd_a60.MCU_MaxMotorTorque < 0 ? 0 : cd_a60.MCU_MaxMotorTorque;

                            //电机最大发电扭矩
                            str_data1 = str_all.Substring(9, 2);
                            cd_a60.Motor_MaxGenTorque = Convert.ToInt32(str_data1, 16);
                            //TODO Filter 范围0-254
                            cd_a60.Motor_MaxGenTorque = cd_a60.Motor_MaxGenTorque < 0 ? 0 : cd_a60.Motor_MaxGenTorque;

                            //电机转速
                            str_data1 = str_all.Substring(12, 4);
                            str_data1 = regex.Replace(str_data1, "");
                            data2 = Convert.ToInt32(str_data1, 16);
                            str_data2 = Convert.ToString(data2, 2);
                            if (str_data2.Length < 12)
                                str_data2 = str_data2.PadLeft(12, '0');
                            cd_a60.Motor_Revolution = Convert.ToInt32(str_data2, 2) * 10 - 20000;


                            //电机控制器母线电容放电状态
                            str_data1 = str_all.Substring(19, 1);
                            str_data2 = Convert.ToString(Convert.ToInt32(str_data1, 16), 2);
                            if (str_data2.Length < 4)
                                str_data2 = str_data2.PadLeft(4, '0');
                            str_data1 = str_data2.Substring(1, 1);
                            switch (str_data1)
                            {
                                case "0":
                                    cd_a60.MCU_ActiveDischarge = "0";
                                    break;
                                case "1":
                                    cd_a60.MCU_ActiveDischarge = "1";
                                    break;
                                default:
                                    break;
                            }

                            break;

                        //Motor_DCCurrent 直流母线电压
                        //Motor_ControllerTemp 电机控制器温度
                        //Motor_Temperature 电机温度
                        case "02 04":
                            //直流母线电压
                            str_data2 = str_all.Substring(6, 4);
                            str_data2 = regex.Replace(str_data2, "");
                            data2 = Convert.ToInt32(str_data2, 16);
                            str_data2 = Convert.ToString(data2, 2);
                            if (str_data2.Length < 12)
                                str_data2 = str_data2.PadLeft(12, '0');
                            str_data2 = str_data2.Substring(0, 10);

                            //TODO Filter 范围0-511
                            cd_a60.Motor_DCCurrent = Convert.ToInt32(str_data2, 2) * 0.5;

                            //电机控制器温度
                            str_data2 = str_all.Substring(9, 5);
                            str_data2 = regex.Replace(str_data2, "");
                            data2 = Convert.ToInt32(str_data2, 16);
                            str_data2 = Convert.ToString(data2, 2);
                            if (str_data2.Length < 16)
                                str_data2 = str_data2.PadLeft(16, '0');
                            str_data2 = str_data2.Substring(2, 13);
                            //TODO Filter 范围0-100
                            cd_a60.Motor_ControllerTemp = Convert.ToInt32(str_data2, 2) * 0.02;

                            //电机温度
                            str_data1 = str_all.Substring(13, 6);
                            str_data1 = regex.Replace(str_data1, "");
                            str_data2 = Convert.ToString(Convert.ToInt32(str_data1, 16), 2);
                            if (str_data2.Length < 16)
                                str_data2 = str_data2.PadLeft(16, '0');
                            str_data1 = str_data2.Substring(3, 13);
                            //TODO Filter 范围0-100
                            cd_a60.Motor_Temperature = Convert.ToInt32(str_data1, 2) * 0.02;
                            break;

                        //double IC_TotalOdmeter;//总里程
                        //double IC_Odmeter;//小计里程
                        //double VCU_CruisingRange 剩余行驶里程
                        //VCU_BrakeEnergyReturnIntension 电机状态及强度信度
                        case "04 85":
                            //总里程
                            str_data1 = str_all.Substring(6, 8);
                            str_data1 = regex.Replace(str_data1, "");
                            cd_a60.IC_TotalOdmeter = Convert.ToInt32(str_data1, 16) * 0.1;
                            //TODO Filter 范围0-999999.9
                            cd_a60.IC_TotalOdmeter = cd_a60.IC_TotalOdmeter < 0 ? 0 : cd_a60.IC_TotalOdmeter;
                            cd_a60.IC_TotalOdmeter = Math.Round(cd_a60.IC_TotalOdmeter, 1);

                            //小计里程
                            str_data2 = str_all.Substring(15, 5);
                            str_data2 = regex.Replace(str_data2, "");
                            cd_a60.IC_Odmeter = Convert.ToInt32(str_data2, 16) * 0.1;
                            //TODO Filter 范围0-999.9
                            cd_a60.IC_Odmeter = cd_a60.IC_Odmeter < 0 ? 0 : cd_a60.IC_Odmeter;//小于0 取0
                            cd_a60.IC_Odmeter = Math.Round(cd_a60.IC_Odmeter, 1);//取一位小数

                            //剩余行驶里程
                            str_data2 = str_all.Substring(21, 4);
                            str_data2 = regex.Replace(str_data2, "");
                            data2 = Convert.ToInt32(str_data2, 16);
                            str_data2 = Convert.ToString(data2, 2);
                            if (str_data2.Length < 12)
                                str_data2 = str_data2.PadLeft(12, '0');
                            str_data2 = str_data2.Substring(0, 10);
                            cd_a60.VCU_CruisingRange = Convert.ToInt32(str_data2, 2);
                            //TODO Filter 范围0-1000
                            cd_a60.VCU_CruisingRange = cd_a60.VCU_CruisingRange < 0 ? 0 : cd_a60.VCU_CruisingRange;

                            //电机状态及强度信度
                            str_data2 = str_all.Substring(27, 2);
                            //TODO Filter 范围-50-150
                            data1 = Convert.ToInt32(str_data2, 16) - 50;
                            if (data1 > 0)
                                cd_a60.VCU_BrakeEnergy = "1";//2035-10-30修改
                            else if (data1 == 0)
                                cd_a60.VCU_BrakeEnergy = "0";
                            else
                                cd_a60.VCU_BrakeEnergy = "2";
                            break;

                        default:
                            break;
                    }
                }

                #endregion

                //充电状态   
                //充电时 根据快慢冲 不同字段 显示 正在充电
                //非充电   两个值都显示 非充电状态

                if (!string.IsNullOrEmpty(cd_a60.BMS_SlowChargeSt) && !string.IsNullOrEmpty(cd_a60.BMS_FastChargeSt) && !string.IsNullOrEmpty(cd_a60.BMS_ChargeFS.ToString()))
                {
                    //快慢充
                    if (cd_a60.BMS_SlowChargeSt.Equals("1") || cd_a60.BMS_FastChargeSt.Equals("1"))
                    {
                        cd_a60.BMS_ChargeSt = "1";
                        if (cd_a60.BMS_SlowChargeSt.Equals("1"))
                        {
                            //慢充
                            cd_a60.BMS_ChargeFS = 2;
                        }
                        if (cd_a60.BMS_FastChargeSt.Equals("1"))
                        {
                            //快充
                            cd_a60.BMS_ChargeFS = 1;
                        }
                    }
                    else
                    {
                        //非充电
                        cd_a60.BMS_ChargeFS = 0;
                        cd_a60.BMS_ChargeSt = cd_a60.BMS_SlowChargeSt;
                    }
                }

                //老架构使用,新库 直接按类属性名 存储
                //sqlstr = string.Format("update ev_{0} set Curent={1},Voltage={2},SOC={3},MotorRevolution={4},MotorTemperature={5},OutputPower={6},ABS_VehSpd={7},Distance={8},BatteryTemp_Ave={9},BatteryTemp_Max={10},BatteryTemp_Min={11} where time='{12}'", sysno, cd_ej04.Curent, cd_ej04.Voltage, cd_ej04.SOC, cd_ej04.MotorRevolution, cd_ej04.MotorTemperature, cd_ej04.OutputPower, cd_ej04.ABS_VehSpd, cd_ej04.Distance, cd_ej04.BatteryTemp_Ave, cd_ej04.BatteryTemp_Max, cd_ej04.BatteryTemp_Min, ltime);
                //alarm = "{\"index\":\"1\",\"Sysno\":\"" + sysno + "\",\"time\":\"" + ltime + "\",\"Curent\":\"" + cd_ej04.Curent + "\",\"Voltage\":\"" + cd_ej04.Voltage + "\",\"SOC\":\"" + cd_ej04.SOC + "\",\"MotorRevolution\":\"" + cd_ej04.MotorRevolution + "\",\"MotorTemperature\":\"" + cd_ej04.MotorTemperature + "\",\"OutputPower\":\"" + cd_ej04.OutputPower + "\",\"ABS_VehSpd\":\"" + cd_ej04.ABS_VehSpd + "\",\"Distance\":\"" + cd_ej04.Distance + "\",\"BatteryTemp_Ave\":\"" + cd_ej04.BatteryTemp_Ave + "\",\"BatteryTemp_Max\":\"" + cd_ej04.BatteryTemp_Max + "\",\"BatteryTemp_Min\":\"" + cd_ej04.BatteryTemp_Min + "\"}";
                //data = "";
            }
            else if (index == 5)
            {
                //郑州 日产 锐骐
                Candata_ZzRuiQi cd_zzrq = cd as Candata_ZzRuiQi;
                cantime = infos[infos.Count() - 1];
                ParsZzRQ.GetPars(infos, ref canfault, ref cd_zzrq);

            }
            else if (index == 6)
            {
                //郑州 日产 帅客
                Candata_ZzShuaiKe cd_zzsk = cd as Candata_ZzShuaiKe;
                ParsZzSK.GetPars(infos, ref canfault, ref cd_zzsk);
                cantime = infos[infos.Count() - 1];

            }
            else if (index == 200)
            {
                #region MyRegion


                #endregion

            }

            //foreach (System.Reflection.PropertyInfo p in cd.GetType().GetProperties())
            //{
            //    string typevalue=p.GetValue(cd, null) as string;
            //    if (!string.IsNullOrEmpty(typevalue))
            //    {
            //        jeams += "$" + p.Name + "^" + typevalue;
            //    }
            //}


            foreach (System.Reflection.PropertyInfo p in cd.GetType().GetProperties())
            {
                jeams += string.Format("${0}^{1}", p.Name, p.GetValue(cd, null));
            }

            jeams = jeams.Substring(1, jeams.Length - 1);
            for (int i = 0; i < jeams.Split('$').Count(); i++)
            {

                if (!lis.Contains(jeams.Split('$')[i]))
                {
                    lis.Add(jeams.Split('$')[i]);
                }

            }


            return lis;



        }






    }
}