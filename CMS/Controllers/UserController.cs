using BusinessDatabase.CommonObj;
using BusinessDatabase.DataObj;
using CMS.Controllers;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Utilities;

namespace CMS.Controllers
{
   
    public class UserController : BasicController
    {
        public string tableName = "users";
        private readonly ILog _log = LogManager.GetLogger(typeof(UserController));

        //
        // GET: /User/
        public ActionResult Index()
        {
            var userBo = new HelperBo();
        
            var listGroup = userBo.SelectWhere(null, "groupusers", null, null);
            if (listGroup.success)
            {
                ViewBag.listGroup = listGroup.data;
            }
            return View();
        }
        [HttpPost]
        public ActionResult GetListGroup()
        {
            HelperBo helperBo = new HelperBo();
            var listGroup = helperBo.SelectWhere(null, "groupusers", null, null);
            if (listGroup.success)
            {
                return Content(JsonConvert.SerializeObject(new
                {
                    data = listGroup.data
                }), "application/json");
            }
            return Content(JsonConvert.SerializeObject(new
            {
                data = String.Empty
            }), "application/json");
        }

        [HttpPost]
        public ActionResult ChangeGroup(string group_id, int id)
        {

            HelperBo helperBo = new HelperBo();
            var UserDto = new UserDto();
            UserDto.GroupId = group_id;
            var resp = helperBo.UpdateWhere(tableName, UserDto, "ID="+id);
            return Content(JsonConvert.SerializeObject(new
            {
                data = resp.message
            }), "application/json");
        }

        [HttpPost]
        public ActionResult ToggleStatus(int id)
        {

            var helperBo = new HelperBo();
            var res = helperBo.ToggleWhere(tableName,"STATUS","ID="+id);
            var result = "failed";
            if (res.success)
            {
                result = "success";
            }
            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = result
            }), "application/json");
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {

            var userBo = new HelperBo();
            var res = userBo.DeleteWhere(tableName,"ID="+id);

            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = res.message
            }), "application/json");
        }


        public ActionResult AjaxGetDataPaging(JQueryDataTableParamModel param)
        {
            
           
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(param.ExtendCondition);
            var User_Name = values["User_Name"];
            param.sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            param.sortDirection = Request["sSortDir_0"];         
            var startDate = Convert.ToDateTime(values["StartDate"]).ToString("yyyy-MM-dd");
            var endDate = Convert.ToDateTime(values["EndDate"]).ToString("yyyy-MM-dd");
            var helperBo = new HelperBo();

            var query = "  (Username like '%" + User_Name + "%' or Name like '%" + User_Name + "%')";

            var result = helperBo.SelectWhere("s.*,g.GroupName", tableName + " s left join groupusers g on s.GroupId=g.ID ", query, param.iDisplayStart + "," + param.iDisplayLength);
            if (result.message == "error")
            {
                _log.Error("ERROR:" + result.error.message, result.error.exception);
                return null;
            }
            else
            {

                var totalCount = helperBo.SelectWhere("count(*) as totalrows", tableName + " s left join groupusers g on s.GroupId=g.ID ", query, null);
                long countItem = 0;
                if (totalCount != null)
                {
                    countItem = totalCount.data[0].totalrows;
                }
                if (param.sSearch != null)
                {
                    result.data = result.data.Where(x => x.Name.Contains(param.sSearch) || x.Username.Contains(param.sSearch)).ToList();
                }

                var str = JsonConvert.SerializeObject(new
                {
                    draw = param.sEcho,
                    recordsTotal = countItem,
                    recordsFiltered = countItem,
                    data = result.data
                  
                });
                return Content(str, "application/json");
            }
        }

        public ActionResult Add()
        {
            return View();
        }


        public ActionResult Edit(int id)
        {
            var helper = new HelperBo();
            var res = helper.SelectWhere(null,tableName,"ID="+id,null);
            var userDto = res.data[0];
            return View(userDto);
        }

        public ActionResult ChangePassword()
        {
            var curUser = new UserDto();
            var acc = Session["Account"];
            if (acc != null)
            {
                var a = (UserDto)acc;
                curUser = a;
            }
            return View(curUser);
        }
        [HttpPost]
        public ActionResult ChangePass(string oldPass, string newPass)
        {
            oldPass = Common.Md5Hash(oldPass);
            newPass = Common.Md5Hash(newPass);
            var res = "failed";
            var curUser = new UserDto();
            var acc = Session["Account"];
            if (acc != null)
            {
                var a = (UserDto)acc;
                curUser = a;
                if (oldPass == curUser.Password)
                {
                    var userBo = new HelperBo();
                    curUser.Password = newPass;
                    var result = userBo.UpdateWhere(tableName, curUser, "ID="+curUser.ID);
                    if (result.success)
                    {
                        res = result.message;
                    }
                }
            }

            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = res
            }), "application/json");
        }

        [HttpPost]
        public ActionResult Edit(string ID, string USER_NAME, string USER_PASS, string CONFIRM_USER_PASS, string EMP_ID)
        {
            var resetPass = "";
            // User input new password & confirm password => reset pass
            if (USER_PASS.Equals(CONFIRM_USER_PASS, StringComparison.InvariantCulture) && USER_PASS.Length > 0)
            {
                resetPass = USER_PASS;
            }
            if (resetPass != "")
            {
                resetPass = Common.Md5Hash(resetPass);
            }
            var res = "failed";
            var curUser = new UserDto();
            var acc = Session["Account"];
            if (acc != null)
            {
                var a = (UserDto) acc;
                curUser = a;
                // reset Password
                if (resetPass != "")
                {
                    var userBo = new HelperBo();
                    a.Password = resetPass;
                    a.CreatedDate = DateUtils.FormatYYYYMMDD(DateTime.Now);
                    var result = userBo.UpdateWhere(tableName, a, "ID=" + ID);
                   
                    if (result.success)
                    {
                        res = result.message;
                    }
                }
                else
                {
                    var helper = new HelperBo();
                    // find duplicate username or emp_id in user table, if yes => not allow to update
                    var isExist = helper.IsExistWhere(tableName, "Username='" + USER_NAME + "'  AND ID !=" + ID + " ");

                    if (isExist.message == "1")
                    {
                        res = "Failed";
                    }
                    else
                    {
                        var userBo = new HelperBo();
                        var UserDto = new UserDto()
                        {
                            ID = Int32.Parse(ID),
                            Username = USER_NAME.Trim(),

                        };
                        userBo.UpdateWhere(tableName,UserDto,"ID="+UserDto.ID);
                        res = "Success";
                    }
                }
            }

            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = res
            }), "application/json");
        }
    }
}