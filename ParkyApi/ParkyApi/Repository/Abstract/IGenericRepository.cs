using ParkyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository.Abstract
{
    public interface IGenericRepository<T>
    {
        ICollection<T> GetAll();
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        public bool ExsistsById(int id);
        public bool ExsistsByName(string name);
        T GetById(int id);
    }
}
