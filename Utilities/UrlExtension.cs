using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Utilities
{
    public static class UrlExtention
    {
        public static string ToImageLink(this string url, bool isWeb = false)
        {
            if (string.IsNullOrEmpty(url))
            {
                return WebConfigurationManager.AppSettings["DefaultImg"];
            }

            if (!url.StartsWith("http"))
            {
                var baseUrl = "";
                if (isWeb)
                {

                    baseUrl = WebConfigurationManager.AppSettings["WebUrl"];
                }
                else
                {
                    baseUrl = WebConfigurationManager.AppSettings["baseUrl"];

                }
                return string.Format("{0}/{1}", baseUrl, url);
            }

            return url;
        }
        public static string ToImageWebLink(this string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return WebConfigurationManager.AppSettings["DefaultImg"];
            }

            if (!url.StartsWith("http"))
            {
                return string.Format("{0}{1}", WebConfigurationManager.AppSettings["WebUrl"], url);
            }

            return url;
        }
        public static string LinkRedirect(this string id, bool isProfs)
        {
            if (isProfs)
            {
                return "/ManagerProfs/ViewDetails/" + id;
            }
            return "/ManagerCustomer/Detail/" + id;
        }
    }
}
