using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDatabase.DataObj
{
  public class DocumentStatusLogDto
    {
        public int ID { get; set; }
        public string CustomerID { get; set; }
        public int OldStatus { get; set; }
        public int NewStatus { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }
}
