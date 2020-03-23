using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDatabase.CommonObj
{
    public class EventViewModel
    {

        public string ID { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public bool allDay { get; set; }
        public string[] className = { };
        public string icon { get; set; }
        public string staffId { get; set; }
        public string customerId { get; set; }
        public string description { get; set; }
        public string meetingLocation { get; set; }
        public string idUpdate { get; set; }

    }
}
