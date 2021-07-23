using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace IF4101_proyecto3_web.Data
{
    public class ConnectionDb
    {
        public SqlCommand SqlCommand { get; set; }
        public SqlConnection SqlConnection { get; set; }
        public SqlDataReader SqlDataReader { get; set; }
        public SqlParameter ParameterReturn { get; set; }

        public IConfiguration Configuration { get; }

        public void InitSqlComponents(string commandText)
        {
            this.SqlConnection = new SqlConnection("Data Source=163.178.107.10;Initial Catalog=IF4101_proyecto3_B95212_B97452;Persist Security Info=True;User ID=laboratorios;Password=KmZpo.2796;Pooling=False");
            this.SqlCommand = new SqlCommand(commandText, this.SqlConnection);
        }

        public void CreateParameter(string parameterName, SqlDbType dbType, object value)
        {
            SqlParameter sqlParameter = new(parameterName, dbType);
            sqlParameter.Value = value;
            this.SqlCommand.Parameters.Add(sqlParameter);
        }

        public void CreateParameterOutput()
        {
            this.ParameterReturn = new SqlParameter();
            ParameterReturn.Direction = ParameterDirection.ReturnValue;
            this.SqlCommand.Parameters.Add(this.ParameterReturn);
        }

        public void ExecuteNonQuery()
        {
            this.ExecuteConnectionCommands();
            this.SqlCommand.ExecuteNonQuery();
            this.SqlConnection.Close();
        }

        public void ExcecuteReader()
        {
            this.ExecuteConnectionCommands();
            this.SqlDataReader = this.SqlCommand.ExecuteReader();
        }

        public void ExecuteConnectionCommands()
        {
            this.SqlConnection.Open();
            this.SqlCommand.CommandType = CommandType.StoredProcedure;
        }
    }
}
