using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    /// <summary>
    /// 报警标志位定义
    /// </summary>
public class Cls_Warning
    {
        private string cw_jjbj;

        /// <summary>
        /// 紧急报警
        /// </summary>
        public string Cw_jjbj
        {
            get { return cw_jjbj; }
            set { cw_jjbj = value; }
        }


        private string cw_csbj;

        /// <summary>
        /// //超速报警
        /// </summary>
        public string Cw_csbj
        {
            get { return cw_csbj; }
            set { cw_csbj = value; }
        }


        private string cw_pljs;
        /// <summary>
        /// //疲劳驾驶
        /// </summary>
        public string Cw_pljs
        {
            get { return cw_pljs; }
            set { cw_pljs = value; }
        }

        private string cw_wxyj;
        /// <summary>
        /// //危险预警
        /// </summary>
        public string Cw_wxyj
        {
            get { return cw_wxyj; }
            set { cw_wxyj = value; }
        }

        private string cw_gnns_gz;
        /// <summary>
        /// //gnss模块发生故障
        /// </summary>
        public string Cw_gnns_gz
        {
            get { return cw_gnns_gz; }
            set { cw_gnns_gz = value; }
        }

        private string cw_gnns_tx_wjjd;
        /// <summary>
        /// //gnss天线未接或被剪断
        /// </summary>
        public string Cw_gnns_tx_wjjd
        {
            get { return cw_gnns_tx_wjjd; }
            set { cw_gnns_tx_wjjd = value; }
        }

        private string cw_gnns_tx_dl;
        /// <summary>
        /// //gnss天线短路
        /// </summary>
        public string Cw_gnns_tx_dl
        {
            get { return cw_gnns_tx_dl; }
            set { cw_gnns_tx_dl = value; }
        }

        private string cw_zdzdy_qy;
        /// <summary>
        /// //终端主电源欠压
        /// </summary>
        public string Cw_zdzdy_qy
        {
            get { return cw_zdzdy_qy; }
            set { cw_zdzdy_qy = value; }
        }

        private string cw_zdzdy_dd;
        /// <summary>
        /// //终端主电源掉电
        /// </summary>
        public string Cw_zdzdy_dd
        {
            get { return cw_zdzdy_dd; }
            set { cw_zdzdy_dd = value; }
        }

        private string cw_zdxs;
        /// <summary>
        /// //终端lcd或显示屏故障
        /// </summary>
        public string Cw_zdxs
        {
            get { return cw_zdxs; }
            set { cw_zdxs = value; }
        }

        private string cw_tts;
        /// <summary>
        /// //tts 模块故障
        /// </summary>
        public string Cw_tts
        {
            get { return cw_tts; }
            set { cw_tts = value; }
        }

        private string cw_sxdgz;
        /// <summary>
        /// //摄像头故障
        /// </summary>
        public string Cw_sxdgz
        {
            get { return cw_sxdgz; }
            set { cw_sxdgz = value; }
        }

        private string cw_icmk;
        /// <summary>
        /// //道路运输证ic卡模块故障
        /// </summary>
        public string Cw_icmk
        {
            get { return cw_icmk; }
            set { cw_icmk = value; }
        }

        private string cw_csyj;
        /// <summary>
        /// //超速预警
        /// </summary>
        public string Cw_csyj
        {
            get { return cw_csyj; }
            set { cw_csyj = value; }
        }

        private string cw_pljsyj;
/// <summary>
        /// //疲劳驾驶预警
/// </summary>
        public string Cw_pljsyj
        {
            get { return cw_pljsyj; }
            set { cw_pljsyj = value; }
        }

        private string cw_cslj;
        /// <summary>
        /// //当天累计驾驶超时
        /// </summary>
        public string Cw_cslj
        {
            get { return cw_cslj; }
            set { cw_cslj = value; }
        }

        private string cw_cstc;
        /// <summary>
        /// //超时停车
        /// </summary>
        public string Cw_cstc
        {
            get { return cw_cstc; }
            set { cw_cstc = value; }
        }

        private string cw_jcqy;
        /// <summary>
        /// //进出区域
        /// </summary>
        public string Cw_jcqy
        {
            get { return cw_jcqy; }
            set { cw_jcqy = value; }
        }

        private string cw_jclx;
        /// <summary>
        /// //进出路线
        /// </summary>
        public string Cw_jclx
        {
            get { return cw_jclx; }
            set { cw_jclx = value; }
        }

        private string cw_ldxs;
        /// <summary>
        /// //路段行驶时间不足/过长
        /// </summary>
        public string Cw_ldxs
        {
            get { return cw_ldxs; }
            set { cw_ldxs = value; }
        }

        private string cw_lxpl;
        /// <summary>
        /// //路线偏离报警
        /// </summary>
        public string Cw_lxpl
        {
            get { return cw_lxpl; }
            set { cw_lxpl = value; }
        }



        private string cw_vss;
        /// <summary>
        /// //车辆vss故障
        /// </summary>
        public string Cw_vss
        {
            get { return cw_vss; }
            set { cw_vss = value; }
        }

        private string cw_clyl;
        /// <summary>
        /// //车辆油量异常
        /// </summary>
        public string Cw_clyl
        {
            get { return cw_clyl; }
            set { cw_clyl = value; }
        }


        private string cw_clbd;
        /// <summary>
        /// //车辆被盗(通过车辆防盗器)
        /// </summary>
        public string Cw_clbd
        {
            get { return cw_clbd; }
            set { cw_clbd = value; }
        }


        private string cw_clffdh;
        /// <summary>
        /// //车辆非法点火
        /// </summary>
        public string Cw_clffdh
        {
            get { return cw_clffdh; }
            set { cw_clffdh = value; }
        }

        private string cw_clffwy;
        /// <summary>
        /// //车辆非法位移
        /// </summary>
        public string Cw_clffwy
        {
            get { return cw_clffwy; }
            set { cw_clffwy = value; }
        }

        private string cw_pzyj;
        /// <summary>
        /// //碰撞预警
        /// </summary>
        public string Cw_pzyj
        {
            get { return cw_pzyj; }
            set { cw_pzyj = value; }
        }


        private string cw_cfyj;
        /// <summary>
        /// //侧翻预警
        /// </summary>
        public string Cw_cfyj
        {
            get { return cw_cfyj; }
            set { cw_cfyj = value; }
        }
        private string cw_ffkm;
        /// <summary>
        /// //非法开门预警
        /// </summary>
        public string Cw_ffkm
        {
            get { return cw_ffkm; }
            set { cw_ffkm = value; }
        }
       

    }
