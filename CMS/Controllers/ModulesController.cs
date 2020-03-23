using BusinessDatabase.CommonObj;
using BusinessDatabase.DataObj;
using BusinessDatabase.Object;
using CMS.Controllers;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace WooriCMS.Controllers
{
    public class ModulesController : BasicController
    {
        public string tableName = "module";
        private readonly ILog _log = LogManager.GetLogger(typeof(ModulesController));
        //
        // GET: /Group/
        public ActionResult Index()
        {
            //var HelperBo = new HelperBo();
            //HelperBo.GetDataPaging()
            return View();
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

            var helperBo = new HelperBo();
            var moduleList = helperBo.SelectWhere(null, tableName,"ID="+id,null);
            var t = new ModuleDto();
            if (moduleList!=null && moduleList.data != null)
            {
                t.position = moduleList.data[0].position;
            }
          
            var res = helperBo.DeleteWhere(tableName,"ID="+id);
            if (res.success)
            {
                var HelperBo = new HelperBo();
                var resReorder = HelperBo.ReorderModule(t.position.ToString(),tableName);
            }
            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = res
            }), "application/json");
        }
        [HttpPost]
        public ActionResult MovePosition(int id, string val)
        {
            var helperBo = new HelperBo();
            var res = helperBo.MovePosition(id.ToString(),tableName, val);
            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = res
            }), "application/json");
        }
        public ActionResult AjaxGetDataPaging(JQueryDataTableParamModel param)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(param.ExtendCondition);
            param.sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            param.sortDirection = Request["sSortDir_0"];
            var helperBo = new HelperBo();
            var NAME = values["NAME"];
            var query = " Name like '%" + NAME + "%' or Description like '%" + NAME + "%' ";
            var startDate = Convert.ToDateTime(values["StartDate"]).ToString("yyyy-MM-dd");
            var endDate = Convert.ToDateTime(values["EndDate"]).ToString("yyyy-MM-dd");
            var result = helperBo.SelectWhere(null, tableName, query, param.iDisplayStart + "," + param.iDisplayLength);
            if (result.message == "error")
            {
                _log.Error("ERROR:" + result.error.message, result.error.exception);
                return null;
            }
            else
            {

                var totalCount = helperBo.SelectWhere("count(ID) as totalrows", tableName, query, null);
                long countItem = 0;
                if (totalCount != null)
                {
                    countItem = totalCount.data[0].totalrows;
                }
                if (param.sSearch != null)
                {
                    result.data = result.data.Where(x => x.Name.Contains(param.sSearch) || x.Description.Contains(param.sSearch)).ToList();
                    ;
                }
                
                var str = JsonConvert.SerializeObject(new
                {                   
                    draw = param.sEcho,
                    recordsTotal = countItem,
                    recordsFiltered = countItem,
                    data = result.data,
                });
                return Content(str, "application/json");
            }
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(string NAME, string DESCRIPTION, int STATUS, string ICON, int position)
        {
            string res = "Failed";
            var a = (UserDto)Session["Account"];   
            var helperBo = new HelperBo();
            var resExist = helperBo.IsExistWhere(tableName, "NAME='"+NAME+"'");
            var statusVal = STATUS == 1 ? true : false;
            if (resExist.message == "0")
            {
                var maxP = 1;
                var maxPosition = helperBo.SelectWhere("MAX(position) as mposition", tableName, null, null);
                if (maxPosition.success)
                {
                    maxP = maxPosition.data[0].mposition;
                }
                var moduleDto = new ModuleDto()
                {
                    Name = NAME,
                    Description = DESCRIPTION,
                    Status = statusVal,
                    Icon = ICON,
                    position = maxP+1,
                    ModifiedDate = DateTime.Now.FormatYYYYMMDD(),
                    ModifiedBy=a.Username
                };
                // not exist
                var resultAdd = helperBo.Add(tableName, moduleDto);
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
            var t = new ModuleDto();
            var res1 = helperBo.SelectWhere("*", tableName, "ID=" + id, null);

            if (res1.data != null)
            {
                var res = res1.data[0];
                t.ID = res.Id;
                t.Name = res.Name;
                t.Description = res.Description;
                t.Status = res.Status==1?true:false;
                t.ModifiedBy = res.ModifiedBy;
                t.ModifiedDate = res.ModifiedDate.ToString();
                t.position = res.position;
                t.Icon = res.Icon;
            }else
            {
                _log.Error("Error", res1.error.exception);
            }
            return View(t);
        }

        [HttpPost]
        public ActionResult Edit(int ID, string NAME, string DESCRIPTION, int STATUS, string icon, int position)
        {
            var a = (UserDto)Session["Account"];
            var HelperBo = new HelperBo();
            var statusVal = STATUS == 1 ? true : false;
            var module = new ModuleDto()
            {
                ID=ID,
                Name=NAME,
                Description=DESCRIPTION,
                Status=statusVal,
                Icon=icon,
                position=position
            };
            var res = HelperBo.UpdateWhere(tableName,module,"ID="+ID);

            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = res.message
            }), "application/json");
        }


    }
}