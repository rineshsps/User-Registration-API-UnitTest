using System;
using System.Collections.Generic;
using System.Text;

namespace Mongo.DTOs
{
    public class AuthenticateDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
