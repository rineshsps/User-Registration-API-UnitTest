using Mongo.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mongo.Services.Interfaces
{
    public interface IJWTServices
    {
        string GenerateToken(UserDisplayDTO user);
     }
}
