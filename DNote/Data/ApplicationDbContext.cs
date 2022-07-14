using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DNote.Models;

namespace DNote.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DNote.Models.Category>? Category { get; set; }
        public DbSet<DNote.Models.Note>? Note { get; set; }
    }
}