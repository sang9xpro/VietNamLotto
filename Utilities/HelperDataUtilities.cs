using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utilities
{
    public static class HelperDataUtilities
    {
        /// <summary>
        /// convert dynamic object to dictionary object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary(dynamic obj)
        {
            Dictionary<string, string> myDict = new Dictionary<string, string>();
            Type t = obj.GetType();

            foreach (var pi in t.GetProperties())
            {
                myDict[pi.Name] = pi.GetValue(obj, null)?.ToString();
            }
            return myDict;
        }
        /// <summary>
        ///  convert 1 dictionary to use in Insert query
        ///  return two string : attribute: example: Name,Birthday,CreatedDate
        ///                    : value    : example: 'Hien','2019-06-18','2020-02-19 00:33:30'
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static QueryStr ToInsertString(Dictionary<string, string> obj)
        {
            var dict = new QueryStr();
            var attrString = "";
            var valString = "";
            foreach (var item in obj)
            {
                if (item.Key != "ID" && !String.IsNullOrEmpty(item.Value))
                {
                    attrString += item.Key + ",";
                    var val = "";
                    if (item.Value == "True")
                    {
                        val = "1";
                    }else if (item.Value == "False")
                    {
                        val = "0";
                    }else
                    {
                        val = "'" + item.Value + "'";
                    }
                    valString += val+",";
                }
            }
            valString = Regex.Escape(valString);
            attrString = StringUtils.RemoveLastChacter(attrString);
            valString = StringUtils.RemoveLastChacter(valString);
            dict.AttrStr = attrString;
            dict.ValStr = valString;
            return dict;
        }

        /// <summary>
        ///  convert 1 dictionary to use in Update query
        ///  return two string : attribute: example: Name='Hien',Birthday='2019-06-18',CreatedDate='2020-02-19 00:33:30'
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToUpdateString(Dictionary<string, string> obj)
        {
            var attrString = "";
            foreach (var item in obj)
            {
                if (item.Key != "ID" && !String.IsNullOrEmpty(item.Value))
                {
                    
                    var val = "";
                    if (item.Value == "True")
                    {
                        val = "1";
                    }
                    else if (item.Value == "False")
                    {
                        val = "0";
                    }else if (item.Value == "0")
                    {
                        val = "";

                    }
                    else
                    {
                        val = "'" + item.Value + "'";
                    }
                    if (val != "")
                    {
                        attrString += item.Key + "=" + val + ",";
                    }
                   
                }
            }
            attrString = StringUtils.RemoveLastChacter(attrString);
            return attrString;
        }

       
    }

    public class QueryStr
    {
        public string AttrStr { get; set; }
        public string ValStr { get; set; }
    }
}
