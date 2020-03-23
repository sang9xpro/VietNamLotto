using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDatabase.Object
{
    public class ModuleDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string Icon { get; set; }
       public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public int position { get; set; }

    }
    public class ModuleModel
    {
        public ModuleChildModel data { get; set; }
    }

    public class ModuleChildModel
    {
        public int ID { get; set; }
        public String NAME { get; set; }
        public String DESCRIPTION { get; set; }
        public int STATUS { get; set; }
        public string ICON { get; set; }
        public int MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public int POSITION { get; set; }
    }

}
