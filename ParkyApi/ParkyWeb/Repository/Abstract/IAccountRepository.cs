using ParkyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Repository.Abstract
{
    public interface IAccountRepository : IRepository<User>
    {
        Task<User> LoginAsync(string url, User user);
        Task<bool> RegisterAsync(string url, User user);
    }
}
