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
    public class RoleGroupService : IRoleGroupService
    {
        /// <summary>
        /// Get data from group table with paging
        /// </summary>
        /// <param name="param"></param>
        /// <param name="active"></param>
        /// <param name="locked"></param>
        /// <param name="keyword"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public string GetDataPaging(JQueryDataTableParamModel param, string group_name, string startdate, string enddate)
        {
            var pageIndex = 0;
            if (Convert.ToInt32(param.iDisplayStart) > 0)
            {
                pageIndex = Convert.ToInt32(param.iDisplayStart) / Convert.ToInt32(param.iDisplayLength);
            }
            var request = new GroupRequest()
            {
                pageIndex = pageIndex,
                pageSize = param.iDisplayLength,
                startDate = startdate,
                endDate = enddate,
                GROUP_NAME = group_name,
                STATUS=null
            };


            var result = GetDataPaging(request).data;
            if (result == null || result.data==null )
            {
                result = new GroupResponse();
                result.total = 0;
                result.data = new List<GroupDataResponse>();

            }
            if (param.sSearch!=null)
            {
                return JsonConvert.SerializeObject(new
                {
                    param.sEcho,
                    iTotalRecords = result.total,
                    iTotalDisplayRecords = result.total,
                    aaData = result.data.Where(x => x.ID.ToString().IndexOf(param.sSearch) != -1 || x.GROUP_NAME.ToString().IndexOf(param.sSearch) != -1 || x.DESCRIPTION.ToString().IndexOf(param.sSearch) != -1 || x.MODIFIED_BY.ToString().IndexOf(param.sSearch) != -1 || x.MODIFIED_DATE.ToString().IndexOf(param.sSearch) != -1).ToList()
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

        public ResponseBase<GroupResponse> GetDataPaging(GroupRequest request)
        {
            return HttpClientCore<GroupResponse>.ExecuteRequest(Common.BaseUrl, "/api/group/getdatapaging", request, Method.POST);
        }
        public ResponseBase<GroupResponse> Get(string id)
        {
            var groupBo = new GroupBo();

            return HttpClientCore<GroupResponse>.ExecuteRequest(Common.BaseUrl, "/api/group/" + id + "", null, Method.GET);
        }

      

    }
}
