using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooriCMS.Business.Helper;
using WooriCMS.Dto.Base;
using WooriCMS.Dto.Request;

namespace WooriCMS.Service
{
    public interface IGroupToFunctionService
    {
        string GetDataPaging(JQueryDataTableParamModel param, int GROUP_ID, string startdate, string enddate);

    
       
    }
}
