﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using TaskManagementApi.Repositories;
using TaskManagementApi.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
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
        public async Task<IActionResult> Register(User user)
        {
            if (user == null)
                return BadRequest("User information is required.");

            if (string.IsNullOrWhiteSpace(user.Username) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                return BadRequest("Username, Email and Password are required.");
            }

            if (!IsValidEmail(user.Email))
                return BadRequest("Invalid email format.");

            if (_userRepository.GetByUsername(user.Username) != null)
                return Conflict("Username is already taken.");

            if (_userRepository.GetByEmail(user.Email) != null)
                return Conflict("Email is already in use.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            _userRepository.Add(user);

            return CreatedAtAction(nameof(GetAllUsers), new { id = user.Id }, user);
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.PasswordHash))
            {
                return BadRequest("Email or Password cannot be blank.");
            }
            var userLogin = _userRepository.GetByEmail(user.Email);
            if (userLogin == null)
            {
                return Unauthorized("Email or password is incorrect.");
            }
            if (!BCrypt.Net.BCrypt.Verify(user.PasswordHash, userLogin.PasswordHash))
            {
                return Unauthorized("Email or password is incorrect.");
            }

            var claims = new[]
            {   new Claim(ClaimTypes.NameIdentifier, userLogin.Id.ToString()),
                new Claim(ClaimTypes.Name, userLogin.Username),
                new Claim(ClaimTypes.Email, userLogin.Email),
                new Claim(ClaimTypes.Role, userLogin.Role),
            };  

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddSeconds(15),
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new
            {
                Username = userLogin.Username,
                Email = userLogin.Email,
                Role=userLogin.Role,
                Token = accessToken
            });
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

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user== null)
            {
                return NotFound(new { message = $"User with Id {id} does not exist." });
            }

            _userRepository.Delete(id);
            return NoContent();
        }

    }
}
