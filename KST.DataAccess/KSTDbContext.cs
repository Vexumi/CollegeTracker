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
    
    public DbSet<Subject> Subjects { get; set; }
    
    public DbSet<Speciality> Specialities { get; set; }
    
    public DbSet<Project> Projects { get; set; }
    
    public DbSet<AuthorizationHistory> AuthorizationHistories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subject>().HasData(
            new List<Subject>()
            {
                new Subject()
                {
                    Id = 1,
                    Title = "Математика",
                    Description = "Самая точная наука"
                },
                new Subject()
                {
                    Id = 2,
                    Title = "Физика",
                    Description = "Наука о природе и ее законах"
                },
                new Subject()
                {
                    Id = 3,
                    Title = "Химия",
                    Description = "Наука о веществах и их взаимодействии"
                },
                new Subject()
                {
                    Id = 4,
                    Title = "Биология",
                    Description = "Наука о живых организмах"
                },
                new Subject()
                {
                    Id = 5,
                    Title = "Информатика",
                    Description = "Наука об информации и вычислительной технике"
                },
                new Subject()
                {
                    Id = 6,
                    Title = "Литература",
                    Description = "Изучение художественных произведений и словесности"
                },
                new Subject()
                {
                    Id = 7,
                    Title = "История",
                    Description = "Изучение прошлого человечества"
                },
                new Subject()
                {
                    Id = 8,
                    Title = "География",
                    Description = "Наука о Земле и ее поверхности"
                },
                new Subject()
                {
                    Id = 9,
                    Title = "Английский язык",
                    Description = "Изучение иностранного языка"
                },
                new Subject()
                {
                    Id = 10,
                    Title = "Физкультура",
                    Description = "Физическое развитие и спортивные навыки"
                },
                new Subject()
                {
                    Id = 11,
                    Title = "Музыка",
                    Description = "Изучение музыки и музыкальной грамотности"
                },
                new Subject()
                {
                    Id = 12,
                    Title = "ОБЖ",
                    Description = "Основы безопасности жизнедеятельности"
                }
            }
        );

        modelBuilder.Entity<Speciality>().HasData(
            new List<Speciality>()
            {
                new Speciality()
                {
                    Id = 1,
                    Title = "Разработка веб-сайтов",
                    Description = "Разработка веб-сайтов на заказ",
                },
                new Speciality()
                {
                    Id = 2,
                    Title = "Разработка 1с",
                    Description = "Разработка 1с на заказ",
                },
                new Speciality()
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
                new Group()
                {
                    Id = 1,
                    LaunchDate = DateTime.UtcNow,
                    StopDate = DateTime.UtcNow.AddYears(4),
                    Number = "107в1",
                    SpecialityId = 1,
                },
                new Group()
                {
                    Id = 2,
                    LaunchDate = DateTime.UtcNow,
                    StopDate = DateTime.UtcNow.AddYears(4),
                    Number = "107в2",
                    SpecialityId = 1,
                },
                new Group()
                {
                    Id = 3,
                    LaunchDate = DateTime.UtcNow,
                    StopDate = DateTime.UtcNow.AddYears(5),
                    Number = "107a1",
                    SpecialityId = 2,
                }
                ,
                new Group()
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
                new User()
                {
                    Id = 1,
                    Email = "student1@gmail.com",
                    PasswordHash = "123456",
                    Fullname = "Иванов Иван Иванович",
                    PhoneNumber = "88005554535",
                    Username = "IvIvIv",
                    Role = UserRoles.Student
                },
                new User()
                {
                    Id = 2,
                    Email = "student2@gmail.com",
                    PasswordHash = "123456",
                    Fullname = "Иванов2 Иван2 Иванович2",
                    PhoneNumber = "88005554535",
                    Username = "Iv2Iv2Iv2",
                    Role = UserRoles.Student
                },
                new User()
                {
                    Id = 3,
                    Email = "student3@gmail.com",
                    PasswordHash = "123456",
                    Fullname = "Иванов3 Иван3 Иванович3",
                    PhoneNumber = "88005554535",
                    Username = "Iv3Iv3Iv3",
                    Role = UserRoles.Student
                }
            }
        );

        modelBuilder.Entity<Student>().HasData(
            new List<Student>()
            {
                new Student()
                {
                    Id = 1,
                    GroupId = 1,
                    UserInfoId = 1
                },
                new Student()
                {
                    Id = 2,
                    GroupId = 1,
                    UserInfoId = 2
                },
                new Student()
                {
                    Id = 3,
                    GroupId = 2,
                    UserInfoId = 3
                }
            }
        );
    }
}