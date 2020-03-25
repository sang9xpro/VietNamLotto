using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDatabase.DataObj
{
  public class ContractDto
    {
        public int ID { get; set; }
        public int CUSTOMER_ID { get; set; }
        public int PRODUCT_ID { get; set; }
        public string TYPE { get; set; }
        public string RATIO { get; set; }
        public string DATE { get; set; }
    }
}
