using BusinessDatabase.CommonObj;
using BusinessDatabase.DataObj;
using CMS.Controllers;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Utilities;

namespace CMS.Controllers
{
   
    public class CustomerController : BasicController
    {
        public string tableName = "customers";
        private readonly ILog _log = LogManager.GetLogger(typeof(CustomerController));

        //
        // GET: /User/
        public ActionResult Index()
        {
            var helperBo = new HelperBo();
           var listStatus= helperBo.SelectWhere(null, "documentstatusdetail", null, null);
            if (listStatus.success)
            {
                ViewBag.listStatus = listStatus.data;
            }
            return View();
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
            var listInputParams = new List<String>();
            var Name = values["Name"]!=""? " Name like '%" + values["Name"] + "%' " : "";
            var IdNumber = values["IdNumber"] != "" ? " IdNumber like '%" + values["IdNumber"] + "%' " : "";
            var StaffId = values["StaffId"] != "" ? " StaffId like '%" + values["StaffId"] + "%' " : "";
            var PidNumber = values["CIF"] != "" ? " CIF like '%" + values["CIF"] + "%' " : "";
            var ModifiedBy = values["ModifiedBy"] != "" ? " ModifiedBy like '%" + values["ModifiedBy"] + "%' " : "";
            var DocumentStatus = values["DocumentStatus"] != "" ? " DocumentStatus = '" + values["DocumentStatus"] + "' " : "";
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
            foreach(var item in listInputParams)
            {
                queryStr += item + " AND ";
            }
          
            queryStr += " 1 ";
            queryStr += " AND ModifiedDate >= '"+startDate+" 00:00:00' and ModifiedDate <='" + endDate + " 23:59:59'";
            
            param.sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            param.sortDirection = Request["sSortDir_0"];         
           
            var helperBo = new HelperBo();
            var result = helperBo.SelectWhere("a.*,b.Description,b.IsOk,b.Style", tableName + " a left join DocumentStatusDetail b on a.DocumentStatus = b.ID", queryStr + " ORDER BY a.ModifiedDate DESC ", param.iDisplayStart + "," + param.iDisplayLength);


            if (result.message == "error")
            {
                _log.Error("ERROR:" + result.error.message, result.error.exception);
                return null;
            }
            else
            {

                var totalCount = helperBo.SelectWhere("count(*) as totalrows", tableName , queryStr, null);
                long countItem = 0;
                if (totalCount != null)
                {
                    countItem = totalCount.data[0].totalrows;
                }
                if (param.sSearch != null)
                {
                    result.data = result.data.Where(x => x.Name.Contains(param.sSearch) || x.IdNumber.Contains(param.sSearch) || x.StaffId.ToString().Contains(param.sSearch) || x.CIF.ToString().Contains(param.sSearch) ).ToList();
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
            var helperBo = new HelperBo();
            var acc = Session["Account"];
            var curUser = new UserDto();
            if (acc != null)
            {
                curUser = (UserDto)acc;
            }
            ViewBag.StaffId = curUser.Username;
            var listStaff= helperBo.SelectWhere(null, "users",null, null);
            dynamic list =null;
            if (listStaff.success)
            {
                list = listStaff.data;
            }
            return View(list);
        }


        public ActionResult Edit(int id)
        {
            var helperBo = new HelperBo();
            var listStaff = helperBo.SelectWhere(null, "users", null, null);
            dynamic list = null;
            if (listStaff.success)
            {
                list = listStaff.data;
            }
            ViewBag.list = list;
            var res = helperBo.SelectWhere(null,tableName,"ID="+id,null);
            var userDto = res.data[0];
            return View(userDto);
        }

        [HttpPost]
        public ActionResult ChangeDocumentStatus(int ID, string DocumentStatus,string[] Recruiter)
        {
            if (Recruiter==null)
            {
                return Content(JsonConvert.SerializeObject(new
                {
                    isSuccess = "Please choose at least 1 recruiter for this customer!"
                }), "application/json");
            }
            var userBo = new HelperBo();
            var obj = new CustomerDto();
            var list=userBo.SelectWhere(null, tableName, "ID=" + ID, null);
            if (list.success)
            {
                var item = list.data[0];
                if (item.DocumentStatus.ToString() == "2"|| item.DocumentStatus.ToString() == "3")
                {
                    obj.DocumentStatus = DocumentStatus;
                }
                obj.DocumentStatusDate = DateUtils.FormatYYYYMMDD(DateTime.Now);    
            }
           
            var strMessage = "";
            if (DocumentStatus == "5")
            {
                foreach (var rec in Recruiter)
                {
                    var recruiterDto = new RecruiterDto();
                    recruiterDto.IDCustomer = ID.ToString();
                    recruiterDto.IDAgent = rec;
                    recruiterDto.ModifiedDate = DateUtils.FormatYYYYMMDD(DateTime.Now);
                    var res = userBo.Add("customerrecruiter", recruiterDto);
                    if (res.message == "success")
                    {
                        strMessage += "User ID: " + rec + " has been added as this customer's recruiter.<br>";
                    }
                }
            }
            var res1 = userBo.UpdateWhere(tableName, obj, "ID=" + ID);
            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = res1.message
            }), "application/json");
        }
        public ActionResult ImageView(int id)
        {
            var helper = new HelperBo();
            var res = helper.SelectWhere(null, tableName, "ID=" + id, null);
            var listStaff = helper.SelectWhere(null, "users", null, null);
            if (listStaff.success)
            {
                ViewBag.listStaff = listStaff.data;
            }
            var list = new List<string>();
            if (res.success)
            {
                var userDto = new CustomerDto();
                 var temp=res.data[0];
                userDto.CIF = temp.CIF!=null?temp.CIF.ToString():"";
                userDto.ID = temp.ID;
                userDto.IdNumber = temp.IdNumber;
                userDto.ModifiedBy = temp.ModifiedBy!=null?temp.ModifiedBy.ToString():"";
                userDto.ModifiedDate = temp.ModifiedDate != null ? temp.ModifiedDate.ToString() : "";
                userDto.Name = temp.Name;
                userDto.StaffId = temp.StaffId.ToString();
                userDto.DocumentStatus = temp.DocumentStatus.ToString()==""?"1": temp.DocumentStatus.ToString();
                userDto.DocumentStatusDate = DateUtils.FormatYYYYMMDD(temp.DocumentStatusDate.ToString());
                if (userDto.DocumentStatus == "5")
                {
                    var recruiterRes=helper.SelectWhere("cr.*,s.Name", "customerrecruiter cr,users s ", "cr.IDAgent=s.Username and IDCustomer=" + id, null);
                    if (recruiterRes.success)
                    {
                        var strRe="";
                        foreach(var item in recruiterRes.data)
                        {
                            strRe +=""+ item.Name + " - ID: " + item.IDAgent + ", ";
                        }
                       
                            ViewBag.strRe = StringUtils.RemoveLastChacter(strRe);
                        
                    }
                }
                var url=(ConfigurationManager.AppSettings["ImgServer"] + temp.UrlPath.ToString());
                var request = (HttpWebRequest)WebRequest.Create(url);
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string html = reader.ReadToEnd().Replace("<A HREF","\n<A HREF");
                            var regex = new Regex("<A HREF=\".*\">(?<name>.*)</A>");
                            MatchCollection matches = regex.Matches(html);
                            if (matches.Count > 0)
                            {
                                foreach (Match match in matches)
                                {
                                    if (match.Success && match.Value.IndexOf("To Parent Directory")==-1 )
                                    {
                                        
                                        list.Add(url + "/" + match.Groups["name"].ToString());
                                    }
                                }
                            }
                            
                        }

                    }
                }
                catch(Exception ex)
                {
                    ViewBag.list = list;
                    _log.Error("Error", ex);
                    return View(userDto);
                }

                ViewBag.list = list;

                return View(userDto);
            }
            else
            {
                _log.Error("ERROR", res.error.exception);
                ViewBag.list = list;
            }
                return View(res.data[0]);
        }
        [HttpPost]
        public ActionResult Add(string Name, string UrlPath, string CIF, string StaffID,string PhoneNumber, string Address,string IdNumber)
        {
            var acc = Session["Account"];
            var curUser = new UserDto();
            if (acc != null)
            {
                curUser = (UserDto)acc;
            }
            var res = "";
            var obj = new CustomerDto()
            {
                Name = Name,
                UrlPath = UrlPath,
                CIF = CIF,
                StaffId = StaffID,
                ModifiedBy = curUser.Username,
                ModifiedDate = DateUtils.FormatYYYYMMDD(DateTime.Now),
                PhoneNumber=PhoneNumber,
                Address=Address,
                IdNumber=IdNumber
            };
            var helperBo = new HelperBo();
            var resExist = helperBo.IsExistWhere(tableName, "IdNumber='" + IdNumber.Trim() + "' || CIF='"+ CIF.Trim()+ "'");
            if (resExist.message == "0")
            {
                var resultAdd = helperBo.Add(tableName, obj);
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
        [HttpPost]
        public ActionResult Edit(string ID,string Name, string UrlPath, string CIF, string StaffID, string PhoneNumber, string Address, string IdNumber)
        {
            var res = "";
            var obj = new CustomerDto()
            {
                Name=Name,
                IdNumber=IdNumber,
                CIF=CIF,
                StaffId=StaffID,
                PhoneNumber=PhoneNumber,
                Address=Address
                
            };
            // check if IDNumber exist while ID!=ID
            var helperBo = new HelperBo();
            var checkExist = helperBo.IsExistWhere(tableName, "ID!=" + ID + " AND IdNumber='" + IdNumber.Trim() + "' AND CIF='" + CIF.Trim() + "' ");
            if (checkExist.success)
            {
                if (checkExist.message == "0")
                {
                var resultUpdate= helperBo.UpdateWhere(tableName, obj, "ID=" + ID);
                    res = resultUpdate.message;
                }
            }else
            {
                _log.Error("Error", checkExist.error.exception);
            }

            return Content(JsonConvert.SerializeObject(new
            {
                isSuccess = res
            }), "application/json");
        }
    }
}