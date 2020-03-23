using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooriCMS.Business.BusinessObject;
using WooriCMS.Business.Helper;
using WooriCMS.BusinessDatabase.Object;
using WooriCMS.Dto.Base;
using WooriCMS.Dto.Request;
using WooriCMS.Utilities;

namespace WooriCMS.Service
{
    public class RoleUserService : IRoleUserService
    {
        /// <summary>
        /// Get data from user table with paging
        /// </summary>
        /// <param name="param"></param>
        /// <param name="active"></param>
        /// <param name="locked"></param>
        /// <param name="keyword"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public string GetDataPaging(JQueryDataTableParamModel param, string user_name, string startdate, string enddate)
        {
            var pageIndex = 0;
            if (Convert.ToInt32(param.iDisplayStart) > 0)
            {
                pageIndex = Convert.ToInt32(param.iDisplayStart) / Convert.ToInt32(param.iDisplayLength);
            }
            var request = new UserRequest()
            {
                pageIndex = pageIndex,
                pageSize = param.iDisplayLength,
                startDate = startdate,
                endDate = enddate,
                user_name = user_name
            };


            var result = GetDataPaging(request).data;
            if (result == null || result.data == null)
            {
                result = new UserResponse();
                result.total = 0;
                result.data = new List<UserDataResponse>();

            }
            if (param.sSearch!=null)
            {
                var tmpList = result.data.Where(x => x.branch.IndexOf(param.sSearch) != -1 || x.created_by.ToString().IndexOf(param.sSearch) != -1 || x.id.ToString().IndexOf(param.sSearch) != -1 || x.emp_id.ToString().IndexOf(param.sSearch) != -1 || x.user_name.IndexOf(param.sSearch) != -1).ToList();
                return JsonConvert.SerializeObject(new
                {
                    param.sEcho,
                    iTotalRecords = tmpList.Count,
                    iTotalDisplayRecords = tmpList.Count,
                    aaData = tmpList
                });
            }
            else return JsonConvert.SerializeObject(new
            {
                param.sEcho,
                iTotalRecords = result.total,
                iTotalDisplayRecords = result.total,
                aaData = result.data.ToList()
            });
          
        }

        public ResponseBase<UserResponse> GetDataPaging(UserRequest request)
        {
            return HttpClientCore<UserResponse>.ExecuteRequest(Common.BaseUrl, "/api/roluser/getdatapaging", request, Method.POST);
        }
        public ResponseBase<UserResponse> Get(string id)
        {
            var userBo = new UserBo();

            return HttpClientCore<UserResponse>.ExecuteRequest(Common.BaseUrl, "/api/roluser/" + id + "", null, Method.GET);
        }

      

    }
}
