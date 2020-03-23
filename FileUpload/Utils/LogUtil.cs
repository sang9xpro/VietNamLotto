using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileUpload.Utils
{
    public class LogUtil
    {
        private static readonly log4net.ILog Writer = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static void WriteError(string className, string message )
        {
            LogUtil.Writer.Error(className +  " with log: "+ message);
        }


    }
}