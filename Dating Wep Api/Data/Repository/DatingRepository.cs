using Dating_Wep_Api.Data.IRepository;
using Dating_Wep_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Dating_Wep_Api.Data.Repository
{
    public class DatingRepository : IDatingRepository
    {
        private readonly ApplicationDBContext _db;
        public DatingRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public void Add<T>(T entity) where T : class
        {
            _db.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _db.Remove(entity);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            return await _db.Photos.FirstAsync(p => p.Id == id);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _db.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _db.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
