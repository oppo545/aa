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
using SuperSocket.SocketEngine;

namespace ServerCenterLis
{
    /// <summary>
    /// 008/9.0  与9.0.cs 解析完全相同  接收处多了段 转码
    /// StringRequestInfo    StringRequestInfo
    /// //TODO udp
    /// </summary>
    public class ProtocolFormat_24Fixed : CommandBase<Telnet_Session, StringRequestInfo>
    {
        public override string Name
        {
            get { return "24"; }
        }


        public override void ExecuteCommand(Telnet_Session session, StringRequestInfo requestInfo)
        {
            /// 入库数据时间
            DateTime dtimeput;
            // session.Send("requestInfo.Key:" + requestInfo.Key + "requestInfo.Body" + requestInfo.Body);

            //TODO udp
            //string canstr = Encoding.ASCII.GetString(requestInfo.Body);
            //StringBuilder sb = new StringBuilder();
            //foreach (byte b in requestInfo.Body)
            //{
            //    sb.Append(b.ToString("x").Length == 1 ? "0" + b.ToString("x") + " " : b.ToString("x") + " ");
            //}

            //   session.Send("__"+sb.ToString());
            //canstr = sb.ToString().ToUpper();
            /* str[0]
             * str[1]29 29
             * str[2]85 00 28 37 B7 37 B7 08 06 28 11 31 16...
             *      主信令
             */

            //session.Ft 必须放在 session回话开始时 new,只会new 一遍,放在这里new 就 来一条指令 new了一次
            //2014-05-28 todotzl


            string canstr = requestInfo.Body.ToUpper();
            //  numlog++;
            //   WriteLog.WriteLogZLOther("24", numlog + "_" + canstr, false);

            if (!canstr.Contains("24 24"))
            {
                return;
            }

            //    string[] str = System.Text.RegularExpressions.Regex.Split(canstr, "(24 24 )", RegexOptions.IgnoreCase);


            string Canstr = canstr.Substring(6, canstr.Length - 6);
            string strindex = Canstr.Substring(0, 2);//主信令

            string temp = "";

            //session.Send(str[2].ToString());


            session.Ly = session.getClsLy();
            session.Ly.Lb = Canstr.Substring(0, 2);//80
            session.Ly.Lc = Canstr.Substring(3, 5);//00 26
            session.Ly.Ld = Canstr.Substring(9, 11);//00 80 01 8B

            //Canstr 最后取了一个空格
            session.Ly.Le = Canstr.Substring(Canstr.Length - 6, 2);
            session.Ly.Lf = Canstr.Substring(Canstr.Length - 3, 2);


            string cr = Canstr.Substring(21, Canstr.Length - 21);//命令信息

            //session.siCode = "伪ip转系统编号算法(session.Ly.Ld)//13位号码,首位去0";
            // session.siCode = session.Ly.Ld;//测试
            //session.Send(session.siCode);


            session.Ft = session.getClsFt();
            session.Ft.Simno = PublicMethods.WipToSysNo(session.Ly.Ld);
            session.siCode = session.Ft.Simno;

            //测试用
            //if (session.siCode.Equals("18000000003"))
            //{
            //                       StreamWriter writer = File.AppendText(System.AppDomain.CurrentDomain.BaseDirectory + "SendStr_24.txt");

            //        writer.WriteLine(DateTime.Now.ToString() + " SendStr：");
            //        writer.WriteLine(":18000000003--");
            //        writer.WriteLine("------------------------------------------------");
            //        writer.Close();
            //}
            //接受的指令 直接 发送给车机(中转)
            //30-点名,38 39-开门,关门  (暂无 现分时租赁车辆 使用088 设备)||40-设置停车报警,3F-设置超速报警,6B-里程重设 ,5F-疲劳驾驶
            if (strindex.Equals("30") || strindex.Equals("38") || strindex.Equals("39") || strindex.Equals("40") || strindex.Equals("3F") || strindex.Equals("6B") || strindex.Equals("5F"))
            {
                session.isSet = true;
            }
            else
            {
                session.isSet = false;
            }

            if (!session.isSet)
            {
                //if (AnalysisServer.LisOnline != null)
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

            }

            //上发数据 到车载设备[非web]
            if (session.isSet)
            {
                foreach (var s in session.AppServer.GetAllSessions())
                {
                    if (s.siCode.Equals(session.siCode))
                    {
                        //TODO udp
                        byte[] btemp = PublicMethods.strToToHexByte(canstr);
                        s.Send(btemp, 0, btemp.Length);
                    }
                }
            }
            StringBuilder buffer = new StringBuilder();
            string strinfo = "", strnew, SignalName = "";



            #region 一般位置数据  位置||点名||位置补传
            if (strindex.Equals("80") || strindex.Equals("81") || strindex.Equals("8E"))
            {
                #region MyRegion

                //80 00 26 00 80 01 8B 14 05 27 11 48 41 03 02 77 57 11 42 54 61 00 00 00 00 C0 47 01 07 A1 0C 57 00 00 04 36 00 FF 00 73 0D 
                //a  b     c           d


                //位置数据 
                // StreamWriter writer = File.AppendText(System.AppDomain.CurrentDomain.BaseDirectory + "SendStr_count.txt");
                // writer.WriteLine(DateTime.Now.ToString() + " SendStr：");
                // writer.WriteLine("主信令：" + session.Ly.Lb + "--");
                // writer.WriteLine("包长：" + session.Ly.Lc + "--");
                // writer.WriteLine("伪ip：" + session.Ly.Ld + "--");


                session.Pt1 = session.getClsPt1();
                string[] sis = cr.Substring(0, 17).Split(' ');//时间
                session.Pt1.Time = string.Format("20{0}-{1}-{2} {3}:{4}:{5}", sis[0], sis[1], sis[2], sis[3], sis[4], sis[5]);
                //  writer.WriteLine("时间：" + session.Pt1.Time + "--");

                //纬度
                sis = cr.Substring(18, 11).Split(' ');
                //session.Pt1.Latitude = string.Format("{0}{1}.{2}{3}{4}", sis[0].Substring(1, 1), sis[1].Substring(0, 1), sis[1].Substring(1, 1), sis[2], sis[3]);

                double a = double.Parse(string.Format("{0}{1}.{2}{3}", sis[1].Substring(1, 1), sis[2].Substring(0, 1), sis[2].Substring(1, 1), sis[3]));
                a = Math.Round(a / 60, 5);
                double b = double.Parse(string.Format("{0}{1}", sis[0].Substring(1, 1), sis[1].Substring(0, 1)));
                double c = a + b;
                session.Pt1.Latitude = c.ToString();

                //经度
                sis = cr.Substring(30, 11).Split(' ');
                //session.Pt1.Longitude = string.Format("{0}{1}.{2}{3}{4}", sis[0], sis[1].Substring(0, 1), sis[1].Substring(1, 1), sis[2], sis[3]);

                a = double.Parse(string.Format("{0}{1}.{2}{3}", sis[1].Substring(1, 1), sis[2].Substring(0, 1), sis[2].Substring(1, 1), sis[3]));
                a = Math.Round(a / 60, 5);
                b = double.Parse(string.Format("{0}{1}", sis[0], sis[1].Substring(0, 1)));
                c = a + b;
                session.Pt1.Longitude = c.ToString();




                session.Pt1.Speed = float.Parse(PublicMethods.GetZeroSuppression(cr.Substring(42, 5))).ToString();//速度
                //  writer.WriteLine("速度：" + session.Pt1.Speed + "--");

                sis = cr.Substring(48, 5).Split(' ');
                session.Pt1.Direction = float.Parse(string.Format("{0}{1}", sis[0].Substring(1, 1), sis[1])).ToString();//方向
                // writer.WriteLine("方向：" + session.Pt1.Direction + "--");


                string dw = cr.Substring(54, 2);//定位
                session.Pt1.Positioning = PublicMethods.Get10To2(PublicMethods.Getdecimal(dw).ToString()).Substring(0, 1);


                //writer.WriteLine("定位：" + session.Pt1.Positioning + "--");



                session.Pt1.Kz = cr.Substring(57, 2);
                string kzstatus = GetTwo(cr.Substring(57, 2));
                //空调检测（0：开 1：关）
                string d2 = kzstatus.Substring(5, 1);



                string dd = "";

                //车匙开关 1：车钥打开  0：车钥关闭
                session.Pt1.Cskg = cr.Substring(60, 2);
                session.Pt1.Cskg = int.Parse(session.Pt1.Cskg).ToString();
                //session.Pt1.Ad = cr.Substring(63, 11);

                //writer.WriteLine("Kz：" + session.Pt1.Kz + "--");
                //writer.WriteLine("Cskg：" + session.Pt1.Cskg + "--");
                //writer.WriteLine("Ad：" + session.Pt1.Ad + "--");

                //088协议 是1/千米,此处为1/米 为了统一 /1000
                session.Pt1.Mileage = PublicMethods.Getdecimal(PublicMethods.GetZeroSuppression(cr.Substring(75, 11))).ToString();//里程数
                session.Pt1.Mileage = (double.Parse(session.Pt1.Mileage) / 1000).ToString();
                //writer.WriteLine("里程数：" + session.Pt1.Mileage + "--");



                // writer.WriteLine("校验：" + session.Ly.Le + "--");
                //  writer.WriteLine("包尾：" + session.Ly.Lf + "--");




                if (strindex.Equals("81"))
                {
                    //点名回执
                    string info = string.Format("04|{0}_{1}_{2}_{3}_{4}_{5}_''_{6}_{7}", session.siCode, session.Pt1.Time, session.Pt1.Latitude, session.Pt1.Longitude, session.Pt1.Speed, session.Pt1.Direction, session.Pt1.Positioning, session.Pt1.Mileage);
                    //((Telnet_Server)session.AppServer).DespatchMessage(session.siCode + "|30", info);

                    SignalName = "Gps_Replay";
                    strnew = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'", session.siCode, session.Pt1.Longitude, session.Pt1.Latitude, session.Pt1.Speed, session.Pt1.Direction, "", session.Pt1.Cskg, session.Pt1.Positioning, session.Pt1.Mileage, "", session.Pt1.Time, SignalName, "");
                    strinfo = PublicMethods.FomatToJosnByNew(0, "G04", strnew);
                    strinfo = strinfo.Replace("'", "");
                    //点名
                    session.SendAsToServers(strinfo);
                }
                else
                {
                    //上线  2035/01/06 整理新逻辑  发送到中心服务器  g02  代表上线, 下线不变
                    if (session.Ft != null)
                    {
                        if (!string.IsNullOrEmpty(session.siCode))
                        {
                            if (string.IsNullOrEmpty(session.Pt1.Cskg))
                            {
                                session.Pt1.Cskg = "0";
                            }
                            strinfo = "insert into vehiclerunninginfo.onlinerecode(SystemNo,OnlineTime,Acc,IsOnline)  VALUES('" + session.siCode + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + session.Pt1.Cskg + "','1');";
                            session.mqs.SendMsg(strinfo);
                            //  strinfo = PublicMethods.FomatToJosnByOnLine(session.siCode, "1");
                            // session.SendAsToServers(strinfo);
                            Telnet_Session.isonline = true;

                            // session.SendAsToServers( strinfo);
                        }
                    }

                    #region 老mysql库入库
                    //TODOOLD 位置汇报 老mysql库入库，仅作测试 
                    //      string sqlstr = string.Format("insert into ev_information.ev_{0} (`Time`,P_Longitude,P_Latitude,P_direction,P_speed,P_mileage) values ('{1}',{2},{3},{4},{5},{6})", session.siCode, session.Pt1.Time, session.Pt1.Longitude, session.Pt1.Latitude, session.Pt1.Direction, session.Pt1.Speed, session.Pt1.Mileage);
                    //   session.mqs.SendMsg(sqlstr);
                    #endregion

                    //TODO OLD 测试 发送给WEB
                    string info = string.Format("02|{0}_{1}_{2}_{3}_{4}_{5}__{6}_{7}", session.siCode, session.Pt1.Time, session.Pt1.Latitude, session.Pt1.Longitude, session.Pt1.Speed, session.Pt1.Direction, session.Pt1.Positioning, session.Pt1.Mileage);
                    //((Telnet_Server)session.AppServer).DespatchMessage(info);

                    #region [sql]充电桩信息 管理 默认充电桩 系统编号为 132开头, 其他为车载终端
                    if (session.siCode.Substring(0, 3).Contains("132"))
                    {
                        #region [sql经纬度] 充电桩地理位置入库，仅为null时 【入库 业务数据表】 2035-3-4

                        if (!string.IsNullOrEmpty(session.Pt1.Longitude.Trim()) && !string.IsNullOrEmpty(session.Pt1.Latitude.Trim()) && !string.IsNullOrEmpty(session.siCode.Trim()))
                        {
                            //新结构
                            temp = string.Format("1^{0},{1},{2}", session.Pt1.Longitude, session.Pt1.Latitude, session.siCode);

                            //入库队列
                            if (!string.IsNullOrEmpty(temp))
                            {
                                //killf_sql
                                session.mqs.SendMsg(temp);
                            }
                        }
                        #endregion

                        //发送充电桩充电状态
                        info = string.Format("{0},{1}", session.siCode, d2);
                        session.SendAsToServers(PublicMethods.FomatToJosnByCharging("G20", info));

                        #region [sql 是否充电] 更新充电桩充电状态 【入库 业务数据表】 2035-3-12

                        if (!string.IsNullOrEmpty(d2))
                        {
                            //新结构
                            temp = string.Format("2^{0},{1}", d2, session.siCode);

                            //入库队列
                            if (!string.IsNullOrEmpty(temp))
                            {
                                session.mqs.SendMsg(temp);
                            }
                        }
                        #endregion


                        #region [sql] 更新充电桩 充电起始时间 【入库 业务数据表】 2035-3-19

                        DateTime dtime;
                        bool regtime = DateTime.TryParse(session.Pt1.Time, out dtime);

                        if (regtime)
                        {
                            if (d2.Equals("0") && !session.isCharging) //上一次充电状态为 false 本次为true 说明这是充电桩第一次充电
                            {
                                //新结构
                                temp = string.Format("3^{0},{1}", dtime.ToString("yyyy-MM-dd HH:mm:ss"), session.siCode);

                                //入库队列
                                if (!string.IsNullOrEmpty(temp))
                                {
                                    session.mqs.SendMsg(temp);
                                }
                            }

                            if (d2.Equals("0"))
                            {
                                //充电状态
                                session.isCharging = true;
                            }
                            else
                            {
                                session.isCharging = false;
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region GPS位置入库
                    if (!session.Pt1.Longitude.Trim().Equals("0")) //过滤经纬度为0的数据
                    {
                        //TODO Filter  过滤 时间超过服务器时间的数据  
                        if (PublicMethods.TimeDetermine(session.siCode, session.Pt1.Time, out dtimeput))
                        {
                            //新结构
                            temp = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'", session.siCode, session.Pt1.Longitude, session.Pt1.Latitude, session.Pt1.Speed, session.Pt1.Direction, "", session.Pt1.Cskg, session.Pt1.Positioning, session.Pt1.Mileage, "", dtimeput.ToString("yyyy-MM-dd HH:mm:ss"), "", "");
                            temp = "insert into vehiclerunninginfo.runningrecord(`SystemNo`,`Longitude`,`Latitude`,`Speed`,`Direction`,`Elevation`,`Acc`,`IsLocation`,`Mileage`,`Oil`,`CurrentTime`,`SignalName`,`CurrentValue`)  values(" + temp + ");";
                            //入库队列
                            if (!string.IsNullOrEmpty(temp))
                            {

                                session.mqs.SendMsg(temp);
                            }
                        }
                    }

                    #endregion

                    #region GPS位置 发送给中心服务器
                    //TODO Filter  过滤 时间超过服务器时间的数据  
                    PublicMethods.TimeDetermine(session.siCode, session.Pt1.Time, out dtimeput);
                    //{
                    //发送给中心服务器
                    ////  strinfo = PublicMethods.FomatToJosn("client_message", "2", session.siCode, "G02", buffer.ToString());
                    SignalName = "Gps_Location";
                    strnew = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'", session.siCode, session.Pt1.Longitude, session.Pt1.Latitude, session.Pt1.Speed, session.Pt1.Direction, "", "", session.Pt1.Positioning, session.Pt1.Mileage, "", dtimeput.ToString("yyyy-MM-dd HH:mm:ss"), SignalName, "");
                    strinfo = PublicMethods.FomatToJosnByNew(0, "G02", strnew);
                    strinfo = strinfo.Replace("'", "");
                    //GPS位置
                    //session.SendAsToServers( strinfo);
                    //2035-04-21 修改 将大量发送的数据 与 命令 分开


                    session.SendAsToServers(strinfo);
                    //}

                    #endregion



                    //StreamWriter writer = File.AppendText(System.AppDomain.CurrentDomain.BaseDirectory + "SendStr_24_80.txt");

                    //writer.WriteLine(DateTime.Now.ToString() + " SendStr：");
                    //writer.WriteLine(":" + session.siCode+"--"+session.Pt1.Time+"--");
                    //writer.WriteLine("------------------------------------------------");
                    //writer.Close();
                }

                // writer.WriteLine("------------------------------------------------");

                // writer.Close();

                //平台通用应答
                byte[] btemp = SendPttyyd(session.Ly);
                session.Send(btemp, 0, btemp.Length);

                #endregion
            }
            #endregion
            #region 终端心跳
            else if (strindex.Equals("21"))
            {
                //终端心跳 平台通用应答
                byte[] btemp = SendPttyyd(session.Ly);
                session.Send(btemp, 0, btemp.Length);
            }
            #endregion
            #region //查看状态 车机回复 车载设备状态数据
            else if (strindex.Equals("83"))
            {
                //14 05 27 11 48 41 03 02 77 57 11 42 54 61 00 00 00 00 C0 47 01 07 A1 0C 57 00 00 04 36 00 FF 00 73 0D
                session.Sts = session.getClsSts();
                session.Sts.Dw = cr.Substring(24, 2);
                session.Sts.Cylx = cr.Substring(27, 2);
                session.Sts.Acco = cr.Substring(30, 5);
                session.Sts.Accc = cr.Substring(60, 5);
                session.Sts.Fsfs = cr.Substring(36, 2);
                //Convert.ToInt32(PublicMethods.GetZeroSuppression(session.Sts.Accc), 16).ToString()
                string senstr = string.Format("05|{0}_{1}_{2}_{3}_{4}_{5}", session.siCode, session.Sts.Dw, session.Sts.Cylx, Convert.ToInt32(PublicMethods.GetZeroSuppression(session.Sts.Acco), 16).ToString(), Convert.ToInt32(PublicMethods.GetZeroSuppression(session.Sts.Accc), 16).ToString(), session.Sts.Fsfs);

                //  ((Telnet_Server)session.AppServer).DespatchMessage(session.siCode + "|31", senstr);

                //StreamWriter writer = File.AppendText(System.AppDomain.CurrentDomain.BaseDirectory + "SendStr_24_83.txt");

                //writer.WriteLine(DateTime.Now.ToString() + " SendStr：");
                //writer.WriteLine(":" + senstr + "--");
                //writer.WriteLine("------------------------------------------------");
                //writer.Close();

                session.Send(senstr);
            }
            #endregion
            #region //BMK车载设备通用应答数据 -参数设置回馈[设置停车报警,设置超速报警,里程重设],开门，关门 回复[废弃]
            else if (strindex.Equals("85"))
            {
                string zxl = cr.Substring(0, 2);//主信令
                string zxlz = cr.Substring(3, 2);//子信令
                string result = cr.Substring(6, 2);//结果 1:成功/确认 0:失败 
                result = result.Equals("0") ? "1" : "0";

                string fg = string.Format("{0} {1}", zxl, result);
                strinfo = "";
                if (zxl.Equals("40") || zxl.Equals("3F") || zxl.Equals("6B") || zxl.Equals("5F"))//参数设置回馈
                {
                    //0 成功 1,失败
                    temp = "14|" + session.siCode + "," + int.Parse(result);
                    // ((Telnet_Server)session.AppServer).DespatchMessage(session.siCode + "|" + zxl, temp);


                    if (zxl.Equals("6B"))//里程重设回复
                    {
                        buffer.Clear();
                        buffer.Append("\"signalName\":\"ResetMileageReply\",");
                        buffer.Append("\"time\":\"" + session.Pt1.Time + "\",");
                        buffer.Append("\"result\":\"" + result + "\"");
                        strinfo = PublicMethods.FomatToJosn("client_message", "2", session.siCode, "G10", buffer.ToString());
                    }
                    if (zxl.Equals("40"))//设置停车报警
                    {
                        buffer.Clear();
                        buffer.Append("\"signalName\":\"ParkingAlarmReply\",");
                        buffer.Append("\"time\":\"" + session.Pt1.Time + "\",");
                        buffer.Append("\"result\":\"" + result + "\"");
                        strinfo = PublicMethods.FomatToJosn("client_message", "2", session.siCode, "G12", buffer.ToString());
                    }
                    if (zxl.Equals("3F"))//设置超速报警
                    {
                        buffer.Clear();
                        buffer.Append("\"signalName\":\"OverspeedAlarmReply\",");
                        buffer.Append("\"time\":\"" + session.Pt1.Time + "\",");
                        buffer.Append("\"result\":\"" + result + "\"");
                        strinfo = PublicMethods.FomatToJosn("client_message", "2", session.siCode, "G13", buffer.ToString());
                    }
                    if (zxl.Equals("5F"))//疲劳驾驶
                    {
                        buffer.Clear();
                        buffer.Append("\"signalName\":\"FatigueDrivingReply\",");
                        buffer.Append("\"time\":\"" + session.Pt1.Time + "\",");
                        buffer.Append("\"result\":\"" + result + "\"");
                        strinfo = PublicMethods.FomatToJosn("client_message", "2", session.siCode, "G11", buffer.ToString());
                    }
                    //指令 设置回执
                    session.SendAsToServers(strinfo);
                }

                if (zxl.Equals("38") || zxl.Equals("39"))//开门，关门 回复
                {
                    temp = "14|" + session.siCode + "," + int.Parse(result);
                    // ((Telnet_Server)session.AppServer).DespatchMessage(session.siCode + "|" + zxl, temp);

                    buffer.Clear();
                    buffer.Append("\"systemNo\":\"" + session.siCode + "\",");
                    if (zxl.Equals("38")) //关门
                    {
                        buffer.Append("\"signalName\":\"returnCar\",");
                    }
                    if (zxl.Equals("39")) //开门
                    {
                        buffer.Append("\"signalName\":\"takeCar\",");
                    }

                    buffer.Append("\"value\":\"" + result + "\"");
                    strinfo = PublicMethods.FomatToJosn("client_message", "2", session.siCode, "G18", buffer.ToString());
                    //查看状态
                    session.SendAsToServers(strinfo);
                }

                //记录指令回复
                WriteLog.WriteLogZL("retrunRecv:" + strinfo);
            }
            #endregion
            #region //报警
            else if (strindex.Equals("82"))
            {
                //string zxl = cr.Substring(0, 2);//主信令
                //string zxlz = cr.Substring(3, 2);//子信令
                //string result = cr.Substring(6, 2);//结果 1:成功/确认 0:失败 
                //result = result.Equals("0") ? "1" : "0";

                //string fg = string.Format("{0} {1}", zxl, result);
                //if (zxl.Equals("40") || zxl.Equals("3F") || zxl.Equals("6B"))//参数设置回馈
                //{
                //    //0 成功 1,失败
                //    temp = "14|" + session.siCode + "," + int.Parse(result);
                //    ((Telnet_Server)session.AppServer).DespatchMessage(session.siCode + "|" + zxl, temp);

                //}

                string aram1 = GetTwo(Canstr.Substring(Canstr.Length - 9, 2));
                string aram2 = GetTwo(Canstr.Substring(Canstr.Length - 12, 2));
                //"12345678"
                string cshi = aram1.Substring(6, 1);
                string tc = aram1.Substring(5, 1);
                string csu = aram1.Substring(6, 1);

                string signalstrs = "";


                string errorstr = "";
                if (cshi.Equals("1"))
                {
                    errorstr = "车辆处于疲劳驾驶状态";
                    strinfo = "11|" + session.siCode + "," + session.Pt1.Time + "," + errorstr;
                    //((Telnet_Server)session.AppServer).DespatchMessage(strinfo);
                }
                if (tc.Equals("1"))
                {
                    errorstr = "车辆处于超时停车状态";
                    strinfo = "12|" + session.siCode + "," + session.Pt1.Time + "," + errorstr;
                    // ((Telnet_Server)session.AppServer).DespatchMessage(strinfo);
                }
                if (csu.Equals("1"))
                {
                    errorstr = "车辆处于超速报警状态";
                    strinfo = "13|" + session.siCode + "," + session.Pt1.Time + "," + errorstr;
                    // ((Telnet_Server)session.AppServer).DespatchMessage(strinfo);
                }
                signalstrs += cshi.Equals("1") ? "GPS_FatigueDriving_Warning," : "";
                signalstrs += tc.Equals("1") ? "GPS_OvertimeParking," : "";
                signalstrs += csu.Equals("1") ? "GPS_FatigueDriving," : "";

                string strtoc = "";//入库 
                #region  GPS报警 入库+推送
                for (int i = 0; i < signalstrs.Split(',').Count(); i++)
                {
                    SignalName = signalstrs.Split(',')[i];
                    //报警入库+推送
                    if (!string.IsNullOrEmpty(SignalName))
                    {
                        //TODO Filter  过滤 时间超过服务器时间的数据  
                        if (PublicMethods.TimeDetermine(session.siCode, session.Pt1.Time, out dtimeput))
                        {
                            strtoc = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'", session.siCode, session.Pt1.Longitude, session.Pt1.Latitude, session.Pt1.Speed, session.Pt1.Direction, "", "", session.Pt1.Positioning, session.Pt1.Mileage, "", dtimeput.ToString("yyyy-MM-dd HH:mm:ss"), SignalName, "");

                            temp = "insert into vehiclerunninginfo.runningrecord(`SystemNo`,`Longitude`,`Latitude`,`Speed`,`Direction`,`Elevation`,`Acc`,`IsLocation`,`Mileage`,`Oil`,`CurrentTime`,`SignalName`,`CurrentValue`)  values(" + strtoc + ");";
                            //入库队列
                            if (!string.IsNullOrEmpty(temp))
                            {

                                session.mqs.SendMsg(temp);
                            }

                        }

                        //推送给中心服务器
                        if (!string.IsNullOrEmpty(strtoc))
                        {
                            strtoc = PublicMethods.FomatToJosnByNew(1, "G15", strtoc);
                            strtoc = strtoc.Replace("'", "");
                            //GPS报警数据
                            //session.SendAsToServers( strtoc); //指令队列
                            session.SendAsToServers(strtoc); //大量数据（can，gps报警）
                        }

                    }


                }
                #endregion

                //平台通用应答
                byte[] btemp = SendPttyyd(session.Ly);
                session.Send(btemp, 0, btemp.Length);
            }
            #endregion

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
        /// 得到 10进制数据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int Getdecimal(string info)
        {
            return Convert.ToInt32(info, 16);
        }





        /// <summary>
        /// 平台通用应答
        /// </summary>
        /// <param name="Layout ly">格式类</param>
        /// <returns></returns>
        private byte[] SendPttyyd(Layout ly)
        {
            string info = string.Format("21 00 05 {0} {1} {2}", ly.Le, ly.Lb, "00");

            string jy = PublicMethods.GetJy(info.Substring(0, info.Length).Split(' '));
            info = string.Format("{0} {1}", info, jy).ToUpper();


            //008无消息封装
            // info = GetXxfz(info);

            //加上标示位
            info = string.Format("{0} {1} {2}", "24 24", info, "0D").ToUpper();

            byte[] btemp = PublicMethods.strToToHexByte(info);

            return btemp;
        }
    }
}