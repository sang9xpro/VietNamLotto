using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CMS.Filters;
using BusinessDatabase.CommonObj;
using log4net;
using FileUpload.BusinessDatabase.DataObj;
using BusinessDatabase.Object;
using BusinessDatabase.DataObj;
using Utilities;

namespace CMS.Controllers
{
    [CustomActionFilter]
    public class FunctionController : BasicController
    {
        public string tableName = "modulefunctions";
        private readonly ILog _log = LogManager.GetLogger(typeof(FunctionController));
        //
        // GET: /Group/
        public ActionResult Index()
        {

            //var FunctionBo = new FunctionBo();
            //FunctionBo.GetDataPaging()
            return View();
        }

        [HttpPost]
        public ActionResult ToggleStatus(int id)
        {

            var helperBo = new HelperBo();
            var res = helperBo.ToggleWhere(tableName, "STATUS","ID="+id);
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
        public ActionResult ToggleIsNew(int id)
        {

            var helperBo = new HelperBo();
            var res = helperBo.ToggleWhere(tableName, "IS_NEW", "ID=" + id);
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
            var funcDto = helperBo.SelectWhere(null,tableName,"ID="+id, null);
            var res = helperBo.DeleteWhere(tableName,"ID="+id);
            var t = new FunctionDto();
            if (funcDto != null && funcDto != null)
            {
                t.MODULE_ID = funcDto.data[0].MODULE_ID;
                t.POSITION = funcDto.data[0].POSITION;
            }
            
            if (res.success)
            {
                var functionBo = new HelperBo();
                var resReorder = functionBo.ReorderFunction(t.POSITION.ToString(),t.MODULE_ID.ToString(), tableName);
            }

            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = true
            }), "application/json");
        }


        public ActionResult AjaxGetDataPaging(JQueryDataTableParamModel param)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(param.ExtendCondition);
            param.sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            param.sortDirection = Request["sSortDir_0"];
            var helperBo = new HelperBo();
            var NAME = values["NAME"];
            var startDate = Convert.ToDateTime(values["StartDate"]).ToString("yyyy-MM-dd");
            var endDate = Convert.ToDateTime(values["EndDate"]).ToString("yyyy-MM-dd");
            var query = " m.Id=f.MODULE_ID and (m.NAME like '%" + NAME + "%' or f.Name like'%" + NAME + "%') order by MODULE_ID,POSITION ASC ";
        
            var result = helperBo.SelectWhere("f.*,m.Name as ModuleName", tableName+" f, module m", query, param.iDisplayStart + "," + param.iDisplayLength);
            if (result.message == "error")
            {
                _log.Error("ERROR:" + result.error.message, result.error.exception);
                return null;
            }
            else
            {

                var totalCount = helperBo.SelectWhere("count(f.ID) as totalrows", tableName + " f, module m", " m.Id=f.MODULE_ID and (m.NAME like '%" + NAME + "%' or f.Name like'%" + NAME + "%') ", null);
                long countItem = 0;
                if (totalCount != null)
                {
                    countItem = totalCount.data[0].totalrows;
                }
                if (param.sSearch != null)
                {
                    result.data = result.data.Where(x => x.NAME.Contains(param.sSearch) || x.ModuleName.Contains(param.sSearch)).ToList();
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
            var list = new List<ModuleDto>();
            var helperBo = new HelperBo();
            var res = helperBo.SelectWhere(null,"module",null,null);
            if (res.data != null)
            {
                foreach (var item in res.data)
                {
                    var dto = new ModuleDto();
                    dto.ID = item.Id;
                    dto.Name = item.Name;
                    list.Add(dto);
                }

            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Add(string NAME, int IS_NEW, int STATUS, int POSITION, int MODULE_ID, string LINKURL)
        {
            string res = "Failed";
            var a = (UserDto)Session["Account"];
            var helperBo = new HelperBo();
            var resExist = helperBo.IsExistWhere(tableName, "NAME='"+NAME+"'");
            var statusVal = STATUS == 1 ? true : false;
            var isNewVal = IS_NEW == 1 ? true : false;
            if (resExist.message == "0")
            {
                var dto = new FunctionDto()
                {
                    NAME=NAME,
                    STATUS= statusVal,
                    IS_NEW= isNewVal,
                    POSITION=POSITION,
                    MODULE_ID=MODULE_ID,
                    LINKURL=LINKURL,
                    MODIFIED_BY=Int32.Parse(a.Username),
                    MODIFIED_AT = DateTime.Now.FormatYYYYMMDD()
                };
                // not exist
               var resultAdd = helperBo.Add(tableName,dto);
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
            var list = new List<ModuleDto>();
            var helperBo = new HelperBo();
            var rest = helperBo.SelectWhere(null, "module",null, null);
            if (rest.data != null)
            {
                var tmp = rest.data[0];
                var t1 = new ModuleDto();
               t1.ID = tmp.Id;
               t1.Name = tmp.Name;
               t1.Description = tmp.Description;
               t1.Status = tmp.Status == 1 ? true : false;
               t1.ModifiedBy = tmp.ModifiedBy;
               t1.ModifiedDate = tmp.ModifiedDate.ToString();
               t1.position = tmp.position;
               t1.Icon = tmp.Icon;
                list.Add(t1);
            }
            else
            {
                _log.Error("Error", rest.error.exception);
            }

            ViewBag.listModule = list;
            var t = new FunctionDto();
            var res2 = helperBo.SelectWhere(null,tableName,"ID="+id,null);
            if (res2.data != null)
            {
                var res = res2.data[0];
                t.ID = res.ID;
                t.NAME = res.NAME;
                t.MODULE_ID = res.MODULE_ID;
                t.STATUS = res.STATUS==1?true:false;
                t.MODIFIED_BY = Int32.Parse(res.MODIFIED_BY);
                t.MODIFIED_AT = res.MODIFIED_AT.ToString();
                t.POSITION = res.POSITION;
                t.IS_NEW = res.IS_NEW==1?true:false;
                t.LINKURL = res.LINKURL;
            }
            return View(t);
        }
        [HttpPost]
        public ActionResult MovePosition(int id, string val)
        {
            var helperBo = new HelperBo();
            var res = helperBo.MovePisitionFunction(id, tableName, Int32.Parse(val));
            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = res
            }), "application/json");
        }

        [HttpPost]
        public ActionResult Edit(int ID, string NAME, int MODULE_ID, int STATUS, int IS_NEW, string LINKURL)
        {
            var a = (UserDto)Session["Account"];
            var HelperBo = new HelperBo();
            var statusVal = STATUS == 1 ? true : false;
            var isNewVal = IS_NEW == 1 ? true : false;
            var moduleFunction = new FunctionDto()
            {
                NAME=NAME,
                MODULE_ID=MODULE_ID,
                STATUS=statusVal,
                IS_NEW= isNewVal,
                LINKURL=LINKURL
            };
            var res = HelperBo.UpdateWhere(tableName, moduleFunction, "ID=" + ID);

            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = res.message
            }), "application/json");
        }

        [HttpPost]
        public ActionResult GetMaxPosition()
        {
            var functionBo = new HelperBo();
            var res = functionBo.SelectWhere("MODULE_ID,MAX(POSITION) as MPOSITION",tableName, "1=1 GROUP BY MODULE_ID", null);

            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = res.success,
                data = res.data,
                message = res.message
            }), "application/json");

        }

    }
}