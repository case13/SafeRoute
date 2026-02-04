using Microsoft.EntityFrameworkCore;
using SafeRoute.Domain.Entities;
using SafeRoute.Shared.Enums.Status;
using SafeRoute.Shared.Enums.Tipos;

namespace SafeRoute.Infrastructure.Data
{
    public class SafeRouteDbContext : DbContext
    {
        public SafeRouteDbContext(DbContextOptions<SafeRouteDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<RuleViolation> RuleViolations => Set<RuleViolation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>().HasData(
                new
                {
                    Id = 1,
                    LegalName = "Case13 Solutions",
                    Name = "Case13 Tecnology ME",
                    Registry = "32230441000176",
                    StatusCompany = StatusBasicEnum.Ativo,
                    CreatedAt = new DateTime(2026, 1, 1),
                    UpdatedAt = (DateTime?)null,
                    IsActive = true
                }
            );

            modelBuilder.Entity<Project>().HasData(
                new
                {  
                    Id = 1,
                    Name = "Projeto Teste",
                    ExternalId = "TEST-PROJECT-001",
                    CompanyId = 1,
                    CreatedAt = new DateTime(2026, 1, 1),
                    IsActive = true
                }
            );

            // Padroniza nome da tabela = nome da entidade
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
                entity.SetTableName(entity.DisplayName());

            // Company
            modelBuilder.Entity<Company>(e =>
            {
                e.HasKey(x => x.Id);

                e.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                e.Property(x => x.LegalName)
                    .IsRequired()
                    .HasMaxLength(200);

                e.Property(x => x.Registry)
                    .IsRequired()
                    .HasMaxLength(50);

                e.Property(x => x.StatusCompany)
                    .IsRequired();

                e.HasMany(x => x.Users)
                    .WithOne(x => x.Company)
                    .HasForeignKey(x => x.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(x => x.Projects)
                    .WithOne(x => x.Company)
                    .HasForeignKey(x => x.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Project
            modelBuilder.Entity<Project>(e =>
            {
                e.HasKey(x => x.Id);

                e.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                e.Property(x => x.ExternalId)
                    .HasMaxLength(200);

                e.HasOne(x => x.Company)
                    .WithMany(c => c.Projects)
                    .HasForeignKey(x => x.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // User
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(x => x.Id);

                e.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                e.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                e.HasIndex(x => x.Email)
                    .IsUnique();

                e.Property(x => x.PasswordHash)
                    .IsRequired();

                e.Property(x => x.PasswordSalt)
                    .IsRequired();

                e.Property(x => x.UserType)
                    .IsRequired();

                e.Property(x => x.UserStatus)
                    .IsRequired();

                e.HasOne(x => x.Company)
                    .WithMany(c => c.Users)
                    .HasForeignKey(x => x.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // RefreshToken
            modelBuilder.Entity<RefreshToken>(e =>
            {
                e.HasKey(x => x.Id);

                e.Property(x => x.Token)
                    .IsRequired()
                    .HasMaxLength(500);

                e.Property(x => x.ExpiresAt)
                    .IsRequired();

                e.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // RuleViolation
            modelBuilder.Entity<RuleViolation>(e =>
            {
                e.HasKey(x => x.Id);

                e.Property(x => x.ElementExternalId).IsRequired();
                e.Property(x => x.ElementType).IsRequired();
                e.Property(x => x.RuleCode).IsRequired();
                e.Property(x => x.Message).IsRequired();
                e.Property(x => x.Severity).IsRequired();

                e.HasOne(x => x.Project)
                    .WithMany(p => p.RuleViolations)
                    .HasForeignKey(x => x.ProjectId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
