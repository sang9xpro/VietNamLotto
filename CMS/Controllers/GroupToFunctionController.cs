using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CMS.Filters;
using BusinessDatabase.CommonObj;
using BusinessDatabase.Object;
using BusinessDatabase.DataObj;
using log4net;
using BusinessDatabase.DataObj.Response;
using System.Linq;

namespace CMS.Controllers
{
    [CustomActionFilter]
    public class GroupToFunctionController : BasicController
    {
        public string tableName = "group2functions";
        private readonly ILog _log = LogManager.GetLogger(typeof(FunctionController));
        //
        // GET: /Group/
        public ActionResult ChangeRole(int id)
        {
            ViewBag.groupId = id;
            var listGroupToFunction = new List<GroupToFunctionDataResponse>();
            var helperBo = new HelperBo();
            var listModule = new List<ModuleDto>();
            #region GetListModule
            var moduleResponse = helperBo.SelectWhere(null,"module",null,null);
            if (moduleResponse.data != null)
            {
                foreach (var item in moduleResponse.data)
                {
                    var dto = new ModuleDto();
                    dto.ID = item.Id;
                    dto.Name = item.Name;
                    listModule.Add(dto);
                }

            }         
            #endregion
            #region GetListFunction


            // list function
            var listFunction = new List<FunctionDto>();
            var functionResponse = helperBo.SelectWhere(null, "modulefunctions", null, null);
            if (moduleResponse.data != null)
            {
                foreach (var item in functionResponse.data)
                {
                    var dto = new FunctionDto();
                    dto.ID = item.ID;
                    dto.NAME = item.NAME;
                    dto.IS_NEW = item.IS_NEW==1?true:false;
                    dto.LINKURL = item.LINKURL;
                    dto.MODULE_ID = item.MODULE_ID;
                    foreach (var module in listModule)
                    {
                        if (module.ID == dto.MODULE_ID)
                        {
                            dto.MODULE_NAME = module.Name;
                        }
                    }
                    dto.POSITION = item.POSITION;
                    dto.STATUS = item.STATUS==1?true:false;
                    listFunction.Add(dto);

                }

            }
            
            #endregion
            #region GetListSelectedGroup2Function
            var listSelected = helperBo.SelectWhere(null,tableName,"ID_GROUP="+id,null);

            #endregion
            #region FillAllFunctionToTable
            foreach (var item in listFunction)
            {
                var g2fDto = new GroupToFunctionDataResponse();
                g2fDto.ID = 0;
                g2fDto.ID_GROUP = id;
                g2fDto.ID_FUNCTION = item.ID;
                g2fDto.ALLOW_VIEW = 0;
                g2fDto.ALLOW_ADD = 0;
                g2fDto.ALLOW_EDIT = 0;
                g2fDto.ALLOW_DEL = 0;
                g2fDto.ALLOW_EXPORT = 0;
                g2fDto.FNAME = item.MODULE_NAME + " - " + item.NAME;
                listGroupToFunction.Add(g2fDto);
            }
            #endregion
            #region UpdateSelectedFunctionToAboveTable
            if (listSelected != null)
            {
                foreach (var item in listSelected.data)
                {
                    foreach (var gtf in listGroupToFunction)
                    {
                        if (item.ID_GROUP == gtf.ID_GROUP && item.ID_FUNCTION == gtf.ID_FUNCTION)
                        {
                            gtf.ID = item.ID;
                            gtf.ALLOW_VIEW = item.ALLOW_VIEW;
                            gtf.ALLOW_ADD = item.ALLOW_ADD;
                            gtf.ALLOW_EDIT = item.ALLOW_EDIT;
                            gtf.ALLOW_DEL = item.ALLOW_DEL;
                            gtf.ALLOW_EXPORT = item.ALLOW_EXPORT;
                        }
                    }
                }
            }
            #endregion
            return View(listGroupToFunction);
        }

