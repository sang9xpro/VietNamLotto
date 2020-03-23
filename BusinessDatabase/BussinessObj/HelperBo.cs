using BusinessDatabase.Dto.Base;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using Utilities;

namespace BusinessDatabase.CommonObj
{
    public class HelperBo
    {

        public MySqlCommand CallStoreProcedure(string procedureName, List<ProcedureParams> listParam, MySqlConnection conn)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;

            foreach(var item in listParam)
            {
                cmd.Parameters.AddWithValue(item.ParamName, item.ParamVal);
                cmd.Parameters[item.ParamName].Direction = item.Direction;
            }
            return cmd;
        }
        /// <summary>
        ///  insert Object 'obj' to table 'tablename'
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ResponseBase Add(string tablename, dynamic obj)
        {
            var responseBase = new ResponseBase();
            // myDict is the object that we convert from dynamic "obj"
            var objDict = HelperDataUtilities.ToDictionary(obj);
            QueryStr query = HelperDataUtilities.ToInsertString(objDict);
            var dbConnection = new DBConnection();
            dbConnection.conn.Open();
            var list = new List<ProcedureParams>();
            list.Add(new ProcedureParams("@p_table_name", tablename, ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_attr", query.AttrStr, ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_val", query.ValStr, ParameterDirection.Input));
            try
            {
                var result = this.CallStoreProcedure("HelperAdd", list, dbConnection.conn).ExecuteNonQuery();
                responseBase.success = true;
                responseBase.message = "success";
            }
            catch(Exception ex)
             {
                responseBase.success = false;
                responseBase.message = "error";
                responseBase.error = new ErrorResponseBase()
                {
                    message = ex.Message,
                    errCode = ex.StackTrace.ToString()
                };
            }
            finally
            {
                dbConnection.conn.Close();
            }
            return responseBase;
        }
        /// <summary>
        /// Delete from 1 table
        /// </summary>
        /// <returns></returns>
        public ResponseBase DeleteWhere(string tablename, string whereclause)
        {
            var responseBase = new ResponseBase();
            var dbConnection = new DBConnection();
            dbConnection.conn.Open();
            var list = new List<ProcedureParams>();
            list.Add(new ProcedureParams("@p_table_name", tablename, ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_where_clause", whereclause, ParameterDirection.Input));
            try
            {
                var result = this.CallStoreProcedure("HelperDeleteWhere", list, dbConnection.conn).ExecuteNonQuery();
                responseBase.success = true;
                responseBase.message = "success";
            }
            catch (Exception ex)
            {
                responseBase.success = false;
                responseBase.message = "error";
                responseBase.error = new ErrorResponseBase()
                {
                    message = ex.Message,
                    errCode = ex.StackTrace.ToString()
                };
            }
            finally
            {
                dbConnection.conn.Close();
            }
            return responseBase;
        }
        /// <summary>
        /// check if exist in table name
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="whereclause"> example: username='Hien' </param>
        /// <returns></returns>
        public ResponseBase IsExistWhere(string tablename,string whereclause)
        {
            var responseBase = new ResponseBase();
            var dbConnection = new DBConnection();
            dbConnection.conn.Open();
            var list = new List<ProcedureParams>();
            list.Add(new ProcedureParams("@p_table_name", tablename, ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_where_clause", whereclause, ParameterDirection.Input));
            try
            {
                var result = this.CallStoreProcedure("HelperIsExistWhere", list, dbConnection.conn).ExecuteScalar();
                responseBase.success = true;
                responseBase.message = result.ToString();
            }
            catch (Exception ex)
            {
                responseBase.success = false;
                responseBase.message = "error";
                responseBase.error = new ErrorResponseBase()
                {
                    message = ex.Message,
                    errCode = ex.StackTrace.ToString()
                };
            }
            finally
            {
                dbConnection.conn.Close();
            }
            return responseBase;
        }
        /// <summary>
        /// Move position of module ( menu )
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tablename"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public ResponseBase MovePosition(string id, string tablename, string val)
        {
            var responseBase = new ResponseBase();
            var dbConnection = new DBConnection();
            dbConnection.conn.Open();
            var list = new List<ProcedureParams>();
            list.Add(new ProcedureParams("@p_id", id, ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_table_name", tablename, ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_val", val, ParameterDirection.Input));
            try
            {
                var result = this.CallStoreProcedure("HelperMovePosition", list, dbConnection.conn).ExecuteNonQuery();
                responseBase.success = true;
                responseBase.message = "success";
            }
            catch (Exception ex)
            {
                responseBase.success = false;
                responseBase.message = "error";
                responseBase.error = new ErrorResponseBase()
                {
                    message = ex.Message,
                    errCode = ex.StackTrace.ToString()
                };
            }
            finally
            {
                dbConnection.conn.Close();
            }
            return responseBase;
        }
        /// <summary>
        /// move position of function (sub-module)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tablename"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public ResponseBase MovePisitionFunction(int id, string tablename, int val)
        {
            var responseBase = new ResponseBase();
            var dbConnection = new DBConnection();
            dbConnection.conn.Open();
            var list = new List<ProcedureParams>();
            list.Add(new ProcedureParams("@p_id", id.ToString(), ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_table_name", tablename, ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_val", val.ToString(), ParameterDirection.Input));
            try
            {
                var result = this.CallStoreProcedure("HelperMovePositionFunction", list, dbConnection.conn).ExecuteNonQuery();
                responseBase.success = true;
                responseBase.message = "success";
            }
            catch (Exception ex)
            {
                responseBase.success = false;
                responseBase.message = "error";
                responseBase.error = new ErrorResponseBase()
                {
                    message = ex.Message,
                    errCode = ex.StackTrace.ToString()
                };
            }
            finally
            {
                dbConnection.conn.Close();
            }
            return responseBase;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="tablename"></param>
        /// <param name="whereclause"></param>
        /// <param name="limitclause"></param>
        /// <returns></returns>
        public ResponseBase<List<dynamic>> SelectWhere(string attr,string tablename,string whereclause,string limitclause)
        {
            var listObj = new List<dynamic>();
            var responseBase = new ResponseBase<List<dynamic>>();
           
            var dbConnection = new DBConnection();
            dbConnection.conn.Open();
            var list = new List<ProcedureParams>();
            list.Add(new ProcedureParams("@p_attr", attr, ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_table_name", tablename, ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_where_clause", whereclause, ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_limit_clause", limitclause, ParameterDirection.Input));
            try
            {
                var reader = this.CallStoreProcedure("HelperSelectWhere", list, dbConnection.conn).ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {

                        var expandoObject = new ExpandoObject() as IDictionary<string, object>;
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var Name = reader.GetName(i);
                            var val = reader.GetValue(i);
                            expandoObject.Add(Name, val);
                        }
                        listObj.Add(expandoObject);

                    }
                    responseBase.message = "True";
                    responseBase.success = true;
                    responseBase.data = listObj;
                }

                responseBase.success = true;
                responseBase.message = "success";
            }
            catch (Exception ex)
            {
                responseBase.success = false;
                responseBase.message = "error";
                responseBase.error = new ErrorResponseBase()
                {
                    message = ex.Message,
                    errCode = ex.StackTrace.ToString(),
                    exception = ex
                };
            }
            finally
            {
                dbConnection.conn.Close();
            }
            return responseBase;
        }
        /// <summary>
        ///  toggle attr attribute ( of type bool ), example : STATUS 1 then toogle to 0, if STATUS=0 then toggle to 1
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="attr"></param>
        /// <param name="whereclause"></param>
        /// <returns></returns>
        public ResponseBase ToggleWhere(string tablename,string attr,string whereclause)
        {
            var responseBase = new ResponseBase();
            var dbConnection = new DBConnection();
            dbConnection.conn.Open();
            var list = new List<ProcedureParams>();
            list.Add(new ProcedureParams("@p_table_name", tablename, ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_attr", attr, ParameterDirection.Input));
             list.Add(new ProcedureParams("@p_where_clause", whereclause, ParameterDirection.Input));
            try
            {
                var result = this.CallStoreProcedure("HelperToggleWhere", list, dbConnection.conn).ExecuteNonQuery();
                responseBase.success = true;
                responseBase.message = "success";
            }
            catch (Exception ex)
            {
                responseBase.success = false;
                responseBase.message = "error";
                responseBase.error = new ErrorResponseBase()
                {
                    message = ex.Message,
                    errCode = ex.StackTrace.ToString()
                };
            }
            finally
            {
                dbConnection.conn.Close();
            }
            return responseBase;
        }
        /// <summary>
        /// update table by object input
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="obj"></param>
        /// <param name="whereclause"></param>
        /// <returns></returns>
        public ResponseBase UpdateWhere(string tablename, dynamic obj,string whereclause)
        {
            var responseBase = new ResponseBase();
            // myDict is the object that we convert from dynamic "obj"
            var objDict = HelperDataUtilities.ToDictionary(obj);
            var query = HelperDataUtilities.ToUpdateString(objDict);
            var dbConnection = new DBConnection();
            dbConnection.conn.Open();
            var list = new List<ProcedureParams>();
            list.Add(new ProcedureParams("@p_table_name", tablename, ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_attr", query, ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_where_clause", whereclause, ParameterDirection.Input));
            try
            {
                var result = this.CallStoreProcedure("HelperUpdateWhere", list, dbConnection.conn).ExecuteNonQuery();
                responseBase.success = true;
                responseBase.message = "success";
            }
            catch (Exception ex)
            {
                responseBase.success = false;
                responseBase.message = "error";
                responseBase.error = new ErrorResponseBase()
                {
                    message = ex.Message,
                    errCode = ex.StackTrace.ToString()
                };
            }
            finally
            {
                dbConnection.conn.Close();
            }
            return responseBase;
        }
        /// <summary>
        /// reorder another modules after move position of 1 specific module
        /// </summary>
        /// <returns></returns>
        public ResponseBase ReorderModule(string val, string tablename)
        {
            var responseBase = new ResponseBase();
            var dbConnection = new DBConnection();
            dbConnection.conn.Open();
            var list = new List<ProcedureParams>();
            list.Add(new ProcedureParams("@p_val", val, ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_table_name", tablename, ParameterDirection.Input));
            try
            {
                var result = this.CallStoreProcedure("HelperReorderModule", list, dbConnection.conn).ExecuteNonQuery();
                responseBase.success = true;
                responseBase.message = "success";
            }
            catch (Exception ex)
            {
                responseBase.success = false;
                responseBase.message = "error";
                responseBase.error = new ErrorResponseBase()
                {
                    message = ex.Message,
                    errCode = ex.StackTrace.ToString()
                };
            }
            finally
            {
                dbConnection.conn.Close();
            }
            return responseBase;
        }
        /// <summary>
        /// reorder another functions after move position of 1 specific function
        /// </summary>
        /// <returns></returns>
        public ResponseBase ReorderFunction(string val, string moduleid, string tablename)
        {
            var responseBase = new ResponseBase();
            var dbConnection = new DBConnection();
            dbConnection.conn.Open();
            var list = new List<ProcedureParams>();
            list.Add(new ProcedureParams("@p_val", val, ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_module_id", moduleid, ParameterDirection.Input));
            list.Add(new ProcedureParams("@p_table_name", tablename, ParameterDirection.Input));
            try
            {
                var result = this.CallStoreProcedure("HelperReorderFunction", list, dbConnection.conn).ExecuteNonQuery();
                responseBase.success = true;
                responseBase.message = "success";
            }
            catch (Exception ex)
            {
                responseBase.success = false;
                responseBase.message = "error";
                responseBase.error = new ErrorResponseBase()
                {
                    message = ex.Message,
                    errCode = ex.StackTrace.ToString()
                };
            }
            finally
            {
                dbConnection.conn.Close();
            }
            return responseBase;
        }
    }
}
