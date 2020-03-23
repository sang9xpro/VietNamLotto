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
    public class FunctionService : IFunctionService
    {
        /// <summary>
        /// Get data from function table with paging
        /// </summary>
        /// <param name="param"></param>
        /// <param name="active"></param>
        /// <param name="locked"></param>
        /// <param name="keyword"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public string GetDataPaging(JQueryDataTableParamModel param, string function_name, string startdate, string enddate)
        {
            var pageIndex = 0;
            if (Convert.ToInt32(param.iDisplayStart) > 0)
            {
                pageIndex = Convert.ToInt32(param.iDisplayStart) / Convert.ToInt32(param.iDisplayLength);
            }
            var request = new FunctionRequest()
            {
                pageIndex = pageIndex,
                pageSize = param.iDisplayLength,
                NAME = function_name,
                STATUS=null
            };


            var result = GetDataPaging(request).data;
            if (result == null || result.data==null )
            {
                result = new FunctionResponse();
                result.total = 0;
                result.data = new List<FunctionDataResponse>();

            }
            if (param.sSearch!=null)
            {
                return JsonConvert.SerializeObject(new
                {
                    param.sEcho,
                    iTotalRecords = result.total,
                    iTotalDisplayRecords = result.total,
                    aaData = result.data.Where(x => x.ID.ToString().IndexOf(param.sSearch) != -1 || x.NAME.ToString().IndexOf(param.sSearch) != -1 || x.MODULE_NAME.ToString().IndexOf(param.sSearch) != -1 || x.MODIFIED_BY.ToString().IndexOf(param.sSearch) != -1 || x.MODIFIED_AT.ToString().IndexOf(param.sSearch) != -1).OrderBy(x => x.MODULE_ID).OrderBy(x => x.POSITION).ToList()
                });
            }

            else return JsonConvert.SerializeObject(new
            {
                param.sEcho,
                iTotalRecords = result.total,
                iTotalDisplayRecords = result.total,
                aaData = result.data.OrderBy(x=>x.MODULE_NAME).OrderBy(x=>x.POSITION).ToList()
            });
          
        }

        public ResponseBase<FunctionResponse> GetDataPaging(FunctionRequest request)
        {
            return HttpClientCore<FunctionResponse>.ExecuteRequest(Common.BaseUrl, "/api/function/getdatapaging", request, Method.POST);
        }
        public ResponseBase<FunctionResponse> Get(string id)
        {
            var functionBo = new FunctionBo();

            return HttpClientCore<FunctionResponse>.ExecuteRequest(Common.BaseUrl, "/api/function/" + id + "", null, Method.GET);
        }

      

    }
}
