using Mongo.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Services.Interfaces
{
    public interface IEmailServices
    {
        Task SendMail(User user);
    }
}
