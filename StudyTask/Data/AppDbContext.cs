using Microsoft.EntityFrameworkCore;
using StudyTask.Models;

namespace StudyTask.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Curator> Curators => Set<Curator>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Administrator> Administrators => Set<Administrator>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
        public DbSet<TaskCompletion> TaskCompletions => Set<TaskCompletion>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                const string connectionString =
                    "Server=(localdb)\\ServerCool;Database=StudyTaskDb;Trusted_Connection=True;" +
                    "TrustServerCertificate=True;MultipleActiveResultSets=True";

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrator>().HasData(new Administrator
            {
                Id = 1,
                Login = "admin",
                PasswordHash = "A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3", // admin/123
                FullName = "Администратор",
                RoleId = 3,                                   // Administrator
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
            modelBuilder.Entity<Role>(e =>
            {
                e.ToTable("Roles");
                e.HasKey(r => r.Id);
                e.Property(r => r.Name).IsRequired().HasMaxLength(50);
                e.HasIndex(r => r.Name).IsUnique();

                e.HasData(
                    new Role { Id = 1, Name = nameof(Curator) },
                    new Role { Id = 2, Name = nameof(Student) },
                    new Role { Id = 3, Name = nameof(Administrator) });
            });

            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Curator>(nameof(Curator))
                .HasValue<Student>(nameof(Student))
                .HasValue<Administrator>(nameof(Administrator));

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users");
                e.HasKey(u => u.Id);

                e.Property(u => u.Login).IsRequired().HasMaxLength(50);
                e.HasIndex(u => u.Login).IsUnique();
                e.Property(u => u.PasswordHash).IsRequired().HasMaxLength(200);
                e.Property(u => u.FullName).IsRequired().HasMaxLength(150);

                e.HasOne(u => u.Role)
                    .WithMany()
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Group>(e =>
            {
                e.ToTable("Groups");
                e.HasKey(g => g.Id);
                e.Property(g => g.Name).IsRequired().HasMaxLength(50);
                e.Property(g => g.Specialty).HasMaxLength(100);

                e.HasOne(g => g.Curator)
                    .WithMany(c => c.Groups)
                    .HasForeignKey(g => g.CuratorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TaskEntity>(e =>
            {
                e.ToTable("Tasks");
                e.HasKey(t => t.Id);
                e.Property(t => t.Title).IsRequired().HasMaxLength(200);
                e.Property(t => t.Description).HasMaxLength(2000);

                e.HasOne(t => t.Author)
                    .WithMany()
                    .HasForeignKey(t => t.CuratorId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(t => t.TargetGroup)
                    .WithMany()
                    .HasForeignKey(t => t.GroupId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(t => t.TargetStudent)
                    .WithMany()
                    .HasForeignKey(t => t.TargetStudentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TaskCompletion>(e =>
            {
                e.ToTable("TaskCompletions");
                e.HasKey(c => c.Id);

                e.Property(c => c.Status).IsRequired();

                e.Property(c => c.StudentComment).HasMaxLength(1000);
                e.Property(c => c.CuratorComment).HasMaxLength(1000);
                e.Property(c => c.FileLink).HasMaxLength(500);

                e.HasOne(c => c.Task)
                    .WithMany(t => t.Completions)
                    .HasForeignKey(c => c.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(c => c.Student)
                    .WithMany(s => s.Completions)
                    .HasForeignKey(c => c.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(c => new { c.TaskId, c.StudentId }).IsUnique();
            });
        }
    }
}