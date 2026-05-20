using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Identity;

namespace TaskManagement.Infrastructure.Persistence
{
    public class ApplicationDbContext
      : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {


        private readonly ICurrentUserService
          _currentUserService;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService =
                currentUserService;
        }
        public DbSet<Project> Projects => Set<Project>();

        public DbSet<ProjectTask> Tasks => Set<ProjectTask>();
        protected override void OnModelCreating(
     ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Project>()
                .HasMany(x => x.Tasks)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId);
        }

        public override Task<int> SaveChangesAsync(
      bool acceptAllChangesOnSuccess,
      CancellationToken cancellationToken = default)
        {
            foreach (var entry
                     in ChangeTracker
                         .Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:

                        entry.Entity.CreatedDate =
                            DateTime.UtcNow;

                        entry.Entity.CreatedBy =
                            _currentUserService.UserId
                            ?? "System";

                        break;

                    case EntityState.Modified:

                        entry.Entity.LastModifiedDate =
                            DateTime.UtcNow;

                        entry.Entity.LastModifiedBy =
                            _currentUserService.UserId
                            ?? "System";

                        break;
                }
            }

            return base.SaveChangesAsync(
                acceptAllChangesOnSuccess,
                cancellationToken);
        }
    }
}
