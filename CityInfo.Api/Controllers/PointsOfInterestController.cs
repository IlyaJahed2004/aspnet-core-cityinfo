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
        private readonly CitiesDataStore _citiesDataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, 
             IMailService mailSerivce,
             CitiesDataStore citiesDataStore)
        {
            _logger = logger;
            _mailService = mailSerivce;
            _citiesDataStore = citiesDataStore;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            //throw new Exception("Exception sample.");

            try
            {
                var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);

                if (city == null)
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }

                return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"Exception while getting points of interest for city with id {cityId}",ex);
                return StatusCode(500,
                    "A problem happened while handling your request.");
            }

        }   


        [HttpGet("{PointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetSpecificPointOfInterestOfCity(int cityId, int PointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var PointOfInterest = city.PointsOfInterest.FirstOrDefault(points => points.Id == PointOfInterestId);
            if (PointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(PointOfInterest);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointofinterest)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var maxPointOfInterestId = _citiesDataStore.Cities.SelectMany(
                c => c.PointsOfInterest).Max(x => x.Id);

            var PointOfInterestToAdd = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointofinterest.Name,
                Description = pointofinterest.Description
            };

            city.PointsOfInterest.Add(PointOfInterestToAdd);

            return CreatedAtRoute(
                "GetPointOfInterest", 
                new
                {
                    cityId = cityId,
                    PointOfInterestId = PointOfInterestToAdd.Id
                },
                PointOfInterestToAdd
                );
        }

        [HttpPut("{PointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int PointOfInterestId, PointOfInterestDto PointOfInterest)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var PointOfInterestForUpdate = city.PointsOfInterest.FirstOrDefault(x => x.Id == PointOfInterestId);
            if (PointOfInterestForUpdate == null)
            {
                return NotFound();
            }

            PointOfInterestForUpdate.Name = PointOfInterest.Name;
            PointOfInterestForUpdate.Description = PointOfInterest.Description;

            return NoContent();
        }



        [HttpPatch("{PointOfInterestId}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId, int PointOfInterestId,
             JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(p => p.Id == PointOfInterestId);

            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities
                .FirstOrDefault(p => p.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var aimedPointOfInterest = city.PointsOfInterest
                .FirstOrDefault(x => x.Id == pointOfInterestId);

            if (aimedPointOfInterest == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(aimedPointOfInterest);
            _mailService.send("Point of interest deleted", $"Pointof interest {aimedPointOfInterest.Name} with id {pointOfInterestId} was deleted.");

            return NoContent();
        }


    }
}
