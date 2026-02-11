using CityInfo.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    // [Route]: Defines the base URL for this controller. 
    // Since this is a "child" resource of a city, the URL structure reflects that hierarchy.
    [Route("api/cities/{cityId}/PointsOfInterest")]

    // [ApiController]: Enables smart features like automatic 400 responses for validation errors
    // and automatic parameter binding (e.g., inferring [FromBody] for complex types).
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        // GET: api/cities/1/pointsofinterest
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            // 1. Find the city first because points of interest belong to a city.
            // Uses the in-memory store (CitiesDataStore).
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);

            // 2. Validation: If the city doesn't exist, we can't return its points.
            if (city == null)
            {
                return NotFound(); // Returns 404 Status Code
            }

            // 3. Return the list of points for that specific city with 200 OK.
            return Ok(city.PointsOfInterest);
        }

        // GET: api/cities/1/pointsofinterest/2
        // Name = "GetPointOfInterest": This gives a unique internal name to this route.
        // We need this Name so we can reference it later in the POST method (CreatedAtRoute).
        [HttpGet("{PointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetSpecificPointOfInterestOfCity(int cityId, int PointOfInterestId)
        {
            // 1. Check if the city exists.
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            // 2. Check if the specific point of interest exists within that city.
            var PointOfInterest = city.PointsOfInterest.FirstOrDefault(points => points.Id == PointOfInterestId);
            if (PointOfInterest == null)
            {
                return NotFound();
            }

            // 3. Return the single point of interest.
            return Ok(PointOfInterest);
        }

        // POST: api/cities/1/pointsofinterest
        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointofinterest)
        {
            // 1. Validation: Does the city exist? If not, we can't add a point to it.
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            // 2. Calculate the new ID.
            // Since we don't have a real database (SQL) to auto-generate IDs, 
            // we find the current maximum ID across ALL cities and add 1.
            // Note: This is just for demo purposes.
            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
                c => c.PointsOfInterest).Max(x => x.Id);

            // 3. Mapping: Convert the incoming 'CreationDto' (from user) 
            // to the full 'PointOfInterestDto' (for storage).
            var PointOfInterestToAdd = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointofinterest.Name,
                Description = pointofinterest.Description
            };

            // 4. Persistence: Add the new point to the in-memory list.
            city.PointsOfInterest.Add(PointOfInterestToAdd);

            // 5. Return 201 Created.
            // CreatedAtRoute helper method creates a response with a "Location" header.
            // Argument 1 ("GetPointOfInterest"): Must match the Name defined in the HttpGet attribute above.
            // Argument 2 (new { ... }): Provides values for the route parameters ({cityId} and {PointOfInterestId}).
            // Argument 3 (PointOfInterestToAdd): The actual object to return in the response body.
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

        // Updates a resource completely.
        [HttpPut("{PointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int PointOfInterestId, PointOfInterestDto PointOfInterest)
        {
            // 1. Check if the city exists.
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            // 2. Find the point of interest inside that city.
            // Note: In a real DB, we'd query the DB directly. Here we search in-memory.
            var PointOfInterestForUpdate = city.PointsOfInterest.FirstOrDefault(x => x.Id == PointOfInterestId);
            if (PointOfInterestForUpdate == null)
            {
                return NotFound();
            }

            // 3. Update the fields in the memory store.
            // We map the values from the incoming DTO (PointOfInterest) to the stored object (PointOfInterestForUpdate).
            PointOfInterestForUpdate.Name = PointOfInterest.Name;
            PointOfInterestForUpdate.Description = PointOfInterest.Description;

            // 4. Return 204 No Content.
            // This is the standard response for a successful PUT request where no data needs to be returned.
            return NoContent();
        }
    }
}
