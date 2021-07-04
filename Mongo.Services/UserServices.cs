using Mongo.Database.Interfaces;
using Mongo.Database.Models;
using Mongo.DTOs;
using Mongo.Services.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Mongo.Services
{
    public class UserServices : IUserServices
    {
        private readonly IMongoCollection<User> _users;

        public UserServices(IUsersContext db)
        {
            _users = db.GetUserssCollection();
        }

        public User Create(User user)
        {
            var validUser = GetUser(user.UserName);
            if (validUser == null)
            {
                _users.InsertOne(user);
                return user;
            }
            else
            {
                throw new System.Exception("UserName is already exits");
            }
        }

        public void Delete(string id)
        {
            _users.DeleteOne(user => user.Id == id);
        }

        public User GetUser(string userName) => _users.Find(user => user.UserName == userName).FirstOrDefault();

        public List<User> GetUsers()
        {
            return _users.Find(user => true).ToList();
        }

        public User Update(User user)
        {
            GetUser(user.Id);
            _users.ReplaceOne(b => b.Id == user.Id, user);
            return user;
        }

        public User Authenticate(AuthenticateDTO authenticate)
        {
            return _users.Find(user => user.UserName == authenticate.UserName && user.Password == authenticate.Password).FirstOrDefault();
        }
    }
}
