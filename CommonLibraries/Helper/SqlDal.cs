using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibraries
{
    public class SqlDal
    {
        private static SqlDal sqldal;
        public static SqlDal CreateInstend()
        {
            if (sqldal == null)
            {
                sqldal = new SqlDal();
            }
            return sqldal;
        }
        public string webserviceurl = System.Configuration.ConfigurationManager.AppSettings["webservice"];
        public string methodname = System.Configuration.ConfigurationManager.AppSettings["methodname"]; 
        public bool IsDbExists(string systemno)
        {
            object[] args = new object[1];
            args[0] = systemno;
            object result = WebServiceHelper.InvokeWebService(webserviceurl, methodname, args);//路径，方法名，参数  
            return ((string)result).Contains("VehicleNo");
        }
    }
}
