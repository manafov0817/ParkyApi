using ParkyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository.Abstract
{
    public interface ITrailRepository : IGenericRepository<Trail> {
        ICollection<Trail> GetTrailsInNationalPark(int id);
    }
}
