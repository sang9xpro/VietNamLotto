using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDatabase.DataObj
{
    public class UserDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string CreatedDate { get; set; }
        public string GroupId { get; set; }


        public UserDto(string Name, string Username, string Password)
        {
            this.Password = Password;
            this.Name = Name;
            this.Username = Username;
        }

        public UserDto()
        {

        }
    }
}
