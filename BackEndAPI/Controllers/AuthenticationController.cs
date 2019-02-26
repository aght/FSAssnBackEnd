using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BackEndAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RegistrationAPI.Data;
using RegistrationAPI.Models;

namespace BackEndAPI.Controllers
{
    [EnableCors("AccessPolicy")]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public AuthenticationController(IConfiguration configuration,
            UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost, Route("register")]
        public async Task<ActionResult<RegisterModel>> Register([FromBody] RegisterModel user)
        {
            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found",
                    username = user.UserName
                });
            }

            var validator = new PasswordValidator<User>();
            if (!(await validator.ValidateAsync(_userManager, null, user.Password)).Succeeded)
            {
                return BadRequest("Password must have at least 1 uppercase letter," +
                    " 1 lowercase letter," +
                    " 1 number," +
                    " 1 special character," +
                    " and be at least 8 characters long");
            }

            if (await _userManager.FindByNameAsync(user.UserName) != null)
            {
                return BadRequest(new
                {
                    message = "Username already exists",
                    conflict = user.UserName
                });
            }

            if (await _userManager.FindByEmailAsync(user.Email) != null)
            {
                return BadRequest(new
                {
                    message = "Email is being used by another user",
                    conflict = user.Email
                });
            }

            User newUser = buildRegisteringUser(user);

            var result = await _userManager.CreateAsync(newUser);
            if (result.Succeeded)
            {
                await _userManager.AddPasswordAsync(newUser, user.Password);
                await _userManager.AddToRoleAsync(newUser, "Member");
            }
            else
            {
                return BadRequest("Error registering user");
            }

            return Ok(new
            {
                username = user.UserName,
                password = "{password}",
                email = user.Email,
                firstname = user.FirstName,
                lastname = user.LastName,
                country = user.Country,
                mobilenumber = user.MobileNumber
            });
        }

        [HttpPost, Route("login")]
        public async Task<ActionResult<LoginModel>> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString())
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");
                claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var signinKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

                int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Site"],
                    audience: _configuration["Jwt:Site"],
                    claims: claimsIdentity.Claims,
                    expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                    signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(
                  new
                  {
                      token = new JwtSecurityTokenHandler().WriteToken(token),
                      expiration = token.ValidTo,
                      role = await _userManager.GetRolesAsync(user)
                  });
            }

            return Unauthorized();
        }

        public async Task<ActionResult<string>> IsValidUsername([FromBody] string username)
        {
            return Ok(new { valid = !(await _userManager.FindByNameAsync(username) == null) });
        }

        public async Task<ActionResult<string>> IsValidEmail([FromBody] string email)
        {
            return Ok(new { valid = !(await _userManager.FindByEmailAsync(email) == null) });
        }

        private User buildRegisteringUser(RegisterModel model)
        {
            return new User()
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Country = model.Country,
                MobileNumber = model.MobileNumber
            };
        }
    }
}