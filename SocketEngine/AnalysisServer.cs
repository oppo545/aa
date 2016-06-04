
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using System.Configuration;
using System.Messaging;
using Apache.NMS.ActiveMQ;
using Apache.NMS;
using System.Text.RegularExpressions;

namespace SuperSocket.SocketEngine
{

    /// <summary>
    /// Server
    /// </summary>
    public class AnalysisServer
    {
        /// <summary>
        /// The strip
        /// </summary>
        private static string strip = ConfigurationManager.AppSettings["AnalysisServerIp"].ToString();
        /// <summary>
        /// The strport
        /// </summary>
        private static string strport = ConfigurationManager.AppSettings["AnalysisServerPort"].ToString();

        /// <summary>
        /// The clientIp
        /// </summary>
        private static string clientIp = ConfigurationManager.AppSettings["ClientIp"].ToString();

        /// <summary>
        /// The Polltime
        /// </summary>
        private static int Polltime = int.Parse(ConfigurationManager.AppSettings["Polltime"].ToString());

        /// <summary>
        /// The st
        /// </summary>
        public static Socket st = null;

        /// <summary>
        /// The st0 两个平台，本地客户端连接两个 中心服务器
        /// </summary>
        public static Socket st0 = null;

        #region 当前客户端给各个session 建立连接 发送消息


        /// <summary>
        /// The ts port
        /// </summary>
        private static string tsPort = ConfigurationManager.AppSettings["Telnet_Server_Port"].ToString();

        /// <summary>
        /// Telnet_Server 2035
        /// </summary>
        public static Socket st1 = null;
        /// <summary>
        /// Telnet_Server_vsn9 2014 TCP
        /// </summary>
        public static Socket st2 = null;

        /// <summary>
        /// Telnet_Server_vsn9 2014 UDP
        /// </summary>
        public static Socket st2u = null;

        /// <summary>
        /// Telnet_Server_vsn6 2013
        /// </summary>
        public static Socket st3 = null;
        #endregion
        /// <summary>
        /// The is as connect false-&gt; not connect
        /// </summary>
        public static bool IsStConnect = false;

        /// <summary>
        /// The is as connect false-&gt; not connect
        /// </summary>
        public static bool IsASConnect = false;

        /// <summary>
        /// ASConnectB Is Connect
        /// </summary>
        public static bool IsASConnectB = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisServer" /> class.
        /// </summary>
        public AnalysisServer()
        {
            //  Connect();
        }
        /// <summary>
        /// 存放指令 回执 消息
        /// </summary>
        static MessageQueue mq;

        /// <summary>
        /// 存放GPS+Can (大量的数据)
        /// </summary>
        static MessageQueue mq1;

        /// <summary>
        /// Socket是否创建完毕  防止多线程去创建SOCKET
        /// </summary>
        private static bool IsSingleCreate = false;
        /// <summary>
        /// Creates the socket.
        /// </summary>
        /// <param name="indexc">默认为0 开启两个客户端</param>
        public static void CreateSocket(int indexc = 0)
        {
            try
            {
                //IsAllowConnectB false  IsASConnectB 一定为false  正常
                //IsAllowConnectB true  IsASConnectB false 总结果 false 重连(正常)
                //IsAllowConnectB true  IsASConnectB true 总结果 true  正常
                if ((!IsASConnect || (IsAllowConnectB && !IsASConnectB)) && !IsSingleCreate)
                {
                    IsSingleCreate = true;
                    if (indexc == 100)
                    {
                        indexnum = 0;
                        Connect();
                        WriteLog.WriteErrorLog("ErrorAS", "Connect正常", true);
                        sendDataTimer = new System.Threading.Timer(new System.Threading.TimerCallback(SendDataCallback), null, Polltime, Polltime);
                    }
                    else
                    {
                        if (IsckRewiring && !timer2.Enabled)
                        {
                            //正在运行&&timer.Enabled=false
                            timer2.Enabled = true;
                        }
                        if (!IsckRewiring)
                        {
                            indexnum = indexc;
                            RunRewiring();
                        }
                    }
                }
                IsSingleCreate = false;
            }
            catch (Exception exc)
            {
                WriteLog.WriteErrorLog("ErrorAS", "______创建异常:" + exc.Message, true);
            }
        }
        /// <summary>
        ///发送到中心服务器Timer
        /// </summary>
        static System.Threading.Timer sendDataTimer = null;
        /// <summary>
        /// 入库lis
        /// </summary>
        static List<string> lissql = new List<string>();
        /// <summary>
        /// 装载箱1
        /// </summary>
        static List<string> listemp1 = new List<string>();
        /// <summary>
        /// 装载箱1
        /// </summary>
        static List<string> listemp2 = new List<string>();
        /// <summary>
        /// 集合轮询调用开关
        /// </summary>
        static bool flagtemp = false;
        /// <summary>
        /// 重连计时器
        /// </summary>
        static System.Timers.Timer timer2;
        /// <summary>
        /// 重连 分钟数
        /// </summary>
        private static int t1 = int.Parse(ConfigurationManager.AppSettings["Rewiring"].ToString());
        /// <summary>
        /// 是否正在重连
        /// </summary>
        private static bool IsckRewiring = false;
        /// <summary>
        /// lock
        /// </summary>
        private static object _lock = new object();

        /// <summary>
        /// 单例启动
        /// </summary>
        private static void RunRewiring()
        {
            if (timer2 == null)
            {
                Rewiring();
            }
            else
            {
                timer2.Enabled = true;
            }
        }

