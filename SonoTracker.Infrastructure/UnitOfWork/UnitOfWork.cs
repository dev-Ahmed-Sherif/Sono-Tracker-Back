using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SonoTracker.Common.DTO.Identity.User;
using SonoTracker.Common.Infrastructure.Repository;
using SonoTracker.Common.Infrastructure.UnitOfWork;
using SonoTracker.Domain.Entities.Base;
using SonoTracker.Domain.Entities.Identity;
using SonoTracker.Infrastructure.Repository;

namespace SonoTracker.Infrastructure.UnitOfWork
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class 
    {
        private DbContext _context;
        private IDbContextTransaction _transaction;
        private Dictionary<string, dynamic> _repositories;
        private readonly UserData _user;
        public IRepository<T> Repository { get; }
        public UnitOfWork(DbContext context,UserData user)
        {
            _context = context;
            _user = user;
            Repository = new Repository<T>(_context);
        }

        #region Public Methods
        /// <summary>
        /// Get Repository Instance
        /// </summary>
        /// <typeparam name="TB"></typeparam>
        /// <returns></returns>
        public IRepository<TB> GetRepository<TB>() where TB : class
        {
            _repositories ??= new Dictionary<string, dynamic>();
            var type = typeof(TB).Name;
            if (_repositories.ContainsKey(type))
            {
                return (IRepository<TB>)_repositories[type];
            }

            var repositoryType = typeof(Repository<TB>);
            var repository = Activator.CreateInstance(repositoryType, _context);
            _repositories.Add(type, repository);
            return _repositories[type];
        }
        /// <summary>
        /// Save Changes Async
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            ApplyChangesDate();
            return await _context.SaveChangesAsync();
        }
        public void ApplyChangesDate()
        {
            var entries = _context.ChangeTracker.Entries<BaseEntity<Guid>>();
            foreach (var item in entries)
            {
                switch (item.State)
                {
                    case EntityState.Added:
                        item.Entity.CreatedDate = DateTime.Now;
                        item.Entity.CreatedById = _user.Id ?? null;
                        item.Entity.CreatedByEmployeeAr = _user.Name ?? null;
                        item.Entity.ModifiedDate = DateTime.Now;
                        item.Entity.ModifiedById = _user.Id ?? null;
                        item.Entity.ModifiedByEmployeeAr = _user.Name ?? null;
                        break;
                    case EntityState.Modified:
                        item.Entity.ModifiedDate = DateTime.Now;
                        item.Entity.ModifiedById = _user.Id ?? null;
                        item.Entity.ModifiedByEmployeeAr = _user.Name ?? null;
                        break;
                }
            }
        }
        /// <summary>
        /// Save Changes
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return  _context.SaveChanges();
        }
        /// <summary>
        /// Start Transaction
        /// </summary>
        public void StartTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }
        /// <summary>
        /// Commit Transaction
        /// </summary>
        public void CommitTransaction()
        {
            _transaction.Commit();
            _transaction.Dispose();
        }
        /// <summary>
        /// Rollback
        /// </summary>
        public void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }
        /// <summary>
        /// Dispose Resource
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (_context == null)
            {
                return;
            }

            _context.Dispose();
            _context = null;
        }

        #endregion

    }
}