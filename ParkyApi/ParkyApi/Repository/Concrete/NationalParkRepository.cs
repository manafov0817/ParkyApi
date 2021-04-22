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
    public class NationalParkRepository : GenericRepository<NationalPark, ParkyApiDbContext>, INationalParkRepository
    {
        public new bool ExsistsByName(string name)
        {
            using (var context = new ParkyApiDbContext())
            {
                return context.NationalParks.Any(e => e.Name == name);
            }
        }
        public new bool ExsistsById(int id)
        {
            using (var context = new ParkyApiDbContext())
            {
                return context.NationalParks.Any(e => e.Id == id);
            }
        }
    }
}
