using MySql.Data.MySqlClient;
namespace BusinessDatabase
{

    class DBConnection
    {

    
        public MySqlConnection conn;

        public DBConnection()
        {
            InitializeProceduce();
        }


        private void InitializeProceduce()
        {
            string connectionString = System.Configuration.ConfigurationManager.AppSettings["DbConnection"];
            if(this.conn == null)
            {
                this.conn = new MySqlConnection();
            }
            this.conn.ConnectionString = connectionString;
        }



    }
}
