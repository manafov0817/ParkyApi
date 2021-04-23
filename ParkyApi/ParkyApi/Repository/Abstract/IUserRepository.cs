using ParkyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository.Abstract
{
    public interface IUserRepository  {
        public User Authenticate(string username, string password);
        public bool IsUniqueUser(string username );
        public User Register(string username, string password);
    }
}
