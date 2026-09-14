using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_Management.DbContexts;
using Task_Management.Dtos.Auth;
using Task_Management.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Task_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TaskManagementContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(TaskManagementContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("Signup")]
        public IActionResult Signup(SignupDto dto)
        {
            try
            {
                var emailExists = _context.Users.Any(x => x.Email == dto.Email);

                if (emailExists)
                {
                    return BadRequest("Email already exists");
                }

                var usernameExists = _context.Users.Any(x => x.Username == dto.Username);

                if (usernameExists)
                {
                    return BadRequest("Username already exists");
                }

                User user = new User
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    HashedPassword = dto.Password
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDto dto)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x =>
                    x.Email == dto.Email &&
                    x.HashedPassword == dto.Password);

                if (user == null)
                {
                    return BadRequest("Invalid Email or Password");
                }

                var token = GenerateToken(user);

                return Ok(new
                {
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Username", user.Username),
                new Claim("Email", user.Email)
             
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }


 }