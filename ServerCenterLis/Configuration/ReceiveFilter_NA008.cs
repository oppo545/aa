using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.Facility.Protocol;
using SuperSocket.Common;
namespace ServerCenterLis
{
    class ReceiveFilter_NA008 : FixedHeaderReceiveFilter<StringRequestInfo>
    {

        public ReceiveFilter_NA008(SwitchReceiveFilter switcher)
        : base(5)
    {

            //24 24 80 00 26 xx xx xx ... 00 0D


            //24 24 包头
            //80 主信令
            //00 26 包长
            //xx xx xx ... 00 0D 整个指 包的长度
            //xx xx xx ...  内容
            //00 校验
            //0D 包尾

        //FixedHeaderReceiveFilter - 头部格式固定并且包含内容长度的协议 [格式固定 是指长度]
        //那么 对于FixedHeaderReceiveFilter来说  24 24 80 00 26就是头部  00 26 就是被包含的长度
        //所以第一个数字为 5(字节)

        //GetBodyLengthFromHeader 方法的作用 是取到 包长
        //header byte数组 也就是 24 24 xx 00 26  
//数组下标从0开始, 那么第二和第三个数字为 3,4

        //ResolveRequestInfo 是构建StringRequestInfo对象  确定 Command 和body
        //header.Array[header.Offset]
        //因为 这里我的 Command 是直接取24  没有取 主信令 当 Command 类标示

        //所以第四个数字为 0(下标)  header.Array[header.Offset+0]


    }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            ////3 起始包长字节 ,2个字节
            //var strLen = Encoding.ASCII.GetString(header, offset + 3, 2);
            //int num = PublicMethods.Get16To10(PublicMethods.GetAsciiToHex(strLen));
            //return num;

            // 这里是指 包长
            return (int)header[offset + 3] * 256 + (int)header[offset + 4];
        }

        protected override StringRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            //var body = Encoding.ASCII.GetString(bodyBuffer, offset, length);
            //return new StringRequestInfo("23", body, new string[] { body });
            byte[] bodydata = new byte[length];
            Array.Copy(bodyBuffer, offset, bodydata, 0, length);

            StringBuilder sb = new StringBuilder();
            byte[] byteinfo = null;
            if (offset == 5) //判断为udp
            {
                byteinfo = header.Array;
            }
            else{
               byteinfo= CopyToBig(header.Array, bodydata);
            }

            foreach (byte b in byteinfo)
            {
                sb.Append(b.ToString("x").Length == 1 ? "0" + b.ToString("x") + " " : b.ToString("x") + " ");
            }

            return new StringRequestInfo("24", sb.ToString().ToUpper(), new string[] { });
        }

        //protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        //{
        //    // 这里是指 包长
        //    return (int)header[offset + 3] * 256 + (int)header[offset + 4];
        //}

        //protected override StringRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        //{
           
        //    byte[] bodydata = new byte[length];
        //    Array.Copy(bodyBuffer, offset, bodydata, 0, length);
        //    //udp 24 24 21 00 06 80 34 0A 81 18 0D   多一段
        //    //          24 24 21 00 06 80 34 0a 81 18 0d 80 34 0a 81 18 0d 
        //    //tcp 24 24 21 00 06 00 00 80 01 A6 0D  一样
        //    //      24 24 21 00 06 00 00 80 01 a6 0d 
        //    if (offset == 5)
        //    {
        //        PublicMethods.isudp = true;
        //        // UDP 直接使用 header.Array
        //        return new StringRequestInfo(header.Array[header.Offset].ToString("X"), header.Array);
        //    }
        //    else
        //    {
        //        PublicMethods.isudp = false;
        //        // TCP 合并 使用CopyToBig(header.Array, bodydata)
        //        return new StringRequestInfo(header.Array[header.Offset].ToString("X"), CopyToBig(header.Array, bodydata));
        //    }
        //}




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