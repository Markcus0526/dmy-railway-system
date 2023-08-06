using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Web.Hosting;
using System.IO;

namespace TLWebService.ServiceModel
{
    public class Common
    {
        public static String physicalpath = ConfigurationManager.AppSettings["ServerPhysicalPath"];

        public static string CheDuiGanBu = "车队干部";
        public static string KeShiGanBu = "科室干部";
        private static String gmailAccount = "test.ride2gather@gmail.com";
        private static String gmailPwd = "1695506955";

        public static int PASSWORD_LENGTH = 6;

        public class ExecType
        {
            public const string SectorExec = "科室干部";
            public const string TeamExec = "车队干部";
        }

        public static string GetMD5Hash(string value)
        {
            MD5 md5Hasher = MD5.Create();

            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++ )
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static String generateRandomPassword(int size)
        {
            Random _rng = new Random();
            string _chars = "0123456789";
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
                buffer[i] = _chars[_rng.Next(_chars.Length)];

            return new string(buffer);
        }

        public static bool sendEmail(String email_addr, String title, String content)
        {
            MailMessage msg = new MailMessage();
            bool bSuc = true;

            try
            {
                msg.From = new MailAddress(gmailAccount);
                msg.To.Add(email_addr);
                msg.Subject = title;
                msg.Body = content;

                SmtpClient client = new SmtpClient();
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(gmailAccount, gmailPwd);
                client.Host = "smtp.gmail.com";
                /*client.Port = 25;*/
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(msg);
            }
            catch (Exception ex)
            {
                Common.WriteLogFile("sendEmail", ex.Message);

                bSuc = false;
            }

            return bSuc;
        }

        public static void WriteLogFile(string methodName, string message)
        {
            try
            {
                string filepath = HostingEnvironment.MapPath("~/") + "\\" + "log.txt";
                if (!string.IsNullOrEmpty(message))
                {
                    using (FileStream file = new FileStream(filepath, File.Exists(filepath) ? FileMode.Append : FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        StreamWriter streamWriter = new StreamWriter(file);
                        streamWriter.WriteLine(System.DateTime.Now + " - " + methodName + " - " + message);
                        streamWriter.Close();
                    }
                }
            }
            catch {}
        }

        public static string ErrToMessage(int nErrCode)
        {
            string strRet = string.Empty;

            switch (nErrCode)
            {
                case -2:
                    strRet = "此用户不存在";
                    break;
                case 500:
                    strRet = "服务器内部错误";
                    break;
            }

            return strRet;
        }
    }
}