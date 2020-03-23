using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDatabase.Dto.Base
{
    public class ErrorResponseBase
    {
        public string message { get; set; }
        public string errCode { get; set; }
        public Exception exception { get; set; }
    }
}
