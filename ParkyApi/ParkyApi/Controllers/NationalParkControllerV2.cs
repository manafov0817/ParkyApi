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
    [ApiVersion("2.0")]
    [ApiController]
    public class NationalParkControllerV2 : ControllerBase
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;
        public NationalParkControllerV2(INationalParkRepository nationalParkRepository, IMapper mapper)
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
    }
}
