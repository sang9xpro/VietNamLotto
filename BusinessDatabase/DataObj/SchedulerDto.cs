using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDatabase.DataObj
{
    public class SchedulerDto
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string CustomerId { get; set; }
        public string StaffId { get; set; }
        public string MeetingDate { get; set; }
        public string TimeEnd { get; set; }
        public string MeetingLocation { get; set; }
        public string Status { get; set; }
        public string ModifyDate { get; set; }
        public string DocumentStatus { get; set; }
        public string ModifyBy { get; set; }
        public string TimeStart { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public string KYC { get; set; }
        public string Desciption { get; set; }
        public string MeetingStartDate { get; set; }
        public string MeetingEndDate { get; set; }
    }
}
