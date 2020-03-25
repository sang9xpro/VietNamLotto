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
   
    public class SchedulerController : BasicController
    {
        public string tableName = "scheduler";
        private readonly ILog _log = LogManager.GetLogger(typeof(SchedulerController));

        //public JsonResult GetEvents(DateTime start,DateTime end)
        //{
        //    var viewModel = new EventViewModel();
        //    var events = new List<EventViewModel>();
        //}
        //
        // GET: /User/

        public JsonResult GetEvents(DateTime start, DateTime end,int id)
        {
            var userBo = new HelperBo();
            var listEvents = new List<EventViewModel>();
            var listSchedule = userBo.SelectWhere("a.*", tableName+" a, customers b", " a.CustomerId=b.IdNumber and b.ID="+id, null);
            if (listSchedule.success)
            {
                ViewBag.listSchedule = listSchedule.data;
               
                foreach (var item in listSchedule.data)
                {
                    var style = item.Color.ToString().Split(' ');
                   

                    var events = new EventViewModel();
                    var listClassName = new List<string>();
                    foreach (var k in style)
                    {
                        if (k != "")
                        {
                            listClassName.Add(k);
                        }
                    }
                    events.className = listClassName.ToArray();
                    events.allDay = item.MeetingStartDate == item.MeetingEndDate ? true : false;
                    events.title = item.Title;
                    events.start = item.MeetingStartDate.ToString();
                    events.end = item.MeetingEndDate.ToString();
                    events.ID = item.ID.ToString();
                    events.idUpdate = item.ID.ToString();
                    events.icon = item.Icon.ToString()==""?"":item.Icon;
                    events.staffId = item.StaffId.ToString() == "" ? "" : item.StaffId.ToString();
                    events.customerId = item.CustomerId.ToString() == "" ? "" : item.CustomerId.ToString();
                    events.description = item.Description.ToString() == "" ? "" : item.Description.ToString();
                    events.meetingLocation = item.MeetingLocation.ToString() == "" ? "" : item.MeetingLocation.ToString();
                    listEvents.Add(events);
                }
                
            }
            return Json(listEvents.ToArray(), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Index(int id)
        {
            // id customer ( các lịch hẹn của customer đó) 
           
            var userBo = new HelperBo();

            var acc = Session["Account"];
            UserDto curUvser = new UserDto();
            if (acc != null)
            {
                var a = (UserDto)acc;
                curUvser = a;
            }else
            {
                //TODO: Show error Page
            }
            dynamic customer = null;
            var customerInfo = userBo.SelectWhere(null, "customers", "ID=" + id, null);
            if (customerInfo.success)
            {
                customer = customerInfo.data[0];
            }
            var listStaff = userBo.SelectWhere(null, "users", "GroupId=4", null);
           
            if (listStaff.success)
            {
                ViewBag.listStaff = listStaff.data;
            }
            return View(customer);
        }

        public ActionResult AjaxGetDataPagingScheduler(JQueryDataTableParamModel param)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(param.ExtendCondition);
            var listInputParams = new List<String>();
            var Name = values["Name"] != "" ? " b.Name like '%" + values["Name"] + "%' " : "";
            var IdNumber = values["IdNumber"] != "" ? " b.IdNumber like '%" + values["IdNumber"] + "%' " : "";
            var StaffId = values["StaffId"] != "" ? " a.StaffId like '%" + values["StaffId"] + "%' " : "";
            var PidNumber = values["CIF"] != "" ? " b.CIF like '%" + values["CIF"] + "%' " : "";
            var ModifiedBy = values["ModifiedBy"] != "" ? " a.ModifyBy like '%" + values["ModifiedBy"] + "%' " : "";
            var DocumentStatus = values["DocumentStatus"] != "" ? " b.DocumentStatus = '" + values["DocumentStatus"] + "' " : "";
            var startDate = Convert.ToDateTime(values["StartDate"]).ToString("yyyy-MM-dd");
            var endDate = Convert.ToDateTime(values["EndDate"]).ToString("yyyy-MM-dd");
            if (Name != "")
            {
                listInputParams.Add(Name);
            }
            if (IdNumber != "")
            {
                listInputParams.Add(IdNumber);
            }
            if (StaffId != "")
            {
                listInputParams.Add(StaffId);
            }
            if (PidNumber != "")
            {
                listInputParams.Add(PidNumber);
            }
            if (ModifiedBy != "")
            {
                listInputParams.Add(ModifiedBy);
            }
            if (DocumentStatus != "")
            {
                listInputParams.Add(DocumentStatus);
            }

            var queryStr = "";
            foreach (var item in listInputParams)
            {
                queryStr += item + " AND ";
            }

            queryStr += " 1 ";
            queryStr += " AND a.MeetingEndDate >= '" + startDate + " 00:00:00' and a.MeetingEndDate <='" + endDate + " 23:59:59'  AND b.DocumentStatus=c.ID and a.CustomerId=b.ID ORDER BY a.MeetingEndDate DESC";

            param.sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            param.sortDirection = Request["sSortDir_0"];

            var helperBo = new HelperBo();
            var result = helperBo.SelectWhere("a.ID,a.CustomerID,a.StaffId,a.MeetingLocation,a.Description,a.Title,b.ID as CID,a.MeetingEndDate, a.MeetingStartDate,a.ModifyDate,a.ModifyBy,b.Address,b.CIF,b.DocumentStatus,b.DocumentStatusDate,b.IdNumber,b.Name,b.PhoneNumber,b.UrlPath,c.Description as 'Desc',c.Style,c.IsOk,c.ID as 'DID' ", tableName + "  a , customers b, documentstatusdetail c ", queryStr , param.iDisplayStart + "," + param.iDisplayLength);


            if (result.message == "error")
            {
                _log.Error("ERROR:" + result.error.message, result.error.exception);
                return null;
            }
            else
            {

                var totalCount = helperBo.SelectWhere("count(*) as totalrows", tableName + "  a , customers b, documentstatusdetail c ", queryStr, null);
                long countItem = 0;
                if (totalCount != null)
                {
                    countItem = totalCount.data[0].totalrows;
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
        public ActionResult IndexScheduler()
        {
            var helperBo = new HelperBo();
            var listStatus = helperBo.SelectWhere(null, "documentstatusdetail", null, null);
            if (listStatus.success)
            {
                ViewBag.listStatus = listStatus.data;
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
        public ActionResult UpdateSchedule(dynamic list)
        {
            var errorResponse = "";
            var curUser = new UserDto();
            var acc = Session["Account"];
            if (acc != null)
            {
                var a = (UserDto)acc;
                curUser = a;
            }
            HelperBo helperBo = new HelperBo();
            var customerId = "";
            var hasScheduler = false;
            var objJson = JsonConvert.DeserializeObject(list);
            if (objJson.events != null)
            {


                foreach (dynamic item in objJson.events)
                {
                    var color = "";
                    foreach (var j in item.className)
                    {
                        color += j + " ";
                    }
                    var schedulerDto = new SchedulerDto();

                    schedulerDto.ID = item.ID;
                   schedulerDto.CustomerId = item.customerId;
                   schedulerDto.Description = item.description;
                   schedulerDto.StaffId = item.staffId;

                    schedulerDto.MeetingEndDate = item.end.ToString()!=""? DateUtils.FormatYYYYMMDD(item.end.ToString()): DateUtils.FormatYYYYMMDD(item.start.ToString());
                   schedulerDto.MeetingLocation = item.meetingLocation;
                   schedulerDto.ModifyDate = DateUtils.FormatYYYYMMDD(DateTime.Now);
                   schedulerDto.ModifyBy = curUser.Username.ToString();
                   schedulerDto.MeetingStartDate = DateUtils.FormatYYYYMMDD(item.start.ToString());
                   schedulerDto.Color = color;
                   schedulerDto.Icon = item.icon;
                    schedulerDto.Title = item.title;
                    
                    customerId = item.customerId;
                    // if ID=0 then insert
                    if (schedulerDto.ID.ToString() == "0")
                    {
                        // check staff's schedule for make sure not same date and time
                        var res = helperBo.IsExistWhere(tableName, " StaffId='" + schedulerDto.StaffId + "' AND ((MeetingStartDate between '" + schedulerDto.MeetingStartDate + "' AND '" + schedulerDto.MeetingEndDate + "') OR (MeetingEndDate between '" + schedulerDto.MeetingStartDate + "' AND '" + schedulerDto.MeetingEndDate + "') )");
                        if (res.success)
                        {
                            // not exist => add
                            if (res.message == "0")
                            {
                                var result = helperBo.Add(tableName, schedulerDto);
                                if (result.success)
                                {
                                    errorResponse+= "<div class='alert alert-success fade in'><button class='close' data-dismiss='alert'>×</button><i class='fa-fw fa fa-check'></i><strong>Success</strong> in adding " + schedulerDto.StaffId+" meet this customer in this date: "+schedulerDto.MeetingStartDate+" to "+schedulerDto.MeetingEndDate+ ".</div>";
                                    hasScheduler = true;
                                }
                            }
                            else
                            {
                                errorResponse+= "<div class='alert alert-danger fade in'><button class='close' data-dismiss='alert'>×</button><i class='fa-fw fa fa-times'></i><strong>Error!</strong>This staff " + schedulerDto.StaffId+ " has duplicate meeting time: " + schedulerDto.MeetingStartDate + " to " + schedulerDto.MeetingEndDate + ". Please check! </div>";
                            }
                        }

                    }
                    // else update
                    else
                    {
                      var updateRes=  helperBo.UpdateWhere(tableName, schedulerDto, "ID=" + schedulerDto.ID);
                        if (updateRes.success)
                        {
                            errorResponse += "<div class='alert alert-success fade in'><button class='close' data-dismiss='alert'>×</button><i class='fa-fw fa fa-check'></i><strong>Success</strong>This staff " + schedulerDto.StaffId + " has been updated successfully. meeting time: " + schedulerDto.MeetingStartDate + " to " + schedulerDto.MeetingEndDate + ". </div>";
                            hasScheduler = true;
                        }
                    }
                   
                   
                }
                #region Check if customer has scheduler or not

                if (hasScheduler)
                {
                    var listSchedule = helperBo.SelectWhere(null, tableName, "CustomerId=" + customerId, null);
                    if (listSchedule.success)
                    {
                        if (listSchedule.data.Count > 0)
                        {
                            var objCustomer = helperBo.SelectWhere(null, "customers", "IdNumber=" + customerId, null);
                            if (objCustomer.success)
                            {
                                var customer = objCustomer.data[0];
                                if (customer.DocumentStatus != null || customer.DocumentStatus == 1)
                                {
                                    customer.DocumentStatus = 6;  /// chuyển trạng thái từ 1 > 6 (Đã thiết lập lịch cho khách và agent, nhưng chưa gặp)
                                }
                                var customerDto = new CustomerDto()
                                {
                                    DocumentStatus = customer.DocumentStatus.ToString()
                                };

                                helperBo.UpdateWhere("customers", customerDto, "ID=" + customer.ID);
                                var obj = new ContractDto()
                                {
                                    CustomerID = customer.ID.ToString(),
                                    OldStatus = 1,
                                    NewStatus = 6,
                                    ModifiedBy = curUser.Username.ToString(),
                                    ModifiedDate = DateUtils.FormatYYYYMMDD(DateTime.Now)
                                };
                                helperBo.Add("documentstatuslog", obj);
                            }
                        }
                    }

                }
                #endregion
                return Content(JsonConvert.SerializeObject(new
                {
                    data = errorResponse
                }), "application/json");
            }
            else
            {
                return Content(JsonConvert.SerializeObject(new
                {
                    data = "You did not set any schedule for this customer"
                }), "application/json");
            }
         
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