        [HttpPost]
        public ActionResult ToggleAllowview(int id, int idGroup)
        {
            var helperBo = new HelperBo();
            var res = helperBo.IsExistWhere(tableName, "ID_GROUP=" + idGroup + " AND ID_FUNCTION=" + id);
            var result = "failed";
            if (res.success)
            {
                // not exist => add new
                if (res.message == "0")
                {
                    var g2f = new GroupToFunctionDto()
                    {
                        ID_FUNCTION = id,
                        ID_GROUP = idGroup,
                        ALLOW_ADD = 0,
                        ALLOW_DEL = 0,
                        ALLOW_EDIT = 0,
                        ALLOW_EXPORT = 0,
                        ALLOW_VIEW = 1
                    };
                    helperBo.Add(tableName, g2f);
                }
                //exist => update
                else
                {
                    helperBo.ToggleWhere(tableName, "ALLOW_VIEW", "ID_GROUP=" + idGroup + " AND ID_FUNCTION=" + id);
                }
                result = "success";
            }
            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = result
            }), "application/json");
        }
        [HttpPost]
        public ActionResult ToggleAllowadd(int id, int idGroup)
        {

            var helperBo = new HelperBo();
            var res = helperBo.IsExistWhere(tableName, "ID_GROUP=" + idGroup + " AND ID_FUNCTION=" + id);
            var result = "failed";
            if (res.success)
            {
                // not exist => add new
                if (res.message == "0")
                {
                    var g2f = new GroupToFunctionDto()
                    {
                        ID_FUNCTION = id,
                        ID_GROUP = idGroup,
                        ALLOW_ADD = 1,
                        ALLOW_DEL = 0,
                        ALLOW_EDIT = 0,
                        ALLOW_EXPORT = 0,
                        ALLOW_VIEW = 0
                    };
                    helperBo.Add(tableName, g2f);
                }
                //exist => update
                else
                {
                    helperBo.ToggleWhere(tableName, "ALLOW_ADD", "ID_GROUP=" + idGroup + " AND ID_FUNCTION=" + id);
                }
                result = "success";
            }
            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = result
            }), "application/json");
        }
        [HttpPost]
        public ActionResult ToggleAllowedit(int id, int idGroup)
        {
            var helperBo = new HelperBo();
            var res = helperBo.IsExistWhere(tableName, "ID_GROUP=" + idGroup + " AND ID_FUNCTION=" + id);
            var result = "failed";
            if (res.success)
            {
                // not exist => add new
                if (res.message == "0")
                {
                    var g2f = new GroupToFunctionDto()
                    {
                        ID_FUNCTION = id,
                        ID_GROUP = idGroup,
                        ALLOW_ADD = 0,
                        ALLOW_DEL = 0,
                        ALLOW_EDIT = 1,
                        ALLOW_EXPORT = 0,
                        ALLOW_VIEW = 0
                    };
                    helperBo.Add(tableName, g2f);
                }
                //exist => update
                else
                {
                    helperBo.ToggleWhere(tableName, "ALLOW_EDIT", "ID_GROUP=" + idGroup + " AND ID_FUNCTION=" + id);
                }
                result = "success";
            }
            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = result
            }), "application/json");
        }
        [HttpPost]
        public ActionResult ToggleAllowdel(int id, int idGroup)
        {
            var helperBo = new HelperBo();
            var res = helperBo.IsExistWhere(tableName, "ID_GROUP=" + idGroup + " AND ID_FUNCTION=" + id);
            var result = "failed";
            if (res.success)
            {
                // not exist => add new
                if (res.message == "0")
                {
                    var g2f = new GroupToFunctionDto()
                    {
                        ID_FUNCTION = id,
                        ID_GROUP = idGroup,
                        ALLOW_ADD = 0,
                        ALLOW_DEL = 1,
                        ALLOW_EDIT = 0,
                        ALLOW_EXPORT = 0,
                        ALLOW_VIEW = 0
                    };
                    helperBo.Add(tableName, g2f);
                }
                //exist => update
                else
                {
                    helperBo.ToggleWhere(tableName, "ALLOW_DEL", "ID_GROUP=" + idGroup + " AND ID_FUNCTION=" + id);
                }
                result = "success";
            }
            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = result
            }), "application/json");
        
        }

        [HttpPost]
        public ActionResult ToggleAllowexport(int id, int idGroup)
        {
        var helperBo = new HelperBo();
        var res = helperBo.IsExistWhere(tableName, "ID_GROUP=" + idGroup + " AND ID_FUNCTION=" + id);
        var result = "failed";
        if (res.success)
        {
            // not exist => add new
            if (res.message == "0")
            {
                var g2f = new GroupToFunctionDto()
                {
                    ID_FUNCTION = id,
                    ID_GROUP = idGroup,
                    ALLOW_ADD = 0,
                    ALLOW_DEL = 0,
                    ALLOW_EDIT = 0,
                    ALLOW_EXPORT = 1,
                    ALLOW_VIEW = 0
                };
                helperBo.Add(tableName, g2f);
            }
            //exist => update
            else
            {
                helperBo.ToggleWhere(tableName, "ALLOW_EXPORT", "ID_GROUP=" + idGroup + " AND ID_FUNCTION=" + id);
            }
            result = "success";
        }
        return Content(JsonConvert.SerializeObject(new
        {
            isSuccess = result
        }), "application/json");
    }
        [HttpPost]
        public ActionResult SetAllowVal(int id, int val)
        {

            var groupToFunctionBo = new HelperBo();
            var obj = new GroupToFunctionDto()
            {
                ALLOW_ADD=val,
                ALLOW_DEL=val,
                ALLOW_EDIT=val,
                ALLOW_EXPORT=val,
                ALLOW_VIEW=val
            };
            var res = groupToFunctionBo.UpdateWhere(tableName, val,"ID="+id);
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

        public ActionResult AjaxGetDataPaging(JQueryDataTableParamModel param)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(param.ExtendCondition);
            param.sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            param.sortDirection = Request["sSortDir_0"];
            var ID_GROUP = Int32.Parse(values["ID_GROUP"]);
            var startDate = Convert.ToDateTime(values["StartDate"]).ToString("yyyy-MM-dd");
            var endDate = Convert.ToDateTime(values["EndDate"]).ToString("yyyy-MM-dd");
            var helperBo = new HelperBo();
           
            var query = " (ID_GROUP="+ ID_GROUP + ") and gf.ID_GROUP=g.ID and gf.ID_FUNCTION=f.ID order by gf.ID";

            var result = helperBo.SelectWhere("g.GROUP_NAME as GNAME,gf.ID,gf.*,f.NAME as FNAME", tableName + " gf,groupusers g,modulefunctions f", query, null);
            if (result.message == "error")
            {
                _log.Error("ERROR:" + result.error.message, result.error.exception);
                return null;
            }
            else
            {

                var totalCount = helperBo.SelectWhere("count(*) as totalrows", tableName + " gf,groupusers g,modulefunctions f", query, null);
                long countItem = 0;
                if (totalCount != null)
                {
                    countItem = totalCount.data[0].totalrows;
                }
                if (param.sSearch != null)
                {
                    result.data = result.data.Where(x => x.GNAME.Contains(param.sSearch) || x.FNAME.Contains(param.sSearch)).ToList();
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
            var res = helperBo.SelectWhere(null, "module", null, null);
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
        /// <summary>
        ///  if ID = 0 > add more, if ID> 0 then update 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ID_GROUP"></param>
        /// <param name="ID_FUNCTION"></param>
        /// <param name="ALLOW_VIEW"></param>
        /// <param name="ALLOW_ADD"></param>
        /// <param name="ALLOW_EDIT"></param>
        /// <param name="ALLOW_DEL"></param>
        /// <param name="ALLOW_EXPORT"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(int ID, int ID_GROUP, int ID_FUNCTION, int ALLOW_VIEW, int ALLOW_ADD, int ALLOW_EDIT, int ALLOW_DEL, int ALLOW_EXPORT)
        {
            string res = "Failed";
            var groupToFunctionBo = new HelperBo();


            if (ID == 0)
            {
                var obj = new GroupToFunctionDto()
                {
                    ALLOW_VIEW = ALLOW_VIEW,
                    ALLOW_EXPORT = ALLOW_EXPORT,
                    ALLOW_EDIT = ALLOW_EDIT,
                    ALLOW_DEL = ALLOW_DEL,
                    ALLOW_ADD = ALLOW_ADD,
                    ID_FUNCTION = ID_FUNCTION,
                    ID_GROUP = ID_GROUP
                };
                // not exist
            
                var resultAdd = groupToFunctionBo.Add(tableName, obj);
                if (resultAdd.success == true)
                {
                    res = "success";
                }
            }
            else
            {
                var obj = new GroupToFunctionDto()
                {
                    ALLOW_VIEW = ALLOW_VIEW,
                    ALLOW_EXPORT = ALLOW_EXPORT,
                    ALLOW_EDIT = ALLOW_EDIT,
                    ALLOW_DEL = ALLOW_DEL,
                    ALLOW_ADD = ALLOW_ADD,
                    ID_FUNCTION = ID_FUNCTION,
                    ID_GROUP = ID_GROUP
                };
              var  result = groupToFunctionBo.UpdateWhere(tableName, obj,"ID="+ID);
                if (result.success)
                {
                    res = result.message;
                }else
                {
                    _log.Error("ERROR", result.error.exception);
                }
            }

            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = res
            }), "application/json");
        }



    }
}