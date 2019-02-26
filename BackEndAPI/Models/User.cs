using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace RegistrationAPI.Models
{
    public class User : IdentityUser
    {
        public User() : base() { }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string MobileNumber { get; set; }
    }
}
