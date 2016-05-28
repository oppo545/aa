using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.Facility.Protocol;
using SuperSocket.Common;
namespace ServerCenterLis
{
    class ReceiveFilter_HM : FixedHeaderReceiveFilter<StringRequestInfo>
    {

        public ReceiveFilter_HM(SwitchReceiveFilter switcher)
        : base(24)
    {
    }

        //protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        //{
        //    // 这里是指 包长, 报文的包长 没有包含校验码 所有 +1 <<SuperSocket 1.6.4 通过FixedHeaderReceiveFilter解析自定义协议 - Freiheit - 博客园.htm>>
        //    return (int)header[offset + 22] * 256 + (int)header[offset + 23]+1;
        //}

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            //23 起始包长字节 ,2个字节
            //var strLen = Encoding.ASCII.GetString(header, offset + 22, 2);
            //int num = PublicMethods.Get16To10(PublicMethods.GetAsciiToHex(strLen));
            ////因为包长未包含校验值,所以加1
            //return num + 1;

            //TODO 
            //PublicMethods.GetAsciiToHex  将 0  给过滤了

            return (int)header[offset + 22] * 256 + (int)header[offset + 23] + 1;
        }

        protected override StringRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            //var body = Encoding.ASCII.GetString(bodyBuffer, offset, length);
            //return new StringRequestInfo("23", body, new string[] { body });
            byte[] bodydata = new byte[length];
            Array.Copy(bodyBuffer, offset, bodydata, 0, length);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in CopyToBig(header.Array, bodydata))
            {
                sb.Append(b.ToString("x").Length == 1 ? "0" + b.ToString("x") + " " : b.ToString("x") + " ");
            }

            return new StringRequestInfo("23", sb.ToString().ToUpper(), new string[] { });
        }

      
        #region 两个byte数组 追加
        public byte[] getbyte(byte[] a, byte[] b)
        {
            Array.Resize(ref a, a.Length + b.Length);
            b.CopyTo(a, a.Length - b.Length);
            return a;
        }

        private byte[] CopyToBig(byte[] bBig, byte[] bSmall)
        {
            byte[] tmp = new byte[bBig.Length + bSmall.Length];
            System.Buffer.BlockCopy(bBig, 0, tmp, 0, bBig.Length);
            System.Buffer.BlockCopy(bSmall, 0, tmp, bBig.Length, bSmall.Length);
            return tmp;
        }
        #endregion

    }
    
}