using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Ecom.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure
{
    public static class InfrastructureRegisteraion
    {
        public static IServiceCollection infrastructureConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddScoped<ICategoryRepository, CategoryRepository>();
            //services.AddScoped<IProductRepository, ProductRepository>();
            //services.AddScoped< IPhotoRepository, PhotoRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>(); //apply UnitOfWork
            services.AddDbContext<AppDbContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("EcomDatabase"));
            
            });
            return services;
        }
    }
}
