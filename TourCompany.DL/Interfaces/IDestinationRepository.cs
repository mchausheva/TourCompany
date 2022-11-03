using TourCompany.Models.Models;

namespace TourCompany.DL.Interfaces
{
    public interface IDestinationRepository
    {
        public Task<IEnumerable<Destination>> GetAllDestination();
    }
}
