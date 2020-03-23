using BusinessDatabase.CommonObj;
using BusinessDatabase.DataObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Utilities;

namespace CMS.Controllers
{
    public class BasicController : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
      
          
            base.Initialize(requestContext);
            #region init user session
            var authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null) return;
            if (Session[ModuleConstant.SessionMySessionUserID] != null &&
                Session[ModuleConstant.SessionMySessionUserLoginname] != null &&
                Session[ModuleConstant.SessionMySessionUserFullName] != null) return;
            var encTicket = authCookie.Value;
            if (String.IsNullOrEmpty(encTicket)) return;
            var ticket = FormsAuthentication.Decrypt(encTicket);
            var id = new UserIdentity(ticket);
            var accountBo = new HelperBo();
            var member = accountBo.SelectWhere(null,"users","ID="+id.Id.ToString(),null);
            var UserDto = new UserDto();
            if (member.data != null && member.data.Count>0)
            {
                UserDto.ID= member.data[0].ID;
                string GroupId = member.data[0].GroupId.ToString();
                if (GroupId!="" )
                {
                    UserDto.GroupId = member.data[0].GroupId;
                }

                UserDto.CreatedDate = member.data[0].CreatedDate.ToString()                ;
                UserDto.Name = member.data[0].Name;
                UserDto.Password = member.data[0].Password;
                UserDto.Username = member.data[0].Username;
             
            }
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();    
            if(UserDto!=null && UserDto.Username!=null)
            {
                Session[ModuleConstant.SessionMySessionUserLoginname] = UserDto.Username;
                Session[ModuleConstant.SessionMySessionUserFullName] = UserDto.Username;
                Session[ModuleConstant.SessionMySessionUserID] = UserDto;

            }

            #endregion

        }

    }

}