using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Protocol;

namespace ServerCenterLis
{
    public class SwitchReceiveFilter : IReceiveFilter<StringRequestInfo>
    {
        private IReceiveFilter<StringRequestInfo> m_FilterHM;
        private byte m_BeginMarkHM = 0x23;

        private IReceiveFilter<StringRequestInfo> m_FilterNA088;
        private byte m_BeginMarkNA088 = 0x7E;

        private IReceiveFilter<StringRequestInfo> m_FilterNA008;
        private byte m_BeginMarkNA008 = 0x24;

        public SwitchReceiveFilter()
        {
           m_FilterHM = new ReceiveFilter_HM(this);
           m_FilterNA088 = new ReceiveFilter_NA088(this);

           m_FilterNA008 = new ReceiveFilter_NA008(this);
        }

        public StringRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = length;
            var flag = readBuffer[offset];

            if (flag == m_BeginMarkNA088) //部标_龙安 088  7E
                 NextReceiveFilter = m_FilterNA088;
            else if (flag == m_BeginMarkNA008) //部标_龙安 008  24  TCP|UDP
                NextReceiveFilter = m_FilterNA008;
            else if (flag == m_BeginMarkHM) //地标_海马 23  TCP
                 NextReceiveFilter = m_FilterHM;
            else
                State = FilterState.Error;

            return null;
        }


        public int LeftBufferSize { get; private set; }

        public IReceiveFilter<StringRequestInfo> NextReceiveFilter { get; private set; }

        public IReceiveFilter<BinaryRequestInfo> NextReceiveFilterBri { get; private set; }

        public void Reset()
        {

        }

        public FilterState State { get; private set; }
    }
}
