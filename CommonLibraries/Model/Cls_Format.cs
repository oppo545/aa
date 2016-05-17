using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    /// <summary>
    ///格式位
    /// </summary>
   public class Cls_Format
    {

        private string xxid;

        /// <summary>
        /// 消息id
        /// </summary>
        public string Xxid
        {
            get { return xxid; }
            set { xxid = value; }
        }


        private string xxtcd;
        /// <summary>
        /// 消息体长度
        /// </summary>
        public string Xxtcd
        {
            get { return xxtcd; }
            set { xxtcd = value; }
        }

        private string simno_y;

        /// <summary>
        /// sim卡号 原始 [01 00 00...]
        /// </summary>
        public string Simno_y
        {
            get { return simno_y; }
            set { simno_y = value; }
        }

       

        private string simno;

        /// <summary>
        /// sim卡号 [11位数据:12345678910]
        /// </summary>
        public string Simno
        {
            get { return simno; }
            set { simno = value; }
        }


        private string xxlsh;
        /// <summary>
        /// 消息流水号
        /// </summary>
        public string Xxlsh
        {
            get { return xxlsh; }
            set { xxlsh = value; }
        }

    }



