using Microsoft.EntityFrameworkCore;
using MvcBach.Models;

namespace MvcBach.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}
        
        
        public DbSet<HeThongPhanPhoi> HeThongPhanPhoi { get; set;}
        public DbSet<DaiLy> DaiLy{ get; set;}
        public DbSet<Student> Student { get; set;}

        

    }
}

