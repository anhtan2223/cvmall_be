using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace Framework.Core.Helpers.Email
{
    public class EmailHelper : IEmailHelper
    {
        private readonly IConfiguration configuration;

        public EmailHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public bool SendEmailWithBody(string body, string emailTo, string subject, string pathAttachment = "")
        {
            var from = new MailAddress(configuration.GetValue<string>("MailDetail:MailFrom"));
            var to = new MailAddress(emailTo);
            var mail = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(pathAttachment))
            {
                Attachment attachment = new Attachment(pathAttachment);
                mail.Attachments.Add(attachment);
            }

            SendEMail(mail);
            return true;
        }

        public bool SendMultiEmailWithBody(string body, List<string> listEmailTo, string subject, string pathAttachment = "")
        {
            var mail = new MailMessage()
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(pathAttachment))
            {
                Attachment attachment = new Attachment(pathAttachment);
                mail.Attachments.Add(attachment);
            }

            mail.From = new MailAddress(configuration.GetValue<string>("MailDetail:MailFrom"));

            foreach (var emailTo in listEmailTo)
            {
                mail.To.Add(emailTo);
            }

            SendEMail(mail);
            return true;
        }

        public void SendEmailInFunction(Uri baseUrl, string body)
        {
            HttpClient httpClient = new HttpClient();
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = baseUrl,
                Content = new StringContent(body, Encoding.UTF8, "application/json"),
            };

            httpClient.SendAsync(httpRequest);
        }

        /// <summary>
        /// Create email body content corresponding to the values ​​of object 
        /// (Body must be formed as a field [$NAME_FIELD$])
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public string GenerateBodyEmailByObj<T>(string body, T values)
        {
            if (values == null) return body;

            Type myType = values.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            foreach (PropertyInfo prop in props)
            {
                string propName = prop.Name;
                object propValue = prop.GetValue(values, null) ?? string.Empty;
                
                if (!string.IsNullOrWhiteSpace(propName))
                    body = body.Replace($"[${propName.ToUpper()}$]", propValue.ToString());
            }

            return body;
        }

        public string GenerateBodyEmailWithTemplate<T>(string pathTemplate, T values)
        {
            string body = GetTemplate(pathTemplate);

            return GenerateBodyEmailByObj<T>(body, values);
        }    

        #region private
        private void SendEMail(MailMessage message)
        {
            SmtpClient smtpClient = new SmtpClient
            {
                Host = configuration.GetValue<string>("MailDetail:Host"),
                Port = configuration.GetValue<int>("MailDetail:Port"),
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(configuration.GetValue<string>("MailDetail:UserName"), configuration.GetValue<string>("MailDetail:Password")),
                EnableSsl = true
            };
            smtpClient.Send(message);
        }

        private string GetTemplate(string templateFullPath)
        {
            string template;

            if (!File.Exists(templateFullPath))
                throw new ArgumentException("Template file does not exist: " + templateFullPath);

            using (StreamReader reader = new StreamReader(templateFullPath))
            {
                template = reader.ReadToEnd();
                reader.Close();
            }

            return template;
        }
        #endregion
    }
}
