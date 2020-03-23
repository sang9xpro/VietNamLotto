using BusinessDatabase.BussinessObj;
using BusinessDatabase.CommonObj;
using BusinessDatabase.DataObj;
using CMS.Filters;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace CMS.Controllers
{
    //[CustomActionFilter]
    public class LoginController : Controller
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(LoginController));
        //
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult GetId(int id)
        {
            var ubo = new HelperBo();
            var a = ubo.SelectWhere("*","users","ID="+id.ToString(),null);

            return View(a.data);
        }
        [HttpPost]
        public ActionResult Register(string name,string username, string password)
        {
            var result = "failed";
            var user = new HelperBo();
            var res = user.IsExistWhere("users"," Username='"+username.Trim()+"'");

            if (res.success == true && res.message == "0")
            {
                var dto = new UserDto();
                dto.Name = username;
                //dto.Password = Common.Md5Hash(password);
                dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                //dto.CreatedDate = DateTime.Now.ToString();
                dto.Username = username;
                var resregister=  user.Add("users", dto);
                result = "success";
            }
            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = result
            }), "application/json");
        }
        [HandleError]
        [HttpPost]
        public ActionResult LogOn(string username, string password)
        {
            var isTrue = false;
            var passEncrypted=BCrypt.Net.BCrypt.HashPassword(password);
            _log.Info("logon");
            string result = "";
            var user = new HelperBo();
            var userDto = new UserDto();
           var staffHelperBo = new StaffHelperBo();
            userDto.Username = username;
            userDto.Password = password;
            var resultUser = user.SelectWhere(null, "users", "Username=" + username, null);

         
                 isTrue = BCrypt.Net.BCrypt.Verify(userDto.Password, resultUser.data[0].Password);
            
            if (!isTrue)
            {
                return Content(JsonConvert.SerializeObject(new
                {
                    isSuccess = Common.Failed
                }), "application/json");
            }
                var infouser = user.SelectWhere("*","users", " Username='"+ username + "' AND GroupId!=4",null);
            if (infouser.data!=null)
            {
                var item = infouser.data[0];
                    userDto.Name = item.Name.ToString();
                    userDto.Username = item.Username.ToString();
                    userDto.ID = item.ID;
                    userDto.CreatedDate = item.CreatedDate.ToString();
                    userDto.Password = item.Password.ToString();
                userDto.GroupId = item.GroupId.ToString();
                 result = Common.Success;
               
                Common.SetAuthenticationCookie(userDto.ID, username, userDto.Password, true);
                Session["Account"] = userDto;
                // Trường hợp ko nhớ tài khoản
            }
            else
            {

                result = Common.Failed;
            }
            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = result
            }), "application/json");
        }

        public ActionResult LogOut()
        {
            Common.SignOut();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdatePassword(int id, string password)
        {
            var userBo = new HelperBo();
            var UserDto = new UserDto();
          var res=  userBo.SelectWhere("*", "users", " ID=" + id, null);
            if (res.data != null)
            {
                foreach(var i in res.data)
                {
                    UserDto.ID = i.ID;
                    UserDto.Name = i.Name;
                    UserDto.CreatedDate = i.CreatedDate;
                    UserDto.Username = i.Username;
                    UserDto.Password = i.Password;
                }
            }
            if (UserDto.Username!=null)
            {
                UserDto.Password = Common.Md5Hash(password);
            }
            var result = userBo.UpdateWhere("users", UserDto,null);
            if (result.success)
            {

            }
            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = result.message
            }), "application/json");
        }
    }
}