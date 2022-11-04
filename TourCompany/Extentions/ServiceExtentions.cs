﻿using TourCompany.BL.Interfaces;
using TourCompany.BL.Services;
using TourCompany.DL.Interfaces;
using TourCompany.DL.Repositories;

namespace TourCompany.Extentions
{
    public static class ServiceExtentions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IDestinationRepository, DestinationRepository>();
            services.AddSingleton<IReservationRepository, ReservationRepository>();
            services.AddSingleton<ICustomerRespository, CustomerRepository>();
            
            return services;
        }
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IDestinationService, DestinationService>();

            return services;
        }
    }
}