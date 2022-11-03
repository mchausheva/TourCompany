using MediatR;
using TourCompany.Models.Models;

namespace TourCompany.Models.MediatR
{
    public record GetAllDestinationCommand : IRequest<IEnumerable<Destination>>
    {
    }
}
