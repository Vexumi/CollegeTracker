using Microsoft.EntityFrameworkCore;
using CollegeTracker.DataAccess.Models;

namespace CollegeTracker.DataAccess;

public partial class CollegeTrackerDbContext : DbContext
{
    public CollegeTrackerDbContext(DbContextOptions<CollegeTrackerDbContext> options): base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Student> Students { get; set; }
    
    public DbSet<Teacher> Teachers { get; set; }
    
    public DbSet<Administrator> Administrators { get; set; }
    
    public DbSet<Group> Groups { get; set; }
    
    public DbSet<Subject> Subjects { get; set; }
    
    public DbSet<Speciality> Specialities { get; set; }
    
    public DbSet<Project> Projects { get; set; }
    
    public DbSet<AuthorizationHistory> AuthorizationHistories { get; set; }
}