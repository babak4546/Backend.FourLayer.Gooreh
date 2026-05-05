using GoorehDomain.Entities;
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
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserLogData>()
                .HasOne(x => x.AppUser)
                .WithMany()
                .HasForeignKey(x=>x.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
