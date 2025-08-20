using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Models;
using PAWScrum.Models.Entities;

namespace PAWScrum.Data.Context
{
    public partial class PAWScrumDbContext : DbContext
    {
        public PAWScrumDbContext() { }
        public PAWScrumDbContext(DbContextOptions<PAWScrumDbContext> options) : base(options) { }

        public virtual DbSet<ActivityLog> ActivityLog { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<ProductBacklogItem> ProductBacklogItems { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectMember> ProjectMembers { get; set; }  
        public virtual DbSet<Sprints> Sprints { get; set; }
        public virtual DbSet<SprintBacklogItem> SprintBacklogItems { get; set; }
        public virtual DbSet<UserTask> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityLog>(entity =>
            {
                entity.ToTable("ActivityLog");
                entity.HasKey(e => e.ActivityId);

                entity.Property(e => e.ActivityId).HasColumnName("ActivityId");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.ProjectId).HasColumnName("ProjectId");
                entity.Property(e => e.Action).HasColumnName("Action").IsRequired();
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");

                entity.HasOne(e => e.Project)
                      .WithMany(p => p.ActivityLogs)
                      .HasForeignKey(e => e.ProjectId)
                      .HasConstraintName("FK_ActivityLog_Projects_ProjectId");

                entity.HasOne(e => e.User)
                      .WithMany(u => u.ActivityLogs)
                      .HasForeignKey(e => e.UserId)
                      .HasConstraintName("FK_ActivityLog_Users_UserId");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comments");

                entity.HasKey(e => e.CommentId);
                entity.HasOne(e => e.Task)
                    .WithMany(t => t.Comments)
                    .HasForeignKey(e => e.TaskId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.SprintItem)
                    .WithMany(s => s.Comments)
                    .HasForeignKey(e => e.SprintItemId)          
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProductBacklogItem>(entity =>
            {
                entity.HasKey(e => e.ItemId).HasName("PK__ProductB__727E838B0C383396");

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("(getdate())")
                      .HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.Title).HasMaxLength(200);

                entity.HasOne(d => d.Project).WithMany(p => p.ProductBacklogItems)
                      .HasForeignKey(d => d.ProjectId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__ProductBa__Proje__48CFD27E");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.ProjectId).HasName("PK__Projects__761ABEF0AFF11888");

                entity.Property(e => e.IsArchived).HasDefaultValue(false);
                entity.Property(e => e.ProjectName).HasMaxLength(200);
                entity.Property(e => e.RepositoryUrl).HasMaxLength(300);
                entity.Property(e => e.Status).HasMaxLength(50).HasDefaultValue("Active");
                entity.Property(e => e.Visibility).HasMaxLength(20).HasDefaultValue("Private");

                entity.HasOne(d => d.Owner).WithMany(p => p.Projects)
                      .HasForeignKey(d => d.OwnerId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__Projects__OwnerI__3E52440B");
            });

            modelBuilder.Entity<ProjectMember>(entity =>
            {
                entity.HasKey(e => new { e.ProjectId, e.UserId }).HasName("PK__ProjectM__A76232346906DF47");

                entity.HasOne(d => d.Project).WithMany(p => p.ProjectMembers)
                      .HasForeignKey(d => d.ProjectId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__ProjectMe__Proje__412EB0B6");

                entity.HasOne(d => d.User).WithMany(p => p.ProjectMembers)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__ProjectMe__UserI__4222D4EF");
            });

            modelBuilder.Entity<Sprints>(entity =>
            {
                entity.HasKey(e => e.SprintId).HasName("PK__Sprints__29F16AC0B58C4F9D");
                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Project).WithMany(p => p.Sprints)
                      .HasForeignKey(d => d.ProjectId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__Sprints__Project__44FF419A");
            });

            modelBuilder.Entity<SprintBacklogItem>(entity =>
            {
                entity.HasKey(e => e.SprintItemId).HasName("PK__SprintBa__B07E5F760CDB7026");

                entity.Property(e => e.CompletedHours).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.EstimationHours).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.SprintBacklogItems)
                      .HasForeignKey(d => d.AssignedTo)
                      .HasConstraintName("FK__SprintBac__Assig__4D94879B");

                entity.HasOne(d => d.ProductBacklogItem).WithMany(p => p.SprintBacklogItems)
                      .HasForeignKey(d => d.ProductBacklogItemId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__SprintBac__Produ__4CA06362");

                entity.HasOne(d => d.Sprint).WithMany(p => p.SprintBacklogItems)
                      .HasForeignKey(d => d.SprintId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__SprintBac__Sprin__4BAC3F29");
            });

            modelBuilder.Entity<UserTask>(entity =>
            {
                entity.HasKey(e => e.TaskId);
                entity.ToTable("Tasks", "dbo");

                entity.Property(e => e.EstimationHours).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.CompletedHours).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.Title).HasMaxLength(200);

                entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.Tasks)
                      .HasForeignKey(d => d.AssignedTo)
                      .HasConstraintName("FK__Tasks__AssignedT__5165187F");

                entity.HasOne(d => d.SprintItem).WithMany(p => p.Tasks)
                      .HasForeignKey(d => d.SprintItemId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__Tasks__SprintIte__5070F446");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C4A27407A");

                entity.HasIndex(e => e.Username, "UQ__Users__536C85E48AE1EE81").IsUnique();
                entity.HasIndex(e => e.Email, "UQ__Users__A9D10534A01BED78").IsUnique();

                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.PasswordHash).HasMaxLength(255);
                entity.Property(e => e.Role).HasMaxLength(50);
                entity.Property(e => e.Username).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}