using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class PublicMethods
{
    /// <summary>
    /// 是否为udp
    /// X,公共类, 调用没用
    /// </summary>
    public static bool isudp;

    /// <summary>
    /// The timeout
    /// </summary>
    private static int timeout =int.Parse( ConfigurationManager.AppSettings["Timeout"].ToString());

    /// <summary>
    /// 获取时间或日期 2035-11-11 14:25:01 || 2035-11-11
    /// 0 日期, 1时间
    /// the world time 
    /// </summary>
    /// <param name="info"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static string GetGMT8Data(string info, int index)
    {
        string date = GetZeroSuppression(info);
        string year, month, day, hour = "", min = "", Seconds = "", result = "";
        year = PublicMethods.GetFomartZero(Getdecimal(date.Substring(0, 2)).ToString(), 2);
        month = PublicMethods.GetFomartZero(Getdecimal(date.Substring(2, 2)).ToString(), 2);
        day = PublicMethods.GetFomartZero(Getdecimal(date.Substring(4, 2)).ToString(), 2);
        if (index > 0)
        {
            hour = PublicMethods.GetFomartZero(Getdecimal(date.Substring(6, 2)).ToString(), 2);
            min = PublicMethods.GetFomartZero(Getdecimal(date.Substring(8, 2)).ToString(), 2);
            Seconds = PublicMethods.GetFomartZero(Getdecimal(date.Substring(10, 2)).ToString(), 2);
            result = string.Format("20{0}-{1}-{2} {3}:{4}:{5}", year, month, day, hour, min, Seconds);
        }
        else
        {
            result = string.Format("20{0}-{1}-{2}", year, month, day);
        }
        return result;
    }

    /// <summary>
    /// 判断时间 是否为正确时间格式,  是否超过 系统时间
    /// </summary>
    /// <param name="systemno">系统编号</param>
    /// <param name="dtime">需要判断的时间</param>
    /// <param name="dt">返回正确的时间 out</param>
    /// <returns>是否为正确时间格式</returns>
    public static bool TimeDetermine(string systemno, string dtime, out DateTime dt)
    {

        bool istime = false;
        //容错 不超过timeout分钟
        DateTime dtnow = DateTime.Now.AddMinutes(timeout);
        if (string.IsNullOrEmpty(dtime))
        {
            dt = dtnow.AddMinutes(-timeout);
            return istime;
        }
        istime = DateTime.TryParse(dtime, out dt);
        if (istime)
        {
            //TODO Filter  过滤 时间超过服务器时间的数据  
            if (DateTime.Compare(dtnow, dt) < 0)
            {
                WriteLog.WriteFilterLog(systemno + "时间超过服务器时间" + timeout + "分钟：" + dt);
                dt = dtnow.AddMinutes(-timeout);
            }
            else
            {
                dt = DateTime.Parse(dtime);
            }
        }
        else
        {
            dt = dtnow.AddMinutes(-timeout);
            WriteLog.WriteFilterLog(systemno + "不是正确的时间格式：" + dt);
        }
        return istime;
    }

    /// <summary>
    /// 格式化时间
    /// </summary>
    /// <param name="systemno"></param>
    /// <param name="dtime"></param>
    /// <returns></returns>
    public static string GetFormatTme(string systemno, string dtime)
    {
        string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        if (!string.IsNullOrEmpty(dtime))
        {
            DateTime dt = DateTime.Now;
            //容错 不超过timeout分钟
            DateTime dtnow = DateTime.Now.AddMinutes(timeout);
            bool istime = DateTime.TryParse(dtime, out dt); //转换失败 dt也会被赋值
            if (istime)
            {
                //TODO Filter  过滤 时间超过服务器时间的数据  
                if (DateTime.Compare(dtnow, dt) < 0)
                {
                    WriteLog.WriteFilterLog(systemno + "时间超过服务器时间" + timeout + "分钟：" + dt);
                    dt = dtnow.AddMinutes(-timeout);
                }
                else
                {
                    dt = DateTime.Parse(dtime);
                }
            }
            else
            {
                dt = DateTime.Parse(time);
                WriteLog.WriteFilterLog(systemno + "不是正确的时间格式：" + dt);
            }
            time = dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
        return time;
    }

    /// <summary>
    /// 是否为日期型字符串-正则验证 
    /// </summary>
    /// <param name="info">时间</param>
    /// <returns></returns>
    public static bool IsDateTimeByReg(string info)
    {
        return Regex.IsMatch(info, @"^(((((1[6-9]|[2-9]/d)/d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]/d|3[01]))|(((1[6-9]|[2-9]/d)/d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]/d|30))|(((1[6-9]|[2-9]/d)/d{2})-0?2-(0?[1-9]|1/d|2[0-8]))|(((1[6-9]|[2-9]/d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?/d):[0-5]?/d:[0-5]?/d)$ ");
    }

    /// <summary>
    /// 是否为日期型字符串-DateTime.TryParse
    /// </summary>
    /// <param name="info">时间</param>
    /// <returns></returns>
    public static bool IsDateTimeByTry(string info)
    {
        DateTime dt;
        return DateTime.TryParse(info, out dt);
    }

    /// <summary>
    /// 邰
    /// </summary>
    /// <param name="str_time"></param>
    /// <returns></returns>
    public static DateTime AnalysisTime(string str_time)
    {
        string date_y = str_time.Substring(0, 2); //年
        string date_m = str_time.Substring(3, 2); //月
        string date_d = str_time.Substring(6, 2); //日
        string time_h = str_time.Substring(9, 2); //时
        string time_m = str_time.Substring(12, 2);//分
        string time_s = str_time.Substring(15, 2);//秒

        DateTime time1 = DateTime.Now;
        int year = Convert.ToInt32(date_y, 16);
        int yearNow = Convert.ToInt32(time1.Year.ToString().Substring(2, 2));
        if (year != yearNow)
            year = yearNow;

        int mon = Convert.ToInt32(date_m, 16);
        if (mon != time1.Month)
            mon = time1.Month;

        int day = Convert.ToInt32(date_d, 16);
        if (day != time1.Day)
            day = time1.Day;

        int hour = Convert.ToInt32(time_h, 16);
        //if (hour != time1.Hour)
        if (hour > 23)
            hour = time1.Hour;

        int min = Convert.ToInt32(time_m, 16);
        if (min > 59)
            min = time1.Minute;

        int sec = Convert.ToInt32(time_s, 16);
        if (sec > 59)
            sec = time1.Second;

        string tttt = string.Format("20{0}-{1}-{2} {3}:{4}:{5}", year, mon, day, hour, min, sec);

        return Convert.ToDateTime(tttt);
    }//

    #region 格式化字符串


    /// <summary>
    /// 格式化字符串, 补足0, nums长度
    /// </summary>
    /// <param name="info">The info.</param>
    /// <param name="nums">位数 1-&gt;01 2</param>
    /// <returns>System.String.</returns>
    public static string GetFomartZero(string info, int nums)
    {
        if (info.Length < nums)
        {
            info = info.PadLeft(nums, '0');
        }
        return info;
    }

    /// <summary>
    /// 格式化字符串, 补足0,添加空格 2 N(字节)
    /// </summary>
    /// <param name="info">The info.</param>
    /// <param name="nums">位数 00 00-&gt;0000 4</param>
    /// <returns>System.String.</returns>
    public static string GetFomartZK(string info, int nums)
    {
        if (info.Length < nums * 2)
        {
            info = info.PadLeft(nums * 2, '0');
        }
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
        return temp.ToUpper();
    }

    #endregion

    /// <summary>
    /// 大小端转换
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public static string GetTransformationsSizeSide(string info)
    {
        string result = "";
        for (int i = info.Split(' ').Length-1; i >=0 ; i--)
        {
            result += info.Split(' ')[i].ToString()+" ";
        }
        return result;
    }

    #region 进制转换

    /// <summary>
    ///  10转16(加空格,补0)
    ///  字节
    /// </summary>
    /// <param name="data"></param>
    /// <param name="num">字节</param>
    /// <returns></returns>
    public static string Get10To16(string data, int num)
    {
        string info = Convert.ToString(int.Parse(data), 16).ToUpper();
        return GetFomartZK(info, num);
    }


    /// <summary>
    ///  16转2(去空格,补0)
    /// </summary>
    /// <param name="data"></param>
    /// <param name="num">字节数 00=1</param>
    /// <returns></returns>
    public static string Get16To2(string data, int num)
    {
        int data1 = Convert.ToInt32(GetZeroSuppression(data), 16);
        data = Convert.ToString(data1, 2);
        if (data.Length < 8 * num)
        {
            data = data.PadLeft(8 * num, '0');
        }
        return data;
    }




    /// <summary>
    /// 得到 10进制数据
    /// 16->10(未去空格)
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public static int Getdecimal(string info)
    {
        return Convert.ToInt32(info, 16);
    }


    /// <summary>
    /// 将原始数据(去空格)转化为10进制
    /// 16TO10
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public static int Get16To10(string info)
    {
        return Convert.ToInt32(GetZeroSuppression(info), 16);
    }

    /// <summary>
    /// 16进制字符串改为Ascii
    /// 4c 4d 56 30 31 32 33 34 35 36 37 38 39 41 42 43 44 -> LMV0123456789ABCD
    /// </summary>
    /// <param name="infos"></param>
    /// <returns></returns>
    public static string GetHexToAscii(string infos)
    {
        string[] temps = infos.Split(' ');
        string info = "";
        for (int i = 0; i < temps.Length; i++)
        {
            byte b = Convert.ToByte(temps[i].ToString(), 16);
            byte babb = b;
            info += ((char)babb).ToString();
        }
        return info;
    }

    /// <summary>
    /// Ascii转换 16进制字符串
    ///  LMV0123456789ABCD -> 4c 4d 56 30 31 32 33 34 35 36 37 38 39 41 42 43 44
    /// </summary>
    /// <param name="infos"></param>
    /// <returns></returns>
    public static string GetAsciiToHex(string infos)
    {
        byte[] ba = System.Text.ASCIIEncoding.Default.GetBytes(infos);
        StringBuilder sb = new StringBuilder();
        foreach (byte b in ba)
        {
            sb.Append(b.ToString("x") + " ");
        }
        return sb.ToString().Trim().ToUpper();
    }

    /// <summary>
    /// 10转2
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public static string Get10To2(string info)
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
    /// 2转10
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public static int Get2To10(string info)
    {
        return Convert.ToInt32(info, 2);
    }

    /// <summary>
    /// 字符串转16进制字节数组
    /// </summary>
    /// <param name="hexString"></param>
    /// <returns></returns>
    public static byte[] strToToHexByte(string hexString)
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
    /// 获取极值验证
    /// </summary>
    /// <param name="num"></param>
    /// <param name="num1"></param>
    /// <param name="num2"></param>
    /// <returns></returns>
    public static double GetExtremeCheck(object num, double num1, double num2)
    {
        if (double.Parse(num.ToString()) < num1)
        {
            num = num1;
        }
        if (double.Parse(num.ToString()) > num2)
        {
            num = num2;
        }
        return double.Parse(num.ToString());
    }


    #region 部标

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
        str1 = Get10To2(str1);
        str2 = Get10To2(str2);
        str3 = Get10To2(str3);
        str4 = Get10To2(str4);

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
    #endregion

    #region ToCenter FomatJosn
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

    /// <summary>
    /// 充电状态
    /// </summary>
    /// <param name="identifying"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string FomatToJosnByCharging(string identifying, string data)
    {
        string[] infos = data.Split(',');
        StringBuilder buffer = new StringBuilder();
        buffer.Append("{\"key\":\"client_message\",");//client_message_charging 卢还未写，暂时用 TODO
        buffer.Append("\"sendType\":\"2\",");
        buffer.Append("\"receiver\":\"" + infos[0] + "\",");
        //buffer.Append("\"timestamp\":\"" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\",");
        buffer.Append("\"identifying\":\"" + identifying + "\",");
        buffer.Append("\"data\":{");
        buffer.Append("\"systemNo\":\"" + infos[0] + "\",");
        buffer.Append("\"chargingStatus\":\"" + infos[1] + "\"");
        buffer.Append("}");
        buffer.Append("}");
        return buffer.ToString();
    }

    #endregion


    /// <summary>
    /// 获取字符串中符合正则的内容
    /// </summary>
    /// <param name="str">源码</param>
    /// <param name="regx">正则</param>
    /// <returns></returns>
    public static List<string> GetContentByReg(string str, string regx)
    {
        List<string> lis = new List<string>();
        try
        {
            string tmpStr = regx;
            MatchCollection TitleMatch = Regex.Matches(str, tmpStr, RegexOptions.IgnoreCase);
            int u = TitleMatch.Count;
            for (int i = 0; i < u; i++)
            {
                lis.Add(TitleMatch[i].Groups[0].ToString());
            }
        }
        catch (Exception ex)
        {

        }
        return lis;
    }


    public static string GetCanFaultStr(string faultSignalName, string faultCodes, int faultLever, int faultInt)
    {
        string resultstr = "";
        if (faultInt > 0)
            resultstr += string.Format("${0}:{1}:{2}:{3}", faultSignalName, faultCodes, faultLever, faultInt);
        return resultstr;

    }


}