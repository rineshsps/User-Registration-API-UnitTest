using Microsoft.Extensions.Options;
using Mongo.Database.Models;
using Mongo.Services.Interfaces;
using Mongo.Settings;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly IOptions<ApplicationSettings> _settings;

        public EmailServices(IOptions<ApplicationSettings> settings)
        {
            this._settings = settings;
        }

        public async Task SendMail(User user)
        {
            try
            {
                if (_settings.Value.AppSettings.EnableSendMail)
                {
                    var apiKey = _settings.Value.AppSettings.SendGridSecret;
                    var client = new SendGridClient(apiKey);

                    var subject = "Registration Succesful.";
                    var from = new EmailAddress(_settings.Value.AppSettings.FromEmail, _settings.Value.AppSettings.FromName);
                    var to = new EmailAddress(user.UserName, user.FullName);
                    var plainTextContent = "Welcome, User registration successful.";
                    var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

                    var response = await client.SendEmailAsync(msg);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
