using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using YourForum.Core.Infrastructure;
using YourForum.Core.Models;

namespace YourForum.Core.Data
{
    public class YourForumContext : DbContext
    {
        private IDbContextTransaction _currentTransaction;

        private TenantProvider _tenantProvider;

        public YourForumContext(DbContextOptions<YourForumContext> options, TenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>()
                .HasQueryFilter(a => a.TenantId == _tenantProvider.GetTenantId());


            builder.Entity<Post>()
                .HasQueryFilter(a => a.TenantId == _tenantProvider.GetTenantId());

            base.OnModelCreating(builder);
        }
        
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddTimestamps();
            AddTenant();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            AddTenant();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is IEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((IEntity)entity.Entity).DateCreated = DateTime.UtcNow;
                }

                ((IEntity)entity.Entity).DateModified = DateTime.UtcNow;
            }
        }

        private void AddTenant()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is ITenantEntity && (x.State == EntityState.Added));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((ITenantEntity)entity.Entity).TenantId = _tenantProvider.GetTenantId();
                }
            }
        }



        #region TransactionRequests
        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
        #endregion
    }
}
