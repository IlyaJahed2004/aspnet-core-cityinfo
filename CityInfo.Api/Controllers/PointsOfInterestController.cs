using CityInfo.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    [Route("api/cities/{cityId}/PointsOfInterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
            if(city == null)
            {
                return NotFound();
            }
            return Ok(city.PointsOfInterest);
        }
        [HttpGet("{PointOfInterestId}")]
        public ActionResult<PointOfInterestDto> GetSpecificPointOfInterestOfCity(int cityId, int PointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if(city == null)
            {
                return NotFound();
            }

            var PointOfInterest = city.PointsOfInterest.FirstOrDefault(points => points.Id == PointOfInterestId);
            if(PointOfInterest == null)
            { 
                return NotFound();
            }
            return Ok(PointOfInterest);
        }
    }
}
