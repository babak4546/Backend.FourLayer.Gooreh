using GoorehDomain.Entities;
using GoorehDomain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehInfrastructure.DbContextes
{
    public class GoorehDbContext : DbContext
    {
        public GoorehDbContext(DbContextOptions<GoorehDbContext> o)
            : base(o) { }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserLogData> UserLogDatas { get; set; }
        public DbSet<MiddlewareLog> MiddlewareLogs { get; set; }
        public DbSet<UserNote> UserNotes { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }
        public DbSet<UserProduct> UserProducts { get; set; }
        public override int SaveChanges()
        {
            BeforeSaveChanges();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            BeforeSaveChanges();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            BeforeSaveChanges();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            return base.SaveChangesAsync(cancellationToken);
        }
        private void BeforeSaveChanges()
        {
            foreach (var item in ChangeTracker.Entries<BaseThing>())
            {
                if (item.State == EntityState.Modified)
                {
                    item.Entity.EditedIn = DateTime.Now;
                }
                else if (item.State == EntityState.Added)
                {
                    item.Entity.CreatedIn = DateTime.Now;
                    item.Entity.Guid = Guid.NewGuid().ToString();
                }

            }
            foreach (var item in ChangeTracker.Entries<UserProduct>())
            {
                if (item.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                {
                    item.Entity.ConcurrencyStamp = Guid.NewGuid().ToString();
                }

                //else if (item.State == EntityState.Modified || item.State == EntityState.Deleted)
                //{
                //    item.Entity.ConcurrencyStamp = Guid.NewGuid().ToString();
                //}

            }
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserLogData>()
                .HasOne(x => x.AppUser)
                .WithMany()
                .HasForeignKey(x => x.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<UserNote>()
                .HasOne(x => x.AppUser)
                .WithMany()
                .HasForeignKey(f => f.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<UserContact>()
                .HasOne(x => x.AppUser)
                .WithMany()
                .HasForeignKey(x => x.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);
            // migim keh cheh propi cuncurrency token hast
            builder.Entity<UserProduct>()
                .Property(p => p.ConcurrencyStamp)
                .IsConcurrencyToken();

        }
    }
}
