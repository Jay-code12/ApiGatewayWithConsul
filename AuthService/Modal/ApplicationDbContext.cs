using AuthService.Modal.data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Modal
{
    public class ApplicationDbContext : IdentityDbContext<Auth>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Auth> Auths { get; set; }
    }
}
