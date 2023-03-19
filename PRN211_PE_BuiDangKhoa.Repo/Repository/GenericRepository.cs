
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PRN211PE_SU22_BuiDangKhoa.Repo.Models;
using PRN211PE_SU22_BuiDangKhoa.Repo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;

namespace Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected BankAccountTypeContext _context;
        protected DbSet<T> dbSet;

        public GenericRepository(BankAccountTypeContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }

        public virtual void Create(T entity)
        {
            dbSet.Add(entity);

            _context.SaveChanges();
        }

        public virtual T Delete(Expression<Func<T, bool>> predicate)
        {
            T _entity = Getby(predicate);

            if(_entity != null)
            {
                _context.Set<T>().Remove(_entity);
                int i = _context.SaveChanges();
                Console.WriteLine(i);
            }

            return _entity;
        }

        public virtual ICollection<T> GetAll(Expression<Func<T, bool>> predicate = null, params string[] navigationProperties)
        {
            IQueryable<T> query = dbSet;

            //when need include navigation properties
            if(navigationProperties.Length != 0)
            {
                query = ApplyNavigation(navigationProperties);
            }

            //when predicate is null, return all records (dont have any condition)
            if(predicate == null)
            {
                return dbSet.ToList();
            }

            return query.Where(predicate).ToList();
        }

        public virtual T Getby(Expression<Func<T, bool>> predicate, params string[] navigationProperties)
        {
            IQueryable<T> query = dbSet;

            //when need include navigation properties
            if(navigationProperties.Length != 0)
            {
                query = ApplyNavigation(navigationProperties);
            }

            return query.FirstOrDefault(predicate);
        }

        public virtual void Update(T entity)
        {
            _context.Attach(entity).State = EntityState.Modified;

            _context.SaveChanges();
        }

        private IQueryable<T> ApplyNavigation(params string[] navigationProperties)
        {
            var query = dbSet.AsQueryable();

            foreach(string navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }

            return query;
        }

    }
}

