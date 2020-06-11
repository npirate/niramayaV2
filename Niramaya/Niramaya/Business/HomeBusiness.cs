using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Niramaya.Database;
using Niramaya.Models;

namespace Niramaya.Business
{
    public class HomeBusiness : LoginViewModel
    {
        private readonly ILogger<HomeBusiness> _logger;

        public string businessEmail;

        public HomeBusiness()
        {           

        }

        HomeDAL homeDAL = new HomeDAL();

        public float isEmailRegisteredService(LoginViewModel loginViewModel)
        {
          
            return homeDAL.isEmailRegistered(loginViewModel);
        }

        public float isUsernameUniqueService(LoginViewModel loginViewModel)
        {
            
            return homeDAL.isUsernameUnique(loginViewModel);
        }

        public int GenerateOTP()
        {
            Random rnd = new Random();
            int otp = rnd.Next(1000, 9999);
            return otp;
        }

        private byte[] Hashpassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 4;     //2 core
            argon2.Iterations = 2;
            argon2.MemorySize = 1024 * 1024;    // 1 GB

            return argon2.GetBytes(16);
        }


        public bool doUserRegister(LoginViewModel loginViewModel)
        {
            var basevalue = System.Convert.ToBase64String(Hashpassword(loginViewModel.Password, Startup.salt));

            return homeDAL.insertUserRegistration(loginViewModel, basevalue);
        }

        public bool doUpdatePassword(LoginViewModel loginViewModel)
        {
            var basevalue = System.Convert.ToBase64String(Hashpassword(loginViewModel.Password, Startup.salt));

            return homeDAL.updatePassword(loginViewModel, basevalue);
        }

        public string doUserLogin(LoginViewModel loginViewModel)
        {
            var basevalue = System.Convert.ToBase64String(Hashpassword(loginViewModel.Password, Startup.salt));

            return homeDAL.checkUserLogin(loginViewModel, basevalue);
        }

        public DataSet SearchDocService(ProfileViewModel profileViewModel, int pageindex = 1)
        {
            return homeDAL.SearchDocData(profileViewModel, pageindex);
        }

        public int GetSearchDocCountService(ProfileViewModel ProfileViewModel)
        {
            return homeDAL.GetSearchDocCount(ProfileViewModel);
        }

        public DataSet GetBookProfileService(BookViewModel bookViewModel)
        {
            return homeDAL.GetBookProfileData(bookViewModel);
        }

        public bool insertBookingService(BookViewModel bookViewModel)
        {
           
            return homeDAL.insertBookingData(bookViewModel);
        }
    }
}
