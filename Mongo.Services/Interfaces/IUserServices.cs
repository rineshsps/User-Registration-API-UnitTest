using Mongo.Database.Models;
using Mongo.DTOs;
using System.Collections.Generic;

namespace Mongo.Services.Interfaces
{
    public interface IUserServices
    {
        List<User> GetUsers();
        User GetUser(string id);
        User Create(User book);
        User Update(User book);
        void Delete(string id);
        User Authenticate(AuthenticateDTO authenticate);
    }
}
