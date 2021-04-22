using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _trailRepository;
        private readonly INationalParkRepository _nationalParkRepository;
        public TrailsController(ITrailRepository trailRepository,
                                INationalParkRepository nationalParkRepository)
        {
            _trailRepository = trailRepository;
            _nationalParkRepository = nationalParkRepository;
        }

        public IActionResult Index()
        {
            return View(new Trail());
        }

        public async Task<IActionResult> Upsert(int? id)
        {

            IEnumerable<NationalPark> npList = await _nationalParkRepository.GetAllAsync(SD.NationalParkUrl);

            TrailsViewModel viewModel = new TrailsViewModel()
            {
                NationalParkList = npList.Select(np => new SelectListItem
                {
                    Text = np.Name,
                    Value = np.Id.ToString()
                })

            };

            if (id == null)
            {
                return View(viewModel);
            }

            viewModel.Trail = await _trailRepository.GetAsync(SD.TrailUrl, id.GetValueOrDefault());

            if (viewModel.Trail == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(TrailsViewModel obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Trail.Id == 0)
                {
                    await _trailRepository.CreateAsync(SD.TrailUrl, obj.Trail);
                }
                else
                {
                    await _trailRepository.UpdateAsync(SD.NationalParkUrl + obj.Trail.Id, obj.Trail);
                }
                return RedirectToAction("Index", "Trails");
            }
            else
            {
                IEnumerable<NationalPark> npList = await _nationalParkRepository.GetAllAsync(SD.NationalParkUrl);

                TrailsViewModel objVM = new TrailsViewModel()
                {
                    NationalParkList = npList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Trail = obj.Trail
                };
                return View(objVM);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepository.DeleteAync(SD.TrailUrl, id);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }

        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { data = await _trailRepository.GetAllAsync(SD.TrailUrl) });
        }
    }
}
