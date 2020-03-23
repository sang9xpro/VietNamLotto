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
    public class GroupToFunctionService : IGroupToFunctionService
    {
        /// <summary>
        /// Get data from GroupToFunction table with paging
        /// </summary>
        /// <param name="param"></param>
        /// <param name="active"></param>
        /// <param name="locked"></param>
        /// <param name="keyword"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public string GetDataPaging(JQueryDataTableParamModel param, int idGroup, string startdate, string enddate)
        {
            var pageIndex = 0;
            if (Convert.ToInt32(param.iDisplayStart) > 0)
            {
                pageIndex = Convert.ToInt32(param.iDisplayStart) / Convert.ToInt32(param.iDisplayLength);
            }
            var request = new GroupToFunctionRequest()
            {
                pageIndex = pageIndex,
                pageSize = param.iDisplayLength,
                ID_GROUP = idGroup
            };


            var result = GetDataPaging(request).data;
            if (result == null || result.data==null )
            {
                result = new GroupToFunctionResponse();
                result.total = 0;
                result.data = new List<GroupToFunctionDataResponse>();

            }
            if (param.sSearch!=null)
            {
                return JsonConvert.SerializeObject(new
                {
                    param.sEcho,
                    iTotalRecords = result.total,
                    iTotalDisplayRecords = result.total,
                    aaData = result.data.Where(x => x.ID.ToString().IndexOf(param.sSearch) != -1 || x.GNAME.ToString().IndexOf(param.sSearch) != -1 || x.FNAME.ToString().IndexOf(param.sSearch) != -1 ).OrderBy(x => x.GNAME).ToList()
                });
            }

            else return JsonConvert.SerializeObject(new
            {
                param.sEcho,
                iTotalRecords = result.total,
                iTotalDisplayRecords = result.total,
                aaData = result.data.OrderBy(x => x.GNAME).ToList()
            });
          
        }

        public ResponseBase<GroupToFunctionResponse> GetDataPaging(GroupToFunctionRequest request)
        {
            return HttpClientCore<GroupToFunctionResponse>.ExecuteRequest(Common.BaseUrl, "/api/grouptofunction/getdatapaging", request, Method.POST);
        }
     

      

    }
}
