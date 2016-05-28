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
    ///  根据16进制(一个字节)获取值二进制的10进制
    ///  起始所在字节标识 E  默认以0开始  (紧计算了位)
    /// </summary>
    /// <param name="data">数据</param>
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

    #region 封装 上标数据格式
    
   
    /// <summary>
    ///  封装 上标数据格式
    /// </summary>
    /// <param name="commandName"></param>
    /// <param name="lsh"></param>
    /// <param name="vin"></param>
    /// <param name="dataUnit"></param>
    /// <returns></returns>
    public string GetFormatByMarkedVehicles(string commandName, string lsh, string vin, string dataUnit)
    {
        string result = "7E ";
        string info = string.Format("23 23 {0} {1} 00 {2} {3}", commandName, lsh, vin, PublicMethods.Get10To16(dataUnit.Split(' ').Count().ToString(), 2).ToString(), dataUnit);
        result += string.Format("{0} {1} 7E", info, PublicMethods.GetJy(info.Split(' ')));
        return result;
    }

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="registime">注册时间</param>
    /// <param name="VehicleType">车辆类型</param>
    /// <param name="VehicleModels">车辆型号</param>
    /// <param name="EnergyStorageDeviceType">储能装置种类</param>
    /// <param name="DrivingMotorType">驱动电机类型</param>
    /// <param name="DriveMotorRatedPower">驱动电机额定功率</param>
    /// <param name="DriveMotorRatedSpeed">驱动电机额定转速</param>
    /// <param name="DriveMotorRatedTorque">驱动电机额定转矩</param>
    /// <param name="DriveMotorInstallationQuantity">驱动电机安装数量</param>
    /// <param name="DriveMotorArrangementType">驱动电机布置型式/位置</param>
    /// <param name="DrivingMotorCoolingMode">驱动电机冷却方式</param>
    /// <param name="DrivingRangeElectricVehicle">电动汽车续驶里程</param>
    /// <param name="MaxSpeedElectricVehicle">电动汽车最高车速</param>
    /// <param name="dldcgs">动力蓄电池包总数N</param>
    /// <param name="Id">动力蓄电池包序号</param>
    /// <param name="ProductionCode">生产厂商代码</param>
    /// <param name="BatteryTypeCode">电池类型代码</param>
    /// <param name="RatedEnergy">额定能量</param>
    /// <param name="RatedVoltage">额定电压</param>
    /// <param name="BatteryProductionDateCode">电池生产日期代码</param>
    /// <param name="SerialNumber">流水号</param>
    /// <returns></returns>
    public string GetFormatByRegister(string registime, string VehicleType, string VehicleModels, string EnergyStorageDeviceType, string DrivingMotorType, string DriveMotorRatedPower, string DriveMotorRatedSpeed, string DriveMotorRatedTorque, string DriveMotorInstallationQuantity, string DriveMotorArrangementType, string DrivingMotorCoolingMode, string DrivingRangeElectricVehicle, string MaxSpeedElectricVehicle, string dldcgs, string Id, string ProductionCode, string BatteryTypeCode, string RatedEnergy, string RatedVoltage, string BatteryProductionDateCode, string SerialNumber)
    {
        string result = "7E ";

        return result;
    }

    /// <summary>
    ///    点火与熄火时间
    /// </summary>
    /// <param name="StartupTime">启动时间</param>
    /// <param name="TurnOffTime">熄火时间</param>
    /// <returns></returns>
    public string GetFormatByUpdate1(string StartupTime, string TurnOffTime)
    {
        string result = "7E ";

        return result;
    }
    /// <summary>
    /// 累计行驶里程
    /// </summary>
    /// <param name="AccumulatedMileage">累计行驶里程</param>
    /// <returns></returns>
    public string GetFormatByUpdate2(string AccumulatedMileage)
    {
        string result = "7E ";

        return result;
    }
    /// <summary>
    /// 定位数据
    /// </summary>
    /// <param name="LocationState">定位状态</param>
    /// <param name="Longitude">经度</param>
    /// <param name="Latitude">纬度</param>
    /// <param name="Speed">速度</param>
    /// <param name="Direction">方向</param>
    /// <returns></returns>
    public string GetFormatByUpdate3(string LocationState, string Longitude, string Latitude, string Speed, string Direction)
    {
        string result = "7E ";

        return result;
    }
    /// <summary>
    /// 驱动电机数据
    /// </summary>
    /// <param name="Motor_ControllerTemp">电机控制器温度</param>
    /// <param name="Motor_Revolution">驱动电机转速</param>
    /// <param name="Motor_Temperature">驱动电机温度</param>
    /// <param name="Motor_DCCurrent">电机母线电流</param>
    /// <returns></returns>
    public string GetFormatByUpdate4(string Motor_ControllerTemp, string Motor_Revolution, string Motor_Temperature, string Motor_DCCurrent)
    {
        string result = "7E ";

        return result;
    }
    /// <summary>
    /// 车辆状态
    /// </summary>
    /// <param name="AcceleratorPedalStroke">加速踏板行程</param>
    /// <param name="BrakePedalState">制动踏板状态</param>
    /// <param name="PowerSystemReady">动力系统就绪</param>
    /// <param name="EmergencyPowerRequest">紧急下电请求</param>
    /// <param name="VehicleCurrentStatus">车辆当前状态</param>
    /// <returns></returns>
    public string GetFormatByUpdate5(string AcceleratorPedalStroke, string BrakePedalState, string PowerSystemReady, string EmergencyPowerRequest, string VehicleCurrentStatus)
    {
        string result = "7E ";

        return result;
    }
    /// <summary>
    /// 动力蓄电池包高低温数据
    /// </summary>
    /// <param name="PowerBatteryPackTotalN">动力蓄电池包总数N</param>
    /// <param name="TemperatureValuesList"></param>
    /// <returns></returns>
    public string GetFormatByUpdate6(string PowerBatteryPackTotalN, string TemperatureValuesList)
    {
        string result = "7E ";

        return result;
    }
    /// <summary>
    /// 电池包总体数据
    /// </summary>
    /// <param name="HighVoltageBatteryCurrent">高压电池电流</param>
    /// <param name="BMS_SOC">电池电量(SOC)</param>
    /// <param name="ResidualCapacity">剩余能量</param>
    /// <param name="BMS_Voltage">电池总电压</param>
    /// <param name="BMS_CellBattTemp_Max">单体最高温度</param>
    /// <param name="BMS_CellBattTemp_Min">单体最低温度</param>
    /// <param name="BMS_CellBattVoltage_Max">单体最高电压</param>
    /// <param name="BMS_CellBattVoltage_Min">单体最低电压</param>
    /// <param name="InsulationResistance">绝缘电阻值</param>
    /// <param name="BatteryBalancedActivation">电池均衡激活</param>
    /// <param name="LiquidFuelConsumption">液体燃料消耗量</param>
    /// <returns></returns>
    public string GetFormatByUpdate7(string HighVoltageBatteryCurrent, string BMS_SOC, string ResidualCapacity, string BMS_Voltage, string BMS_CellBattTemp_Max, string BMS_CellBattTemp_Min, string BMS_CellBattVoltage_Max, string BMS_CellBattVoltage_Min, string InsulationResistance, string BatteryBalancedActivation, string LiquidFuelConsumption)
    {
        string result = "7E ";

        return result;
    }
    #endregion
}
