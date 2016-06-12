using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;


public class MqActive : Mq
{

    //声明连接对象工厂
    private static IConnectionFactory factory;

    public string icfactory = ConfigurationManager.AppSettings["icfactory"].ToString();

    public MqActive()
    {
        try
        {
            //初始化工厂
            factory = new ConnectionFactory(icfactory);
            sendDataTimer = new System.Threading.Timer(new System.Threading.TimerCallback(SendDataCallback), null, Polltime, Polltime);
            sendDataTimer1 = new System.Threading.Timer(new System.Threading.TimerCallback(SendDataCallback1), null, Polltime, Polltime);
            sendDataTimer2 = new System.Threading.Timer(new System.Threading.TimerCallback(SendDataCallback2), null, Polltime, Polltime);
            sendDataTimerbyGPS = new System.Threading.Timer(new System.Threading.TimerCallback(SendDataCallbackbyGPS), null, Polltime, Polltime);
            sendDataTimerbyAlarm = new System.Threading.Timer(new System.Threading.TimerCallback(SendDataCallbackbyAlarm), null, Polltime, Polltime);
            sendDataTimerbyForward = new System.Threading.Timer(new System.Threading.TimerCallback(SendDataCallbackbyForward), null, Polltime, Polltime);
            sendDataTimerbyFSZL = new System.Threading.Timer(new System.Threading.TimerCallback(SendDataCallbackbyFSZL), null, Polltime, Polltime);
            sendDataTimerbyCharge = new System.Threading.Timer(new System.Threading.TimerCallback(SendDataCallbackbyCharge), null, Polltime, Polltime);
        }
        catch (Exception)
        {

        }

    }
    static System.Threading.Timer sendDataTimer = null;
    static System.Threading.Timer sendDataTimer1 = null;
    static System.Threading.Timer sendDataTimer2 = null;
  

    /// <summary>
    /// The Polltime
    /// </summary>
    private static int Polltime = int.Parse(ConfigurationManager.AppSettings["Polltime"].ToString());
    /// <summary>
    /// 入库lis
    /// </summary>
    static List<string> lissql1 = new List<string>();
    /// <summary>
    /// 装载箱1
    /// </summary>
    static List<string> listemp1 = new List<string>();
    /// <summary>
    /// 装载箱1
    /// </summary>
    static List<string> listemp11 = new List<string>();
    /// <summary>
    /// 集合轮询调用开关
    /// </summary>
    static bool flagtemp1 = false;
    /// <summary>
    /// lock
    /// </summary>
    private static object _lock1 = new object();


    /// <summary>
    /// 入库lis
    /// </summary>
    static List<string> lissql2 = new List<string>();
    /// <summary>
    /// 装载箱1
    /// </summary>
    static List<string> listemp2 = new List<string>();
    /// <summary>
    /// 装载箱1
    /// </summary>
    static List<string> listemp22 = new List<string>();
    /// <summary>
    /// 集合轮询调用开关
    /// </summary>
    static bool flagtemp2 = false;
    /// <summary>
    /// lock
    /// </summary>
    private static object _lock2 = new object();

    /// <summary>
    /// 入库lis
    /// </summary>
    static List<string> lissql3 = new List<string>();
    /// <summary>
    /// 装载箱1
    /// </summary>
    static List<string> listemp3 = new List<string>();
    /// <summary>
    /// 装载箱1
    /// </summary>
    static List<string> listemp33 = new List<string>();
    /// <summary>
    /// 集合轮询调用开关
    /// </summary>
    static bool flagtemp3 = false;
    /// <summary>
    /// lock
    /// </summary>
    private static object _lock3 = new object();


