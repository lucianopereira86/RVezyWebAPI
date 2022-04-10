using Microsoft.Extensions.DependencyInjection;
using RVezy.Domain.Domain.Interfaces;
using RVezy.Infra.Infra.Context;
using RVezy.Infra.Infra.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using RVezy.Infra.Infra.Mappers;
using RVezy.Domain.Domain.Mappers;

namespace RVezy.Common.DI
{
    public static class CommonExtensions
    {
        public static IServiceCollection AddCommonDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IListingRepository, ListingRepository>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(Assembly.GetEntryAssembly());
            services.AddAutoMapper(typeof(AutoMapperProfileCsv));
            services.AddAutoMapper(typeof(AutoMapperProfileEf));
            return services;
        }
    }
}
