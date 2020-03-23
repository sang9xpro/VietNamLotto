using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDatabase.DataObj
{
    public class FunctionDto
    {
        public int ID { get; set; }
        public int MODULE_ID { get; set; }
        public string MODULE_NAME { get; set; }
        public string NAME { get; set; }
        public bool IS_NEW { get; set; }
        public bool STATUS { get; set; }
        public int MODIFIED_BY { get; set; }
        public string MODIFIED_AT { get; set; }
        public int POSITION { get; set; }
        public string LINKURL { get; set; }

    }
    public class FunctionModel
    {
        public FunctionChildModel data { get; set; }
    }

    public class FunctionChildModel
    {
        public int ID { get; set; }
        public int MODULE_ID { get; set; }
        public string MODULE_NAME { get; set; }
        public string LINKURL { get; set; }
        public string NAME { get; set; }
        public int IS_NEW { get; set; }
        public int STATUS { get; set; }
        public int MODIFIED_BY { get; set; }
        public DateTime MODIFIED_AT { get; set; }
        public int POSITION { get; set; }
    }

}