    /// <summary>
    /// Can
    /// </summary>
    /// <param name="sender"></param>
    private static void SendDataCallback(Object sender)
    {
        lock (_lock1)
        {
            try
            {
                if ((listemp1.Count > 0 || listemp11.Count > 0) && lissql1.Count == 0)
                {
                    if (!flagtemp1)
                    {
                        flagtemp1 = true;
                        lissql1.AddRange(listemp1);
                        listemp1.Clear();
                    }
                    else
                    {
                        flagtemp1 = false;
                        lissql1.AddRange(listemp11);
                        listemp11.Clear();
                    }
                    if (lissql1.Count > 0)
                    {
                        sendObject("Can", lissql1);
                        lissql1.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogZLOther("ErrorAS", "______Mq异常:SendDataCallback" + ex.Message, true);
            }
        }
    }
    /// <summary>
    /// PutQueue
    /// </summary>
    /// <param name="sender"></param>
    private static void SendDataCallback1(Object sender)
    {
        lock (_lock2)
        {
            try
            {
                if ((listemp2.Count > 0 || listemp22.Count > 0) && lissql2.Count == 0)
                {
                    if (!flagtemp2)
                    {
                        flagtemp2 = true;
                        lissql2.AddRange(listemp2);
                        listemp2.Clear();
                    }
                    else
                    {
                        flagtemp2 = false;
                        lissql2.AddRange(listemp22);
                        listemp22.Clear();
                    }
                    if (lissql2.Count > 0)
                    {
                        sendObject1("PutQueue", lissql2);
                        lissql2.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogZLOther("ErrorAS", "______Mq异常:SendDataCallback" + ex.Message, true);
            }
        }
    }
    /// <summary>
    /// OriginalMessage
    /// </summary>
    /// <param name="sender"></param>
    private static void SendDataCallback2(Object sender)
    {
        lock (_lock3)
        {
            try
            {
                if ((listemp3.Count > 0 || listemp33.Count > 0) && lissql3.Count == 0)
                {
                    if (!flagtemp3)
                    {
                        flagtemp3 = true;
                        lissql3.AddRange(listemp3);
                        listemp3.Clear();
                    }
                    else
                    {
                        flagtemp3 = false;
                        lissql3.AddRange(listemp33);
                        listemp33.Clear();
                    }
                    if (lissql3.Count > 0)
                    {
                        sendObject("ActiveMQ.OrderReply", lissql3);
                        lissql3.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogZLOther("ErrorAS", "______Mq异常:SendDataCallback2" + ex.Message, true);
            }
        }
    }
    IMessageProducer prod2;
    ITextMessage message2;

    IMessageProducer prod3;
    ITextMessage message3;

    static IConnection connection;
    static ISession session;

    static IMessageProducer prod;
    static ITextMessage message;
    public static void sendObject(string mqname, List<string> SQLStringList)
    {
        try
        {
            List<string> lisSql = new List<string>();
            lisSql.AddRange(SQLStringList);
            using (connection = factory.CreateConnection())
            {

                //通过连接创建Session会话
                using (session = connection.CreateSession())
                {
                    //通过会话创建生产者，方法里面new出来的是MQ中的Queue
                    prod = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(mqname));
                    for (int i = 0; i < lisSql.Count(); i++)
                    {
                        //创建一个发送的消息对象
                        message = prod.CreateTextMessage();
                        //给这个对象赋实际的消息
                        message.Text = lisSql[i].ToString();
                        //设置消息对象的属性，这个很重要哦，是Queue的过滤条件，也是P2P消息的唯一指定属性
                        message.Properties.SetString("filter", "demo");
                        //生产者把消息发送出去，几个枚举参数MsgDeliveryMode是否长链，MsgPriority消+息优先级别，发送最小单位，当然还有其他重载
                        prod.Send(message, MsgDeliveryMode.Persistent, MsgPriority.Normal, TimeSpan.MinValue);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lissql1.Clear();
        }
    }

    static IMessageProducer prod1;
    static ITextMessage message1;
    public static void sendObject1(string mqname, List<string> SQLStringList)
    {
        try
        {
            List<string> lisSql = new List<string>();
            lisSql.AddRange(SQLStringList);
            using (connection = factory.CreateConnection())
            {

                //通过连接创建Session会话
                using (session = connection.CreateSession())
                {
                    //通过会话创建生产者，方法里面new出来的是MQ中的Queue
                    prod1 = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(mqname));
                    for (int i = 0; i < lisSql.Count(); i++)
                    {
                        //创建一个发送的消息对象
                        message1 = prod1.CreateTextMessage();
                        //给这个对象赋实际的消息
                        message1.Text = lisSql[i].ToString();
                        //设置消息对象的属性，这个很重要哦，是Queue的过滤条件，也是P2P消息的唯一指定属性
                        message1.Properties.SetString("filter", "demo");
                        //生产者把消息发送出去，几个枚举参数MsgDeliveryMode是否长链，MsgPriority消+息优先级别，发送最小单位，当然还有其他重载
                        prod1.Send(message1, MsgDeliveryMode.Persistent, MsgPriority.Normal, TimeSpan.MinValue);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lissql1.Clear();
        }
    }

    public void SendMsg0(string info)
    {
        SendMsg(info);
    }
    public void SendMsg(string info)
    {
        if (!flagtemp2)
            listemp2.Add(info);
        else listemp22.Add(info);
    }

    /// <summary>
    /// sendCan
    /// </summary>
    /// <param name="info"></param>
    public void SendMsg2(string info)
    {
        if (!flagtemp1)
            listemp1.Add(info);
        else listemp11.Add(info);
    }

    /// <summary>
    /// GPS
    /// </summary>
    /// <param name="info"></param>
    public void SendMsg3(string info)
    {
        if (!flagGPS)
            lisGPS1.Add(info);
        else lisGPS2.Add(info);
    }
    public void SendMsgByAlarm(string info)
    {
        if (!flagAlarm)
            lisAlarm1.Add(info);
        else lisAlarm2.Add(info);
    }

    public void SendMsg4(string info)
    {
        if (!flagtemp3)
            listemp3.Add(info);
        else listemp33.Add(info);
    }

    public static Mq CreateInstance()
    {
        if (mqa == null)
        {
            mqa = new MqActive();
        }
        return mqa;
    }
    static MqActive mqa;


    #region Send GPS To Center
    static System.Threading.Timer sendDataTimerbyGPS = null;
    /// <summary>
    /// 入库lis
    /// </summary>
    static List<string> lisGPS = new List<string>();
    /// <summary>
    /// 装载箱1
    /// </summary>
    static List<string> lisGPS1 = new List<string>();
    /// <summary>
    /// 装载箱1
    /// </summary>
    static List<string> lisGPS2 = new List<string>();
    /// <summary>
    /// 集合轮询调用开关
    /// </summary>
    static bool flagGPS = false;
    /// <summary>
    /// lock
    /// </summary>
    private static object _lockGPS = new object();
                                                   
    private static void SendDataCallbackbyGPS(Object sender)
    {
        lock (_lockGPS)
        {
            try
            {
                if ((lisGPS1.Count > 0 || lisGPS2.Count > 0) && lisGPS.Count == 0)
                {
                    if (!flagGPS)
                    {
                        flagGPS = true;
                        lisGPS.AddRange(lisGPS1);
                        lisGPS1.Clear();
                    }
                    else
                    {
                        flagGPS = false;
                        lisGPS.AddRange(lisGPS2);
                        lisGPS2.Clear();
                    }
                    if (lisGPS.Count > 0)
                    {
                        sendObjectbyGPS("GPS", lisGPS);
                        lisGPS.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogZLOther("ErrorAS", "______Mq异常:SendDataCallback" + ex.Message, true);
            }
        }
    }
    static IMessageProducer prodGPS;
    static ITextMessage messageGPS;
    public static void sendObjectbyGPS(string mqname, List<string> SQLStringList)
    {
        try
        {
            List<string> lisSql = new List<string>();
            lisSql.AddRange(SQLStringList);
            using (connection = factory.CreateConnection())
            {

                //通过连接创建Session会话
                using (session = connection.CreateSession())
                {
                    //通过会话创建生产者，方法里面new出来的是MQ中的Queue
                    prodGPS = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(mqname));
                    for (int i = 0; i < lisSql.Count(); i++)
                    {
                        //创建一个发送的消息对象
                        messageGPS = prodGPS.CreateTextMessage();
                        //给这个对象赋实际的消息
                        messageGPS.Text = lisSql[i].ToString();
                        //设置消息对象的属性，这个很重要哦，是Queue的过滤条件，也是P2P消息的唯一指定属性
                        messageGPS.Properties.SetString("filter", "demo");
                        //生产者把消息发送出去，几个枚举参数MsgDeliveryMode是否长链，MsgPriority消+息优先级别，发送最小单位，当然还有其他重载
                        prodGPS.Send(messageGPS, MsgDeliveryMode.Persistent, MsgPriority.Normal, TimeSpan.MinValue);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lisGPS.Clear();
        }
    }

    #endregion

    #region Send Alarm To Center
    static System.Threading.Timer sendDataTimerbyAlarm = null;
    /// <summary>
    /// 入库lis
    /// </summary>
    static List<string> lisAlarm = new List<string>();
    /// <summary>
    /// 装载箱1
    /// </summary>
    static List<string> lisAlarm1 = new List<string>();
    /// <summary>
    /// 装载箱1
    /// </summary>
    static List<string> lisAlarm2 = new List<string>();
    /// <summary>
    /// 集合轮询调用开关
    /// </summary>
    static bool flagAlarm = false;
    /// <summary>
    /// lock
    /// </summary>
    private static object _lockAlarm = new object();

    private static void SendDataCallbackbyAlarm(Object sender)
    {
        lock (_lockAlarm)
        {
            try
            {
                if ((lisAlarm1.Count > 0 || lisAlarm2.Count > 0) && lisAlarm.Count == 0)
                {
                    if (!flagAlarm)
                    {
                        flagAlarm = true;
                        lisAlarm.AddRange(lisAlarm1);
                        lisAlarm1.Clear();
                    }
                    else
                    {
                        flagAlarm = false;
                        lisAlarm.AddRange(lisAlarm2);
                        lisAlarm2.Clear();
                    }
                    if (lisAlarm.Count > 0)
                    {
                        sendObjectbyAlarm("Alarm", lisAlarm);
                        lisAlarm.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogZLOther("ErrorAS", "______Mq异常:SendDataCallback" + ex.Message, true);
            }
        }
    }
    static IMessageProducer prodAlarm;
    static ITextMessage messageAlarm;
    public static void sendObjectbyAlarm(string mqname, List<string> SQLStringList)
    {
        try
        {
            List<string> lisSql = new List<string>();
            lisSql.AddRange(SQLStringList);
            using (connection = factory.CreateConnection())
            {

                //通过连接创建Session会话
                using (session = connection.CreateSession())
                {
                    //通过会话创建生产者，方法里面new出来的是MQ中的Queue
                    prodAlarm = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(mqname));
                    for (int i = 0; i < lisSql.Count(); i++)
                    {
                        //创建一个发送的消息对象
                        messageAlarm = prodAlarm.CreateTextMessage();
                        //给这个对象赋实际的消息
                        messageAlarm.Text = lisSql[i].ToString();
                        //设置消息对象的属性，这个很重要哦，是Queue的过滤条件，也是P2P消息的唯一指定属性
                        messageAlarm.Properties.SetString("filter", "demo");
                        //生产者把消息发送出去，几个枚举参数MsgDeliveryMode是否长链，MsgPriority消+息优先级别，发送最小单位，当然还有其他重载
                        prodAlarm.Send(messageAlarm, MsgDeliveryMode.Persistent, MsgPriority.Normal, TimeSpan.MinValue);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lisAlarm.Clear();
        }
    }

    #endregion

   //发送方法在里面--------------------------------------------------------------------------------------------------

    #region Send Forward To Center
    static System.Threading.Timer sendDataTimerbyForward = null;
    /// <summary>
    /// 入库lis
    /// </summary>
    static List<string> lisForward = new List<string>();
    /// <summary>
    /// 装载箱1
    /// </summary>
    static List<string> lisForward1 = new List<string>();
    /// <summary>
    /// 装载箱1
    /// </summary>
    static List<string> lisForward2 = new List<string>();
    /// <summary>
    /// 集合轮询调用开关
    /// </summary>
    static bool flagForward = false;
    /// <summary>
    /// lock
    /// </summary>
    private static object _lockForward = new object();

    private static void SendDataCallbackbyForward(Object sender)
    {
        lock (_lockForward)
        {
            try
            {
                if ((lisForward1.Count > 0 || lisForward2.Count > 0) && lisForward.Count == 0)
                {
                    if (!flagForward)
                    {
                        flagForward = true;
                        lisForward.AddRange(lisForward1);
                        lisForward1.Clear();
                    }
                    else
                    {
                        flagForward = false;
                        lisForward.AddRange(lisForward2);
                        lisForward2.Clear();
                    }
                    if (lisForward.Count > 0)
                    {
                        sendObjectbyForward("ForwardToSHDB", lisForward);
                        lisForward.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogZLOther("ErrorAS", "______Mq异常:SendDataCallback" + ex.Message, true);
            }
        }
    }
    public void SendMsgByForwardToSHDB(string info)
    {
        if (!flagForward)
            lisForward1.Add(info);
        else lisForward2.Add(info);
    }
    static IMessageProducer prodForward;
    static ITextMessage messageForward;
    public static void sendObjectbyForward(string mqname, List<string> SQLStringList)
    {
        try
        {
            List<string> lisSql = new List<string>();
            lisSql.AddRange(SQLStringList);
            using (connection = factory.CreateConnection())
            {

                //通过连接创建Session会话
                using (session = connection.CreateSession())
                {
                    //通过会话创建生产者，方法里面new出来的是MQ中的Queue
                    prodForward = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(mqname));
                    for (int i = 0; i < lisSql.Count(); i++)
                    {
                        //创建一个发送的消息对象
                        messageForward = prodForward.CreateTextMessage();
                        //给这个对象赋实际的消息
                        messageForward.Text = lisSql[i].ToString();
                        //设置消息对象的属性，这个很重要哦，是Queue的过滤条件，也是P2P消息的唯一指定属性
                        messageForward.Properties.SetString("filter", "demo");
                        //生产者把消息发送出去，几个枚举参数MsgDeliveryMode是否长链，MsgPriority消+息优先级别，发送最小单位，当然还有其他重载
                        prodForward.Send(messageForward, MsgDeliveryMode.Persistent, MsgPriority.Normal, TimeSpan.MinValue);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lisForward.Clear();
        }
    }

    #endregion

    #region Send FSZL To Center  分时租赁
    static System.Threading.Timer sendDataTimerbyFSZL = null;
    /// <summary>
    /// 入库lis
    /// </summary>
    static List<string> lisFSZL = new List<string>();
    /// <summary>
    /// 装载箱1
    /// </summary>
    static List<string> lisFSZL1 = new List<string>();
    /// <summary>
    /// 装载箱1
    /// </summary>
    static List<string> lisFSZL2 = new List<string>();
    /// <summary>
    /// 集合轮询调用开关
    /// </summary>
    static bool flagFSZL = false;
    /// <summary>
    /// lock
    /// </summary>
    private static object _lockFSZL = new object();

    private static void SendDataCallbackbyFSZL(Object sender)
    {
        lock (_lockFSZL)
        {
            try
            {
                if ((lisFSZL1.Count > 0 || lisFSZL2.Count > 0) && lisFSZL.Count == 0)
                {
                    if (!flagFSZL)
                    {
                        flagFSZL = true;
                        lisFSZL.AddRange(lisFSZL1);
                        lisFSZL1.Clear();
                    }
                    else
                    {
                        flagFSZL = false;
                        lisFSZL.AddRange(lisFSZL2);
                        lisFSZL2.Clear();
                    }
                    if (lisFSZL.Count > 0)
                    {
                        sendObjectbyFSZL("FSZL_Reply", lisFSZL);
                        lisFSZL.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogZLOther("ErrorAS", "______FSZLMq异常:SendDataCallback" + ex.Message, true);
            }
        }
    }
    public void SendMsgByFSZL(string info)
    {
        if (!flagFSZL)
            lisFSZL1.Add(info);
        else lisFSZL2.Add(info);
    }
    static IMessageProducer prodFSZL;
    static ITextMessage messageFSZL;
    public static void sendObjectbyFSZL(string mqname, List<string> SQLStringList)
    {
        try
        {
            List<string> lisSql = new List<string>();
            lisSql.AddRange(SQLStringList);
            using (connection = factory.CreateConnection())
            {

                //通过连接创建Session会话
                using (session = connection.CreateSession())
                {
                    //通过会话创建生产者，方法里面new出来的是MQ中的Queue
                    prodFSZL = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(mqname));
                    for (int i = 0; i < lisSql.Count(); i++)
                    {
                        //创建一个发送的消息对象
                        messageFSZL = prodFSZL.CreateTextMessage();
                        //给这个对象赋实际的消息
                        messageFSZL.Text = lisSql[i].ToString();
                        //设置消息对象的属性，这个很重要哦，是Queue的过滤条件，也是P2P消息的唯一指定属性
                        messageFSZL.Properties.SetString("filter", "demo");
                        //生产者把消息发送出去，几个枚举参数MsgDeliveryMode是否长链，MsgPriority消+息优先级别，发送最小单位，当然还有其他重载
                        prodFSZL.Send(messageFSZL, MsgDeliveryMode.Persistent, MsgPriority.Normal, TimeSpan.MinValue);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lisFSZL.Clear();
        }
    }

    #endregion

    #region Send Charge To Center  充电桩
    static System.Threading.Timer sendDataTimerbyCharge = null;
    /// <summary>
    /// 入库lis
    /// </summary>
    static List<string> lisCharge = new List<string>();
    /// <summary>
    /// 装载箱1
    /// </summary>
    static List<string> lisCharge1 = new List<string>();
    /// <summary>
    /// 装载箱1
    /// </summary>
    static List<string> lisCharge2 = new List<string>();
    /// <summary>
    /// 集合轮询调用开关
    /// </summary>
    static bool flagCharge = false;
    /// <summary>
    /// lock
    /// </summary>
    private static object _lockCharge = new object();

    private static void SendDataCallbackbyCharge(Object sender)
    {
        lock (_lockCharge)
        {
            try
            {
                if ((lisCharge1.Count > 0 || lisCharge2.Count > 0) && lisCharge.Count == 0)
                {
                    if (!flagCharge)
                    {
                        flagCharge = true;
                        lisCharge.AddRange(lisCharge1);
                        lisCharge1.Clear();
                    }
                    else
                    {
                        flagCharge = false;
                        lisCharge.AddRange(lisCharge2);
                        lisCharge2.Clear();
                    }
                    if (lisCharge.Count > 0)
                    {
                        sendObjectbyCharge("chargeReceiveQueue", lisCharge);
                        lisCharge.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogZLOther("ErrorAS", "______ChargeMq异常:SendDataCallback" + ex.Message, true);
            }
        }
    }
    public void SendMsgByCharge(string info)
    {
        if (!flagCharge)
            lisCharge1.Add(info);
        else lisCharge2.Add(info);
    }
    static IMessageProducer prodCharge;
    static ITextMessage messageCharge;
    public static void sendObjectbyCharge(string mqname, List<string> SQLStringList)
    {
        try
        {
            List<string> lisSql = new List<string>();
            lisSql.AddRange(SQLStringList);
            using (connection = factory.CreateConnection())
            {

                //通过连接创建Session会话
                using (session = connection.CreateSession())
                {
                    //通过会话创建生产者，方法里面new出来的是MQ中的Queue
                    prodCharge = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(mqname));
                    for (int i = 0; i < lisSql.Count(); i++)
                    {
                        //创建一个发送的消息对象
                        messageCharge = prodCharge.CreateTextMessage();
                        //给这个对象赋实际的消息
                        messageCharge.Text = lisSql[i].ToString();
                        //设置消息对象的属性，这个很重要哦，是Queue的过滤条件，也是P2P消息的唯一指定属性
                        messageCharge.Properties.SetString("filter", "demo");
                        //生产者把消息发送出去，几个枚举参数MsgDeliveryMode是否长链，MsgPriority消+息优先级别，发送最小单位，当然还有其他重载
                        prodCharge.Send(messageCharge, MsgDeliveryMode.Persistent, MsgPriority.Normal, TimeSpan.MinValue);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lisCharge.Clear();
        }
    }

    #endregion
}


