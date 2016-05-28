using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

/// <summary>
/// 日志记录
/// </summary>
public class WriteLog
{
    /// <summary>
    /// The witlogxt
    /// </summary>
    private static StreamWriter witlogxt;
    /// <summary>
    /// 
    /// </summary>
    private static string logxtpath = "";

    //非错误日志记录
    private static string LogDebug = System.Configuration.ConfigurationManager.AppSettings["LogDebug"].ToString();

    private static string LogInfo = System.Configuration.ConfigurationManager.AppSettings["LogInfo"].ToString();
    private static string LogInfobySystemNo = System.Configuration.ConfigurationManager.AppSettings["LogInfobySystemNo"].ToString();


    /// <summary>
    /// Writes the log.  Filter  过滤错误问题(时间等,null值)
    /// </summary>
    /// <param name="recvStr">The recv string.</param>
    public static void WriteFilterLog(string recvStr)
    {
        WriteLogs("Filter", "LogError", recvStr);
    }
    public static void WriteErrorLog(string name, string recvStr, bool flag = false)
    {
        WriteLogs(name, "LogError", recvStr, flag);
    }

    /// <summary>
    /// Writes the log.
    /// </summary>
    /// <param name="recvStr">The recv string.</param>
    public static void WriteOrdersLog(string recvStr)
    {
        WriteLogs("orders", "LogDebug/Debug", recvStr);
    }



    /// <summary>
    /// 记录 重连
    /// </summary>
    /// <param name="recvStr">The recv string.</param>
    /// <param name="flag">if set to <c>true</c> [flag].</param>
    public static void WriteLogRewiring(string recvStr, bool flag = false)
    {
        WriteLogs("Rewiring", "LogError", recvStr, flag);
    }

    public static void WriteLogs(string name, string path, string recvStr, bool flag = false)
    {
        try
        {

            if (LogDebug.Equals("Yes") || flag)
            {
                logxtpath = string.Format("{0}/{1:yyyyMMdd}_{2}.txt", DealDir(path), DateTime.Now, name);
                //消息处理
                witlogxt = File.AppendText(logxtpath);
                witlogxt.WriteLine(string.Format("{0}\r\n {1}", DateTime.Now, recvStr));
                witlogxt.WriteLine("------------------------------------------------");
                witlogxt.Close();
                witlogxt.Dispose();
            }
        }
        catch (Exception)
        {

        }

    }

    /// <summary>
    /// 记录 Other 发来的指令 和对此指令的回复
    /// </summary>
    /// <param name="other">other name</param>
    /// <param name="recvStr">The recv string.</param>
    /// <param name="flag">是否跳过日志发送开关写日志<c>true</c> [flag].</param>
    public static void WriteLogZLOther(string other, string recvStr, bool flag = false)
    {
        WriteLogs(other + "zl", "LogDebug", recvStr, flag);
    }

    /// <summary>
    /// 记录 Online/..  Log
    /// LogDebug/Debug
    /// </summary>
    /// <param name="name">logname</param>
    /// <param name="recvStr">The recv string.</param>
    /// <param name="flag">是否跳过日志发送开关写日志<c>true</c> [flag].</param>
    public static void WriteTestLog(string name, string recvStr, bool flag = false)
    {
        WriteLogs(name, "LogDebug/Debug", recvStr, flag);
    }

    /// <summary>
    /// 记录 ￥ 发来的指令 和对此指令的回复
    /// </summary>
    /// <param name="recvStr">The recv string.</param>
    /// <param name="flag">if set to <c>true</c> [flag].</param>
    public static void WriteLogZL(string recvStr, bool flag = false)
    {
        WriteLogs("zl", "LogDebug", recvStr, flag);
    }

    /// <summary>
    /// 日志绝对路径
    /// </summary>
    private static string absolutePath = System.Configuration.ConfigurationManager.AppSettings["AbsolutePath"].ToString();
    private static string DealDir(string path)
    {
        if (string.IsNullOrEmpty(absolutePath))
        {
            absolutePath = System.AppDomain.CurrentDomain.BaseDirectory;
        }
        path = Path.Combine(absolutePath, path);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        return path;
    }
    /// <summary>
    /// 记录 ￥ 发送到中心服务器的 非 终端通用应答 指令
    /// </summary>
    /// <param name="recvStr">The recv string.</param>
    /// <param name="flag">if set to <c>true</c> [flag].</param>
    public static void WriteSendLog(string recvStr, bool flag = false)
    {
        WriteLogs("ToCenter", "LogDebug/Send", recvStr, flag);
    }



    /// <summary>
    /// 记录 中心服务器 下发的指令
    /// </summary>
    /// <param name="recvStr">The recv string.</param>
    public static void WriteLogrecvStr(string recvStr)
    {
        WriteLogs("FromCenter", "LogDebug/RecvStr", recvStr);
    }

    /// <summary>
    /// 记录 从设备中发来的原始报文  (过滤车辆)
    /// </summary>
    /// <param name="recvStr">The recv string.</param>
    public static void WriteLogrecvStr(string systemNo, string recvStr)
    {
        if (LogInfobySystemNo.Contains("," + systemNo + ","))
        {
            WriteLogs("From" + systemNo, "LogDebug/RecvStr", recvStr,true);
        }
    }
    /// <summary>
    /// 记录 从设备中发来的原始报文  (过滤车型)
    /// </summary>
    /// <param name="recvStr">The recv string.</param>
    public static void WritRerecvStrFromClient(int models, string recvStr)
    {
        try
        {
            //-0   全部车型
            if (LogInfo.Contains(",-0,") || LogInfo.Contains("," + models + ","))
            {
                logxtpath = string.Format("{0}/{1:yyyyMMdd}_FromClient{2}.txt", DealDir("LogDebug/RecvStr"), DateTime.Now, models);
                //消息处理
                StreamWriter witlogxt1 = File.AppendText(logxtpath);
                witlogxt1.WriteLine(string.Format("{0}\r\n {1}", DateTime.Now, recvStr));
                witlogxt1.WriteLine("------------------------------------------------");
                witlogxt1.Close();
            }
        }
        catch (Exception)
        {

        }

    }

    public static void WriteLogMeaning(string typename, string recvStr, object obj)
    {
        try
        {
            //非错误日志记录  -10
            if (LogInfo.Contains(",-10,"))
            {
                //消息处理
                logxtpath = string.Format("{0}/{1:yyyyMMdd}_{2}Parsd.txt", DealDir("LogDebug/Debug"), DateTime.Now, typename);
                witlogxt = File.AppendText(logxtpath);
                witlogxt.WriteLine(string.Format("{0}\r", DateTime.Now));
                witlogxt.WriteLine(recvStr);
                foreach (System.Reflection.PropertyInfo p in obj.GetType().GetProperties())
                {
                    //判断是否是 非泛型
                    if (p.PropertyType.IsGenericType) continue;
                    object[] objs = obj.GetType().GetProperty(p.Name).GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (objs.Length > 0)
                    {
                        witlogxt.WriteLine(string.Format("{0}\t{1}\t\t\t{2}", p.GetValue(obj, null), ((DescriptionAttribute)objs[0]).Description, p.Name));
                    }
                }
                witlogxt.WriteLine("------------------------------------------------");
                witlogxt.Close();
                witlogxt.Dispose();
            }
        }
        catch (Exception)
        {
            witlogxt.Close();
            witlogxt.Dispose();
        }

    }
}
