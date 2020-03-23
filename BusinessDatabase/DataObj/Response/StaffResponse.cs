using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDatabase.DataObj.Response
{
    public class StaffResponse
    {
        public int total { get; set; }
        public int currentPage { get; set; }
        public List<StaffDataResponse> data { get; set; }
    }

    public class StaffDataResponse : UserDto
    {
        public StaffDataResponse()
        {

        }


        public StaffDataResponse(int ID, string Name, string Username, string Password, string CreatedDate)
        {
            this.ID = ID;
            this.Password = Password;
            this.Name = Name;
            this.Username = Username;
            this.CreatedDate = CreatedDate;
        }

    }
}
