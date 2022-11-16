using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SplitExpense.Core.Models;
using SplitExpense.Core.Models.Core;
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

            var userConnections = new List<UserConnection>();
            var newUser = new Database.User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                Password = UtilityService.GetHashPassword(user.Password),
                IsActive = true,
            };

            if(user.ReferralId.HasValue)
            {
                var referral = this.DB.SingleOrDefault<UserInvite>("WHERE ReferralId = @0 AND ExpiresOn > @1 AND ReferralType = @2 AND IsDeleted = @3", user.ReferralId.Value, DateTime.UtcNow, InviteType.App, false);
                if (referral != null)
                {
                    newUser.ReferralId = referral.Id;
                    userConnections.Add(new UserConnection()
                    {
                        UserId = referral.UserId,
                    });

                    userConnections.Add(new UserConnection()
                    {
                        ConnectedUserId = referral.UserId,
                    });
                }                
            }

            try
            {
                this.DB.BeginTransaction();

                var userId = this.DB.Insert(newUser);
                if (userConnections.Any())
                {
                    userConnections[0].ConnectedUserId = userId;
                    userConnections[1].UserId = userId;
                }
                this.DB.BulkInsert(userConnections);

                this.DB.CompleteTransaction();

                return userId;
            }
            catch(Exception ex)
            {
                this.DB.AbortTransaction();
                throw new Exception("Error in creating User");
            }
        }

        public bool DoesUserExists(User user)
        {
            return this.DB.Exists<User>("WHERE (Email = @0) AND IsDeleted = 0", user.Email);
        }

        public User GetUserById(int id)
        {
            return this.DB.SingleOrDefault<User>("WHERE Id = @0 AND IsDeleted = 0", id);
        }

        public IEnumerable<SearchConnectionUserListItem> GetUserConnectionSearchResults(int userId, string query)
        {
            return this.DB.Fetch<SearchConnectionUserListItem>("; EXEC [GetUserConnectionSearchResults] @@UserId = @0, @@Query = @1", userId, query);
        }

        public IEnumerable<UserConnectionListItem> GetUserConnections(int userId)
        {
            return this.DB.Fetch<UserConnectionListItem>("; EXEC [GetUserConnectionListItems] @@UserId = @0", userId);
        }

        public IEnumerable<UserSearchResultItem> GetUserSearchResultItems(int userId, string query) 
        {
            return this.DB.Fetch<UserSearchResultItem>("; EXEC [GetUserSearchResult] @@UserId = @0, @@Query = @1", userId, query);
        }

        public bool AddUserConnection(int userId, UserConnection userConnection)
        {
            if(this.DB.Exists<UserConnection>("WHERE UserId = @0 AND ConnectedUserId = @1 AND IsDeleted = @2", userId, userConnection.ConnectedUserId, false))
            {
                throw new Exception("User is already connected");
            }

            var userConnections = new List<UserConnection>();
            userConnections.Add(new UserConnection()
            {
                UserId = userId,
                ConnectedUserId = userConnection.ConnectedUserId
            });

            userConnections.Add(new UserConnection()
            {
                UserId = userConnection.ConnectedUserId,
                ConnectedUserId = userId
            });

            this.DB.BulkInsert(userConnections);

            return true;
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
