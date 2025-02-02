using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NewsSite.Models.Utility
{
    public class Utility
    {
        public DateTime ConvertToShamsi(DateTime dt)
        {
            try
            {
                PersianCalendar pc = new PersianCalendar();
                int Day = pc.GetDayOfMonth(dt);
                int Month = pc.GetMonth(dt);
                int Year = pc.GetYear(dt);

                return Convert.ToDateTime(Year.ToString() + "/" + Month.ToString() + "/" + Day.ToString());
            }
            catch
            {

                return Convert.ToDateTime("1300/1/1");
            }
        }

        public bool SendEmail(string Smtp, string FromEmail, string Password, string To, string Subject, string Message)
        {
            try
            {
                MailMessage MyMessage = new MailMessage();
                MyMessage.From = new MailAddress(FromEmail);
                MyMessage.To.Add(To);
                MyMessage.Subject = Subject;
                MyMessage.Body = Message;
                MyMessage.IsBodyHtml = true;
                MyMessage.Priority = MailPriority.High;
                //if (Attach != null)
                //{
                //    string filename = Attach.FileName;
                //    string filepath = "D:\\" + filename;
                //    Attach.SaveAs(filepath);
                //    Attachment attach = new Attachment(filepath);
                //    attach.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                //    MyMessage.Attachments.Add(attach);
                //}

                SmtpClient mysmtp = new SmtpClient(Smtp);
                mysmtp.UseDefaultCredentials = false;
                mysmtp.EnableSsl = true;
                mysmtp.Credentials = new NetworkCredential(FromEmail, Password);
                mysmtp.Port = 25;
                mysmtp.Send(MyMessage);
                return true;
            }
            catch
            {

                return false;
            }

        }

        public string GetNameNewsType(string Type)
        {
            switch (Type)
            {
                case "News":
                    {
                        return "خبر";
                    }
                case "Sp":
                    {
                        return "ویژه";
                    }
                case "Memo":
                    {
                        return "یاداشت";
                    }
                default:
                    return "";
            }
        }
    }
}