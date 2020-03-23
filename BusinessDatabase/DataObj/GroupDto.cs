using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.BusinessDatabase.DataObj
{
    public class GroupDto
    {
        public int ID { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }
    public class GroupModel
    {
        public GroupChildModel data { get; set; }
    }

    public class GroupChildModel
    {
        public int ID { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int MODIFIED_BY { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

}
