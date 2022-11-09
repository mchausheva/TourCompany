using AutoMapper;
using TourCompany.Models.Models;
using TourCompany.Models.Requests;

namespace TourCompany.AutoMapper
{
    public class AutoMappings : Profile
    {
        public AutoMappings()
        {
            CreateMap<ReservationRequest, Reservation>();
            CreateMap<CustomerRequest, Customer>();

            CreateMap<Reservation, ReservationMongoRequest>();
            CreateMap<ReservationMongoRequest, Reservation>();
        }
    }
}
