using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDatabase.DataObj.Response
{
    public class CustomerResponse
    {
        public int total { get; set; }
        public int currentPage { get; set; }
        public List<CustomerDataResponse> data { get; set; }
    }

   
}
