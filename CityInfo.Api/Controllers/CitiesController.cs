using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _CityInfoRepo;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository CityInfoRepo, IMapper mapper)
        {
            _CityInfoRepo = CityInfoRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
        {
            var cityEntities = await _CityInfoRepo.GetCitiesAsync();
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            var cityentity = await _CityInfoRepo.GetCityAsync(id, includePointsOfInterest);
            if (cityentity == null)
            {
                return NotFound();
            }
            if (includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(cityentity));
            }

            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(cityentity));
        }
    }
}
