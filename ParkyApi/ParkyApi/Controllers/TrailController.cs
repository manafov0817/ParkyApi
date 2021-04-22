using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models;
using ParkyApi.Models.DTOs;
using ParkyApi.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Controllers
{
    [Route("api/v{version:apiVersion}/trail")]
    [ApiController]
    public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;
        public TrailController(ITrailRepository  trailRepository, IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }

        /// <summary>
        ///  Get All Trails
         /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetTrails()
        {
            var trails = _trailRepository.GetAll();

            var trailsDTO = new List<TrailDTO>();

            foreach (var trail in trails)
            {
                trailsDTO.Add(_mapper.Map<TrailDTO>(trail));
            }

            return Ok(trailsDTO);
        }

        /// <summary>
        /// Get Individual National Park
        /// </summary>
        /// <param name="trailId">The Id of Trail <param>
        /// <returns></returns>
        [HttpGet("{trailId:int}")]
        public IActionResult GetTrail(int trailId)
        {
            var trail = _trailRepository.GetById(trailId);

            if (trail == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TrailDTO>(trail));
        }


        [HttpPost]
        public IActionResult Create([FromBody] TrailUpsertDTO trailDTO)
        {
            if (trailDTO == null)
            {
                return BadRequest();
            }
            if (_trailRepository.ExsistsByName(trailDTO.Name))
            {
                ModelState.AddModelError("", "National Park exsists");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var trail = _mapper.Map<Trail>(trailDTO);
            if (!_trailRepository.Create(trail))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trail.Name} ");
                return StatusCode(500, ModelState);
            }
            return Ok();

        }


        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        public IActionResult Update(int trailId, TrailUpsertDTO trailDTO)
        {
            if (trailDTO == null || trailId == 0)
            {
                return BadRequest(ModelState);
            }
            Trail trail = _mapper.Map<Trail>(trailDTO);

            if (!_trailRepository.Update(trail))
            {
                ModelState.AddModelError("", $"Something went wrong saving the record {trail.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }


        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        public IActionResult Delete(int trailId)
        {
            if (!_trailRepository.ExsistsById(trailId))
            {
                return NotFound();
            }
            Trail trail = (Trail)_trailRepository.GetById(trailId);
            if (!_trailRepository.Delete(trail))
            {

                ModelState.AddModelError("", $"Something went wrong deleting the record {trail}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
    }
}
 