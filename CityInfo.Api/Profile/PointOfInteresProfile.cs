using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CityInfo.Api.Profile
{
    public class PointOfInteresProfile : AutoMapper.Profile
    {
        public PointOfInteresProfile()
        {
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
            CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
            CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>();
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();
        }
    }
}
