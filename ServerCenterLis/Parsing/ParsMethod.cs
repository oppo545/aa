using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ParsMethod
{

    #region Big

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="BeginningBytes">起始字节标识 E</param>
    ///// <param name="Datas">数据</param>
    ///// <param name="StartBytes">起始字节 A</param>
    ///// <param name="StartBit">起始位 B</param>
    ///// <param name="BitLength">位长度 C</param>
    ///// <returns></returns>
    //public static int GetParsBig(string Datas, int StartBytes, int StartBit, int BitLength)
    //{
    //    //int BeginningBytes, 
    //    //BeginningBytes E 这里默认从1 为起始字节 
    //    //以0的,加1再传值
    //    StartBytes = StartBytes - 1;

    //    int result = 0;
    //    //获取在当前字节位的bit下标            bit=B-8A+8E
    //    int bitLocation = StartBit - 8 * StartBytes;

    //    //int number = getBytesNumberCount(StartBytes, StartBit, BitLength);
    //    int number = Datas.Split(' ').Count();
    //    //info
    //    int data2 = Convert.ToInt32(Datas.Replace(" ", ""), 16);
    //    string str_data3 = Convert.ToString(data2, 2);
    //    if (str_data3.Length < number * 8)
    //        str_data3 = str_data3.PadLeft(number * 8, '0');

    //    //获取位的起始byte下标 byte.sub..(0,length)       8D-C-bit,E
    //    bitLocation = 8 * number - BitLength - bitLocation;
    //    string str_data = str_data3.Substring(bitLocation, BitLength);
    //    result = Convert.ToInt32(str_data, 2);

    //    return result;
    //}
    #endregion
    #region  Little

    /// <summary>
    ///  获取小端模式解释
    /// </summary>
    /// <param name="BeginningBytes">起始字节标识 E</param>
    /// <param name="Datas">数据</param>
    /// <param name="StartBytes">起始字节 A</param>
    /// <param name="StartBit">起始位 B</param>
    /// <param name="BitLength">位长度 C</param>
    /// <returns></returns>
    public static int GetParsLittle(string Datas, int StartBytes, int StartBit, int BitLength)
    {
        //int BeginningBytes, 
        //BeginningBytes E 这里默认从1 为起始字节 
        //以0的,加1再传值
        StartBytes = StartBytes - 1;

        int result = 0;
        //获取在当前字节位的bit下标            bit=B-8A+8E   (E为1的话,会多减8,所以开始默认为1,减一)
        int bitLocation = StartBit - 8 * StartBytes;

        //int number = getBytesNumberCount(StartBytes, StartBit, BitLength);
        int number = Datas.Split(' ').Count();
        string infotemp = "";
        if (number > 1)
        {
            infotemp = GetTurnOrder(Datas);
        }
        else
        {
            infotemp = Datas.Replace(" ", "");
        }

        //info
        int data2 = Convert.ToInt32(infotemp, 16);
        string str_data3 = Convert.ToString(data2, 2);
        if (str_data3.Length < number * 8)
            str_data3 = str_data3.PadLeft(number * 8, '0');

        bitLocation = 8 * number - BitLength - bitLocation;
        string str_data = str_data3.Substring(bitLocation, BitLength);
        result = Convert.ToInt32(str_data, 2);

        return result;
    }


    #endregion


    #region 解析[两种顺序的不同,只是跟哪几个字节有关,当前 直接传递字节,半自动,所以 不管顺序]
    /// <summary>
    ///  根据二进制获取值
    ///  起始所在字节标识 E  默认以0开始
    /// </summary>
    /// <param name="data">二进制数据</param>
    /// <param name="StartBit">起始位 B</param>
    /// <param name="BitLength">位长度 C</param>
    ///  <param name="StartBytes">起始所在字节标识 E  默认以0开始</param>
    /// <returns></returns>
    public static int GetValuebyBinary(string data, int StartBit, int BitLength, int StartBytes=0)
    {
        //7 6 5 4 3 2 1 0   //bit顺序
        //0 0 0 0 0 0 0 0   //二进制
        //0 1 2 3 4 5 6 7   //下标
        //7-(B+C-1)  =8-B-C
        return PublicMethods.Get2To10(PublicMethods.Get16To2(data, 1).Substring(8 - (StartBit - 8 * StartBytes) - BitLength, BitLength));
    }
    /// <summary>
    ///              Get16To10
    /// </summary>
    /// <param name="Datas"></param>
    /// <param name="Offset"></param>
    /// <returns>int</returns>
    public static int GetParsWholeByte(string Datas,int Resolution=1, int Offset = 0)
    {
        return PublicMethods.Get16To10(PublicMethods.GetZeroSuppression(Datas)) * Resolution + Offset;
    }
    /// <summary>
    /// Get16To10
    /// </summary>
    /// <param name="Datas">16进制直接转10</param>
    /// <param name="Resolution"></param>
    /// <param name="Offset"></param>
    /// <returns>double</returns>
    public static double GetParsWholeByte(string Datas, double Resolution, int Offset = 0)
    {
        decimal d = (decimal)(PublicMethods.Get16To10(PublicMethods.GetZeroSuppression(Datas)) * Resolution) + Offset;
        return (double)Math.Round(d, 2, MidpointRounding.AwayFromZero) ;
    }
    /// <summary>
    ///          获取小端模式解释
    /// </summary>
    /// <param name="BeginningBytes">起始字节标识</param>
    /// <param name="Datas">数据</param>
    /// <param name="StartBytes">起始字节</param>
    /// <param name="StartBit">起始位</param>
    /// <param name="BitLength">位长度</param>
    /// <param name="Resolution">精度</param>
    /// <param name="Offset">偏移量</param>
    /// <returns></returns>
    public static double GetParsLittle(string Datas, int StartBytes, int StartBit, int BitLength, double Resolution, int Offset = 0)
    {
        return (double)Math.Round((decimal)(GetPars(1, Datas, StartBytes, StartBit, BitLength) * Resolution) + Offset, 2, MidpointRounding.AwayFromZero);
    }
    public static int GetParsLittle(string Datas, int StartBytes, int StartBit, int BitLength, int Offset = 0)
    {
        return GetPars(1, Datas, StartBytes, StartBit, BitLength) + Offset;
    }
    public static int GetParsBig(string Datas, int StartBytes, int StartBit, int BitLength, int Offset = 0)
    {
        return GetPars(0, Datas, StartBytes, StartBit, BitLength) + Offset;
    }
    /// <summary>
    ///获取大端模式解释
    /// </summary>
    /// <param name="BeginningBytes">起始字节标识 </param>
    /// <param name="Datas">数据</param>
    /// <param name="StartBytes">起始字节</param>
    /// <param name="StartBit">起始位</param>
    /// <param name="BitLength">位长度</param>
    /// <param name="Resolution">精度</param>
    /// <param name="Offset">偏移量</param>
    /// <returns></returns>
    public static double GetParsBig(string Datas, int StartBytes, int StartBit, int BitLength, double Resolution, int Offset = 0)
    {
        return (double)Math.Round((decimal)(GetPars(0, Datas, StartBytes, StartBit, BitLength) * Resolution) + Offset, 2, MidpointRounding.AwayFromZero);
    }


    /// <summary>
    ///  获取解释
    /// </summary>
    /// <param name="index">大端:0 小端:1</param>
    /// <param name="BeginningBytes">起始字节标识 E</param>
    /// <param name="Datas">数据</param>
    /// <param name="StartBytes">起始字节 A</param>
    /// <param name="StartBit">起始位 B</param>
    /// <param name="BitLength">位长度 C</param>
    /// <returns></returns>
    public static int GetPars(int index, string Datas, int StartBytes, int StartBit, int BitLength)
    {
        //int BeginningBytes, 
        //BeginningBytes E 这里默认从1 为起始字节 
        //以0的,加1再传值
        StartBytes = StartBytes - 1;

        int result = 0;
        //获取在当前字节位的bit下标            bit=B-8A+8E
        int bitLocation = StartBit - 8 * StartBytes;

        //int number = getBytesNumberCount(StartBytes, StartBit, BitLength);
        int number = Datas.Split(' ').Count();
        string infotemp = "";
        if (index == 1 && number > 1)
        {
            infotemp = GetTurnOrder(Datas);
        }
        else
        {
            infotemp = Datas.Replace(" ", "");
        }

        //info
        int data2 = Convert.ToInt32(infotemp, 16);
        string str_data3 = Convert.ToString(data2, 2);
        if (str_data3.Length < number * 8)
            str_data3 = str_data3.PadLeft(number * 8, '0');

        //获取位的起始byte下标 byte.sub..(0,length)       8D-C-bit,E
        bitLocation = 8 * number - BitLength - bitLocation;
        string str_data = str_data3.Substring(bitLocation, BitLength);
        result = Convert.ToInt32(str_data, 2);

        return result;
    }

    /// <summary>
    /// 自定义协议报警等级对照                   一级故障		致命故障
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static int GetFaultlever(string data)
    {
        int result = 1;
        if (data.Equals("11"))
        {
            result = 4;
        } if (data.Equals("21"))
        {
            result = 3;
        } if (data.Equals("31"))
        {
            result = 2;
        } if (data.Equals("41"))
        {
            result = 1;
        }
        return result;
    }
    #endregion

    #region Unit
    public static int getBytesNumberCount(int StartBytes, int StartBit, int BitLength)
    {
        int result = 3;
        return result;
    }
    /// <summary>
    /// 数组反转
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public static string GetTurnOrder(string info)
    {
        string resultinfo = "";
        string[] temps = info.Split(' ');
        for (int i = temps.Count() - 1; i >= 0; i--)
        {
            resultinfo += temps[i];
        }
        return resultinfo;
    }

    static ArrayList strsignal = new ArrayList() {"00 10", "00 11", "00 15" };
    static int[] signallength = new int[5] { 1, 1, 2, 3, 4 };
    public static int GetNumberbyId(string id)
    {
        int result = 1; int temp = -1;
        temp = strsignal.IndexOf(id);
        result = temp == -1 ? result : temp;
        return signallength[result];
    }

    #endregion


}
