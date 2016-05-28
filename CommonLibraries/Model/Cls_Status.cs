using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    /// <summary>
    /// 状态位定义
    /// </summary>
public class Cls_Status
    {
        private string cs_acc;
        /// <summary>
        /// acc 0关 1开
        /// </summary>
        public string Cs_acc
        {
            get { return cs_acc; }
            set { cs_acc = value; }
        }
        private string cs_islocation;
        /// <summary>
        /// 0未定位 1定位
        /// </summary>
        public string Cs_islocation
        {
            get { return cs_islocation; }
            set { cs_islocation = value; }
        }
        private string cs_northorsouth;
        /// <summary>
        /// 0北纬 1南纬
        /// </summary>
        public string Cs_northorsouth
        {
            get { return cs_northorsouth; }
            set { cs_northorsouth = value; }
        }
        private string cs_eastorwest;
        /// <summary>
        /// 0东经 1西经
        /// </summary>
        public string Cs_eastorwest
        {
            get { return cs_eastorwest; }
            set { cs_eastorwest = value; }
        }
        private string cs_operate;
        /// <summary>
        /// 0运营状态 1停运状态
        /// </summary>
        public string Cs_operate
        {
            get { return cs_operate; }
            set { cs_operate = value; }
        }
        private string cs_Latilongencry;
        /// <summary>
        /// 0经纬度未经保密插件加密 1经纬度已经保密插件加密
        /// </summary>
        public string Cs_Latilongencry
        {
            get { return cs_Latilongencry; }
            set { cs_Latilongencry = value; }
        }
        private string cs_loading;
        /// <summary>
        /// 00空车 01半载 10保留 11满载
        /// </summary>
        public string Cs_loading
        {
            get { return cs_loading; }
            set { cs_loading = value; }
        }
        private string cs_oil;
        /// <summary>
        /// 0车辆油路正常 1车辆油路断开
        /// </summary>
        public string Cs_oil
        {
            get { return cs_oil; }
            set { cs_oil = value; }
        }
        private string cs_circuitry;

        /// <summary>
        /// 0车辆电路正常 1车辆电路断开
        /// </summary>
        public string Cs_circuitry
        {
            get { return cs_circuitry; }
            set { cs_circuitry = value; }
        }
        private string cs_dool;
        /// <summary>
        /// 0车门解锁 1车门加锁
        /// </summary>
        public string Cs_dool
        {
            get { return cs_dool; }
            set { cs_dool = value; }
        }
        private string cs_dool1;
        /// <summary>
        /// 0门1关 1门1开[前门]
        /// </summary>
        public string Cs_dool1
        {
            get { return cs_dool1; }
            set { cs_dool1 = value; }
        }
        private string cs_dool2;
        /// <summary>
        /// 0门2关 1门2开[中门]
        /// </summary>
        public string Cs_dool2
        {
            get { return cs_dool2; }
            set { cs_dool2 = value; }
        }
        private string cs_dool3;
        /// <summary>
        /// 0门3关 1门3开[后门]
        /// </summary>
        public string Cs_dool3
        {
            get { return cs_dool3; }
            set { cs_dool3 = value; }
        }
        private string cs_dool4;
        /// <summary>
        /// 0门4关 1门4开[驾驶席门]
        /// </summary>
        public string Cs_dool4
        {
            get { return cs_dool4; }
            set { cs_dool4 = value; }
        }
        private string cs_dool5;
        /// <summary>
        /// 0门5关 1门5开[自定义]
        /// </summary>
        public string Cs_dool5
        {
            get { return cs_dool5; }
            set { cs_dool5 = value; }
        }
        private string cs_isgpsloca;
        /// <summary>
        /// 0未使用gps卫星进行定位 1使用gps卫星进行定位
        /// </summary>
        public string Cs_isgpsloca
        {
            get { return cs_isgpsloca; }
            set { cs_isgpsloca = value; }
        }
        private string cs_isbdwxloca;
        /// <summary>
        /// 0未使用北斗卫星进行定位 1使用北斗卫星进行定位
        /// </summary>
        public string Cs_isbdwxloca
        {
            get { return cs_isbdwxloca; }
            set { cs_isbdwxloca = value; }
        }
        private string cs_isnassloca;
        /// <summary>
        /// 0未使用GLONASS卫星进行定位 1使用GLONASS卫星进行定位
        /// </summary>
        public string Cs_isnassloca
        {
            get { return cs_isnassloca; }
            set { cs_isnassloca = value; }
        }
        private string cs_isgaliloca;
        /// <summary>
        /// 0未使用Galileo卫星进行定位 1使用Galileo卫星进行定位
        /// </summary>
        public string Cs_isgaliloca
        {
            get { return cs_isgaliloca; }
            set { cs_isgaliloca = value; }
        }

    }
