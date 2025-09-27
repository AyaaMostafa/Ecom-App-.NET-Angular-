using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;

        public ProductRepository(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
        }

        public async Task<bool> AddAsync(AddPrroductDTO productDTO)
        {
            if (productDTO == null) return false;
            var product = mapper.Map<Product>(productDTO);
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            var ImagePath = await imageManagementService.AddImagesAsync(productDTO.Photo, productDTO.Name);
            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id,
            }).ToList();
            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;




        }

     

        public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO)
        {

            if (updateProductDTO == null) return false;

            var findProduct = await context.Products
                .Include(m => m.Category)
                .Include(m => m.Photos)
                .FirstOrDefaultAsync(m => m.Id == updateProductDTO.Id);

            if (findProduct == null) return false;
            mapper.Map(updateProductDTO, findProduct);

            var FindPhoto= await context.Photos.Where(m=>m.ProductId == updateProductDTO.Id).ToListAsync();
                foreach (var photo in FindPhoto)
                {   
                    imageManagementService.DeleteImageAsync(photo.ImageName);
                }
                context.Photos.RemoveRange(FindPhoto);
            
            if (updateProductDTO.Photo != null && updateProductDTO.Photo.Any())
            {
                var imagePaths = await imageManagementService.AddImagesAsync(updateProductDTO.Photo, updateProductDTO.Name);
                var Photo = imagePaths.Select(path => new Photo
                {
                    ImageName = path,
                    ProductId = findProduct.Id
                }).ToList();
                await context.Photos.AddRangeAsync(Photo);
            }

            await context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteAsync(Product product)
        {
            var photos = await context.Photos.Where(m => m.ProductId == product.Id).ToListAsync();
            foreach (var item in photos)
            {
                imageManagementService.DeleteImageAsync(item.ImageName);

            }
            context.Products.RemoveRange(product);
            await context.SaveChangesAsync();
            return true;

        }

    }
}
