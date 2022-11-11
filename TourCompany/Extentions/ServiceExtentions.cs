using TourCompany.BL.Interfaces;
using TourCompany.BL.Kafka;
using TourCompany.BL.Services;
using TourCompany.DL.Interfaces;
using TourCompany.DL.Repositories;
using TourCompany.Models.Models;

namespace TourCompany.Extentions
{
    public static class ServiceExtentions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IDestinationRepository, DestinationRepository>();
            services.AddSingleton<IReservationRepository, ReservationRepository>();
            services.AddSingleton<ICustomerRespository, CustomerRepository>();

            services.AddSingleton<IReservationRepositoryMongo, ReservationRepositoryMongo>();
            services.AddSingleton<ICustomerRespositoryMongo, CustomerRepositoryMongo>();
            
            return services;
        }
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IDestinationService, DestinationService>();

            services.AddSingleton<ReservationProducer>();
            services.AddSingleton<IProducerService<int, Customer>, CustomerProducer>();

            services.AddHostedService<ReservationConsumer>();
            services.AddHostedService<CustomerConsumer>();

            return services;
        }
    }
}
