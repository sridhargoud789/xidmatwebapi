
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ServicesAPI.Helpers
{
    public class WebMail
    {
        /// <summary>
        /// This helper class sends an email message using the System.Net.Mail namespace
        /// </summary>
        /// <param name="fromEmail">Sender email address</param>
        /// <param name="toEmail">Recipient email address</param>
        /// <param name="bcc">Blind carbon copy email address</param>
        /// <param name="cc">Carbon copy email address</param>
        /// <param name="subject">Subject of the email message</param>
        /// <param name="body">Body of the email message</param>
        /// <param name="attachment">File to attach</param>

        #region Static Members

        public void SendMailMessage(
      string TO,
      string CC,
      string Subject,
      string Body,
      out bool Status,
      out string Message)
        {
            ConfigurationManager.AppSettings["SMTP_FROM_EMAILID"].ToString();
            ConfigurationManager.AppSettings["SMTP_FROM_PASSWORD"].ToString();
            ConfigurationManager.AppSettings["SMTP_FROM_BCC"].ToString();
            try
            {
                MailMessage message = new MailMessage("support@xidmat.com", TO);
                message.From = new MailAddress("support@xidmat.com");
                
                message.CC.Add(CC);
                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = true;
                new SmtpClient()
                {
                    Credentials = ((ICredentialsByHost)new NetworkCredential("support@xidmat.com", "Abcd1234$$")),
                    Host = "relay-hosting.secureserver.net"
                }.Send(message);
                message.Dispose();
                Status = true;
                Message = "SUCCESS";
            }
            catch (Exception ex)
            {
                Status = false;
                Message = ex.Message;
            }
        }


        /// <summary>
        /// Determines whether an email address is valid.
        /// </summary>
        /// <param name="emailAddress">The email address to validate.</param>
        /// <returns>
        /// 	<c>true</c> if the email address is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValidEmailAddress(string emailAddress)
        {
            // An empty or null string is not valid
            if (String.IsNullOrEmpty(emailAddress))
            {
                return (false);
            }

            // Regular expression to match valid email address
            string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            // Match the email address using a regular expression
            Regex re = new Regex(emailRegex);
            if (re.IsMatch(emailAddress))
                return (true);
            else
                return (false);
        }

        #endregion
    }
}