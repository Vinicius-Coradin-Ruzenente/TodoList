using TodoList.Model;
using TodoList.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace TodoList.Services
{    
    public class UserServices(TodoListDbContext db, IOptions<AppSettings> appSettings) : IUserServices
    {
        private readonly TodoListDbContext _db = db;
        private readonly AppSettings _appSettings = appSettings.Value;

        public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest request)
        {
            var user = await _db.Users.SingleOrDefaultAsync(user => user.Username == request.Username && user.Password == request.Password);
            if (user == null) { return null; }
            var token = await GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public async Task<List<User>> GetUsers(bool? isActive)
        {
            if (isActive == null) { return await _db.Users.ToListAsync(); }
            return await _db.Users.Where(user => user.IsActive == isActive).ToListAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _db.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User?> CreateUser(AddUpdateUser userObj)
        {
            var user = new User()
            {               
                Username = userObj.Username,
                Password = userObj.Password,
                Email = userObj.Email,
                IsActive = userObj.IsActive,
            };
            _db.Users.Add(user);
            var result = await _db.SaveChangesAsync();
            return result >= 0 ? user : null;
        }

        public async Task<User?> UpdateUser(int id, AddUpdateUser userObj)
        {
            var user = await _db.Users.FirstOrDefaultAsync(index => index.Id == id);
            if (user != null)
            {
                user.Username = userObj.Username;
                user.Password = userObj.Password;
                user.Email = userObj.Email;
                user.UpdatedAt = DateTime.UtcNow.ToLocalTime();
                user.IsActive = userObj.IsActive;

                var result = await _db.SaveChangesAsync();
                return result >= 0 ? user : null;
            }
            return null;
            
        }

        public async Task<bool> DeleteUserById(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null )
            {
                _db.Users.Remove(user);
                var result = await _db.SaveChangesAsync();
                return result >= 0;
            }
            return false;
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity([new Claim("id", user.Id.ToString())]),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.CreateToken(tokenDescriptor);
            });
            return tokenHandler.WriteToken(token);
        }
    }
}