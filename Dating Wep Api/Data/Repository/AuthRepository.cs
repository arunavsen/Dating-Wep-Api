using Dating_Wep_Api.Data.IRepository;
using Dating_Wep_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Dating_Wep_Api.Data.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDBContext _context;

        public AuthRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string username, string password)
        {
            if (!string.IsNullOrEmpty(username))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

                if (user != null)
                {
                    if (VerifyPassword(password, user.PasswordSalt, user.PasswordHash))
                    {
                        return user;
                    }
                }
            }

            return null;
        }

        private bool VerifyPassword(string password, byte[] passwordSalt, byte[] passwordHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }                   
                }
            }

            return true;
        }

        public async Task<User> RegisterUser(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExist(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            if(await _context.Users.AnyAsync(x => x.Username == username))
            {
                return true;
            }
            return false;
        }
    }
}
