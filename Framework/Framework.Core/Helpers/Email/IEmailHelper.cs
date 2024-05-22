using System.Net.Mail;

namespace Framework.Core.Helpers.Email
{
    public interface IEmailHelper
    {
        bool SendEmailWithBody(string body, string emailTo, string subject, string pathAttachment = "");
        bool SendMultiEmailWithBody(string body, List<string> listEmailTo, string subject, string pathAttachment = "");
        void SendEmailInFunction(Uri baseUrl, string body);
        string GenerateBodyEmailByObj<T>(string body, T values);
        string GenerateBodyEmailWithTemplate<T>(string pathTemplate, T values);
    }
}
