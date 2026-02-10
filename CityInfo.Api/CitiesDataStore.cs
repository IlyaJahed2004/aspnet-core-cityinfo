using CityInfo.Api.Models;

namespace CityInfo.Api
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

        // Singleton Pattern (Static Instance)
        // We use a static property so the entire application accesses the same instance of the list.
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            // Initialize dummy data for testing purposes
            Cities = new List<CityDto>
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with that big park."
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral that was never really finished."
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "The one with that big tower."
                },
                new CityDto()
                {
                    Id = 4,
                    Name = "Tokyo",
                    Description = "The one with the busy crossing and neon lights."
                },
                new CityDto()
                {
                    Id = 5,
                    Name = "Tehran",
                    Description = "The bustling capital with beautiful mountains in the north."
                }
            };
        }
    }
}
