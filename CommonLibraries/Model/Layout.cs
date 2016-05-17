using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    /// <summary>
    /// 格式类
    /// </summary>
public class Layout
    {

        public Layout()
        {
            this.La = "24 24";
        }

        private string la;

        /// <summary>
        /// 包头
        /// </summary>
        public string La
        {
            get { return la; }
            set { la = value; }
        }

        private string lb;
        /// <summary>
        /// 主信令
        /// </summary>
        public string Lb
        {
            get { return lb; }
            set { lb = value; }
        }


        private string lc;

        /// <summary>
        /// 包长
        /// </summary>
        public string Lc
        {
            get { return lc; }
            set { lc = value; }
        }

        private string ld;

        /// <summary>
        /// 伪ip
        /// </summary>
        public string Ld
        {
            get { return ld; }
            set { ld = value; }
        }

        private string le;
        /// <summary>
        /// 校验
        /// </summary>
        public string Le
        {
            get { return le; }
            set { le = value; }
        }

        private string lf;
        /// <summary>
        /// 包尾
        /// </summary>
        public string Lf
        {
            get { return lf; }
            set { lf = value; }
        }


        private string wlgs;
        /// <summary>
        /// 围栏个数 最大支持60个
        /// </summary>
        public string Wlgs
        {
            get { return wlgs; }
            set { wlgs = value; }
        }


        private List<Fence> lis_Fe;

        /// <summary>
        /// 围栏内容
        /// </summary>
        internal List<Fence> Lis_Fe
        {
            get { return lis_Fe; }
            set { lis_Fe = value; }
        }

    }