        /// <summary>
        /// 开启重连timer
        /// </summary>
        private static void Rewiring()
        {
            lock (_lock)
            {
                try
                {
                    //正在重连
                    IsckRewiring = true;
                    if (!istimer)
                    {
                        //重连
                        timer2 = new System.Timers.Timer();
                        timer2.Interval = 1000 * 60;
                        timer2.Elapsed += new System.Timers.ElapsedEventHandler(timer2_Tick);
                        timer2.Enabled = true;

                        timer2.Start();
                    }
                }
                catch (Exception ex)
                {
                    WriteLog.WriteErrorLog("ErrorAS", "__重连异常" + ex.Message, true);
                }
            }
        }
        /// <summary>
        /// 分钟数 计时
        /// </summary>
        private static int numRewiring = 0;
        /// <summary>
        /// 开启 对应端口 客户端
        /// </summary>
        private static int indexnum = 0;
        /// <summary>
        /// timer正在运行
        /// </summary>
        private static bool istimer = false;
        private static void timer2_Tick(object sender, EventArgs e)
        {
            try
            {

                istimer = true;
                if (numRewiring > t1)
                {
                    numRewiring = 0;
                }
                numRewiring++;
                WriteLog.WriteLogRewiring(string.Format("等待重连:{0} Time:{1:yyyy-MM-dd HH:mm:ss}", numRewiring, DateTime.Now), true);
                if (numRewiring == t1)
                {
                    Connect();
                    IsckRewiring = false;
                    istimer = false;
                    //初始化
                    numRewiring = 0;
                    timer2.Stop();
                    WriteLog.WriteLogRewiring(string.Format("重连完毕 Time:{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now), true);
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteErrorLog("ErrorAS", "__重连异常" + ex.Message, true);
            }
        }

        ///// <summary>
        ///// 计数器
        ///// </summary>
        //public static int msmqcount;
        /// <summary>
        /// mq_ReceiveCompleted
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ReceiveCompletedEventArgs" /> instance containing the event data.</param>
        static void mq_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            MessageQueue mq = (MessageQueue)sender; 
            System.Messaging.Message m= mq.EndReceive(e.AsyncResult);
            try
            {
                //throw new NotImplementedException();
                //处理消息
                string str = m.Body.ToString();

                //int index = 0;
                //if (!AnalysisServer.IsASConnect)
                //{
                //    index = 1;
                //    CreateSocket(1);
                //}
                //if (!AnalysisServer.IsASConnectB)
                //{
                //    index = 2;
                //    CreateSocket(2);
                //}

                if (AnalysisServer.IsASConnect || AnalysisServer.IsASConnectB)
                {
                    //AnalysisServer.SendMsg(strinfo);
                    // SendMsg(str);
                    if (!flagtemp)
                        listemp1.Add(str);
                    else listemp2.Add(str);

                    //继续下一条消息
                    mq.BeginReceive();
                }
                else
                {
                    if (IsckRewiring && !timer2.Enabled)
                    {
                        //正在运行&&timer.Enabled=false
                        timer2.Enabled = true;
                    }
                    if (!IsckRewiring)
                    {
                        RunRewiring();
                    }
                    //    CreateSocket(index);
                    //    if (AnalysisServer.IsASConnect)
                    //    {
                    //        // AnalysisServer.SendMsg(strinfo);
                    //        SendMsg(str);
                    //        //继续下一条消息
                    //        mq.BeginReceive();

                    //        AnalysisServer.msmqcount--;
                    //    }
                }

            }
            catch (System.Xml.XmlException ex)
            {
                createMSMQReceive();
                WriteLog.WriteErrorLog("ErrorAS", string.Format("______读取异常1:{0}___________________", ex), true);
            }
            catch (Exception ex)
            {
                mq.BeginReceive();
                WriteLog.WriteErrorLog("ErrorAS", string.Format("______读取异常2:{0}___________________{1}", ex, m), true);
            }
        }
        /// <summary>
        /// lock
        /// </summary>
        private static object _lock1 = new object();
        private static void SendDataCallback(Object sender)
        {
            lock (_lock1)
            {
                try
                {
                    if (listemp1.Count > 0 || listemp2.Count > 0 && lissql.Count == 0)
                    {
                        if (!flagtemp)
                        {
                            flagtemp = true;
                            lissql.AddRange(listemp1);
                            listemp1.Clear();
                        }
                        else
                        {
                            flagtemp = false;
                            lissql.AddRange(listemp2);
                            listemp2.Clear();
                        }
                        if (lissql.Count > 0)
                        {
                            for (int i = 0; i < lissql.Count(); i++)
                            {
                                SendMsg(lissql[i]);
                            }
                            lissql.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLog.WriteErrorLog("ErrorAS", "______异常:SendDataCallback" + ex.Message, true);
                }
            }
        }
        /// <summary>
        /// consumer_Listener active 
        /// </summary>
        /// <param name="message"></param>
        static void consumer_Listener(IMessage message)
        {
            ITextMessage msg = (ITextMessage)message;

            if (AnalysisServer.IsASConnect || AnalysisServer.IsASConnectB)
            {
                //AnalysisServer.SendMsg(strinfo);
                SendMsg(msg.Text);
            }
            else
            {
                if (IsckRewiring && !timer2.Enabled)
                {
                    //正在运行&&timer.Enabled=false
                    timer2.Enabled = true;
                }
                if (!IsckRewiring)
                {
                    indexnum = 0;
                    RunRewiring();
                }
            }
        }

        /// <summary>
        /// The thread clientA
        /// </summary>
        public static Thread threadClient = null;//负责 监听 服务端A发送来的消息的线程

        /// <summary>
        /// The thread clientB
        /// </summary>
        public static Thread threadClientB = null;//负责 监听 服务端B发送来的消息的线程


 
        //public static Thread threadheartbeat = null;//发送心跳


        /// <summary>
        /// Connects this instance.
        /// </summary>
        public static void Connect()
        {
            int indexc = indexnum;
            //判断是哪个客户端断掉
            int index = 0;
            IPAddress ip = null;
            IPEndPoint ipe = null;

            #region ClientA
            try
            {

                if (!IsASConnect)
                {

                    IsASConnect = true;
                    WriteLog.WriteSendLog("OpenA", true);

                    ip = IPAddress.Parse(strip);
                    ipe = new IPEndPoint(ip, int.Parse(strport.Split(',')[0]));
                    st = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    if (!clientIp.Equals("0"))
                    {
                        IPEndPoint localIPE = new IPEndPoint(IPAddress.Parse(clientIp), 0);
                        st.Bind(localIPE);
                    }
                    st.Connect(ipe);
                    WriteLog.WriteSendLog(string.Format("OpenA:{0}:{1}", ipe.Address,ipe.Port), true);

                    index = 1;

                    threadClient = null;
                    //接受A中心服务器的消息
                    threadClient = new Thread(ReceiveMsg);
                    threadClient.IsBackground = true;
                    threadClient.SetApartmentState(ApartmentState.MTA);
                    threadClient.Start();

                }
            }
            catch (Exception exc)
            {
                WriteLog.WriteErrorLog("ErrorAS", string.Format("______ConnectA异常"));
                WriteLog.WriteErrorLog("ErrorAS", string.Format("______ConnectA异常:exc{0}:链接中心服务器失败,{1}__本地ip:{2}__端口:{3}", index, exc.Message, IPAddress.Parse(((IPEndPoint)st.LocalEndPoint).Address.ToString()), ((IPEndPoint)st.LocalEndPoint).Port), true);
                IsASConnect = false;
            }
            #endregion

            #region ClientB

            try
            {
                if (!IsASConnectB)
                {
                    //为0时，不连接B平台
                    if (!strport.Split(',')[1].Equals("0"))
                    {

                        IsASConnectB = true;
                        WriteLog.WriteSendLog("OpenB", true);



                        //安全监控
                        ip = IPAddress.Parse(strip);
                        ipe = new IPEndPoint(ip, int.Parse(strport.Split(',')[1]));
                        st0 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        if (!clientIp.Equals("0"))
                        {
                            IPEndPoint localIPE = new IPEndPoint(IPAddress.Parse(clientIp), 0);
                            st0.Bind(localIPE);
                        }
                        st0.Connect(ipe);
                        WriteLog.WriteSendLog(string.Format("OpenB:{0}:{1}", ipe.Address, ipe.Port), true);

                        index = 2;

                        //接受B中心服务器的消息
                        threadClientB = null;
                        threadClientB = new Thread(ReceiveMsgB);
                        threadClientB.IsBackground = true;
                        threadClientB.SetApartmentState(ApartmentState.MTA);
                        threadClientB.Start();

                    }
                }
            }
            catch (Exception exc)
            {
                WriteLog.WriteErrorLog("ErrorAS", string.Format("______读取异常:exc{0}:链接中心服务器失败,{1}__本地ip:{2}__端口:{3}", index, exc.Message, IPAddress.Parse(((IPEndPoint)st.LocalEndPoint).Address.ToString()), ((IPEndPoint)st.LocalEndPoint).Port), true);
                if (index == 0)
                {
                    IsASConnect = false;
                }
                else if (index == 1)
                {
                    IsASConnectB = false;
                }
            }
            #endregion

            try
            {
                StringBuilder buffer = new StringBuilder();
                buffer.Append("{\"key\":\"client_bind\",");
                buffer.Append("\"sendType\":\"2\",");
                buffer.Append("\"data\":{");
                buffer.Append("\"account\":\"Data Insert Service\",");
                buffer.Append("\"deviceId\":\"Note3\",");
                buffer.Append("\"channel\":\"client\",");
                buffer.Append("\"deviceModel\":\"client\"");
                buffer.Append("}");
                buffer.Append("}\b");

                SendMsg(buffer.ToString(), indexc);
            }
            catch (Exception exc)
            {
                WriteLog.WriteErrorLog("ErrorAS", string.Format("______读取异常:exc{0}:发送绑定信息失败,{1}", index, exc.Message), true);
            }


            #region 打开消息队列链接

            try
            {
                //心跳
                //threadheartbeat = new Thread(SendHeartbeat);
                //threadheartbeat.IsBackground = true;
                //threadheartbeat.SetApartmentState(ApartmentState.STA);
                //threadheartbeat.Start();

                string mqIdentify = System.Configuration.ConfigurationManager.AppSettings["mqIdentify"].ToString();
                switch (mqIdentify.Trim().ToLower())
                {
                    case "msmq":
                        createMSMQReceive();
                        break;
                    case "activemq":
                        createActiveMQReceive();
                        break;
                    default:
                        break;

                }


                //在线列表
                if (LisOnline == null)
                {
                    LisOnline = new List<string>();
                }

                //BMK 2035-11-04 修改, 因为开启了 空闲清理,所以当被清理后,此方法因为 重连中心服务器 调用,老被重连,
                //所以 开启了空闲清理功能,此处不初始化
                //if (!IsStConnect)
                //{
                //    //接入服务器
                //    SessionOpen();
                //}
            }
            catch (Exception ex)
            {
                WriteLog.WriteErrorLog("ErrorAS", "______打开消息队列链接异常:" + ex.Message, true);
            }
            #endregion


        }

        /// <summary>
        /// 创建ACtiveMQ接受
        /// </summary>
        public static void createActiveMQReceive()
        {
            string icfactory = System.Configuration.ConfigurationManager.AppSettings["icfactory"].ToString();
            IConnectionFactory factory = new ConnectionFactory(icfactory);
            //通过工厂构建连接
            IConnection connection = factory.CreateConnection();
            //启动连接，监听的话要主动启动连接
            connection.Start();
            //通过连接创建一个会话
            ISession session = connection.CreateSession();
            //通过会话创建一个消费者，这里就是Queue这种会话类型的监听参数设置
            IMessageConsumer consumer = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue("firstQueue"), "filter='demo'");

            //注册监听事件 1
            consumer.Listener += new MessageListener(consumer_Listener);

            IMessageConsumer consumer1 = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue("secondQueue"), "filter='demo'");
            //注册监听事件 2
            consumer1.Listener += new MessageListener(consumer_Listener);

        }
        /// <summary>
        /// 创建MSMQ接受
        /// </summary>
        public static void createMSMQReceive()
        {
            try
            {

                //新建消息循环队列或连接到已有的消息队列
                string path = ".\\private$\\killf_Lu";
                if (MessageQueue.Exists(path))
                {
                    mq = new MessageQueue(path);
                }
                else
                {
                    mq = MessageQueue.Create(path);
                }
                mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                mq.ReceiveCompleted += mq_ReceiveCompleted;
                mq.BeginReceive();

                //新建消息循环队列或连接到已有的消息队列
                path = ".\\private$\\killf_Lu_GC";
                if (MessageQueue.Exists(path))
                {
                    mq1 = new MessageQueue(path);
                }
                else
                {
                    mq1 = MessageQueue.Create(path);
                }
                mq1.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                mq1.ReceiveCompleted += mq_ReceiveCompleted;
                mq1.BeginReceive();

                WriteLog.WriteErrorLog("ErrorAS", "MSMQ启动正常", true);
            }
            catch (Exception ex)
            {
                WriteLog.WriteErrorLog("ErrorAS", "创建MSMQ异常:" + ex.Message, true);
            }
        }


        /// <summary>
        /// Sessions the open.
        /// </summary>
        public static void SessionOpen()
        {
            string[] infos = tsPort.Split(',');
            //string indextype = "";
            //string msgstr = "";
            try
            {
                IPAddress ip = IPAddress.Parse("127.0.0.1");

                int temp = int.Parse(infos[0].ToString());
                if (temp != 0)
                {
                    IPEndPoint ipe = new IPEndPoint(ip, temp);
                    st1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    st1.Connect(ipe);
                    //发送一个空包 创造出标示(用于清理后,根据OtherClient初始化标识)，用以空闲清理后恢复
                    //所有需要 从网站平台下发指令的 都需要
                    //indextype = "2";
                    //msgstr = "7E 00 00 00 00 00 00 00 00 00 00 7E";
                }
                //temp = int.Parse(infos[1].ToString());
                //if (temp != 0)
                //{
                //    //TCP
                //    IPEndPoint ipe = new IPEndPoint(ip, temp);
                //    st2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //    st2.Connect(ipe);

                //    //UDP
                //    st2u = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //    st2u.Connect(ipe);
                //}
                //temp = int.Parse(infos[2].ToString());
                //if (temp != 0)
                //{
                //    IPEndPoint ipe = new IPEndPoint(ip, temp);
                //    st3 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //    st3.Connect(ipe);
                //}
                IsStConnect = true;

                //if (!string.IsNullOrEmpty(indextype))
                //{
                //    ST_SendMsg(indextype, msgstr);
                //}

                //indextype = "4";
                //msgstr = "23 23 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
                //if (!string.IsNullOrEmpty(indextype))
                //{
                //    ST_SendMsg(indextype, msgstr);
                //}

            }
         
            catch (Exception exc)
            {
                IsStConnect = false;
                WriteLog.WriteErrorLog("ErrorAS", "______SessionOpen异常:" + exc.Message, true);
            }

        }


        /// <summary>
        /// Closes this instance.
        /// </summary>
        public static void Close()
        {
            if (IsASConnect)
            {
                st.Close();
                st0.Close();
                LisOnline = null;
                WriteLog.WriteSendLog("close");
            }
        }


        #region 接收服务端发送来的消息数据
        /// <summary>
        /// 接收服务端B 发送来的消息数据
        /// </summary>
        private static void ReceiveMsgB()
        {
            while (true && IsASConnectB)
            {
                WriteLog.WriteSendLog("ReceiveReadyB");
                try
                {
                    string recvStr = "";
                    byte[] recvBytes = new byte[1024];
                    int bytes;
                    bytes = st0.Receive(recvBytes, recvBytes.Length, 0);

                    recvStr += Encoding.UTF8.GetString(recvBytes, 0, bytes);
                    //重连
                    if (bytes == 0)
                    {
                        //服务器异常断开,链路不正常
                        IsASConnect = false;
                        RunRewiring();
                        return;
                    }

                    WriteLog.WriteLogrecvStr("recvStrB:" + recvStr);
                    //组包
                    recvStr = lastReceiveMsg + recvStr;
                    if (!string.IsNullOrEmpty(recvStr))
                    {
                        //获取不完整的包
                        List<String> lislast = GetContentByReg(recvStr, @"(?<=}})[^}]+$");
                        if (lislast.Count > 0)
                        {
                            lastReceiveMsg = lislast[lislast.Count - 1];
                        }
                        else
                        {
                            lastReceiveMsg = "";
                        }

                        //分包
                        List<String> lis = GetContentByReg(recvStr, @"{""key"":[^\}]+}}");
                        for (int i = 0; i < lis.Count; i++)
                        {
                            WriteLog.WriteLogrecvStr("RecvStrBConfirmed:");
                            //接收中心服务器 数据,发送给车载终端
                            ST_MsgToClient(lis[i].ToString());
                        }

                    }
                }
                catch (Exception ex)
                {
                    WriteLog.WriteErrorLog("ErrorAS", "______接受B异常:" + ex.Message, true);
                }

            }

        }

        private static string lastReceiveMsg;
        /// <summary>
        /// 接收服务端A 发送来的消息数据
        /// </summary>
        private static void ReceiveMsg()
        {
            while (true && IsASConnect)
            {
                WriteLog.WriteSendLog("ReceiveReadyA");
                try
                {
                    string recvStr = "";
                    byte[] recvBytes = new byte[1024];
                    int bytes;
                    bytes = st.Receive(recvBytes, recvBytes.Length, 0);

                    
                    recvStr += Encoding.UTF8.GetString(recvBytes, 0, bytes);
                    //重连
                    if (bytes == 0)
                    {
                        //服务器异常断开,链路不正常
                        IsASConnect = false;
                        RunRewiring();
                        return;
                    }
                    
                    WriteLog.WriteLogrecvStr("recvStrA:" + recvStr);
                    //组包
                    recvStr = lastReceiveMsg + recvStr;
                    if (!string.IsNullOrEmpty(recvStr))
                    {
                        //获取不完整的包
                        List<String> lislast = GetContentByReg(recvStr, @"(?<=}})[^}]+$");
                        if (lislast.Count > 0)
                        {
                            lastReceiveMsg = lislast[lislast.Count - 1];
                        }
                        else
                        {
                            lastReceiveMsg = "";
                        }
                        //分包
                        List<String> lis = GetContentByReg(recvStr, @"{""key"":[^\}]+}}");
                        for (int i = 0; i < lis.Count; i++)
                        {
                            WriteLog.WriteLogrecvStr("RecvStrAConfirmed");
                            //接收中心服务器 数据,发送给车载终端
                            ST_MsgToClient(lis[i].ToString());
                        }

                    }
                }
                catch (Exception ex)
                {
                    WriteLog.WriteErrorLog("ErrorAS", "______接受A异常:" + ex.Message, true);
                }

            }

        }
        #endregion

        /// <summary>
        /// 获取匹配的集合
        /// </summary>
        /// <param name="str"></param>
        /// <param name="regx"></param>
        /// <returns></returns>
        public static List<string> GetContentByReg(string str, string regx)
        {
            string tmpStr = regx;
            MatchCollection TitleMatch = Regex.Matches(str, tmpStr, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace);
            //来执行完 MatchCollection mc = reg.Matches(strPage) 的时候，有多少个匹配的数量是不确定的，
            //而是要等到访问mc的Count或Item属性时，才真正第去执行匹配的过程，算出匹配的数量。
            //超时限制 解决方法 让程序跑完这一求值过程 
            int u = TitleMatch.Count;

            List<string> lis = new List<string>();
            for (int i = 0; i < u; i++)
            {
                lis.Add(TitleMatch[i].Groups[0].ToString());
            }
            return lis;
        }
      
        //private static void SendHeartbeat()
        //{
        //    //空闲时（无任务） 发送心跳
        //    while (true && msmqcount <= 0)
        //    {
        //        try
        //        {
        //            Thread.Sleep(60000);

        //            SendMsg("{\"key\":\"\",\"sender\":\"\",\"sendType\":\"0\",\"receiver\":\"\",\"identifying\":\"\",\"interChange\":\"\",\"data\":{\"heartbeat\":\"1\"}}");

        //        }
        //        catch (Exception)
        //        {

        //        }

        //    }

        //}

        /// <summary>
        /// 是否允许链接b平台
        /// </summary>
        private static bool IsAllowConnectB = !strport.Split(',')[1].Equals("0");
        /// <summary>
        /// Sends the MSG. 发送消息给中心服务器
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="indexc">标示</param>
        public static void SendMsg(string msg, int indexc = 0)
        {
            //标示 判断是哪个平台发送失败
            int index = 0;

            try
            {
                if (!IsASConnect)
                {
                    // Connect();
                    CreateSocket(1);
                }

                //允许开启第二客户端 
                if (IsAllowConnectB && !IsASConnectB)
                {
                    //IsAllowConnectB = true;
                    // Connect();
                    CreateSocket(2);
                }

              //  string smsg = msg + "\b";
                string smsg = msg;

                //按长度发送
                //0000{"key":"","sender":"","sendType":"0","receiver":"null","timestamp":"1417491229642","identifying":"G170","interChange":"","data":{"systemNo":"13300000017","signalName":"shutDown"}}
                // smsg = polishIndex(msg, 4) + msg;
                byte[] bs = Encoding.UTF8.GetBytes(smsg);
                int num = 0;

                //发送给所有服务器
                if (indexc == 0)
                {
                    if (IsASConnect) //A平台
                    {
                        num = st.Send(bs, bs.Length, 0);
                        WriteLog.WriteSendLog("sendA:" + smsg);
                        WriteLog.WriteSendLog("num:" + num + "Length:" + bs.Length);
                        index = 1;
                    }
                    //B平台
                    if (!strport.Split(',')[1].Equals("0") && IsASConnectB)
                    {
                        num = st0.Send(bs, bs.Length, 0);
                        WriteLog.WriteSendLog("sendB:" + smsg);
                        WriteLog.WriteSendLog("num:" + num + "Length:" + bs.Length);
                        index = 2;
                    }
                }
                else if (indexc == 1 && IsASConnect)
                {
                    //单独A平台
                    num = st.Send(bs, bs.Length, 0);
                    WriteLog.WriteSendLog("sendA:" + smsg);
                    WriteLog.WriteSendLog("num:" + num + "Length:" + bs.Length);
                    index = 1;
                }
                else if (indexc == 2 && IsASConnectB)
                {
                    //单独B平台
                    num = st0.Send(bs, bs.Length, 0);
                    WriteLog.WriteSendLog("sendB:" + smsg);
                    WriteLog.WriteSendLog("num:" + num + "Length:" + bs.Length);
                    index = 2;


                }

            }
            catch (Exception exc)
            {
                WriteLog.WriteErrorLog("ErrorAS", "______读取异常:" + "exc" + index + ":发送中心服务器数据失败," + exc.Message, true);
                if (index == 0)
                {
                    IsASConnect = false;
                }
                else if (index == 1)
                {
                    IsASConnectB = false;
                }
                //CreateSocket();;
            }
        }

        /// <summary>
        /// 不҉足҉位҉补҉充҉
        /// </summary>
        /// <param name="msgLengt">The MSG lengt.</param>
        /// <param name="condition">The condition.</param>
        /// <returns>System.String.</returns>
        public static string polishIndex(string msgLengt, int condition)
        {
            String newSize = "";
            if (msgLengt.Length == condition)
            {
                return msgLengt;
            }
            else
            {
                newSize = "0" + msgLengt;
            }
            return polishIndex(newSize, condition);
        }



        /// <summary>
        /// Sends the MSG. 发送消息给服务器
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="msg">The MSG.</param>
        public static void ST_SendMsg(string type, string msg)
        {
            try
            {
                if (!IsStConnect)
                {
                    SessionOpen();
                }

                if (IsStConnect)
                {
                    if (type.Equals("2") || type.Equals("4"))//龙安 2013 - 海马设备
                    {
                        //添加 本地客户端发送标识 MiddleIndex
                        //7e 85 69 00 7e
                        msg = msg.Substring(0, msg.Length - 2) + MiddleIndex + msg.Substring(msg.Length - 3, 3);
                    }
                    //  byte[] bs = Encoding.Unicode.GetBytes(msg);
                    byte[] bs = strToToHexByte(msg);

                    string tempinfo = "";
                    if (type.Equals("1"))//龙安9.0 TCP
                    {
                        tempinfo = "龙安9.0_TCP设备";
                        // st2.Send(bs, bs.Length, 0);
                    }
                    if (type.Equals("11"))//龙安9.0 UDP
                    {
                        tempinfo = "龙安9.0_UDP设备";
                        //st2u.Send(bs, bs.Length, 0);
                    }
                    if (type.Equals("2"))//龙安 2013
                    {
                        tempinfo = "龙安2013设备";
                        // st1.Send(bs, bs.Length, 0);

                    }
                    if (type.Equals("3"))//博实结 V2.8.10
                    {
                        tempinfo = "博实结设备";
                    }
                    if (type.Equals("4"))//北京地标
                    {
                        tempinfo = "北京地标_海马设备";
                        //    st3.Send(bs, bs.Length, 0);
                    }
                    st1.Send(bs, bs.Length, 0);
                    WriteLog.WriteLogZL(string.Format("send to {0}:{1}", tempinfo, msg));
                }

            }
            catch (Exception exc)
            {
                WriteLog.WriteLogZL("exc:发送指令失败," + exc.Message, true);
                IsStConnect = false;
                SessionOpen();
            }
        }


        /// <summary>
        /// Sends as to servers.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="strinfo">The strinfo.</param>
        private static void ST_SendToServers(string type, string strinfo)
        {
            if (IsStConnect)
            {
                ST_SendMsg(type, strinfo);
            }
            else
            {
                SessionOpen();
                if (IsStConnect)
                {
                    ST_SendMsg(type, strinfo);
                }
            }
        }

        /// <summary>
        /// 本地客户端发送指令标识,其他发送的不予处理
        /// </summary>
        private static string MiddleIndex = ConfigurationManager.AppSettings["MiddleIndex"].ToString();

        #region 封装与解析
        /// <summary>
        /// 发送信息到 车载终端
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public static void ST_MsgToClient(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                string qqstr = "";
                try
                {
                    string index = string.Empty;
                    string info = string.Empty;
                    qqstr = GetDateStr3(msg);//请求语句

                    string qqzl = qqstr.Split('_')[0];//自定义指令

                    string yj = qqstr.Split('_')[1];//|之后的指令

                    string temp = "";//指令
                    string vehicleMode = "";//终端型号

                    #region G03-车辆在线列表请求 【已废弃_版本变更】
                    if (qqzl.Equals("G03"))   //车辆在线列表请求
                    {
                        string Online = "";
                        string[] infos = qqstr.Split('_')[1].Split(',');
                        foreach (var item in infos)
                        {
                            if (LisOnline.Contains(item))
                            {
                                if (string.IsNullOrEmpty(Online))
                                {
                                    Online = item;
                                }
                                else
                                {
                                    Online += "_" + item;
                                }

                            }
                        }

                        //封装成json格式,发送给中心服务器->web
                        info = FomatToJosn("client_message", "2", qqstr.Split('_')[2].ToString(), qqstr.Split('_')[0].ToString(), "\"sysnos\":\"" + Online + "\"");
                        //在线列表


                        if (!string.IsNullOrEmpty(info))
                        {
                            //mq.Send(info);//以消息队列 发送消息
                            SendMsg(info); //直接发送
                        }
                    }
                    #endregion
                    #region G04-点名 √
                    else if (qqzl.Equals("G04"))
                    {
                        //判断 终端型号
                        vehicleMode = qqstr.Split('_')[2].ToString();
                        if (!string.IsNullOrEmpty(vehicleMode))
                        {
                            if (vehicleMode.Equals("1") || vehicleMode.Equals("11"))//9.0 TCP  || UDP
                            {
                                //点名 9.0
                                string yhzfc = string.Format("30 00 06 {0}", SysNoToIp(yj));
                                temp = string.Format("24 24 {0} {1} 0D", yhzfc, GetJy(yhzfc.Split(' ')));

                                index = "|30";
                            }
                            else if (vehicleMode.Equals("2"))
                            {
                                temp = GetFomartNullStr("82 01", yj);

                                index = "|82 01";
                            }
                            else if (vehicleMode.Equals("3"))
                            {

                                //博实结 V2.8.10
                                string yhzfc = string.Format("30 00 06 {0}", SysNoToIp(yj));
                                temp = string.Format("29 29 {0} {1} 0D", yhzfc, GetJy(yhzfc.Split(' ')));

                                index = "|30";
                            }




                        }
                    }
                    #endregion
                    #region 008- G05-查看状态 √
                    else if (qqzl.Equals("G05"))
                    {

                        //判断 终端型号
                        vehicleMode = qqstr.Split('_')[2].ToString();
                        if (!string.IsNullOrEmpty(vehicleMode))
                        {
                            if (vehicleMode.Equals("1") || vehicleMode.Equals("11"))//9.0 TCP  || UDP
                            {
                                //查看状态   9.0    
                                string yhzfc = string.Format("31 00 06 {0}", SysNoToIp(yj));
                                temp = string.Format("24 24 {0} {1} 0D", yhzfc, GetJy(yhzfc.Split(' ')));

                                index = "|31";
                            }
                            else if (vehicleMode.Equals("2"))
                            {
                                temp = GetFomartNullStr("81 04", yj);

                                index = "|81 04";
                            }


                        }


                        // info = "伪ip:" + SysNoToIp(yj) + "校验:" + GetJy(yhzfc.Split(' ')) + "temp:" + temp;
                    }
                    #endregion
                    #region G10-里程重设 √
                    else if (qqzl.Equals("G10"))//里程重设
                    {
                        //获取系统编号与数值集合  13500000001-13500000002,600000,2-11
                        string[] sendstr = yj.Split(',');
                        string[] sysnos = sendstr[0].Split('-');
                        //判断 终端型号[根据协议]
                        vehicleMode = qqstr.Split('_')[2].ToString();
                        string[] vms = vehicleMode.Split('-');
                        if (!string.IsNullOrEmpty(vehicleMode))
                        {
                            //循环发送
                            for (int i = 0; i < sysnos.Length; i++)
                            {
                                if (vms[i].Equals("1") || vms[i].Equals("11"))//9.0 TCP  || UDP
                                {
                                    string cs = Convert.ToString(int.Parse(sendstr[1].ToString()), 16).ToString();
                                    string yhzfc = string.Format("6B 00 08 {0} {1}", SysNoToIp(sysnos[i].ToString()), GetFomartZ(cs, 4));
                                    temp = string.Format("24 24 {0} {1} 0D", yhzfc, GetJy(yhzfc.Split(' ')));

                                    index = "|6B";
                                    //TODOMore 多车设置 循环发送
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        //按照协议发送到 不同Telnet_Session 
                                        ST_SendToServers(vms[i], temp);
                                    }
                                    //置空 后面不设置
                                    temp = "";
                                }
                                else if (vms[i].Equals("2"))//2011/2013
                                {

                                    string cs = Convert.ToString(int.Parse(sendstr[1].ToString()) * 10, 16).ToString();
                                    //00 01 流水号
                                    string yhzfc = string.Format("81 03 00 08 {0} 00 80 01 00 80 04 {1}", GetFomartSysno(sysnos[i].ToString()), GetFomartZ(cs, 8));
                                    temp = string.Format("7E {0} {1} 7E", yhzfc, GetJy(yhzfc.Split(' ')));
                                    index = "|81 03";
                                    //TODOMore 多车设置 循环发送
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        //按照协议发送到 不同Telnet_Session 
                                        ST_SendToServers(vms[i], temp);
                                    }
                                    //置空 后面不设置
                                    temp = "";

                                }
                            }




                        }
                    }
                    #endregion
                    #region G11-疲劳驾驶设置 √
                    else if (qqzl.Equals("G11"))//疲劳驾驶设置
                    {
                        //获取系统编号与数值集合  13500000001-13500000002,600000,2-11
                        string[] sendstr = yj.Split(',');
                        string[] sysnos = sendstr[0].Split('-');

                        //判断 终端型号[根据协议]
                        vehicleMode = qqstr.Split('_')[2].ToString();
                        string[] vms = vehicleMode.Split('-');
                        if (!string.IsNullOrEmpty(vehicleMode))
                        {
                            //循环发送
                            for (int i = 0; i < sysnos.Length; i++)
                            {
                                if (vms[i].Equals("1") || vms[i].Equals("11"))//9.0 TCP  || UDP
                                {
                                    string cs = Convert.ToString(int.Parse(sendstr[1]), 16);//单位为小时
                                    string buffer = Convert.ToString(int.Parse(sendstr[2]), 16);//单位为分钟
                                    string yhzfc = string.Format("5F 00 08 {0} {1} {2}", SysNoToIp(sysnos[i].ToString()), GetFomartZ(cs, 2), GetFomartZ(buffer, 2));
                                    temp = string.Format("24 24 {0} {1} 0D", yhzfc, GetJy(yhzfc.Split(' ')));

                                    index = "|5F";
                                    //TODOMore 多车设置 循环发送
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        //按照协议发送到 不同Telnet_Session 
                                        ST_SendToServers(vms[i], temp);
                                    }
                                    //置空 后面不设置
                                    temp = "";
                                }
                                else if (vms[i].Equals("2"))//2011/2013
                                {
                                    string cs = Convert.ToString(int.Parse(sendstr[1].ToString()) * 60 * 60, 16).ToString();//单位为s
                                    string yhzfc = string.Format("81 03 00 08 {0} 00 57 01 00 57 04 {1}", GetFomartSysno(sysnos[i].ToString()), GetFomartZ(cs, 8));
                                    temp = string.Format("7E {0} {1} 7E", yhzfc, GetJy(yhzfc.Split(' ')));
                                    index = "|81 03";
                                    //TODOMore 多车设置 循环发送
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        //按照协议发送到 不同Telnet_Session 
                                        ST_SendToServers(vms[i], temp);
                                    }
                                    //置空 后面不设置
                                    temp = "";
                                }
                            }

                        }
                    }
                    #endregion
                    #region G12-停车报警 √
                    else if (qqzl.Equals("G12")) //停车报警
                    {
                        //获取系统编号与数值集合  13500000001-13500000002,600000,2-11
                        string[] sendstr = yj.Split(',');
                        string[] sysnos = sendstr[0].Split('-');

                        //判断 终端型号[根据协议]
                        vehicleMode = qqstr.Split('_')[2].ToString();
                        string[] vms = vehicleMode.Split('-');
                        if (!string.IsNullOrEmpty(vehicleMode))
                        {
                            //循环发送
                            for (int i = 0; i < sysnos.Length; i++)
                            {
                                if (vms[i].Equals("1") || vms[i].Equals("11"))//9.0 TCP  || UDP
                                {
                                    string cs = Convert.ToString(int.Parse(sendstr[1].ToString()), 16).ToString();
                                    string yhzfc = string.Format("40 00 07 {0} {1}", SysNoToIp(sysnos[i].ToString()), GetFomartZ(cs, 2));
                                    temp = string.Format("24 24 {0} {1} 0D", yhzfc, GetJy(yhzfc.Split(' ')));

                                    index = "|40";
                                    //TODOMore 多车设置 循环发送
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        //按照协议发送到 不同Telnet_Session 
                                        ST_SendToServers(vms[i], temp);
                                    }
                                    //置空 后面不设置
                                    temp = "";
                                }
                                else if (vms[i].Equals("2"))//2011/2013
                                {
                                    string cs = Convert.ToString(int.Parse(sendstr[1].ToString()) * 60, 16).ToString();
                                    string yhzfc = string.Format("81 03 00 08 {0} 00 5A 01 00 5A 04 {1}", GetFomartSysno(sysnos[i].ToString()), GetFomartZ(cs, 8));
                                    temp = string.Format("7E {0} {1} 7E", yhzfc, GetJy(yhzfc.Split(' ')));
                                    index = "|81 03";
                                    //TODOMore 多车设置 循环发送
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        //按照协议发送到 不同Telnet_Session 
                                        ST_SendToServers(vms[i], temp);
                                    }
                                    //置空 后面不设置
                                    temp = "";
                                }
                            }

                        }
                    }
                    #endregion
                    #region G13-超速报警 √
                    else if (qqzl.Equals("G13"))//超速报警
                    {
                        //获取系统编号与数值集合  13500000001-13500000002,600000,2-11
                        string[] sendstr = yj.Split(',');
                        string[] sysnos = sendstr[0].Split('-');

                        //判断 终端型号[根据协议]
                        vehicleMode = qqstr.Split('_')[2];
                        string[] vms = vehicleMode.Split('-');
                        if (!string.IsNullOrEmpty(vehicleMode))
                        {
                            //循环发送
                            for (int i = 0; i < sysnos.Length; i++)
                            {
                                if (vms[i].Equals("1") || vms[i].Equals("11"))//9.0 TCP  || UDP
                                {
                                    string cs = Convert.ToString(int.Parse(sendstr[1]), 16);
                                    string yhzfc = string.Format("3F 00 07 {0} {1}", SysNoToIp(sysnos[i].ToString()), GetFomartZ(cs, 2));
                                    temp = string.Format("24 24 {0} {1} 0D", yhzfc, GetJy(yhzfc.Split(' ')));

                                    index = "|3F";
                                    //TODOMore 多车设置 循环发送
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        //按照协议发送到 不同Telnet_Session 
                                        ST_SendToServers(vms[i], temp);
                                    }
                                    //置空 后面不设置
                                    temp = "";
                                }
                                else if (vms[i].Equals("2"))//2011/2013
                                {

                                    string cs = Convert.ToString(int.Parse(sendstr[1].ToString()) * 10, 16).ToString();
                                    string yhzfc = string.Format("81 03 00 08 {0} 00 55 01 00 55 04 {1}", GetFomartSysno(sysnos[i].ToString()), GetFomartZ(cs, 8));
                                    temp = string.Format("7E {0} {1} 7E", yhzfc, GetJy(yhzfc.Split(' ')));
                                    index = "|81 03";
                                    //TODOMore 多车设置 循环发送
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        //按照协议发送到 不同Telnet_Session 
                                        ST_SendToServers(vms[i], temp);
                                    }
                                    //置空 后面不设置
                                    temp = "";
                                }
                            }

                        }
                    }
                    #endregion
                    #region 088,地标 - 分时租赁 取车还车 √
                    #region G16-//开门 下发指令  关闭油路
                    else if (qqzl.Equals("G16"))//开门 下发指令  关闭油路
                    {
                        //判断 终端型号
                        vehicleMode = qqstr.Split('_')[2].ToString();
                        if (!string.IsNullOrEmpty(vehicleMode))
                        {
                            if (vehicleMode.Equals("1") || vehicleMode.Equals("11"))
                            {
                                //9.0
                                string yhzfc = string.Format("39 00 06 {0}", SysNoToIp(yj));
                                temp = string.Format("24 24 {0} {1} 0D", yhzfc, GetJy(yhzfc.Split(' ')));

                                index = "|39";
                            }
                            else if (vehicleMode.Equals("2"))
                            {
                                //088
                                string cs = Convert.ToString(1, 16).ToString();  // 00 01 流水号  01是指控制
                                string yhzfc = string.Format("8F 00 00 02 {0} 00 01 01 {1}", GetFomartSysno(yj), GetFomartZ(cs, 2));
                                temp = string.Format("7E {0} {1} 7E", yhzfc, GetJy(yhzfc.Split(' ')));
                                index = "|8F 00";

                            }
                            else if (vehicleMode.Equals("4"))
                            {
                                //地标
                                string cs = Convert.ToString(1, 16).ToString();
                                DateTime dtnow = DateTime.Now;
                                string time = string.Format("{0} {1} {2} {3} {4} {5}", PublicMethods.Get10To16(dtnow.Year.ToString().Substring(2, 2), 1), PublicMethods.Get10To16(dtnow.Month.ToString(), 1), PublicMethods.Get10To16(dtnow.Day.ToString(), 1), PublicMethods.Get10To16(dtnow.Hour.ToString(), 1), PublicMethods.Get10To16(dtnow.Minute.ToString(), 1), PublicMethods.Get10To16(dtnow.Second.ToString(), 1));
                                //消息体长度为11 +2(验证发送方)=13       //时间+命令id+控制命令+命令时间+流水号      回复 命令+流水号
                                string yhzfc = string.Format("82 FE {0} 00 00 0D {1} 87 01 {2} 00 01", PublicMethods.GetAsciiToHex(yj), time, GetFomartZ(cs, 2));
                                temp = string.Format("23 23 {0} {1}", yhzfc, GetJy(yhzfc.Split(' ')));
                                //设置时间,总数,id,参数,id,参数
                                index = "|82 FE";
                            }
                        }

                        //判断是否

                    }
                    #endregion
                    #region G17-//关门 下发指令
                    else if (qqzl.Equals("G17"))
                    {
                        //判断 终端型号
                        vehicleMode = qqstr.Split('_')[2].ToString();
                        if (!string.IsNullOrEmpty(vehicleMode))
                        {
                            if (vehicleMode.Equals("1") || vehicleMode.Equals("11"))
                            {
                                //9.0
                                string yhzfc = string.Format("38 00 06 {0}", SysNoToIp(yj));
                                temp = string.Format("24 24 {0} {1} 0D", yhzfc, GetJy(yhzfc.Split(' ')));

                                index = "|38";
                            }
                            else if (vehicleMode.Equals("2"))
                            {
                                //088
                                string cs = Convert.ToString(1, 16).ToString();
                                string yhzfc = string.Format("8F 00 00 02 {0} 00 02 02 {1}", GetFomartSysno(yj), GetFomartZ(cs, 2));
                                temp = string.Format("7E {0} {1} 7E", yhzfc, GetJy(yhzfc.Split(' ')));
                                index = "|8F 00";

                            }
                            else if (vehicleMode.Equals("4"))
                            {
                                //地标
                                string cs = Convert.ToString(1, 16).ToString();
                                DateTime dtnow = DateTime.Now;
                                string time = string.Format("{0} {1} {2} {3} {4} {5}", PublicMethods.Get10To16(dtnow.Year.ToString().Substring(2, 2), 1), PublicMethods.Get10To16(dtnow.Month.ToString(), 1), PublicMethods.Get10To16(dtnow.Day.ToString(), 1), PublicMethods.Get10To16(dtnow.Hour.ToString(), 1), PublicMethods.Get10To16(dtnow.Minute.ToString(), 1), PublicMethods.Get10To16(dtnow.Second.ToString(), 1));
                                //消息体长度为11 +2(验证发送方)=13       //时间+命令id+控制命令+命令时间+流水号      回复 命令+流水号
                                string yhzfc = string.Format("82 FE {0} 00 00 0D {1} 87 02 {2} 00 02", PublicMethods.GetAsciiToHex(yj), time, GetFomartZ(cs, 2));
                                temp = string.Format("23 23 {0} {1}", yhzfc, GetJy(yhzfc.Split(' ')));
                                //设置时间,总数,id,参数,id,参数
                                index = "|82 FE";
                            }
                        }
                    }
                    #endregion
                    #region G160-//紧指开门 下发指令
                    else if (qqzl.Equals("G160"))
                    {
                        //判断 终端型号
                        vehicleMode = qqstr.Split('_')[2].ToString();
                        if (!string.IsNullOrEmpty(vehicleMode))
                        {
                            if (vehicleMode.Equals("1") || vehicleMode.Equals("11"))
                            {
                                //9.0
                                //string yhzfc = string.Format("39 00 06 {0}", SysNoToIp(yj));
                                //temp = string.Format("24 24 {0} {1} 0D", yhzfc, GetJy(yhzfc.Split(' ')));

                                //index = "|39";
                            }
                            else if (vehicleMode.Equals("2"))
                            {
                                //088
                                string cs = Convert.ToString(1, 16).ToString();
                                //string yhzfc = string.Format("8F 00 00 02 {0} 00 03 03 {1}", GetFomartSysno(yj), GetFomartZ(cs, 2));
                                //BMK 2035-10-11 修改为 开门同样发送取车指令  00 03 03-> 流水号(用作回复标识),指令
                                string yhzfc = string.Format("8F 00 00 02 {0} 00 03 01 {1}", GetFomartSysno(yj), GetFomartZ(cs, 2));
                                temp = string.Format("7E {0} {1} 7E", yhzfc, GetJy(yhzfc.Split(' ')));
                                index = "|8F 00";

                            }
                            else if (vehicleMode.Equals("4"))
                            {
                                //地标
                                string cs = Convert.ToString(1, 16).ToString();
                                DateTime dtnow = DateTime.Now;
                                string time = string.Format("{0} {1} {2} {3} {4} {5}", PublicMethods.Get10To16(dtnow.Year.ToString().Substring(2, 2), 1), PublicMethods.Get10To16(dtnow.Month.ToString(), 1), PublicMethods.Get10To16(dtnow.Day.ToString(), 1), PublicMethods.Get10To16(dtnow.Hour.ToString(), 1), PublicMethods.Get10To16(dtnow.Minute.ToString(), 1), PublicMethods.Get10To16(dtnow.Second.ToString(), 1));
                                //消息体长度为11 +2(验证发送方)=13       //时间+命令id+控制命令+命令时间+流水号      回复 命令+流水号
                                string yhzfc = string.Format("82 FE {0} 00 00 0D {1} 87 01 {2} 00 03", PublicMethods.GetAsciiToHex(yj), time, GetFomartZ(cs, 2));
                                temp = string.Format("23 23 {0} {1}", yhzfc, GetJy(yhzfc.Split(' ')));
                                //设置时间,总数,id,参数,id,参数
                                index = "|82 FE";
                            }
                        }

                        //判断是否

                    }
                    #endregion
                    #region G170-//紧指关门 下发指令
                    else if (qqzl.Equals("G170"))
                    {
                        //判断 终端型号
                        vehicleMode = qqstr.Split('_')[2].ToString();
                        if (!string.IsNullOrEmpty(vehicleMode))
                        {
                            if (vehicleMode.Equals("1") || vehicleMode.Equals("11"))
                            {
                                //9.0
                                //string yhzfc = string.Format("38 00 06 {0}", SysNoToIp(yj));
                                //temp = string.Format("24 24 {0} {1} 0D", yhzfc, GetJy(yhzfc.Split(' ')));

                                //index = "|38";
                            }
                            else if (vehicleMode.Equals("2"))
                            {
                                //088
                                string cs = Convert.ToString(1, 16).ToString();
                                string yhzfc = string.Format("8F 00 00 02 {0} 00 04 04 {1}", GetFomartSysno(yj), GetFomartZ(cs, 2));
                                temp = string.Format("7E {0} {1} 7E", yhzfc, GetJy(yhzfc.Split(' ')));
                                index = "|8F 00";

                            }
                            else if (vehicleMode.Equals("4"))
                            {
                                //地标
                                 string cs = Convert.ToString(1, 16).ToString();
                                DateTime dtnow = DateTime.Now;
                                string time = string.Format("{0} {1} {2} {3} {4} {5}", PublicMethods.Get10To16(dtnow.Year.ToString().Substring(2, 2), 1), PublicMethods.Get10To16(dtnow.Month.ToString(), 1), PublicMethods.Get10To16(dtnow.Day.ToString(), 1), PublicMethods.Get10To16(dtnow.Hour.ToString(), 1), PublicMethods.Get10To16(dtnow.Minute.ToString(), 1), PublicMethods.Get10To16(dtnow.Second.ToString(), 1));
                                //消息体长度为11 +2(验证发送方)=13       //时间+命令id+控制命令+命令时间+流水号      回复 命令+流水号
                                string yhzfc = string.Format("82 FE {0} 00 00 0D {1} 87 04 {2} 00 04", PublicMethods.GetAsciiToHex(yj), time,  GetFomartZ(cs, 2));
                                temp = string.Format("23 23 {0} {1}", yhzfc, GetJy(yhzfc.Split(' ')));
                                //设置时间,总数,id,参数,id,参数
                                index = "|82 FE";
                            }
                        }
                    }
                    #endregion
                    #endregion
                    #region G19- GPS上传时间设置
                    else if (qqzl.Equals("G19"))
                    {
                        //获取系统编号与数值集合  13500000001-13500000002,60,2-11
                        string[] sendstr = yj.Split(',');
                        string[] sysnos = sendstr[0].Split('-');

                        //判断 终端型号[根据协议]
                        vehicleMode = qqstr.Split('_')[2].ToString();
                        string[] vms = vehicleMode.Split('-');
                        if (!string.IsNullOrEmpty(vehicleMode))
                        {
                            //循环发送
                            for (int i = 0; i < sysnos.Length; i++)
                            {
                                if (vms[i].Equals("1") || vms[i].Equals("11"))//9.0 TCP  || UDP
                                {
                                    string cs = Convert.ToString(int.Parse(sendstr[1]), 16);
                                    string csb = Convert.ToString(int.Parse(sendstr[2]), 16);
                                    string yhzfc = string.Format("34 00 0A {0} {1}", SysNoToIp(sysnos[i].ToString()), GetFomartZ(cs, 2), GetFomartZ(csb, 2));
                                    temp = string.Format("24 24 {0} {1} 0D", yhzfc, GetJy(yhzfc.Split(' ')));

                                    index = "|34";

                                    //TODOMore 多车设置 循环发送
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        //按照协议发送到 不同Telnet_Session 
                                        ST_SendToServers(vms[i], temp);
                                    }
                                    //置空 后面不设置
                                    temp = "";
                                }

                                if (vms[i].Equals("2"))//2011/2013
                                {
                                    string cs = Convert.ToString(int.Parse(sendstr[1].ToString()), 16).ToString().ToUpper();
                                    //29 缺省时间汇报间隔 单位为秒 >0
                                    //00 29 01 00 29 04 {1}
                                    string yhzfc = string.Format("81 03 00 08 {0} 00 29 01 00 29 04 {1}", GetFomartSysno(sysnos[i].ToString()), GetFomartZ(cs, 8));
                                    temp = string.Format("7E {0} {1} 7E", yhzfc, GetJy(yhzfc.Split(' ')));
                                    index = "|81 03";

                                    //TODOMore 多车设置 循环发送
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        //按照协议发送到 不同Telnet_Session 
                                        ST_SendToServers(vms[i], temp);
                                    }
                                    //置空 后面不设置
                                    temp = "";
                                }
                                if (vms[i].Equals("4")) //地标
                                {
                                    DateTime dtnow = DateTime.Now;
                                    string time = string.Format("{0} {1} {2} {3} {4} {5}", PublicMethods.Get10To16(dtnow.Year.ToString().Substring(2, 2), 1), PublicMethods.Get10To16(dtnow.Month.ToString(), 1), PublicMethods.Get10To16(dtnow.Day.ToString(), 1), PublicMethods.Get10To16(dtnow.Hour.ToString(), 1), PublicMethods.Get10To16(dtnow.Minute.ToString(), 1), PublicMethods.Get10To16(dtnow.Second.ToString(), 1));
                                    string indexk = "02";
                                    string indexv = sendstr[1];
                                    int blength = 7 + 1; string sendvalue = "";
                                    blength += 2;
                                    sendvalue = PublicMethods.Get10To16(indexv, 2);
                                    blength += 2;//发送标识

                                    //消息体长度为15 +2(验证发送方)=17
                                    //系统编号,长度,时间,参数id,值
                                    string yhzfc = string.Format("81 FE {0} 00 {1} {2} 01 {3} {4}", PublicMethods.GetAsciiToHex(sysnos[i].ToString()), PublicMethods.Get10To16(blength.ToString(), 2), time, indexk, sendvalue);
                                    temp = string.Format("23 23 {0} {1}", yhzfc, GetJy(yhzfc.Split(' ')));
                                    //设置时间,总数,id,参数,id,参数

                                    //TODOMore 多车设置 循环发送
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        //按照协议发送到 不同Telnet_Session 
                                        ST_SendToServers(vms[i], temp);
                                    }
                                    //置空 后面不设置
                                    temp = "";
                                }

                            }
                        }
                    }
                    #endregion
                    #region G22- 设置 综合平台IP 地址, 综合平台端口
                    else if (qqzl.Equals("G22"))
                    {
                        //获取系统编号与数值集合  G22  _ 13500000001-13500000002 , 111.175.187.206:2013  _  2-11
                        string[] sendstr = yj.Split(',');
                        string[] sysnos = sendstr[0].Split('-');

                        //判断 终端型号[根据协议]
                        vehicleMode = qqstr.Split('_')[2].ToString();
                        string[] vms = vehicleMode.Split('-');
                        if (!string.IsNullOrEmpty(vehicleMode))
                        {
                            //循环发送
                            for (int i = 0; i < sysnos.Length; i++)
                            {
                                if (vms[i].Equals("4")) //地标
                                {
                                    DateTime dtnow = DateTime.Now;
                                    string time = string.Format("{0} {1} {2} {3} {4} {5}", PublicMethods.Get10To16(dtnow.Year.ToString().Substring(2, 2), 1), PublicMethods.Get10To16(dtnow.Month.ToString(), 1), PublicMethods.Get10To16(dtnow.Day.ToString(), 1), PublicMethods.Get10To16(dtnow.Hour.ToString(), 1), PublicMethods.Get10To16(dtnow.Minute.ToString(), 1), PublicMethods.Get10To16(dtnow.Second.ToString(), 1));
                                    string[] ipv4 = sendstr[1].Split(':')[0].Split('.');
                                    string ip = string.Format("00 00 {0} {1} {2} {3}", PublicMethods.Get10To16(ipv4[0], 1), PublicMethods.Get10To16(ipv4[1], 1), PublicMethods.Get10To16(ipv4[2], 1), PublicMethods.Get10To16(ipv4[3], 1));
                                    string port = PublicMethods.Get10To16(sendstr[1].Split(':')[1], 2);
                                    //消息体长度为17 +2(验证发送方)=19
                                    string yhzfc = string.Format("81 FE {0} 00 00 13 {1} 02 04 {2} 05 {3}", PublicMethods.GetAsciiToHex(sysnos[i].ToString()), time, ip, port);
                                    temp = string.Format("23 23 {0} {1}", yhzfc, GetJy(yhzfc.Split(' ')));
                                    //设置时间,总数,id,参数,id,参数

                                    //TODOMore 多车设置 循环发送
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        //按照协议发送到 不同Telnet_Session 
                                        ST_SendToServers(vms[i], temp);
                                    }
                                    //置空 后面不设置
                                    temp = "";
                                }

                            }
                        }
                    }
                    #endregion
                    #region G23- 车载终端控制命令 远程升级
                    else if (qqzl.Equals("G23"))
                    {
                        //获取系统编号与数值集合  G23  _ 13500000001-13500000002 , 111.175.187.206:2013  _  2-11
                        string[] sendstr = yj.Split(',');
                        string[] sysnos = sendstr[0].Split('-');

                        //判断 终端型号[根据协议]
                        vehicleMode = qqstr.Split('_')[2].ToString();
                        string[] vms = vehicleMode.Split('-');
                        if (!string.IsNullOrEmpty(vehicleMode))
                        {
                            //循环发送
                            for (int i = 0; i < sysnos.Length; i++)
                            {
                                if (vms[i].Equals("4")) //地标
                                {
                                    DateTime dtnow = DateTime.Now;
                                    string time = string.Format("{0} {1} {2} {3} {4} {5}", PublicMethods.Get10To16(dtnow.Year.ToString().Substring(2, 2), 1), PublicMethods.Get10To16(dtnow.Month.ToString(), 1), PublicMethods.Get10To16(dtnow.Day.ToString(), 1), PublicMethods.Get10To16(dtnow.Hour.ToString(), 1), PublicMethods.Get10To16(dtnow.Minute.ToString(), 1), PublicMethods.Get10To16(dtnow.Second.ToString(), 1));


                                    //系统标识,包长,时间,参数
                                    // string url = string.Format("http://{0}/upgrade;", sendstr[1]);
                                    //BMK 2035-12-02 修改为动态目录
                                    // string url = string.Format("http://{0};", sendstr[1]);
                                    //BMK 2035-12-18 修改为ftp, 所以 改为全部动态
                                    string url = string.Format("{0};", sendstr[1]);
                                    url = PublicMethods.GetAsciiToHex(url);
                                    int blength = 7 + url.Split(' ').Count() + 2;//7个固定+ascii+本系统的发送标识
                                    string yhzfc = string.Format("82 FE {0} 00 {1} {2} 01 {3}", PublicMethods.GetAsciiToHex(sysnos[i].ToString()), PublicMethods.Get10To16(blength.ToString(), 2), time, url);
                                    temp = string.Format("23 23 {0} {1}", yhzfc, GetJy(yhzfc.Split(' ')));


                                    //TODOMore 多车设置 循环发送
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        //按照协议发送到 不同Telnet_Session 
                                        ST_SendToServers(vms[i], temp);
                                    }
                                    //置空 后面不设置
                                    temp = "";
                                }

                            }
                        }
                    }
                    #endregion
                    #region G24- 车载终端控制命令-无参 (车载终端关机,车载终端复位,车载设备恢复出厂设置,断开数据通讯链路)
                    else if (qqzl.Equals("G24"))
                    {
                        //获取系统编号与数值集合  G24  _ 13500000001-13500000002 ,  2  _  2-11
                        string[] sendstr = yj.Split(',');
                        string[] sysnos = sendstr[0].Split('-');

                        //判断 终端型号[根据协议]
                        vehicleMode = qqstr.Split('_')[2].ToString();
                        string[] vms = vehicleMode.Split('-');
                        if (!string.IsNullOrEmpty(vehicleMode))
                        {
                            //循环发送
                            for (int i = 0; i < sysnos.Length; i++)
                            {
                                if (vms[i].Equals("4")) //地标
                                {
                                    DateTime dtnow = DateTime.Now;
                                    string time = string.Format("{0} {1} {2} {3} {4} {5}", PublicMethods.Get10To16(dtnow.Year.ToString().Substring(2, 2), 1), PublicMethods.Get10To16(dtnow.Month.ToString(), 1), PublicMethods.Get10To16(dtnow.Day.ToString(), 1), PublicMethods.Get10To16(dtnow.Hour.ToString(), 1), PublicMethods.Get10To16(dtnow.Minute.ToString(), 1), PublicMethods.Get10To16(dtnow.Second.ToString(), 1));

                                    //消息体长度为7 +2(验证发送方)=9
                                    string yhzfc = string.Format("82 FE {0} 00 00 09 {1} {2}", PublicMethods.GetAsciiToHex(sysnos[i].ToString()), time, sendstr[1].PadLeft(2, '0'));
                                    temp = string.Format("23 23 {0} {1}", yhzfc, GetJy(yhzfc.Split(' ')));
                                    //设置时间,总数,id,参数,id,参数

                                    //TODOMore 多车设置 循环发送
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        //按照协议发送到 不同Telnet_Session 
                                        ST_SendToServers(vms[i], temp);
                                    }
                                    //置空 后面不设置
                                    temp = "";
                                }

                            }
                        }
                    }
                    #endregion
                    #region G25- 参数设置
                    //(车载终端本地存储时间周期 1,正常时信息上报时间周期 2,出现报警时信息上报时间周期 3, 硬件版本 6, 固件版本 7, 车载终端心跳发送周期 8 ,终端应答超时时间 9 ,平台应答超时时间 A)
                    else if (qqzl.Equals("G25"))
                    {
                        //获取系统编号与数值集合  G25  _ 13500000001-13500000002 ,1-6000 _  2-11
                        string[] sendstr = yj.Split(',');
                        string[] sysnos = sendstr[0].Split('-');

                        //判断 终端型号[根据协议]
                        vehicleMode = qqstr.Split('_')[2].ToString();
                        string[] vms = vehicleMode.Split('-');
                        if (!string.IsNullOrEmpty(vehicleMode))
                        {
                            //循环发送
                            for (int i = 0; i < sysnos.Length; i++)
                            {
                                if (vms[i].Equals("4")) //地标
                                {
                                    DateTime dtnow = DateTime.Now;
                                    string time = string.Format("{0} {1} {2} {3} {4} {5}", PublicMethods.Get10To16(dtnow.Year.ToString().Substring(2, 2), 1), PublicMethods.Get10To16(dtnow.Month.ToString(), 1), PublicMethods.Get10To16(dtnow.Day.ToString(), 1), PublicMethods.Get10To16(dtnow.Hour.ToString(), 1), PublicMethods.Get10To16(dtnow.Minute.ToString(), 1), PublicMethods.Get10To16(dtnow.Second.ToString(), 1));
                                    string indexk = PublicMethods.Get10To16(sendstr[1].Split('-')[0], 1);
                                    string indexv = sendstr[1].Split('-')[1];
                                    int blength = 7 + 1; string sendvalue = "";
                                    if (indexk.Equals("06") || indexk.Equals("07"))//5个字节
                                    {
                                        blength += 5;
                                        sendvalue = PublicMethods.GetAsciiToHex(indexv);
                                    }
                                    else if (indexk.Equals("08"))//1个字节
                                    {
                                        blength += 1;
                                        sendvalue = PublicMethods.Get10To16(indexv, 1);
                                    }
                                    else//2个字节
                                    {
                                        blength += 2;
                                        sendvalue = PublicMethods.Get10To16(indexv, 2);
                                    }

                                    blength += 2;//发送标识

                                    //消息体长度为15 +2(验证发送方)=17
                                    //系统编号,长度,时间,参数id,值
                                    string yhzfc = string.Format("81 FE {0} 00 {1} {2} 01 {3} {4}", PublicMethods.GetAsciiToHex(sysnos[i].ToString()), PublicMethods.Get10To16(blength.ToString(), 2), time, indexk, sendvalue);
                                    temp = string.Format("23 23 {0} {1}", yhzfc, GetJy(yhzfc.Split(' ')));
                                    //设置时间,总数,id,参数,id,参数

                                    //TODOMore 多车设置 循环发送
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        //按照协议发送到 不同Telnet_Session 
                                        ST_SendToServers(vms[i], temp);
                                    }
                                    //置空 后面不设置
                                    temp = "";
                                }

                            }
                        }
                    }
                    #endregion
                    #region G26- 参数查询
                    //(车载终端本地存储时间周期 1,正常时信息上报时间周期 2,出现报警时信息上报时间周期 3, 硬件版本 6, 固件版本 7, 车载终端心跳发送周期 8 ,终端应答超时时间 9 ,平台应答超时时间 A)
                    else if (qqzl.Equals("G26"))
                    {
                        //获取系统编号与数值集合  G26  _ 13500000001-13500000002 ,1-2 _  2-11
                        string[] sendstr = yj.Split(',');
                        string[] sysnos = sendstr[0].Split('-');

                        //判断 终端型号[根据协议]
                        vehicleMode = qqstr.Split('_')[2].ToString();
                        string[] vms = vehicleMode.Split('-');
                        if (!string.IsNullOrEmpty(vehicleMode))
                        {
                            //循环发送
                            for (int i = 0; i < sysnos.Length; i++)
                            {
                                if (vms[i].Equals("4")) //地标
                                {
                                    DateTime dtnow = DateTime.Now;
                                    string time = string.Format("{0} {1} {2} {3} {4} {5}", PublicMethods.Get10To16(dtnow.Year.ToString().Substring(2, 2), 1), PublicMethods.Get10To16(dtnow.Month.ToString(), 1), PublicMethods.Get10To16(dtnow.Day.ToString(), 1), PublicMethods.Get10To16(dtnow.Hour.ToString(), 1), PublicMethods.Get10To16(dtnow.Minute.ToString(), 1), PublicMethods.Get10To16(dtnow.Second.ToString(), 1));
                                    string[] indexk = sendstr[1].Split('-');
                                    string countCs = PublicMethods.Get10To16(indexk.Count().ToString(), 1);
                                    string sendvalue = "";
                                    int blength = 7;//
                                    for (int j = 0; j < indexk.Count(); j++)
                                    {
                                        sendvalue += PublicMethods.Get10To16(indexk[j], 1) + " ";
                                        blength += 1;
                                    }
                                    sendvalue = sendvalue.Trim();

                                    blength += 2;//发送标识

                                    //消息体长度为 +2(验证发送方)=
                                    //系统编号,长度,时间,参数总数,参数id
                                    string yhzfc = string.Format("80 FE {0} 00 {1} {2} {3} {4}", PublicMethods.GetAsciiToHex(sysnos[i].ToString()), PublicMethods.Get10To16(blength.ToString(), 2), time, countCs, sendvalue);
                                    temp = string.Format("23 23 {0} {1}", yhzfc, GetJy(yhzfc.Split(' ')));
                                    //设置时间,总数,id,参数,id,参数

                                    //TODOMore 多车设置 循环发送
                                    if (!string.IsNullOrEmpty(temp))
                                    {
                                        //按照协议发送到 不同Telnet_Session 
                                        ST_SendToServers(vms[i], temp);
                                    }
                                    //置空 后面不设置
                                    temp = "";
                                }
                            }

                        }
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(temp))
                    {
                        //按照协议发送到 不同Telnet_Session 
                        ST_SendToServers(vehicleMode, temp);
                    }


                }
                catch (Exception ex)
                {

                    WriteLog.WriteErrorLog("ErrorAS", "封装指令出错:" + ex.Message.ToString() + "\r\n原始:" + msg + "\r\n封装:" + qqstr, true);
                }
            }
        }

        /// <summary>
        /// Gets the date STR3.
        /// </summary>
        /// <param name="a">A.</param>
        /// <returns>System.String.</returns>
        public static string GetDateStr3(string a)
        {
            string temp = "";
            try
            {
                CommonJsonModel model = DeSerialize(a);
                string identifying = model.GetValue("identifying");
                switch (identifying)
                {
                    case "G03":
                        {
                            //车辆在线统计
                            temp = identifying + "_" + model.GetModel("data").GetValue("sysnos").Replace('_', ',') + "_" + model.GetValue("sender");
                            break;
                        }
                    case "G04":
                        {
                            //点名
                            temp = identifying + "_" + model.GetModel("data").GetValue("sysnos") + "_" + model.GetModel("data").GetValue("deviceProtocolId");
                            break;
                        }
                    case "G05":
                        {
                            //查看状态
                            temp = identifying + "_" + model.GetModel("data").GetValue("sysnos") + "_" + model.GetModel("data").GetValue("deviceProtocolId");
                            break;
                        }
                    case "G10":
                        {
                            //里程重设
                            temp = identifying + "_" + string.Format("{0},{1}", model.GetModel("data").GetValue("sysnos").Replace('_', '-'), model.GetModel("data").GetValue("numerical")) + "_" + model.GetModel("data").GetValue("deviceProtocolId").Replace('_', '-');
                            break;
                        }
                    case "G11":
                        {
                            //疲劳驾驶设置
                            temp = identifying + "_" + string.Format("{0},{1},{2}", model.GetModel("data").GetValue("sysnos").Replace('_', '-'), model.GetModel("data").GetValue("numerical"), model.GetModel("data").GetValue("Buffer")) + "_" + model.GetModel("data").GetValue("deviceProtocolId").Replace('_', '-');
                            break;
                        }
                    case "G12":
                        {
                            //超时停车设置
                            temp = identifying + "_" + string.Format("{0},{1}", model.GetModel("data").GetValue("sysnos").Replace('_', '-'), model.GetModel("data").GetValue("numerical")) + "_" + model.GetModel("data").GetValue("deviceProtocolId").Replace('_', '-');
                            break;
                        }
                    case "G13":
                        {
                            //超速报警设置
                            temp = identifying + "_" + string.Format("{0},{1}", model.GetModel("data").GetValue("sysnos").Replace('_', '-'), model.GetModel("data").GetValue("numerical")) + "_" + model.GetModel("data").GetValue("deviceProtocolId").Replace('_', '-');
                            break;
                        }
                    //case "G14":  2035-04-30  修改为 终端参数设置回执
                    //    {
                    //        //参数设置
                    //        temp = identifying + "_" + string.Format("{0},{1}", model.GetModel("data").GetValue("sysnos"), model.GetModel("data").GetValue("numerical")) + "_" + model.GetModel("data").GetValue("deviceProtocolId");
                    //        break;
                    //    }
                    case "G16":   //开门  油路控制
                    case "G17":      //关门  油路控制
                    case "G160":       //开门
                    case "G170":     //关门
                        {
                            //    temp = identifying + "_" + model.GetModel("data").GetValue("systemNo") + "_" + model.GetModel("data").GetValue("deviceProtocolId");
                            temp = identifying + "_" + model.GetModel("data").GetValue("systemNo") + "_" + model.GetModel("data").GetValue("deviceProtocolId");
                            break;
                        }
                    //case "G16":
                    //    {
                    //        //开门  油路控制
                    //        //    temp = identifying + "_" + model.GetModel("data").GetValue("systemNo") + "_" + model.GetModel("data").GetValue("deviceProtocolId");
                    //        temp = identifying + "_" + model.GetModel("data").GetValue("systemNo") + "_4";
                    //        break;
                    //    }
                    //case "G17":
                    //    {
                    //        //关门  油路控制
                    //        // temp = identifying + "_" + model.GetModel("data").GetValue("systemNo") + "_" + model.GetModel("data").GetValue("deviceProtocolId");
                    //        temp = identifying + "_" + model.GetModel("data").GetValue("systemNo") + "_4";
                    //        break;
                    //    }
                    //case "G160":
                    //    {
                    //        //开门
                    //        temp = identifying + "_" + model.GetModel("data").GetValue("systemNo") + "_4";
                    //        break;
                    //    }
                    //case "G170":
                    //    {
                    //        //关门
                    //        temp = identifying + "_" + model.GetModel("data").GetValue("systemNo") + "_4";
                    //        break;
                    //    }
                    case "G19":
                        {
                            //GPS上传时间间隔设置
                            //temp = identifying + "_" + string.Format("{0},{1},{2}", model.GetModel("data").GetValue("sysnos").Replace('_', '-'), model.GetModel("data").GetValue("valueA"), model.GetModel("data").GetValue("valueB")) + "_2";
                            temp = identifying + "_" + string.Format("{0},{1}", model.GetModel("data").GetValue("sysnos").Replace('_', '-'), model.GetModel("data").GetValue("value")) + "_" + model.GetModel("data").GetValue("deviceProtocolId").Replace('_', '-');
                            break;
                        }
                    case "G22": //设置 综合平台IP 地址, 综合平台端口
                        {
                            temp = string.Format("{0}_{1}_{2}", identifying, string.Format("{0},{1}", model.GetModel("data").GetValue("sysnos").Replace('_', '-'), model.GetModel("data").GetValue("numerical")), model.GetModel("data").GetValue("deviceProtocolId").Replace('_', '-'));
                            break;
                        }
                    case "G23": //车载终端控制命令 有参(远程升级)
                        {
                            temp = string.Format("{0}_{1}_{2}", identifying, string.Format("{0},{1}", model.GetModel("data").GetValue("sysnos").Replace('_', '-'), model.GetModel("data").GetValue("numerical")), model.GetModel("data").GetValue("deviceProtocolId").Replace('_', '-'));
                            break;
                        }
                    case "G24": //车载终端控制命令 无参
                        {
                            temp = string.Format("{0}_{1}_{2}", identifying, string.Format("{0},{1}", model.GetModel("data").GetValue("sysnos").Replace('_', '-'), model.GetModel("data").GetValue("numerical")), model.GetModel("data").GetValue("deviceProtocolId").Replace('_', '-'));
                            break;
                        }
                    case "G25": //参数设置 (车载终端本地存储时间周期 1,正常时信息上报时间周期 2,出现报警时信息上报时间周期 3, 硬件版本 6, 固件版本 7, 车载终端心跳发送周期 8 ,终端应答超时时间 9 ,平台应答超时时间 A)
                        {
                            temp = string.Format("{0}_{1}_{2}", identifying, string.Format("{0},{1}", model.GetModel("data").GetValue("sysnos").Replace('_', '-'), model.GetModel("data").GetValue("numerical").Replace('_', '-')), model.GetModel("data").GetValue("deviceProtocolId").Replace('_', '-'));
                            break;
                        }
                    case "G26": //参数查询 (车载终端本地存储时间周期 1,正常时信息上报时间周期 2,出现报警时信息上报时间周期 3, 硬件版本 6, 固件版本 7, 车载终端心跳发送周期 8 ,终端应答超时时间 9 ,平台应答超时时间 A)
                        {
                            temp = string.Format("{0}_{1}_{2}", identifying, string.Format("{0},{1}", model.GetModel("data").GetValue("sysnos").Replace('_', '-'), model.GetModel("data").GetValue("numerical").Replace('_', '-')), model.GetModel("data").GetValue("deviceProtocolId").Replace('_', '-'));
                            break;
                        }
                    default:
                        {
                            temp = "default";
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteErrorLog("ErrorAS", "GetDateStr3:" + ex.Message.ToString(), true);
            }
            return temp;
        }

        /// <summary>
        /// Des the serialize.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns>CommonJsonModel.</returns>
        public static CommonJsonModel DeSerialize(string json)
        {
            return new CommonJsonModel(json);
        }
        #endregion


        #region 封装引用源
        /// <summary>
        /// 获取车辆 终端型号
        /// </summary>
        /// <param name="sysno">The sysno.</param>
        /// <returns>System.String.</returns>
        public static string GetVehicleMode(string sysno)
        {
            string info = "";
            //try
            //{
            //    using (SqlConnection conn = CSqlHelper.GetConnction())
            //    {
            //        string sql = string.Format("select Vehicle_Mode from Vehicles where SystemNo='{0}'", sysno);
            //        DataTable dt = CSqlHelper.GetDataSet(conn, sql).Tables[0];
            //        if (dt != null)
            //        {
            //            info = dt.Rows[0][0].ToString();
            //        }

            //    }
            //}
            //catch (Exception) { }

            return info;
        }

        /// <summary>
        /// 隔偶加空格|格式化 系统编号
        /// 13622222123 -&gt;01 36 22 22 21 23
        /// </summary>
        /// <param name="info">The info.</param>
        /// <returns>System.String.</returns>
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
        /// 格式化字符串, 补足0,添加空格 2 N(字节)
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="nums">位数 00 00-&gt;0000 4</param>
        /// <returns>System.String.</returns>
        public static string GetFomartZ(string info, int nums)
        {
            int num = nums - info.Length;
            string a = "";
            for (int i = 0; i < num; i++)
            {
                a += "0";
            }
            info = a + info;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < info.Length; ++i)
            {
                sb.Append(info[i]);
                if ((i + 1) % 2 == 0)
                    sb.Append(" ");
            }
            string result = sb.ToString().Substring(0, sb.Length - 1);
            return result;
        }

        /// <summary>
        /// 得到校验码
        /// </summary>
        /// <param name="infos">The infos.</param>
        /// <returns>System.String.</returns>
        public static string GetJy(string[] infos)
        {
            int a, b, x = 0;
            for (int i = 0; i < infos.Length; i++)
            {
                if (i != infos.Length - 1)
                {
                    if (i == 0)
                    {
                        a = Convert.ToInt32(infos[i].ToString(), 16);
                    }
                    else
                    {
                        a = x;
                    }
                    b = Convert.ToInt32(infos[i + 1].ToString(), 16);
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
        /// 系统编号转换伪ip
        /// 龙安9.0协议版
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>System.String.</returns>
        private static string SysNoToIp(string sysNo)
        {
            StringBuilder sb = new StringBuilder();
            sysNo = sysNo.Remove(0, 1);
            string[] group = { sysNo.Substring(0, 2), sysNo.Substring(2, 2), sysNo.Substring(4, 2), sysNo.Substring(6, 2), sysNo.Substring(8, 2) };
            string[] binaryGroup = new string[4];
            for (int i = 1; i < 5; i++)
            {
                binaryGroup[i - 1] = Convert.ToString(Convert.ToInt32(group[i]), 2).PadLeft(8, '0');
            }
            string temp = Convert.ToString(Convert.ToInt32(group[0]) - 30, 2).PadLeft(4, '0');
            for (int j = 0; j < 4; j++)
            {
                string supBit = (Convert.ToInt32(binaryGroup[j].Substring(0, 1)) + Convert.ToInt32(temp.Substring(j, 1))).ToString();
                string bindString = supBit + binaryGroup[j].Substring(1);
                sb.Append(string.Format("{0:x}", Convert.ToInt32(bindString, 2)).PadLeft(2, '0') + " ");
            }
            return sb.ToString(0, sb.Length - 1);
        }

        /// <summary>
        /// 封装消息体为null的指令
        /// </summary>
        /// <param name="str">80 01</param>
        /// <param name="sysno">The sysno.</param>
        /// <returns>System.String.</returns>
        public static string GetFomartNullStr(string str, string sysno)
        {
            string info = string.Format("{0} 00 00 {1} 00 01", str, GetFomartSysno(sysno));

            string jy = GetJy(info.Substring(0, info.Length).Split(' '));

            info = string.Format("{0} {1} {2} {3}", "7E", info, jy, "7E").ToUpper();

            return info;

        }

        /// <summary>
        /// Fomats to josn.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sendType">The sendType.</param>
        /// <param name="receiver">receiver</param>
        /// <param name="identifying">The identifying.</param>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        public static string FomatToJosn(string key, string sendType, string receiver, string identifying, string data)
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("{\"key\":\"" + key + "\",");
            buffer.Append("\"sendType\":\"" + sendType + "\",");
            buffer.Append("\"receiver\":\"" + receiver + "\",");
            //buffer.Append("\"timestamp\":\"" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\",");
            if (identifying.Equals("G03"))
            {
                buffer.Append("\"interChange\":\"1\",");
            }
            buffer.Append("\"identifying\":\"" + identifying + "\",");
            buffer.Append("\"data\":{");
            buffer.Append(data);
            buffer.Append("}");
            buffer.Append("}");
            return buffer.ToString();
        }

        /// <summary>
        /// 16进制字符串转字节数组
        /// </summary>
        /// <param name="hexString">The hex string.</param>
        /// <returns>System.Byte[][].</returns>
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


        #endregion



        /// <summary>
        /// online
        /// </summary>
        private static List<string> lis_online;
        /// <summary>
        /// online
        /// </summary>
        /// <value>The lis online.</value>
        public static List<string> LisOnline
        {
            get { return lis_online; }
            set { lis_online = value; }
        }



    }
}
