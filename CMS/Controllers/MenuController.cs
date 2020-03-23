using BusinessDatabase.CommonObj;
using BusinessDatabase.DataObj;
using BusinessDatabase.DataObj.Response;
using BusinessDatabase.Dto.Base;
using BusinessDatabase.Object;
using FileUpload.BusinessDatabase.DataObj;
using FileUpload.BusinessDatabase.DataObj.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Utilities;

namespace CMS.Controllers
{
    public class MenuController : BasicController
    {
        //
        // GET: /Menu/
        public ActionResult Index()
        {
            var tmpRequestRawUrl = Request.RawUrl.Split('/');
            var count = 0;
            foreach (var item in tmpRequestRawUrl)
            {
                count++;
            }
            var functionName = "Index";
            if (count > 2)
            {
                functionName = Request.RawUrl.Split('/')[2];
            }
            var controllerName = Request.RawUrl.Split('/')[1];
            var list = new List<dynamic>();
            var listFunctionAllow = new List<FunctionDto>();

            var acc = Session["Account"];  
            if (acc != null)
            {
                #region List allow functions of Current User
                var a = (UserDto)acc;
                ViewBag.USER_GROUP_ID = a.GroupId;
                ViewBag.displaySpecialFunction = false;
                var helperBo = new HelperBo();
               
                var UserName = a.Username.Trim();
                var DateNow = DateTime.Now.ToString("yyyyMMdd");
                var key = Common.Md5Hash(UserName + DateNow);
                ViewBag.Key = key;
                var groupOfUser = a.GroupId;
                var listFunctions = new ResponseBase<List<dynamic>>();
                if (groupOfUser == "")
                {

                }else
                {
                     listFunctions = helperBo.SelectWhere("*", "group2functions", "ID_GROUP=" + groupOfUser+" AND ALLOW_VIEW=1", null);
                    if (listFunctions.success)
                    {
                            list = listFunctions.data;
                    }
                }
               
                #endregion

                #region AllModules
                var listModule = helperBo.SelectWhere("*", "module",null, null);
                var listModuleObject = new List<ModuleDto>();

                foreach (var item in listModule.data)
                {
                    var dto = new ModuleDto();
                    dto.ID = item.Id;
                    dto.Icon = item.Icon;
                    dto.Description = item.Description;
                    dto.Name = item.Name;
                    dto.Status = item.Status==1?true:false;
                    dto.position = item.position;
                    listModuleObject.Add(dto);
                }
                #endregion

                var listChosenModule = new List<ModuleDto>();

                foreach (var item1 in list)
                {
                    var function = helperBo.SelectWhere("*", "modulefunctions", "ID=" + item1.ID_FUNCTION, null);
                   
                    if (function.success && function.data.Count >0)
                    {
                        var functionDto = new FunctionDto();
                        var item = function.data;
                            functionDto.ID = item[0].ID;
                            functionDto.IS_NEW = item[0].IS_NEW==1?true:false;
                            functionDto.LINKURL = item[0].LINKURL;
                            functionDto.MODULE_ID = item[0].MODULE_ID;
                            functionDto.NAME = item[0].NAME;
                            functionDto.POSITION = item[0].POSITION;
                            functionDto.STATUS = item[0].STATUS == 1 ? true : false;
                        foreach (var j in listModuleObject)
                        {
                            if (j.ID == functionDto.MODULE_ID)
                            {
                                functionDto.MODULE_NAME = j.Name;
                                if (!listChosenModule.Exists(x => x.ID == j.ID))
                                {
                                    listChosenModule.Add(j);
                                }

                            }
                        }
                        listFunctionAllow.Add(functionDto);
                    }
                }
                ViewBag.ChosenModule = listChosenModule.OrderBy(x => x.position);
            }
            //
          //  var isAllowAccessPage = false;
            var isAllowAccessPage = true;
            if (listFunctionAllow.Count == 0 && controllerName.Equals("Error", StringComparison.CurrentCultureIgnoreCase))
            {
                isAllowAccessPage = true;
            }
            else
            {
                foreach (var i in listFunctionAllow)
                {
                    var t = i.LINKURL.Split('/');
                    var tcon = t[1];
                    var tact = t[2];
                    if (tcon.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase) || controllerName.Equals("Error", StringComparison.CurrentCultureIgnoreCase) || controllerName.Equals("Home", StringComparison.CurrentCultureIgnoreCase) || controllerName.Equals("GroupToFunction", StringComparison.CurrentCultureIgnoreCase) || (controllerName.Equals("RequestForm", StringComparison.CurrentCultureIgnoreCase) && (functionName.Equals("IndexManager", StringComparison.CurrentCultureIgnoreCase) || functionName.Equals("EditManager", StringComparison.CurrentCultureIgnoreCase))))
                    {
                        isAllowAccessPage = true;
                    }
                }
            }
            ViewBag.ShouldRedirect = "false";
            if (!isAllowAccessPage)
            {
                ViewBag.ShouldRedirect = "true";

            }

            return PartialView("_MainMenu", listFunctionAllow.OrderBy(y => y.POSITION).GroupBy(x => x.MODULE_ID));


        }
        
        public ActionResult Info()
        {
            var acc = Session["Account"];
            if (acc != null)
            {
                var a = (UserDto)acc;
                return PartialView("_Info",a);
            }
            return PartialView("_Info");
        }
        public ActionResult BreadCrumb()
        {
            var t = Request.RawUrl.Split('/');

            ViewBag.controllerName = t[1];

            if (t.Length > 2)
            {
                ViewBag.actionName = t[2];
            }
            else
            {
                ViewBag.actionName = "Index";
            }
            //ViewBag.actionName = Request.RawUrl.Split('/')[2] == null ? "Index" : Request.RawUrl.Split('/')[2];

            return PartialView("_BreadCrumb", null);
        }

        public ActionResult ContentBreadCrumb()
        {
            var t = Request.RawUrl.Split('/');

            ViewBag.controllerName = t[1];

            if (t.Length > 2)
            {
                ViewBag.actionName = t[2];
            }
            else
            {
                ViewBag.actionName = "Index";
            }

            return PartialView("_ContentBreadCrumb", null);
        }
    }
}