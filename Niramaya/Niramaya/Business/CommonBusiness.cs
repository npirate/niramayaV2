using Niramaya.Database;
using Niramaya.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Niramaya.Business
{
    public class CommonBusiness : DAL
    {

        DAL dAL = new DAL();
        /// <summary>
        /// Common function to send email
        /// </summary>
        /// <param name="toemail"></param>
        /// <param name="IsHtml"></param>
        /// 
        /// 
        /// <returns>true if successfully mail sent, false otherwise</returns>
        public bool SendEmail(string typeoftemplate, string toemail, bool IsHtml, string otp = "xx")
        {
            bool success;

            var smtpdata = dAL.GetSMTPData();
            DataRow dr;
            if (smtpdata.Tables[0].Rows.Count > 0)
            {
                dr = smtpdata.Tables[0].Rows[0];
            }
            else
            {
                success = false;
                return success;
            }

            //string toemail,string subject,string body, bool IsHtml
            string GmailHost = dr["Host"].ToString();
            int GmailPort = (Int16)dr["Port"]; // Gmail can use ports 25, 465 & 587; but must be 25 for medium trust environment.
            bool GmailSSL = (bool)dr["SSL"];

            string GmailUsername = dr["Username"].ToString();
            string GmailPassword = dr["Password"].ToString();

            SmtpClient smtp = new SmtpClient();
            smtp.Host = GmailHost;
            smtp.Port = GmailPort;
            smtp.EnableSsl = GmailSSL;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(GmailUsername, GmailPassword);

            try
            {
                Dictionary<string, string> templateDict = dAL.getEmailtemplate(typeoftemplate);//fetch user registration email template


                if (templateDict["EmailSubject"].Length > 0)
                {
                    using (var message = new MailMessage(GmailUsername, toemail))
                    {
                        message.Subject = templateDict["EmailSubject"];
                        message.Body = !String.IsNullOrEmpty(otp) ? templateDict["EmailBody"].Replace("$", otp).Replace("#", Models.LoginViewModel.timeout.ToString()) : templateDict["EmailBody"];//replace otp and time out in email body
                        message.IsBodyHtml = IsHtml;
                        smtp.Send(message);
                    }
                    success = true;
                }
                else
                    success = false;

            }
            catch (Exception)
            {
                success = false;
                throw;
            }

            return success;
        }

        public ArrayList addStringArraytoArraylist(string[] myarray)
        {
            ArrayList myArrayList = new ArrayList();

            for (int i = 0; i < myarray.Length; i++)
            {
                myArrayList.Add(myarray[i]);
            }
            return myArrayList;
        }

        public object assignDBNull(object str)
        {
            if (string.IsNullOrEmpty(str.ToString()))
            {
                str = DBNull.Value;//if DOB is passed empty, assign Datetype NULL value
            }
            else
            {
                str = DateTime.Parse(str.ToString());
            }

            return str;
        }

        public object assignEmptyString(object str)
        {
            if (str == null)
            {
                str = "";//if null, make it empty string
            }

            return str.ToString();
        }

        public DateTime stringToDate(string str)//this function is not used till now!
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            DateTime dd = DateTime.ParseExact(str, Strings.dateStandardFormant, culture);

            var indianDateOnly = DateTime.Parse(dd.ToString("dd MMM yyyy"));

            return indianDateOnly;

        }

        public DataSet GetUserDataService(string username)
        {
            return dAL.GetUserData(username);
        }

        public int[] AssignValidTimeslot(TimeSpan? f1, TimeSpan? t1, TimeSpan? f2, TimeSpan? t2, TimeSpan? f3, TimeSpan? t3)
        {
            int[] allowedslot = new int[48];
            ArrayList timespan = new ArrayList();
            for (int i = 0; i < 24; i++)
            {
                for (int j = 0; j <= 30; j += 30)
                {
                    timespan.Add(new TimeSpan(i, j, 0));
                }
            }

            for (int k = 0; k < 48; k++)
            {
                TimeSpan temp = TimeSpan.Parse(timespan[k].ToString());
                if (temp >= f1 && temp <= t1)
                    allowedslot[k] = 1;
                else if (f2 != null && t2 != null)
                {
                    if (temp >= f2 && temp <= t2)
                        allowedslot[k] = 1;
                }
                else if (f3 != null && t3 != null)
                {
                    if (temp >= f3 && temp <= t3)
                        allowedslot[k] = 1;
                }
                else
                {
                    allowedslot[k] = 0;
                }
            }
            return allowedslot;
        }

        public bool SendBookingEmail(string typeoftemplate, string toemail, bool IsHtml, BookViewModel bookviewModel)
        {
            bool success;

            var smtpdata = dAL.GetSMTPData();
            DataRow dr;
            if (smtpdata.Tables[0].Rows.Count > 0)
            {
                dr = smtpdata.Tables[0].Rows[0];
            }
            else
            {
                success = false;
                return success;
            }

            //string toemail,string subject,string body, bool IsHtml
            string GmailHost = dr["Host"].ToString();
            int GmailPort = (Int16)dr["Port"]; // Gmail can use ports 25, 465 & 587; but must be 25 for medium trust environment.
            bool GmailSSL = (bool)dr["SSL"];

            string GmailUsername = dr["Username"].ToString();
            string GmailPassword = dr["Password"].ToString();

            SmtpClient smtp = new SmtpClient();
            smtp.Host = GmailHost;
            smtp.Port = GmailPort;
            smtp.EnableSsl = GmailSSL;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(GmailUsername, GmailPassword);

            try
            {
                Dictionary<string, string> templateDict = dAL.getEmailtemplate(typeoftemplate);//fetch user registration email template


                if (templateDict["EmailSubject"].Length > 0)
                {
                    using (var message = new MailMessage(GmailUsername, toemail))
                    {
                        message.Subject = templateDict["EmailSubject"];
                        message.Body = templateDict["EmailBody"].Replace("[[1]]", bookviewModel.doc_Fname).Replace("[[2]]", bookviewModel.doc_Lname).Replace("[[3]]", bookviewModel.Pat_Fname).Replace("[[4]]", bookviewModel.Pat_Lname).Replace("[[5]]", bookviewModel.Pat_BookDate.HasValue ? bookviewModel.Pat_BookDate.Value.ToString("dd/MM/yyyy") : "").Replace("[[6]]", bookviewModel.Pat_slot.ToString()).Replace("[[7]]", bookviewModel.Pat_Phone);//replace all parameters in email body
                        message.IsBodyHtml = IsHtml;
                        smtp.Send(message);
                    }
                    success = true;
                }
                else
                    success = false;

            }
            catch (Exception)
            {
                success = false;
                throw;
            }

            return success;
        }
    }
}
