﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models;
using ParkyApi.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepo;
        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User model)
        {
            var user = _userRepo.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthenticationModel model)
        {
            bool ifUserNameUnique = _userRepo.IsUniqueUser(model.Username);
            if (!ifUserNameUnique)
            {
                return BadRequest(new { message = "Username already exists" });
            }
            var user = _userRepo.Register(model.Username, model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Error while registering" });
            }
            return Ok();
        }
    }
}