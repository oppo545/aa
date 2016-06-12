
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
    /// 使用ActiveMq 接收中心服务器下发的消息
    /// </summary>
    public class ActiveMqReceive
    {


        /// <summary>
        /// The is as connect false-&gt; not connect
        /// </summary>
        public static bool IsStConnect = false;

        /// <summary>
        /// The is as connect false-&gt; not connect
        /// </summary>
        public static bool IsASConnect = false;


        /// <summary>
        /// Socket是否创建完毕  防止多线程去创建SOCKET
        /// </summary>
        private static bool IsSingleCreate = false;
        /// <summary>
        /// 
        /// </summary>
        public static void CreateSocket()
        {
            try
            {
                if ((!IsASConnect) && !IsSingleCreate)
                {
                    IsSingleCreate = true;
                    Connect();
                    WriteLog.WriteErrorLog("ErrorAS", "Connect正常", true);
                }
                IsSingleCreate = false;
            }
            catch (Exception exc)
            {
                WriteLog.WriteErrorLog("ErrorAS", "______创建异常:" + exc.Message, true);
            }
        }


        /// <summary>
        /// consumer_Listener active 
        /// </summary>
        /// <param name="message"></param>
        static void consumer_Listener(IMessage message)
        {
            ITextMessage msg = (ITextMessage)message;

            if (IsASConnect)
            {
                WriteLog.WriteSendLog("ReceiveReadyA");
                //接收中心服务器数据,分析,发送给车载终端
                AnalysisServer.ST_MsgToClient(msg.Text);
            }

        }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        public static void Connect()
        {
            try
            {
                IsASConnect = true;
                createActiveMQReceive();
            }
            catch (Exception ex)
            {
                IsASConnect = false;
                WriteLog.WriteErrorLog("ErrorAS", "______打开消息队列链接异常:" + ex.Message, true);
            }


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
            //通过会话创建一个消费者，这里就是Queue这种会话类型的监听参数设置    //CentralCommand
            IMessageConsumer consumer = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue("CentralCommand"));

            //注册监听事件 1  下发指令测试软件
            consumer.Listener += new MessageListener(consumer_Listener);

            //智能充电下发
            IMessageConsumer consumer1 = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue("chargeSendQueue"));
            consumer1.Listener += new MessageListener(consumer_Listener);

            //分时租赁
            IMessageConsumer consumer2 = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue("FSZL_MSG"));
            consumer2.Listener += new MessageListener(consumer_Listener);
        }
    }
}
