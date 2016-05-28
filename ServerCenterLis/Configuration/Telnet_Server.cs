using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using System.Threading;



namespace ServerCenterLis
{

    public class Telnet_Server : AppServer<Telnet_Session>
    {

        //自定义命令行协议
        //public Telnet_Server()
        //    : base(new CommandLineReceiveFilterFactory(Encoding.Default, new BasicRequestInfoParser(":", ",")))
        //{

        //}
        //public Telnet_Server()
        //    // : base(new CommandLineReceiveFilterFactory(Encoding.Default, new BasicRequestInfoParser()))
        //    : base(new TerminatorReceiveFilterFactory("7E"))
        //{

        //}

        public Telnet_Server()
            : base(new DefaultReceiveFilterFactory<SwitchReceiveFilter, StringRequestInfo>())
        {

        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            return base.Setup(rootConfig, config);
        }

        protected override void OnStartup()
        {
    
            WriteLog.WriteOrdersLog("BMK_Telnet_Server_OnStartup");
            base.OnStartup();
        }

        protected override void OnStopped()
        {
            //全体下线
            string strinfo = "update vehiclerunninginfo.onlinerecodenow set Acc=0 , IsOnline=0 where IsOnline=1";
            new MsMq().SendMsg(strinfo);
            WriteLog.WriteOrdersLog("BMK_Telnet_Server_OnStopped");
            base.OnStopped();
        }


    }
}
