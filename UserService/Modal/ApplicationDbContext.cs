using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UserService.Modal.Data;

namespace UserService.Modal
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
