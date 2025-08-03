using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Constants;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using WorkTask = BusinessObjects.Models.WorkTask;

namespace BusinessObjects
{
    public partial class TaskManagementContext : DbContext
    {
        public TaskManagementContext(){ }

        public TaskManagementContext(DbContextOptions<TaskManagementContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Board> Boards { get; set; }
        public virtual DbSet<Column> Columns { get; set; }
        public virtual DbSet<WorkTask> Tasks { get; set; }
        public virtual DbSet<Label> Labels { get; set; }
        public virtual DbSet<TaskLabel> TaskLabels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasMaxLength(36);
                entity.Property(x => x.Name).HasColumnName("Name").HasMaxLength(256);
                entity.Property(x => x.Email).HasColumnName("Email").HasMaxLength(256);
                entity.Property(x => x.Role).HasColumnName("Role").HasMaxLength(20);
                entity.Property(x => x.Password).HasColumnName("Password").HasMaxLength(256);
                entity.Property(x => x.Salt).HasColumnName("Salt").HasMaxLength(256);
                entity.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime");
            });

            modelBuilder.Entity<Board>(entity =>
            {
                entity.ToTable("Board");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasMaxLength(36);
                entity.Property(x => x.Title).HasColumnName("Title").HasMaxLength(256);
                entity.Property(x => x.Description).HasColumnName("Description").HasMaxLength(256);
                entity.Property(x => x.OwnerId).HasColumnName("OwnerId").HasMaxLength(36);
                entity.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime");
                entity.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").HasColumnType("datetime"); 
                entity.Property(x => x.IsTemplate).HasColumnName("IsTemplate").HasDefaultValue(false);
            });

            modelBuilder.Entity<BoardMember>(entity =>
            {
                entity.ToTable("BoardMember");
                entity.HasKey(x => new { x.BoardId, x.MemberId});

                entity.Property(x => x.BoardId).HasMaxLength(36);
                entity.Property(x => x.MemberId).HasMaxLength(36);
                entity.Property(x => x.Role).HasColumnName("Role").HasMaxLength(20);

                entity.HasOne(x => x.Board)
                    .WithMany(x => x.BoardMembers)
                    .HasForeignKey(x => x.BoardId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Member)
                    .WithMany(l => l.BoardMembers)
                    .HasForeignKey(tl => tl.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>()
                .HasMany(x => x.Boards)
                .WithMany(x => x.Members)
                .UsingEntity<BoardMember>();

            modelBuilder.Entity<Column>(entity =>
            {
                entity.ToTable("Column");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasMaxLength(36);
                entity.Property(x => x.Title).HasColumnName("Title").HasMaxLength(256);
                entity.Property(x => x.Position).HasColumnName("Position");
                entity.Property(x => x.BoardId).HasColumnName("BoardId").HasMaxLength(36);

                entity.HasOne(x => x.Board).WithMany(x => x.Columns)
                    .HasForeignKey(x => x.BoardId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WorkTask>(entity =>
            {
                entity.ToTable("Task");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasMaxLength(36);
                entity.Property(x => x.Title).HasColumnName("Title").HasMaxLength(256);
                entity.Property(x => x.Description).HasColumnName("Description").HasMaxLength(256);
                entity.Property(x => x.Position).HasColumnName("Position");
                entity.Property(x => x.Priority).HasColumnName("Priority").HasMaxLength(50);
                entity.Property(x => x.DueDate).HasColumnName("DueDate").HasColumnType("datetime");
                entity.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime");
                entity.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").HasColumnType("datetime");
                entity.Property(x => x.ColumnId).HasColumnName("ColumnId").HasMaxLength(36);

                entity.HasOne(x => x.Column).WithMany(x => x.Tasks)
                    .HasForeignKey(x => x.ColumnId)
                    .OnDelete(DeleteBehavior.Cascade);                                    
            });

            modelBuilder.Entity<Label>(entity =>
            {
                entity.ToTable("Label");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasMaxLength(36);
                entity.Property(x => x.Name).HasColumnName("Name").HasMaxLength(256);
                entity.Property(x => x.Color).HasColumnName("Color").HasMaxLength(50);
                entity.Property(x => x.BoardId).HasColumnName("BoardId").HasMaxLength(36);

                entity.HasOne(x => x.Board).WithMany(x => x.Labels)
                    .HasForeignKey(x => x.BoardId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TaskLabel>(entity =>
            {
                entity.ToTable("TaskLabel");
                entity.HasKey(x => new { x.TaskId, x.LabelId });

                entity.Property(x => x.TaskId).HasMaxLength(36);
                entity.Property(x => x.LabelId).HasMaxLength(36);

                entity.HasOne(x => x.Task)
                    .WithMany(t => t.TaskLabels)
                    .HasForeignKey(tl => tl.TaskId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Label)
                    .WithMany(l => l.TaskLabels)
                    .HasForeignKey(tl => tl.LabelId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<WorkTask>()
                .HasMany(x => x.Labels)
                .WithMany(x => x.Tasks)
                .UsingEntity<TaskLabel>();

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = "17d88407-91ce-4b33-8fbb-9639b12a495e",
                Name = "Admin",
                Email = "admin",
                Password = "Vcdy30nqeMZFh2FCVp2F8uktoTqQcJcKU6Bf0oS2o30=", // 12345
                Salt = "8GpL6j9M7maqTG/s928A0w==",
                CreatedAt = new DateTime(2025, 5, 22, 15, 30, 0),
                Role = UserRoles.Admin
            });
        }
    }
}
