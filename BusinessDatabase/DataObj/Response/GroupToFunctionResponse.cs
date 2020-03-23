using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDatabase.DataObj.Response
{
    public class GroupToFunctionResponse
    {
        public int total{get;set;}
        public int currentPage{get;set;}
        public List<GroupToFunctionDataResponse> data {get;set;}
    }
    public class GroupToFunctionDataResponse: GroupToFunctionDto
    {
  
        public string GNAME { get; set; }
        public string FNAME { get; set; }
    }
}
