using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using TaskManagementApi.Repositories;
using TaskManagementApi.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
namespace TaskManagementApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public UserController(UserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register(string username, string email, string password)
        {
            if (!IsValidEmail(email))
                return BadRequest("Invalid email format.");

            if (string.IsNullOrWhiteSpace(password))
                return BadRequest("Password is required.");

            if (_userRepository.GetByUsername(username) != null)
                return Conflict("Username is already taken.");
            if (_userRepository.GetByEmail(email) != null)
                return Conflict("Email is already in use.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new User
            {
                Username = username,
                Email = email,
                PasswordHash = hashedPassword,
            };

            _userRepository.Add(newUser);
            return CreatedAtAction(nameof(GetAllUsers), new { id = newUser.Id }, newUser);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(string email, string password)
        {

            var user = _userRepository.GetByEmail(email);
            if (user == null) return Unauthorized("Invalid email or password.");
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return Unauthorized("Invalid email or password.");

            else
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Roles),
                 };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: credentials);


                var Accesstoken = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new
                {
                    Username = user.Username,
                    Email = user.Email,
                    Token = Accesstoken
                });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAll();
            return Ok(users);
        }
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

       

    }
}
