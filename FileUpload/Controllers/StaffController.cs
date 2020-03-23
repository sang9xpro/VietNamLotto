using BusinessDatabase.BussinessObj;
using BusinessDatabase.DataObj.Request;
using BusinessDatabase.DataObj.Response;
using FileUpload.Utils;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using BusinessDatabase.Dto.Base;
using Utilities;
namespace FileUpload.Api.Controllers
{

    public class StaffController : ApiController
    {

        /// <summary>
        ///  insert if not exist staff in database 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("api/staff/signup")]
        [HttpPost]
        public ResponseBase<StaffResponse> Add(RequestBase request)
        {
            var response = new ResponseBase<StaffResponse>();

            var data = JsonConvert.DeserializeObject<StaffRequestDto>(request.data.ToString());
            /// check exist staff
            var staffHelperBo = new StaffHelperBo();
            var isExist = staffHelperBo.IsExist(data.Username);
            // if exist staff => return error message

            if (isExist)
            {
                response.message = "Staff already existed";
                response.success = false;
                return response;
            }
            else
            {
                //else if not exist => add new staff => return result
                //data.Password = Common.Md5Hash(data.Password);
                data.Password = BCrypt.Net.BCrypt.HashPassword(data.Password);
                try
                {
                    var res = staffHelperBo.Add(data);
                    if (res)
                    {
                        response.message = "Insert new staff successfully.";
                        response.success = true;
                    }
                    else
                    {
                        response.message = "Can't insert new staff.";
                        response.success = false;
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteError(this.ToString(), "Stack Trace: " + ex.StackTrace);
                    LogUtil.WriteError(this.ToString(), "Message: " + ex.Message.ToString());
                    ErrorResponseBase errorResponse = new ErrorResponseBase();
                    errorResponse.errCode = ex.GetHashCode().ToString(); ;
                    errorResponse.message = ex.Message.ToString();
                    response.success = false;
                    response.message = "error";
                    response.error = errorResponse;
                }
            }
            return response;
        }

        /// <summary>
        ///  insert if not exist staff in database 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("api/staff/login")]
        [HttpPost]
        public ResponseBase<StaffResponse> Login(RequestBase request)
        {
            var response = new ResponseBase<StaffResponse>();

            var data = JsonConvert.DeserializeObject<StaffRequestDto>(request.data.ToString());

            /// validate data request 
            /// 
            if(data == null || data.Username == null || data.Password == null)
            {
                response.message = "User or Password is Empty";
                response.success = false;
                return response;
            }

            //data.Password = Common.Md5Hash(data.Password);
            //data.Password = BCrypt.Net.BCrypt.HashPassword(data.Password);
            var staffHelperBo = new StaffHelperBo();
            try
            {
                var login = staffHelperBo.GetUserByName(data);

                if(login.Username != null)
                {
                    var isTrue = BCrypt.Net.BCrypt.Verify(data.Password, login.Password);

                    if (isTrue)
                    {
                        response.message = "Login Success";
                        response.success = true;
                        var dataReponse = new StaffResponse();
                        dataReponse.data = new System.Collections.Generic.List<StaffDataResponse>();
                        dataReponse.data.Add(new StaffDataResponse(login.ID, login.Name, login.Username, login.Password, login.CreatedDate));
                        response.data = dataReponse;
                        return response;
                    }
                    else
                    {
                        response.message = "Wrong User or Password";
                        response.success = false;
                        return response;
                    }
                   
                }
                else
                {
                    response.message = "User or Password does not exist";
                    response.success = false;
                    return response;
                }

            }
            catch (Exception ex)
            {
                LogUtil.WriteError(this.ToString(), "Stack Trace: " + ex.StackTrace);
                LogUtil.WriteError(this.ToString(), "Message: " + ex.Message.ToString());
                ErrorResponseBase errorResponse = new ErrorResponseBase();
                errorResponse.errCode = ex.GetHashCode().ToString(); ;
                errorResponse.message = ex.Message.ToString();
                response.success = false;
                response.message = "error";
                response.error = errorResponse;
            }
            //
            return response;
        }
    }
}
