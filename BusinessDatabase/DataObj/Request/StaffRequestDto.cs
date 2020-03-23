using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDatabase.DataObj.Request
{
    public class StaffRequestDto : UserDto
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
