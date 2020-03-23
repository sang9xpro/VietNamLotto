using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FileUpload.Utils
{
    public class PasswordUtil
    {

        public static String convertPasswordToMd5(String pass)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            var byteResult = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pass));

            StringBuilder builder = new StringBuilder();

            //Convert to Hexa String
            foreach (byte single in byteResult)
            {
                builder.Append(single.ToString("x2"));
            }

            return builder.ToString();
        }

    }
}