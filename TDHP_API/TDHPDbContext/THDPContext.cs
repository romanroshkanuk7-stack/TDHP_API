using TDHP_API.TDHPDbContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TDHP_API.TDHPDbContext
{
    public class THDPContext : IdentityDbContext<UserEntity, RoleEntity, string>
    {
        public THDPContext(DbContextOptions<THDPContext> db)
            : base(db)
        {
        }

        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<CourseEntity> Courses { get; set; }
        public DbSet<WorkshopEntity> Workshops { get; set; }
        public DbSet<CourseScheduleEntity> CourseSchedules { get; set; }
        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<AddressEntity> Addresses { get; set; }
        public DbSet<ProgramEntity> Programs { get; set; }
        public DbSet<PerformanceCategoryEntity> PerformanceCategories { get; set; }
        public DbSet<PlayEntity> Plays { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customer -> Course (many-to-many relationship)
            builder.Entity<CustomerEntity>()
                .HasMany(c => c.Courses)
                .WithMany(c => c.Customers)
                .UsingEntity(j => j.ToTable("CustomerCourses"));

            // Customer -> Workshop (many customers per workshop)
            builder.Entity<CustomerEntity>()
                .HasOne(c => c.Workshop)
                .WithMany(w => w.Customers)
                .HasForeignKey(c => c.WorkshopId)
                .OnDelete(DeleteBehavior.SetNull);

            // Customer -> Address (one-to-one)
            builder.Entity<CustomerEntity>()
                .HasOne(c => c.Address)
                .WithOne(a => a.Customer)
                .HasForeignKey<AddressEntity>(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // CourseSchedule -> Group
            builder.Entity<CourseScheduleEntity>()
                .HasOne(cs => cs.Group)
                .WithMany(g => g.Schedules)
                .HasForeignKey(cs => cs.GroupId)
                .OnDelete(DeleteBehavior.SetNull);

            // CourseSchedule -> Course
            builder.Entity<CourseScheduleEntity>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.Schedules)
                .HasForeignKey(cs => cs.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // PerformanceCategory -> Play
            builder.Entity<PlayEntity>()
                .HasOne(p => p.PerformanceCategory)
                .WithMany(pc => pc.Plays)
                .HasForeignKey(p => p.PerformanceCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
