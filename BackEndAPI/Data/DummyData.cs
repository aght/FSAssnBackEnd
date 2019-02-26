using BackEndAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RegistrationAPI.Data;
using RegistrationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndAPI.Data
{
    public class DummyData
    {
        public static async Task Initialize(ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            context.Database.EnsureCreated();

            if (context.Boats != null && context.Boats.Any())
            {
                return;
            }

            //context.AddRange(GetBoats().ToArray());
            //context.SaveChanges();

            const string roleAdmin = "Admin";
            const string roleMember = "Member";
            const string defaultPassword = "P@$$w0rd";

            if (await roleManager.FindByNameAsync(roleAdmin) == null)
            {
                await roleManager.CreateAsync(new Role(roleAdmin, "Administrator", DateTime.Now));
            }

            if (await roleManager.FindByNameAsync(roleMember) == null)
            {
                await roleManager.CreateAsync(new Role(roleMember, "Member", DateTime.Now));
            }

            if (await userManager.FindByNameAsync("a") == null)
            {
                User user = new User()
                {
                    UserName = "a",
                    Email = "a@a.a",
                    FirstName = "Admin",
                    LastName = "Admin",
                    Country = "Canada",
                    MobileNumber = "6045678910"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, defaultPassword);
                    await userManager.AddToRoleAsync(user, roleAdmin);
                }
            }

            if (await userManager.FindByNameAsync("m") == null)
            {
                User user = new User()
                {
                    UserName = "m",
                    Email = "m@m.m",
                    FirstName = "Member",
                    LastName = "Member",
                    Country = "Canada",
                    MobileNumber = "7788881234"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, defaultPassword);
                    await userManager.AddToRoleAsync(user, roleMember);
                }
            }

            if (await userManager.FindByNameAsync("andy") == null)
            {
                User user = new User()
                {
                    UserName = "andy",
                    Email = "andytang43@gmail.com",
                    FirstName = "Andy",
                    LastName = "Tang",
                    Country = "Canada",
                    MobileNumber = "6902341234"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, defaultPassword);
                    await userManager.AddToRoleAsync(user, roleAdmin);
                }
            }

            if (await userManager.FindByEmailAsync("test") == null)
            {
                User user = new User()
                {
                    UserName = "test",
                    Email = "test@test.test",
                    FirstName = "Test",
                    LastName = "Tester",
                    Country = "Canada",
                    MobileNumber = "6902341234"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, defaultPassword);
                    await userManager.AddToRoleAsync(user, roleMember);
                }
            }

            if (await userManager.FindByNameAsync("tommy") == null)
            {
                User user = new User()
                {
                    UserName = "tommy",
                    Email = "c@c.c",
                    FirstName = "Tommy",
                    LastName = "Yeh",
                    Country = "Canada",
                    MobileNumber = "6902341234"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, defaultPassword);
                    await userManager.AddToRoleAsync(user, roleMember);
                }
            }
        }

        private static List<Boat> GetBoats()
        {
            return new List<Boat>()
            {
                new Boat()
                {
                    BoatName = "BERTRAM 35",
                    Picture = "Picture",
                    LengthInFeet = 35,
                    Make = "Bertram",
                    Description = "THE BERTRAM 35 was built on the timeless foundation of the original 31s." +
                    " Each hull is infused with 100% vinyl ester composite and designed with pocketed propellers," +
                    " enormous cockpit space, and equipped with a Seakeeper gyro stabilizer, Subzero refrigerators, " +
                    "Fusion stereo system and a bow thruster — all as standard equipment on each boat. No other boat in" +
                    " this class meets the criteria of the Bertram 35', a boat as luxurious and elegant as it is rugged" +
                    " and durable."
                }
            };
        }
    }
}