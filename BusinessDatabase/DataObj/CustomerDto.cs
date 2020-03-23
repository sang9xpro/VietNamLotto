using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDatabase.DataObj
{
    public class CustomerDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string IdNumber { get; set; }
        public string CIF { get; set; }
        public string StaffId { get; set; }
        public string UrlPath { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string DocumentStatus { get; set; }
        public string DocumentStatusDate { get; set; }
    }
}
