using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDatabase.DataObj
{
    public class GroupToFunctionDto
    {
        public int ID { get; set; }
        public int ID_GROUP { get; set; }
        public int ID_FUNCTION { get; set; }
        public int ALLOW_VIEW { get; set; }
        public int ALLOW_ADD { get; set; }
        public int ALLOW_EDIT { get; set; }
        public int ALLOW_DEL { get; set; }
        public int ALLOW_EXPORT { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }
}
