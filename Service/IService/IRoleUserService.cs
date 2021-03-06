﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooriCMS.Business.Helper;
using WooriCMS.Dto.Base;
using WooriCMS.Dto.Request;

namespace WooriCMS.Service
{
    public interface IRoleUserService
    {
        string GetDataPaging(JQueryDataTableParamModel param, string User_Name, string startdate, string enddate);

        ResponseBase<UserResponse> Get(string customerId);
       
    }
}
