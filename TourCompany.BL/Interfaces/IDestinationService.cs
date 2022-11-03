using TourCompany.Models.Models;

namespace TourCompany.BL.Interfaces
{
    public interface IDestinationService
    { 
        public Task<IEnumerable<Destination>> GetAllDestinations();
    }
}
