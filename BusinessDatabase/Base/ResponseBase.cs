using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDatabase.Dto.Base
{
    public class ResponseBase
    {
        public bool success { get; set; }
        public string message { get; set; }
        public ErrorResponseBase error { get; set; }
    }

    public class ResponseBase<T> : ResponseBase
    {
        public T data { get; set; }
      
    }
}
