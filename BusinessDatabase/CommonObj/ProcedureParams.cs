using System.Data;

namespace BusinessDatabase.CommonObj
{
    public class ProcedureParams
    {

        public ProcedureParams(string ParamName, string ParamVal, ParameterDirection Direction)
        {
            this.ParamName = ParamName;
            this.ParamVal = ParamVal;
            this.Direction = Direction;
        }
        public string ParamName { get; set; }
        public string ParamVal { get; set; }
        public ParameterDirection Direction { get; set; }
    }
}
