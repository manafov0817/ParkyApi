using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepo;
        private readonly IAccountRepository _accountRepository;
        private readonly ITrailRepository _trailRepo;
        public HomeController(ILogger<HomeController> logger, INationalParkRepository npRepo,
            ITrailRepository trailRepo, IAccountRepository accountRepository)
        {
            _npRepo = npRepo;
            _trailRepo = trailRepo;
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel listOfParksAndTrails = new IndexViewModel()
            {
                NationalParkList = await _npRepo.GetAllAsync(SD.NationalParkUrl, HttpContext.Session.GetString("JWToken")),
                TrailList = await _trailRepo.GetAllAsync(SD.TrailUrl, HttpContext.Session.GetString("JWToken")),
            };
            return View(listOfParksAndTrails);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            User userModel = await _accountRepository.LoginAsync(SD.AccountUrl + "authenticate/", user);
            if (userModel.Token == null)
            {
                return View();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(ClaimTypes.Name, userModel.Username));

            identity.AddClaim(new Claim(ClaimTypes.Role, userModel.Role));

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            HttpContext.Session.SetString("JWToken", userModel.Token);
            TempData["alert"] = "Welcome " + userModel.Username;

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            bool result = await _accountRepository.RegisterAsync(SD.AccountUrl + "register/", user);
            if (!result)
            {
                return View();
            }
 
            TempData["alert"] = "Registration Successful";

            return RedirectToAction("Login", "Home");
        }

        public async Task<IActionResult> LogOut(User user)
        {
            await HttpContext.SignOutAsync();

            HttpContext.Session.SetString("JWToken", "");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
