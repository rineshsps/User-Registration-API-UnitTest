using Mongo.Database.Interfaces;
using Mongo.Database.Models;
using Mongo.DTOs;
using Mongo.Services.Interfaces;
using MongoDB.Driver;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo.Services
{
    public class UserServices : IUserServices
    {
        private readonly IMongoCollection<User> _users;
        private readonly IEmailServices _emailServices;

        public UserServices(IUsersContext db, IEmailServices emailServices)
        {
            _users = db.GetUserssCollection();
            this._emailServices = emailServices;
        }

        /// <summary>
        /// User creation with password hash
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User Create(User user)
        {
            //Hashing the password for security 
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var validUser = GetUser(user.UserName);
            if (validUser == null)
            {
                user.Active = true;
                user.CreatedDate = DateTime.Now;

                _users.InsertOne(user);

                // Welcoming email via Sendgrid |  This functionaliy can move to message queues
                _emailServices.SendMail(user).Wait();

                return user;
            }
            else
            {
                throw new System.Exception("UserName is already exits");
            }
        }

        public User GetUser(string userName) => _users.Find(user => user.UserName == userName).FirstOrDefault();

        /// <summary>
        /// Authenticat & Generate token
        /// </summary>
        /// <param name="authenticate"></param>
        /// <returns></returns>
        public User Authenticate(AuthenticateDTO authenticate)
        {
            var user = _users.Find(user => user.UserName == authenticate.UserName).Single();

            // check account found and verify password
            var isValid = BCrypt.Net.BCrypt.Verify(authenticate.Password, user.Password);
            if (!isValid)
            {
                // authentication failed
                return null;
            }
            else
            {
                return user;
            }
        }
    }
}
