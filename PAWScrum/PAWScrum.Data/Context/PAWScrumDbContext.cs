using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PAWScrum.Models;
using PAWScrum.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PAWScrum.Data.Context
{
    public partial class PAWScrumDbContext : DbContext
    {
        public PAWScrumDbContext() { }

        public PAWScrumDbContext(DbContextOptions<PAWScrumDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<ProductBacklogItem> ProductBacklogItems { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectMember> ProjectMembers { get; set; }
        public virtual DbSet<Sprint> Sprints { get; set; }
        public virtual DbSet<SprintBacklogItem> SprintBacklogItems { get; set; }
        public virtual DbSet<UserTask> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Mantener la conexión de QA como principal (es la base de datos actual)
                optionsBuilder.UseSqlServer(
                    "Data Source=SQL1004.site4now.net;" +
                    "Initial Catalog=db_ab9ba9_pawscrum;" +
                    "User Id=db_ab9ba9_pawscrum_admin;" +
                    "Password=Cafecafe04;" +
                    "TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de ActivityLog (combinando ambas)
            modelBuilder.Entity<ActivityLog>(entity =>
            {
                entity.ToTable("ActivityLog");
                entity.HasKey(e => e.LogId); // Usar LogId de QA

                entity.Property(e => e.LogId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("LogId");

                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.ProjectId).HasColumnName("ProjectId");
                entity.Property(e => e.Action).HasColumnName("Action").IsRequired();
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");

                // Relaciones mejoradas (combinación)
                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ActivityLogs)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_ActivityLog_Projects_ProjectId")
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(d => d.User)
                    .WithMany(u => u.ActivityLogs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ActivityLog_Users_UserId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de Comment (combinando ambas)
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFCAEA75DC6D");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                // Relaciones mejoradas
                entity.HasOne(d => d.SprintItem)
                    .WithMany(s => s.Comments)
                    .HasForeignKey(d => d.SprintItemId)
                    .HasConstraintName("FK__Comments__Sprint__5629CD9C")
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(d => d.Task)
                    .WithMany(t => t.Comments)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK__Comments__TaskId__571DF1D5")
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Comments__UserId__5535A963")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de ProductBacklogItem (combinando ambas)
            modelBuilder.Entity<ProductBacklogItem>(entity =>
            {
                entity.ToTable("ProductBacklogItems");
                entity.HasKey(e => e.ItemId).HasName("PK_ProductBacklogItems");

                entity.Property(e => e.ItemId)
                    .HasColumnName("ItemId")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.Title).HasMaxLength(200);

                // Relación con Project
                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProductBacklogItems)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_ProductBacklogItems_Projects")
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación con SprintBacklogItems
                entity.HasMany(d => d.SprintBacklogItems)
                    .WithOne(s => s.ProductBacklogItem)
                    .HasForeignKey(s => s.ProductBacklogItemId)
                    .HasConstraintName("FK_SprintBacklogItems_ProductBacklogItems")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de Project (combinando ambas)
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.ProjectId).HasName("PK__Projects__761ABEF0AFF11888");

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ProjectKey)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.Visibility)
                    .HasMaxLength(20)
                    .HasDefaultValue("Private");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Active");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date");

                entity.Property(e => e.SprintDuration)
                    .HasColumnType("int");

                entity.Property(e => e.RepositoryUrl)
                    .HasMaxLength(300);

                entity.Property(e => e.IsArchived)
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE");

                entity.HasIndex(e => e.ProjectKey)
                    .IsUnique()
                    .HasDatabaseName("IX_Projects_ProjectKey");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Projects__OwnerI__3E52440B");
            });

            // Configuración de ProjectMember (combinando ambas)
            modelBuilder.Entity<ProjectMember>(entity =>
            {
                entity.ToTable("ProjectMembers");
                entity.HasKey(e => new { e.ProjectId, e.UserId })
                    .HasName("PK_ProjectMembers");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectId");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.Role).HasMaxLength(50);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectMembers)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_ProjectMembers_Projects")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProjectMembers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ProjectMembers_Users")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.UserId)
                    .HasDatabaseName("IX_ProjectMembers_UserId");
            });

            // Configuración de Sprint (combinando ambas)
            modelBuilder.Entity<Sprint>(entity =>
            {
                entity.ToTable("Sprints");
                entity.HasKey(e => e.SprintId).HasName("PK_Sprints");

                entity.Property(e => e.SprintId)
                    .HasColumnName("SprintId")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ProjectId)
                    .HasColumnName("ProjectId")
                    .IsRequired();

                entity.Property(e => e.Name)
                    .HasColumnName("Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate)
                    .HasColumnName("StartDate")
                    .HasColumnType("DATE");

                entity.Property(e => e.EndDate)
                    .HasColumnName("EndDate")
                    .HasColumnType("DATE");

                entity.Property(e => e.Goal)
                    .HasColumnName("Goal")
                    .HasColumnType("NVARCHAR(MAX)");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Sprints)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_Sprints_Projects")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de SprintBacklogItem (combinando ambas)
            modelBuilder.Entity<SprintBacklogItem>(entity =>
            {
                entity.HasKey(e => e.SprintItemId).HasName("PK__SprintBa__B07E5F760CDB7026");

                entity.Property(e => e.CompletedHours).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.EstimationHours).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.AssignedToNavigation)
                    .WithMany(p => p.SprintBacklogItems)
                    .HasForeignKey(d => d.AssignedTo)
                    .HasConstraintName("FK__SprintBac__Assig__4D94879B");

                entity.HasOne(d => d.ProductBacklogItem)
                    .WithMany(p => p.SprintBacklogItems)
                    .HasForeignKey(d => d.ProductBacklogItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SprintBac__Produ__4CA06362");

                entity.HasOne(d => d.Sprint)
                    .WithMany(p => p.SprintBacklogItems)
                    .HasForeignKey(d => d.SprintId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SprintBac__Sprin__4BAC3F29");
            });

            // Configuración de UserTask (combinando ambas)
            modelBuilder.Entity<UserTask>(entity =>
            {
                entity.HasKey(e => e.TaskId).HasName("PK__Tasks__7C6949B15F1BC7FE");
                entity.ToTable("Tasks", "dbo");

                entity.Property(e => e.CompletedHours).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.EstimationHours).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.Title).HasMaxLength(200);

                entity.HasOne(d => d.AssignedToNavigation)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.AssignedTo)
                    .HasConstraintName("FK__Tasks__AssignedT__5165187F");

                entity.HasOne(d => d.SprintItem)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.SprintItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tasks__SprintIte__5070F446");
            });

            // Configuración de User (combinando ambas)
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

            // REMOVER: Esta línea estaba mal en BranchPao (no pertenece aquí)
            // [Column("CommentText")] public string Text { get; set; } = string.Empty;

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}