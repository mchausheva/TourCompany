using AutoMapper;
using TourCompany.Models.Models;
using TourCompany.Models.Requests;

namespace TourCompany.AutoMapper
{
    internal class AutoMappings : Profile
    {
        public AutoMappings()
        {
            CreateMap<ReservationRequest, Reservation>();
        }
    }
}
