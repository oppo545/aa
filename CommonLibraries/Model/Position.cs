using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    /// <summary>
    /// 一般位置数据类 9.0
    /// </summary>
public class Position
    {

        private string time;

        /// <summary>
        /// 　年月日时分秒  6字节
        ///08年6月5日	14点26分34秒表示为：08H,06H,05H,104H,26H,34H
        /// 例:08 06 05 14 26 34 		
        /// </summary>
        public string Time
        {
            get { return time; }
            set { time = value; }
        }


        private string latitude;

        /// <summary>
        /// 纬度 4字节
        /// 例:北纬22度32.556分表示为： 02 23 25 56
        /// </summary>
        public string Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }


        private string longitude;

        /// <summary>
        /// 经度   4个字节
        /// 东经114度05.281分表示为：（注意：1度=60分）
        /// 例:11 40 52 81
        /// </summary>
        public string Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }


        private string speed;

        /// <summary>
        /// 速度 2个字节 
        /// 例:120公里/小时表示为 01 20
        /// </summary>
        public string Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        private string direction;

        /// <summary>
        /// 方向 2个字节
        /// 例:154度表示为: 01 54
        /// </summary>
        public string Direction
        {
            get { return direction; }
            set { direction = value; }
        }



        private string positioning;

        /// <summary>
        /// 定位 1个字节
        /// 例:f8
        /// </summary>
        public string Positioning
        {
            get { return positioning; }
            set { positioning = value; }
        }




        private string mileage;
        /// <summary>
        /// 里程数 3字节
        /// 例:00 4E 20
        /// </summary>
        public string Mileage
        {
            get { return mileage; }
            set { mileage = value; }
        }


        //Ａ　Ｂ　Ｃ　Ｄ
        //7F FC 5F 00

        private string a;

        /// <summary>
        /// A 1字节
        /// 例:7F
        /// </summary>
        public string A
        {
            get { return a; }
            set { a = value; }
        }



        private string b;

        /// <summary>
        /// Ｂ 1字节
        /// 例:FC
        /// </summary>
        public string B
        {
            get { return b; }
            set { b = value; }
        }


        private string c;

        /// <summary>
        /// C 1字节
        /// 例:5F
        /// </summary>
        public string C
        {
            get { return c; }
            set { c = value; }
        }

        private string d;

        /// <summary>
        /// D 1字节
        /// 例:00
        /// </summary>
        public string D
        {
            get { return d; }
            set { d = value; }
        }


        private string ww;

        /// <summary>
        /// W W
        /// :00 3c
        /// </summary>
        public string Ww
        {
            get { return ww; }
            set { ww = value; }
        }

        private string e;
        /// <summary>
        /// 停车超时时间   1个字节 单位分,如10分，表示为0x0A 
        /// :00
        /// </summary>
        public string E
        {
            get { return e; }
            set { e = value; }
        }

        private string r;

        /// <summary>
        ///  超速设置门阀   1个字节 单位公里/小时,如100公里/小时，表示为0x64
        ///  :00
        /// </summary>
        public string R
        {
            get { return r; }
            set { r = value; }
        }


        private string t;

        /// <summary>
        ///  电子围栏设置个数 1个字节 HEX表示，0~0xFF
        ///  :00
        /// </summary>
        public string T
        {
            get { return t; }
            set { t = value; }
        }

        private string y;

        /// <summary>
        /// 登签ID（2） 与登签ID（1）组成登签ID号 
        /// 1个字节 00
        /// </summary>
        public string Y
        {
            get { return y; }
            set { y = value; }
        }


        private string u;

        /// <summary>
        /// 保留 
        /// 1个字节 00
        /// </summary>
        public string U
        {
            get { return u; }
            set { u = value; }
        }


        private string i;

        /// <summary>
        /// 中心下发的主命令  1个字节
        /// </summary>
        public string I
        {
            get { return i; }
            set { i = value; }
        }

        public string Kz { get; set; }

        public string Cskg { get; set; }

        public string Ad { get; set; }

    }
