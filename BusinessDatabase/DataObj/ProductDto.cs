using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDatabase.DataObj
{
    public class ProductDto
    {
        public int ID { get; set; }
        public string PRODUCT_NAME { get; set; }
        public double RATIO { get; set; }
        public string TYPE { get; set; }
        public double PRICE { get; set; }
    }
}
