﻿
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp,options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Database"));
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            });
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            return services;
        }
    }
}
