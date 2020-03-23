using BusinessDatabase.BussinessObj;
using BusinessDatabase.CommonObj;
using BusinessDatabase.DataObj;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestOnly
{
    class Program
    {
     public   static void Main(string[] args)
        {
            var customerBo = new CustomerHelperBo();

            //var result = customerBo.InsertUser("Long", "9", "99900084", "/Updloaded/long99900084");


            //string pattern = Regex.Escape("[") +"(.*?)]";
            //string input = "The animal [what kind?] was visible [by whom?] from the window.";
            //MatchCollection matches = Regex.Matches(input, pattern);
            //int commentNumber = 0;
            //Console.WriteLine("{0} produces the following matches:", pattern);
            //foreach(Match match in matches)
            //{
            //    Console.WriteLine(" {0}:{1}", ++commentNumber, match.Value);
            //}

            var DeSample = "De 01 04 06  07 08 12x50, đầu 1  x500, dit 1 x 120, Bo 37 x 250, To to x 120, beto x 200, 05 06 x1500, 131 151 393 x15, bo 35x 20, dau 0-1-2-3 x 120";
            string[] arrBang = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "90", "91", "92", "93", "94", "95", "96", "97", "98", "99" };

            var listItems = DeSample.Split(',');
            foreach(var item in listItems)
            {
                if (item != "")
                {
                    var temp = item.Split('x');
                    var price = temp[1].Trim();
                    var giatri = temp[0].Trim();
                }
           
                
                // gia tri can xu ly để chuẩn hóa 

             
            }
            //var helperBo = new HelperBo();
            //var UserDto = new UserDto();
            //UserDto.Name = "a";
            //UserDto.Password = "b";
            //UserDto.Username = "c";
            //UserDto.CreatedDate = "2020-02-18 00:00:00";
            //var res=helperBo.Add("users", UserDto);

            //helperBo.UpdateWhere("users", UserDto, "ID=1");
        }
    }
}
