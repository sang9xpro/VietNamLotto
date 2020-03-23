using BusinessDatabase.CommonObj;
using BusinessDatabase.Dto.Base;
using FileUpload.BusinessDatabase.DataObj.Response;


namespace FileUpload.Service
{
    public interface IRoleGroupService
    {
        string GetDataPaging(JQueryDataTableParamModel param, string groupname, string startdate, string enddate);

        ResponseBase<GroupResponse> Get(string name);
       
    }
}
