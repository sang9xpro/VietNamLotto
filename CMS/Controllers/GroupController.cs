using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CMS.Filters;
using BusinessDatabase.CommonObj;
using FileUpload.BusinessDatabase.DataObj;
using BusinessDatabase.DataObj;
using BusinessDatabase.Dto.Base;
using FileUpload.BusinessDatabase.DataObj.Response;
using System.Linq;
using Utilities;
using log4net.Config;
using log4net;

namespace CMS.Controllers
{
    //[CustomActionFilter]
    public class GroupController : BasicController
    {
        public string tableName = "groupusers";
        public GroupController()
        {
            XmlConfigurator.Configure();
        }
        private readonly ILog _log = LogManager.GetLogger(typeof(GroupController));
        //
        // GET: /Group/
        public ActionResult Index()
        {
            //var GroupBo = new GroupBo();
            //GroupBo.GetDataPaging()
            return View();
        }

        [HttpPost]
        public ActionResult ToggleStatus(int id)
        {

            var helperBo = new HelperBo();
            var res = helperBo.ToggleWhere(tableName, "Status","ID="+id);
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

            var helperBo = new HelperBo();
            var res = helperBo.DeleteWhere(tableName, "ID=" + id);

            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = res
            }), "application/json");
        }


        public ActionResult AjaxGetDataPaging(JQueryDataTableParamModel param)
        {
            var response = new ResponseBase<GroupResponse>();
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(param.ExtendCondition);
            param.sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            param.sortDirection = Request["sSortDir_0"]; 
            var GroupBo = new HelperBo();
            var Group_Name = values["Group_Name"];
            
             var query = " GroupName like '%" + Group_Name + "%' or Description='%"+ Group_Name + "%' ";


            var result = GroupBo.SelectWhere(null, tableName, query, "" + param.iDisplayStart + "," + param.iDisplayLength);
           
            if (result.message == "error")
            {
                _log.Error("ERROR:"+result.error.message,result.error.exception);
                return null;
            }else
            {

                var totalCount = GroupBo.SelectWhere("count(ID) as totalrows", tableName, query, null);
                long countItem = 0;
                if (totalCount != null)
                {
                    countItem = totalCount.data[0].totalrows;
                }
                var list = new List<GroupDataResponse>();
            var groupResponse = new GroupResponse();
            if (result != null)
            {
                foreach(var item in result.data)
                {
                    var uDto = new GroupDataResponse();
                    uDto.ID = item.ID;
                    uDto.GroupName = item.GroupName;
                    uDto.Description = item.Description;
                    uDto.ModifiedBy = item.ModifiedBy;
                    uDto.ModifiedDate = item.ModifiedDate.ToString();
                    uDto.Status = item.Status;
                    list.Add(uDto);
                }
                groupResponse.data = list;
                groupResponse.currentPage = Convert.ToInt32(param.iDisplayStart) / Convert.ToInt32(param.iDisplayLength);
                groupResponse.total = list.Count;

                response.data = groupResponse;
                response.success = true;
                response.message = "success";
            }
                if (param.sSearch != null)
                {
                    list = list.Where(x => x.GroupName.Contains(param.sSearch) || x.Description.Contains(param.sSearch)).ToList();
                    ;
                }
            var str= JsonConvert.SerializeObject(new
            {
                //param.sEcho,
                //iTotalRecords = groupResponse.total,
                //iToTalDisplayRecords = groupResponse.total,
                //aaData = groupResponse.data,
                draw =param.sEcho,
                recordsTotal= countItem,
                recordsFiltered = countItem,
                data = groupResponse.data,
            });
            return Content(str, "application/json");
            }
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(string GROUP_NAME, string DESCRIPTION, int STATUS)
        {
            string res = "Failed";
            var a = (UserDto)Session["Account"];
            var groupBo = new HelperBo();
            var helperBo = new HelperBo();
            var resExist = helperBo.IsExistWhere(tableName,"GroupName='"+GROUP_NAME+"'");
            var statusval = false;
            if (STATUS == 1)
            {
                statusval = true;
            }
            if (resExist.message == "0")
            {
                var groupDto = new GroupDto()
                {
                    GroupName = GROUP_NAME,
                    Description = DESCRIPTION,
                    Status = statusval,
                    ModifiedBy = a.Username.ToString(),
                    ModifiedDate = null
                };
                // not exist
                var resultAdd = groupBo.Add(tableName, groupDto);
                if (resultAdd.success == true)
                {
                    res = "success";
                }
            }

            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = res
            }), "application/json");
        }

        public ActionResult Edit(int id)
        {
            var helperBo = new HelperBo();
            var t = new GroupDto();
            var res1 = helperBo.SelectWhere("*",tableName,"ID="+id,"LIMIT 1");
            if (res1.data != null)
            {
              var  res = res1.data[0];
                    t.ID = res.ID;
                    t.GroupName = res.data.GroupName;
                    t.Description = res.data.Description;
                    t.Status = res.data.Status;
                    t.ModifiedBy = res.data.ModifiedBy;
                    t.ModifiedDate = res.data.ModifiedDate;
                
            }
            return View(t);
        }

        [HttpPost]
        public ActionResult Edit(int ID, string GROUP_NAME, string DESCRIPTION, int STATUS)
        {
            var a = (UserDto)Session["Account"];
            var groupBo = new HelperBo();
            var statusVal = STATUS == 1 ? true : false;
            var groupDto = new GroupDto()
            {
               ID=ID,
               GroupName=GROUP_NAME,
               Description=DESCRIPTION,
               Status= statusVal
            };
            var res = groupBo.UpdateWhere(tableName, groupDto,"ID="+ID);

            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = res
            }), "application/json");
        }


    }
}