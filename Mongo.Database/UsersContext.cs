using Microsoft.Extensions.Options;
using Mongo.Database.Interfaces;
using Mongo.Database.Models;
using Mongo.Settings;
using MongoDB.Driver;

namespace Mongo.Database
{
    public class UsersContext : IUsersContext
    {
        private readonly IMongoCollection<User> _users;

        public UsersContext(IOptions<ApplicationSettings> usersDbConfig)
        {
            var client = new MongoClient(usersDbConfig.Value.AppSettings.ConnectionString);
            var database = client.GetDatabase(usersDbConfig.Value.AppSettings.DatabaseName);
            _users = database.GetCollection<User>(usersDbConfig.Value.AppSettings.CollectionName);
        }

        public IMongoCollection<User> GetUserssCollection() => _users;
    }
}
