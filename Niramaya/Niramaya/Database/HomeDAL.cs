using Microsoft.AspNetCore.Http;
using Niramaya.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Niramaya.Database
{
    public class HomeDAL : DAL
    {
        static SqlConnection sqlConnHomeDAL;
        public HomeDAL()
        {
            sqlConnHomeDAL = new SqlConnection(Startup.ConnectionString);
        }
        public HomeDAL(SqlConnection sqlConnHomeDAL) : base(sqlConnHomeDAL)
        {
            //DAL constructor called to assign connection string property
        }

        public int isEmailRegistered(LoginViewModel loginViewModel)
        {
            int emailCount = 0;
            try
            {
                using (sqlConnHomeDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "isEmailRegistered";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnHomeDAL))
                    {
                        sqlConnHomeDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@Email", loginViewModel.Email);

                        emailCount = (int)sqlCmd.ExecuteScalar();
                        sqlConnHomeDAL.Close();
                    }
                }

            }
            catch (Exception e)
            {
                return -1;
            }
            return emailCount;
        }

        public int isUsernameUnique(LoginViewModel loginViewModel)
        {
            int usernameCount = 0;
            try
            {
                using (sqlConnHomeDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "isUsernameUnique";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnHomeDAL))
                    {
                        sqlConnHomeDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@username", loginViewModel.Username);

                        usernameCount = (int)sqlCmd.ExecuteScalar();
                        sqlConnHomeDAL.Close();
                    }
                }

            }
            catch (Exception e)
            {
                return -1;
            }
            return usernameCount;
        }

        public bool insertUserRegistration(LoginViewModel loginViewModel, string phash)
        {
            bool success = false;
            try
            {
                using (sqlConnHomeDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "RegisterUser";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnHomeDAL))
                    {
                        sqlConnHomeDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@email", LoginViewModel.varEmail);
                        sqlCmd.Parameters.AddWithValue("@username", loginViewModel.Username);
                        sqlCmd.Parameters.AddWithValue("@phash", phash);
                        int k = sqlCmd.ExecuteNonQuery();
                        if (k != 0)
                        {
                            success = true;
                        }
                        sqlConnHomeDAL.Close();
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

        public string checkUserLogin(LoginViewModel loginViewModel, string phash)
        {
            string returnValue = "";

            try
            {
                using (sqlConnHomeDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "isUserLogin";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnHomeDAL))
                    {
                        sqlConnHomeDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@email", loginViewModel.Email);
                        sqlCmd.Parameters.AddWithValue("@phash", phash);
                        var k = sqlCmd.ExecuteScalar();

                        if (k.ToString() != "none")
                        {
                            returnValue = k.ToString();
                        }
                        sqlConnHomeDAL.Close();
                    }
                }

            }
            catch (System.Exception e)
            {
                throw;
            }

            return returnValue;
        }

        public bool updatePassword(LoginViewModel loginViewModel, string phash)
        {
            bool success = false;
            try
            {
                using (sqlConnHomeDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "updatePassword";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnHomeDAL))
                    {
                        sqlConnHomeDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@email", LoginViewModel.varEmail);
                        sqlCmd.Parameters.AddWithValue("@phash", phash);
                        int k = sqlCmd.ExecuteNonQuery();
                        if (k != 0)
                        {
                            success = true;
                        }
                        sqlConnHomeDAL.Close();
                    }
                }

            }
            catch (System.Exception e)
            {
                success = false;
                throw;
            }

            return success;
        }

        public DataSet SearchDocData(ProfileViewModel profileViewModel, int pageindex = 1)
        {
            DataSet srDataSet = new DataSet();

            try
            {
                using (sqlConnHomeDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "SearchDoctor";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnHomeDAL))
                    {
                        sqlConnHomeDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@Input_text", profileViewModel.doc_SearchText);
                        sqlCmd.Parameters.AddWithValue("@page_index", pageindex);
                        sqlCmd.Parameters.AddWithValue("@get_count", 0);

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(srDataSet);

                        sqlConnHomeDAL.Close();
                    }
                }

            }
            catch (Exception e)
            {
                throw;
            }
            return srDataSet;
        }

        public int GetSearchDocCount(ProfileViewModel profileViewModel)
        {
            int count = 0;
            try
            {
                using (sqlConnHomeDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "SearchDoctor";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnHomeDAL))
                    {
                        sqlConnHomeDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@Input_text", profileViewModel.doc_SearchText);
                        sqlCmd.Parameters.AddWithValue("@get_count", 1);

                        count = Convert.ToInt32(sqlCmd.ExecuteScalar());

                        sqlConnHomeDAL.Close();
                    }
                }

            }
            catch (Exception e)
            {
                throw;
            }
            return count;
        }

        public DataSet GetBookProfileData(BookViewModel bookViewModel)
        {
            DataSet srDataSet = new DataSet();

            try
            {
                using (sqlConnHomeDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "GetProfileData";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnHomeDAL))
                    {
                        sqlConnHomeDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@username", bookViewModel.doc_username);
                        sqlCmd.Parameters.AddWithValue("@get_count", 0);

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(srDataSet);

                        sqlConnHomeDAL.Close();
                    }
                }

            }
            catch (Exception e)
            {
                throw;
            }
            return srDataSet;
        }

        public bool insertBookingData(BookViewModel bookViewModel)
        {
            bool success = false;
            try
            {
                using (sqlConnHomeDAL = new SqlConnection(Startup.ConnectionString))
                {
                    string sqlQuery = "InsertBookings";
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConnHomeDAL))
                    {
                        sqlConnHomeDAL.Open();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@userid", BookViewModel.doc_userid);
                        sqlCmd.Parameters.AddWithValue("@username", BookViewModel.username);
                        sqlCmd.Parameters.AddWithValue("@caller_Fname", bookViewModel.Pat_Fname);
                        sqlCmd.Parameters.AddWithValue("@caller_Lname", bookViewModel.Pat_Lname);
                        sqlCmd.Parameters.AddWithValue("@caller_Mob", bookViewModel.Pat_Phone);
                        sqlCmd.Parameters.AddWithValue("@Book_date", bookViewModel.Pat_BookDate);
                        sqlCmd.Parameters.AddWithValue("@Book_timeslot", bookViewModel.Pat_slot);
                        int k = sqlCmd.ExecuteNonQuery();
                        if (k != 0)
                        {
                            success = true;
                        }
                        sqlConnHomeDAL.Close();
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
