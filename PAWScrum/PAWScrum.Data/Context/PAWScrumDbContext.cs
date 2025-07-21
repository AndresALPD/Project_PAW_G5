using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PAWScrum.Models;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PAWScrum.Data.Context
{
    public partial class PAWScrumDbContext : DbContext
    {
        public PAWScrumDbContext(DbContextOptions<PAWScrumDbContext> options)
            : base(options)
        {
        }
        //Error se hace conexion en appsetting
        //Configurar la conexión a la base de datos
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Data Source=SQL1004.site4now.net;Initial Catalog=db_abaa68_pawscrum;User Id=db_abaa68_pawscrum;Password=Cafecafe04;TrustServerCertificate=True;");
        //    }
        //}


        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<ProductBacklogItem> ProductBacklogItems { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<ProjectMember> ProjectMembers { get; set; }
        public virtual DbSet<Sprints> Sprints { get; set; }
        public virtual DbSet<SprintBacklogItem> SprintBacklogItems { get; set; }
        public virtual DbSet<UserTask> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración explícita para ActivityLog
            modelBuilder.Entity<ActivityLog>(entity =>
            {
                entity.HasKey(e => e.LogId); 

                entity.Property(e => e.LogId)
                    .ValueGeneratedOnAdd(); 

                // Configuración de relaciones
                entity.HasOne(d => d.Project)
                    .WithMany()
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFCAEA75DC6D");
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.SprintItem).WithMany(p => p.Comments)
                    .HasForeignKey(d => d.SprintItemId)
                    .HasConstraintName("FK__Comments__Sprint__5629CD9C");

                entity.HasOne(d => d.Task).WithMany(p => p.Comments)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK__Comments__TaskId__571DF1D5");

                entity.HasOne(d => d.User).WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comments__UserId__5535A963");
            });

            modelBuilder.Entity<ProductBacklogItem>(entity =>
            {
                // Configuración de la tabla
                entity.ToTable("ProductBacklogItems");

                // Clave primaria
                entity.HasKey(e => e.ItemId)
                    .HasName("PK_ProductBacklogItems");

                entity.Property(e => e.ItemId)
                    .HasColumnName("ItemId")
                    .ValueGeneratedOnAdd();

                
                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProductBacklogItem) 
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_ProductBacklogItems_Projects")
                    .OnDelete(DeleteBehavior.Cascade);

                
                entity.HasMany(d => d.SprintBacklogItems)
                    .WithOne(s => s.ProductBacklogItem)
                    .HasForeignKey(s => s.ProductBacklogItemId) // Cambiado a ProductBacklogItemId
                    .HasConstraintName("FK_SprintBacklogItems_ProductBacklogItems")
                    .OnDelete(DeleteBehavior.Cascade); // Cambiado a Cascade para consistencia
            });


            //CHRISTOPHER PROJETS
            //    CREATE TABLE Projects(
            //    ProjectId INT PRIMARY KEY IDENTITY,
            //    ProjectName NVARCHAR(200) NOT NULL,
            //    ProjectKey NVARCHAR(10) UNIQUE NOT NULL,
            //    Description NVARCHAR(500) NULL,
            //    OwnerId INT NOT NULL,
            //    Visibility NVARCHAR(20), --'Private' CHECK(Visibility IN('Private', 'Public'))
            //    Status NVARCHAR(20), --DEFAULT 'Active' CHECK(Status IN('Active', 'Completed', 'Archived'))
            //    StartDate DATE NULL,
            //    EndDate DATE NULL,
            //    SprintDuration INT,
            //    RepositoryUrl NVARCHAR(200) NULL,
            //    IsArchived BIT DEFAULT 0,
            //    CreatedDate DATETIME DEFAULT GETDATE(),
            //    FOREIGN KEY(OwnerId) REFERENCES Users(UserId)
            //);
            modelBuilder.Entity<Projects>(entity =>
            {
                entity.HasKey(e => e.ProjectId)
                    .HasName("PK__Projects__761ABEF0AFF11888");

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ProjectKey)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.Visibility)
                    .HasMaxLength(20);
                // Consider adding: .HasConversion<string>() if using enum

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasDefaultValue("Active"); 

                entity.Property(e => e.StartDate)
                    .HasColumnType("date");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date");

                entity.Property(e => e.SprintDuration)
                    .HasColumnType("int");

                entity.Property(e => e.RepositoryUrl)
                    .HasMaxLength(200);

                entity.Property(e => e.IsArchived)
                    .HasDefaultValue(false); 

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE()"); 

                entity.HasIndex(e => e.ProjectKey)
                    .IsUnique()
                    .HasDatabaseName("IX_Projects_ProjectKey"); 

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Projects__OwnerI__3E52440B");
            });


            modelBuilder.Entity<ProjectMember>(entity =>
            {
                // Configuración básica de la tabla
                entity.ToTable("ProjectMembers");

                // Clave primaria compuesta con nombres descriptivos
                entity.HasKey(e => new { e.ProjectId, e.UserId })
                    .HasName("PK_ProjectMembers");

                // Configuración de propiedades
                entity.Property(e => e.ProjectId)
                    .HasColumnName("ProjectId");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserId");

                entity.Property(e => e.Role)
                    .HasColumnName("Role")
                    .HasColumnType("NVARCHAR(50)")
                    .HasMaxLength(50)
                    .IsRequired();

                // Relación con Project
                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectMembers)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_ProjectMembers_Projects")
                    .OnDelete(DeleteBehavior.Cascade); // Mejor comportamiento para miembros

                // Relación con User
                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProjectMembers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ProjectMembers_Users")
                    .OnDelete(DeleteBehavior.Cascade);

                // Índices adicionales para mejorar el rendimiento
                entity.HasIndex(e => e.UserId)
                    .HasDatabaseName("IX_ProjectMembers_UserId");

                entity.HasIndex(e => e.Role)
                    .HasDatabaseName("IX_ProjectMembers_Role");
            });

            modelBuilder.Entity<Sprints>(entity =>
            {
                // Configuración de tabla y clave primaria
                entity.ToTable("Sprints");
                entity.HasKey(e => e.SprintId)
                    .HasName("PK_Sprints"); // Nombre más descriptivo que el generado automáticamente

                // Configuración de propiedades
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

                // Configuración de relación con Project
                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Sprints)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_Sprints_Projects") 
                    .OnDelete(DeleteBehavior.Restrict); 
            });

           

            modelBuilder.Entity<UserTask>(entity =>
            {
                entity.HasKey(e => e.TaskId).HasName("PK__Tasks__7C6949B15F1BC7FE");
                entity.Property(e => e.CompletedHours).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.EstimationHours).HasColumnType("decimal(5, 2)");
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