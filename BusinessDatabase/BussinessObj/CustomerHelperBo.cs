using System;
using System.Collections.Generic;
using System.Data;
using BusinessDatabase.CommonObj;
using BusinessDatabase.DataObj;

namespace BusinessDatabase.BussinessObj
{
    public class CustomerHelperBo
    {
        /// <summary>
        ///  Add User using UserDto obj
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool Add(CustomerDto dto)
        {
            var dbConnection = new DBConnection();
            dbConnection.conn.Open();
            var helper = new HelperBo();
            var list = new List<ProcedureParams>();
            list.Add(new ProcedureParams("@name", dto.Name, ParameterDirection.Input));
            list.Add(new ProcedureParams("@idnumber", dto.IdNumber, ParameterDirection.Input));
            list.Add(new ProcedureParams("@staffid", dto.StaffId, ParameterDirection.Input));
            list.Add(new ProcedureParams("@CIF", dto.CIF, ParameterDirection.Input));
            list.Add(new ProcedureParams("@urlpath", dto.UrlPath, ParameterDirection.Input));
            var result = helper.CallStoreProcedure("AddCustomer", list, dbConnection.conn).ExecuteNonQuery();
            dbConnection.conn.Close();
            return result == 1 ? true : false;
        }
        /// <summary>
        /// Inquiry staff via staffId
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public bool IsExist(String staffId)
        {
            var dbConnection = new DBConnection();

            dbConnection.conn.Open();

            HelperBo helper = new HelperBo();

            var list = new List<ProcedureParams>();

            list.Add(new ProcedureParams("@idnum", staffId, ParameterDirection.Input));

            var result = helper.CallStoreProcedure("IsCustomerExist", list, dbConnection.conn).ExecuteScalar().ToString();
            dbConnection.conn.Close();
            return result == "1" ? true : false;

        }

    }

}
