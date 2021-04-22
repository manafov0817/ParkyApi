using Microsoft.EntityFrameworkCore;
using ParkyApi.Data;
using ParkyApi.Models;
using ParkyApi.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository.Concrete
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
         where TEntity : class
         where TContext : DbContext, new()
    {
        public ICollection<TEntity> GetAll()
        {
            using (var context = new TContext())
            {

                return context.Set<TEntity>().ToList();
            }
        }

        public bool Create(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().Add(entity);
                context.SaveChanges();
                return Result();

            }
        }

        public bool Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().Remove(entity);
                context.SaveChanges();
                return Result();
            }
        }

        public bool Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().Update(entity);
                context.SaveChanges();
                return Result();
            }
        }

        public bool Result()
        {
            using (var context = new TContext())
            {
                return context.SaveChanges() >= 0 ? true : false;
            }
        }

        public bool ExsistsById(int id)
        {
            using (var context = new TContext())
            {
                return context.Set<AbstractModel>().Any(e => e.Id == id);
            }
        }

        public bool ExsistsByName(string name)
        {
            using (var context = new TContext())
            {
                return context.Set<AbstractModel>().Any(e => e.Name == name);
            }
        }

        public TEntity GetById(int id)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().Find(id);
            }
        }
    }
}
