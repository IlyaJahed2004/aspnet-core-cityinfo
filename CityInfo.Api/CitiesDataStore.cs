using CityInfo.Api.Models;

namespace CityInfo.Api
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

        // Singleton Pattern (Static Instance)
        // We use a static property so the entire application accesses the same instance of the list.

        //public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            // Initialize dummy data for testing purposes
            Cities = new List<CityDto>
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with that big park.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto
                        {
                            Id = 1,
                            Name = "Central Park",
                            Description = "The most visited urban park in the United States."
                        },
                        new PointOfInterestDto
                        {
                            Id = 2,
                            Name = "Empire State Building",
                            Description = "A 102-story skyscraper located in Midtown Manhattan."
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral that was never really finished.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto
                        {
                            Id = 3,
                            Name = "Cathedral of Our Lady",
                            Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
                        },
                        new PointOfInterestDto
                        {
                            Id = 4,
                            Name = "Antwerp Central Station",
                            Description = "The finest example of railway architecture in Belgium."
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "The one with that big tower.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto
                        {
                            Id = 5,
                            Name = "Eiffel Tower",
                            Description = "A wrought-iron lattice tower on the Champ de Mars."
                        },
                        new PointOfInterestDto
                        {
                            Id = 6,
                            Name = "The Louvre",
                            Description = "The world's largest art museum."
                        }
                    }
                },
                new CityDto()
                {
                    Id = 4,
                    Name = "Tehran",
                    Description = "The bustling capital with beautiful mountains in the north.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto
                        {
                            Id = 7,
                            Name = "Milad Tower",
                            Description = "A multi-purpose tower in Tehran. It is the sixth-tallest tower."
                        },
                        new PointOfInterestDto
                        {
                            Id = 8,
                            Name = "Golestan Palace",
                            Description = "The oldest of the historic monuments in Tehran, a world heritage site."
                        }
                    }
                }
            };
        }
    }
}
