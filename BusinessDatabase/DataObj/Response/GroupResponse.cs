using FileUpload.BusinessDatabase.DataObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.BusinessDatabase.DataObj.Response
{
    public class GroupResponse
    {
        public int total{get;set;}
        public int currentPage{get;set;}
        public List<GroupDataResponse> data {get;set;}
    }
    public class GroupDataResponse : GroupDto
    {
    }
}
