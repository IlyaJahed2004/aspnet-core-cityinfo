using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace CityInfo.Api.Controllers
{
    [ApiController]
    public class CitiesController : ControllerBase
    {
        [HttpGet("api/cities")]
        public JsonResult GetCities()
        {
            return new JsonResult(CitiesDataStore.Current.Cities);
        }
    }
}
