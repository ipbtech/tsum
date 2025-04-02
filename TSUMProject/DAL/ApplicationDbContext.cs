using Microsoft.EntityFrameworkCore;
using TSUMProject.DAL.Entities;

namespace TSUMProject.DAL;

public class ApplicationDbContext : DbContext
{
    public DbSet<FileProject> FileProject { get; set; }
    public DbSet<Project> Project { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }
}

