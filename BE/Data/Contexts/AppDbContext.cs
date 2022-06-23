using System.Reflection;
using BE.Data.Enums;
using BE.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        #region Constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        #endregion

        #region Property
        DbSet<Tasks> tasks { get; set; }

        #endregion

        #region Method
        // Use Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tasks>(e =>
            {
                e.ToTable("Tasks");
                e.HasKey(e => e.idTask);
                e.Property(e => e.idParent).HasDefaultValue(0);
                e.Property(e => e.taskName).IsRequired().HasColumnType("varchar(50)");
                e.Property(e => e.description).HasColumnType("text");
                e.Property(e => e.isDeleted).HasDefaultValue(false);
                e.HasQueryFilter(e => !e.isDeleted);
                e.Property(e => e.status).IsRequired().HasDefaultValue(Status.open);
                e.Property(e => e.tag).IsRequired().HasDefaultValue(Tags.task);
                e.Property(e => e.assignee).IsRequired();
                e.Property(e => e.startTaskDate).HasColumnType("timestamp");
                e.Property(e => e.endTaskDate).HasColumnType("timestamp");
                e.Property(e => e.createTaskDate).HasDefaultValue(DateTime.Now)
                                            .HasColumnType("timestamp");
                e.Property(e => e.createUser).IsRequired();
                e.Property(e => e.idProject).IsRequired();
            });
        }
        #endregion
    }
}