using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RegistrationAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BackEndAPI.Models;

namespace RegistrationAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<Boat> Boats { get; set; }
    }
}
