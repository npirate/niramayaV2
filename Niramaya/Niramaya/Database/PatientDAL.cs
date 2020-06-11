using Niramaya.Business;
using Niramaya.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Niramaya.Database
{
    public class PatientDAL : DAL
    {
        static SqlConnection sqlConnPatientDAL;
        CommonBusiness commonBusiness = new CommonBusiness();

        public PatientDAL()
        {
            sqlConnPatientDAL = new SqlConnection(Startup.ConnectionString);
        }
        public PatientDAL(SqlConnection sqlConnPatientDAL) : base(sqlConnPatientDAL)
        {
            //DAL constructor called to assign connection string property
        }

        public bool AddPatientData(PatientViewModel patientViewModel, ArrayList inputParameter)
        {
            bool success = false;

            inputParameter[11] = commonBusiness.assignEmptyString(inputParameter[11]); //if occupation is null or not supplied, make it empty
            try
            {
                using (sqlConnPatientDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "AddPatientDetail";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnPatientDAL))
                    {
                        sqlConnPatientDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@Fname", inputParameter[0]);
                        sqlCmd.Parameters.AddWithValue("@Mname", inputParameter[1]);
                        sqlCmd.Parameters.AddWithValue("@Lname", inputParameter[2]);
                        sqlCmd.Parameters.AddWithValue("@Gender", inputParameter[3]);
                        sqlCmd.Parameters.AddWithValue("@DOB", DateTime.Parse(inputParameter[4].ToString()));
                        sqlCmd.Parameters.AddWithValue("@Mob", inputParameter[5]);
                        sqlCmd.Parameters.AddWithValue("@Pemail", inputParameter[6]);
                        sqlCmd.Parameters.AddWithValue("@Address1", inputParameter[7]);
                        sqlCmd.Parameters.AddWithValue("@pincode", inputParameter[8]);
                        sqlCmd.Parameters.AddWithValue("@city", inputParameter[9]);
                        sqlCmd.Parameters.AddWithValue("@state", inputParameter[10]);
                        sqlCmd.Parameters.AddWithValue("@occupation", inputParameter[11]);
                        sqlCmd.Parameters.AddWithValue("@createdDatetime", DateTime.Parse(inputParameter[12].ToString()));
                        sqlCmd.Parameters.AddWithValue("@modifiedDatetime", DateTime.Parse(inputParameter[13].ToString()));

                        int k = sqlCmd.ExecuteNonQuery();
                        if (k != 0)
                        {
                            success = true;
                        }
                        sqlConnPatientDAL.Close();
                    }
                }

            }
            catch (Exception e)
            {
                success = false;
                throw;
            }

            return success;
        }

        public DataSet SearchPatientData(PatientViewModel patientViewModel, ArrayList inputParameter, int pageindex = 1)
        {
            DataSet srDataSet = new DataSet();

            inputParameter[3] = commonBusiness.assignDBNull(inputParameter[3]); //if DOB is passed empty, assign Datetype NULL value

            try
            {
                using (sqlConnPatientDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "SearchPatient";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnPatientDAL))
                    {
                        sqlConnPatientDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@Pat_FirstName", inputParameter[0]);
                        sqlCmd.Parameters.AddWithValue("@Pat_MiddleName", inputParameter[1]);
                        sqlCmd.Parameters.AddWithValue("@Pat_LastName", inputParameter[2]);
                        sqlCmd.Parameters.AddWithValue("@Pat_DOB", inputParameter[3]);
                        sqlCmd.Parameters.AddWithValue("@Pat_mobile", inputParameter[4]);
                        sqlCmd.Parameters.AddWithValue("@Pat_PinCode", inputParameter[5]);
                        sqlCmd.Parameters.AddWithValue("@page_index", pageindex);
                        sqlCmd.Parameters.AddWithValue("@get_count", 0);

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(srDataSet);

                        sqlConnPatientDAL.Close();
                    }
                }

            }
            catch (Exception e)
            {
                throw;
            }
            return srDataSet;
        }

        public int GetPatientDataCount(PatientViewModel patientViewModel, ArrayList inputParameter)
        {
            inputParameter[3] = commonBusiness.assignDBNull(inputParameter[3]); //if DOB is passed empty, assign Datetype NULL value

            int count = 0;
            try
            {
                using (sqlConnPatientDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "SearchPatient";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnPatientDAL))
                    {
                        sqlConnPatientDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@Pat_FirstName", inputParameter[0]);
                        sqlCmd.Parameters.AddWithValue("@Pat_MiddleName", inputParameter[1]);
                        sqlCmd.Parameters.AddWithValue("@Pat_LastName", inputParameter[2]);
                        sqlCmd.Parameters.AddWithValue("@Pat_DOB", inputParameter[3]);
                        sqlCmd.Parameters.AddWithValue("@Pat_mobile", inputParameter[4]);
                        sqlCmd.Parameters.AddWithValue("@Pat_PinCode", inputParameter[5]);
                        sqlCmd.Parameters.AddWithValue("@get_count", 1);

                        count = Convert.ToInt32(sqlCmd.ExecuteScalar());

                        sqlConnPatientDAL.Close();
                    }
                }

            }
            catch (Exception e)
            {
                throw;
            }
            return count;
        }

    }
}
