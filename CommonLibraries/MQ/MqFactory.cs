using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

   public  class MqFactory
    {

       public static Mq CreateInstance(string name)
       {
           switch (name.Trim().ToLower())
           {
               case "msmq":
                   return  MsMq.CreateInstance();
               case "activemq":
                   return MqActive.CreateInstance();
               default:
                   throw new Exception("no found class");
           }
       }

    }
