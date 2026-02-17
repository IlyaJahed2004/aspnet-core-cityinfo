using CityInfo.Api.Entities;
using CityInfo.Api.Models;

namespace CityInfo.Api.Profiles
{
    public class CityProfile : AutoMapper.Profile
    {
        public CityProfile()
        {
            CreateMap<Entities.City, CityWithoutPointsOfInterestDto>();
            CreateMap<Entities.City, CityDto>();
        }
    }
}
