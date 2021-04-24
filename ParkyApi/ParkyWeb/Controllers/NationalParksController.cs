using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    [Authorize]
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        public NationalParksController(INationalParkRepository nationalParkRepository)
        {
            _nationalParkRepository = nationalParkRepository;
        }

        public IActionResult Index()
        {
            return View(new NationalPark());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark nationalPark = new NationalPark();
            if (id == null)
            {
                return View(nationalPark);
            }

            nationalPark = await _nationalParkRepository.GetAsync(SD.NationalParkUrl, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));

            if (nationalPark == null)
            {
                return NotFound();
            }

            return View(nationalPark);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Upsert(NationalPark obj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    obj.Picture = p1;
                }
                else
                {
                    var objFromDb = await _nationalParkRepository.GetAsync(SD.NationalParkUrl, obj.Id, HttpContext.Session.GetString("JWToken"));
                    obj.Picture = objFromDb.Picture;
                }
                if (obj.Id == 0)
                {
                    await _nationalParkRepository.CreateAsync(SD.NationalParkUrl, obj, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _nationalParkRepository.UpdateAsync(SD.NationalParkUrl + obj.Id, obj, HttpContext.Session.GetString("JWToken"));
                }
                return RedirectToAction("Index", "NationalParks");
            }
            else
            {
                return View(obj);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _nationalParkRepository.DeleteAync(SD.NationalParkUrl, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }

        public async Task<IActionResult> GetAllNationalParks()
        {
            return Json(new { data = await _nationalParkRepository.GetAllAsync(SD.NationalParkUrl, HttpContext.Session.GetString("JWToken")) });
        }
    }
}
