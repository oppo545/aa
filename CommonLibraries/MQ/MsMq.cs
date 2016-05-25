using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;


public class MsMq : Mq
{
    public static Mq CreateInstance()
    {
        if (mqa == null)
        {
            mqa = new MsMq();
        }
        return mqa;
    }
    static MsMq mqa;
    public MsMq()
    {
        startMsmq();
    }

    public void startMsmq()
    {
        try
        {
            //入库队列 _sql
            string path = ".\\private$\\killf_sql";
            if (MessageQueue.Exists(path))
            {
                mq0 = new MessageQueue(path);
            }
            else
            {
                mq0 = MessageQueue.Create(path);
            }
            // mq0.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

            //入库队列
            path = ".\\private$\\killf";
            if (MessageQueue.Exists(path))
            {
                mq = new MessageQueue(path);
            }
            else
            {
                mq = MessageQueue.Create(path);
            }
            //  mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

            //入列can字段 根据用户自定义设置报警阈值 报警并发送提醒
            //转入 中心服务器处理 【卢】
            //path = ".\\private$\\killf_alarm";
            //if (MessageQueue.Exists(path))
            //{
            //    mq1 = new MessageQueue(path);
            //}
            //else
            //{
            //    mq1 = MessageQueue.Create(path);
            //}
            //mq1.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

            //入列 消息【车机上传，回复】到中心服务器
            path = ".\\private$\\killf_Lu";
            if (MessageQueue.Exists(path))
            {
                mq2 = new MessageQueue(path);
            }
            else
            {
                mq2 = MessageQueue.Create(path);
            }
            //  mq2.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

            //入列 消息【车机上传，回复】到中心服务器  GPS+Can 因为指令很多 避免影响命令 分开发送
            path = ".\\private$\\killf_Lu_GC";
            if (MessageQueue.Exists(path))
            {
                mq3 = new MessageQueue(path);
            }
            else
            {
                mq3 = MessageQueue.Create(path);
            }
            // mq3.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

            WriteLog.WriteErrorLog("ErrorSend", string.Format("______MSMQ启动___________________"), true);
        }
        catch (Exception ex)
        {
            WriteLog.WriteErrorLog("ErrorSend", string.Format("______MSMQ启动失败:{0}___________________", ex), true);
        }
    }
    
    private MessageQueue mq0;
    public void SendMsg0(string info)
    {
        try
        {
            if (!string.IsNullOrEmpty(info))
            {
                Message msg = new Message();
                msg.Body = info;                           //设置消息队列的内容
                msg.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });  //设置队列的格式
                mq0.Send(msg);
            }
        }
        catch (Exception ex)
        {
            WriteLog.WriteErrorLog("ErrorSend", string.Format("______killf_sql发送异常:{0}___________________{1}", ex, info), true);
            startMsmq();
        }

    }
    private MessageQueue mq;
    public void SendMsg(string info)
    {
        try
        {
            if (!string.IsNullOrEmpty(info))
            {
                Message msg = new Message();
                msg.Body = info;                           //设置消息队列的内容
                msg.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });  //设置队列的格式
                mq.Send(msg);
            }
        }
        catch (Exception ex)
        {
            WriteLog.WriteErrorLog("ErrorSend", string.Format("______killf发送异常:{0}___________________{1}", ex, info), true);
            startMsmq();
        }

    }


    //private MessageQueue mq1;
    //public void SendMsg1(string info)
    //{
    //    mq1.Send(info);
    //}

    private MessageQueue mq2;
    public void SendMsg2(string info)
    {
        try
        {
            if (!string.IsNullOrEmpty(info))
            {
                Message msg = new Message();
                msg.Body = info + "\b";                           //设置消息队列的内容
                msg.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });  //设置队列的格式
                mq2.Send(msg);
                //mq2.Send(info + "\b");
            }
        }
        catch (Exception ex)
        {
            WriteLog.WriteErrorLog("ErrorSend", string.Format("______killf_Lu发送异常:{0}___________________{1}", ex, info), true);
            startMsmq();
        }

    }

    private MessageQueue mq3;
    public void SendMsg3(string info)
    {
        try
        {
            if (!string.IsNullOrEmpty(info))
            {
                Message msg = new Message();
                msg.Body = info + "\b";                           //设置消息队列的内容
                msg.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });  //设置队列的格式
                mq3.Send(msg);
            }
        }
        catch (Exception ex)
        {
            WriteLog.WriteErrorLog("ErrorSend", string.Format("______killf_Lu_GC发送异常:{0}___________________{1}", ex, info), true);
            startMsmq();
        }

    }

    public void SendMsg4(string info)
    {

    }

    public void SendMsgByAlarm(string info)
    {
        try
        {
            if (!string.IsNullOrEmpty(info))
            {
                Message msg = new Message();
                msg.Body = info + "\b";                           //设置消息队列的内容
                msg.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });  //设置队列的格式
                mq3.Send(msg);
            }
        }
        catch (Exception ex)
        {
            WriteLog.WriteErrorLog("ErrorSend", string.Format("______killf_Lu_GC发送异常:{0}___________________{1}", ex, info), true);
            startMsmq();
        }

    }
}
