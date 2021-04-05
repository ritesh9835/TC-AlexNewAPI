using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TazzerClean.Util.Models;

namespace TazzerClean.Util
{
    public class CummunicationHelper : EmailHelper, IDisposable
    {
        public CummunicationHelper(IConfiguration configuration) : base(configuration)
        {
        }

        //email after registration
        public async Task SendEmailVerifyEmail(WelcomeEmail welcomeEmail, string to)
        {
            try
            {
                using StreamReader sr = new StreamReader("EmailTemplates/Welcome.html");
                string s = sr.ReadToEnd();
                string body = s.Replace("{logo_url}", welcomeEmail.LogoUrl)
                    .Replace("{full_name}", welcomeEmail.Name)
                    .Replace("{verify_email_link}", welcomeEmail.VerifyUrl);

                await SendEmail(to,EmailSubjectConstants.WelcomeEmail, body);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Invitation mail
        public async Task SendEmailInvitation(WelcomeEmail welcomeEmail, string to)
        {
            try
            {
                using StreamReader sr = new StreamReader("EmailTemplates/InviteEmail.html");
                string s = sr.ReadToEnd();
                string body = s.Replace("{logo_url}", welcomeEmail.LogoUrl)
                    .Replace("{full_name}", welcomeEmail.Name)
                    .Replace("{verify_email_link}", welcomeEmail.VerifyUrl);

                await SendEmail(to, EmailSubjectConstants.WelcomeEmail, body);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Email verification
        public async Task SendEmailVerifyCode(WelcomeEmail welcomeEmail,string code, string to)
        {
            try
            {
                using StreamReader sr = new StreamReader("EmailTemplates/VerifyEmail.html");
                string s = sr.ReadToEnd();
                string body = s.Replace("{logo_url}", welcomeEmail.LogoUrl)
                    .Replace("{full_name}", welcomeEmail.Name)
                    .Replace("{code}", code);

                await SendEmail(to, EmailSubjectConstants.WelcomeEmail, body);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendForgotPassword(Common passwordEmail,string token, string to)
        {
            try
            {
                using StreamReader sr = new StreamReader("EmailTemplates/ForgotPassword.html");
                string s = sr.ReadToEnd();
                string body = s.Replace("{logo_url}", passwordEmail.LogoUrl)
                    .Replace("{full_name}", passwordEmail.Name)
                    .Replace("{token}", token);

                await SendEmail(to, EmailSubjectConstants.ForgotPassword, body);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
