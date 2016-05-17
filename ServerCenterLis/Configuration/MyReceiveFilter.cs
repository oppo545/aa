using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.Facility.Protocol;

namespace ServerCenterLis
{
    class MyReceiveFilter : BeginEndMarkReceiveFilter<StringRequestInfo>
    {
        //开始和结束标记也可以是两个或两个以上的字节
        //         //0x7E
        private readonly static byte[] BeginMark = new byte[] { 0x7E };
        private readonly static byte[] EndMark = new byte[] { 0x7E };

        public MyReceiveFilter()
            : base(BeginMark, EndMark) //传入开始标记和结束标记
        {

        }


        protected override StringRequestInfo ProcessMatchedRequest(byte[] readBuffer, int offset, int length)
        {
        
            StringBuilder sb = new StringBuilder();
            foreach (byte b in readBuffer)
            {
                sb.Append(b.ToString("x").Length == 1 ? "0" + b.ToString("x") + " " : b.ToString("x") + " ");
            }

            return new StringRequestInfo("__", sb.ToString().ToUpper(), new string[] { });
        }


    }
    
}