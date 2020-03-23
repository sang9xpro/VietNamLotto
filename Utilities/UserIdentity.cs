using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Utilities
{
    public class UserIdentity : IIdentity, IPrincipal
    {
        private readonly FormsAuthenticationTicket _ticket;
        public UserIdentity(FormsAuthenticationTicket ticket)
        {
            _ticket = ticket;
        }
        public string UserId
        {
            get { return _ticket.UserData; }
        }
        public int Id
        {
            get
            {

                var temp = _ticket.UserData.Split(';');
                return Convert.ToInt32(temp[0]);
            }
        }
        public string Name
        {
            get
            {
                var temp = _ticket.UserData.Split(';');
                return temp[1];
            }
        }
        public string AuthenticationType { get { return "Accounts"; } }
        public bool IsAuthenticated { get { return true; } }
        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public IIdentity Identity { get { return this; } }
    }
}
