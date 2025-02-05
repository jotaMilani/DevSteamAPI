using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevSteamAPI.Data
{
    public class DevSteamAPIContext : IdentityDbContext
    {
        public DevSteamAPIContext(DbContextOptions<DevSteamAPIContext> options)
            : base(options)
        { }

        //sobrescrever o metodo OnModelCreating 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
