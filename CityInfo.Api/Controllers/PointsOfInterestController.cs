using System.Threading.Tasks;
using AutoMapper;
using CityInfo.Api.Entities;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    [Route("api/cities/{cityId}/PointsOfInterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _CityInfoRepo;
        private readonly IMapper _mapper;
        private readonly CitiesDataStore _citiesDataStore;

        public PointsOfInterestController(
            ILogger<PointsOfInterestController> logger,
            IMailService mailSerivce,
            ICityInfoRepository cityInfoRepo,
            IMapper mapper
        )
        {
            _logger = logger;
            _mailService = mailSerivce;
            _CityInfoRepo = cityInfoRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(
            int cityId
        )
        {
            if (!await _CityInfoRepo.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City with id {cityId} wasn't found when accessing points of interest."
                );
                return NotFound();
            }
            var PointsOfInterestForCity = await _CityInfoRepo.GetPointsOfInterestForCityAsync(
                cityId
            );
            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(PointsOfInterestForCity));
        }

        [HttpGet("{PointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetSpecificPointOfInterestOfCity(
            int cityId,
            int PointOfInterestId
        )
        {
            if (!await _CityInfoRepo.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var PointOfInterest = await _CityInfoRepo.GetPointOfInterestForCityAsync(
                cityId,
                PointOfInterestId
            );
            if (PointOfInterest == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PointOfInterestDto>(PointOfInterest));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterestAsync(
            int cityId,
            PointOfInterestForCreationDto pointofinterest
        )
        {
            if (!await _CityInfoRepo.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var PointOfInterestToAdd = _mapper.Map<PointOfInterest>(pointofinterest);

            await _CityInfoRepo.AddPointOfInterestForCityAsync(cityId, PointOfInterestToAdd);
            await _CityInfoRepo.SaveChangesAsync();

            var createdPointOfInterestToReturn = _mapper.Map<Models.PointOfInterestDto>(
                PointOfInterestToAdd
            );

            return CreatedAtRoute(
                "GetPointOfInterest",
                new { cityId = cityId, PointOfInterestId = createdPointOfInterestToReturn.Id },
                createdPointOfInterestToReturn
            );
        }

        [HttpPut("{PointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(
            int cityId,
            int PointOfInterestId,
            PointOfInterestDto PointOfInterest
        )
        {
            if (!await _CityInfoRepo.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var pointOfInterestEntity = await _CityInfoRepo.GetPointOfInterestForCityAsync(
                cityId,
                PointOfInterestId
            );
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }
            _mapper.Map(PointOfInterest, pointOfInterestEntity);
            await _CityInfoRepo.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{PointOfInterestId}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterestAsync(
            int cityId,
            int PointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument
        )
        {
            if (!await _CityInfoRepo.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var pointOfInterestEntity = await _CityInfoRepo.GetPointOfInterestForCityAsync(
                cityId,
                PointOfInterestId
            );
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(
                pointOfInterestEntity
            );

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            await _CityInfoRepo.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterestAsync(
            int cityId,
            int pointOfInterestId
        )
        {
            if (!await _CityInfoRepo.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var pointOfInterestEntity = await _CityInfoRepo.GetPointOfInterestForCityAsync(
                cityId,
                pointOfInterestId
            );
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _CityInfoRepo.DeletePointOfInterest(pointOfInterestEntity);

            _mailService.send(
                "Point of interest deleted",
                $"Pointof interest {pointOfInterestEntity.Name} with id {pointOfInterestId} was deleted."
            );

            return NoContent();
        }
    }
}
