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
    public class TrailRepository : GenericRepository<Trail, ParkyApiDbContext>, ITrailRepository
    {
        public ICollection<Trail> GetTrailsInNationalPark(int id)
        {
            using (var context = new ParkyApiDbContext())
            {
                return context.Trails.Where(t => t.NationalParkId == id).Include(t => t.NationalPark).ToList();
            }
        }
        public new Trail GetById(int id)
        {
            using (var context = new ParkyApiDbContext())
            {
                return context.Trails.Where(t => t.Id == id).Include(t => t.NationalPark).FirstOrDefault();
            }
        }

        public new ICollection<Trail> GetAll()
        {
            using (var context = new ParkyApiDbContext())
            {

                return context.Trails.Include(t => t.NationalPark).ToList();
            }
        }
        public new bool ExsistsByName(string name)
        {
            using (var context = new ParkyApiDbContext())
            {
                return context.Trails.Any(e => e.Name == name);
            }
        }
        public new bool ExsistsById(int id)
        {
            using (var context = new ParkyApiDbContext())
            {
                return context.Trails.Any(e => e.Id == id);
            }
        }
    }
}
