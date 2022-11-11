using TourCompany.Models.Models;
using TourCompany.Models.Requests;

namespace TourCompany.DL.Interfaces
{
    public interface ICustomerRespositoryMongo
    {
        public Task<CustomerMongoRequest> GetCustomerById(int customerId);
        public Task<CustomerMongoRequest> SaveCustomer(Customer customer);
        public Task<CustomerMongoRequest> UpdateCustomer(Customer customer);
        public Task<CustomerMongoRequest> DeleteCustomer(int customerId);
    }
}