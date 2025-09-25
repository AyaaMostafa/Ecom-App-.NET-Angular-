using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;

namespace Ecom.API.Mapping
{
    public class PhotoMapping : Profile
    {
        public PhotoMapping()
        {
            CreateMap<Photo, PhotoDTO>().ReverseMap();

        }
    }
}
