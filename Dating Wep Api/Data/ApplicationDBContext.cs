using Dating_Wep_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Dating_Wep_Api.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {

        }

        public DbSet<Value> Values { get; set; }
    }
}
