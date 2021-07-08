using Mongo.Database.Models;
using Mongo.DTOs;
using System.Collections.Generic;

namespace Mongo.Services.Interfaces
{
    public interface IUserServices
    {
        User GetUser(string id);
        User Create(User book);
        User Authenticate(AuthenticateDTO authenticate);
    }
}
