using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibraries
{
    [Serializable]
    public class VehicleInfo
    {
        ///// <summary>
        ///// 是否为补传
        ///// </summary>
        //public bool isSupplement = false;

        /// <summary>
        /// 是否为充电状态
        /// </summary>
        public bool isCharging = false;
        /// <summary>
        /// 是否为充电桩 （暂时未用)
        /// </summary>
        public bool isChargPile = false;
        /// <summary>
        /// 是否为web下发指令
        /// </summary>
        public bool isSet = false;
        //0 关， 1  开
        public string isacc = "0";

        /// <summary>
        /// 系统识别码
        /// System identification code
        /// </summary>
        public string siCode;
        /// <summary>
        /// 车型
        /// </summary>
        public int vlsType = -1;

        /// <summary>
        ///记录 是否激活
        /// </summary>
        public bool isActivate = false;

        /// <summary>
        ///记录 设备信息 是否入库 
        /// </summary>
        public bool isDeviceRecord = false;

        /// <summary>
        ///记录 设备信息 入库 是否 完整  
        /// </summary>
        public bool isCompleteRecord = false;
        /// <summary>
        /// 上一条 有效 经度
        /// </summary>
        public double EffectiveLong = 0;
        /// <summary>
        /// 上一条 有效 纬度
        /// </summary>
        public double EffectiveLat = 0;

        /// <summary>
        /// The lastcantime
        /// </summary>
        public string Lastcantime="";
    }
}
