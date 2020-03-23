using System;
using System.Collections.Generic;
using System.Data;
using BusinessDatabase.CommonObj;
using BusinessDatabase.DataObj;

namespace BusinessDatabase.BussinessObj
{
    public class StaffHelperBo
    {

        /// <summary>
        ///  Add User using UserDto obj
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool Add(UserDto dto)
        {
            var dbConnection = new DBConnection();
            dbConnection.conn.Open();
            var helper = new HelperBo();
            var list = new List<ProcedureParams>();
            list.Add(new ProcedureParams("@name", dto.Name, ParameterDirection.Input));
            list.Add(new ProcedureParams("@staffId", dto.Username, ParameterDirection.Input));
            list.Add(new ProcedureParams("@password", dto.Password, ParameterDirection.Input));
            list.Add(new ProcedureParams("@result", "", ParameterDirection.Output));
            var result = helper.CallStoreProcedure("SignUp", list, dbConnection.conn).ExecuteNonQuery();
            dbConnection.conn.Close();
            return result==1?true:false;
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

            list.Add(new ProcedureParams("@uname", staffId, ParameterDirection.Input));

            var result = helper.CallStoreProcedure("IsExist", list, dbConnection.conn).ExecuteScalar().ToString();
            dbConnection.conn.Close();
           return result=="1"?true:false;

        }

        /// <summary>
        /// Login 
        /// </summary>
        /// <param name="staff"></param>
        /// <returns></returns>
        public UserDto GetUserByName(UserDto staff)
        {
            var dbConnection = new DBConnection();

            dbConnection.conn.Open();

            HelperBo helper = new HelperBo();

            var list = new List<ProcedureParams>();

            list.Add(new ProcedureParams("@uname", staff.Username, ParameterDirection.Input));
            //list.Add(new ProcedureParams("@pword", staff.Password, ParameterDirection.Input));

            var result = helper.CallStoreProcedure("Login", list, dbConnection.conn).ExecuteReader();

            UserDto staffResult = new UserDto();

            if (result.Read())
            {
                staffResult.ID = (Int32)result["ID"];
                staffResult.Name = result["Name"].ToString();
                staffResult.Username = result["Username"].ToString();
                staffResult.Password = result["Password"].ToString();
                staffResult.CreatedDate = result["CreatedDate"].ToString();
            }
            dbConnection.conn.Close();
            return staffResult;

        }
    }

}
