using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace Utilities
{
    public static class StringUtils
    {
        public static string RemoveLastChacter(string str)
        {
            if (str.Length > 0)
            {
                return str.Remove(str.Length - 1);
            }
            else
                return str;
           
        }
        public static string FormatDateTimeVn(DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm:ss");
        }
        public static string GetSex(this int? sex)
        {
            var result = "";
            switch (sex)
            {
                case 0:
                    result = "Nam";
                    break;
                case 1:
                    result = "Nữ";
                    break;
                default:
                    result = "Bê đê";
                    break;
            }
            return result;
        }

        public static string GetPathImg(string url, int? sex)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Format("{0}/{1}", WebConfigurationManager.AppSettings["PathImg"], sex == 0 ? WebConfigurationManager.AppSettings["DefaultImgmale"] : WebConfigurationManager.AppSettings["DefaultImgfemale"]);
            }

            if (!url.StartsWith("http"))
            {
                return string.Format("{0}/{1}", WebConfigurationManager.AppSettings["PathImg"], url);
            }

            return url;
        }

        public static string GetImgDefault()
        {
            return string.Format("{0}/{1}", WebConfigurationManager.AppSettings["PathImg"], WebConfigurationManager.AppSettings["DefaultImgmale"]);
        }
        public static string GetLogo(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Format("{0}/{1}", WebConfigurationManager.AppSettings["PathImg"], WebConfigurationManager.AppSettings["DefaultImgmale"]);
            }

            if (!url.StartsWith("http"))
            {
                return string.Format("{0}/{1}", WebConfigurationManager.AppSettings["PathImg"], url);
            }

            return url;
        }

        public static string GetString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "Chưa có";
            }
            else
            {
                return str;
            }
        }
        public static int GetInt(this int? str)
        {
            if (Convert.ToInt32(str) != 0)
            {
                if (str != null) return (int)str;
            }
            return 0;
        }

        public static string GetStatus(bool? isStatus, int type)
        {
            var result = "";
            if (type == 1)
            {

                if (isStatus == true)
                {
                    result += "<i  class='fa fa-check-circle statusVerify' style='color:green;'></i>";
                }
                else
                {
                    result += "<i class='fa fa-ban' style='color:red;'></i>";
                }
            }
            else
            {
                if (isStatus == false)
                {
                    result += "<i  class='fa fa-check-circle statusVerify' style='color:green;'></i>";
                }
                else
                {
                    result += "<i class='fa fa-ban' style='color:red;'></i>";
                }
            }

            return result;
        }
        public static bool? CheckValue(this string value)
        {
            if (string.IsNullOrEmpty(value) || value == "-1") return null;
            if (value == "0")
            {
                return false;
            }
            return true;
        }

        public static string GetQuoteType(this int quoteType)
        {
            string result;
            switch (quoteType)
            {
                case 1:
                    result = "Tất cả";
                    break;
                case 2:
                    result = "Theo giờ";
                    break;
                case 3:
                    result = "Khoảng giá";
                    break;
                default:
                    result = "Tất cả";
                    break;
            }
            return result;
        }

        public static string FormatMoney(this int money)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            return double.Parse(money.ToString()).ToString("#,###", cul.NumberFormat);
        }

        public static string ConvertImgToBase64(this string url)
        {
            var _sb = new StringBuilder();
            Byte[] _byte = GetImageFromUrl(url);
            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
            return "data:image/png;base64," + _sb.ToString();
        }
        private static byte[] GetImageFromUrl(string url)
        {
            Stream stream = null;
            byte[] buf;

            try
            {
                var myProxy = new WebProxy();
                var req = (HttpWebRequest)WebRequest.Create(url);

                var response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (var br = new BinaryReader(stream))
                {
                    var len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }

                if (stream != null) stream.Close();
                response.Close();
            }
            catch (Exception exp)
            {
               
                buf = null;
            }

            return (buf);
        }
        public static string pad(bool isLeft, string str, int length, char c)
        {
            if (str == null)
            {
                str = "";
            }
            if (str.Length > length)
            {
                return str.Substring(0, length);
            }
            else
            {
                string str1 = "";
                int num = length - str.Length;
                for (int i = 0; i < num; i++)
                {
                    str1 += c;
                }
                if (isLeft)
                {
                    return str1 + str;
                }
                else
                    return str + str1;
            }
        }

        public static bool CheckFolderName(this string fName)
        {
            fName = fName.Replace("\\/", "/").Replace("\\", "/");
            if ((!Directory.Exists(fName)))
            {
                Directory.CreateDirectory(fName);
                return true;
            }
            return false;
        }

        public static string RemoveCharacterUnicode(this string str)
        {

            for (int i = 1; i < VietnameseSigns.Length; i++)
            {

                for (int j = 0; j < VietnameseSigns[i].Length; j++)

                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);

            }

            return str;

        }
        private static readonly string[] VietnameseSigns = new string[]
{

"aAeEoOuUiIdDyY",

"áàạảãâấầậẩẫăắằặẳẵ",

"ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

"éèẹẻẽêếềệểễ",

"ÉÈẸẺẼÊẾỀỆỂỄ",

"óòọỏõôốồộổỗơớờợởỡ",

"ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

"úùụủũưứừựửữ",

"ÚÙỤỦŨƯỨỪỰỬỮ",

"íìịỉĩ",

"ÍÌỊỈĨ",

"đ",

"Đ",

"ýỳỵỷỹ",

"ÝỲỴỶỸ"

};

        public static string StandingPhone(this string number)
        {
            var phone = "";
            if (string.IsNullOrEmpty(number)) return phone;
            phone = number.Replace(" ", "");
            if (phone.StartsWith("84"))
            {
                phone = phone.Remove(0, 2);
            }

            if (phone.StartsWith("0"))
            {
                phone = phone.Remove(0, 1);
            }

            phone = "0" + phone;
            return phone;
        }

        public static string ReplaceSpace(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            const RegexOptions options = RegexOptions.None;
            var regex = new Regex("[ ]{2,}", options);
            return regex.Replace(value, " ");
        }

        public static string Encrypt(string plainText, string password)
        {
            if (plainText == null)
            {
                return null;
            }
            if (password == null)
            {
                password = String.Empty;
            }
            // Get the byte of the string
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            // hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            var bytesEncryped = StringUtils.Encrypt(bytesToBeEncrypted, passwordBytes);
            return Convert.ToBase64String(bytesEncryped);
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }
            return encryptedBytes;
        }

        public static string Decrypt(string encryptedText, string password)
        {
            if (encryptedText == null) {
                return null;

            }
            if (password == null)
            {
                password = String.Empty;
            }
            // get the bytes of the string
            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            var byteDecrypted = StringUtils.Decrypt(bytesToBeDecrypted, passwordBytes);
            return Encoding.UTF8.GetString(byteDecrypted);
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }
            return decryptedBytes;
        }
        /// <summary>
        /// convert object to XML. Note that object need to serializable
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static string ToXmlString(object dto)
        {
            var serializer1 = new XmlSerializer(dto.GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var output = "";
            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter swriter = new StreamWriter(stream, Encoding.UTF8))
            {
                serializer1.Serialize(swriter, dto, ns);
                byte[] arr = stream.ToArray();
                var encoded = Convert.ToBase64String(arr);
                output = Encoding.UTF8.GetString(stream.ToArray());
               
              
            }
            return output;
        }

        public static byte[] ToXmlStream(object dto)
        {
            var serializer1 = new XmlSerializer(dto.GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter swriter = new StreamWriter(stream, Encoding.UTF8))
            {
                serializer1.Serialize(swriter, dto, ns);
                byte[] arr = stream.ToArray();
                return arr;
            }

        }


        public static string splitStringByChar(string stringData, char character, bool isLeft)
        {

            if(stringData == null)
            {
                return String.Empty;
            }

            int charLocation = stringData.IndexOf(character);

            if(charLocation > 0)
            {
                if (isLeft)
                {
                    return stringData.Substring(0, charLocation);
                }else
                {
                    return stringData.Substring(stringData.LastIndexOf(character) + 1);
                }               
            }
            else
            {
                return String.Empty;
            }

        }
    }
}
