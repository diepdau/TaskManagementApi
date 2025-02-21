﻿using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Models;
using TaskManagementApi.Interfaces;

namespace TaskManagementApi
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly TaskManagementDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(TaskManagementDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll() => _dbSet.ToList();

        public T GetById(int id) => _dbSet.Find(id);

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }
    }

}
