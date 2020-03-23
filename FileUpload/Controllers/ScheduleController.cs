using BusinessDatabase.BussinessObj;
using BusinessDatabase.DataObj.Request;
using BusinessDatabase.DataObj.Response;
using FileUpload.Utils;
using Newtonsoft.Json;
using System;
using BusinessDatabase.Dto.Base;
using Utilities;
using BusinessDatabase.DataObj;
using System.Collections.Generic;
using System.Web.Http;
using BusinessDatabase.CommonObj;

namespace FileUpload.Api.Controllers
{

    public class ScheduleController : ApiController
    {
        public string tableName = "scheduler";

        /// <summary>
        ///  insert if not exist staff in database 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("api/schedule/listByStaffId")]
        [HttpPost]
        public ResponseBase<ScheduleReponse> GetScheduleListById(RequestBase request)
        {
            var response = new ResponseBase<ScheduleReponse>();

            var data = JsonConvert.DeserializeObject<ScheduleRequestDto>(request.data.ToString());

            data.StaffId = data.StaffId.ToString().Replace(" ", String.Empty).RemoveCharacterUnicode().Trim();

            List<ScheduleDto> listData = new List<ScheduleDto>();

            var helperBo = new HelperBo();
            response.data = new ScheduleReponse();
            response.data.data = new List<dynamic>();

            var result = new ResponseBase<List<dynamic>>();
            var totalCount = new ResponseBase<List<dynamic>>();

            if(String.IsNullOrEmpty(data.StaffId))
            {
                result = helperBo.SelectWhere("*", tableName, null, data.PageIndex + "," + data.PageSize);
                totalCount = helperBo.SelectWhere("count(*) as totalrows", tableName, null, null);
            }
            else
            {
                result = helperBo.SelectWhere("*", tableName, " StaffId like '%" + data.StaffId + "%'", data.PageIndex + "," + data.PageSize);
                totalCount = helperBo.SelectWhere("count(*) as totalrows", tableName, " StaffId like '%" + data.StaffId + "%'", null);
            }


            if (result.success)
            {
                var dataReponse = new ScheduleReponse();
                response.data = dataReponse;
                dataReponse.data = new List<dynamic>();
                foreach (var r in result.data)
                {
                    dataReponse.data.Add(r);
                }

                long countItem = 0;
                if (totalCount != null)
                {
                    countItem = totalCount.data[0].totalrows;
                }
                dataReponse.currentPage = data.PageIndex;
                dataReponse.total = countItem;
                response.message = "Success";
                response.success = true;
                return response;
            }

            response.message = "No found";
            response.success = false;
            return response;
        }

       
    }
}
