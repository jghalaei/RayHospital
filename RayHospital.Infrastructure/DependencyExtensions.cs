using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RayHospital.Domain.Entities;
using RayHospital.Domain.Entities.Consultation;
using RayHospital.Infrastructure.Repositories;
using RayHospital.Interfaces;

namespace RayHospital.Infrastructure
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Doctor>, Repository<Doctor>>();
            services.AddScoped<IRepository<TreatmentRoom>, Repository<TreatmentRoom>>();
            services.AddScoped<IRepository<Consultation>, Repository<Consultation>>();
            return services;
        }
    }
}