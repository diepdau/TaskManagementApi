using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi;
using TaskManagementApi.Repositories;
using BCrypt.Net;
namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet]
        public IActionResult GetAllUsers() => Ok(_userRepository.GetAll());

        [HttpPost("register")]

        public IActionResult Register(string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("User name is required.");

            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required.");

            if (string.IsNullOrWhiteSpace(password))
                return BadRequest("Password is required.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new Models.User
            {
                Username = username,
                Email = email,
                PasswordHash = hashedPassword,
            };

            _userRepository.Add(newUser);
            return CreatedAtAction(nameof(GetAllUsers), newUser);
        }

    }
}
