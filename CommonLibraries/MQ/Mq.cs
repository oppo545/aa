using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public interface Mq
    {
        /// <summary>
        /// 入库队列
        /// </summary>
        /// <param name="info"></param>
        void SendMsg0(string info);

        /// <summary>
        /// 入库队列
        /// </summary>
        /// <param name="info"></param>
        void SendMsg(string info);


        /// <summary>
        /// 车载设备指令回复  [发送给中心服务器,Can]
        /// </summary>
        /// <param name="info"></param>
        void SendMsg2(string info);

        /// <summary>
        /// 车载设备上传 GPS+上线+回复 [发送给中心服务器]
        /// </summary>
        /// <param name="info"></param>
        void SendMsg3(string info);

        /// <summary>
        /// 下发指令回复小程序
        /// </summary>
        /// <param name="info"></param>
        void SendMsg4(string info);

        /// <summary>
        /// 车载设备上传 alarm [发送给中心服务器]
        /// </summary>
        /// <param name="info"></param>
        void SendMsgByAlarm(string info);

    }
