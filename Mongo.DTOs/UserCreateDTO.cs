using System;
using System.ComponentModel.DataAnnotations;

namespace Mongo.DTOs
{
    public class UserCreateDTO 
    {
        [Required]
        [MinLength(3)]
        public string FullName { get; set; }
        [EmailAddress]
        [Required]
        [MinLength(3)]
        public string UserName { get; set; }//Email
        [Required]
        [MinLength(3)]
        public string Password { get; set; }
    }
}
