using BusinessDatabase.BussinessObj;
using BusinessDatabase.DataObj.Request;
using BusinessDatabase.DataObj.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Utilities;
using BusinessDatabase.Dto.Base;
using System.IO;
using BusinessDatabase.CommonObj;
using FileUpload.Utils;
using BusinessDatabase.DataObj;

namespace FileUpload.Api.Controllers
{

    public class CustomerController : ApiController
    {
        /// <summary>
        ///  insert if not exist Customer in database 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("api/customer/signup")]
        [HttpPost]
        public async Task<ResponseBase<CustomerResponse>> Add()
        {
            var response = new ResponseBase<CustomerResponse>();
            if (!Request.Content.IsMimeMultipartContent())
            {
                response.success = false;
                response.message = "Unsupported Media Type";
                return response;
            }
            var request = "";
            var listFileNew = new List<FileAndMimeType>();

            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();
            foreach (var item in filesReadToProvider.Contents)
            {
                if (item.Headers.ContentDisposition.Name == "\"jsonKey\"")
                {
                    request = await item.ReadAsStringAsync();
                }
                else
                {
                    var obj = new FileAndMimeType();

                    var fileBytes = await item.ReadAsByteArrayAsync();
                    obj.file = fileBytes;
                    obj.fileName = item.Headers.ContentDisposition.FileName;
                    listFileNew.Add(obj);

                }
            }


            // staff : who logged in into system, will upload customer information for registering for one customer
            var dataRequest = JsonConvert.DeserializeObject<RequestBase>(request);
            var dataRequest1 = dataRequest.data;
            var data = JsonConvert.DeserializeObject<CustomerDto>(dataRequest1.ToString());

            var helper = new HelperBo();
            var isExist = helper.IsExistWhere("customers", " Idnumber = " + data.IdNumber);

            if (data.IdNumber == null)
            {
                response.message = "Customer Id can not be emty";
                response.success = false;
                return response;
            }

            if (data.CIF == null)
            {
                response.message = "Customer Persnonal Identify can not be emty";
                response.success = false;
                return response;
            }
            // if exist Customer => Still Save file to local, return success and message "Customer already existed";
            if (isExist.message == "1")
            {
                data.UrlPath = FileSaveHelper.SaveFileToLocal(listFileNew, data);

                if (data.UrlPath == null)
                {
                    // don't have file
                    response.message = "File not found.";
                    response.success = false;
                    return response;
                }
                else
                {

                    response.message = "Customer already existed, only Insert the file";
                    response.success = true;
                    return response;
                }
            }
            else
            {
                //else if not exist => get the file from request, upload to folder in server add new Customer => return result
                var folderCustomer = data.Name.ToString().Replace(" ", String.Empty).RemoveCharacterUnicode().Trim() + data.IdNumber.ToString().Trim();

                var folderStaff = data.StaffId.ToString().Trim();

                try
                {
                    data.UrlPath = FileSaveHelper.SaveFileToLocal(listFileNew, data);

                    if(data.UrlPath == null)
                    {
                        // don't have file
                        response.message = "File not found.";
                        response.success = false;
                        return response;
                    }

                    //var res = CustomerHelperBo.Add(data);
                    var res = helper.Add("customers" , data);
                    if (res.success)
                    {
                        response.message = "Insert new customer successfully.";
                        response.success = true;
                    }
                    else
                    {
                        response.message = "Can't insert new customer.";
                        response.success = false;
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteError(this.ToString(), "Stack Trace: " + ex.StackTrace);
                    LogUtil.WriteError(this.ToString(), "Message: " + ex.Message.ToString());
                    ErrorResponseBase errorResponse = new ErrorResponseBase();
                    errorResponse.errCode = ex.StackTrace.ToString();
                    errorResponse.message = ex.Message.ToString();
                    response.success = false;
                    response.message = "error";
                    response.error = errorResponse;
                }
            }
            return response;
        }



    }
}
