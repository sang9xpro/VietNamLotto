using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileUpload.Models
{
    public class StaffResponse
    {
        public string status { get; set; }
        public string count { get; set; }

        public string message { get; set; }

        public StaffResponse(string status, string count, string message)
        {
            this.status = status;
            this.count = count;
            this.message = message;
        }

        public StaffResponse()
        {
            
        }

    }
}