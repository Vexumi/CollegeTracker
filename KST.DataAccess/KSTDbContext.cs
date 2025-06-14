using KST.DataAccess.Enums;
using KST.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace KST.DataAccess;

public partial class KSTDbContext : DbContext
{
    public KSTDbContext(DbContextOptions<KSTDbContext> options): base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Student> Students { get; set; }
    
    public DbSet<Teacher> Teachers { get; set; }
    
    public DbSet<Administrator> Administrators { get; set; }
    
    public DbSet<Group> Groups { get; set; }
    
    public DbSet<Speciality> Specialities { get; set; }
    
    public DbSet<Project> Projects { get; set; }
    
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    
    public DbSet<ProjectAttachment> ProjectAttachment { get; set; }
    
    public DbSet<AuthorizationHistory> AuthorizationHistories { get; set; }
    
    public DbSet<Message> Messages { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Speciality>().HasData(
            new List<Speciality>()
            {
                new()
                {
                    Id = 1,
                    Title = "Разработка веб-сайтов",
                    Description = "Разработка веб-сайтов на заказ",
                },
                new()
                {
                    Id = 2,
                    Title = "Разработка 1с",
                    Description = "Разработка 1с на заказ",
                },
                new()
                {
                    Id = 3,
                    Title = "Изготовление кровель",
                    Description = "Изготовление кровель на заводе",
                }
            }
        );

        modelBuilder.Entity<Group>().HasData(
            new List<Group>()
            {
                new()
                {
                    Id = 1,
                    LaunchDate = DateTime.UtcNow,
                    StopDate = DateTime.UtcNow.AddYears(4),
                    Number = "107в1",
                    SpecialityId = 1,
                },
                new()
                {
                    Id = 2,
                    LaunchDate = DateTime.UtcNow,
                    StopDate = DateTime.UtcNow.AddYears(4),
                    Number = "107в2",
                    SpecialityId = 1,
                },
                new()
                {
                    Id = 3,
                    LaunchDate = DateTime.UtcNow,
                    StopDate = DateTime.UtcNow.AddYears(5),
                    Number = "107a1",
                    SpecialityId = 2,
                }
                ,
                new()
                {
                    Id = 4,
                    LaunchDate = DateTime.UtcNow,
                    StopDate = DateTime.UtcNow.AddYears(5),
                    Number = "107a2",
                    SpecialityId = 2,
                }
            }
        );

        modelBuilder.Entity<User>().HasData(
            new List<User>()
            {
                new()
                {
                    Id = 1,
                    Email = "student1@gmail.com",
                    PasswordHash = "5Pq0DMdlOMptZkP5uKQcJDhknCjT1++zB7AKR+QA6AGAoDa7lW9TjkIcSa0AsvoE0uDAQU/phufpGPKgcVAOMA==",
                    Fullname = "Иванов Иван Иванович",
                    PhoneNumber = "88005554535",
                    Username = "IvIvIv",
                    Role = UserRoles.Student
                },
                new()
                {
                    Id = 2,
                    Email = "student2@gmail.com",
                    PasswordHash = "5Pq0DMdlOMptZkP5uKQcJDhknCjT1++zB7AKR+QA6AGAoDa7lW9TjkIcSa0AsvoE0uDAQU/phufpGPKgcVAOMA==",
                    Fullname = "Иванов2 Иван2 Иванович2",
                    PhoneNumber = "88005554535",
                    Username = "Iv2Iv2Iv2",
                    Role = UserRoles.Student
                },
                new()
                {
                    Id = 3,
                    Email = "student3@gmail.com",
                    PasswordHash = "5Pq0DMdlOMptZkP5uKQcJDhknCjT1++zB7AKR+QA6AGAoDa7lW9TjkIcSa0AsvoE0uDAQU/phufpGPKgcVAOMA==",
                    Fullname = "Иванов3 Иван3 Иванович3",
                    PhoneNumber = "88005554535",
                    Username = "Iv3Iv3Iv3",
                    Role = UserRoles.Student
                },
                new()
                {
                    Id = 4,
                    Email = "admin@gmail.com",
                    PasswordHash = "5Pq0DMdlOMptZkP5uKQcJDhknCjT1++zB7AKR+QA6AGAoDa7lW9TjkIcSa0AsvoE0uDAQU/phufpGPKgcVAOMA==",
                    Fullname = "Admin Admin Admin",
                    PhoneNumber = "88005554535",
                    Username = "Admin",
                    Role = UserRoles.Admin
                },
                new()
                {
                    Id = 5,
                    Email = "teacher@gmail.com",
                    PasswordHash = "5Pq0DMdlOMptZkP5uKQcJDhknCjT1++zB7AKR+QA6AGAoDa7lW9TjkIcSa0AsvoE0uDAQU/phufpGPKgcVAOMA==",
                    Fullname = "Teacher Teacher Teacher",
                    PhoneNumber = "88005554535",
                    Username = "Teacher",
                    Role = UserRoles.Teacher
                }
            }
        );
        
        modelBuilder.Entity<Teacher>().HasData(
            new List<Teacher>()
            {
                new()
                {
                    Id = 1,
                    UserInfoId = 5
                }
            }
        );
        
        modelBuilder.Entity<Project>().HasData(
            new List<Project>()
            {
                new()
                {
                    Id = 1,
                    Title = "Разработка 1С приложения для Магнита",
                    Description = "Разработка 1С приложения для Магнит",
                    TeacherId = 1,
                    SpecialityId = 2,
                    StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    Deadline = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(6)),
                }
            }
        );
        
        modelBuilder.Entity<Student>().HasData(
            new List<Student>()
            {
                new()
                {
                    Id = 1,
                    GroupId = 1,
                    UserInfoId = 1,
                    ProjectId = 1
                },
                new()
                {
                    Id = 2,
                    GroupId = 1,
                    UserInfoId = 2,
                    ProjectId = 1
                },
                new()
                {
                    Id = 3,
                    GroupId = 2,
                    UserInfoId = 3
                }
            }
        );

        modelBuilder.Entity<ProjectTask>().HasData(
            new List<ProjectTask>()
            {
                new ()
                {
                    Id = 1,
                    Title = "Test 1",
                    Description = "Test Description 1",
                    EstimatedHours = 4,
                    ProjectId = 1,
                    AssignedToId = 1
                },
                new ()
                {
                    Id = 2,
                    Title = "Test 2",
                    Description = "Test Description 2",
                    EstimatedHours = 8,
                    ProjectId = 1,
                    AssignedToId = 1
                },
                new ()
                {
                    Id = 3,
                    Title = "Test 3",
                    Description = "Test Description 3",
                    EstimatedHours = 2,
                    ProjectId = 1,
                    AssignedToId = 1
                },
            });
    }
}