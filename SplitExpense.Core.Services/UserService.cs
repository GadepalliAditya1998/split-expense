﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SplitExpense.Core.Models;
using SplitExpense.Core.Models.ViewModels;
using SplitExpense.Core.Services.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Database = SplitExpense.Core.Models.Core.Database;

namespace SplitExpense.Core.Services
{
    public class UserService
    {
        private readonly DatabaseContext DB;
        private readonly IConfiguration configuration;

        public UserService(DatabaseContext db, IConfiguration configuration)
        {
            this.DB = db;
            this.configuration = configuration;
        }

        public int CreateUser(AddUser user)
        {
            if (this.DB.Exists<User>("WHERE Email = @0 AND IsDeleted = @1", user.Email, false))
            {
                throw new Exception("User already exists");
            }

            var newUser = new Database.User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                ReferralId = user.ReferralId,
                Password = UtilityService.GetHashPassword(user.Password),
            };

            return this.DB.Insert(newUser);
        }

        public bool DoesUserExists(User user)
        {
            return this.DB.Exists<User>("WHERE (Email = @0) AND IsDeleted = 0", user.Email);
        }

        public User GetUserById(int id)
        {
            return this.DB.SingleOrDefault<User>("WHERE Id = @0 AND IsDeleted = 0", id);
        }


        public string LoginUser(LoginUser loginUser)
        {
            if (string.IsNullOrEmpty(loginUser.Email) || string.IsNullOrEmpty(loginUser.Password))
            {
                throw new Exception("Username or password shouldn't be empty");
            }

            var user = this.DB.SingleOrDefault<Database.User>("WHERE Email = @0 AND IsDeleted = @1", loginUser.Email, false);
            if (user == null)
            {
                throw new Exception("User doesn't exists");
            }

            if(!BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
            {
                throw new Exception("Invalid credentials");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
            };

            var token = new JwtSecurityToken
            (
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}
