using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepo;
         private readonly ITrailRepository _trailRepo;
        public HomeController(ILogger<HomeController> logger, INationalParkRepository npRepo,
            ITrailRepository trailRepo )
        {
            _npRepo = npRepo;
            _trailRepo = trailRepo;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel listOfParksAndTrails = new IndexViewModel()
            {
                NationalParkList = await _npRepo.GetAllAsync(SD.NationalParkUrl ),
                TrailList = await _trailRepo.GetAllAsync(SD.TrailUrl ),
            };
            return View(listOfParksAndTrails);
        }
    }
}
