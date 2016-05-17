using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    /// <summary>
    /// 一般位置数据类
    /// </summary>
public class Cls_Position
    {

        private string p_mark;

        /// <summary>
        /// 报警标志
        /// </summary>
        public string P_mark
        {
            get { return p_mark; }
            set { p_mark = value; }
        }


        private string p_state;

        /// <summary>
        /// 状态
        /// </summary>
        public string P_state
        {
            get { return p_state; }
            set { p_state = value; }
        }

        private double p_latitude;

        /// <summary>
        /// 纬度
        /// </summary>
        public double P_latitude
        {
            get { return p_latitude; }
            set { p_latitude = value; }
        }


        private double p_longitude;

        /// <summary>
        /// 经度
        /// </summary>
        public double P_longitude
        {
            get { return p_longitude; }
            set { p_longitude = value; }
        }



        private string p_elevation;

        /// <summary>
        /// 高程
        /// </summary>
        public string P_elevation
        {
            get { return p_elevation; }
            set { p_elevation = value; }
        }

        private string p_speed;

        /// <summary>
        /// 速度
        /// </summary>
        public string P_speed
        {
            get { return p_speed; }
            set { p_speed = value; }
        }

        private string p_direction;

        /// <summary>
        /// 方向
        /// </summary>
        public string P_direction
        {
            get { return p_direction; }
            set { p_direction = value; }
        }

        private string p_time;

        /// <summary>
        /// 时间
        /// </summary>
        public string P_time
        {
            get { return p_time; }
            set { p_time = value; }
        }



        private string p_mileage;

        /// <summary>
        /// 里程
        /// </summary>
        public string P_mileage
        {
            get { return p_mileage; }
            set { p_mileage = value; }
        }

        private string p_oil;

        /// <summary>
        /// 油量
        /// </summary>
        public string P_oil
        {
            get { return p_oil; }
            set { p_oil = value; }
        }

        private string p_speedx;

        /// <summary>
        /// 行驶记录功能获取的速度
        /// </summary>
        public string P_speedx
        {
            get { return p_speedx; }
            set { p_speedx = value; }
        }



        private string p_rid;

        /// <summary>
        /// 需要人工确认报警事件的id
        /// </summary>
        public string P_rid
        {
            get { return p_rid; }
            set { p_rid = value; }
        }


        private string p_csspeend;

        /// <summary>
        /// 超速报警附加信息
        /// </summary>
        public string P_csspeend
        {
            get { return p_csspeend; }
            set { p_csspeend = value; }
        }

        private string csspeend_wzlx;
        /// <summary>
        /// _超速报警位置类型
        /// </summary>
        public string Csspeend_wzlx
        {
            get { return csspeend_wzlx; }
            set { csspeend_wzlx = value; }
        }


        private string csspeend_qyid;
        /// <summary>
        ///_超速区域或路段id
        /// </summary>
        public string Csspeend_qyid
        {
            get { return csspeend_qyid; }
            set { csspeend_qyid = value; }
        }



        private string p_jclx;

        /// <summary>
        /// 进出区域/路线报警
        /// </summary>
        public string P_jclx
        {
            get { return p_jclx; }
            set { p_jclx = value; }
        }

        private string jclx_wzlx;
        /// <summary>
        /// _进出区域/路线报警附加 位置类型
        /// </summary>
        public string Jclx_wzlx
        {
            get { return jclx_wzlx; }
            set { jclx_wzlx = value; }
        }

        private string jclx_qyid;
        /// <summary>
        /// _区域或线路id
        /// </summary>
        public string Jclx_qyid
        {
            get { return jclx_qyid; }
            set { jclx_qyid = value; }
        }

        private string jclx_Angle;
        /// <summary>
        /// _方向
        /// </summary>
        public string Jclx_Angle
        {
            get { return jclx_Angle; }
            set { jclx_Angle = value; }
        }



        private string p_ldgc;

        /// <summary>
        /// 路段行驶时间不足/过长报警
        /// </summary>
        public string P_ldgc
        {
            get { return p_ldgc; }
            set { p_ldgc = value; }
        }

        private string ldgc_ldid;
        /// <summary>
        /// _路段行驶时间不足/过长报警路段id
        /// </summary>
        public string Ldgc_ldid
        {
            get { return ldgc_ldid; }
            set { ldgc_ldid = value; }
        }

        private string ldgc_time;
        /// <summary>
        /// _路段行驶时间 单位为s
        /// </summary>
        public string Ldgc_time
        {
            get { return ldgc_time; }
            set { ldgc_time = value; }
        }

        private string ldgc_result;
        /// <summary>
        /// _结果
        /// </summary>
        public string Ldgc_result
        {
            get { return ldgc_result; }
            set { ldgc_result = value; }
        }




        private string p_kzcl;

        /// <summary>
        /// 扩展车辆信号状态位
        /// </summary>
        public string P_kzcl
        {
            get { return p_kzcl; }
            set { p_kzcl = value; }
        }


        private string p_ios;

        /// <summary>
        /// Io状态位
        /// </summary>
        public string P_ios
        {
            get { return p_ios; }
            set { p_ios = value; }
        }

        private string io_sdxm;
        /// <summary>
        /// _Io状态位	深度休眠状态
        /// </summary>
        public string Io_sdxm
        {
            get { return io_sdxm; }
            set { io_sdxm = value; }
        }

        private string io_xm;
        /// <summary>
        /// _ 休眠状态
        /// </summary>
        public string Io_xm
        {
            get { return io_xm; }
            set { io_xm = value; }
        }



        private string p_mnl;

        /// <summary>
        /// 模拟量
        /// </summary>
        public string P_mnl
        {
            get { return p_mnl; }
            set { p_mnl = value; }
        }

        private string mol_ad0;
        /// <summary>
        /// _模拟量ad0
        /// </summary>
        public string Mol_ad0
        {
            get { return mol_ad0; }
            set { mol_ad0 = value; }
        }
        private string mol_ad1;
        /// <summary>
        /// _模拟量ad1
        /// </summary>
        public string Mol_ad1
        {
            get { return mol_ad1; }
            set { mol_ad1 = value; }
        }

        private string p_wxqd;

        /// <summary>
        /// 无线通信网络信号强度
        /// </summary>
        public string P_wxqd
        {
            get { return p_wxqd; }
            set { p_wxqd = value; }
        }

        private string p_gnss;

        /// <summary>
        /// GNSS定位卫星数
        /// </summary>
        public string P_gnss
        {
            get { return p_gnss; }
            set { p_gnss = value; }
        }



      
    }
