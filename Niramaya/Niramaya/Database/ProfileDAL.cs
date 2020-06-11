using Niramaya.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Niramaya.Database
{
    public class ProfileDAL : DAL
    {
        static SqlConnection sqlConnProfileDAL;

        public bool saveProfileData(ProfileViewModel profileViewModel)
        {
            bool success = false;
            try
            {
                using (sqlConnProfileDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "UpdateDocDetail";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnProfileDAL))
                    {
                        sqlConnProfileDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@username", ProfileViewModel.doc_username);
                        sqlCmd.Parameters.AddWithValue("@Fname", profileViewModel.doc_Fname);
                        sqlCmd.Parameters.AddWithValue("@Mname", profileViewModel.doc_Mname);
                        sqlCmd.Parameters.AddWithValue("@Lname", profileViewModel.doc_Lname);
                        sqlCmd.Parameters.AddWithValue("@Gender", profileViewModel.doc_Gender);
                        sqlCmd.Parameters.AddWithValue("@DOB", profileViewModel.doc_DOB);
                        sqlCmd.Parameters.AddWithValue("@Grad", profileViewModel.doc_Degree);
                        sqlCmd.Parameters.AddWithValue("@PostGrad", profileViewModel.doc_PostDegree);
                        sqlCmd.Parameters.AddWithValue("@Mob", profileViewModel.doc_Phone);
                        sqlCmd.Parameters.AddWithValue("@Pemail", profileViewModel.doc_Email);
                        sqlCmd.Parameters.AddWithValue("@clinicname", profileViewModel.doc_Clinicname);
                        sqlCmd.Parameters.AddWithValue("@Address1", profileViewModel.doc_Address1);
                        sqlCmd.Parameters.AddWithValue("@Address2", profileViewModel.doc_Address2);
                        sqlCmd.Parameters.AddWithValue("@city", profileViewModel.doc_City);
                        sqlCmd.Parameters.AddWithValue("@state", profileViewModel.doc_State);
                        sqlCmd.Parameters.AddWithValue("@pincode", profileViewModel.doc_Pincode);
                        sqlCmd.Parameters.AddWithValue("@clinicphone", profileViewModel.doc_ClinicsPhone);
                        sqlCmd.Parameters.AddWithValue("@services", profileViewModel.doc_Services);
                        int k = sqlCmd.ExecuteNonQuery();
                        if (k != 0)
                        {
                            success = true;
                        }
                        sqlConnProfileDAL.Close();
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

        public DataSet GetProfileData(ProfileViewModel profileViewModel, string username)
        {
            DataSet srDataSet = new DataSet();

            try
            {
                using (sqlConnProfileDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "GetProfileData";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnProfileDAL))
                    {
                        sqlConnProfileDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@username", username);
                        sqlCmd.Parameters.AddWithValue("@get_count", 0);//it will give count for ispublish = 1

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(srDataSet);

                        sqlConnProfileDAL.Close();
                    }
                }

            }
            catch (Exception e)
            {
                throw;
            }
            return srDataSet;
        }

        public int GetPublishedProfileDataCount(ProfileViewModel profileViewModel, string username)
        {
            int count = 0;
            try
            {
                using (sqlConnProfileDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "GetProfileData";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnProfileDAL))
                    {
                        sqlConnProfileDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@username", username);
                        sqlCmd.Parameters.AddWithValue("@get_count", 1);

                        count = Convert.ToInt32(sqlCmd.ExecuteScalar());

                        sqlConnProfileDAL.Close();
                    }
                }

            }
            catch (Exception e)
            {
                throw;
            }
            return count;
        }

        public bool UpdateAppointmentData(ProfileViewModel profileViewModel)
        {
            bool success = false;
            try
            {
                using (sqlConnProfileDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "UpdateAppointment";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnProfileDAL))
                    {
                        sqlConnProfileDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@userid", ProfileViewModel.doc_userid);
                        sqlCmd.Parameters.AddWithValue("@daychecked", string.Join(",", profileViewModel.DayChecked));
                        sqlCmd.Parameters.AddWithValue("@f1", profileViewModel.from1);
                        sqlCmd.Parameters.AddWithValue("@t1", profileViewModel.to1);
                        sqlCmd.Parameters.AddWithValue("@f2", (object)profileViewModel.from2 ?? DBNull.Value);
                        sqlCmd.Parameters.AddWithValue("@t2", (object)profileViewModel.to2 ?? DBNull.Value);
                        sqlCmd.Parameters.AddWithValue("@f3", (object)profileViewModel.from3 ?? DBNull.Value);
                        sqlCmd.Parameters.AddWithValue("@t3", (object)profileViewModel.to3 ?? DBNull.Value);

                        int k = sqlCmd.ExecuteNonQuery();
                        if (k != 0)
                        {
                            success = true;
                        }

                        sqlConnProfileDAL.Close();
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

        public bool PublishProfileData(ProfileViewModel profileViewModel, string username)
        {
            bool success = false;
            try
            {
                using (sqlConnProfileDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "doPublish";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnProfileDAL))
                    {
                        sqlConnProfileDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@username", username);

                        int k = sqlCmd.ExecuteNonQuery();
                        if (k != 0)
                        {
                            success = true;
                        }

                        sqlConnProfileDAL.Close();
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

        public bool UnPublishProfileData(ProfileViewModel profileViewModel, string username)
        {
            bool success = false;
            try
            {
                using (sqlConnProfileDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "doUnpublish";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnProfileDAL))
                    {
                        sqlConnProfileDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@username", username);

                        int k = sqlCmd.ExecuteNonQuery();
                        if (k != 0)
                        {
                            success = true;
                        }

                        sqlConnProfileDAL.Close();
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


    }
}
