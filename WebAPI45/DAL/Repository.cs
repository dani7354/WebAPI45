using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebAPI45.DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        private readonly DbSet<T> _DbSet;
        public Repository(DbContext context)
        {
            Context = context;
            _DbSet = Context.Set<T>();
        }

        public void Add(T entity)
        {
            _DbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _DbSet.AddRange(entities);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _DbSet.Where(predicate);
        }

        public T Get(int id)
        {
            return _DbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _DbSet.ToList();
        }

        public void Remove(T entity)
        {
            _DbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _DbSet.RemoveRange(entities);
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _DbSet.SingleOrDefault(predicate);
        }
    }
}
