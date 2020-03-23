using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
namespace Utilities
{
    public class Common
    {
        public static string SuccessCode = "00";
        public static string Success = "success";
        public static string NoDataCode = "01";
        public static string NoData = "no data";
        public static string FailCode = "99";
        public static string Fail = "fail";
        public static string Failed = "failed";
     
        public static HttpContext Context
        {
            get { return HttpContext.Current; }
        }

        public static object WebConfigurationManager { get; private set; }

        public static void SignOut()
        {
            FormsAuthentication.SignOut();

            HttpCookie cookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-1);
                Context.Response.AppendCookie(cookie);
            }

            Context.Session.Abandon();

        }

        public static void SetAuthenticationCookie(int id, string username, string password, bool isPersit)
        {

            var Authticket = new
                FormsAuthenticationTicket(1,
                    username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(1200),
                    true,
                    string.Format("{0};{1};{2}", id, username, password),
                    FormsAuthentication.FormsCookiePath);

            string hash = FormsAuthentication.Encrypt(Authticket);



            var Authcookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash)
            {
                HttpOnly = true,
                Path = FormsAuthentication.FormsCookiePath,
                Domain = FormsAuthentication.CookieDomain
            };

            if (Authticket.IsPersistent) Authcookie.Expires = Authticket.Expiration;

            FormsAuthentication.SetAuthCookie(username, isPersit);

            Context.Response.AppendCookie(Authcookie);

        }

        public static string Md5Hash(string value)
        {
            return Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(new UTF8Encoding().GetBytes(value)));
        }

        public static T SafetyCast<T>(object value, T defaultValue)
        {
            try
            {
                if (value == null || value is DBNull) return defaultValue;
                if (typeof(T).BaseType == typeof(Enum))
                    return (T)Enum.Parse(typeof(T), value.ToString(), true);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }


        public static List<string> GetListDirectory(string path, string searchPattern = "*")
        {
            try{
                return Directory.GetDirectories(path,searchPattern).ToList();
            }catch(UnauthorizedAccessException){
                return new List<string>();
            }
        }

        public static List<string> GetDirectories(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (searchOption == SearchOption.TopDirectoryOnly)
            {
                return Directory.GetDirectories(path, searchPattern).ToList();
            }
            var directories = new List<string>(GetDirectories(path, searchPattern));
            return directories;
        }
    }
}
