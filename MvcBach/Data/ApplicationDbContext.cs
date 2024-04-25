using Microsoft.EntityFrameworkCore;
using MvcBach.Models;

namespace MvcBach.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}
        
        public DbSet<Student> Student { get; set;}
        

    }
}

