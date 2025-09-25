using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;

namespace Ecom.API.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDTO>().ForMember(x => x.categoryName,
                op=> op.MapFrom(src=>src.Category.Name )).ReverseMap();
            CreateMap<Photo, PhotoDTO>().ReverseMap();
            CreateMap<AddPrroductDTO, Product>().ForMember(m=>m.Photos, op=>op.Ignore())
                .ReverseMap();

            
        }
    }
}
