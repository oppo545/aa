using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Fence
    {
        private string a;
        /// <summary>
        /// 小纬 4字节
        /// </summary>
        public string A
        {
            get { return a; }
            set { a = value; }
        }
        private string b;
        /// <summary>
        /// 小经 4字节
        /// </summary>
        public string B
        {
            get { return b; }
            set { b = value; }
        }
        private string c;
        /// <summary>
        /// 大纬 4字节
        /// </summary>
        public string C
        {
            get { return c; }
            set { c = value; }
        }
        private string d;
        /// <summary>
        /// 大经 4字节
        /// </summary>
        public string D
        {
            get { return d; }
            set { d = value; }
        }
        private string e;
        /// <summary>
        /// 围栏号 1字节 范围为1~255
        /// </summary>
        public string E
        {
            get { return e; }
            set { e = value; }
        }
        private string f;
        /// <summary>
        /// 围栏报警方式 1字节
        /// </summary>
        public string F
        {
            get { return f; }
            set { f = value; }
        }




    }
