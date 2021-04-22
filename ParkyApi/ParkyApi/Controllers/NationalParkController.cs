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
    [Route("api/v{version:apiVersion}/nationalPark")]
    [ApiController]
    public class NationalParkController : ControllerBase
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;
        public NationalParkController(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get All National Parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var nationalParks = _nationalParkRepository.GetAll();

            var nationalParksDTO = new List<NationalParkDTO>();

            foreach (var nationalPark in nationalParks)
            {
                nationalParksDTO.Add(_mapper.Map<NationalParkDTO>(nationalPark));
            }

            return Ok(nationalParksDTO);
        }

        /// <summary>
        /// Get Individual National Park
        /// </summary>
        /// <param name="nationalParkId">The Id of National Park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark = _nationalParkRepository.GetById(nationalParkId);

            if (nationalPark == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<NationalParkDTO>(nationalPark));
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Create([FromBody] NationalParkDTO nationalParkDTO)
        {
            if (nationalParkDTO == null)
            {
                return BadRequest();
            }
            if (_nationalParkRepository.ExsistsByName(nationalParkDTO.Name))
            {
                ModelState.AddModelError("", "National Park exsists");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var nationalPark = _mapper.Map<NationalPark>(nationalParkDTO);
            if (!_nationalParkRepository.Create(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {nationalPark.Name} ");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalPark.Id }, nationalPark);

        }


        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        public IActionResult Update(int nationalParkId, NationalParkDTO nationalParkDTO)
        {
            if (nationalParkDTO == null || nationalParkId == 0)
            {
                return BadRequest(ModelState);
            }
            NationalPark nationalPark = _mapper.Map<NationalPark>(nationalParkDTO);

            if (!_nationalParkRepository.Update(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong saving the record {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }


        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        public IActionResult Delete(int nationalParkId)
        {
            if (!_nationalParkRepository.ExsistsById(nationalParkId))
            {
                return NotFound();
            }
            NationalPark nationalPark = (NationalPark)_nationalParkRepository.GetById(nationalParkId);
            if (!_nationalParkRepository.Delete(nationalPark))
            {

                ModelState.AddModelError("", $"Something went wrong deleting the record {nationalPark}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
    }
}
