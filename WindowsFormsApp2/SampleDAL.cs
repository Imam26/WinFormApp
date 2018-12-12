using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Common;
using System.Configuration;

namespace WindowsFormsApp2
{
    class SampleDAL
    {
        private DbConnection cnStr = null;

        public void OpenConnection(DbProviderFactory factory,string connectionStr)
        {
            cnStr = factory.CreateConnection();
            cnStr.ConnectionString = connectionStr;

            cnStr.Open();
        }

        public void CloseConnection()
        {
            if (cnStr.State == System.Data.ConnectionState.Open)
            {
                cnStr.Close();
            }
        }
        
        public void InsertToSellersAndCostumers(List<object []> insertedData, string tbName)
        {
            StringBuilder insertCmd = new StringBuilder();

            try
            {
                using (DbCommand cmd = cnStr.CreateCommand())
                {
                    for (int i = 0; i < insertedData.Count; i++)
                    {
                        insertCmd.AppendFormat($"Insert into {tbName} values(@id{i}, @name{i}, @surname{i}) ");

                        DbParameter[] param = new DbParameter[3] 
                        { cmd.CreateParameter(), cmd.CreateParameter(), cmd.CreateParameter() };

                        param[0].ParameterName = $"@id{i}";
                        param[0].DbType = System.Data.DbType.Int32;
                        param[0].Value = (int)insertedData[i][0];
                        cmd.Parameters.Add(param[0]);
                        param[1].ParameterName = $"@name{i}";
                        param[1].DbType = System.Data.DbType.String;
                        param[1].Value = (string)insertedData[i][1];
                        cmd.Parameters.Add(param[1]);
                        param[2].ParameterName = $"@surname{i}";
                        param[2].DbType = System.Data.DbType.String;
                        param[2].Value = (string)insertedData[i][2];
                        cmd.Parameters.Add(param[2]);
                    }

                    cmd.CommandText = insertCmd.ToString();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (DbException ex)
            {
                throw ex;
            }
        }

        public void InsertToSales(List<object[]> insertedData)
        {
            StringBuilder insertCmd = new StringBuilder();

            try
            {
                using (DbCommand cmd = cnStr.CreateCommand())
                {
                    for (int i = 0; i < insertedData.Count; i++)
                    {
                        insertCmd.AppendFormat($"Insert into Sales values(@costId{i}, @selId{i}, @sum{i}, @data{i}) ");

                        DbParameter[] param = new DbParameter[4]
                        { cmd.CreateParameter(), cmd.CreateParameter(), cmd.CreateParameter(), cmd.CreateParameter() };

                        param[0].ParameterName = $"@costId{i}";
                        param[0].DbType = System.Data.DbType.Int32;
                        param[0].Value = (int)insertedData[i][0];
                        cmd.Parameters.Add(param[0]);
                        param[1].ParameterName = $"@selId{i}";
                        param[1].DbType = System.Data.DbType.String;
                        param[1].Value = (int)insertedData[i][1];
                        cmd.Parameters.Add(param[1]);
                        param[2].ParameterName = $"@sum{i}";
                        param[2].DbType = System.Data.DbType.String;
                        param[2].Value = (int)insertedData[i][2];
                        cmd.Parameters.Add(param[2]);
                        param[3].ParameterName = $"@data{i}";
                        param[3].DbType = System.Data.DbType.Date;
                        param[3].Value = ((DateTime)insertedData[i][3]).Date;
                        cmd.Parameters.Add(param[3]);
                    }

                    cmd.CommandText = insertCmd.ToString();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (DbException ex)
            {
                throw ex;
            }
        }
    }
};
