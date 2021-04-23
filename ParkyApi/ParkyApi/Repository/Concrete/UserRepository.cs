using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyApi.Data;
using ParkyApi.Models;
using ParkyApi.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParkyApi.Repository.Concrete
{
    public class UserRepository : IUserRepository
    {
        private readonly ParkyApiDbContext _parkyApiDbContext;
        private readonly AppSettings _appSettings;
        public UserRepository(ParkyApiDbContext parkyApiDbContext,
                             IOptions<AppSettings> appSettings)
        {
            _parkyApiDbContext = parkyApiDbContext;
            _appSettings = appSettings.Value;
        }
        public User Authenticate(string username, string password)
        {
            var user = _parkyApiDbContext.Users.SingleOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
      
             var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
          
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
                                (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                
            };
         
            var token = tokenHandler.CreateToken(tokenDescriptor);

            user.Token = tokenHandler.WriteToken(token);

            return user;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _parkyApiDbContext.Users.SingleOrDefault(x => x.Username == username);

            // return null if user not found
            if (user == null)
                return true;

            return false;
        }

        public User Register(string username, string password)
        {
            User userObj = new User()
            {
                Username = username,
                Password = password,
                Role = "Admin"
            };

            _parkyApiDbContext.Users.Add(userObj);
            _parkyApiDbContext.SaveChanges();
            userObj.Password = "";
            return userObj;
        }
    }
}
