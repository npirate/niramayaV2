using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Niramaya.Database
{
    public class DAL
    {
        SqlConnection sqlConnDAL;
        public DAL()
        {
            sqlConnDAL = new SqlConnection(Startup.ConnectionString);
        }

        public DAL(SqlConnection sqlConn)
        {
            sqlConn = new SqlConnection(Startup.ConnectionString);
        }

        public Dictionary<string, string> getEmailtemplate(string typeoftemplate)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            try
            {
                using (sqlConnDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "GetEmailTemplate";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnDAL))
                    {
                        sqlConnDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@Type", typeoftemplate);
                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dict.Add("TemplateType", reader["TemplateType"] as string);
                                dict.Add("EmailSubject", reader["EmailSubject"] as string);
                                dict.Add("EmailBody", reader["EmailBody"] as string);
                            }
                        }
                        sqlConnDAL.Close();
                    }
                }

            }
            catch (System.Exception e)
            {
                //get count of dict count, which will be zero
            }

            return dict;
        }

        public DataSet GetUserData(string username)
        {
            DataSet srDataSet = new DataSet();

            try
            {
                using (sqlConnDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "getUserDetailFromUsername";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnDAL))
                    {
                        sqlConnDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@username", username);

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(srDataSet);

                        sqlConnDAL.Close();
                    }
                }

            }
            catch (Exception e)
            {
                throw;
            }
            return srDataSet;
        }

        public DataSet GetSMTPData()
        {
            DataSet srDataSet = new DataSet();

            try
            {
                using (sqlConnDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "GetSMTPConfig";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnDAL))
                    {
                        sqlConnDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(srDataSet);

                        sqlConnDAL.Close();
                    }
                }

            }
            catch (Exception e)
            {
                throw;
            }
            return srDataSet;
        }
    }
}
