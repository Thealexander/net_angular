using ControlAcceso.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ControlAcceso.Core.Persistence
{
    public class SeguridadContexto : IdentityDbContext<Usuario>
    {
        public SeguridadContexto(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

       // public DbSet <MyEntidad> MyEntidad { get; set; }
    }
}
