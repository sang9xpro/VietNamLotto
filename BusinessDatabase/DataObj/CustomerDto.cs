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
        public string NAME { get; set; }
        public string AGE { get; set; }
        public string ADDRESS { get; set; }
        public string NUMBER_PHONE { get; set; }
        public string TYPE { get; set; }

        private List<ContractDto> list;

        public List<ContractDto> GetList()
        {
            return list;
        }

        public void SetList(List<ContractDto> value)
        {
            list = value;
        }
    }
}
