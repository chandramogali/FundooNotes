using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ModelLayer
{
    public class Send
    {
        public string SendingMail(string emailTo, string token)
        {
            try
            {
                string emailFrom = "mogalihpp97@gmail.com";

                MailMessage message = new MailMessage(emailFrom,emailTo);

                string mailBody = "Token generated :" + token;
                message.Subject = "Generated token willexpire after 1 hour";
                message.Body = mailBody.ToString();
                message.BodyEncoding= Encoding.UTF8;
                message.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com",587);
                NetworkCredential credential = new NetworkCredential("mogalihpp97@gmail.com", "bpnl wesp ewtk mtnx");

                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials= credential;
                smtpClient.Send(message);
                return emailFrom;

            }
            catch(Exception ex) {
                throw ex;
            }
        }
    }
}
