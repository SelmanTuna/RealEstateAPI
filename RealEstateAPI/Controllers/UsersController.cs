﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RealEstateAPI.Data;
using RealEstateAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealEstateAPI.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        ApiDbContext _dbContext = new ApiDbContext();

        private IConfiguration _config;

        public UsersController(IConfiguration config)
        {
            _config = config;
        }
        
        [HttpPost("[action]")]
        public IActionResult Register([FromBody] User user)
        {
            var userExists =_dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (userExists!=null)
            {
                return BadRequest("User with same email already exists");
            }
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("[action]")]
        public IActionResult Login([FromBody] User user) 
        {
            var currentUser = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
            if(currentUser == null)
            {
                return NotFound();
            }
            var securtiyKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials=new SigningCredentials(securtiyKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(jwt);
            
        }
    }
}
